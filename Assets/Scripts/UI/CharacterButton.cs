using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    public CharacterConfig character;
    public Button button;
    public TMPro.TextMeshProUGUI name_text;
    public DialogueDisplay dialogue_display_prefab;
    
    private void Start()
    {
        name_text.text = character.name;
        button.gameObject.SetActive(QuestlineManager.instance.unlocked_characters.Contains(character));
        QuestlineManager.instance.character_unlock_delegate += (CharacterConfig character) =>
        {
            if (character == this.character)
            {
                button.gameObject.SetActive(true);
            }
        };

        button.onClick.AddListener(() => {
            DialogueEntry entry = QuestlineManager.instance.PickQuestline(character).current_dialogue_entry;
            DialogueDisplay dialogue_display = Instantiate(dialogue_display_prefab, PanelSlotManager.instance.dialogue_panel);
            dialogue_display.entry = entry;
            
        });
    }
}
