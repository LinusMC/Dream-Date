using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceGirl.MainGame
{
    public class MainGame_MapManager : MonoBehaviour
    {
        public PointManager pointManager;
        public EventPointManager eventPointManager;

        public Point[] points { get { return pointManager.points; } }

        public float timePerPoint = 0.5f;

        public Sprite icon;


        public List<EventInfo> pointEventList = new List<EventInfo>();


        public MainGame_CharacterController characterCtrl;
        public DiceController diceCtrl;


        public int eventNumber = 5;

        Point curPoint { get { return points[curIndex]; } }

        List<Point> emptyPointList = new List<Point>();

        [SerializeField]
        int curIndex;

        Coroutine stepCoro;


        bool isMoving;

        private void Start()
        {
            curIndex = 0;
            SetCharacterPosition(GetPointPosition(curIndex));

            //GetEmptyPointList();
            //for (int i = 0; i < eventNumber; i++)
            //    GenerateEvent();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                RollDice(Random.Range(1, 7));
            if (Input.GetMouseButtonDown(1))
                RollDice(Random.Range(-6, 0));

            if (Input.GetKeyDown(KeyCode.Space))
                RollDice(Random.Range(1, 7));

            if (Input.GetKeyDown(KeyCode.Alpha1))
                RollDice(1);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                RollDice(2);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                RollDice(3);
            if (Input.GetKeyDown(KeyCode.Alpha4))
                RollDice(4);
            if (Input.GetKeyDown(KeyCode.Alpha5))
                RollDice(5);
            if (Input.GetKeyDown(KeyCode.Alpha6))
                RollDice(6);


        } 


        public void RollDice(int number)
        {
            if (isMoving) return;

            isMoving = true;
            diceCtrl.Roll(number, StartMove);
        }

        void StartMove(int number)
        {
            if (stepCoro != null) StopCoroutine(stepCoro);
            stepCoro = StartCoroutine(IStep(number));
        }

        IEnumerator IStep(int pointIndex)
        {
            //characterCtrl.SetState(1);    
            while (pointIndex != 0)
            {
                int dir = pointIndex > 0 ? 1 : -1; //此处再看时需要好好思考


                int nextStep = (curIndex + dir) % points.Length;
                print(nextStep);

                if (nextStep < 0)
                    nextStep = points.Length + nextStep;

                float speed = Vector2.Distance(points[curIndex].transform.position, points[nextStep].transform.position) / timePerPoint; //根据两点之间的距离计算速度
                while (!MoveCharacterToPosition(GetPointPosition(nextStep), speed))
                    yield return null;
                curIndex = nextStep;
                pointIndex -= dir;
            }

            CheckCurPoint();
            //characterCtrl.SetState(0);
            isMoving = false;
        }

        public bool MoveCharacterToPosition(Vector2 pos, float speed)
        {
            return characterCtrl.MoveToPosition(pos, speed);
        }

        public void SetCharacterPosition(Vector2 pos)
        {
            characterCtrl.transform.position = pos;
        }

        List<Point> GetEmptyPointList()
        {
            emptyPointList.Clear();
            foreach (var point in points)
                if (point.pointEvent == null)
                    emptyPointList.Add(point);
            return emptyPointList;
        }

        //public void GenerateEvent()
        //{
        //    if (emptyPointList.Count == 0) return;

        //    var point = emptyPointList[Random.Range(0, emptyPointList.Count)];
        //    while (point == curPoint)
        //        point = emptyPointList[Random.Range(0, emptyPointList.Count)];

        //    emptyPointList.Remove(point);
        //    point.Init(new EventInfo(1, "Name", icon));
        //}

        public void CheckCurPoint()
        {
            //if (curPoint.pointEvent != null)
            var eventPoint = eventPointManager.GetEventPoint(curPoint.transform.position);
            if (eventPoint)
            {
                eventPoint.DoEvent();
                //emptyPointList.Add(curPoint);
                //GenerateEvent();
            }
        }

        Vector2 GetPointPosition(int index)
        {
            return points[index].transform.position;
        }

    }
}