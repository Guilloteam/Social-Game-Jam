using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSlotManager : MonoBehaviour
{
    public static PanelSlotManager instance;
    public RectTransform dialogue_panel;
    public RectTransform slideshow_panel;

    private void Awake()
    {
        instance = this;
    }

}
