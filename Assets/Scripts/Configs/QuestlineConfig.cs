using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConditionType
{
    None, Variable, Character_Unlock
}

public enum ConditionOperator
{
    StrictlySuperior, Equal, StrictlyInferior, Different, SuperiorOrEqual, InferiorOrEqual,
}

[System.Serializable]
public struct ScenarioStepEffect
{
    public VariableConfig variable;
    public int to_add;
}

[System.Serializable]
public struct SlideshowPopupConfig
{
    public IntroScreen prefab;
}

[System.Serializable]
public struct ConditionConfig
{
    public ConditionType condition_type;
    public ConditionOperator condition_operator;
    public VariableConfig variable;
    public int condition_value;
    public CharacterConfig character_value;
    public bool expected_unlock_state;
    public bool is_condition_filled
    {
        get
        {
            switch(condition_type)
            {
                case ConditionType.None: return true;
                case ConditionType.Variable:
                    int value = QuestlineManager.instance.GetVariable(variable);

                    switch(condition_operator)
                    {
                        case ConditionOperator.Equal:
                            return value == condition_value;
                        case ConditionOperator.Different:
                            return value != condition_value;
                        case ConditionOperator.SuperiorOrEqual:
                            return value >= condition_value;
                        case ConditionOperator.InferiorOrEqual:
                            return value <= condition_value;
                        case ConditionOperator.StrictlyInferior:
                            return value < condition_value;
                        case ConditionOperator.StrictlySuperior:
                            return value > condition_value;
                        default:
                            return true;
                    }
                case ConditionType.Character_Unlock:
                    return QuestlineManager.instance.unlocked_characters.Contains(character_value) == expected_unlock_state;
                default:
                    return true;
            }
        }
    }
}

[System.Serializable]
public struct BranchResultConfig
{
    [TextArea(3, 5)]
    public string answer;
    public ConditionConfig[] conditions;
    public ScenarioStepEffect[] effects;
    public DialogueEntry[] dialogue_entries;
    public QuestlineConfig[] unlocks;
    public bool trigger_ending;

    public bool is_available 
    { 
        get
        {
            for(int i=0; i<conditions.Length; i++)
            {
                if(!conditions[i].is_condition_filled)
                    return false;
            }
            return true;
        }
    }
}

[System.Serializable]
public class DialogueEntry
{
    [TextArea(3, 5)]
    public string dialogue_line;
    public QuestlineConfig[] unlocks;
    public CharacterConfig character;
    public BranchResultConfig[] answers;
    public bool checkpoint;
    public bool translated;
    [TextArea(3, 5)]
    public string translated_dialogue_line;
    public bool on_phone;
}

public enum QuestlinePlaymode
{
    PlayOnce, Loop, Checkpoint, Ending
}

[CreateAssetMenu()]
public class QuestlineConfig : ScriptableObject 
{
    public QuestlinePlaymode play_mode;
    public SlideshowPopupConfig post_slideshow;
    public DialogueEntry[] entries;
}

