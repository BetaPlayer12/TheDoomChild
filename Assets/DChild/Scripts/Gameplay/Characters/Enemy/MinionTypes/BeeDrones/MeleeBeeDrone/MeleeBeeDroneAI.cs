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
        //Patience Handler
        private float m_currentPatience;
        private bool m_enablePatience;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_stateHandle.SetState(State.ReevaluateSituation);
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                m_stateHandle.SetState(State.Chasing);
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
            m_stateHandle.ApplyQueuedState();
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
                m_stateHandle.SetState(State.Patrol);
            }
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
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            m_agent.Stop();
        }

        public override void ApplyData()
        {
            base.ApplyData();
            UpdateAttackDeciderList();
        }
        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.RapidSting, m_info.rapidSting.range),
                                    new AttackInfo<Attack>(Attack.StingerDive, m_info.stingerdive.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();
        }


        private void Update()
        {
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    if (m_targetInfo.isValid == false)
                    {
                        m_stateHandle.SetState(State.Patrol);
                    }
                    break;

                case State.Patrol:
                    m_animation.SetAnimation(0, m_info.patrol.animation, true);
                    var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    m_patrolHandle.Patrol(m_agent, m_info.patrol.speed, characterInfo);
                    break;

                case State.Turning:
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    m_agent.Stop();
                    m_turnHandle.Execute(m_info.turnAnimation);
                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.StingerDive:
                            m_animation.EnableRootMotion(true, false);
                            m_attackHandle.ExecuteAttack(m_info.stingerdive.animation);
                            m_animation.AddAnimation(0, m_info.idleAnimation, true, 0);
                            break;
                        case Attack.RapidSting:
                            StartCoroutine(RapidStingRoutine());
                            break;
                    }
                    m_attackDecider.hasDecidedOnAttack = false;
                    break;
                case State.Chasing:
                    if (IsFacingTarget())
                    {
                        var target = m_targetInfo.position;
                        target.y -= 0.5f;
                        m_animation.DisableRootMotion();
                        m_animation.SetAnimation(0, m_info.move.animation, true);
                        m_agent.SetDestination(target);
                        if (m_agent.hasPath)
                        {
                            m_agent.Move(m_info.move.speed);
                        }

                        m_attackDecider.DecideOnAttack();
                        if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range))
                        {
                            m_stateHandle.SetState(State.Attacking);
                        }
                    }
                    else
                    {
                        m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    if (m_targetInfo.isValid)
                    {
                        m_stateHandle.SetState(State.Chasing);
                    }
                    else
                    {
                        m_stateHandle.SetState(State.Idle);
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
