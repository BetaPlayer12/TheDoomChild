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
using DChild.Gameplay.Environment;
using UnityEngine.Rendering.Universal;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/SkeletonMiner")]
    public class SkeletonMinerAI : CombatAIBrain<SkeletonMinerAI.Info>, IResetableAIBrain, IKnockbackable
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField, TabGroup("Movement")]
            private MovementInfo m_walkMiner = new MovementInfo();
            public MovementInfo walkMiner => m_walkMiner;
            [SerializeField, TabGroup("Movement")]
            private MovementInfo m_walkNormal = new MovementInfo();
            public MovementInfo walkNormal => m_walkNormal;

            //Attack Behaviours
            [SerializeField, TabGroup("Attack")]
            private SimpleAttackInfo m_attack = new SimpleAttackInfo();
            public SimpleAttackInfo attack => m_attack;
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
            private BasicAnimationInfo m_idleOnGroundAnimation;
            public BasicAnimationInfo idleOnGroundAnimation => m_idleOnGroundAnimation;
            [SerializeField]
            private BasicAnimationInfo m_walkToIdleAnimation;
            public BasicAnimationInfo walkToIdleAnimation => m_walkToIdleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_walkToIdleMiningGroundAnimation;
            public BasicAnimationInfo walkToIdleMiningGroundAnimation => m_walkToIdleMiningGroundAnimation;
            [SerializeField]
            private BasicAnimationInfo m_walkToIdleMiningWallAnimation;
            public BasicAnimationInfo walkToIdleMiningWallAnimation => m_walkToIdleMiningWallAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleToWalkAnimation;
            public BasicAnimationInfo idleToWalkAnimation => m_idleToWalkAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleMiningGroundAnimation;
            public BasicAnimationInfo idleMiningGroundAnimation => m_idleMiningGroundAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleToMiningGroundAnimation;
            public BasicAnimationInfo idleToMiningGroundAnimation => m_idleToMiningGroundAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleMiningWallAnimation;
            public BasicAnimationInfo idleMiningWallAnimation => m_idleMiningWallAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleToMiningWallAnimation;
            public BasicAnimationInfo idleToMiningWallAnimation => m_idleToMiningWallAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleToAttackAnimation;
            public BasicAnimationInfo idleToAttackAnimation => m_idleToAttackAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_resurrectAnimation;
            public BasicAnimationInfo resurrectAnimation => m_resurrectAnimation;
            [SerializeField]
            private BasicAnimationInfo m_detectAnimation;
            public BasicAnimationInfo detectAnimation => m_detectAnimation;
            [SerializeField]
            private BasicAnimationInfo m_detectToWalkAnimation;
            public BasicAnimationInfo detectToWalkAnimation => m_detectToWalkAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation;
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation;
            public BasicAnimationInfo turnAnimation => m_turnAnimation;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_walkMiner.SetData(m_skeletonDataAsset);
                m_walkNormal.SetData(m_skeletonDataAsset);
                m_attack.SetData(m_skeletonDataAsset);

                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_idleOnGroundAnimation.SetData(m_skeletonDataAsset);
                m_walkToIdleAnimation.SetData(m_skeletonDataAsset);
                m_walkToIdleMiningGroundAnimation.SetData(m_skeletonDataAsset);
                m_walkToIdleMiningWallAnimation.SetData(m_skeletonDataAsset);
                m_idleToWalkAnimation.SetData(m_skeletonDataAsset);
                m_idleMiningGroundAnimation.SetData(m_skeletonDataAsset);
                m_idleToMiningGroundAnimation.SetData(m_skeletonDataAsset);
                m_idleMiningWallAnimation.SetData(m_skeletonDataAsset);
                m_idleToMiningWallAnimation.SetData(m_skeletonDataAsset);
                m_idleToAttackAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_resurrectAnimation.SetData(m_skeletonDataAsset);
                m_detectAnimation.SetData(m_skeletonDataAsset);
                m_detectToWalkAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Detect,
            Mining,
            Standby,
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
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private Light2D m_pointLight;
        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Modules")]
        private Health m_health;
        [SerializeField, TabGroup("Modules")]
        private GameObject m_Damager_1;
        [SerializeField, TabGroup("Modules")]
        private GameObject m_Damager_2;

        private float m_currentPatience;
        private float m_currentCD;
        private float m_currentFullCD;
        private float m_currentTimeScale;
        private bool m_enablePatience;
        private bool m_isDetecting;
        private Vector2 m_startpoint;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;

        [SerializeField, Spine.Unity.SpineAnimation(dataField = "m_skeletonAnimation")]
        private string m_miningAnimation;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;

        private State m_turnState;

        private Coroutine m_attackRoutine;
        private Coroutine m_miningRoutine;
        private Coroutine m_sneerRoutine;
        private Coroutine m_patienceRoutine;

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
                //if (/*!TargetBlocked() && */!m_enablePatience)
                //{
                //}
                m_selfCollider.enabled = false;

                if (!m_isDetecting)
                {
                    if (m_miningRoutine != null)
                    {
                        StopCoroutine(m_miningRoutine);
                        m_miningRoutine = null;
                    }
                    m_isDetecting = true;
                    m_stateHandle.OverrideState(State.Detect);
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

        private void CustomTurn()
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
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
            m_stateHandle.SetState(State.Mining);
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();

            //base.OnDestroyed(sender, eventArgs);

            //m_stateHandle.OverrideState(State.WaitBehaviourEnd);

            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);

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
            m_animation.SetAnimation(0, m_info.deathAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathAnimation);
            m_animation.SetAnimation(0, m_info.idleOnGroundAnimation, true);
            yield return new WaitForSeconds(m_info.dismantleDuration);
            m_animation.SetAnimation(0, m_info.resurrectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.resurrectAnimation);
            m_flinchHandle.gameObject.SetActive(true);
            m_health.SetHealthPercentage(1f);
            m_hitbox.Enable();
            enabled = true;
            m_pointLight.enabled = true;
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.OverrideState(m_targetInfo.isValid ? State.Detect : State.Mining);
            yield return null;
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idleAnimation.animation)
            {
                m_flinchHandle.m_autoFlinch = true;
                StopAllCoroutines();
                m_selfCollider.enabled = true;
                //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                m_stateHandle.Wait(State.ReevaluateSituation);
            }
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation)
            //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
            if (m_flinchHandle.m_autoFlinch)
            {
                m_flinchHandle.m_autoFlinch = false;
                m_animation.SetEmptyAnimation(0, 0);
                //m_animation.SetAnimation(0, m_info.idleAnimation, true);
                m_stateHandle.ApplyQueuedState();
            }
        }

        private IEnumerator DetectRoutine()
        {
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_selfCollider.enabled = true;
            m_animation.SetAnimation(0, m_info.detectToWalkAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectToWalkAnimation);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator AttackRoutine()
        {
            m_flinchHandle.m_enableMixFlinch = false;
            m_animation.SetAnimation(0, m_info.idleToAttackAnimation, false);
            var waitTime = m_animation.animationState.GetCurrent(0).AnimationEnd * 0.85f;
            yield return new WaitForSeconds(waitTime);
            m_flinchHandle.m_enableMixFlinch = true;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.idleToAttackAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            //m_attackHandle.ExecuteAttack(m_info.attack.animation, m_info.idleAnimation);
            yield return null;
        }

        private IEnumerator MiningRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idleAnimation.animation)
            {
                var idleToMiningAnim = m_miningAnimation == m_info.idleMiningGroundAnimation.animation ? m_info.idleToMiningGroundAnimation : m_info.idleToMiningWallAnimation;
                m_animation.SetAnimation(0, idleToMiningAnim, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, idleToMiningAnim);
            }
            else if (m_animation.GetCurrentAnimation(0).ToString() == m_info.walkMiner.animation)
            {
                var walkToMiningAnim = m_miningAnimation == m_info.idleMiningGroundAnimation.animation ? m_info.walkToIdleMiningGroundAnimation : m_info.walkToIdleMiningWallAnimation;
                m_animation.SetAnimation(0, walkToMiningAnim, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, walkToMiningAnim);
            }
            m_animation.SetAnimation(0, m_miningAnimation, true);
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

        private IEnumerator ChangeAttackDamageRoutine()
        {

            yield return new WaitForSeconds(1.5f);
            m_Damager_2.SetActive(true);
            m_Damager_1.SetActive(false);
            yield return new WaitForSeconds(1f);
            m_Damager_2.SetActive(false);
            m_Damager_1.SetActive(true);
     
        }
        protected override void Start()
        {
            base.Start();
            m_currentTimeScale = UnityEngine.Random.Range(1.0f, 2.0f);
            m_currentFullCD = UnityEngine.Random.Range(m_info.attackCD * .5f, m_info.attackCD * 2f);
            
            //m_spineEventListener.Subscribe(m_info.explodeEvent, m_explodeFX.Play);
            m_startpoint = transform.position;
        }

        protected override void Awake()
        {
            base.Awake();
            
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_stateHandle = new StateHandle<State>(State.Mining, State.WaitBehaviourEnd);
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
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.Mining:
                    if (IsFacing(m_startpoint))
                    {
                        if (Vector2.Distance(m_startpoint, transform.position) < 5f && !m_wallSensor.allRaysDetecting)
                        {
                            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                                m_movement.Stop();

                            m_selfCollider.enabled = true;
                            m_miningRoutine = StartCoroutine(MiningRoutine());
                        }
                        else
                        {
                            m_animation.EnableRootMotion(true, false);
                            if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                            {
                                m_selfCollider.enabled = false;
                                m_animation.SetAnimation(0, m_info.walkMiner.animation, true).TimeScale = m_currentTimeScale;
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

                    //m_attackRoutine = StartCoroutine(AttackRoutine()); //commented this out in favor of using attack handle to work with timescale 2

                    m_attackHandle.ExecuteAttack(m_info.idleToAttackAnimation.animation, m_info.idleAnimation.animation);
                    StartCoroutine(ChangeAttackDamageRoutine());
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
                            if (IsTargetInRange(m_info.attack.range) && !m_wallSensor.allRaysDetecting)
                            {
                                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                                    m_movement.Stop();

                                m_selfCollider.enabled = true;
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                m_animation.EnableRootMotion(true, false);
                                if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                                {
                                    m_selfCollider.enabled = false;
                                    if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idleAnimation.animation)
                                    {
                                        m_animation.SetAnimation(0, m_info.idleToWalkAnimation, false);
                                    }
                                    m_animation.AddAnimation(0, m_info.walkNormal.animation, true, 0).TimeScale = m_currentTimeScale;
                                    //m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.move.speed);
                                }
                                else
                                {
                                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                                        m_movement.Stop();

                                    m_selfCollider.enabled = true;
                                    if (m_animation.GetCurrentAnimation(0).ToString() == m_info.walkNormal.animation)
                                    {
                                        m_animation.SetAnimation(0, m_info.walkToIdleAnimation, false);
                                    }
                                    m_animation.AddAnimation(0, m_info.idleAnimation.animation, true, 0);
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
                        m_stateHandle.SetState(State.Mining);
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
            m_stateHandle.OverrideState(State.Mining);
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