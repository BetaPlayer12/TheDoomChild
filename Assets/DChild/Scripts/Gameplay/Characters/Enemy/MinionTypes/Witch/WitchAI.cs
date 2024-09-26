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

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/Witch")]
    public class WitchAI : CombatAIBrain<WitchAI.Info>, IResetableAIBrain
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;


            //Attack Behaviours
            [SerializeField, TabGroup("Attack")]
            private SimpleAttackInfo m_attack = new SimpleAttackInfo();
            public SimpleAttackInfo attack => m_attack;
            [SerializeField, MinValue(0), TabGroup("Attack")]
            private float m_attackCD;
            public float attackCD => m_attackCD;
            //
            [SerializeField, MinValue(0)]
            private float m_patience;
            public float patience => m_patience;

            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;
            [SerializeField]
            private float m_runBackAndForthDistance;
            public float runBackAndForthDistance => m_runBackAndForthDistance;
            [SerializeField]
            private int m_timesToRunBackAndForth;
            public int timesToRunBackAndForth => m_timesToRunBackAndForth;
            [SerializeField]
            private float m_wallOffset;
            public float wallOffset => m_wallOffset;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_onShriekEvent;

            public string onShriekEvent => m_onShriekEvent;

            //Animations
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleSittingAnimation;
            public BasicAnimationInfo idleSittingAnimation => m_idleSittingAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleStandingAnimation;
            public BasicAnimationInfo idleStandingAnimation => m_idleStandingAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_detectSittingAnimation;
            public BasicAnimationInfo detectSittingAnimation => m_detectSittingAnimation;
            [SerializeField]
            private BasicAnimationInfo m_detectStandingAnimation;
            public BasicAnimationInfo detectStandingAnimation => m_detectStandingAnimation;
            [SerializeField]
            private BasicAnimationInfo m_dodgeAnimation;
            public BasicAnimationInfo dodgeAnimation => m_dodgeAnimation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation;
            public BasicAnimationInfo turnAnimation => m_turnAnimation;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_move.SetData(m_skeletonDataAsset);
                m_attack.SetData(m_skeletonDataAsset);

                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_idleSittingAnimation.SetData(m_skeletonDataAsset);
                m_idleStandingAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_detectSittingAnimation.SetData(m_skeletonDataAsset);
                m_detectStandingAnimation.SetData(m_skeletonDataAsset); 
                m_dodgeAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Detect,
            Idle,
            Buang,
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
            RunAttack,
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
        private WayPointPatrol m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private List<FlinchHandler> m_flinchHandles;
        private int m_flinchID;

        private float m_currentPatience;
        private float m_currentCD;
        private float m_currentFullCD;
        private float m_currentTimeScale;
        private float m_currentRunAttackDuration;
        private bool m_enablePatience;
        private bool m_isDetecting;
        private Vector2 m_startPoint;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;
        [SerializeField, TabGroup("Hurtbox")]
        private Collider2D m_attackBB;
        [SerializeField, TabGroup("Particle")]
        private ParticleSystem m_shriekFX;

        [SerializeField]
        private bool m_willStand;

        [SerializeField, ReadOnly]
        private float m_leftPoint;
        [SerializeField, ReadOnly]
        private float m_rightPoint;
        [SerializeField, ReadOnly]
        private int m_moveCount = 0;
        [SerializeField, ReadOnly]
        private int m_timesToRunBackAndForth = 0;

        [SerializeField, ReadOnly]
        private bool m_runBackAndForthPointsDecided = false;
        private Vector2 m_patrolDestination;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private State m_turnState;

        private Coroutine m_attackRoutine;
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

                m_selfCollider.enabled = false;

                if (!m_isDetecting)
                {
                    m_isDetecting = true;
                    m_stateHandle.SetState(State.Detect);
                }
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
            var wayPoints = m_patrolHandle.GetWaypoints();
            for (int i = 0; i < wayPoints.Length; i++)
            {
                if (Vector2.Distance(m_patrolDestination, wayPoints[i]) > 1f)
                {
                    m_patrolDestination = wayPoints[i];
                    break;
                }
            }
            m_stateHandle.ApplyQueuedState();
        }

        private void ResetBuangVariables()
        {
            if (m_runBackAndForthPointsDecided)
            {
                m_runBackAndForthPointsDecided = false;
                m_leftPoint = 0;
                m_rightPoint = 0;
                m_moveCount = 0;
            }
            
        }

        private void SetRunBackAndForthPoints()
        {
            if (!m_runBackAndForthPointsDecided)
            {
                m_leftPoint = transform.position.x - m_info.runBackAndForthDistance;
                m_rightPoint = transform.position.x + m_info.runBackAndForthDistance;

                if (m_wallSensor.isDetecting || !m_edgeSensor.isDetecting)
                {
                    if(transform.localScale.x < 1) //less than one is facing left so wall/edge sensor is facing left
                    {
                        m_leftPoint = transform.position.x + m_info.wallOffset;
                    }
                    else
                    {
                        m_rightPoint = transform.position.x - m_info.wallOffset;
                    }
                }

                Vector2[] RunPoints = new Vector2[2];
                RunPoints[0] = new Vector2(m_leftPoint, m_centerMass.position.y);
                RunPoints[1] = new Vector2(m_rightPoint, m_centerMass.position.y);

                m_patrolHandle.SetWayPoints(RunPoints);

                m_runBackAndForthPointsDecided = true;
            }
         
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
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
            for (int i = 0; i < m_flinchHandles.Count; i++)
            {
                m_flinchHandles[i].gameObject.SetActive(false);
            }
            m_hitbox.Disable();
            m_animation.DisableRootMotion();
            m_animation.SetEmptyAnimation(0, 0);
            m_animation.SetAnimation(0, m_info.deathAnimation, false);
            m_character.physics.UseStepClimb(true);
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation || m_animation.GetCurrentAnimation(0).ToString() != m_info.idleSittingAnimation.animation)
                m_movement.Stop();

            m_selfCollider.enabled = false;
            m_attackBB.enabled = false;
        }

        private void OnDamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
        {
            if (m_targetInfo.isValid)
            {
                switch (IsFacingTarget())
                {
                    case true:
                        m_flinchID = 0;
                        break;
                    case false:
                        m_flinchID = 1;
                        break;
                }
            }
            else
            {
                m_flinchID = UnityEngine.Random.Range(0, m_flinchHandles.Count);
            }
            m_flinchHandles[m_flinchID].gameObject.SetActive(true);
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idleAnimation.animation || m_animation.GetCurrentAnimation(0).ToString() == m_info.idleSittingAnimation.animation)
            {
                StopAllCoroutines();
                m_selfCollider.enabled = false;
                //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                m_stateHandle.Wait(State.ReevaluateSituation);
                m_flinchHandles[m_flinchID].Flinch();
            }
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            m_flinchHandles[m_flinchID].gameObject.SetActive(false);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
        }

        public override void ApplyData()
        {
            base.ApplyData();
            if (m_attackDecider != null)
            {
                UpdateAttackDeciderList();
            }
        }
        public void OnShriekVFX()
        {
            m_shriekFX.Play();
        }

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.RunAttack, m_info.attack.range)/**/);
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {
            if(!m_willStand)
            {
                m_willStand = true;
            }
            m_hitbox.Enable();

            IAIAnimationInfo detectAnimation = m_willStand ? m_info.detectStandingAnimation : m_info.detectSittingAnimation;
            m_animation.SetAnimation(0, detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, detectAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator SneerRoutine()
        {
            IAIAnimationInfo detectAnimation = m_willStand ? m_info.detectSittingAnimation : m_info.detectStandingAnimation;
            m_stateHandle.Wait(State.ReevaluateSituation);
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                m_movement.Stop();

            while (true)
            {
                m_animation.SetAnimation(0, detectAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, detectAnimation);
                
                
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                yield return new WaitForSeconds(1f);
                yield return null;
            }
        }
        
        protected override void Start()
        {
            base.Start();
            m_currentTimeScale = UnityEngine.Random.Range(1.0f, 2.0f);
            m_currentFullCD = UnityEngine.Random.Range(m_info.attackCD * .5f, m_info.attackCD * 2f);
            m_spineEventListener.Subscribe(m_info.onShriekEvent, OnShriekVFX);
            IAIAnimationInfo idleAnimation = m_willStand ? m_info.idleStandingAnimation : m_info.idleSittingAnimation;
            m_animation.SetAnimation(0, idleAnimation, true);

            m_startPoint = transform.position;
            m_timesToRunBackAndForth = m_info.timesToRunBackAndForth * 2;
            //m_spineEventListener.Subscribe(m_info.explodeEvent, m_explodeFX.Play);
        }

        protected override void Awake()
        {
            base.Awake();
            
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            m_damageable.DamageTaken += OnDamageTaken;
            m_patrolHandle.DestinationReached += OnDestinationReached;
            for (int i = 0; i < m_flinchHandles.Count; i++)
            {
                m_flinchHandles[i].FlinchEnd += OnFlinchEnd;
            }
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();
        }

        private void OnDestinationReached(object sender, EventActionArgs eventArgs)
        {
            m_moveCount++;
        }

        private void Update()
        {
            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation || m_animation.GetCurrentAnimation(0).ToString() != m_info.idleSittingAnimation.animation)
                        m_movement.Stop();

                    m_stateHandle.Wait(State.ReevaluateSituation);
                    StartCoroutine(DetectRoutine());
                    break;

                case State.Idle:
                    ResetBuangVariables();
                    IAIAnimationInfo idleAnimation = m_willStand ? m_info.idleStandingAnimation : m_info.idleSittingAnimation;
                    m_animation.SetAnimation(0, idleAnimation, true);

                    break;      

                case State.Buang:
                    SetRunBackAndForthPoints();

                    if(m_moveCount <= m_timesToRunBackAndForth)
                    {
                        if (m_groundSensor.isDetecting)
                        {
                            m_turnState = State.ReevaluateSituation;
                            m_animation.EnableRootMotion(true, false);
                            m_animation.SetAnimation(0, m_info.move.animation, true)/*.TimeScale = 0.5f*/;
                            var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                            m_patrolHandle.Patrol(m_movement, m_info.move.speed, characterInfo);

                            if (m_wallSensor.isDetecting)
                            {
                                ResetBuangVariables();
                                SetRunBackAndForthPoints();
                            }
                        }
                        else
                        {
                            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                                m_movement.Stop();

                            m_isDetecting = false;
                            m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        }
                    }
                    else
                    {
                        m_stateHandle.OverrideState(State.Idle);
                    }                   

                    //Patience();
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);

                    m_animation.EnableRootMotion(true, false);
                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.RunAttack:
                            m_attackHandle.ExecuteAttack(m_info.attack.animation, m_info.idleAnimation.animation);
                            break;
                    }
                    m_attackDecider.hasDecidedOnAttack = false;

                    break;

                case State.Cooldown:
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                        {
                            m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        }
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
                        if (IsFacingTarget() && !TargetBlocked())
                        {
                            ResetBuangVariables();
                            m_attackDecider.DecideOnAttack();
                            if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range))
                            {
                                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                                    m_movement.Stop();

                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                m_animation.EnableRootMotion(true, false);
                                if (!m_wallSensor.allRaysDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                                {
                                    m_selfCollider.enabled = false;
                                    m_animation.SetAnimation(0, m_info.move.animation, true).TimeScale = m_currentTimeScale;
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

                        if (TargetBlocked())
                        {
                            m_stateHandle.SetState(State.Buang);
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
                        m_stateHandle.SetState(State.Idle);
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

            //if (m_targetInfo.isValid)
            //{
            //    if (m_enablePatience && m_stateHandle.currentState != State.Standby && m_stateHandle.currentState != State.Turning)
            //    {
            //        //Patience();
            //        if (TargetBlocked())
            //        {
            //            m_stateHandle.OverrideState(State.Standby);
            //        }
            //    }
            //}
        }

        protected override void OnTargetDisappeared()
        {
            if (m_sneerRoutine != null)
            {
                StopCoroutine(m_sneerRoutine);
                m_sneerRoutine = null;
            }
            m_stateHandle.OverrideState(State.Buang);
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
            transform.position = m_startPoint;
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }
    }
}
