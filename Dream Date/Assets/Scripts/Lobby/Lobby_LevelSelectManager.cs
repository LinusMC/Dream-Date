using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Linus.Saves;

public class Lobby_LevelSelectManager : MonoBehaviour
{
    //---Left---
    public Text levelNameTxt;

    public Image levelProgressFill;
    public Text levelProgressTxt;

    public Image levelIconImg;
    public GameObject levelIconMask;
    public Image[] pointImgs;
    public Sprite[] pointSprs;
   
    public GameObject preLevelBtn, nextLevelBtn;

    //---Right---
    public Text girlNameTxt;
    public Text heightTxt, measurementTxt, birthdayTxt, hobbyTxt;

    //---Middle---
    public Button startBtn;


    List<EasyExcelGenerated.GirlLevel> girlLevelList = null;

    int curGirlID;
    int curLevelIndex;

    Coroutine updateProgressCoro;


    public void Open(int id)
    {
        gameObject.SetActive(true);

        curGirlID = id;
        curLevelIndex = SaveManager.GetGirlLevel(id);
        UpdateLevelInfo();
        UpdateGirlInfo();

    }

    public void Close()
    {
        gameObject.SetActive(false);
    }


    public void UpdateLevelInfo()
    {
        girlLevelList = ConfigManager.Instance.GetGirlLevelListByID(curGirlID);

        if (girlLevelList == null) return;

        var girlLevel = girlLevelList[curLevelIndex];

        levelNameTxt.text = girlLevel.name;

        bool isUnlocked = SaveManager.GetGirlLevel(curGirlID) >= curLevelIndex;

        float progress = 0;
        foreach (var eventID in girlLevel.eventIDs)
        {
            if (SaveManager.IsUnlocked(SaveManager.SAVE_EVENT + eventID))
                progress += ConfigManager.Instance.GetPointEventByID(eventID).progress;
        }


        //Progress
        if (updateProgressCoro != null) StopCoroutine(updateProgressCoro);
        updateProgressCoro = StartCoroutine(IUpdateProgress(progress, 0.3f));

        //levelProgressFill.fillAmount = progress / 100;
        //levelProgressTxt.text = $"{progress}%";

        //Icon
        levelIconImg.sprite = Resources.Load<Sprite>(girlLevel.iconPath);
        levelIconMask.SetActive(!isUnlocked);

        //<>
        preLevelBtn.SetActive(curLevelIndex > 0);
        nextLevelBtn.SetActive(curLevelIndex < girlLevelList.Count - 1);

        for (int i = 0; i < pointImgs.Length; i++)
        {
            pointImgs[i].sprite = pointSprs[i == curLevelIndex ? 0 : 1];
            if (i == curLevelIndex)
                pointImgs[i].GetComponent<DG.Tweening.DOTweenAnimation>().DORestart();
        }
        //Start
        startBtn.interactable = isUnlocked;


    }

    public void UpdateGirlInfo()
    {
        var girl = ConfigManager.Instance.GetGirlByID(curGirlID);

        if (girl == null) return;

        girlNameTxt.text = girl.name;
        heightTxt.text = girl.height;
        measurementTxt.text = girl.measurement;
        birthdayTxt.text = girl.birthday;
        hobbyTxt.text = girl.hobby;

    }

    IEnumerator IUpdateProgress(float toProgress ,float time)
    {
        float counter = 0;

        float fromAmount = levelProgressFill.fillAmount;

        while ((counter += Time.deltaTime) <= time)
        {
            float t = counter / time;

            levelProgressFill.fillAmount = Mathf.Lerp(fromAmount, toProgress / 100, t);
            levelProgressTxt.text = (int)(Mathf.Lerp(fromAmount *100 , toProgress, t)) +"%";

            yield return null;
        }

        levelProgressFill.fillAmount = toProgress / 100;
        levelProgressTxt.text = $"{toProgress}%";
    }

    public void OnSwitchLevel(int dir)
    {
        curLevelIndex = (curLevelIndex + dir) % girlLevelList.Count;
        UpdateLevelInfo();


    }


    public void OnStartBtn()
    {
        Application.LoadLevel("MainGame");
    }


}

