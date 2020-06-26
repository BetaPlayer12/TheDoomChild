using DChild.Gameplay.Characters.AI;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class EyesAI : AIBrain<EyesAI.Info>
    {
        [System.Serializable]
        public class Info : IAIInfo
        {
            [SerializeField, MinValue(0f)]
            private float m_speed;

            public float speed => m_speed;

            public void Initialize()
            {

            }
        }

        private enum State
        {
            Idle,
            Patrol,
            WaitForBehavior
        }

        [SerializeField]
        private bool m_canPatrol;
        [SerializeField, ShowIf("m_canPatrol")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, ShowIf("m_canPatrol")]
        private PathFinderAgent m_agent;
        [SerializeField, ShowIf("m_canPatrol")]
        private SimpleTurnHandle m_turnHandle;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;

        private void OnTurnRequest(object sender, EventActionArgs eventArgs)
        {
            m_turnHandle.Execute();
            m_stateHandle.Wait(State.Patrol);
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
        }

        protected override void Awake()
        {
            base.Awake();
            var startingState = m_canPatrol ? State.Patrol : State.Idle;
            m_stateHandle = new StateHandle<State>(startingState, State.WaitForBehavior);
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_turnHandle.TurnDone += OnTurnDone;
            enabled = m_canPatrol;
        }



        private void Update()
        {
            switch (m_stateHandle.currentState)
            {
                case State.Patrol:
                    m_patrolHandle.Patrol(m_agent, m_info.speed, new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing));
                    break;
                case State.WaitForBehavior:

                    break;
            }
        }
    }

}