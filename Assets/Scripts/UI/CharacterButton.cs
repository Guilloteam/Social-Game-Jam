using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]

public class CharacterButton : MonoBehaviour
{
    public CharacterConfig character;
    public Button button;
    public Image character_icon;
    public TMPro.TextMeshProUGUI name_text;
    public DialogueDisplay dialogue_display_prefab;
    
    private void Start()
    {
        if(Application.IsPlaying(this))
        {
            button.gameObject.SetActive(QuestlineManager.instance.unlocked_characters.Contains(character));
            QuestlineManager.instance.character_unlock_delegate += (CharacterConfig character) =>
            {
                if (character == this.character)
                {
                    button.gameObject.SetActive(true);
                }
            };

            button.onClick.AddListener(() => {
                QuestlineState questline = QuestlineManager.instance.PickQuestline(character);
                if(questline != null)
                {
                    DialogueEntry entry = questline.current_dialogue_entry;
                    DialogueDisplay dialogue_display = Instantiate(dialogue_display_prefab, PanelSlotManager.instance.dialogue_panel);
                    dialogue_display.entry = entry;
                }
            });

        }
    }

    public void Update()
    {
        name_text.text = character.name;
        character_icon.sprite = character.minimap_icon;
    }
}
