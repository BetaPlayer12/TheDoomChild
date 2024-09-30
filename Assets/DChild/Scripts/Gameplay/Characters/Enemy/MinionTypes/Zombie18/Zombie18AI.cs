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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/Zombie18")]
    public class Zombie18AI : CombatAIBrain<Zombie18AI.Info>, IResetableAIBrain
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField, TabGroup("Movement")]
            private MovementInfo m_walk = new MovementInfo();
            public MovementInfo walk => m_walk;
            [SerializeField, TabGroup("Movement")]
            private MovementInfo m_run = new MovementInfo();
            public MovementInfo run => m_run;
            [SerializeField, TabGroup("Movement")]
            private MovementInfo m_runWithTongue = new MovementInfo();
            public MovementInfo runWithTongue => m_runWithTongue;

            //Attack Behaviours
            [SerializeField, TabGroup("Attack")]
            private SimpleAttackInfo m_attack1 = new SimpleAttackInfo();
            public SimpleAttackInfo attack1 => m_attack1;
            [SerializeField, TabGroup("Attack")]
            private SimpleAttackInfo m_attack2 = new SimpleAttackInfo();
            public SimpleAttackInfo attack2 => m_attack2;
            [SerializeField, TabGroup("Attack")]
            private SimpleAttackInfo m_attack3 = new SimpleAttackInfo();
            public SimpleAttackInfo attack3 => m_attack3;
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


            //Animations
            [SerializeField]
            private BasicAnimationInfo m_idle1Animation;
            public BasicAnimationInfo idle1Animation => m_idle1Animation;
            [SerializeField]
            private BasicAnimationInfo m_idle2Animation;
            public BasicAnimationInfo idle2Animation => m_idle2Animation;
            [SerializeField]
            private BasicAnimationInfo m_idle3Animation;
            public BasicAnimationInfo idle3Animation => m_idle3Animation;
            [SerializeField]
            private BasicAnimationInfo m_idle4Animation;
            public BasicAnimationInfo idle4Animation => m_idle4Animation;
            [SerializeField]
            private BasicAnimationInfo m_detectAnimation;
            public BasicAnimationInfo detectAnimation => m_detectAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation;
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_turn1Animation;
            public BasicAnimationInfo turn1Animation => m_turn1Animation;
            [SerializeField]
            private BasicAnimationInfo m_turn2Animation;
            public BasicAnimationInfo turn2Animation => m_turn2Animation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_walk.SetData(m_skeletonDataAsset);
                m_run.SetData(m_skeletonDataAsset);
                m_runWithTongue.SetData(m_skeletonDataAsset);
                m_attack1.SetData(m_skeletonDataAsset);
                m_attack2.SetData(m_skeletonDataAsset);
                m_attack3.SetData(m_skeletonDataAsset);

                m_idle1Animation.SetData(m_skeletonDataAsset);
                m_idle2Animation.SetData(m_skeletonDataAsset);
                m_idle3Animation.SetData(m_skeletonDataAsset);
                m_idle4Animation.SetData(m_skeletonDataAsset);
                m_detectAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_turn1Animation.SetData(m_skeletonDataAsset);
                m_turn2Animation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
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
            Attack1,
            Attack2,
            Attack3,
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
        private FlinchHandler m_flinchHandle;

        private float m_currentPatience;
        private float m_currentCD;
        private float m_currentFullCD;
        private float m_currentTimeScale;
        private float m_currentRunAttackDuration;
        private bool m_enablePatience;
        private bool m_isDetecting;
        private BaseInfo.MovementInfo m_run;
        private Vector2 m_startPoint;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;

        [SerializeField]
        private bool m_willPatrol;
        private Vector2 m_patrolDestination;

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
                    m_turnState = State.Standby;
                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turn1Animation.animation && m_animation.GetCurrentAnimation(0).ToString() != m_info.turn2Animation.animation)
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
            
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            if (m_attackRoutine != null)
            {
                StopCoroutine(m_attackRoutine);
            }
            if (m_sneerRoutine != null)
            {
                StopCoroutine(m_sneerRoutine);
            }
            m_animation.SetEmptyAnimation(0, 0);
            m_animation.SetAnimation(0, m_info.deathAnimation, false);
            m_character.physics.UseStepClimb(true);
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idle1Animation.animation
                || m_animation.GetCurrentAnimation(0).ToString() != m_info.idle2Animation.animation
                || m_animation.GetCurrentAnimation(0).ToString() != m_info.idle3Animation.animation
                || m_animation.GetCurrentAnimation(0).ToString() != m_info.idle4Animation.animation)
                m_movement.Stop();

            m_selfCollider.enabled = false;
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idle1Animation.animation
                || m_animation.GetCurrentAnimation(0).ToString() == m_info.idle2Animation.animation
                || m_animation.GetCurrentAnimation(0).ToString() == m_info.idle3Animation.animation
                || m_animation.GetCurrentAnimation(0).ToString() == m_info.idle4Animation.animation)
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
            m_animation.SetAnimation(0, RandomIdleAnimation(), true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
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
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack1, m_info.attack1.range)
                                  , new AttackInfo<Attack>(Attack.Attack2, m_info.attack2.range)
                                  , new AttackInfo<Attack>(Attack.Attack3, m_info.attack3.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {

            m_hitbox.Enable();
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_animation.SetAnimation(0, RandomIdleAnimation(), true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private string RandomIdleAnimation()
        {
            int idle = UnityEngine.Random.Range(1, 5);

            switch (idle)
            {
                case 1:
                    return m_info.idle1Animation.animation;
                case 2:
                    return m_info.idle2Animation.animation;
                case 3:
                    return m_info.idle3Animation.animation;
                case 4:
                    return m_info.idle4Animation.animation;
            }
            return null;
        }

        private BaseInfo.MovementInfo RandomRunMovement()
        {
            int run = UnityEngine.Random.Range(1, 5);

            switch (run)
            {
                case 1:
                    return m_info.run;
                case 2:
                    return m_info.runWithTongue;
            }
            return null;
        }

        private IEnumerator RandomIdleRoutine()
        {
            while (true)
            {
                var timer = UnityEngine.Random.Range(5, 10);
                var currentTimer = 0f;
                var idleAnim = RandomIdleAnimation();
                m_animation.SetAnimation(0, idleAnim, true);
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
            /*
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
            */

            yield return null;
        }

        private IEnumerator SneerRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idle1Animation.animation
                || m_animation.GetCurrentAnimation(0).ToString() != m_info.idle2Animation.animation
                || m_animation.GetCurrentAnimation(0).ToString() != m_info.idle3Animation.animation
                || m_animation.GetCurrentAnimation(0).ToString() != m_info.idle4Animation.animation)
                m_movement.Stop();

            while (true)
            {
                m_animation.SetAnimation(0, m_info.detectAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
                m_animation.SetAnimation(0, RandomIdleAnimation(), true);
                yield return new WaitForSeconds(1f);
                yield return null;
            }
        }

        protected override void Start()
        {
            base.Start();
            m_currentTimeScale = UnityEngine.Random.Range(1.0f, 2.0f);
            m_currentFullCD = UnityEngine.Random.Range(m_info.attackCD * .5f, m_info.attackCD * 2f);

            //m_aggroCollider.enabled = m_willPatrol ? true : false;

            if (!m_willPatrol)
            {
                m_randomTurnRoutine = StartCoroutine(RandomTurnRoutine());
                m_randomIdleRoutine = StartCoroutine(RandomIdleRoutine());
            }

            m_startPoint = transform.position;
            //m_spineEventListener.Subscribe(m_info.explodeEvent, m_explodeFX.Play);
        }

        protected override void Awake()
        {
            base.Awake();
            
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_stateHandle = new StateHandle<State>(m_willPatrol ? State.Patrol : State.Idle, State.WaitBehaviourEnd);
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
                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idle1Animation.animation
                        || m_animation.GetCurrentAnimation(0).ToString() != m_info.idle2Animation.animation
                        || m_animation.GetCurrentAnimation(0).ToString() != m_info.idle3Animation.animation
                        || m_animation.GetCurrentAnimation(0).ToString() != m_info.idle4Animation.animation)
                        m_movement.Stop();

                    //if (!IsFacingTarget())
                    //{
                    //    m_turnState = State.Detect;
                    //    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turn1Animation.animation && m_animation.GetCurrentAnimation(0).ToString() != m_info.turn2Animation.animation)
                    //        m_stateHandle.SetState(State.Turning);
                    //}
                    //else
                    //{
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        StartCoroutine(DetectRoutine());
                    //}
                    break;

                case State.Idle:
                    break;

                case State.Patrol:
                    //if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting)
                    //{
                    //    m_turnState = State.ReevaluateSituation;
                    //    m_animation.EnableRootMotion(true, false);
                    //    m_animation.SetAnimation(0, m_info.walk.animation, true);
                    //    var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                    //}
                    //else
                    //{
                    //    if (m_animation.animationState.GetCurrent(0).IsComplete)
                    //    {
                    //        m_animation.SetAnimation(0, RandomIdleAnimation(), true);
                    //    }
                    //}
                    /*if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting)
                    {
                        if (IsFacing(m_patrolDestination) && m_edgeSensor.allRaysDetecting)
                        {
                            m_turnState = State.ReevaluateSituation;
                            m_animation.EnableRootMotion(true, false);
                            m_animation.SetAnimation(0, m_info.walk.animation, true);
                            var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        }
                        else
                        {
                            m_turnState = State.Patrol;
                            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turn1Animation.animation && m_animation.GetCurrentAnimation(0).ToString() != m_info.turn2Animation.animation)
                                m_stateHandle.SetState(State.Turning);
                        }
                    }
                    else
                    {*/
                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                        {
                            m_animation.SetAnimation(0, RandomIdleAnimation(), true);
                        }
                    //}
                    break;

                case State.Standby:
                    Patience();
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    var turnAnim = UnityEngine.Random.Range(0, 2) == 1 ? m_info.turn1Animation.animation : m_info.turn2Animation.animation;
                    m_turnHandle.Execute(turnAnim, RandomIdleAnimation());
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    
                    m_selfCollider.enabled = false;
                    m_animation.EnableRootMotion(true, false);
                    var idleAnim = RandomIdleAnimation();
                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.Attack1:
                            m_attackHandle.ExecuteAttack(m_info.attack1.animation, idleAnim);
                            break;
                        case Attack.Attack2:
                            m_attackHandle.ExecuteAttack(m_info.attack2.animation, idleAnim);
                            break;
                        case Attack.Attack3:
                            m_attackHandle.ExecuteAttack(m_info.attack3.animation, idleAnim);
                            break;
                    }
                    m_attackDecider.hasDecidedOnAttack = false;

                    break;

                case State.Cooldown:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    if (!IsFacingTarget())
                    {
                        OnTargetDisappeared();
                        m_turnState = State.ReevaluateSituation;
                        //m_turnState = State.Cooldown;
                        //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turn1Animation.animation && m_animation.GetCurrentAnimation(0).ToString() != m_info.turn2Animation.animation)
                        //    m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                        {
                            m_animation.SetAnimation(0, RandomIdleAnimation(), true);
                        }
                        m_turnState = State.Cooldown;
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
                                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idle1Animation.animation
                                    || m_animation.GetCurrentAnimation(0).ToString() != m_info.idle2Animation.animation
                                    || m_animation.GetCurrentAnimation(0).ToString() != m_info.idle3Animation.animation
                                    || m_animation.GetCurrentAnimation(0).ToString() != m_info.idle4Animation.animation)
                                    m_movement.Stop();

                                m_selfCollider.enabled = true;
                                m_run = null;
                                m_animation.SetAnimation(0, RandomIdleAnimation(), true);
                                m_stateHandle.SetState(State.Attacking);
                            }/*
                            else
                            {
                                if (m_run == null)
                                {
                                    m_run = RandomRunMovement();
                                }
                                m_animation.EnableRootMotion(true, false);
                                if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                                {
                                    m_selfCollider.enabled = false;
                                    m_animation.SetAnimation(0, m_run.animation, true).TimeScale = m_currentTimeScale;
                                    //m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.move.speed);
                                }
                                else
                                {
                                    m_run = null;
                                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idle1Animation.animation
                                        || m_animation.GetCurrentAnimation(0).ToString() != m_info.idle2Animation.animation
                                        || m_animation.GetCurrentAnimation(0).ToString() != m_info.idle3Animation.animation
                                        || m_animation.GetCurrentAnimation(0).ToString() != m_info.idle4Animation.animation)
                                    {
                                        m_animation.SetAnimation(0, m_info.idle1Animation, true);
                                        m_movement.Stop();
                                    }

                                    m_selfCollider.enabled = true;
                                    if (m_animation.animationState.GetCurrent(0).IsComplete)
                                    {
                                        m_animation.SetAnimation(0, RandomIdleAnimation(), true);
                                    }
                                }
                            }*/
                        }
                        else
                        {
                            OnTargetDisappeared();
                            m_turnState = State.ReevaluateSituation;
                            //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turn1Animation.animation && m_animation.GetCurrentAnimation(0).ToString() != m_info.turn2Animation.animation)
                            //    m_stateHandle.SetState(State.Turning);
                        }
                    }
                    break;

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    if (m_targetInfo.isValid)
                    {
                        /*if(IsFacing(m_targetInfo.position))
                        {
                            m_stateHandle.SetState(State.Patrol);
                            OnTargetDisappeared();
                        }else
                        {*/
                            m_stateHandle.SetState(State.Chasing);
                        //}
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

            if (m_targetInfo.isValid)
            {
                if (m_enablePatience && m_stateHandle.currentState != State.Standby && m_stateHandle.currentState != State.Turning)
                {
                    //Patience();
                    if (TargetBlocked())
                    {
                        m_stateHandle.OverrideState(State.Standby);
                    }
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
            transform.position = m_startPoint;
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }
    }
}
