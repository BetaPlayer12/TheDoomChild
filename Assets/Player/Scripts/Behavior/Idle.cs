using System;
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
        [SerializeField]
        private int m_numberOfIdleStates = 0;

        private float idleTImerReset = 0.0f;

        public int currentIdleState = 0;
        public bool combatIdle = false;
        public float timeToIdle;
        public bool idling = false;
        public bool idleStateDone = true;

        private void Start()
        {
            stateManager.isIdle = true;
        }

        void Update()
        {
            CheckIfIdle();

            //if (Input.anyKeyDown)
            //{
            //    idleTimer = 0.0f;
            //    currentIdleState = 0;
            //    idling = false;
            //    idleStateDone = true;
            //}
            //else
            //{
            //    idleTimer += Time.deltaTime;
            //}

            //if (currentIdleState == 0 && idleTimer > timeToIdle && !idleStateDone)
            //{
            //    idleStateDone = true;
            //}

            //if (idleTimer >= timeToIdle && !idling && idleStateDone)
            //{
            //    idling = true;
            //    combatIdle = false;
            //}

            //if (idling && idleStateDone)
            //{
            //    currentIdleState = RandomStatePick();
            //}

            //if (!combatIdle && attack.attacking)
            //{
            //    combatIdle = true;
            //    currentIdleState = 0;
            //}
        }

        private void CheckIfIdle()
        {
            if (rigidBody.velocity == Vector2.zero && !stateManager.isAttacking && !stateManager.isDashing)
            {
                stateManager.isIdle = true;
            }
        }

        private int RandomStatePick()
        {
            //int[] integers = new int[] { 0, 1, 2, 3 };

            int randValue = UnityEngine.Random.Range(0, m_numberOfIdleStates + 1);
            //int statepick = randValue;

            //int statepick = integers[randValue];

            Debug.Log(randValue);

            if (randValue == currentIdleState || randValue == 0)
            {
                currentIdleState = RandomStatePick();
            }

            idleStateDone = false;

            return randValue;
        }

        public void DefaultIdleStateFinished()
        {
            Debug.Log("Default Idle Done");
            currentIdleState = RandomStatePick();
        }

        public void IdleFinish()
        {
            idleStateDone = true;
            currentIdleState = 0;
        }
    }
}