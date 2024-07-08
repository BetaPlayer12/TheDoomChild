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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/FailedExperiment")]
    public class FailedExperimentAI : CombatAIBrain<FailedExperimentAI.Info>, IResetableAIBrain, IKnockbackable, IAmbushingAI
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
            private SimpleAttackInfo m_attack = new SimpleAttackInfo();
            public SimpleAttackInfo attack => m_attack;
            [SerializeField, ValueDropdown("GetAnimations"), TabGroup("Attack")]
            private string m_crawlAttackAnimation;
            public string crawlAttackAnimation => m_crawlAttackAnimation;
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
            private BasicAnimationInfo m_idleAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleCapsuleAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo idleCapsuleAnimation => m_idleCapsuleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo turnAnimation => m_turnAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_counterflinch1Animation = new BasicAnimationInfo();
            public BasicAnimationInfo counterflinch1Animation => m_counterflinch1Animation;
            [SerializeField]
            private BasicAnimationInfo m_counterflinch2Animation = new BasicAnimationInfo();
            public BasicAnimationInfo counterflinch2Animation => m_counterflinch2Animation;
            [SerializeField]
            private BasicAnimationInfo m_crawlAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo crawlAnimation => m_crawlAnimation;
            [SerializeField]
            private BasicAnimationInfo m_evadeAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo evadeAnimation => m_evadeAnimation;
            [SerializeField]
            private BasicAnimationInfo m_rawrAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo rawrAnimation => m_rawrAnimation;
            [SerializeField]
            private BasicAnimationInfo m_dormantAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo dormantAnimation => m_dormantAnimation;
            [SerializeField]
            private BasicAnimationInfo m_awakenAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo awakenAnimation => m_awakenAnimation;
            [SerializeField]
            private BasicAnimationInfo m_fallAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo fallAnimation => m_fallAnimation;
            [SerializeField]
            private BasicAnimationInfo m_landAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo landAnimation => m_landAnimation;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_attack.SetData(m_skeletonDataAsset);

                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_idleCapsuleAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_counterflinch1Animation.SetData(m_skeletonDataAsset);
                m_counterflinch2Animation.SetData(m_skeletonDataAsset);
                m_crawlAnimation.SetData(m_skeletonDataAsset);
                m_evadeAnimation.SetData(m_skeletonDataAsset);
                m_rawrAnimation.SetData(m_skeletonDataAsset);
                m_dormantAnimation.SetData(m_skeletonDataAsset);
                m_awakenAnimation.SetData(m_skeletonDataAsset);
                m_fallAnimation.SetData(m_skeletonDataAsset);
                m_landAnimation.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Dormant,
            Detect,
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
            AttackMelee,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        /*[SerializeField, TabGroup("Reference")]
        private IsolatedCharacterPhysics2D m_characterPhysics;*/
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
        //private bool m_prepAmbush;
        private Vector2 m_startPoint;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;

        //[SerializeField]
        //private bool m_willPatrol;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        [SerializeField]
        private Collider2D m_bitebox;
        [SerializeField]
        private Collider2D m_bodyCollider;
        private State m_turnState;

        private Coroutine m_detectRoutine;
        private Coroutine m_attackRoutine;
        private Coroutine m_sneerRoutine;
        private Coroutine m_patienceRoutine;
        private Coroutine m_evadeRoutine;

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

        private bool TargetBlocked()
        {
            Vector2 wat = m_character.centerMass.position;
            RaycastHit2D hit = Physics2D.Raycast(/*m_projectilePoint.position*/wat, m_targetInfo.position - wat, 1000, LayerMask.GetMask("Player") + DChildUtility.GetEnvironmentMask());
            var eh = hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") ? false : true;
            Debug.DrawRay(wat, m_targetInfo.position - wat);
            Debug.Log("Shot is " + eh + " by " + LayerMask.LayerToName(hit.transform.gameObject.layer));
            return hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") ? false : true;
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
                    m_turnState = State.Standby;
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
            Debug.Log("___1111Die");
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);

            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            m_selfCollider.enabled = false;
            if (m_attackRoutine != null)
            {
                StopCoroutine(m_attackRoutine);
            }
            if (m_sneerRoutine != null)
            {
                StopCoroutine(m_sneerRoutine);
            }
            if (m_evadeRoutine != null)
            {
                StopCoroutine(m_evadeRoutine);
                m_evadeRoutine = null;
            }
            m_animation.SetEmptyAnimation(0, 0);
            m_animation.SetAnimation(0, m_info.deathAnimation, false);
            //m_characterPhysics.UseStepClimb(true);
            m_movement.Stop();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            
            if (!m_damageable.isAlive) {
                Debug.Log("___1111Im Already dead??");
                return;
            }

            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idleAnimation.animation)
            {
                if (m_evadeRoutine != null)
                {
                    StopCoroutine(m_evadeRoutine);
                    m_evadeRoutine = null;
                }
                m_flinchHandle.m_autoFlinch = true;
                StopAllCoroutines();
                //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                m_stateHandle.OverrideState(State.WaitBehaviourEnd);
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
                m_stateHandle.Wait(State.ReevaluateSituation);
                m_evadeRoutine = StartCoroutine(EvadeRoutine());
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
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.AttackMelee, m_info.attack.range)/**/);
            m_attackDecider.hasDecidedOnAttack = false;
        }

        //private IEnumerator DetectRoutine()
        //{
        //    if (!m_willPatrol)
        //    {
        //        m_animation.EnableRootMotion(true, true);
        //        m_animation.SetAnimation(0, m_info.awakenAnimation, false).MixDuration = 0;
        //        yield return new WaitForAnimationComplete(m_animation.animationState, m_info.awakenAnimation);
        //        m_character.physics.simulateGravity = true;
        //        m_hitbox.Enable();
        //    }
        //    if (!IsFacingTarget())
        //    {
        //        CustomTurn();
        //    }
        //    //m_animation.SetAnimation(0, m_info.rawrAnimation, false).MixDuration = 0;
        //    //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rawrAnimation);
        //    //m_animation.DisableRootMotion();
        //    m_willPatrol = true;
        //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
        //    m_stateHandle.OverrideState(State.ReevaluateSituation);
        //    yield return null;
        //}

        private IEnumerator DetectRoutine()
        {
            if (!m_character.physics.simulateGravity)
            {
                m_animation.EnableRootMotion(true, true);
                m_animation.SetAnimation(0, m_info.awakenAnimation, false);
                //m_animation.AddAnimation(0, m_info.idleAnimation, false, 0)/*.TimeScale = 5f*/;
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.awakenAnimation);
                m_character.physics.simulateGravity = true;
                m_animation.DisableRootMotion();
                m_animation.SetAnimation(0, m_info.fallAnimation, true).MixDuration = 0;
                yield return new WaitUntil(() => m_groundSensor.isDetecting);
                //yield return new WaitForSeconds(0.5f);
                //m_animation.SetAnimation(0, m_info.landAnimation, false);
                //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.landAnimation);
                m_animation.SetAnimation(0, m_info.rawrAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rawrAnimation);
            }
            m_hitbox.Enable();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            m_detectRoutine = null;
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
                m_turnState = State.Dormant;
                m_stateHandle.SetState(State.Turning);
                yield return null;
            }
        }

        private IEnumerator SneerRoutine()
        {
            //m_stateHandle.Wait(State.ReevaluateSituation);
            m_movement.Stop();
            while (true)
            {
                m_animation.SetAnimation(0, m_info.rawrAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rawrAnimation);
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 3f));
                //m_animation.SetAnimation(0, m_info.rawrAnimation, false);
                //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rawrAnimation);

                //yield return new WaitForSeconds(3f);
                yield return null;
            }
        }

        private IEnumerator AttackRoutine()
        {
            m_animation.EnableRootMotion(true, true);
            m_animation.SetAnimation(0, m_info.attack.animation, false);
            yield return new WaitForSeconds(0.8f);
            m_bitebox.enabled = true;
            m_bodyCollider.enabled = false;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack.animation);
            m_bodyCollider.enabled = true;
            m_bitebox.enabled = false;
            m_animation.EnableRootMotion(true, false);
            yield return new WaitUntil(() => m_groundSensor.isDetecting);
            m_animation.SetAnimation(0, m_info.crawlAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.crawlAnimation);
            //m_animation.SetAnimation(0, m_info.rawrAnimation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rawrAnimation);
            m_animation.SetAnimation(0, m_info.evadeAnimation, false);
            m_animation.AddAnimation(0, m_info.idleAnimation.animation, true, 0)/*.TimeScale = 5*/;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.idleAnimation);
            m_animation.EnableRootMotion(true, false);
            yield return new WaitUntil(() => m_groundSensor.isDetecting);
            m_attackRoutine = null;
            m_selfCollider.enabled = true;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator EvadeRoutine()
        {
            m_animation.EnableRootMotion(true, true);
            m_selfCollider.enabled = false;
            m_animation.SetAnimation(0, m_info.evadeAnimation, false);
            m_animation.AddAnimation(0, m_info.idleAnimation.animation, true, 0)/*.TimeScale = 5*/;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.idleAnimation);
            m_animation.EnableRootMotion(true, false);
            yield return new WaitUntil(() => m_groundSensor.isDetecting);
            m_evadeRoutine = null;
            m_selfCollider.enabled = true;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        public void LaunchAmbush(Vector2 position)
        {
            enabled = true;
            m_aggroCollider.enabled = true;
            m_stateHandle.OverrideState(State.Detect);
        }

        public void PrepareAmbush(Vector2 position)
        {
            StopAllCoroutines();

            m_character.transform.position = m_startPoint;
            m_aggroCollider.enabled = false;
            m_character.physics.simulateGravity = false;
            m_character.physics.SetVelocity(Vector2.zero);
            m_hitbox.Disable();
            m_animation.SetAnimation(0, m_info.idleCapsuleAnimation, true);
            m_movement.Stop();
            m_stateHandle.OverrideState(State.Dormant);
            enabled = false;
        }

        protected override void Start()
        {
            base.Start();
            m_currentTimeScale = UnityEngine.Random.Range(1.0f, 2.0f);
            m_currentFullCD = UnityEngine.Random.Range(m_info.attackCD * .5f, m_info.attackCD * 2f);

            m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
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
            m_stateHandle = new StateHandle<State>(!enabled ? State.Dormant : State.Patrol, State.WaitBehaviourEnd);
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
                    m_movement.Stop();
                    if (!IsFacingTarget() && m_character.physics.simulateGravity && m_targetInfo.isValid)
                    {
                        m_turnState = State.Detect;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            m_stateHandle.SetState(State.Turning);
                        return;
                    }
                    else
                    {
                        m_stateHandle.SetState(State.Attacking);
                    }

                    if (m_detectRoutine == null)
                    {
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        m_detectRoutine = StartCoroutine(DetectRoutine());
                    }
                    break;

                case State.Dormant:
                    m_animation.SetAnimation(0, m_info.idleCapsuleAnimation, true);
                    m_movement.Stop();
                    break;

                case State.Patrol:
                    if (!m_character.physics.simulateGravity)
                    {
                        m_animation.EnableRootMotion(true, false);
                        m_aggroCollider.enabled = true;
                        m_character.physics.simulateGravity = true;
                        m_hitbox.Enable();
                    }

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


                    m_animation.EnableRootMotion(false, false);
                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.AttackMelee:
                            m_attackRoutine = StartCoroutine(AttackRoutine());
                            break;
                    }
                    m_attackDecider.hasDecidedOnAttack = false;

                    break;

                case State.Cooldown:
                    if (Vector2.Distance(m_targetInfo.position, transform.position) < m_info.targetDistanceTolerance)
                    {
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        m_evadeRoutine = StartCoroutine(EvadeRoutine());
                        return;
                    }

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
                        if (Vector2.Distance(m_targetInfo.position, transform.position) < m_info.targetDistanceTolerance)
                        {
                            m_stateHandle.Wait(State.ReevaluateSituation);
                            m_evadeRoutine = StartCoroutine(EvadeRoutine());
                            return;
                        }

                        if (IsFacingTarget())
                        {
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
                                    m_animation.SetAnimation(0, m_info.move.animation, true);
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

            if (m_enablePatience && m_stateHandle.currentState != State.Standby && m_turnState != State.Standby)
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

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }

        public void HandleKnockback(float resumeAIDelay)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation.animation)
            {
                if (m_attackRoutine != null)
                {
                    StopCoroutine(m_attackRoutine);
                }
                if (m_sneerRoutine != null)
                {
                    StopCoroutine(m_sneerRoutine);
                }
                StopAllCoroutines();
                m_animation.DisableRootMotion();
                m_stateHandle.Wait(State.ReevaluateSituation);
                StartCoroutine(KnockbackRoutine(resumeAIDelay));
            }
        }

        private IEnumerator KnockbackRoutine(float timer)
        {
            //enabled = false;
            //m_flinchHandle.m_autoFlinch = false;
            //m_characterPhysics.UseStepClimb(false);
            m_flinchHandle.gameObject.SetActive(false);
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation.animation)
            {
                //m_flinchHandle.enabled = false;
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.flinchAnimation.animation)
                {
                    m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
                }
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
            yield return new WaitForSeconds(timer);
            //m_characterPhysics.UseStepClimb(true);
            m_flinchHandle.gameObject.SetActive(true);
            //yield return new WaitUntil(() => m_groundSensor.isDetecting);
            //enabled = true;
            //m_flinchHandle.enabled = true;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        public override void ReturnToSpawnPoint()
        {
            transform.position = m_startPoint;
        }
    }
}
