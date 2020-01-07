using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Linus.Saves;

public static class DataManager
{
    public enum Language { CN, TW, EN }
    public static Language language = Language.CN;

    public static void SetLanguage(Language lan)
    {
        language = lan;
        SaveManager.SetLanguage(language); 
    }

}
