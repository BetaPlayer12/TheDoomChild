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

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/Nightmare")]
    public class NightmareAI : CombatAIBrain<NightmareAI.Info>, IResetableAIBrain, ISummonedEnemy
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
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_attackBreakAnimation;
            public string attackBreakAnimation => m_attackBreakAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_prepAttackAnimation;
            public string prepAttackAnimation => m_prepAttackAnimation;
            [SerializeField, MinValue(0)]
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
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            //[SerializeField, ValueDropdown("GetAnimations")]
            //private string m_detectAnimation;
            //public string detectAnimation => m_detectAnimation;
            //[SerializeField, ValueDropdown("GetAnimations")]
            //private string m_counterFlinchAnimation;
            //public string counterFlinchAnimation => m_counterFlinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_fireAnimation;
            public string fireAnimation => m_fireAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinch1Animation;
            public string flinch1Animation => m_flinch1Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinch2Animation;
            public string flinch2Animation => m_flinch2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinch2BackAnimation;
            public string flinch2BackAnimation => m_flinch2BackAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;


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
            Detect,
            Patrol,
            Turning,
            Attacking,
            Cooldown,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            Attack,
            //Attack3_2,
            [HideInInspector]
            _COUNT
        }
        
        [SerializeField, TabGroup("Reference")]
        private Health m_health;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_selfCollider;
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
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;

        private float m_currentPatience;
        private float m_currentCD;
        private float m_currentFullCD;
        private float m_currentTimeScale;
        private float m_currentMoveSpeed;
        private bool m_enablePatience;
        private bool m_isDetecting;
        private Vector2 m_startPoint;
        //private bool m_isCharging;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private State m_turnState;
        //private IEnumerator m_chargeBreakRoutine;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_flinchHandle.m_autoFlinch = true;
            m_animation.animationState.TimeScale = 1;
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable);
                m_animation.animationState.TimeScale = 1;
                m_selfCollider.enabled = false;
                if (m_stateHandle.currentState != State.Chasing && !m_isDetecting)
                {
                    m_isDetecting = true;
                    m_stateHandle.SetState(State.Detect);
                }
                m_currentPatience = 0;
                //var patienceRoutine = PatienceRoutine();
                //StopCoroutine(patienceRoutine);
                m_enablePatience = false;
            }
            else
            {
                //if (!m_enablePatience)
                //{
                //    m_enablePatience = true;
                //    //Patience();
                //    StartCoroutine(PatienceRoutine());
                //}
                m_enablePatience = true;
                //StartCoroutine(PatienceRoutine());
            }
        }

        public void SummonAt(Vector2 position, AITargetInfo target)
        {
            //Lefix commento 3====D----
            enabled = false;
            m_targetInfo = target;
            m_isDetecting = true;
            var xOffSet = position.x - transform.position.x;
            transform.position = new Vector2(m_targetInfo.position.x + xOffSet, GroundPosition(m_targetInfo.position).y);
            m_character.physics.simulateGravity = false;
            m_flinchHandle.gameObject.SetActive(true);
            m_health.SetHealthPercentage(1f);
            m_hitbox.Enable();
            this.gameObject.SetActive(true);
            this.transform.SetParent(null);
            if (!IsFacingTarget())
                CustomTurn();
            Awake();
            m_stateHandle.OverrideState(State.Detect);
            enabled = true;
        }

        public void DestroyObject()
        {
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
                m_selfCollider.enabled = false;
                m_targetInfo.Set(null, null);
                m_flinchHandle.m_autoFlinch = true;
                m_isDetecting = false;
                m_enablePatience = false;
                m_stateHandle.SetState(State.Patrol);
            }
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            StopAllCoroutines();
            GetComponentInChildren<Hitbox>().gameObject.SetActive(false);
            base.OnDestroyed(sender, eventArgs);
            m_movement.Stop();
            m_selfCollider.enabled = false;
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                StopAllCoroutines();
                m_selfCollider.enabled = false;
                StartCoroutine(FlincRoutine());
                //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            }
        }

        private IEnumerator FlincRoutine()
        {
            m_animation.EnableRootMotion(true, false);
            string flinch = "";
            if (IsFacingTarget())
            {
                var randomFlinch = UnityEngine.Random.Range(0, 2);
                flinch = randomFlinch == 1 ? m_info.flinch1Animation : m_info.flinch2Animation;
            }
            else
            {
                flinch = m_info.flinch2BackAnimation;
            }
            m_animation.SetAnimation(0, flinch, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, flinch);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation)
                    m_animation.SetAnimation(0, m_info.move.animation, false);
                //m_animation.SetAnimation(0, m_info.idleAnimation, true);
                m_stateHandle.OverrideState(State.ReevaluateSituation);
            }
        }

        public override void ApplyData()
        {
            base.ApplyData();
            if (m_attackDecider != null)
            {
                UpdateAttackDeciderList();
            }
        }

        public void ResetAI()
        {
            m_selfCollider.enabled = false;
            m_targetInfo.Set(null, null);
            m_flinchHandle.m_autoFlinch = true;
            m_isDetecting = false;
            m_enablePatience = false;
            m_stateHandle.OverrideState(State.Patrol);
            enabled = true;
        }

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack, m_info.attack.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {
            //m_animation.SetAnimation(0, m_info.detectAnimation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        private IEnumerator ChargeAttackRoutine()
        {
            var toTarget = m_targetInfo.position - (Vector2)m_character.centerMass.position;
            m_animation.EnableRootMotion(false, false);
            m_selfCollider.enabled = false;
            m_animation.SetAnimation(0, m_info.prepAttackAnimation, false);
            var waitTime = m_animation.animationState.GetCurrent(0).AnimationEnd * 0.85f;
            yield return new WaitForSeconds(waitTime);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.prepAttackAnimation);
            m_animation.SetAnimation(0, m_info.attack.animation, true);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack.animation);
            //StartCoroutine(m_chargeBreakRoutine);
            float time = 0;
            while (time < 3)
            {
                time += Time.deltaTime;
                //m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_currentMoveSpeed);
                m_character.physics.SetVelocity(toTarget.normalized.x * m_currentMoveSpeed, m_character.physics.velocity.y);
                if (!m_edgeSensor.isDetecting)
                {
                    time = 3;
                }
                yield return null;
            }
            //yield return new WaitForSeconds(3);
            //StopCoroutine(m_chargeBreakRoutine);
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.attackBreakAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attackBreakAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_flinchHandle.m_autoFlinch = true;
            m_selfCollider.enabled = true;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        //private IEnumerator ChargeBreakRoutine()
        //{
        //    while (m_edgeSensor.isDetecting)
        //    {
        //        yield return null;
        //    }
        //    m_movement.Stop();
        //    m_animation.SetAnimation(0, m_info.attackBreakAnimation, false);
        //    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attackBreakAnimation);
        //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
        //    m_stateHandle.ApplyQueuedState();
        //    yield return null;
        //}

        private IEnumerator DetectWallRoutine()
        {
            yield return new WaitUntil(() => m_wallSensor.allRaysDetecting);
            StopAllCoroutines();
            m_attackHandle.ExecuteAttack(m_info.attackBreakAnimation, m_info.idleAnimation);
        }

        //private IEnumerator CounterStrikeRoutine()
        //{
        //    if (!IsFacingTarget())
        //    {
        //        CustomTurn();
        //    }
        //    m_animation.EnableRootMotion(true, false);
        //    m_animation.SetAnimation(0, m_info.counterFlinchAnimation, false);
        //    m_animation.animationState.TimeScale = 2;
        //    yield return new WaitForSeconds(.15f);
        //    m_animation.animationState.TimeScale = 1;
        //    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.counterFlinchAnimation);
        //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
        //    m_stateHandle.OverrideState(State.ReevaluateSituation);
        //    yield return null;
        //}

        protected override void Start()
        {
            base.Start();
            m_animation.SetAnimation(5, m_info.fireAnimation, true);
            m_currentTimeScale = UnityEngine.Random.Range(1.0f, 2.0f);
            m_currentFullCD = UnityEngine.Random.Range(m_info.attackCD * .5f, m_info.attackCD * 2f);
            m_currentMoveSpeed = UnityEngine.Random.Range(m_info.move.speed * .75f, m_info.move.speed * 1.25f);
            //m_chargeBreakRoutine = ChargeBreakRoutine();
            m_startPoint = transform.position;
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
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();
        }


        private void Update()
        {
            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    m_movement.Stop();
                    m_flinchHandle.m_autoFlinch = false;
                    if (IsFacingTarget())
                    {
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        StartCoroutine(DetectRoutine());
                    }
                    else
                    {
                        m_turnState = State.Detect;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.Patrol:
                    if (/*!m_wallSensor.isDetecting &&*/ m_groundSensor.isDetecting)
                    {
                        m_turnState = State.ReevaluateSituation;
                        m_animation.EnableRootMotion(false, false);
                        m_animation.SetAnimation(0, m_info.patrol.animation, true);
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        m_patrolHandle.Patrol(m_movement, m_info.patrol.speed, characterInfo);
                    }
                    else
                    {
                        m_movement.Stop();
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    }
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_movement.Stop();
                    m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                    m_animation.animationState.GetCurrent(0).MixDuration = 1;
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);

                    StartCoroutine(ChargeAttackRoutine());
                    StartCoroutine(DetectWallRoutine());
                    m_attackDecider.hasDecidedOnAttack = false;

                    break;

                case State.Cooldown:
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                            m_stateHandle.SetState(State.Turning);
                    }

                    if (m_currentCD <= m_currentFullCD)
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
                        m_flinchHandle.m_autoFlinch = false;
                        var toTarget = m_targetInfo.position - (Vector2)m_character.centerMass.position;
                        if (IsFacingTarget())
                        {
                            m_attackDecider.DecideOnAttack();
                            if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range) && !m_wallSensor.allRaysDetecting && m_edgeSensor.allRaysDetecting)
                            {
                                m_movement.Stop();
                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                m_animation.EnableRootMotion(false, false);
                                if (!m_wallSensor.allRaysDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                                {
                                    m_selfCollider.enabled = false;
                                    m_animation.SetAnimation(0, m_info.move.animation, true).TimeScale = m_currentTimeScale;
                                    //m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_currentMoveSpeed);
                                    //m_movement.MoveTowards(toTarget.normalized, m_currentMoveSpeed);
                                    m_character.physics.SetVelocity(toTarget.normalized.x * m_currentMoveSpeed, m_character.physics.velocity.y);
                                }
                                else
                                {
                                    m_movement.Stop();
                                    m_selfCollider.enabled = true;
                                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                }
                            }
                        }
                        else
                        {
                            m_turnState = State.ReevaluateSituation;
                            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                                m_stateHandle.SetState(State.Turning);
                        }
                    }
                    break;

                case State.ReevaluateSituation:
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
                    return;
            }

            if (m_enablePatience)
            {
                Patience();
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Patrol);
            GetComponentInChildren<Hitbox>().gameObject.SetActive(true);
            m_flinchHandle.m_autoFlinch = true;
            m_currentPatience = 0;
            m_enablePatience = false;
            m_isDetecting = false;
            m_selfCollider.enabled = false;
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
