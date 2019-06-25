using System;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Refactor.DChild.Gameplay.Characters.AI;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters.Enemies
{
    public class SampleAI : CombatAIBrain<SampleAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField]
            private MovementInfo m_patrol = new MovementInfo();
            [SerializeField]
            private MovementInfo m_move = new MovementInfo();
            [SerializeField]
            private SimpleAttackInfo m_attack = new SimpleAttackInfo();

            public MovementInfo patrol => m_patrol;
            public MovementInfo move => m_move;
            public SimpleAttackInfo attack => m_attack;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_attack.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Patrolling,
            Attacking,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        [SerializeField]
        private SimpleTurnHandle m_turnHandle;
        [SerializeField]
        private MovementHandle2D m_movementHandle;
        [SerializeField]
        private PatrolHandle m_patrolHandle;
        [SerializeField]
        private AttackHandle m_attackHandle;
        [SerializeField]
        private State m_currentState;
        private State m_afterWaitForBehaviourState;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_currentState = State.ReevaluateSituation;
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs)
        {
            m_turnHandle.Execute();
            m_currentState = State.WaitBehaviourEnd;
            m_afterWaitForBehaviourState = State.ReevaluateSituation;
        }

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            base.SetTarget(damageable, m_target);
            m_currentState = State.Chasing;
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
        }

        private void Update()
        {
            switch (m_currentState)
            {
                case State.Patrolling:
                    //Add actual CharacterInfo Later
                    m_animation.SetAnimation(0, m_info.patrol.animation, true);
                    var characterInfo = new PatrolHandle.CharacterInfo(m_character.transform.position, m_character.facing);
                    m_patrolHandle.Patrol(m_movementHandle, m_info.patrol.speed, characterInfo);
                    //Play Animation
                    break;
                case State.Attacking:
                    m_attackHandle.ExecuteAttack(m_info.attack.animation);
                    m_currentState = State.WaitBehaviourEnd;
                    m_afterWaitForBehaviourState = State.ReevaluateSituation;
                    break;
                case State.Chasing:
                    //Put Target Destination
                    var target = m_targetInfo.position;
                    if (Vector2.Distance(target, transform.position) <= m_info.attack.range)
                    {
                        m_currentState = State.Attacking;
                        m_movementHandle.Stop();
                    }
                    else
                    {
                        m_movementHandle.MoveTowards(target, m_info.move.speed);
                    }
                    //Play Animation
                    break;
                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    if (m_targetInfo.isValid)
                    {
                        m_currentState = State.Chasing;
                    }
                    else
                    {
                        m_currentState = State.Patrolling;
                    }
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }
        }
    }
}