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
using DChild.Gameplay.Environment;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/HeavySkeleton")]
    public class HeavySkeletonAI : CombatAIBrain<HeavySkeletonAI.Info>, IResetableAIBrain, IKnockbackable
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField, TabGroup("Movement")]
            private MovementInfo m_walk = new MovementInfo();
            public MovementInfo walk => m_walk;

            //Attack Behaviours
            [SerializeField, TabGroup("Attack")]
            private SimpleAttackInfo m_attack = new SimpleAttackInfo();
            public SimpleAttackInfo attack => m_attack;
            [SerializeField, TabGroup("Attack")]
            private SimpleAttackInfo m_attackBelow = new SimpleAttackInfo();
            public SimpleAttackInfo attackBelow => m_attackBelow;
            [SerializeField, MinValue(0), TabGroup("Attack")]
            private float m_blockRange;
            public float blockRange => m_blockRange;
            [SerializeField, MinValue(0), TabGroup("Attack")]
            private float m_blockDuration;
            public float blockDuration => m_blockDuration;
            [SerializeField, MinValue(0), TabGroup("Attack")]
            private float m_attackCD;
            public float attackCD => m_attackCD;
            //
            [SerializeField, MinValue(0)]
            private float m_dismantleDuration;
            public float dismantleDuration => m_dismantleDuration;
            [SerializeField, MinValue(0)]
            private float m_patience;
            public float patience => m_patience;

            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;


            //Animations
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_blockStartAnimation;
            public BasicAnimationInfo blockStartAnimation => m_blockStartAnimation;
            [SerializeField]
            private BasicAnimationInfo m_blockLoopAnimation;
            public BasicAnimationInfo blockLoopAnimation => m_blockLoopAnimation;
            [SerializeField]
            private BasicAnimationInfo m_blockParryAnimation;
            public BasicAnimationInfo blockParryAnimation => m_blockParryAnimation;
            [SerializeField]
            private BasicAnimationInfo m_counterFlinch1Animation;
            public BasicAnimationInfo counterFlinch1Animation => m_counterFlinch1Animation;
            [SerializeField]
            private BasicAnimationInfo m_counterFlinch2Animation;
            public BasicAnimationInfo counterFlinch2Animation => m_counterFlinch2Animation;
            [SerializeField]
            private BasicAnimationInfo m_parryHardAnimation;
            public BasicAnimationInfo parryHardAnimation => m_parryHardAnimation;
            [SerializeField]
            private BasicAnimationInfo m_parryLightAnimation;
            public BasicAnimationInfo parryLightAnimation => m_parryLightAnimation;
            [SerializeField]
            private BasicAnimationInfo m_parryNormalAnimation;
            public BasicAnimationInfo parryNormalAnimation => m_parryNormalAnimation;
            [SerializeField]
            private BasicAnimationInfo m_disassembledIdleAnimation;
            public BasicAnimationInfo disassembledIdleAnimation => m_disassembledIdleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation;
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation;
            public BasicAnimationInfo turnAnimation => m_turnAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_dismantleAnimation;
            public BasicAnimationInfo dismantleAnimation => m_dismantleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_resurrectAnimation;
            public BasicAnimationInfo resurrectAnimation => m_resurrectAnimation;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_walk.SetData(m_skeletonDataAsset);
                m_attack.SetData(m_skeletonDataAsset);
                m_attackBelow.SetData(m_skeletonDataAsset);

                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_blockStartAnimation.SetData(m_skeletonDataAsset);
                m_blockLoopAnimation.SetData(m_skeletonDataAsset);
                m_blockParryAnimation.SetData(m_skeletonDataAsset);
                m_counterFlinch1Animation.SetData(m_skeletonDataAsset);
                m_counterFlinch2Animation.SetData(m_skeletonDataAsset);
                m_parryHardAnimation.SetData(m_skeletonDataAsset);
                m_parryLightAnimation.SetData(m_skeletonDataAsset);
                m_parryNormalAnimation.SetData(m_skeletonDataAsset);
                m_disassembledIdleAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_dismantleAnimation.SetData(m_skeletonDataAsset);
                m_resurrectAnimation.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Detect,
            Idle,
            Patrol,
            Standby,
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
            AttackBelow,
            Block,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
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
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Modules")]
        private Health m_health;

        private float m_currentPatience;
        private float m_currentCD;
        private float m_currentFullCD;
        private float m_currentTimeScale;
        private float m_currentRunAttackDuration;
        private bool m_enablePatience;
        private bool m_isDetecting;
        private Vector2 m_startpoint;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;
        [SerializeField, TabGroup("FX")]
        private HitFXHandle m_slashFX;
        [SerializeField, TabGroup("FX")]
        private HitFXHandle m_blockFX;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private State m_turnState;

        private Coroutine m_attackRoutine;
        private Coroutine m_sneerRoutine;
        private Coroutine m_patienceRoutine;
        private Coroutine m_randomIdleRoutine;
        private Coroutine m_randomTurnRoutine;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            //m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null /*&& !ShotBlocked()*/)
            {
                base.SetTarget(damageable);
                //if (m_stateHandle.currentState != State.Chasing 
                //    && m_stateHandle.currentState != State.RunAway 
                //    && m_stateHandle.currentState != State.Turning 
                //    && m_stateHandle.currentState != State.WaitBehaviourEnd)
                //{
                //}
                m_selfCollider.enabled = false;

                if (!m_isDetecting)
                {
                    if (m_randomIdleRoutine != null)
                    {
                        StopCoroutine(m_randomIdleRoutine);
                        m_randomIdleRoutine = null;
                    }
                    if (m_randomTurnRoutine != null)
                    {
                        StopCoroutine(m_randomTurnRoutine);
                        m_randomTurnRoutine = null;
                    }
                    m_isDetecting = true;
                    m_stateHandle.SetState(State.Detect);
                }
                m_currentPatience = 0;
                //m_randomIdleRoutine = null;
                //var patienceRoutine = PatienceRoutine();
                //StopCoroutine(patienceRoutine);
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
            if (m_patienceRoutine == null)
            {
                m_patienceRoutine = StartCoroutine(PatienceRoutine());
            }
            if (TargetBlocked())
            {
                if (IsFacingTarget())
                {
                    if (m_sneerRoutine == null)
                    {
                        m_sneerRoutine = StartCoroutine(SneerRoutine());
                    }
                    //else if ()
                    //{
                    //}
                }
                else
                {
                    if (m_sneerRoutine != null)
                    {
                        StopCoroutine(m_sneerRoutine);
                        m_sneerRoutine = null;
                    }
                    //m_enablePatience = false;
                    m_turnState = State.WaitBehaviourEnd;
                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                        m_stateHandle.SetState(State.Turning);
                }
            }
            else if (!TargetBlocked())
            {
                if (m_sneerRoutine != null)
                {
                    if (m_patienceRoutine != null)
                    {
                        StopCoroutine(m_patienceRoutine);
                        m_patienceRoutine = null;
                    }
                    StopCoroutine(m_sneerRoutine);
                    m_sneerRoutine = null;
                    m_enablePatience = false;
                    m_stateHandle.OverrideState(State.ReevaluateSituation);
                }
            }
        }
        private IEnumerator PatienceRoutine()
        {
            //if (m_enablePatience)
            //{
            //    while (m_currentPatience < m_info.patience)
            //    {
            //        m_currentPatience += m_character.isolatedObject.deltaTime;
            //        yield return null;
            //    }
            //}
            yield return new WaitForSeconds(m_info.patience);
            m_selfCollider.enabled = false;
            m_enablePatience = false;
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            if (m_sneerRoutine != null)
            {
                StopCoroutine(m_sneerRoutine);
                m_sneerRoutine = null;
            }
            m_stateHandle.SetState(State.Patrol);
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);
            GameplaySystem.minionManager.Unregister(this);
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            if (m_attackRoutine != null)
            {
                StopCoroutine(m_attackRoutine);
            }
            if (m_sneerRoutine != null)
            {
                StopCoroutine(m_sneerRoutine);
            }
            m_character.physics.UseStepClimb(true);
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                m_movement.Stop();
            
            m_flinchHandle.gameObject.SetActive(false);
            m_animation.SetEmptyAnimation(0, 0);
            m_animation.SetEmptyAnimation(1, 0);
            m_animation.SetEmptyAnimation(2, 0);
            StartCoroutine(ResurrectRoutine());
        }

        private IEnumerator ResurrectRoutine()
        {
            m_hitbox.Disable();
            m_selfCollider.enabled = false;
            m_animation.SetAnimation(0, m_info.dismantleAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.dismantleAnimation);
            m_animation.SetAnimation(0, m_info.disassembledIdleAnimation, true);
            yield return new WaitForSeconds(m_info.dismantleDuration);
            m_flinchHandle.gameObject.SetActive(true);
            m_health.SetHealthPercentage(1f);
            enabled = true;
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.OverrideState(m_targetInfo.isValid ? State.Detect : State.Idle);
            yield return null;
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idleAnimation.animation)
            {
                StopAllCoroutines();
                m_selfCollider.enabled = true;
                //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                m_stateHandle.Wait(State.ReevaluateSituation);
                StartCoroutine(FlinchRoutine());
            }
        }

        private IEnumerator FlinchRoutine()
        {
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private void CounterFlinch(object sender, Damageable.DamageEventArgs eventArgs)
        {
            //Debug.Log("HEAVY SKELETON BLOCKING");
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.blockLoopAnimation.animation)
            {
                StopAllCoroutines();
                //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                m_stateHandle.Wait(State.ReevaluateSituation);
                StartCoroutine(CounterFlinchRoutine());
            }

            //var instance = GameSystem.poolManager.GetPool<FXPool>().GetOrCreateItem(m_hitFX);
            //instance.transform.position = m_character.centerMass.position;
            //instance.transform.rotation = Quaternion.Euler(0, 0, transform.position.x >= m_targetInfo.position.x ? 0 : 180f);
        }

        private IEnumerator CounterFlinchRoutine()
        {
            var randomCounter = UnityEngine.Random.Range(0, 2) == 1 ? m_info.counterFlinch1Animation : m_info.counterFlinch2Animation;
            m_animation.SetAnimation(0, randomCounter, false);
            yield return new WaitForSeconds(0.1f);
            m_hitbox.SetCanBlockDamageState(false);
            yield return new WaitForAnimationComplete(m_animation.animationState, randomCounter);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private void SpawnHitFX(object sender, Damageable.DamageEventArgs eventArgs)
        {
            if (m_targetInfo.isValid && enabled)
            {
                if (m_hitbox.canBlockDamage)
                {
                    m_blockFX.SpawnFX(m_blockFX.transform.position, /*m_targetInfo.facing*/m_targetInfo.transform.position.x > transform.position.x ? HorizontalDirection.Left : HorizontalDirection.Right);
                }
                else
                {
                    m_slashFX.SpawnFX(m_character.centerMass.position, /*m_targetInfo.facing*/m_targetInfo.transform.position.x > transform.position.x ? HorizontalDirection.Left : HorizontalDirection.Right);
                }
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

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack, m_info.attack.range)
                                  , new AttackInfo<Attack>(Attack.AttackBelow, m_info.attackBelow.range)
                                  , new AttackInfo<Attack>(Attack.Block, m_info.blockRange));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {
            m_animation.SetAnimation(0, m_info.resurrectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.resurrectAnimation);
            m_hitbox.Enable();
            m_selfCollider.enabled = true;
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator RandomIdleRoutine()
        {
            while (true)
            {
                var timer = UnityEngine.Random.Range(5, 10);
                var currentTimer = 0f;
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                while (currentTimer < timer)
                {
                    currentTimer += Time.deltaTime;
                    yield return null;
                }
                yield return null;
            }
        }

        private IEnumerator RandomTurnRoutine()
        {
            while (true)
            {
                var timer = UnityEngine.Random.Range(5, 10);
                var currentTimer = 0f;
                while (currentTimer < timer)
                {
                    currentTimer += Time.deltaTime;
                    yield return null;
                }
                m_turnState = State.Idle;
                m_stateHandle.SetState(State.Turning);
                yield return null;
            }
        }

        private IEnumerator SneerRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                m_movement.Stop();

            while (true)
            {
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                //m_animation.SetAnimation(0, m_info.rawrAnimation, false);
                //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rawrAnimation);

                //yield return new WaitForSeconds(3f);
                yield return null;
            }
        }

        private IEnumerator BlockAttackRoutine()
        {
            m_hitbox.SetCanBlockDamageState(true);
            m_animation.SetAnimation(0, m_info.blockStartAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.blockStartAnimation);
            m_animation.SetAnimation(0, m_info.blockLoopAnimation, true);
            float time = 0;
            while (time < m_info.blockDuration)
            {
                if (!IsFacingTarget())
                {
                    CustomTurn();
                }
                time += Time.deltaTime;
                yield return null;
            }
            //yield return new WaitForSeconds(m_info.blockDuration);
            m_hitbox.SetCanBlockDamageState(false);
            var parryAnim = RandomParryAttack();
            m_animation.SetAnimation(0, parryAnim, false).TimeScale = 2;
            yield return new WaitForAnimationComplete(m_animation.animationState, parryAnim);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private string RandomParryAttack()
        {
            int idle = UnityEngine.Random.Range(1, 4);

            switch (idle)
            {
                case 1:
                    return m_info.parryHardAnimation.animation;
                case 2:
                    return m_info.parryLightAnimation.animation;
                case 3:
                    return m_info.parryNormalAnimation.animation;
            }
            return null;
        }

        protected override void Start()
        {
            base.Start();
            m_currentTimeScale = UnityEngine.Random.Range(1.0f, 2.0f);
            m_currentFullCD = UnityEngine.Random.Range(m_info.attackCD * .5f, m_info.attackCD * 2f);

            m_hitbox.Disable();
            m_animation.SetAnimation(0, m_info.disassembledIdleAnimation, false);
            //m_spineEventListener.Subscribe(m_info.explodeEvent, m_explodeFX.Play);
            m_startpoint = transform.position;
        }

        protected override void Awake()
        {
            base.Awake();
            GameplaySystem.minionManager.Register(this);
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_damageable.DamageTaken += CounterFlinch;
            m_damageable.DamageTaken += SpawnHitFX;
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();
        }

        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Edge Sensor is " + m_edgeSensor.isDetecting);
            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                        m_movement.Stop();

                    m_stateHandle.Wait(State.ReevaluateSituation);
                    StartCoroutine(DetectRoutine());
                    break;

                case State.Idle:
                    break;

                case State.Patrol:
                    if (m_randomTurnRoutine == null)
                    {
                        m_randomTurnRoutine = StartCoroutine(RandomTurnRoutine());
                    }
                    if (m_randomIdleRoutine == null)
                    {
                        m_randomIdleRoutine = StartCoroutine(RandomIdleRoutine());
                    }
                    if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting)
                    {
                        m_turnState = State.ReevaluateSituation;
                        m_animation.EnableRootMotion(true, false);
                        m_animation.SetAnimation(0, m_info.walk.animation, true);
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    }
                    else
                    {
                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                        {
                            m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        }
                    }
                    break;

                case State.Standby:
                    Patience();
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);

                    m_selfCollider.enabled = false;
                    m_animation.EnableRootMotion(true, false);
                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.Attack:
                            m_attackHandle.ExecuteAttack(m_info.attack.animation, m_info.idleAnimation.animation);
                            break;
                        case Attack.AttackBelow:
                            m_attackHandle.ExecuteAttack(m_info.attackBelow.animation, m_info.idleAnimation.animation);
                            break;
                        case Attack.Block:
                            StartCoroutine(BlockAttackRoutine());
                            break;
                    }
                    m_attackDecider.hasDecidedOnAttack = false;

                    break;

                case State.Cooldown:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
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
                        if (IsFacingTarget())
                        {
                            m_attackDecider.DecideOnAttack();
                            if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range) && !m_wallSensor.allRaysDetecting)
                            {
                                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                                    m_movement.Stop();

                                m_selfCollider.enabled = true;
                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                m_animation.EnableRootMotion(true, false);
                                if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                                {
                                    m_selfCollider.enabled = false;
                                    m_animation.SetAnimation(0, m_info.walk.animation, true).TimeScale = m_currentTimeScale;
                                    //m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.move.speed);
                                }
                                else
                                {
                                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                                        m_movement.Stop();

                                    m_selfCollider.enabled = true;
                                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                }
                            }
                        }
                        else
                        {
                            m_turnState = State.ReevaluateSituation;
                            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
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

                    if (m_patienceRoutine != null /*&& m_targetInfo.isValid*/)
                    {
                        StopCoroutine(m_patienceRoutine);
                        m_patienceRoutine = null;
                    }

                    if (m_sneerRoutine != null /*&& m_targetInfo.isValid*/)
                    {
                        StopCoroutine(m_sneerRoutine);
                        m_sneerRoutine = null;
                    }
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }

            if (m_enablePatience && m_stateHandle.currentState != State.Standby)
            {
                //Patience();
                if (TargetBlocked())
                {
                    m_stateHandle.OverrideState(State.Standby);
                }
            }
        }

        protected override void OnTargetDisappeared()
        {
            if (m_sneerRoutine != null)
            {
                StopCoroutine(m_sneerRoutine);
                m_sneerRoutine = null;
            }
            m_stateHandle.OverrideState(State.Patrol);
            m_currentPatience = 0;
            m_enablePatience = false;
            m_isDetecting = false;
            m_selfCollider.enabled = false;
        }

        public void ResetAI()
        {
            m_selfCollider.enabled = false;
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            m_enablePatience = false;
            if (m_sneerRoutine != null)
            {
                StopCoroutine(m_sneerRoutine);
                m_sneerRoutine = null;
            }
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            enabled = true;
        }

        public override void ReturnToSpawnPoint()
        {
            transform.position = m_startpoint;
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }


        public void HandleKnockback(float resumeAIDelay)
        {
            StopAllCoroutines();
            m_stateHandle.Wait(State.ReevaluateSituation);
            StartCoroutine(KnockbackRoutine(resumeAIDelay));
        }

        private IEnumerator KnockbackRoutine(float timer)
        {
            //enabled = false;
            //m_flinchHandle.m_autoFlinch = false;
            m_animation.DisableRootMotion();
            //m_flinchHandle.enabled = false;
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            yield return new WaitForSeconds(timer);
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                m_movement.Stop();

            //enabled = true;
            //m_flinchHandle.enabled = true;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }
    }
}