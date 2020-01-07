using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Linus.Saves
{
    [System.Serializable]
    public class SaveData
    {
        [System.Serializable]
        public class IntData
        {
            public string key;
            public int value;

            public IntData(string key, int value)
            {
                this.key = key;
                this.value = value;
            }

            public void Set(int value)
            {
                this.value = value;
            }
        }

        [System.Serializable]
        public class StringData
        {
            public string key;
            public string value;

            public StringData(string key, string value)
            {
                this.key = key;
                this.value = value;
            }

            public void Set(string value)
            {
                this.value = value;
            }
        }

        public List<IntData> intDataList = new List<IntData>();
        public List<StringData> stringDataList = new List<StringData>();
    }


    public static class SaveUtility
    {
        static SaveData saveData = new SaveData();

        static string savePath = Application.persistentDataPath + "/save.sav";/*$"C:/Users/{System.Environment.UserName}/Documents/{Application.productName}/save.sav"; */

        public static void Load()
        {
            try
            {
                if (!File.Exists(savePath))
                    File.WriteAllText(savePath, JsonUtility.ToJson(saveData));
            }
            catch
            {
                MessageBoxTool.MessageBox(new System.IntPtr(), "Error on loading save！", "Warning", 0);

                Application.Quit();
            }

            try
            {
                saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(savePath));
            }
            catch
            {
                File.WriteAllText(savePath, JsonUtility.ToJson(saveData));
            }

        }
        public static void Save()
        {
            File.WriteAllText(savePath, JsonUtility.ToJson(saveData));
        }


        public static void SetInt(string key, int value)
        {
            foreach (var data in saveData.intDataList)
            {
                if (data.key == key)
                {
                    data.Set(value);
                    goto c;
                }
            }
            saveData.intDataList.Add(new SaveData.IntData(key, value));
        c:
            Save();
        }
        public static void SetString(string key, string value)
        {
            foreach (var data in saveData.stringDataList)
            {
                if (data.key == key)
                {
                    data.Set(value);
                    goto c;
                }
            }
            saveData.stringDataList.Add(new SaveData.StringData(key, value));
        c:
            Save();
        }

        public static int GetInt(string key, int defaultValue = 0)
        {
            foreach (var data in saveData.intDataList)
                if (data.key == key)
                    return data.value;
            saveData.intDataList.Add(new SaveData.IntData(key, defaultValue));
            return defaultValue;
        }
        public static string GetString(string key, string defaultValue = "")
        {
            foreach (var data in saveData.stringDataList)
                if (data.key == key)
                    return data.value;

            saveData.stringDataList.Add(new SaveData.StringData(key, defaultValue));
            return defaultValue;
        }

        public static void DeleteKey(string key)
        {
            for (int i = 0; i < saveData.intDataList.Count; i++)
                if (saveData.intDataList[i].key == key)
                {
                    saveData.intDataList.RemoveAt(i);
                    break;
                }
            for (int i = 0; i < saveData.stringDataList.Count; i++)
                if (saveData.stringDataList[i].key == key)
                {
                    saveData.stringDataList.RemoveAt(i);
                    break;
                }
            Save();
        }

        public static void DeleteAll()
        {
            saveData.intDataList.Clear();
            saveData.stringDataList.Clear();
            Save();
        }
    }

}