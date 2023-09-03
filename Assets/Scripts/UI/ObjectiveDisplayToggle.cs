using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveDisplayToggle : MonoBehaviour
{
    public ObjectiveConfig config;
    public TMPro.TextMeshProUGUI[] texts;
    public Animator animator;

    void Start()
    {
        foreach(var text in texts)
            text.text = config.display_text;
    }

    void Update()
    {
        animator.SetBool("Checked", QuestlineManager.instance.GetVariable(config.variable) > 0);
        if (config.visibility_variable == null)
            animator.SetBool("Visible", true);
        else
            animator.SetBool("Visible", QuestlineManager.instance.GetVariable(config.visibility_variable) > 0);
    }
}
