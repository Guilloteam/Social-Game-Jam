using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ObjectiveConfig
{
    public VariableConfig variable;
    public VariableConfig visibility_variable;
    public string display_text;
}
public class ObjectivesDisplay : MonoBehaviour
{
    public ObjectiveConfig[] objectives;
    public ObjectiveDisplayToggle objective_display_prefab;
    public ObjectiveDisplayToggle[] displayed_elements;

    void Start()
    {
        displayed_elements = new ObjectiveDisplayToggle[objectives.Length];
        for(int i=0; i<objectives.Length; i++)
        {
            displayed_elements[i] = Instantiate(objective_display_prefab, transform);
            displayed_elements[i].config = objectives[i];
        }
    }

    void Update()
    {
       for(int i=0; i<objectives.Length; i++) { }
    }
}
