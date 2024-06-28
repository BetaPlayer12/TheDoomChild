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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/LightMinion")]
    public class LightMinionAI : CombatAIBrain<LightMinionAI.Info>, IResetableAIBrain, IBattleZoneAIBrain, IKnockbackable
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
            [SerializeField, TabGroup("Attack")]
            private SimpleAttackInfo m_attackStart = new SimpleAttackInfo();
            public SimpleAttackInfo attackStart => m_attackStart;
            [SerializeField, ValueDropdown("GetAnimations"), TabGroup("Attack")]
            private string m_attackLoop;
            public string attackLoop => m_attackLoop;
            [SerializeField, ValueDropdown("GetAnimations"), TabGroup("Attack")]
            private string m_attackEnd;
            public string attackEnd => m_attackEnd;
            [SerializeField, ValueDropdown("GetAnimations"), TabGroup("Attack")]
            private string m_attackAnticipation;
            public string attackAnticipation => m_attackAnticipation;
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
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_detectAnimation;
            public string detectAnimation => m_detectAnimation;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_trailEvent;
            public string trailEvent => m_trailEvent;


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_walk.SetData(m_skeletonDataAsset);
                m_run.SetData(m_skeletonDataAsset);
                m_attackStart.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Detect,
            Idle,
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
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private Collider2D m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_boundBoxGO;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private EdgeCollider2D m_edgeCollider;
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

        [SerializeField]
        private bool m_willPatrol;

        private List<Vector2> m_Points;
        private Coroutine m_trailDamageCoroutine;
        private Vector2 m_startPosition;
        private float m_currentPatience;
        private float m_currentCD;
        private float m_currentFullCD;
        private float m_currentMoveSpeed;
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
        private ParticleFX m_trailFX;

        [SerializeField, TabGroup("FX")]
        private List<ParticleSystem> m_fxSystem;

        [SerializeField, TabGroup("AttackCollider")]
        private GameObject BodyCollider;
        [SerializeField, TabGroup("AttackCollider")]
        private GameObject GroundRoll;
        [SerializeField, TabGroup("AttackCollider")]
        private GameObject FireTrail;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private State m_turnState;
        private Coroutine m_randomTurnRoutine;
        private Coroutine m_attackCoroutine;


        //[SerializeField]
        //private AudioSource m_Audiosource;
        //[SerializeField]
        //private AudioClip m_AttackClip;
        //[SerializeField]
        //private AudioClip m_DeadClip;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_flinchHandle.m_autoFlinch = true;
            m_animation.DisableRootMotion();
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
                if (Mathf.Abs((m_targetInfo.position.y - 3) - transform.position.y) > 3f && ShotBlocked() || !m_edgeSensor.isDetecting)
                {
                    m_currentPatience = m_info.patience;
                }
                //StartCoroutine(PatienceRoutine());
            }
        }

        private bool ShotBlocked()
        {
            Vector2 wat = transform.position;
            RaycastHit2D hit = Physics2D.Raycast(/*m_projectilePoint.position*/wat, m_targetInfo.position - wat, 1000, LayerMask.GetMask("Player") + DChildUtility.GetEnvironmentMask());
            var eh = hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") ? false : true;
            Debug.DrawRay(wat, m_targetInfo.position - wat);
            Debug.Log("Shot is " + eh + " by " + LayerMask.LayerToName(hit.transform.gameObject.layer));
            return hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") ? false : true;
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            //if (m_stateHandle.currentState == State.Patrol)
            //{

            //}
            //else
            //{
            //}
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
        //private IEnumerator PatienceRoutine()
        //{
        //    //if (m_enablePatience)
        //    //{
        //    //    while (m_currentPatience < m_info.patience)
        //    //    {
        //    //        m_currentPatience += m_character.isolatedObject.deltaTime;
        //    //        yield return null;
        //    //    }
        //    //}
        //    yield return new WaitForSeconds(m_info.patience);
        //    m_targetInfo.Set(null, null);
        //    m_isDetecting = false;
        //    m_enablePatience = false;
        //    m_stateHandle.SetState(State.Patrol);
        //}

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            StopAllCoroutines();
            m_selfCollider.enabled = false;
            GetComponentInChildren<Hitbox>().gameObject.SetActive(false);
            m_boundBoxGO.SetActive(false);
            base.OnDestroyed(sender, eventArgs);
            
            StartCoroutine(StopAttackRoutine());
            m_animation.SetAnimation(0, m_info.deathAnimation, false);
            m_movement.Stop();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                StopAllCoroutines();
                m_animation.animationState.TimeScale = .5f;
                m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            }
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                m_animation.animationState.TimeScale = 1f;
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation)
                    m_animation.SetEmptyAnimation(0, 0);
                //m_animation.SetAnimation(0, m_info.idleAnimation, true);
                m_selfCollider.enabled = false;
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
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack, m_info.attackStart.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        private IEnumerator RandomTurnRoutine()
        {
            while (true)
            {
                Debug.Log("SCORCHLING TURN TURN");
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

        private IEnumerator AttackStartRoutine()
        {
            for (int i = 0; i < m_fxSystem.Count; i++)
            {
                m_fxSystem[i].gameObject.SetActive(true);
            }
            m_flinchHandle.gameObject.SetActive(false);
            m_selfCollider.enabled = false;
            m_animation.SetAnimation(0, m_info.attackStart.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attackStart.animation);
            m_trailDamageCoroutine = StartCoroutine(TrailDamageRoutine());
            m_trailFX.Play();
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator TrailDamageRoutine()
        {
            Vector2 startPoint = transform.position;
            for (int i = 0; i < 2; i++)
            {
                var pos = transform.position - m_edgeCollider.transform.position;
                pos = new Vector2(m_character.facing == HorizontalDirection.Right ? pos.x : -pos.x, pos.y);
                m_Points.Add(pos);
            }
            //m_Points[0] = m_Points[0] + new Vector2(transform.position.x + 35, 0);
            while (m_Points[0].x <= 35)
            {
                m_Points[0] = new Vector2(m_Points[0].x + Time.deltaTime * 40f, m_Points[0].y);
                m_edgeCollider.points = m_Points.ToArray();
                yield return null;
            }
        }

        private IEnumerator ResetTrailDamageRoutine()
        {
            if (m_Points.Count > 0)
            {
                while (m_Points[0].x >= 0)
                {
                    m_Points[0] = new Vector2(m_Points[0].x - Time.deltaTime * 40f, m_Points[0].y);
                    m_edgeCollider.points = m_Points.ToArray();
                    yield return null;
                }
                for (int i = 0; i < m_Points.Count; i++)
                {
                    m_Points[i] = Vector2.zero;
                }
                m_edgeCollider.points = m_Points.ToArray();
                m_Points.Clear();
                m_edgeCollider.points = null;
            }
            yield return null;
        }

        private IEnumerator AttackRoutine()
        {
            m_selfCollider.enabled = false;
            m_animation.SetAnimation(0, m_info.attackLoop, true);
            //yield return new WaitForSeconds(1.4f);
            float countdown = 0;
            BodyCollider.SetActive(false);
            GroundRoll.SetActive(true);
            while (countdown < 1.5f /*|| !m_wallSensor.isDetecting*/)
            {
                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_currentMoveSpeed * 2.5f);
                countdown += Time.deltaTime;
                yield return null;
            }
            m_trailFX.Stop();
            for (int i = 0; i < m_fxSystem.Count; i++)
            {
                m_fxSystem[i].gameObject.SetActive(false);
            }
            //m_trailFX.gameObject.SetActive(false);
            //m_hitbox.SetInvulnerability(Invulnerability.None);
            m_movement.Stop();
            BodyCollider.SetActive(true);
            GroundRoll.SetActive(false);
            m_animation.SetAnimation(0, m_info.attackEnd, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attackEnd);
            m_flinchHandle.gameObject.SetActive(true);
            m_selfCollider.enabled = true;
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            StartCoroutine(ResetTrailDamageRoutine());
            yield return new WaitForSeconds(1f);
            if (m_trailDamageCoroutine != null)
            {
                StopCoroutine(m_trailDamageCoroutine);
            }
            m_attackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator StopAttackRoutine()
        {
            m_trailFX.Stop();
            for (int i = 0; i < m_fxSystem.Count; i++)
            {
                m_fxSystem[i].gameObject.SetActive(false);
            }
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.attackEnd, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attackEnd);
            m_selfCollider.enabled = true;
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            StartCoroutine(ResetTrailDamageRoutine());
            yield return new WaitForSeconds(1f);
            if (m_trailDamageCoroutine != null)
            {
                StopCoroutine(m_trailDamageCoroutine);
            }
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        protected override void Start()
        {
            base.Start();
            m_currentMoveSpeed = UnityEngine.Random.Range(m_info.run.speed * .75f, m_info.run.speed * 1.25f);
            m_currentFullCD = UnityEngine.Random.Range(m_info.attackCD * .5f, m_info.attackCD * 2f);
            m_startPosition = transform.position;

            m_randomTurnRoutine = StartCoroutine(RandomTurnRoutine());
            if (m_willPatrol)
            {
                Debug.Log("STOP RANDOM TURN");
                StopCoroutine(m_randomTurnRoutine);
            }
            m_startpoint = transform.position;
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
            m_stateHandle = new StateHandle<State>(m_willPatrol ? State.Patrol : State.Idle, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();

            m_Points = new List<Vector2>();
        }


        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Edge Sensor is " + m_edgeSensor.isDetecting);
            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    m_movement.Stop();
                    m_flinchHandle.m_autoFlinch = false;
                    StopCoroutine(m_randomTurnRoutine);
                    if (IsFacingTarget())
                    {
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        //StartCoroutine(DetectRoutine());
                        StartCoroutine(AttackStartRoutine());
                    }
                    else
                    {
                        m_turnState = State.Detect;
                        m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_movement.Stop();
                    break;

                case State.Patrol:
                    if (IsFacing(m_startPosition))
                    {
                        if (Vector2.Distance(m_startPosition, transform.position) >= 10f && /*!m_wallSensor.isDetecting &&*/ m_groundSensor.isDetecting)
                        {
                            m_animation.EnableRootMotion(false, false);
                            //m_animation.SetAnimation(0, m_info.walk.animation, true);
                            m_animation.SetAnimation(0, m_info.attackLoop, true);
                            m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_currentMoveSpeed);
                            //var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                            //m_patrolHandle.Patrol(m_movement, m_info.walk.speed, characterInfo);
                        }
                        else
                        {
                            m_movement.Stop();
                            m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        }
                    }
                    else
                    {
                        m_turnState = State.Patrol;
                        m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute();
                    break;

                case State.Attacking:
                    if (IsFacingTarget())
                    {
                        if (IsTargetInRange(m_info.attackStart.range) && !m_wallSensor.allRaysDetecting)
                        {
                            m_stateHandle.Wait(State.Cooldown);
                            m_animation.SetAnimation(0, m_info.idleAnimation, true);

                            m_attackCoroutine = StartCoroutine(AttackRoutine());

                            m_attackDecider.hasDecidedOnAttack = false;
                        }
                        else
                        {
                            if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                            {
                                //var distance = Vector2.Distance(m_targetInfo.position, transform.position);
                                m_animation.EnableRootMotion(false, false);
                                m_selfCollider.enabled = false;
                                //m_animation.SetAnimation(0, distance >= m_info.targetDistanceTolerance ? m_info.run.animation : m_info.walk.animation, true);
                                //m_movement.MoveTowards(Vector2.one * transform.localScale.x, distance >= m_info.targetDistanceTolerance ? m_info.run.speed : m_info.walk.speed);
                                m_animation.SetAnimation(0, m_info.attackLoop, true);
                                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.run.speed * 2.5f);
                            }
                            else
                            {
                                m_movement.Stop();
                                m_selfCollider.enabled = true;
                                m_stateHandle.Wait(State.Cooldown);
                                StartCoroutine(StopAttackRoutine());
                                //m_animation.SetAnimation(0, m_info.idleAnimation, true);
                            }
                        }
                    }
                    else
                    {
                        m_turnState = State.Attacking;
                        m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.Cooldown:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    }

                    if (m_currentCD <= m_currentFullCD /*&& !m_wallSensor.isDetecting*/)
                    {
                        m_currentCD += Time.deltaTime;
                    }
                    else
                    {
                        if (IsFacingTarget() && !m_wallSensor.allRaysDetecting)
                        {
                            m_currentCD = 0;
                            m_stateHandle.Wait(State.ReevaluateSituation);
                            StartCoroutine(AttackStartRoutine());
                        }
                    }

                    break;
                case State.Chasing:
                    {
                        m_attackDecider.hasDecidedOnAttack = true;
                        if (m_attackDecider.hasDecidedOnAttack /*&& IsTargetInRange(m_currentAttackRange) && !m_wallSensor.allRaysDetecting*/)
                        {
                            m_attackDecider.hasDecidedOnAttack = false;
                            m_movement.Stop();
                            m_flinchHandle.m_autoFlinch = false;
                            m_stateHandle.SetState(State.Attacking);
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

            if (m_enablePatience)
            {
                Patience();
                //StartCoroutine(PatienceRoutine());
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Patrol);
            GetComponentInChildren<Hitbox>().gameObject.SetActive(true);
            m_boundBoxGO.SetActive(true);
            m_currentPatience = 0;
            m_enablePatience = false;
            m_isDetecting = false;
            m_selfCollider.enabled = false;
        }

        public void SwitchToBattleZoneAI()
        {
            m_stateHandle.SetState(State.Chasing);
        }

        public void SwitchToBaseAI()
        {
            m_stateHandle.SetState(State.ReevaluateSituation);
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
            if (m_attackCoroutine == null)
            {
                StopAllCoroutines();
                m_stateHandle.Wait(State.ReevaluateSituation);
                StartCoroutine(KnockbackRoutine(resumeAIDelay));
            }
        }

        private IEnumerator KnockbackRoutine(float timer)
        {
            //enabled = false;
            //m_flinchHandle.m_autoFlinch = false;
            m_animation.DisableRootMotion();
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation)
            {
                //m_flinchHandle.enabled = false;
                m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
            yield return new WaitForSeconds(timer);
            //enabled = true;
            //m_flinchHandle.enabled = true;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }
    }
}
