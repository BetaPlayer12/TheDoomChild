using System;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Refactor.DChild.Gameplay.Characters.AI;
using UnityEngine;
using Spine;
using Spine.Unity;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using DChild;
using DChild.Gameplay.Characters.Enemies;

namespace Refactor.DChild.Gameplay.Characters.Enemies
{
    public class MeleeBeeDroneAI : CombatAIBrain<MeleeBeeDroneAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Basic Behaviours
            [SerializeField]
            private MovementInfo m_patrol = new MovementInfo();
            public MovementInfo patrol => m_patrol;
            [SerializeField]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;

            //Attack Behaviours
            [SerializeField]
            private SimpleAttackInfo m_stingerdive = new SimpleAttackInfo();
            public SimpleAttackInfo stingerdive => m_stingerdive;
            [SerializeField]
            private SimpleAttackInfo m_rapidSting = new SimpleAttackInfo();
            public SimpleAttackInfo rapidSting => m_rapidSting;
            //
            [SerializeField, MinValue(0)]
            private float m_patience;
            public float patience => m_patience;

            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idle2Animation;
            public string idle2Animation => m_idle2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_stingerdive.SetData(m_skeletonDataAsset);
                m_rapidSting.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Idle,
            Patrol,
            Turning,
            Attacking,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            StingerDive,
            RapidSting,
            [HideInInspector]
            _COUNT
        }
       
        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private PathFinderAgent m_agent;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [ShowInInspector]
        private State m_currentState;
        [ShowInInspector]
        private Attack m_chosenAttack;

        private State m_afterWaitForBehaviourState;
        //Patience Handler
        private float m_currentPatience;
        private bool m_enablePatience;

        private bool m_hasDecidedOnAttack;
        private float m_focusedAttackRange;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_currentState = State.ReevaluateSituation;
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_currentState = State.Turning;

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                SetState(State.Chasing);
                m_currentPatience = 0;
                m_enablePatience = false;
            }
            else
            {
                m_enablePatience = true;
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_currentState = m_afterWaitForBehaviourState;
        }

        private void WaitTillBehaviourEnd(State nextState)
        {
            m_currentState = State.WaitBehaviourEnd;
            m_afterWaitForBehaviourState = nextState;
        }

        private void SetState(State state)
        {
            if (m_currentState == State.WaitBehaviourEnd)
            {
                m_afterWaitForBehaviourState = state;
            }
            else
            {
                m_currentState = state;
            }
        }

        //Patience Handler
        private void Patience()
        {
            if (m_currentPatience < m_info.patience)
            {
                m_currentPatience += m_character.isolatedObject.deltaTime;
            }
            else
            {
                m_targetInfo.Set(null, null);
                m_enablePatience = false;
                m_currentState = State.Patrol;
            }
        }

        private void DecideOnAttack()
        {
            m_chosenAttack = (Attack)UnityEngine.Random.Range(0, (int)Attack._COUNT - 1);
            switch (m_chosenAttack)
            {
                case Attack.RapidSting:
                    m_focusedAttackRange = m_info.rapidSting.range;
                    break;
                case Attack.StingerDive:
                    m_focusedAttackRange = m_info.stingerdive.range;
                    break;
            }
            m_hasDecidedOnAttack = true;
        }

        private IEnumerator RapidStingRoutine()
        {
            transform.SetParent(m_targetInfo.transform);
            m_character.physics.bodyType = RigidbodyType2D.Kinematic;
            m_animation.SetAnimation(0, m_info.rapidSting.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rapidSting.animation);
            transform.SetParent(null);
            m_character.physics.bodyType = RigidbodyType2D.Dynamic;
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_currentState = m_afterWaitForBehaviourState;
            yield return null;
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            m_agent.Stop();
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
        }

        private void Update()
        {
            switch (m_currentState)
            {
                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    if (m_targetInfo.isValid == false)
                    {
                        m_currentState = State.Patrol;
                    }
                    break;

                case State.Patrol:
                    m_animation.SetAnimation(0, m_info.patrol.animation, true);
                    var characterInfo = new PatrolHandle.CharacterInfo(m_character.transform.position, m_character.facing);
                    m_patrolHandle.Patrol(m_agent, m_info.patrol.speed, characterInfo);
                    break;

                case State.Turning:
                    WaitTillBehaviourEnd(State.ReevaluateSituation);
                    m_agent.Stop();
                    m_turnHandle.Execute(m_info.turnAnimation);
                    break;
                case State.Attacking:
                    WaitTillBehaviourEnd(State.ReevaluateSituation);
                    switch (m_chosenAttack)
                    {
                        case Attack.StingerDive:
                            m_animation.EnableRootMotion(true, false);
                            m_attackHandle.ExecuteAttack(m_info.stingerdive.animation);
                            m_animation.AddAnimation(0, m_info.idleAnimation, true, 0);
                            break;
                        case Attack.RapidSting:
                            WaitTillBehaviourEnd(State.ReevaluateSituation);
                            StartCoroutine(RapidStingRoutine());
                            break;
                    }
                    m_hasDecidedOnAttack = false;
                    break;
                case State.Chasing:
                    if (IsFacingTarget())
                    {
                        var target = m_targetInfo.position;
                        target.y -= 0.5f;
                        m_animation.EnableRootMotion(false, false);
                        m_animation.SetAnimation(0, m_info.move.animation, true);
                        m_agent.SetDestination(target);
                        if (m_agent.hasPath)
                        {
                            m_agent.Move(m_info.move.speed);
                        }

                        if (m_hasDecidedOnAttack == false)
                        {
                            DecideOnAttack();
                        }

                        if (m_hasDecidedOnAttack && Vector2.Distance(m_centerMass.position, m_targetInfo.position) <= m_focusedAttackRange)
                        {
                            m_currentState = State.Attacking;
                        }
                    }
                    else
                    {
                        m_currentState = State.Turning;
                    }
                    break;

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    if (m_targetInfo.isValid)
                    {
                        m_currentState = State.Chasing;
                    }
                    else
                    {
                        m_currentState = State.Idle;
                    }
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }

            if (m_enablePatience)
            {
                Patience();
            }
        }


    }
}
