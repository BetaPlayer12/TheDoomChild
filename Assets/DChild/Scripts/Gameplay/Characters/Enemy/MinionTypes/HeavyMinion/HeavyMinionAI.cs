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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/HeavyMinion")]
    public class HeavyMinionAI : CombatAIBrain<HeavyMinionAI.Info>, IResetableAIBrain, IBattleZoneAIBrain/*, IKnockbackableAI*/
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Basic Behaviours
            [SerializeField]
            private MovementInfo m_walk = new MovementInfo();
            public MovementInfo walk => m_walk;
            [SerializeField, MinValue(0)]
            private float m_patrolIdleTime;
            public float patrolIdleTime => m_patrolIdleTime;

            //Attack Behaviours
            [SerializeField, TabGroup("Tackle")]
            private SimpleAttackInfo m_attackTackle = new SimpleAttackInfo();
            public SimpleAttackInfo attackTackle => m_attackTackle;
            [SerializeField, ValueDropdown("GetAnimations"), TabGroup("Tackle")]
            private string m_attackTackleAnticipationAnimation;
            public string attackTackleAnticipationAnimation => m_attackTackleAnticipationAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), TabGroup("Tackle")]
            private string m_attackTackleChargeAnimation;
            public string attackTackleChargeAnimation => m_attackTackleChargeAnimation;
            [SerializeField, TabGroup("Melee")]
            private SimpleAttackInfo m_attackMelee = new SimpleAttackInfo();
            public SimpleAttackInfo attackMelee => m_attackMelee;
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
            private string m_detectFXEvent;
            public string detectFXEvent => m_detectFXEvent;


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_walk.SetData(m_skeletonDataAsset);
                m_attackTackle.SetData(m_skeletonDataAsset);
                m_attackMelee.SetData(m_skeletonDataAsset);
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
            AttackTackle,
            AttackMelee,
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
        private CircleCollider2D m_explosionBB;
        [SerializeField, TabGroup("Reference")]
        private BoxCollider2D m_chargedDashBB;
        [SerializeField, TabGroup("Reference")]
        private BoxCollider2D m_bodyCollisionBB;
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

        private float m_currentPatience;
        private float m_currentCD;
        private float m_currentFullCD;
        private float m_currentMoveSpeed;
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
        
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_headFX;
        [SerializeField, TabGroup("FX")]
        private ParticleSystemRenderer m_renderer;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_detectionFX;
        [SerializeField, TabGroup("FX")]
        private ParticleSystem m_explosionFX;

        private MaterialPropertyBlock m_propertyBlock;

        [SerializeField]
        private SpineEventListener m_spineListener;

        [SerializeField]
        private bool m_willPatrol = true;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private State m_turnState;


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

        private IEnumerator PatrolTurnRoutine()
        {
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            yield return new WaitForSeconds(m_info.patrolIdleTime);
            m_turnHandle.Execute();
            yield return null;
        }

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
            m_attackDecider.hasDecidedOnAttack = true;
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
                m_isDetecting = false;
                m_enablePatience = false;
                m_stateHandle.SetState(State.Patrol);
                var mainFX = m_headFX.GetComponent<ParticleSystem>().main;
                mainFX.startSize = 1f;
                m_renderer.material.SetVector("Main_Speed", new Vector2(-1, 0));
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
            m_movement.Stop();
            m_headFX.Stop();
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
            m_isDetecting = false;
            m_enablePatience = false;
            m_stateHandle.OverrideState(State.Patrol);
            enabled = true;
        }

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.AttackTackle, m_info.attackTackle.range), 
                                    new AttackInfo<Attack>(Attack.AttackMelee, m_info.attackMelee.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {
            m_animation.SetAnimation(0, m_info.idleAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.idleAnimation);
            m_animation.SetAnimation(0, m_info.detectAnimation, false).TimeScale = .5f;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        private IEnumerator AttackTackleRoutine()
        {
            m_bodyCollisionBB.enabled = false;
            m_animation.SetAnimation(0, m_info.attackTackleAnticipationAnimation, false);
            m_flinchHandle.m_autoFlinch = UnityEngine.Random.Range(0, 100) <= 30 ? true : false;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attackTackleAnticipationAnimation);
            m_animation.SetAnimation(0, m_info.attackTackleChargeAnimation, true);
            yield return new WaitForSeconds(1.5f);
            m_flinchHandle.m_autoFlinch = false;
            m_animation.SetAnimation(0, m_info.attackTackle.animation, false);
            while (m_animation.animationState.GetCurrent(0).TrackTime < m_animation.animationState.GetCurrent(0).AnimationTime)
            {
                if (!m_edgeSensor.isDetecting)
                {
                    m_animation.DisableRootMotion();
                }
                yield return null;
            }
            m_chargedDashBB.enabled = true;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attackTackle.animation);
            m_selfCollider.enabled = true;
            m_chargedDashBB.enabled = false;
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator GroundSmashRoutine()
        {
            m_flinchHandle.gameObject.SetActive(false);
            m_animation.SetEmptyAnimation(1, 0).Alpha = 0;
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.attackMelee.animation, false);
            yield return new WaitForSeconds(1.25f);
            m_bodyCollisionBB.enabled = false;
            m_explosionBB.enabled = true;
            m_explosionFX.Play();
            yield return new WaitForSeconds(0.15f);
            m_explosionBB.enabled = false;
            m_bodyCollisionBB.enabled = true;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attackMelee.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_flinchHandle.gameObject.SetActive(true);
            m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private void IncreaseHeadFX()
        {
            m_detectionFX.Play();
            var mainFX = m_headFX.gameObject.GetComponent<ParticleSystem>().main;
            mainFX.startSize = 1.5f;
            m_propertyBlock.SetVector("Vector2_3CFAEA88", new Vector4(-2, 0));
            m_renderer.SetPropertyBlock(m_propertyBlock);
        }

        protected override void Start()
        {
            base.Start();
            m_currentMoveSpeed = UnityEngine.Random.Range(m_info.walk.speed * .75f, m_info.walk.speed * 1.25f);
            m_currentFullCD = UnityEngine.Random.Range(m_info.attackCD * .5f, m_info.attackCD * 2f);
            m_spineListener.Subscribe(m_info.detectFXEvent, IncreaseHeadFX);
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
            m_stateHandle = new StateHandle<State>(m_willPatrol ? State.Patrol : State.Idle, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            m_propertyBlock = new MaterialPropertyBlock();
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
                    break;

                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_movement.Stop();
                    break;

                case State.Patrol:
                    if (/*!m_wallSensor.isDetecting &&*/ m_groundSensor.isDetecting)
                    {
                        m_turnState = State.Patrol;
                        m_animation.EnableRootMotion(false, false);
                        m_animation.SetAnimation(0, m_info.walk.animation, true);
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        m_patrolHandle.Patrol(m_movement, m_info.walk.speed, characterInfo);
                    }
                    else
                    {
                        m_movement.Stop();
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    }
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    if (m_turnState == State.Patrol)
                    {
                        StartCoroutine(PatrolTurnRoutine());
                    }
                    else
                    {
                        m_turnHandle.Execute();
                    }
                    break;

                case State.Attacking:
                    if (IsFacingTarget())
                    {
                        if (IsTargetInRange(m_info.attackTackle.range) && !m_wallSensor.allRaysDetecting && m_edgeSensor.isDetecting)
                        {
                            m_stateHandle.Wait(State.Cooldown);
                            m_animation.SetAnimation(0, m_info.idleAnimation, true);
                            if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) <= m_info.attackMelee.range)
                            {
                                //m_animation.EnableRootMotion(true, false);
                                //m_attackHandle.ExecuteAttack(m_info.attackMelee.animation, m_info.idleAnimation);
                                m_selfCollider.enabled = true;
                                StartCoroutine(GroundSmashRoutine());
                            }
                            else
                            {
                                m_animation.EnableRootMotion(true, false);
                                m_selfCollider.enabled = false;
                                //m_attackHandle.ExecuteAttack(m_info.attackTackle.animation, m_info.idleAnimation);
                                StartCoroutine(AttackTackleRoutine());
                            }
                            m_attackDecider.hasDecidedOnAttack = false;
                        }
                        else
                        {
                            if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                            {
                                m_animation.EnableRootMotion(false, false);
                                m_selfCollider.enabled = false;
                                m_animation.SetAnimation(0, m_info.walk.animation, true);
                                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_currentMoveSpeed);
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
                        m_currentCD = 0;
                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                    }

                    break;
                case State.Chasing:
                    {
                        m_flinchHandle.m_autoFlinch = false;
                        //m_attackDecider.hasDecidedOnAttack = false;
                        //ChooseAttack();
                        if (m_attackDecider.hasDecidedOnAttack /*&& IsTargetInRange(m_currentAttackRange) && !m_wallSensor.allRaysDetecting*/)
                        {
                            m_movement.Stop();
                            m_stateHandle.SetState(State.Attacking);
                        }
                        else
                        {
                            //m_currentAttack = Vector2.Distance(m_targetInfo.position, transform.position) <= m_info.attackMelee.range + 15 ? Attack.AttackMelee : Attack.AttackTackle;
                            //m_currentAttackRange = m_currentAttack == Attack.AttackMelee ? m_info.attackMelee.range : m_info.attackTackle.range;
                            m_attackDecider.hasDecidedOnAttack = true;
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
            transform.position = m_startPoint;
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }

        public void HandleKnockback(float resumeAIDelay)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation)
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
            //m_flinchHandle.enabled = false;
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            yield return new WaitForSeconds(timer);
            //enabled = true;
            //m_flinchHandle.enabled = true;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }
    }
}
