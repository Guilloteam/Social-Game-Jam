using JetBrains.Annotations;
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
public struct QuestlineState
{
    public List<StepStackElement> step_stack;
    public QuestlineConfig questline;
    public DialogueEntry current_dialogue_entry 
    {
        get
        {
            DialogueEntry entry = questline.entries[step_stack[0].answer_index];
            for(int j=1; j<step_stack.Count;j++)
            {
                entry = entry.answers.options[step_stack[j].answer_index].dialogue_entries[step_stack[j].sequence_cursor];
            }
            return entry;
        }
    }

    public void SelectAnswer(int answer_index)
    {
        step_stack.Add(new StepStackElement { answer_index = answer_index, sequence_cursor = 0});
    }

    public void NextStep()
    {
        DialogueEntry entry = questline.entries[step_stack[0].answer_index];
        for(int i=1; i<step_stack.Count-1;i++)
        {
            entry = entry.answers.options[step_stack[i].answer_index].dialogue_entries[step_stack[i].sequence_cursor];
        }
        StepStackElement top_stack_value = step_stack[step_stack.Count - 1];
        DialogueEntry[] current_entry_sequence = entry.answers.options[step_stack[step_stack.Count - 1].answer_index].dialogue_entries;
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
    }
}

[System.Serializable]
public struct PlayerState
{
    public int money;
    public int rent;
}

public class QuestlineManager : MonoBehaviour
{
    public static QuestlineManager instance;
    public QuestlineConfig[] questlines;
    public QuestlineConfig[] initial_questlines;

    public List<QuestlineState> questline_states = new List<QuestlineState>();

    public List<CharacterConfig> unlocked_characters = new List<CharacterConfig>();

    public System.Action<CharacterConfig> character_unlock_delegate;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        for (int i = 0; i < initial_questlines.Length; i++)
            questline_states.Add(new QuestlineState { questline = initial_questlines[i], step_stack = new List<StepStackElement> { new StepStackElement { answer_index = 0, sequence_cursor = 0 } } });
    }

    public DialogueEntry PickDialogue(CharacterConfig character)
    {
        for(int i=0; i<questline_states.Count; i++)
        {
            QuestlineState questline_state = questline_states[i];
            DialogueEntry dialogue_entry = questline_state.current_dialogue_entry;
            if (dialogue_entry.character == character)
            {
                return dialogue_entry;
            }
        }
        return null;
    }

    /*public void StepQuestline(int questline_index)
    {
        QuestlineState questline_state = questline_states[questline_index];
        
        questline_state.step++;
        questline_states[questline_index] = questline_state;
        

        if(questline_state.step < questline_state.questline.entries.Length)
        {
            QuestlineConfig questline_config = questline_state.questline;
            DialogueEntry entry = questline_config.entries[questline_state.step];
            for(int i=0; i< entry.unlocks.Length; i++)
            {
                questline_states.Add(new QuestlineState { questline = entry.unlocks[i], step = 0 });
            }
        }
    }

    public void ListUnlocks()
    {
        for(int i=0; i<questline_states.Count;i++)
        {
            CharacterConfig character = questline_states[i].questline.entries[questline_states[i].step].character;
            if (!unlocked_characters.Contains(character))
            {
                unlocked_characters.Add(character);
                character_unlock_delegate?.Invoke(character);
            }
        }
    }*/
}
