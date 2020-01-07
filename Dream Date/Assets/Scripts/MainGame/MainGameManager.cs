using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;using Linus.Saves;

namespace DiceGirl.MainGame
{
    public class MainGameManager : MonoBehaviour
    {
        public static MainGameManager Instance;

        public MainGame_UIManager uiManager;
        public MainGame_MapManager mapManager;

        public Image cgImg;
        public DG.Tweening.DOTweenAnimation unlock;

        public int progress;


        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            Instance = null;
        }

        public void AddProgress(int value)
        {
            progress += value;
            uiManager.UpdateProgress();
        }

        public void SetProgress(int value)
        {
            progress = value;
            uiManager.UpdateProgress();
        }

        public void ShowCG(string id)
        {
            cgImg.sprite = Resources.Load<Sprite>($"CG/cg_{id}");
            cgImg.gameObject.SetActive(true);
        }

        public void UnlockCG(string id)
        {
            if (!SaveManager.IsUnlocked(SaveManager.SAVE_CG_ + id))
            {
                SaveManager.Unlock(SaveManager.SAVE_CG_ + id);
                unlock.DORestart();
            }
        }

        //public void SetUIVisible(bool visibled)
        //{
        //    uiManager.SetVisible(visibled);
        //}

        public bool IsUnlockedEvent(string id)
        {
            return SaveManager.IsUnlocked(SaveManager.SAVE_EVENT + id);
        }
        public void UnlockEvent(string id)
        {
            SaveManager.Unlock(SaveManager.SAVE_EVENT + id);
        }

    }
}
