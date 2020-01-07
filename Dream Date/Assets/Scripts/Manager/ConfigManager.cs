using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyExcel;
using EasyExcelGenerated;
using System.Linq;
using DiceGirl.Dialogues;
//using DiceGirl.Dialogues;

public class ConfigManager 
{
    public static ConfigManager Instance { get; } = new ConfigManager();


    readonly EEDataManager eeDataManager = new EEDataManager();

    public Dictionary<string, List<Dialogue>> dialogueListDict = new Dictionary<string, List<Dialogue>>();
    public Dictionary<string, PointEvent> pointEventDict = new Dictionary<string, PointEvent>();

    public Dictionary<string, string> localizationDict_CN = new Dictionary<string, string>();
    public Dictionary<string, string> localizationDict_TW = new Dictionary<string, string>();
    public Dictionary<string, string> localizationDict_EN = new Dictionary<string, string>();

    public Dictionary<int, List<GirlLevel>> girlLevelListDict = new Dictionary<int, List<GirlLevel>>();

    public Dictionary<int, Girl> girlDict = new Dictionary<int, Girl>();

    public ConfigManager()
    {
        eeDataManager.Load();

        LoadGirlData();
        LoadGirlLevelData();

        LoadDialogueData();
        LoadPointEventData();

        LoadLocalizationData();
    }
    
    void LoadDialogueData()
    {
        dialogueListDict.Clear();

        var dialogueList = eeDataManager.GetList<Dialogue>();  //先转成List
        foreach (var dialogue in dialogueList)
        {
            if (!dialogueListDict.ContainsKey(dialogue.ID))
                dialogueListDict.Add(dialogue.ID, new List<Dialogue>());

            dialogueListDict[dialogue.ID].Add(dialogue);

        }
    }
    void LoadPointEventData()
    {
        pointEventDict.Clear();

        var pointEventList = eeDataManager.GetList<PointEvent>();  //先转成List
        pointEventDict = pointEventList.ToDictionary(item => item.ID, item => item);
    }
    void LoadLocalizationData()
    {
        localizationDict_CN.Clear();
        localizationDict_TW.Clear();
        localizationDict_EN.Clear();

        var cnList = eeDataManager.GetList<Localization_CN>();
        localizationDict_CN = cnList.ToDictionary(item => item.ID, item => item.value);
        var twList = eeDataManager.GetList<Localization_TW>();
        localizationDict_TW = twList.ToDictionary(item => item.ID, item => item.value);
        var enList = eeDataManager.GetList<Localization_EN>();
        localizationDict_EN = enList.ToDictionary(item => item.ID, item => item.value);

    }
    void LoadGirlLevelData()
    {
        girlLevelListDict.Clear();

        var girlLevelList = eeDataManager.GetList<GirlLevel>();  //先转成List
        foreach (var girlLevel in girlLevelList)
        {
            if (!girlLevelListDict.ContainsKey(girlLevel.ID))
                girlLevelListDict.Add(girlLevel.ID, new List<GirlLevel>());

            girlLevelListDict[girlLevel.ID].Add(girlLevel);

        }
    }
    void LoadGirlData()
    {
        girlDict.Clear();

        var girlList = eeDataManager.GetList<Girl>();  //先转成List
        girlDict = girlList.ToDictionary(item => item.ID, item => item);
    }

    public List<Dialogue> GetDialogueListByID(string id)
    {
        dialogueListDict.TryGetValue(id, out var dialogueList);
        return dialogueList;
    }

    public PointEvent GetPointEventByID(string id)
    {
        pointEventDict.TryGetValue(id, out var pointEvent);
        return pointEvent;
    }

    public string GetLocalizationValueByKey(string key)
    {
        string value = null;
        switch (DataManager.language)
        {
            case DataManager.Language.CN:
                 localizationDict_CN.TryGetValue(key,out value);
                break;
            case DataManager.Language.TW:
                localizationDict_TW.TryGetValue(key, out value);
                break;
            case DataManager.Language.EN:
                localizationDict_EN.TryGetValue(key, out value);
                break;
        }

        return value ?? "<读取键值失败>";
    }

    public List<GirlLevel> GetGirlLevelListByID(int id)
    {
        girlLevelListDict.TryGetValue(id, out var girlLevelList);
        return girlLevelList;
    }

    public Girl GetGirlByID(int id)
    {
        girlDict.TryGetValue(id, out var girl);
        return girl; 
    }


    //加载资源管理

    public Sprite GetBackground(string name)
    {
        var spr = Resources.Load<Sprite>($"Configs/Backgrounds/{name}");

        if (spr == null && null != GameManager.Instance.patchAB)
            spr = GameManager.Instance.patchAB.LoadAsset<Sprite>(name);

        return spr;
    }

    public DialoguePortrait GetDialoguePortrait(string name)
    {
        var portrait = Resources.Load<DialoguePortrait>($"Configs/DialoguePortraits/{name}");

        if (portrait == null && null != GameManager.Instance.patchAB)
            portrait = GameManager.Instance.patchAB.LoadAsset<DialoguePortrait>(name);

        return portrait;
    }
}


