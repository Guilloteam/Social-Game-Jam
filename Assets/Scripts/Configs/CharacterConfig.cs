using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class CharacterConfig: ScriptableObject 
{
    public new string name;
    public Sprite sprite;
    public string description;
    public string default_dialogue;
}
