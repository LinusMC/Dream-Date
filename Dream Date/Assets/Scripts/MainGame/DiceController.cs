
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DiceController : MonoBehaviour
{

    public Image img;
    public Sprite[] sprs;

    Animator anim;
    //public DG.Tweening.DOTweenAnimation anim;

    int number;

    Coroutine rollCoro;

    System.Action<int> callback;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Roll(int number, System.Action<int> callback)
    {
        this.callback = callback;

        img.transform.localPosition = Vector2.zero;

        this.number = number;

        if (rollCoro != null)
            StopCoroutine(rollCoro);
        rollCoro = StartCoroutine(IRoll());

        anim.Play("Roll");
    }

    IEnumerator IRoll()
    {
        int index = (Mathf.Abs(number) + 3) % sprs.Length;// Random.Range(0, sprs.Length);

        yield return new WaitForSeconds(0.1f);
        float t = 0.2f;
        while (t > 0)
        {
            img.sprite = sprs[(index++) % sprs.Length];
            float interval = 0.1f;
            t -= interval;
            yield return new WaitForSeconds(interval);
        }
        Show();
    }

    public void Show()
    {
        img.sprite = sprs[Mathf.Abs(number) - 1];

        Invoke("CallBack", 1f);
    }

    public void Punch()
    {
        anim.Play("Punch");
    }

    void CallBack()
    {
        callback?.Invoke(number);
    }

    void StartCoro(Coroutine coro, IEnumerator ie)
    {
        if (coro != null)
            StopCoroutine(coro);
        coro = StartCoroutine(ie);
    }

}
