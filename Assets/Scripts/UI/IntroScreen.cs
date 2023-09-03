using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScreen : MonoBehaviour
{
    private CanvasGroup main_canvas_group;
    private CanvasGroup[] slides;
    private int slide_index = -1;
    private float transition_ratio = 0;
    private bool transition = true;
    public float transition_duration = 1;
    public Button button;
    public bool hide;
    void Start()
    {
        main_canvas_group = GetComponent<CanvasGroup>();
        slides = new CanvasGroup[transform.childCount];
        for(int i=0; i<slides.Length; i++)
        {
            slides[i] = transform.GetChild(i).GetComponent<CanvasGroup>();
        }
        button.onClick.AddListener(() =>
        {
            if(slide_index + 1 >= slides.Length)
            {
                hide = true;
                transition_ratio = 0;
                transition = true;
            }
            else if(transition)
            {
                transition = false;
                slide_index++;
                transition_ratio = 0;
            }
            else
            {
                transition = true;
            }
        });
    }

    void Update()
    {
        if(transition)
        {
            transition_ratio += Time.deltaTime / transition_duration;
            if(transition_ratio >= 1)
            {
                transition_ratio = 0;
                slide_index++;
                transition = false;
                if (hide)
                    Destroy(gameObject);
            }
        }
        if(hide)
        {
            main_canvas_group.alpha = 1 - transition_ratio;
        }
        else
        {
            for(int i=0; i<slides.Length; i++)
            {
                slides[i].alpha = Mathf.Clamp01(1 - Mathf.Abs(transition_ratio + slide_index - i));
            }
        }
    }
}
