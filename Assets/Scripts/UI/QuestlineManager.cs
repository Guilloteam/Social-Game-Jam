using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct StepStackElement
{
    public int answer_index;
    public int sequence_cursor;
}

[System.Serializable]
public class QuestlineState
{
    public List<StepStackElement> step_stack;
    public QuestlineConfig questline;
    public StepStackElement[] last_checkpoint;
    public DialogueEntry current_dialogue_entry 
    {
        get
        {
            if(step_stack.Count == 0 || questline.entries.Length <= step_stack[0].sequence_cursor)
                return null;
            DialogueEntry entry = questline.entries[step_stack[0].sequence_cursor];
            for(int j=1; j<step_stack.Count;j++)
            {
                if (entry.answers[step_stack[j].answer_index].dialogue_entries.Length <= step_stack[j].sequence_cursor)
                    return null;
                entry = entry.answers[step_stack[j].answer_index].dialogue_entries[step_stack[j].sequence_cursor];
            }
            return entry;
        }
    }

    public void SelectAnswer(int answer_index)
    {
        DialogueEntry entry = questline.entries[step_stack[0].sequence_cursor];
        for(int i=1; i<step_stack.Count;i++)
        {
            entry = entry.answers[step_stack[i].answer_index].dialogue_entries[step_stack[i].sequence_cursor];
        }
        foreach (QuestlineConfig questline in entry.answers[answer_index].unlocks)
            QuestlineManager.instance.questline_states.Add(new QuestlineState { questline = questline, step_stack = new List<StepStackElement> { new StepStackElement { } } });
        foreach(ScenarioStepEffect effect in entry.answers[answer_index].effects)
        {
            QuestlineManager.instance.ApplyScenarioEffect(effect);
        }
        if (entry.answers[answer_index].dialogue_entries.Length > 0)
        {
            step_stack.Add(new StepStackElement { answer_index = answer_index, sequence_cursor = 0});
        }
        else
        {
            NextStep();
        }
    }

    public void NextStep()
    {
        DialogueEntry[] current_entry_sequence = questline.entries;
        for(int i=1; i<step_stack.Count-1;i++)
        {
            current_entry_sequence = current_entry_sequence[step_stack[i].sequence_cursor].answers[step_stack[i].answer_index].dialogue_entries;
        }
        StepStackElement top_stack_value = step_stack[step_stack.Count - 1];
        if(current_entry_sequence.Length <= top_stack_value.sequence_cursor)
        {
            step_stack.RemoveAt(step_stack.Count-1);
            NextStep();
        }
        else
        {
            top_stack_value.sequence_cursor++;
            step_stack[step_stack.Count - 1] = top_stack_value;
        }
        if (current_dialogue_entry != null && current_dialogue_entry.checkpoint)
        {
             last_checkpoint = step_stack.ToArray();
        }
    }

    public void OnDialogueEnd()
    {
        if (last_checkpoint == null)
            step_stack = new List<StepStackElement> { new StepStackElement { } };
        else
        {
            step_stack = new List<StepStackElement>(last_checkpoint.Length);
            for (int i = 0; i < last_checkpoint.Length; i++)
                step_stack.Add(last_checkpoint[i]);

        }
    }
}

public class QuestlineManager : MonoBehaviour
{
    public static QuestlineManager instance;
    public QuestlineConfig[] questlines;
    public QuestlineConfig[] initial_questlines;
    public Dictionary<VariableConfig, int> variables = new Dictionary<VariableConfig, int>();

    public List<QuestlineState> questline_states = new List<QuestlineState>();

    public List<CharacterConfig> unlocked_characters = new List<CharacterConfig>();

    public System.Action<CharacterConfig> character_unlock_delegate;

    private void Awake()
    {
        instance = this;
        for (int i = 0; i < initial_questlines.Length; i++)
            questline_states.Add(new QuestlineState { questline = initial_questlines[i], step_stack = new List<StepStackElement> { new StepStackElement { answer_index = 0, sequence_cursor = 0 } } });
        UpdateUnlockedCharacterList();
    }

    public int GetVariable(VariableConfig variable)
    {
        if(!variables.ContainsKey(variable))
        {
            variables.Add(variable, variable.default_value);
        }
        return variables[variable];
    }

    public void UpdateUnlockedCharacterList()
    {
        for(int i=0; i < questline_states.Count; i++)
        {
            var character = questline_states[i].current_dialogue_entry.character;
            if (!unlocked_characters.Contains(character))
            {
                unlocked_characters.Add(character);
                character_unlock_delegate?.Invoke(character);
            }
        }
    }

    public void ApplyScenarioEffect(ScenarioStepEffect effect)
    {
        variables[effect.variable] = GetVariable(effect.variable) + effect.to_add;
    }

    public QuestlineState PickQuestline(CharacterConfig character)
    {
        for(int i=0; i<questline_states.Count; i++)
        {
            QuestlineState questline_state = questline_states[i];
            DialogueEntry dialogue_entry = questline_state.current_dialogue_entry;
            if (dialogue_entry != null && dialogue_entry.character == character)
            {
                return questline_states[i];
            }
        }
        return new QuestlineState { questline = character.default_dialogue, step_stack = new List<StepStackElement> { new StepStackElement { } } };
    }

    internal void UnlockQuestlines(QuestlineConfig[] unlocks)
    {
        foreach(QuestlineConfig unlock in unlocks)
        {
            questline_states.Add(new QuestlineState { questline = unlock, step_stack = new List<StepStackElement> { new StepStackElement { } } });
        }
    }

}
