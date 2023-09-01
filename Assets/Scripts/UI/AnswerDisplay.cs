using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerDisplay : MonoBehaviour
{
    public BranchResultConfig answer_config;

    public TMPro.TextMeshPro text;
    public System.Action answer_selected_delegate;

    public void Start()
    {
        text.text = answer_config.answer;
    }
}
