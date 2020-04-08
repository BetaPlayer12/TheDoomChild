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

       
        void Update()
        {

            if (Input.anyKeyDown)
            {
                idleTimer = 0.0f;
                idleState = 0;
            }
            else
            {
                idleTimer += Time.deltaTime; 
            }

            if (!attackMode && attack.attacking)
            {
                attackMode = true;
                idleState = 0;
                //idleTimer += Time.deltaTime;
            }

            if(attackMode && idleTimer > 5.0f)
            {
                attackMode = false;
                idleState = 0;
            }else if (!attackMode && idleTimer > 10.0f && idleState == 0)
            {
                attackMode = false;
                idleState = 1;
            }else if (!attackMode && idleTimer > 15.0f && idleState == 1)
            {
                attackMode = false;
                idleState = 2;
            }
            else if (!attackMode && idleTimer > 20.0f && idleState == 2)
            {
                attackMode = false;
                idleState = 3;
            }
            //Debug.Log("idle timer: " + idleTimer);

        }
    }

}