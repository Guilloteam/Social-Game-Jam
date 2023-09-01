using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConditionType
{
    None, Variable 
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
public struct ConditionConfig
{
    public ConditionType condition_type;
    public ConditionOperator condition_operator;
    public int condition_value;
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
}

[System.Serializable]
public class BranchConfig
{
    public BranchResultConfig[] options;
}

[System.Serializable]
public class DialogueEntry
{
    [TextArea(3, 5)]
    public string dialogue_line;
    public QuestlineConfig[] unlocks;
    public CharacterConfig character;
    public BranchConfig answers;
}

[CreateAssetMenu()]
public class QuestlineConfig : ScriptableObject 
{
    public DialogueEntry[] entries;
}

