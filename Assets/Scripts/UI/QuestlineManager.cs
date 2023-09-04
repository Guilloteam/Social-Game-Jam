using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public bool outro_mode = false;
    public DialogueEntry current_dialogue_entry 
    {
        get
        {
            if(step_stack.Count == 0 || questline == null || questline.entries.Length <= step_stack[0].sequence_cursor)
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
        if (entry.answers[answer_index].trigger_ending)
        {
            QuestlineManager.instance.TriggerOutro();
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
        for(int i=1; i<=step_stack.Count-1;i++)
        {
            current_entry_sequence = current_entry_sequence[step_stack[i-1].sequence_cursor].answers[step_stack[i].answer_index].dialogue_entries;
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
        switch(questline.play_mode)
        {
            case QuestlinePlaymode.PlayOnce:
                break;
            case QuestlinePlaymode.Loop:
                step_stack = new List<StepStackElement> { new StepStackElement { } };
                break;
            case QuestlinePlaymode.Checkpoint:
                if (last_checkpoint == null)
                    step_stack = new List<StepStackElement> { new StepStackElement { } };
                else
                {
                    step_stack = new List<StepStackElement>(last_checkpoint.Length);
                    for (int i = 0; i < last_checkpoint.Length; i++)
                        step_stack.Add(last_checkpoint[i]);
                }
                break;
            case QuestlinePlaymode.Ending:
                QuestlineManager.instance.TriggerOutro();
                break;
        }

    }
}

[System.Serializable]
public class VariableState
{
    public VariableConfig variable;
    public int value;
}

public class QuestlineManager : MonoBehaviour
{
    public static QuestlineManager instance;
    public CvConfig cv_config;
    public DialogueDisplay outro_dialogue_display_prefab;
    public QuestlineConfig[] questlines;
    public QuestlineConfig[] initial_questlines;
    public List<VariableState> variables = new List<VariableState>();

    public List<QuestlineState> questline_states = new List<QuestlineState>();

    public List<CharacterConfig> unlocked_characters = new List<CharacterConfig>();

    public System.Action<CharacterConfig> character_unlock_delegate;

    public QuestlineConfig outro_questline;
    public float outro_transition_duration = 1;
    public CanvasGroup outro_canvas_group;
    public bool playing_outro = false;

    public string next_level_scene;

    private void Awake()
    {
        instance = this;
        for (int i = 0; i < initial_questlines.Length; i++)
            questline_states.Add(new QuestlineState { questline = initial_questlines[i], step_stack = new List<StepStackElement> { new StepStackElement { answer_index = 0, sequence_cursor = 0 } } });
        UpdateUnlockedCharacterList();
    }

    public int GetVariable(VariableConfig variable)
    {
        for(int i=0; i<variables.Count(); i++)
        {
            if (variables[i].variable == variable)
                return variables[i].value;
        }
        variables.Add(new VariableState { variable = variable, value = variable.default_value });
        return variable.default_value;
    }

    public void UpdateUnlockedCharacterList()
    {
        for(int i=questline_states.Count-1; i>=0; i--)
        {
            if (questline_states[i].current_dialogue_entry == null)
                questline_states.RemoveAt(i);
        }
        for(int i=0; i < questline_states.Count; i++)
        {
            DialogueEntry dialogue_entry = questline_states[i].current_dialogue_entry;
            if (dialogue_entry == null)
                continue;
            var character = dialogue_entry.character;
            if (!unlocked_characters.Contains(character))
            {
                unlocked_characters.Add(character);
                character_unlock_delegate?.Invoke(character);
            }
        }
    }

    public void ApplyScenarioEffect(ScenarioStepEffect effect)
    {
        for(int i=0; i<variables.Count(); i++)
        {
            if (variables[i].variable == effect.variable)
                variables[i].value = effect.to_add;
        }
        variables.Add(new VariableState { variable = effect.variable, value = effect.to_add });
    }

    public QuestlineState PickQuestline(CharacterConfig character)
    {
        QuestlineState questline_state = null;
        for(int i=0; i<questline_states.Count; i++)
        {
            questline_state = questline_states[i];
            DialogueEntry dialogue_entry = questline_state.current_dialogue_entry;
            if (dialogue_entry != null && dialogue_entry.character == character)
            {
                return questline_states[i];
            }
        }
        QuestlineConfig default_questline_config = character.default_dialogue_config;
        questline_state = new QuestlineState { questline = default_questline_config, step_stack = new List<StepStackElement> { new StepStackElement { } } };
        for(int i=0; i<character.default_dialogues.Length; i++)
        {
            bool valid = true;
            for(int j=0; j < character.default_dialogues[i].conditions.Length; j++)
            {
                if (!character.default_dialogues[i].conditions[j].is_condition_filled)
                {
                    valid = false;
                    break;
                }
            }
            if(valid)
            {
                questline_state.questline = character.default_dialogues[i].questline;
                break;
            }
        }
        questline_states.Add(questline_state);

        return questline_state;
    }

    internal void UnlockQuestlines(QuestlineConfig[] unlocks)
    {
        foreach(QuestlineConfig unlock in unlocks)
        {
            questline_states.Add(new QuestlineState { questline = unlock, step_stack = new List<StepStackElement> { new StepStackElement { } } });
        }
    }

    public void TriggerOutro()
    {
        StartCoroutine(OutroTransitionCoroutine());
    }

    private IEnumerator OutroTransitionCoroutine()
    {
        playing_outro = true;
        questline_states.Clear();
        outro_canvas_group.blocksRaycasts = true;
        for(float time=0;time<outro_transition_duration;time+=Time.deltaTime)
        {
            outro_canvas_group.alpha = time / outro_transition_duration;
            yield return null;
        }
        outro_canvas_group.alpha = 1;
        QuestlineState questline= new QuestlineState { questline = outro_questline, step_stack = new List<StepStackElement>() { new StepStackElement { } }, outro_mode = true };
        questline_states.Add(questline);
        DialogueEntry entry = questline.current_dialogue_entry;
        DialogueDisplay dialogue_display = Instantiate(outro_dialogue_display_prefab, PanelSlotManager.instance.dialogue_panel);
        dialogue_display.outro_dialogue = true;
        dialogue_display.entry = entry;

    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(next_level_scene);
    }
}
