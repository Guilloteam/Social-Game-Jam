using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnswerDisplay : MonoBehaviour
{
    public BranchResultConfig answer_config;

    public TMPro.TextMeshProUGUI text;
    public System.Action answer_selected_delegate;
    public Button button;

    public void Start()
    {
        text.text = answer_config.answer;
        button.onClick.AddListener(() => { answer_selected_delegate?.Invoke(); });
    }
}
