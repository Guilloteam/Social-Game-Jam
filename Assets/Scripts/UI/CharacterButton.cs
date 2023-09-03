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
    private CanvasGroup canvas_group;
    public float appear_duration = 1;
    
    private void Start()
    {
        canvas_group = GetComponent<CanvasGroup>();
        if(Application.IsPlaying(this))
        {
            if(QuestlineManager.instance.unlocked_characters.Contains(character))
            {
                canvas_group.alpha = 1;
                canvas_group.interactable = true;
            }
            else
            {
                canvas_group.alpha = 0;
                canvas_group.interactable = false;
            }
            QuestlineManager.instance.character_unlock_delegate += (CharacterConfig character) =>
            {
                if (character == this.character)
                {
                    StartCoroutine(UnlockCoroutine());
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

    public IEnumerator UnlockCoroutine()
    {
        for(float time=0; time < appear_duration; time += Time.deltaTime)
        {
            canvas_group.alpha = time / appear_duration;
            yield return null;
        }
        canvas_group.alpha = 1;
        canvas_group.interactable = true;
    }

    public void Update()
    {
        name_text.text = character.name;
        character_icon.sprite = character.minimap_icon;
    }
}
