using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Spine.Unity;

public class Lobby_GirlSelector : MonoBehaviour, IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public Image image;
    public SkeletonGraphic spineAnimation;

    [SpineAnimation]
    public string selected, unselected, hover, confirm, clear, greet;

    Lobby_GirlSelectorManager manager;
    Canvas canvas;

    Coroutine moveToPositionCoro, setConfirmCoro, restoreConfirmCoro;

    private void Awake()
    {
        manager = GetComponentInParent<Lobby_GirlSelectorManager>();
        canvas = GetComponent<Canvas>();
    }

    public void MoveToPosition(Vector2 pos, float time)
    {
        if (moveToPositionCoro != null) StopCoroutine(moveToPositionCoro);
        moveToPositionCoro = StartCoroutine(IMoveToPosition(pos, time));

    }
    IEnumerator IMoveToPosition(Vector2 pos, float time)
    {
        Vector2 startPosition = transform.localPosition;

        float counter = 0;
        while ((counter += Time.deltaTime) <= time)
        {
            float t = counter / time;

            transform.localPosition = Vector2.Lerp(startPosition, pos, Mathf.SmoothStep(0, 1, t));

            transform.localScale = GetScale();

            yield return null;
        }

        transform.localPosition = pos;
        transform.localScale = GetScale();
    }

    Vector2 GetScale()
    {
        float hT = Mathf.InverseLerp(manager.minY, manager.maxY, transform.localPosition.y);
        return Vector2.Lerp(Vector2.one, Vector2.one * manager.minScale, hT);
    }

    public void SetSelect(bool bol,int sortingOrder)
    {
        canvas.sortingOrder = sortingOrder;

        if (spineAnimation == null) return;

        if (bol)
        {
            //spineAnimation.AnimationState.AddEmptyAnimation(0, 0, 0);
            spineAnimation.AnimationState.SetAnimation(0, selected, true).MixDuration = 0.1f;
        }
        else
        {
            spineAnimation.AnimationState.SetAnimation(0, unselected, false).MixDuration = 0.1f;
        }
    }

    public void SetConfirm(bool isSelected,float time)
    {
        if (setConfirmCoro != null) StopCoroutine(setConfirmCoro);
        setConfirmCoro = StartCoroutine(ISetConfirm(isSelected, time));
    }
    IEnumerator ISetConfirm(bool isSelected,float time)
    {
        Vector2 startScale = transform.localScale;
        float counter = 0;
        while ((counter += Time.deltaTime) <= time)
        {
            float t = counter / time;

            if (isSelected)
            {
                transform.localScale = Vector2.Lerp(startScale, Vector2.one * manager.confirmScale, t);
            }
            else
            {
                SetColor(new Color(1, 1, 1, 1 - t));
            }

            yield return null;
        }

        if (isSelected)
            transform.localScale = Vector2.one * manager.confirmScale;
        else
            SetColor(Color.clear);
    }

    public void Restore(bool isSelected,float time)
    {
        if (restoreConfirmCoro != null) StopCoroutine(restoreConfirmCoro);
        restoreConfirmCoro = StartCoroutine(IRestore(isSelected, time));
    }
    IEnumerator IRestore(bool isSelected, float time)
    {
        Vector2 startScale = transform.localScale;
        float counter = 0;
        while ((counter += Time.deltaTime) <= time)
        {
            float t = counter / time;

            if (isSelected)
            {
                transform.localScale = Vector2.Lerp(startScale, Vector2.one, t);
            }
            else
            {
                SetColor(new Color(1, 1, 1, t));
            }

            yield return null;
        }

        if (isSelected)
            transform.localScale = Vector2.one;
        else
            SetColor(Color.white);
    }

    public void SetColor(Color color)
    {
        if (spineAnimation == null)
        {
            image.color = color;
            return;
        }
        spineAnimation.color = color;

    }

    public void OnHover(bool bol)
    {
        if (spineAnimation == null) return;

        if (bol)
        {
            spineAnimation.AnimationState.SetEmptyAnimation(1, 0);
            spineAnimation.AnimationState.AddAnimation(1, hover, false, 0).MixDuration = 0.05f;
        }
        else
        {
            spineAnimation.AnimationState.AddEmptyAnimation(1, 0.1f, 0);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        manager.Select(this);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        manager.Hover(this,true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        manager.Hover(this, false);
    }
}
