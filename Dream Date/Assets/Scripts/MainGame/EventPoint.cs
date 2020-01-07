using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DiceGirl.MainGame
{
    public class EventPoint : MonoBehaviour
    {
        public SpriteRenderer sr;

        public string id;
        public UnityEvent doEvent;

        //public bool isUnlocked { get; private set; }

        private void Awake()
        {
            sr.color = Color.gray;
        }

        public void DoEvent()
        {
            doEvent?.Invoke();
            Unlock();
        }

        public void Unlock()
        {
            sr.color = Color.white;
        }
    }


}