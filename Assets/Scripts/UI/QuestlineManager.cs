using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct QuestlineState
{
    public int step;
    public QuestlineConfig questline;
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
            questline_states.Add(new QuestlineState { questline = initial_questlines[i], step = 0 });
    }

    public DialogueEntry PickDialogue(CharacterConfig character)
    {
        for(int i=0; i<questline_states.Count; i++)
        {
            QuestlineState questline_state = questline_states[i];
            int step = questline_state.step;
            DialogueEntry entry = questline_states[i].questline.entries[step];
            if (entry.character == character)
            {
                StepQuestline(i);
                return entry;
            }
        }
        ListUnlocks();       
        return null;
    }

    public void StepQuestline(int questline_index)
    {
        QuestlineState questline_state = questline_states[questline_index];
        
        questline_state.step++;
        questline_states[questline_index] = questline_state;
        

        if(questline_state.step < questline_state.questline.entries.Length)
        {
            QuestlineConfig questline_config = questline_state.questline;
            DialogueEntry entry = questline_config.entries[questline_state.step];
            for(int i=0; i< entry.questline_unlocks.Length; i++)
            {
                questline_states.Add(new QuestlineState { questline = entry.questline_unlocks[i], step = 0 });
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
    }
}
