using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CharacterConfig: ScriptableObject 
{
    public new string name;
    public Sprite dialogue_portrait;
    public Sprite minimap_icon;
    [TextAreaAttribute(3, 5)]
    public string tooltip;
    public QuestlineConfig default_dialogue_config;
}
