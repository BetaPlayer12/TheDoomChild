using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PlayerNew
{
    public class Idle : PlayerBehaviour
    {
        [SerializeField]
        private Slash attack;
        [SerializeField]
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

            if(idleState == 0 && idleTimer > timeToIdle && !idleStateDone)
            {
                idleStateDone = true;
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