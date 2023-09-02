using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableDisplay : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    public string prefix;
    public VariableConfig variable;
    public int default_value;

    void Start()
    {
        
    }

    void Update()
    {
        text.text = prefix + QuestlineManager.instance.GetVariable(variable);
    }
}
