﻿using System;
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
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/BlobAcid")]
    public class BlobAcidAI : CombatAIBrain<BlobAcidAI.Info>
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
            private float m_attackCD;
            public float attackCD => m_attackCD;
            //

            [SerializeField, MinValue(0)]
            private float m_patience;
            public float patience => m_patience;

            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;


            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_sleepAnimation;
            public string sleepAnimation => m_sleepAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_detectAnimation;
            public string detectAnimation => m_detectAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_smokeEvent;
            public string smokeEvent => m_smokeEvent;


            //[SerializeField]
            //private GameObject m_spitProjectile;
            //public GameObject spitProjectile => m_spitProjectile;


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
            Sleep,
            Detect,
            Turning,
            Attacking,
            Cooldown,
            Chasing,
            Flinch,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            Attack,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Modules")]
        private TransformTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Modules")]
        private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        //Patience Handler
        private float m_currentPatience;
        private bool m_enablePatience;

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_selfCollider;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_smokeFX;

        private float m_targetDistance;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private State m_turnState;

        private bool m_isDetecting;
        private float m_currentCD;

        protected override void Start()
        {
            base.Start();
            m_selfCollider.SetActive(false);
            m_spineEventListener.Subscribe(m_info.smokeEvent, SmokeFX);
        }

        private void SmokeFX()
        {
            //m_smokeFX.Play();
        }

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                m_selfCollider.SetActive(true);
                m_currentPatience = 0;
                m_enablePatience = false;
                //StopCoroutine(PatienceRoutine()); //for latur
                if (m_stateHandle.currentState != State.Chasing && !m_isDetecting)
                {
                    m_isDetecting = true;
                    m_stateHandle.SetState(State.Detect);
                }
                if (transform.localRotation.z != 0)
                {
                    transform.localRotation = Quaternion.Euler(Vector3.zero);
                    GetComponent<IsolatedPhysics2D>().simulateGravity = true;
                    m_animation.DisableRootMotion();
                }
            }
            else
            {
                m_enablePatience = true;
                //StartCoroutine(PatienceRoutine());
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
                m_selfCollider.SetActive(false);
                m_targetInfo.Set(null, null);
                m_isDetecting = false;
                m_enablePatience = false;
                m_stateHandle.SetState(State.Sleep);
            }
        }
        //private IEnumerator PatienceRoutine()
        //{
        //    yield return new WaitForSeconds(m_info.patience);
        //    m_targetInfo.Set(null, null);
        //    m_isDetecting = false;
        //    m_stateHandle.SetState(State.Patrol);
        //}

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            base.OnDestroyed(sender, eventArgs);
            m_movement.Stop();
        }

        private IEnumerator DetectRoutine()
        {
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            //m_animation.SetAnimation(0, m_info.damageAnimation, false);
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation)
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        public override void ApplyData()
        {
            base.ApplyData();
            if (m_attackDecider != null)
            {
                UpdateAttackDeciderList();
            }
        }

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack, m_info.attack.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_stateHandle = new StateHandle<State>(State.Sleep, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            //m_projectileLauncher = new ProjectileLauncher(m_info.projectile.projectileInfo, m_throwPoint);
            UpdateAttackDeciderList();
        }


        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Ground Sensor is " + m_groundSensor.isDetecting);
            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    m_movement.Stop();
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    StartCoroutine(DetectRoutine());
                    break;
                case State.Sleep:
                    m_animation.SetAnimation(0, m_info.sleepAnimation, true);
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute();
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);

                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.Attack:
                            m_animation.EnableRootMotion(false, false);
                            m_attackHandle.ExecuteAttack(m_info.attack.animation, m_info.idleAnimation);
                            break;
                    }
                    m_attackDecider.hasDecidedOnAttack = false;

                    break;

                case State.Cooldown:
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        m_stateHandle.SetState(State.Turning);
                    }

                    if (m_currentCD <= m_info.attackCD)
                    {
                        m_currentCD += Time.deltaTime;
                    }
                    else
                    {
                        m_currentCD = 0;
                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                    }

                    break;
                case State.Chasing:
                    {
                        if (IsFacingTarget())
                        {
                            m_attackDecider.DecideOnAttack();
                            if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range) && !m_wallSensor.allRaysDetecting)
                            {
                                m_movement.Stop();
                                //m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting /*&& m_edgeSensor.isDetecting*/)
                                {
                                    m_animation.EnableRootMotion(false, false);
                                    m_animation.SetAnimation(0, m_info.move.animation, true);
                                    m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.move.speed);
                                }
                                else
                                {
                                    m_attackDecider.hasDecidedOnAttack = false;
                                    m_movement.Stop();
                                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                }
                            }
                        }
                        else
                        {
                            m_turnState = State.ReevaluateSituation;
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
                        m_stateHandle.SetState(State.Sleep);
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

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Sleep);
            m_currentPatience = 0;
            m_enablePatience = false;
            m_isDetecting = false;
            m_selfCollider.SetActive(false);
        }
    }
}
