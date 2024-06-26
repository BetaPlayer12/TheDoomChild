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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/ZombieMushroomBack")]
    public class ZombieMushroomBackAI : CombatAIBrain<ZombieMushroomBackAI.Info>, IResetableAIBrain, IBattleZoneAIBrain, IKnockbackable
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Basic Behaviours
            [SerializeField]
            private MovementInfo m_walk = new MovementInfo();
            public MovementInfo walk => m_walk;
            [SerializeField]
            private MovementInfo m_run = new MovementInfo();
            public MovementInfo run => m_run;

            //Attack Behaviours
            [SerializeField]
            private SimpleAttackInfo m_attack1 = new SimpleAttackInfo();
            public SimpleAttackInfo attack1 => m_attack1;
            [SerializeField]
            private SimpleAttackInfo m_attack2 = new SimpleAttackInfo();
            public SimpleAttackInfo attack2 => m_attack2;


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
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_asleepAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo asleepAnimation => m_asleepAnimation;
            [SerializeField]
            private BasicAnimationInfo m_awakenAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo awakenAnimation => m_awakenAnimation;
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
            private GameObject m_poisoncloud;
            public GameObject poisoncloud => m_poisoncloud;

            [TitleGroup("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_biteEvent;
            public string biteEvent => m_biteEvent;


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_walk.SetData(m_skeletonDataAsset);
                m_run.SetData(m_skeletonDataAsset);
                m_attack2.SetData(m_skeletonDataAsset);

                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_asleepAnimation.SetData(m_skeletonDataAsset);
                m_awakenAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Asleep,
            Awaken,
            Detect,
            Idle,
            Patrol,
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
            Attack2,

            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_legCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_bodyCollider;
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
        private float m_currentMoveSpeed;
        private float m_currentRunAttackDuration;
        private bool m_enablePatience;
        private bool m_isDetecting;
        private bool m_battlezonemode;
        private Vector2 m_startPoint;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;
        [SerializeField]
        private Collider2D attackbox;
        [SerializeField]
        private bool m_willPatrol = true;
        [SerializeField]
        private SpineEventListener m_spineListener;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_currentAttack;
        private float m_currentAttackRange;
        private float m_chaseAttackRange;
        private bool m_isInAttackrange;
        private Vector2 m_currentTargetPos;

        private bool[] m_attackUsed;
        private List<Attack> m_attackCache;
        private List<float> m_attackRangeCache;

        private State m_turnState;
        private Coroutine m_randomTurnRoutine;
        private bool m_sleep = true;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_poisonCloud;
        [SerializeField, TabGroup("FX")]
        private Collider2D m_poisonCloudCollider;
        [SerializeField, TabGroup("FX")]
        private SpawnedObjectDeathHandler m_poisonCloudDeathDelay;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_cloudBurst;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_biteFX;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_flinchHandle.m_enableMixFlinch = true;
            m_animation.DisableRootMotion();
            m_flinchHandle.m_autoFlinch = true;
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable);
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
            m_selfCollider.enabled = false;
            GetComponentInChildren<Hitbox>().gameObject.SetActive(false);
            base.OnDestroyed(sender, eventArgs);
            
            m_movement.Stop();
        }

       

        private void OnDamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idleAnimation.animation)
            {
                StopAllCoroutines();
                //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                m_stateHandle.Wait(State.ReevaluateSituation);
                StartCoroutine(FlinchRoutine());
                m_stateHandle.OverrideState(State.ReevaluateSituation);
            }
        }

        private IEnumerator FlinchRoutine()
        {
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            //m_stateHandle.ApplyQueuedState();
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
            m_attackDecider.SetList(
                                    new AttackInfo<Attack>(Attack.Attack2, m_info.attack2.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {
            m_movement.Stop();
            m_animation.EnableRootMotion(true, true);
            m_animation.DisableRootMotion();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }
        private IEnumerator AwakenRoutine()
        {
            m_movement.Stop();
            m_animation.EnableRootMotion(true, true);
            m_animation.SetAnimation(0, m_info.awakenAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.awakenAnimation);
            StartCoroutine(AttackRoutine1());
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
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                    m_stateHandle.SetState(State.Turning);
                yield return null;
            }
        }
        private IEnumerator AttackRoutine1()
        {
            m_animation.SetAnimation(0, m_info.attack1.animation, false);
            m_cloudBurst.Play();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack1.animation);
            m_cloudBurst.Stop();
            SpawnCloud();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_flinchHandle.m_autoFlinch = true;
            m_stateHandle.SetState(State.ReevaluateSituation);

            yield return null;
        }
        private IEnumerator AttackRoutine2()
        {

            yield return new WaitForSeconds(0.25f);
            m_biteFX.Stop();
            attackbox.enabled = false;
            yield return null;
        }

        private Vector2 GroundPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1000, DChildUtility.GetEnvironmentMask());
            return hit.point;
        }
        private void SpawnCloud()
        {
            //Instantiate(m_info.poisoncloud, this.transform.position, Quaternion.identity);
            m_poisonCloud.Play();
            m_poisonCloudCollider.enabled = true;
            m_poisonCloudDeathDelay.enabled = true;
        }
        private void Bite()
        {
            attackbox.enabled = true;
            m_biteFX.Play();
            StartCoroutine(AttackRoutine2());

        }

        private void ChooseAttack()
        {
            if (!m_attackDecider.hasDecidedOnAttack)
            {
                IsAllAttackComplete();
                for (int i = 0; i < m_attackCache.Count; i++)
                {
                    m_attackDecider.DecideOnAttack();
                    if (m_attackCache[i] != m_currentAttack && !m_attackUsed[i])
                    {
                        m_attackUsed[i] = true;
                        m_currentAttack = m_attackCache[i];
                        m_currentAttackRange = m_attackRangeCache[i];
                        return;
                    }
                }
            }
        }

        private void IsAllAttackComplete()
        {
            for (int i = 0; i < m_attackUsed.Length; ++i)
            {
                if (!m_attackUsed[i])
                {
                    return;
                }
            }
            for (int i = 0; i < m_attackUsed.Length; ++i)
            {
                m_attackUsed[i] = false;
            }
        }

        void AddToAttackCache(params Attack[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                m_attackCache.Add(list[i]);
            }
        }

        void AddToRangeCache(params float[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                m_attackRangeCache.Add(list[i]);
            }
        }

        void UpdateRangeCache(params float[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                m_attackRangeCache[i] = list[i];
            }
        }

        protected override void Start()
        {
            base.Start();
            m_currentMoveSpeed = UnityEngine.Random.Range(m_info.run.speed * .75f, m_info.run.speed * 1.25f);
            m_currentFullCD = UnityEngine.Random.Range(m_info.attackCD * .5f, m_info.attackCD * 2f);

            m_randomTurnRoutine = StartCoroutine(RandomTurnRoutine());
            if (m_willPatrol)
            {
                StopCoroutine(m_randomTurnRoutine);
            }
            m_stateHandle.SetState(State.Asleep);
            m_startPoint = transform.position;
            m_spineListener.Subscribe(m_info.biteEvent, Bite);
        }

        protected override void Awake()
        {
            base.Awake();
            
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            //m_flinchHandle.FlinchStart += OnFlinchStart;
            //m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_damageable.DamageTaken += OnDamageTaken;
            m_stateHandle = new StateHandle<State>(m_willPatrol ? State.Patrol : State.Idle, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();

            m_attackCache = new List<Attack>();
            AddToAttackCache( Attack.Attack2);
            m_attackRangeCache = new List<float>();
            AddToRangeCache( m_info.attack2.range);
            m_attackUsed = new bool[m_attackCache.Count];
        }

        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Edge Sensor is " + m_edgeSensor.isDetecting);
            switch (m_stateHandle.currentState)
            {
                case State.Asleep:
                    m_sleep = true;
                    m_animation.SetAnimation(0, m_info.asleepAnimation, true);
                    break;
                case State.Awaken:
                    if (m_sleep == true)
                    {
                        m_sleep = false;
                        StartCoroutine(AwakenRoutine());
                    }
                    break;

                case State.Detect:
                    m_movement.Stop();
                    m_flinchHandle.m_autoFlinch = false;
                    
                    if (m_sleep == true)
                    {
                            m_stateHandle.SetState(State.Awaken);

                    }
                    else
                    {
                        StopCoroutine(m_randomTurnRoutine);
                        if (IsFacingTarget())
                        {
                            m_stateHandle.Wait(State.ReevaluateSituation);
                            StartCoroutine(DetectRoutine());
                        }
                        else
                        {
                            m_turnState = State.Detect;
                            m_stateHandle.SetState(State.Turning);
                        }
                    }
                    break;

                case State.Idle:
                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                    {
                        m_movement.Stop();
                        m_animation.EnableRootMotion(true, true);
                    }
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    break;

                case State.Patrol:
                    if (/*!m_wallSensor.isDetecting &&*/ m_groundSensor.isDetecting)
                    {
                        m_turnState = State.ReevaluateSituation;
                        m_animation.EnableRootMotion(false, false);
                        m_animation.SetAnimation(0, m_info.walk.animation, true);
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        m_patrolHandle.Patrol(m_movement, m_info.walk.speed, characterInfo);
                    }
                    else
                    {
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                        {
                            m_movement.Stop();
                            m_animation.EnableRootMotion(true, true);
                        }
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    }
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_movement.Stop();
                    m_animation.EnableRootMotion(true, true);
                    m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);

                    m_animation.EnableRootMotion(true, true);
                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        
                        case Attack.Attack2:
                            m_attackHandle.ExecuteAttack(m_info.attack2.animation, m_info.idleAnimation.animation);
                            break;
                    }
                    m_attackDecider.hasDecidedOnAttack = false;

                    break;

                case State.Cooldown:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                    if (IsTargetInRange(100))
                    {
                        if (!IsFacingTarget())
                        {
                            m_turnState = State.Cooldown;
                            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                                m_stateHandle.SetState(State.Turning);
                        }
                        else
                        {
                            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                            {
                                m_movement.Stop();
                                m_animation.EnableRootMotion(true, true);
                            }
                            m_selfCollider.enabled = true;
                            m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        }
                    }
                    else
                    {
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    }

                    if (m_currentCD <= m_info.attackCD && !m_wallSensor.isDetecting)
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
                        var Target = m_targetInfo.position - (Vector2)m_character.centerMass.position;
                        if (IsFacingTarget())
                        {
                            m_attackDecider.DecideOnAttack();
                            if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range) && !m_wallSensor.allRaysDetecting)
                            {
                                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                                {
                                    m_movement.Stop();
                                }
                                m_selfCollider.enabled = true;
                                GetComponent<IsolatedCharacterPhysics2D>().UseStepClimb(false);
                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {

                                if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting && Mathf.Abs(m_targetInfo.position.x - transform.position.x) > m_info.attack2.range - 1)
                                {
                                    var distance = Vector2.Distance(m_targetInfo.position, transform.position);
                                    m_animation.EnableRootMotion(false, false);
                                    m_selfCollider.enabled = false;
                                    m_animation.SetAnimation(0, distance >= m_info.targetDistanceTolerance ? m_info.run.animation : m_info.walk.animation, true);
                                    m_character.physics.SetVelocity(Target.normalized.x * (distance >= m_info.targetDistanceTolerance ? m_currentMoveSpeed : m_info.walk.speed), m_character.physics.velocity.y);
                                }
                                else
                                {
                                    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.idleAnimation.animation)
                                    {
                                        m_movement.Stop();
                                    }
                                    m_animation.EnableRootMotion(true, m_groundSensor.isDetecting ? true : false);
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
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }

            if (m_enablePatience && !m_battlezonemode)
            {
                Patience();
                //StartCoroutine(PatienceRoutine());
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Patrol);
            GetComponentInChildren<Hitbox>().gameObject.SetActive(true);
            m_currentPatience = 0;
            m_enablePatience = false;
            m_isDetecting = false;
            m_selfCollider.enabled = false;
        }

        public void SwitchToBattleZoneAI()
        {
            m_battlezonemode = true;
            m_flinchHandle.m_autoFlinch = false;
            m_stateHandle.SetState(State.Chasing);
        }

        public void SwitchToBaseAI()
        {
            m_battlezonemode = false;
            m_flinchHandle.m_autoFlinch = true;
            m_stateHandle.SetState(State.ReevaluateSituation);
        }

        public override void ReturnToSpawnPoint()
        {
            transform.position = m_startPoint;
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
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation.animation)
            {
                //m_flinchHandle.enabled = false;
                m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
            yield return new WaitForSeconds(timer);
            m_movement.Stop();
            //enabled = true;
            //m_flinchHandle.enabled = true;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }
    }
}

