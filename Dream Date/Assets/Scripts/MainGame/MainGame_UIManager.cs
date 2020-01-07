using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DiceGirl.MainGame
{
    public class MainGame_UIManager : MonoBehaviour
    {
        public Slider progressSlider;

        Coroutine progressCoro;

        int preProgress;

        private void Start()
        {
            progressSlider.value = 0;

        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                UpdateProgress();
        }

        public void UpdateProgress()
        {
            int curProgress = MainGameManager.Instance.progress;

            curProgress = Mathf.Clamp(curProgress, 0, 100);

            if (curProgress == preProgress) return;

            if (progressCoro != null) StopCoroutine(progressCoro);
            progressCoro = StartCoroutine(IUpdateProgress(100, preProgress, curProgress, 1));

            preProgress = curProgress;
        }
        IEnumerator IUpdateProgress(float bas, float from, float to, float time)
        {
            float t = 0;
            while ((t += Time.deltaTime) < time)
            {
                float rate = Mathf.Lerp(from, to, t / time) / bas;
                progressSlider.value = rate;
                //playerHpTxt.text = $"{(int)(bas * rate)}/{bas}";
                yield return null;
            }

            progressSlider.value = to / bas;
            //playerHpTxt.text = $"{to}/{bas}";
        }

        //public void SetVisible(bool visibled)
        //{
        //    gameObject.SetActive(visibled);
        //}

    }
}