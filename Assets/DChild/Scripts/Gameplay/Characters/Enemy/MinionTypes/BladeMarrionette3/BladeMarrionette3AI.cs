using System;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using DChild.Gameplay.Characters.AI;
using UnityEngine;
using Spine;
using Spine.Unity;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using DChild;
using DChild.Gameplay.Characters.Enemies;

namespace DChild.Gameplay.Characters.Enemies
{
    public class BladeMarrionette3AI : CombatAIBrain<BladeMarrionette3AI.Info>
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
            private SimpleAttackInfo m_attack = new SimpleAttackInfo();
            public SimpleAttackInfo attack => m_attack;
            [SerializeField]
            private SimpleAttackInfo m_attackLong = new SimpleAttackInfo();
            public SimpleAttackInfo attackLong => m_attackLong;
            [SerializeField]
            private SimpleAttackInfo m_attackTwitch = new SimpleAttackInfo();
            public SimpleAttackInfo attackTwitch => m_attackTwitch;
            //

            [SerializeField, MinValue(0)]
            private float m_patience;
            public float patience => m_patience;
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_damageAnimation;
            public string damageAnimation => m_damageAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_attack.SetData(m_skeletonDataAsset);
                m_attackLong.SetData(m_skeletonDataAsset);
                attackTwitch.SetData(m_skeletonDataAsset);
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
            Attack,
            AttackLong,
            AttackTwitch,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        //Patience Handler
        private float m_currentPatience;
        private bool m_enablePatience;
        private Vector2 m_startPoint;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_stateHandle.OverrideState(State.ReevaluateSituation);
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
                if (!IsTargetInRange(m_info.targetDistanceTolerance))
                {
                    m_enablePatience = true;
                }
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

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            m_movement.Stop();
        }

        public void SetDirection(float direction)
        {
            transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);
        }

        public override void ApplyData()
        {
            base.ApplyData();
            if (m_attackDecider != null)
            {
                Debug.Log("Update attack list trigger function");
                UpdateAttackDeciderList();
            }
        }
        private void UpdateAttackDeciderList()
        {
            Debug.Log("Update attack list trigger");
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack, m_info.attack.range),
                                    new AttackInfo<Attack>(Attack.AttackLong, m_info.attackLong.range),
                                    new AttackInfo<Attack>(Attack.AttackTwitch, m_info.attackTwitch.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator Wait()
        {
            while (m_animation.skeletonAnimation.AnimationState.GetCurrent(0).IsComplete)
            {
                yield return null;
            }
        }
        protected override void Start()
        {
            base.Start();
            m_startPoint = transform.position;
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
                    break;

                case State.Patrol:
                    m_animation.EnableRootMotion(false, false);
                    m_animation.SetAnimation(0, m_info.patrol.animation, true);
                    var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    m_patrolHandle.Patrol(m_movement, m_info.patrol.speed, characterInfo);
                    break;

                case State.Turning:
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    m_movement.Stop();
                    m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.ReevaluateSituation);

                    StartCoroutine(Wait()); //This is just to fix the transition issue with attacking
                    m_movement.Stop();

                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.Attack:
                            m_animation.EnableRootMotion(true, false);
                            m_attackHandle.ExecuteAttack(m_info.attack.animation, m_info.idleAnimation);
                            m_animation.AddAnimation(0, m_info.idleAnimation, true, 0);
                            break;
                        case Attack.AttackLong:
                            m_animation.EnableRootMotion(true, false);
                            m_attackHandle.ExecuteAttack(m_info.attackLong.animation, m_info.idleAnimation);
                            m_animation.AddAnimation(0, m_info.idleAnimation, true, 0);
                            break;
                        case Attack.AttackTwitch:
                            m_animation.EnableRootMotion(true, false);
                            m_attackHandle.ExecuteAttack(m_info.attackTwitch.animation, m_info.idleAnimation);
                            m_animation.AddAnimation(0, m_info.idleAnimation, true, 0);
                            break;
                    }
                    m_attackDecider.hasDecidedOnAttack = false;

                    break;
                case State.Chasing:
                    {
                        if (IsFacingTarget())
                        {
                            if (!m_wallSensor.isDetecting && m_groundSensor.allRaysDetecting)
                            {
                                m_attackDecider.DecideOnAttack();
                                if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range))
                                {
                                    m_stateHandle.SetState(State.Attacking);
                                }
                                else
                                {
                                    m_animation.EnableRootMotion(false, false);
                                    m_animation.SetAnimation(0, m_info.move.animation, true).TimeScale = 1.5f;
                                    m_movement.MoveTowards(m_targetInfo.position, m_info.move.speed * transform.localScale.x);
                                }
                            }
                            else
                            {
                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                            }
                        }
                        else
                        {
                            m_stateHandle.SetState(State.Turning);
                        }
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
                        m_stateHandle.SetState(State.Patrol);
                    }
                    break;
                case State.WaitBehaviourEnd:
                    //Debug.Log("Still wetting");
                    return;
            }

            if (m_enablePatience)
            {
                Patience();
            }

            m_wallSensor.transform.localScale = new Vector3(transform.localScale.x, m_wallSensor.transform.localScale.y, m_wallSensor.transform.localScale.z);
            m_groundSensor.transform.localScale = new Vector3(transform.localScale.x, m_groundSensor.transform.localScale.y, m_groundSensor.transform.localScale.z);
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Patrol);
            m_currentPatience = 0;
            m_enablePatience = false;
        }

        public void ResetAI()
        {
            m_targetInfo.Set(null, null);
            m_enablePatience = false;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            enabled = true;
        }

        public override void ReturnToSpawnPoint()
        {
            transform.position = m_startPoint;
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }
    }
}
