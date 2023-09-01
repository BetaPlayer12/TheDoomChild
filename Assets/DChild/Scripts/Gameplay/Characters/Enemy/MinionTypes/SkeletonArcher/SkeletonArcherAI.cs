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
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/SkeletonArcher")]
    public class SkeletonArcherAI : CombatAIBrain<SkeletonArcherAI.Info>, IBattleZoneAIBrain, IResetableAIBrain, IKnockbackable
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
            [SerializeField]
            private SimpleAttackInfo m_shootAttack = new SimpleAttackInfo();
            public SimpleAttackInfo shootAttack => m_shootAttack;
            [SerializeField]
            private SimpleAttackInfo m_shootDrawAttack = new SimpleAttackInfo();
            public SimpleAttackInfo shootDrawAttack => m_shootDrawAttack;
            [SerializeField]
            private SimpleAttackInfo m_shootComboAttack = new SimpleAttackInfo();
            public SimpleAttackInfo shootComboAttack => m_shootComboAttack;
            [SerializeField, MinValue(0)]
            private float m_attackCD;
            public float attackCD => m_attackCD;
            [SerializeField, MinValue(0)]
            private float m_dodgeBackSpeed;
            public float dodgeBackSpeed => m_dodgeBackSpeed;
            //
            [SerializeField, MinValue(0)]
            private float m_patience;
            public float patience => m_patience;

            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;
            [SerializeField]
            private Vector2 m_targetOffset;
            public Vector2 targetOffset => m_targetOffset;


            //Animations
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
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
            private SimpleProjectileAttackInfo m_projectile;
            public SimpleProjectileAttackInfo projectile => m_projectile;


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_shootAttack.SetData(m_skeletonDataAsset);
                m_shootDrawAttack.SetData(m_skeletonDataAsset);
                m_shootComboAttack.SetData(m_skeletonDataAsset);
                m_projectile.SetData(m_skeletonDataAsset);

                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
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
        private GameObject m_selfCollider;
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
        private float m_currentRunAttackDuration;
        private bool m_enablePatience;
        private bool m_isDetecting;
        private bool m_battlezonemode;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;

        private float m_targetDistance;

        private ProjectileLauncher m_projectileLauncher;

        [SerializeField]
        private bool m_willPatrol = true;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private Attack m_chosenAttack;
        private State m_turnState;

        [SerializeField]
        private GameObject m_projectilePoint;
        [SerializeField]
        private GameObject m_targetPointIK;
        [SerializeField]
        private GameObject m_arrowPoint;

        private Vector2 m_lastTargetPos;
        private Vector2 m_startPoint;

        private Coroutine m_knockbackRoutine;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_flinchHandle.gameObject.SetActive(true);
            m_selfCollider.SetActive(false);
            m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable);
                m_selfCollider.SetActive(true);
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
                m_selfCollider.SetActive(false);
                m_targetInfo.Set(null, null);
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
            base.OnDestroyed(sender, eventArgs);
            
            m_movement.Stop();
        }

        //private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        //{
        //    StopAllCoroutines();
        //    //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
        //    m_stateHandle.Wait(m_targetInfo.isValid ? State.Cooldown : State.ReevaluateSituation);
        //}

        //private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        //{
        //    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation)
        //    {
        //        m_animation.SetEmptyAnimation(0, 0);
        //        m_animation.SetAnimation(0, m_info.idleAnimation, true);
        //    }
        //    m_stateHandle.ApplyQueuedState();
        //}

        private void OnDamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idleAnimation.animation && m_knockbackRoutine == null)
            {
                StopAllCoroutines();
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
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack1, m_info.shootAttack.range),
                                    new AttackInfo<Attack>(Attack.Attack2, m_info.shootDrawAttack.range),
                                    new AttackInfo<Attack>(Attack.Attack3, m_info.shootComboAttack.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            yield return new WaitForSeconds(2);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        private void LaunchProjectile()
        {
            if (m_targetInfo.isValid)
            {
                m_targetPointIK.transform.position = m_lastTargetPos;
                //m_projectileLauncher.AimAt(m_lastTargetPos);
                m_projectileLauncher.LaunchProjectile(m_character.facing == HorizontalDirection.Right ? m_arrowPoint.transform.right : -m_arrowPoint.transform.right);
            }
        }

        private bool ShotBlocked()
        {
            Vector2 wat = m_projectilePoint.transform.position;
            RaycastHit2D hit = Physics2D.Raycast(/*m_projectilePoint.position*/wat, m_targetInfo.position - wat, 1000, LayerMask.GetMask("Player") + DChildUtility.GetEnvironmentMask());
            var eh = hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") ? false : true;
#if UNITY_EDITOR
            Debug.DrawRay(wat, m_targetInfo.position - wat);
            Debug.Log("Shot is " + eh + " by " + LayerMask.LayerToName(hit.transform.gameObject.layer)); 
#endif
            return hit.transform.gameObject.layer == LayerMask.NameToLayer("Player") ? false : true;
        }

        protected override void Start()
        {
            base.Start();
            m_selfCollider.SetActive(false);

            m_spineEventListener.Subscribe(m_info.projectile.launchOnEvent, LaunchProjectile);
            m_startPoint = transform.position;
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
            m_projectileLauncher = new ProjectileLauncher(m_info.projectile.projectileInfo, m_arrowPoint.transform);
            m_stateHandle = new StateHandle<State>(m_willPatrol ? State.Patrol : State.Idle, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();

            //m_info.projectile.projectileInfo.projectile.GetComponent<IsolatedObjectPhysics2D>().gravity.gravityScale = m_gravityScale;
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
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    m_movement.Stop();
                    break;

                case State.Patrol:
                    if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting)
                    {
                        m_turnState = State.ReevaluateSituation;
                        m_animation.EnableRootMotion(false, false);
                        m_animation.SetAnimation(0, m_info.patrol.animation, true);
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        m_patrolHandle.Patrol(m_movement, m_info.patrol.speed, characterInfo);
                    }
                    else
                    {
                        m_movement.Stop();
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    }
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    m_lastTargetPos = new Vector2(m_targetInfo.position.x + UnityEngine.Random.Range(0, m_info.targetOffset.x), m_targetInfo.position.y + UnityEngine.Random.Range(0, m_info.targetOffset.y));

                    switch (m_chosenAttack)
                    {
                        case Attack.Attack1:
                            m_animation.EnableRootMotion(true, false);
                            m_attackHandle.ExecuteAttack(m_info.shootAttack.animation, m_info.idleAnimation.animation);
                            break;
                        case Attack.Attack2:
                            m_animation.EnableRootMotion(true, false);
                            //m_attackHandle.ExecuteAttack(m_info.shootDrawAttack.animation, m_info.idleAnimation.animation);
                            m_attackHandle.ExecuteAttack(m_info.shootAttack.animation, m_info.idleAnimation.animation);
                            break;
                        case Attack.Attack3:
                            /*m_flinchHandle.gameObject.SetActive(false);
                            m_attackHandle.ExecuteAttack(m_info.shootComboAttack.animation, m_info.idleAnimation);*/
                            m_animation.EnableRootMotion(false, false);
                            //
                            m_attackHandle.ExecuteAttack(m_info.shootAttack.animation, m_info.idleAnimation.animation);
                            break;
                    }

                    break;

                case State.Cooldown:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            m_stateHandle.SetState(State.Turning);
                    }

                    if (m_currentCD <= m_info.attackCD)
                    {
                        m_currentCD += Time.deltaTime;
                    }
                    else
                    {
                        m_currentCD = 0;
                        m_selfCollider.SetActive(true);
                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                    }

                    break;
                case State.Chasing:
                    {
                        if (IsFacingTarget())
                        {
                            m_attackDecider.DecideOnAttack();
                            m_chosenAttack = m_attackDecider.chosenAttack.attack;

                            if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range) && !m_wallSensor.allRaysDetecting && !ShotBlocked())
                            {
                                m_movement.Stop();
                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                m_attackDecider.hasDecidedOnAttack = false;
                                //if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                                //{
                                //    var distance = Vector2.Distance(m_targetInfo.position, transform.position);
                                //    m_animation.EnableRootMotion(false, false);
                                //    m_animation.SetAnimation(0, distance >= m_info.targetDistanceTolerance ? m_info.move.animation : m_info.patrol.animation, true);
                                //    m_movement.MoveTowards(Vector2.one * transform.localScale.x, distance >= m_info.targetDistanceTolerance ? m_info.move.speed : m_info.patrol.speed);
                                //}
                                //else
                                //{
                                //    m_movement.Stop();
                                //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                //}

                                /*stupod solution*/
                                m_movement.Stop();
                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                            }
                        }
                        else
                        {
                            var xDistance = Mathf.Abs(m_targetInfo.position.x - transform.position.x);
                            m_turnState = State.ReevaluateSituation;
                            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation && xDistance >= 3)
                            {
                                m_stateHandle.SetState(State.Turning);
                            }
                            else
                            {
                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                            }
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
                    if (m_targetInfo.isValid)
                    {
                        if (IsFacing(m_lastTargetPos))
                        {
                            //m_lastTargetPos = new Vector2(m_lastTargetPos.x + m_info.targetOffset.x, m_lastTargetPos.y + m_info.targetOffset.y);
                            m_targetPointIK.transform.position = m_lastTargetPos;
                        }
                        else
                        {
                            if (m_attackDecider.hasDecidedOnAttack)
                            {
                                CustomTurn();
                            }
                        }
                    }
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
            m_currentPatience = 0;
            m_enablePatience = false;
            m_isDetecting = false;
            m_selfCollider.SetActive(false);
        }

        public void SwitchToBattleZoneAI()
        {
            m_battlezonemode = true;
            m_stateHandle.SetState(State.Chasing);
        }

        public void SwitchToBaseAI()
        {
            m_battlezonemode = false;
            m_stateHandle.SetState(State.ReevaluateSituation);
        }

        public void ResetAI()
        {
            m_selfCollider.SetActive(false);
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            m_enablePatience = false;
            m_stateHandle.OverrideState(State.Patrol);
            enabled = true;
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }

        public void HandleKnockback(float resumeAIDelay)
        {
            StopAllCoroutines();
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_knockbackRoutine = StartCoroutine(KnockbackRoutine(resumeAIDelay));
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
            m_knockbackRoutine = null; 
            //enabled = true;
            //m_flinchHandle.enabled = true;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        public override void ReturnToSpawnPoint()
        {
            transform.position = m_startPoint;
        }
    }
}
