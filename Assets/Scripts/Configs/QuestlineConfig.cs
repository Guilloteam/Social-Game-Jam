using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueEntry
{
    public CharacterConfig character;
    public string text;
    public QuestlineConfig[] questline_unlocks;
}

[CreateAssetMenu()]
public class QuestlineConfig : ScriptableObject 
{
    public DialogueEntry[] entries;
}
