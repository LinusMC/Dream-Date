using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace Linus.Saves
{
    public static class SaveManager
    {
        public const string SAVE_CG_ = "cg_";
        public const string SAVE_EVENT = "event_";
        public const string SAVE_LEVEL = "level_";
        public const string SAVE_LANGUAGE = "language";

        public static DataManager.Language GeLanguage()
        {
            switch (SaveUtility.GetInt(SAVE_LANGUAGE))
            {
                default:
                    return DataManager.Language.CN;
                case 1:
                    return DataManager.Language.TW;
                case 2:
                    return DataManager.Language.EN;
            }
        }

        public static void SetLanguage(DataManager.Language language)
        {
            SaveUtility.SetInt(SAVE_LANGUAGE, (int)language);
        }

        public static bool IsUnlocked(string id)
        {
            return SaveUtility.GetInt(id) >= 1;
        }

        public static void Unlock(string id)
        {
            SaveUtility.SetInt(id, 1);
        }


        public static int GetGirlLevel(int girlID)
        {
            return SaveUtility.GetInt(SAVE_LEVEL + girlID);
        }
        public static void DeleteAll()
        {
            SaveUtility.DeleteAll();
        }
    }
}