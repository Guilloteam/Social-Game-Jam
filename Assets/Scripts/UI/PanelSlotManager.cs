using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSlotManager : MonoBehaviour
{
    public static PanelSlotManager instance;
    public RectTransform dialogue_panel;

    private void Awake()
    {
        instance = this;
    }

}
