using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceGirl.MainGame
{
    public class Point : MonoBehaviour
    {
        public EventPoint pointEvent;

        private void Awake()
        {
            pointEvent = GetComponentInChildren<EventPoint>();
        }

        //public void Init(PointEventInfo info)
        //{

        //    //if (info != null)
        //    //    pointEvent = Instantiate(Resources.Load<PointEvent>($"Prefabs/Events/{info.objName}"),transform);
        //    //else
        //    //    pointEvent = null;

        //}

        public void DoEvent()
        {

            pointEvent?.doEvent?.Invoke(); 
        }
    }

}