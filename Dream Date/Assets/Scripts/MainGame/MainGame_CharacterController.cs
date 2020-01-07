using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DiceGirl.MainGame
{
    public class MainGame_CharacterController : MonoBehaviour
    {
        public Animator characterAnim;


        public bool MoveToPosition(Vector2 pos, float speed)
        {
            Face(pos - (Vector2)transform.position);

            bool isArrived = (Vector2)(transform.position = Vector2.MoveTowards(transform.position, pos, speed * Time.deltaTime)) == pos;
            SetState(isArrived ? 0 : 1);

            return isArrived;
        }

        public void Face(Vector2 dir)
        {
            transform.localScale = new Vector2(dir.x > 0 ? -1 : 1, 1);
        }

        public void SetState(int state)
        {
            characterAnim.SetInteger("state", state);
        }
    }
}