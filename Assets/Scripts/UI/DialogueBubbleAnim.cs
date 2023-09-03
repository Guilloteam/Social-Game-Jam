using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBubbleAnim : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    public float fold_duration = 0.5f;
    public float unfold_duration = 0.5f;
    public string pending_text;
    public float anim_direction = 0;
    public float anim_cursor = 0;
    public AnimationCurve scale_curve = AnimationCurve.Linear(0, 0, 1, 1);
    public AnimationCurve alpha_curve = AnimationCurve.Linear(0, 0, 1, 1);
    public AnimationCurve visibility_curve = AnimationCurve.Linear(0, 1, 1, 1);
    public CanvasGroup canvas_group;
    private Color text_color;
    public GameObject phone_icon;
    public bool show_phone;


    private void Start()
    {
        text_color = text.color;
    }

    public void Show(string text)
    {
        if(anim_cursor > 0)
        {
            pending_text = text;
            anim_direction = -1;
        }
        else
        {
            this.text.text = text;
            anim_direction = 1;
        }
    }

    public void Hide()
    {
        pending_text = null;
        anim_direction = -1;
    }

    void Update()
    {
        if(phone_icon != null)
        {
            phone_icon.SetActive(show_phone);
        }
        float anim_duration = fold_duration;
        if (anim_direction > 0)
            anim_duration = unfold_duration;
        anim_cursor += anim_direction * Time.deltaTime / anim_duration;
        transform.localScale = new Vector3(scale_curve.Evaluate(anim_cursor), 1, 1);
        text.color = new Color(text_color.r, text_color.g, text_color.b, alpha_curve.Evaluate(anim_cursor));
        if(canvas_group != null)
            canvas_group.alpha = visibility_curve.Evaluate(anim_cursor);
        if(anim_cursor < 0)
        {
            anim_cursor = 0;
            if(pending_text != null)
            {
                text.text = pending_text;
                anim_direction = 1;
            }
        }
        if(anim_cursor > 1)
        {
            anim_direction = 0;
            anim_cursor = 1;
        }
    }
}
