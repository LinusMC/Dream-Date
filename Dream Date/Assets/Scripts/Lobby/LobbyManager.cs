using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{

    public Lobby_GirlSelectorManager girlSelectorManager;
    public Lobby_LevelSelectManager levelSelectManager;


    public Text debugTxt;

    private void Update()
    {
        UpdateDebugTxt($"{girlSelectorManager.changePositionTime} {Time.timeScale}");
    }

    public void SetLevelSelectVisible(int index)
    {
        if (index > -1)
            levelSelectManager.Open(index + 1);
        else
            levelSelectManager.Close();
    }

    public void UpdateDebugTxt(string txt)
    {
        debugTxt.text = txt;
    }
}
