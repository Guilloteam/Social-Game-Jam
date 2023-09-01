using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    public CharacterConfig character;
    public Button button;
    private void Start()
    {
        button.onClick.AddListener(() => {
            // = QuestlineManager.instance.PickDialogue(character);
        });
    }
}
