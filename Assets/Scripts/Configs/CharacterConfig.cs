using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ConditionalQuestline
{
    public ConditionConfig[] conditions;
    public QuestlineConfig questline;
}
[CreateAssetMenu()]
public class CharacterConfig: ScriptableObject 
{
    public new string name;
    public Sprite dialogue_portrait;
    public Sprite minimap_icon;
    [TextAreaAttribute(3, 5)]
    public string tooltip;
    public QuestlineConfig default_dialogue_config;
    public ConditionalQuestline[] default_dialogues;

}
