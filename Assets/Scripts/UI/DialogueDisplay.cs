using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueDisplay : MonoBehaviour
{
    public DialogueEntry entry;
    public TMPro.TextMeshProUGUI dialogue_line_text;
    public AnswerDisplay answer_display_prefab;
    public RectTransform answer_panel;
    public Image character_portrait;
    public TMPro.TextMeshProUGUI character_name_text;

    private List<AnswerDisplay> displayed_answers = new List<AnswerDisplay>();


    void Start()
    {
        UpdateDisplay();
    }

    public void UpdateDisplay()
    {
        dialogue_line_text.text = entry.dialogue_line;
        character_portrait.sprite = entry.character.dialogue_portrait;
        character_name_text.text = entry.character.name;

        for (int i = 0; i < displayed_answers.Count; i++)
        {
            Destroy(displayed_answers[i].gameObject);
        }
        displayed_answers.Clear();
        for(int i=0; i<entry.answers.Length; i++)
        {
            if (entry.answers[i].is_available)
            {
                AnswerDisplay answer_display = Instantiate(answer_display_prefab, answer_panel);
                displayed_answers.Add(answer_display);
                answer_display.answer_config = entry.answers[i];
                int answer_index = i;
                answer_display.answer_selected_delegate += () =>
                {
                    QuestlineState questline_state = QuestlineManager.instance.PickQuestline(entry.character);
                    questline_state.SelectAnswer(answer_index);
                    DialogueEntry current_entry = questline_state.current_dialogue_entry;
                    if(current_entry != null)
                        QuestlineManager.instance.UnlockQuestlines(current_entry.unlocks);
                    
                    if(current_entry == null || current_entry.character != entry.character)
                    {
                        questline_state.OnDialogueEnd();
                        Destroy(gameObject);
                        QuestlineManager.instance.UpdateUnlockedCharacterList();
                    }
                    else
                    {
                        entry = questline_state.current_dialogue_entry;
                        UpdateDisplay();
                    }
                };
            }
        }

    }
}
