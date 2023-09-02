using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogueDisplay : MonoBehaviour
{
    public DialogueEntry entry;
    public DialogueBubbleAnim dialogue_line_anim;
    public DialogueBubbleAnim single_answer_anim;
    public AnswerDisplay answer_display_prefab;
    public RectTransform answer_panel;
    public Image character_portrait;
    public TMPro.TextMeshProUGUI character_name_text;
    public Button next_button;

    private List<AnswerDisplay> displayed_answers = new List<AnswerDisplay>();

    public float fade_duration = 0.3f;
    public float bubble_appear_duration = 0.5f;
    public float bubble_appear_delay = 0.5f;


    IEnumerator Start()
    {
        yield return UpdateDisplay();
    }

    public IEnumerator UpdateDisplay()
    {
        character_portrait.sprite = entry.character.dialogue_portrait;
        character_name_text.text = entry.character.name;

        for (int i = 0; i < displayed_answers.Count; i++)
        {
            Destroy(displayed_answers[i].gameObject);
        }
        displayed_answers.Clear();
        List<BranchResultConfig> available_answers = new List<BranchResultConfig>();

        for (int i = 0; i < entry.answers.Length; i++)
        {
            if (entry.answers[i].is_available)
            {
                available_answers.Add(entry.answers[i]);
            }
        }
        dialogue_line_anim.Hide();
        single_answer_anim.Hide();
    
        if(available_answers.Count == 0)
        {
            next_button.gameObject.SetActive(true);
            next_button.onClick.RemoveAllListeners();
            next_button.onClick.AddListener(() =>
            {
                QuestlineState questline_state = QuestlineManager.instance.PickQuestline(entry.character);
                questline_state.NextStep();
                AfterTransition(questline_state);
            });
            dialogue_line_anim.Show(entry.dialogue_line);
        }
        else if (available_answers.Count == 1)
        {
            next_button.gameObject.SetActive(true);
            next_button.onClick.RemoveAllListeners();
            next_button.onClick.AddListener(() =>
            {
                QuestlineState questline_state = QuestlineManager.instance.PickQuestline(entry.character);
                QuestlineConfig questline = questline_state.questline;
                questline_state.SelectAnswer(0);
                AfterTransition(questline_state);
            });
            dialogue_line_anim.Show(entry.dialogue_line);
            yield return new WaitForSeconds(bubble_appear_delay);
            single_answer_anim.Show(entry.answers[0].answer);
        }
        else
        {
            next_button.gameObject.SetActive(false);
            for(int i=0; i<available_answers.Count; i++)
            {
                AnswerDisplay answer_display = Instantiate(answer_display_prefab, answer_panel);
                displayed_answers.Add(answer_display);
                answer_display.answer_config = available_answers[i];
                int answer_index = i;
                answer_display.answer_selected_delegate += () =>
                {
                    QuestlineState questline_state = QuestlineManager.instance.PickQuestline(entry.character);
                    questline_state.SelectAnswer(answer_index);
                    AfterTransition(questline_state);
                };
            }
            dialogue_line_anim.Show(entry.dialogue_line);

        }
    }

    public void AfterTransition(QuestlineState questline_state)
    {
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
            StartCoroutine(UpdateDisplay());
        }
    }
}
