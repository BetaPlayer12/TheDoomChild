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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/Zombie13")]
    public class Zombie13AI : CombatAIBrain<Zombie13AI.Info>, IResetableAIBrain
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField, TitleGroup("Movement")]
            private MovementInfo m_walk = new MovementInfo();
            public MovementInfo walk => m_walk;

            //Attack Behaviours
            [SerializeField, TitleGroup("Combat")]
            private SimpleAttackInfo m_attack = new SimpleAttackInfo();
            public SimpleAttackInfo attack => m_attack;
            [SerializeField, MinValue(0), TitleGroup("Combat")]
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
            private string m_idle1Animation;
            public string idle1Animation => m_idle1Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idle2Animation;
            public string idle2Animation => m_idle2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idle3Animation;
            public string idle3Animation => m_idle3Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_detectAnimation;
            public string detectAnimation => m_detectAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchFrontAnimation;
            public string flinchFrontAnimation => m_flinchFrontAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchBack1Animation;
            public string flinchBack1Animation => m_flinchBack1Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchBack2Animation;
            public string flinchBack2Animation => m_flinchBack2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turn1Animation;
            public string turn1Animation => m_turn1Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turn2Animation;
            public string turn2Animation => m_turn2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_walk.SetData(m_skeletonDataAsset);
                m_attack.SetData(m_skeletonDataAsset);
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

        //private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

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
                    m_turnState = State.WaitBehaviourEnd;
                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turn1Animation && m_animation.GetCurrentAnimation(0).ToString() != m_info.turn2Animation)
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
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idle1Animation
                || m_animation.GetCurrentAnimation(0).ToString() != m_info.idle2Animation
                || m_animation.GetCurrentAnimation(0).ToString() != m_info.idle3Animation)
                m_movement.Stop();

            m_selfCollider.enabled = false;
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idle1Animation
                || m_animation.GetCurrentAnimation(0).ToString() == m_info.idle2Animation
                || m_animation.GetCurrentAnimation(0).ToString() == m_info.idle3Animation)
            {
                StopAllCoroutines();
                //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                m_stateHandle.Wait(State.ReevaluateSituation);
                StartCoroutine(FlinchRoutine());
            }
        }

        private IEnumerator FlinchRoutine()
        {
            if (m_targetInfo.isValid)
            {
                var flinchBackAnim = UnityEngine.Random.Range(0, 2) == 1 ? m_info.flinchBack1Animation : m_info.flinchBack2Animation;
                var flinchAnim = IsFacingTarget() ? m_info.flinchFrontAnimation : flinchBackAnim;
                m_animation.SetAnimation(0, flinchAnim, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_targetInfo.isValid ? flinchAnim : m_info.flinchFrontAnimation);
            }
            else
            {
                m_animation.SetAnimation(0, m_info.flinchFrontAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchFrontAnimation);
            }
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
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack, m_info.attack.range));
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
            int idle = UnityEngine.Random.Range(1, 4);

            switch (idle)
            {
                case 1:
                    return m_info.idle1Animation;
                case 2:
                    return m_info.idle2Animation;
                case 3:
                    return m_info.idle3Animation;
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
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idle1Animation
                || m_animation.GetCurrentAnimation(0).ToString() != m_info.idle2Animation
                || m_animation.GetCurrentAnimation(0).ToString() != m_info.idle3Animation)
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

        private IEnumerator AttackRoutine()
        {
            //m_hitbox.gameObject.SetActive(false);
            m_selfCollider.enabled = false;
            m_animation.SetAnimation(0, m_info.attack.animation, false);
            var waitTime = m_animation.animationState.GetCurrent(0).AnimationEnd * .5f;
            yield return new WaitForSeconds(waitTime);
            //m_hitbox.gameObject.SetActive(true);
            GetComponent<IsolatedCharacterPhysics2D>().UseStepClimb(true);
            m_animation.EnableRootMotion(true, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack.animation);
            GetComponent<IsolatedCharacterPhysics2D>().UseStepClimb(false);
            m_animation.SetAnimation(0, RandomIdleAnimation(), true);
            m_selfCollider.enabled = true;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator TurnRoutine(string currentAnimation)
        {
            if (currentAnimation == m_info.attack.animation)
                yield return new WaitForAnimationComplete(m_animation.animationState, currentAnimation);
            var turnAnim = UnityEngine.Random.Range(0, 2) == 1 ? m_info.turn1Animation : m_info.turn2Animation;
            m_turnHandle.Execute(turnAnim, RandomIdleAnimation());
            yield return null;
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
            m_patrolDestination = m_patrolHandle.wayPoints[m_patrolHandle.startIndex];
            if (!IsFacing(m_patrolDestination))
                CustomTurn();
            //m_spineEventListener.Subscribe(m_info.explodeEvent, m_explodeFX.Play);
            GetComponent<IsolatedCharacterPhysics2D>().UseStepClimb(false);
        }

        protected override void Awake()
        {
            base.Awake();
            //m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
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
                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idle1Animation
                        || m_animation.GetCurrentAnimation(0).ToString() != m_info.idle2Animation
                        || m_animation.GetCurrentAnimation(0).ToString() != m_info.idle3Animation)
                        m_movement.Stop();

                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Detect;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turn1Animation && m_animation.GetCurrentAnimation(0).ToString() != m_info.turn2Animation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        StartCoroutine(DetectRoutine());
                    }
                    break;

                case State.Idle:
                    break;

                case State.Patrol:
                    if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting)
                    {
                        if (IsFacing(m_patrolDestination))
                        {
                            m_turnState = State.Patrol;
                            m_animation.EnableRootMotion(true, false);
                            m_animation.SetAnimation(0, m_info.attack.animation, true);
                            var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        }
                        else
                        {
                            m_turnState = State.Patrol;
                            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turn1Animation && m_animation.GetCurrentAnimation(0).ToString() != m_info.turn2Animation)
                                m_stateHandle.SetState(State.Turning);
                        }
                    }
                    else
                    {
                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                        {
                            m_animation.SetAnimation(0, RandomIdleAnimation(), true);
                        }
                    }
                    break;

                case State.Standby:
                    Patience();
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    StartCoroutine(TurnRoutine(m_animation.GetCurrentAnimation(0)));
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);


                    m_animation.EnableRootMotion(true, false);
                    var idleAnim = RandomIdleAnimation();
                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.Attack:
                            StartCoroutine(AttackRoutine());
                            break;
                    }
                    m_attackDecider.hasDecidedOnAttack = false;

                    break;

                case State.Cooldown:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turn1Animation && m_animation.GetCurrentAnimation(0).ToString() != m_info.turn2Animation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                        {
                            m_animation.SetAnimation(0, RandomIdleAnimation(), true);
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
                        if (IsFacingTarget())
                        {
                            m_attackDecider.DecideOnAttack();
                            if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range) && !m_wallSensor.allRaysDetecting)
                            {
                                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idle1Animation
                                    || m_animation.GetCurrentAnimation(0).ToString() != m_info.idle2Animation
                                    || m_animation.GetCurrentAnimation(0).ToString() != m_info.idle3Animation)
                                    m_movement.Stop();

                                m_animation.SetAnimation(0, RandomIdleAnimation(), true);
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
                                    if (m_wallSensor.allRaysDetecting)
                                    {
                                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idle1Animation
                                            || m_animation.GetCurrentAnimation(0).ToString() != m_info.idle2Animation
                                            || m_animation.GetCurrentAnimation(0).ToString() != m_info.idle3Animation)
                                            m_movement.Stop();

                                        m_selfCollider.enabled = true;
                                        GetComponent<IsolatedCharacterPhysics2D>().UseStepClimb(false);
                                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                                        {
                                            m_animation.SetAnimation(0, RandomIdleAnimation(), true);
                                        }
                                    }
                                    else if (m_wallSensor.isDetecting)
                                    {
                                        m_selfCollider.enabled = false;
                                        GetComponent<IsolatedCharacterPhysics2D>().UseStepClimb(true);
                                        m_animation.SetAnimation(0, m_info.attack.animation, true);
                                    }
                                }
                            }
                        }
                        else
                        {
                            m_turnState = State.ReevaluateSituation;
                            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turn1Animation && m_animation.GetCurrentAnimation(0).ToString() != m_info.turn2Animation)
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
            transform.position = m_startPoint;
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }
    }
}
