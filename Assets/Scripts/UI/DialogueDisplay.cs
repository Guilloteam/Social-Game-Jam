using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueDisplay : MonoBehaviour
{
    public DialogueEntry entry;
    public TMPro.TextMeshProUGUI dialogue_line_text;
    


    void Start()
    {
        dialogue_line_text.text = entry.dialogue_line;
    }

    void Update()
    {
        
    }
}
