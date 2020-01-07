using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DiceGirl.Dialogues;
using Linus.Saves;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public DialogueManager dialogueManager;

    public bool isDebugMode { get; private set; }

    public AssetBundle patchAB { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        Instance = this;
        SaveUtility.Load();
        DataManager.SetLanguage(SaveManager.GeLanguage());
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadPatchAB();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            DataManager.SetLanguage(DataManager.Language.CN);
        if (Input.GetKeyDown(KeyCode.O))
            DataManager.SetLanguage(DataManager.Language.TW);
        if (Input.GetKeyDown(KeyCode.P))
            DataManager.SetLanguage(DataManager.Language.EN);



    }

    public void LoadPatchAB()
    {
        //如果是编译器就自动打开调试模式
#if UNITY_EDITOR
        isDebugMode = true;
#endif

        UnloadPatchAB();
        //fullAB = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/full");
        //if (fullAB == null)
        //{
        //    print("AB not found!");
        //}
        StartCoroutine(ILoadPatchAB());
    }
    IEnumerator ILoadPatchAB()
    {
        if (!isDebugMode)
        {
            UnloadPatchAB();

            //loadingCtrl.Show();

            string path = System.Environment.CurrentDirectory + "/DLC";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (File.Exists(path + "/full"))
            {
                var bundleLoadRequest = AssetBundle.LoadFromFileAsync(path + "/full");

                while (!bundleLoadRequest.isDone)
                {
                    //loadingCtrl.SetProgress(bundleLoadRequest.progress / .9f);
                    yield return null;
                }
                patchAB = bundleLoadRequest.assetBundle;
                print("Patch has loaded!");
            }
            else
            {
                print("Patch has not found!");
            }
        }

        //loadingCtrl.Hide();


    }
    public void UnloadPatchAB()
    {
        if (patchAB != null)
            patchAB.Unload(false);
    }

}
