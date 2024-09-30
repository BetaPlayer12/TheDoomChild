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
using PixelCrushers.DialogueSystem;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/ZombieMushroom")]
    public class ZombieMushroomAI : CombatAIBrain<ZombieMushroomAI.Info>, IResetableAIBrain
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
            [SerializeField, TabGroup("Attack")]
            private SimpleAttackInfo m_meleeAttack = new SimpleAttackInfo();
            public SimpleAttackInfo meleeAttack => m_meleeAttack;
            [SerializeField, TabGroup("Attack")]
            private SimpleAttackInfo m_chargeAttack = new SimpleAttackInfo();
            public SimpleAttackInfo chargeAttack => m_chargeAttack;
            [SerializeField, MinValue(0), TabGroup("Attack")]
            private float m_chargeAttackDuration;
            public float chargeAttackDuration => m_chargeAttackDuration;
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
            private float m_stuckInWallTime;
            public float stuckInWallTime => m_stuckInWallTime;
            [SerializeField]
            private float m_chargeAirTime;
            public float chargeAirTime => m_chargeAirTime;

            //Animations
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_detectAnimation;
            public BasicAnimationInfo detectAnimation => m_detectAnimation;
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
            private BasicAnimationInfo m_chargeAttackAnimation;
            public BasicAnimationInfo chargeAttackAnimation => m_chargeAttackAnimation;
            [SerializeField]
            private BasicAnimationInfo m_chargeImpactStickAnimation;
            public BasicAnimationInfo chargeImpactStickAnimation => m_chargeImpactStickAnimation;
            [SerializeField]
            private BasicAnimationInfo m_stuckLoopAnimation;
            public BasicAnimationInfo stuckLoopAnimation => m_stuckLoopAnimation;
            [SerializeField]
            private BasicAnimationInfo m_unstickAnimation;
            public BasicAnimationInfo unstickAnimation => m_unstickAnimation;
            [SerializeField]
            private BasicAnimationInfo m_chargeToThrustAnimation;
            public BasicAnimationInfo chargeToThrustAnimation => m_chargeToThrustAnimation;


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_meleeAttack.SetData(m_skeletonDataAsset);
                m_chargeAttack.SetData(m_skeletonDataAsset);

                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_detectAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_chargeAttackAnimation.SetData(m_skeletonDataAsset);
                m_chargeImpactStickAnimation.SetData(m_skeletonDataAsset);
                m_unstickAnimation.SetData(m_skeletonDataAsset);
                m_stuckLoopAnimation.SetData(m_skeletonDataAsset);
                m_chargeToThrustAnimation.SetData(m_skeletonDataAsset);
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
            Flinch,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            AttackMelee,
            AttackCharge,
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
        private Collider2D m_aggroCollider;
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
        private float m_currentRunAttackDuration;
        private bool m_enablePatience;
        private bool m_isDetecting;
        private Vector2 m_startPoint;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_playerSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;

        [SerializeField]
        private bool m_willPatrol;

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
                //if (m_stateHandle.currentState != State.Chasing 
                //    && m_stateHandle.currentState != State.RunAway 
                //    && m_stateHandle.currentState != State.Turning 
                //    && m_stateHandle.currentState != State.WaitBehaviourEnd)
                //{
                //}
                m_selfCollider.enabled = false;

                if (!m_isDetecting)
                {
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
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                m_movement.Stop();

            m_selfCollider.enabled = false;
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
                m_stateHandle.ApplyQueuedState();
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
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.AttackMelee, m_info.meleeAttack.range), new AttackInfo<Attack>(Attack.AttackCharge, m_info.chargeAttack.range)/**/);
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {

            m_hitbox.Enable();
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator AttackRoutine()
        {
            m_selfCollider.enabled = false;
            m_animation.SetAnimation(0, m_info.meleeAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.meleeAttack.animation);
            m_selfCollider.enabled = true;
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator ChargeAttackRoutine()
        {
            m_animation.DisableRootMotion();
            //m_animation.EnableRootMotion(true, false);
            m_selfCollider.enabled = false;

            //if (m_groundSensor && m_edgeSensor.isDetecting)
            //{

            //}
            m_animation.SetAnimation(0, m_info.chargeToThrustAnimation.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.chargeToThrustAnimation.animation);

            //while (!m_playerSensor.isDetecting && !m_wallSensor.isDetecting)
            //{
            //    m_animation.SetAnimation(0, m_info.move.animation, true)/*.TimeScale = m_currentTimeScale*/;
            //    m_movement.MoveTowards(Vector2.right * transform.localScale.x, m_intafo.move.speed);
            //    yield return null;
            //}
            var direction = m_character.facing == HorizontalDirection.Right ? Vector2.right : Vector2.left;
            var playerPositionBackOffset = direction * 60;

            var distanceToPlayer = Mathf.Abs(transform.position.x - (m_targetInfo.position.x + playerPositionBackOffset.x));
            Debug.Log("Initial Distance: " + distanceToPlayer);
            m_animation.SetAnimation(0, m_info.chargeAttack.animation, true);
            while (distanceToPlayer >= 7f && !m_wallSensor.isDetecting)
            {
                distanceToPlayer = Mathf.Abs(transform.position.x - (m_targetInfo.position.x + playerPositionBackOffset.x));
                Debug.Log(distanceToPlayer);
                m_movement.MoveTowards(direction, 50);
                yield return null;
            }

            //m_animation.SetAnimation(0, m_info.chargeAttack.animation, true);
            //yield return new WaitForSeconds(m_info.chargeAirTime);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.chargeAttack.animation);

            m_movement.Stop();

            if (m_wallSensor.isDetecting)
            {
                
                Debug.Log("Wall Detected");

                yield return StuckRoutine();
            }

            m_selfCollider.enabled = true;
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator RandomIdleRoutine()
        {
            var timer = UnityEngine.Random.Range(5, 10);
            var currentTimer = 0f;
            while (currentTimer < timer)
            {
                currentTimer += Time.deltaTime;
                yield return null;
            }
            m_stateHandle.Wait(State.Patrol);
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                m_movement.Stop();

            m_animation.SetAnimation(0, m_info.idleAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.idleAnimation);
            m_stateHandle.ApplyQueuedState();
            yield return null;
            StartCoroutine(RandomIdleRoutine());
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
                m_animation.SetAnimation(0, m_info.detectAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
                //m_animation.SetAnimation(0, m_info.rawrAnimation, false);
                //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rawrAnimation);

                //yield return new WaitForSeconds(3f);
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
                m_hitbox.Disable();
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
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
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
                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                        m_movement.Stop();

                    if (!IsFacingTarget() && m_willPatrol)
                    {
                        m_turnState = State.Detect;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        StartCoroutine(DetectRoutine());
                    }
                    break;

                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                        m_movement.Stop();

                    break;

                case State.Patrol:
                    if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting)
                    {
                        m_turnState = State.ReevaluateSituation;
                        m_animation.EnableRootMotion(true, false);
                        m_animation.SetAnimation(0, m_info.patrol.animation, true)/*.TimeScale = 0.5f*/;
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        m_patrolHandle.Patrol(m_movement, m_info.patrol.speed, characterInfo);
                    }
                    else
                    {
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                            m_movement.Stop();

                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
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
                    m_animation.EnableRootMotion(true, false);

                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.AttackMelee:
                            m_attackRoutine = StartCoroutine(AttackRoutine());
                            break;
                        case Attack.AttackCharge:
                            m_attackRoutine = StartCoroutine(ChargeAttackRoutine());
                            break;
                        default:
                            m_attackRoutine = StartCoroutine(AttackRoutine());
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
                        //if (m_animation.animationState.GetCurrent(0).IsComplete)
                        //{
                        //}
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

                //case State.Chasing:
                //    {
                //        //if (IsFacingTarget())
                //        //{
                //        //    m_attackDecider.DecideOnAttack();
                //        //    if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(/*m_attackDecider.chosenAttack.range*/ m_info.meleeAttackRange) && !m_wallSensor.allRaysDetecting)
                //        //    {
                //        //        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                //        //            m_movement.Stop();

                //        //        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                //        //        m_stateHandle.SetState(State.Attacking);
                //        //    }
                //        //    else
                //        //    {
                //        //        m_animation.EnableRootMotion(true, false);
                //        //        if (m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                //        //        {
                //        //            m_selfCollider.enabled = false;
                //        //            m_animation.SetAnimation(0, m_info.move.animation, true)/*.TimeScale = m_currentTimeScale*/;

                //        //            m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.move.speed);
                //        //        }
                //        //        else
                //        //        {
                //        //            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                //        //                m_movement.Stop();

                //        //            m_selfCollider.enabled = true;
                //        //            if (m_animation.animationState.GetCurrent(0).IsComplete)
                //        //            {
                //        //                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                //        //            }
                //        //        }
                //        //    }
                //        //}
                //        //else
                //        //{
                //        //    m_turnState = State.ReevaluateSituation;
                //        //    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                //        //        m_stateHandle.SetState(State.Turning);
                //        //}
                        
                //    }
                //    break;

                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    if (m_targetInfo.isValid)
                    {
                        //change chasing with a check for what attack it should do based on player distance (chase behaviour becomes part of charge attack)
                        //m_stateHandle.SetState(State.Chasing);
                        if (IsFacingTarget())
                        {
                            if (IsTargetInRange(m_info.meleeAttack.range))
                            {
                                m_attackDecider.ForcedDecideOnAttack(0);
                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                m_attackDecider.ForcedDecideOnAttack(1);
                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                m_stateHandle.SetState(State.Attacking);
                            }
                        }
                        else
                        {
                            m_turnState = State.ReevaluateSituation;
                            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                                m_stateHandle.SetState(State.Turning);
                        }
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

        //move this to be done in charge attack instead of stuck state
        private IEnumerator StuckRoutine()
        {
            m_animation.SetAnimation(0, m_info.chargeImpactStickAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.chargeImpactStickAnimation);


            m_animation.SetAnimation(0, m_info.stuckLoopAnimation, true);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.stuckLoopAnimation);
            yield return new WaitForSeconds(m_info.stuckInWallTime);

            m_animation.SetAnimation(0, m_info.unstickAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.unstickAnimation);
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
