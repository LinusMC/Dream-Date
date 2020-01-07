using Linus.Saves;
using UnityEngine;

namespace DiceGirl.MainGame
{
    public class EventPointManager : MonoBehaviour
    {
        public EventPoint[] eventPoints; 

        private void Awake()
        {
            eventPoints = GetComponentsInChildren<EventPoint>();
        }

        public EventPoint GetEventPoint(Vector2 pos)
        {
            foreach (var eventPoint in eventPoints)
                if (Vector2.Distance(eventPoint.transform.position, pos) < 0.5f)
                    return eventPoint;
            return null;
        }

        public int CheckEvents()
        {
            int progress = 0;

            foreach (var eventPoint in eventPoints)
            {
                var pe = ConfigManager.Instance.GetPointEventByID(eventPoint.id);
                if (pe == null) continue;

                if (MainGameManager.Instance.IsUnlockedEvent(pe.ID))
                {
                    eventPoint.Unlock();
                    progress += pe.progress;
                }
            }

            return progress;
        }

        public void DoEvent(EventPoint eventPoint)
        {
            eventPoint.DoEvent();

            var pe = ConfigManager.Instance.GetPointEventByID(eventPoint.id);
            if (pe == null) return;

            bool isUnlocked = MainGameManager.Instance.IsUnlockedEvent(pe.ID);

            System.Action endAction = new System.Action(
                delegate
                {
                    //print(isUnlocked);

                    if (!isUnlocked)
                        MainGameManager.Instance.AddProgress(pe.progress);

                    //如果需要解锁cg
                    if (!string.IsNullOrEmpty(pe.cgID))
                    {
                        MainGameManager.Instance.UnlockCG(pe.cgID);
                        MainGameManager.Instance.ShowCG(pe.cgID);
                    }
                });

            if (!string.IsNullOrEmpty(pe.dialogueID))
                GameManager.Instance.dialogueManager.StartDialogue(pe.dialogueID, endAction);
            else
                endAction?.Invoke();

            MainGameManager.Instance.UnlockEvent(pe.ID);
        }

    }
}
