using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lobby_GirlSelectorManager : MonoBehaviour
{
    public Lobby_GirlSelector[] selectors;

    public Transform[] points;

    public float changePositionTime = 0.3f, confirmTime = 0.2f, restoreTime = 0.2f;
    public float minScale = 0.5f;
    public float confirmScale = 1.3f;

    public Color minColor = Color.clear, maxColor = Color.white;
    public float minY = -130 ,maxY = 0;


    public HorizontalLayoutGroup horizontalLayoutGroup;

    LobbyManager lobbyManager;

    //[SerializeField]
    int curIndex =-1;

    [SerializeField]
    bool isConfirmed;

    [SerializeField]
    bool isAnimating;

    Coroutine updateSelectorCoro;

    private void Awake()
    {
        lobbyManager = GetComponentInParent<LobbyManager>();

        horizontalLayoutGroup.enabled = false;
    }
    private void Start()
    {
        lobbyManager.SetLevelSelectVisible(-1);
        Select(selectors[0]);
    }
 
    public void Select(Lobby_GirlSelector selector)
    {
        if (isAnimating) return;

        if (isConfirmed)
        {
            Restore();
            return;
        }

        var index = System.Array.IndexOf(selectors, selector);

        var 顺序 = index > curIndex;
        if (index == 0 && curIndex == selectors.Length - 1)
            顺序 = true;
        else if (index == selectors.Length - 1 && curIndex == 0)
            顺序 = false;


        if (isConfirmed = curIndex == index)
        {
            Confirm();
            return;
        }

        curIndex = index;
        UpdatePosition(顺序);

    }

    void UpdatePosition(bool 顺序)
    {
        StartAnimating(changePositionTime);

        //for (int i = selectors.Length - 1; i >= 0; i--)
        for (int i = 0; i < selectors.Length; i++)
        {
            var selector = selectors[(i + curIndex) % selectors.Length];

            int topOrder = selectors.Length + 1;
            int order = topOrder - (selectors.Length - (顺序 ?  i: (selectors.Length - i)));
            selector.SetSelect(i == 0, i == 0 ? topOrder : order);

            selector.MoveToPosition(points[i].transform.localPosition, changePositionTime);

        }
    }

    public void Hover(Lobby_GirlSelector selector,bool isEnter)
    {
        if (isConfirmed) return;

        selector.OnHover(isEnter);

    }
    public void Confirm()
    {
        StartAnimating(confirmTime);

        lobbyManager.SetLevelSelectVisible(curIndex);

        for (int i = 0; i < selectors.Length; i++)
        {
            selectors[i].SetConfirm(i == curIndex, confirmTime);
            if (i == curIndex)
            {
                selectors[i].OnHover(false);
            }
        }
    }
    public void Restore()
    {
        if (isAnimating) return;

        StartAnimating(restoreTime);

        isConfirmed = false;

        lobbyManager.SetLevelSelectVisible(-1);

        for (int i = 0; i < selectors.Length; i++)
            selectors[i].Restore(i == curIndex, restoreTime);
    }

    public void StartAnimating(float time)
    {
        isAnimating = true;

        Invoke(nameof(DisableAniamting), time);
    }

    void DisableAniamting()
    {
        isAnimating = false;
    }


}
