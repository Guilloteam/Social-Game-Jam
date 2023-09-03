using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverAnimatorController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Animator[] animators;

    public void OnPointerEnter(PointerEventData eventData)
    {
        foreach(Animator animator in animators)
            animator.SetBool("Opened", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach(Animator animator in animators)
            animator.SetBool("Opened", false);
    }
}
