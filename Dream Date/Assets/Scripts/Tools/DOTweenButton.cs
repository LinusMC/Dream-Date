using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(DOTweenAnimation))]

public class DOTweenButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler,IPointerExitHandler
{
    public enum Type { Click, Press, Enter, Exit }
    public Type type = Type.Click;
    public string id = "Button";

    DOTweenAnimation anim;

    private void Awake()
    {
        anim = GetComponent<DOTweenAnimation>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (type == Type.Click)
            anim.DORestartById(id);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (type == Type.Press)
            anim.DORestartById(id);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (type == Type.Enter)
            anim.DORestartById(id);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (type == Type.Exit)
            anim.DORestartById(id);
    }

}
