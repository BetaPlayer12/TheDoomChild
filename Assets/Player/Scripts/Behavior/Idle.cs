using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerNew
{
    public class Idle : PlayerBehaviour
    {
        [SerializeField]
        private Slash attack;
        private float idleTimer = 0.0f;
        private float idleTImerReset = 0.0f;
        public int idleState = 0;
        public bool attackMode = false;

        public float timeToIdle;
        public bool idling = false;
        public bool idleStateDone = true;
        

       
        void Update()
        {

            if (Input.anyKeyDown)
            {
                idleTimer = 0.0f;
                idleState = 0;
                idling = false;
                idleStateDone = true;
            }
            else
            {
                idleTimer += Time.deltaTime;
            }

            if(idleTimer >= timeToIdle && !idling && idleStateDone)
            {
                idling = true;
                attackMode = false;
            }

            if (idling && idleStateDone)
            {
                idleState = RandomStatePick(idleState);
            }

            if (!attackMode && attack.attacking)
            {
                attackMode = true;
                idleState = 0;
                //idleTimer += Time.deltaTime;
            }

            //if(attackMode && idleTimer > 5.0f)
            //{
            //    attackMode = false;
            //    idleState = 0;
            //}else if (!attackMode && idleTimer > 10.0f && idleState == 0)
            //{
            //    attackMode = false;
            //    idleState = RandomStatePick(0);
            //}else if (!attackMode && idleTimer > 15.0f && idleState == 1)
            //{
            //    attackMode = false;
            //    idleState = RandomStatePick(1);
            //}
            //else if (!attackMode && idleTimer > 20.0f && idleState == 2)
            //{
            //    attackMode = false;
            //    idleState = RandomStatePick(2);
            //}
            //else if (!attackMode && idleTimer > 35.0f && idleState == 3)
            //{
            //    attackMode = false;
            //    idleState = RandomStatePick(3);
            //}
            //Debug.Log("idle timer: " + idleTimer);

        }

        private int RandomStatePick(int currentState)
        {
            int[] integers = new int[] { 0, 1, 2, 3 };

            int randValue = Random.Range(0, integers.Length);
            int statepick = integers[randValue];
           
            
            if(statepick == currentState)
            {
                RandomStatePick(currentState);
            }
            idleStateDone = false;
            return statepick;
        }

        public void IdleFinish()
        {
            idleStateDone = true;
        }
    }

}