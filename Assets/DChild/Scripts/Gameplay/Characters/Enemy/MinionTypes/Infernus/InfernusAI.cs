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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/Infernus")]
    public class InfernusAI : CombatAIBrain<InfernusAI.Info>, IResetableAIBrain, IKnockbackable
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Basic Behaviours
            [SerializeField]
            private MovementInfo m_patrol = new MovementInfo();
            public MovementInfo patrol => m_patrol;
            [SerializeField]
            private MovementInfo m_patrolBackwards = new MovementInfo();
            public MovementInfo patrolBackwards => m_patrolBackwards;
            [SerializeField]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;
            [SerializeField]
            private MovementInfo m_moveBackwards = new MovementInfo();
            public MovementInfo moveBackwards => m_moveBackwards;

            //Attack Behaviours
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
            private string m_idle2Animation;
            public string idle2Animation => m_idle2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_detectAnimation;
            public string detectAnimation => m_detectAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;

            [SerializeField]
            private SimpleProjectileAttackInfo m_projectile;
            public SimpleProjectileAttackInfo projectile => m_projectile;
            [SerializeField]
            private float m_launchInterval;
            public float launchInterval => m_launchInterval;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_flameoffEvent;
            public string flameoffEvent => m_flameoffEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_fireballEvent;
            public string fireballEvent => m_fireballEvent;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_patrolBackwards.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_moveBackwards.SetData(m_skeletonDataAsset);
                m_projectile.SetData(m_skeletonDataAsset);
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
            //AttackMelee,
            AttackRange,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_selfCollider;
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

        private Vector2 m_lastTargetPos;
        private float m_currentPatience;
        private float m_currentCD;
        private float m_currentFullCD;
        private float m_currentMoveSpeed;
        private float m_currentBackMoveSpeed;
        private float m_currentRunAttackDuration;
        private bool m_enablePatience;
        private bool m_isDetecting;
        private Vector2 m_startPoint;
        private Coroutine m_mobileAttackCoroutine;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_backSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_backEdgeSensor;

        [SerializeField, TabGroup("FX")]
        private ParticleSystem m_flameFX;
        [SerializeField, TabGroup("FX")]
        private ParticleSystem m_muzzleFX;

        [SerializeField]
        private bool m_willPatrol;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private ProjectileLauncher m_projectileLauncher;
        [SerializeField]
        private Transform m_projectileStart;

        private State m_turnState;


        //[SerializeField]
        //private AudioSource m_Audiosource;
        //[SerializeField]
        //private AudioClip m_AttackClip;
        //[SerializeField]
        //private AudioClip m_DeadClip;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            //m_animation.DisableRootMotion();
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
            m_animation.SetAnimation(0, m_info.idleAnimation, true).MixDuration = 0;
            m_animation.DisableRootMotion();
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
            
            m_character.physics.UseStepClimb(true);
            m_movement.Stop();
            m_selfCollider.enabled = false;
            if (m_mobileAttackCoroutine != null)
            {
                StopCoroutine(m_mobileAttackCoroutine);
                m_mobileAttackCoroutine = null;
            }
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idleAnimation && m_targetInfo.isValid)
            {
                m_flinchHandle.m_autoFlinch = true;
                m_selfCollider.enabled = false;
                StopAllCoroutines();
                //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                m_stateHandle.Wait(State.ReevaluateSituation);
            }
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                m_flinchHandle.m_autoFlinch = false;
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation)
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                m_stateHandle.OverrideState(m_targetInfo.isValid ? State.Attacking : State.Patrol);
                if (m_targetInfo.isValid && !IsFacingTarget())
                    CustomTurn();
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
            m_attackDecider.SetList(/*new AttackInfo<Attack>(Attack.AttackMelee, m_info.attackMelee.range),*/new AttackInfo<Attack>(Attack.AttackRange, m_info.projectile.range)/**/);
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        private IEnumerator LaunchProjectilRoutine()
        {
            var projectilePos = new Vector2(m_projectileStart.position.x + (50 * transform.localScale.x), m_projectileStart.position.y - 20f);
            /*for (int i = 0; i < 3; i++)
            {
                projectilePos = new Vector2(projectilePos.x, projectilePos.y + 10f);
                m_projectileLauncher.AimAt(projectilePos);
                m_projectileLauncher.LaunchProjectile();
            }*/
            projectilePos = new Vector2(projectilePos.x, projectilePos.y + 20f);
            m_projectileLauncher.AimAt(projectilePos);
            m_projectileLauncher.LaunchProjectile();/*
            yield return new WaitForSeconds(0.8f);
            m_projectileLauncher.AimAt(projectilePos);
            m_projectileLauncher.LaunchProjectile();*/
            yield return null;
        }

        private IEnumerator MobileAttackRoutine()
        {
            yield return new WaitForSeconds(0.25f);
            while (true)
            {
                LaunchProjectile();
                yield return new WaitForSeconds(m_info.launchInterval);
                yield return null;
            }
        }

        private void LaunchProjectile()
        {
            if (m_targetInfo.isValid)
            {
                m_muzzleFX.Play();
                StartCoroutine(LaunchProjectilRoutine());
                //m_targetPointIK.transform.position = m_lastTargetPos;
            }
        }

        private float GroundDistance()
        {
            RaycastHit2D hit = Physics2D.Raycast(m_character.centerMass.position, Vector2.down, 1000, DChildUtility.GetEnvironmentMask());
            if (hit.collider != null)
            {
                return hit.distance;
            }

            return 0;
        }

        private void CustomTurn()
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
        }

        protected override void Start()
        {
            base.Start();
            m_currentMoveSpeed = UnityEngine.Random.Range(m_info.move.speed * .75f, m_info.move.speed * 1.25f);
            m_currentBackMoveSpeed = UnityEngine.Random.Range(m_info.moveBackwards.speed * .75f, m_info.moveBackwards.speed * 1.25f);
            m_currentFullCD = UnityEngine.Random.Range(m_info.attackCD * .5f, m_info.attackCD * 2f);
            m_spineEventListener.Subscribe(m_info.projectile.launchOnEvent, LaunchProjectile);
            m_spineEventListener.Subscribe(m_info.flameoffEvent, m_flameFX.Stop);
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
            m_projectileLauncher = new ProjectileLauncher(m_info.projectile.projectileInfo, m_projectileStart);
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
                    m_movement.Stop();
                    if (IsFacingTarget())
                    {
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        StartCoroutine(DetectRoutine());
                    }
                    else
                    {
                        m_turnState = State.Detect;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
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
                    if (m_mobileAttackCoroutine != null)
                    {
                        StopCoroutine(m_mobileAttackCoroutine);
                        m_mobileAttackCoroutine = null;
                    }
                    m_animation.EnableRootMotion(true, false);
                    m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                    m_animation.animationState.GetCurrent(0).MixDuration = 0;
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);


                    m_animation.EnableRootMotion(true, false);
                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.AttackRange:
                            if (m_targetInfo.isValid)
                            {
                                m_lastTargetPos = m_targetInfo.position;
                            }
                            m_attackHandle.ExecuteAttack(m_info.projectile.animation, m_info.idleAnimation);
                            break;
                    }
                    m_attackDecider.hasDecidedOnAttack = false;

                    break;

                case State.Cooldown:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        var animation = UnityEngine.Random.Range(0, 100) > 2 ? m_info.idleAnimation : m_info.idle2Animation;
                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                        {
                            m_animation.SetAnimation(0, animation, true);
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
                            if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range) && !m_wallSensor.allRaysDetecting && Vector2.Distance(m_targetInfo.position, transform.position) > (m_attackDecider.chosenAttack.range - 5f))
                            {
                                m_movement.Stop();
                                if (m_mobileAttackCoroutine != null)
                                {
                                    StopCoroutine(m_mobileAttackCoroutine);
                                    m_mobileAttackCoroutine = null;
                                }
                                m_selfCollider.enabled = true;
                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                m_animation.DisableRootMotion();
                                if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                                {
                                    if (m_mobileAttackCoroutine == null && IsTargetInRange(m_attackDecider.chosenAttack.range)&& Vector2.Distance(m_targetInfo.position, transform.position) > 50)
                                    {
                                        m_mobileAttackCoroutine = StartCoroutine(MobileAttackRoutine());
                                    }
                                    else if(Vector2.Distance(m_targetInfo.position, transform.position) < 50)
                                    {
                                        m_movement.Stop();
                                        if (m_mobileAttackCoroutine != null)
                                        {
                                            StopCoroutine(m_mobileAttackCoroutine);
                                            m_mobileAttackCoroutine = null;
                                        }
                                        m_selfCollider.enabled = true;
                                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                        m_stateHandle.SetState(State.Attacking);
                                        return;
                                    }
                                    /*if (Vector2.Distance(m_targetInfo.position, transform.position) < 50)
                                    {
                                        //if (m_backSensor.isDetecting || !m_backEdgeSensor.isDetecting)
                                        //{
                                            m_movement.Stop();
                                            if (m_mobileAttackCoroutine != null)
                                            {
                                                StopCoroutine(m_mobileAttackCoroutine);
                                                m_mobileAttackCoroutine = null;
                                            }
                                            m_selfCollider.enabled = true;
                                            m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                            m_stateHandle.SetState(State.Attacking);
                                            return;
                                        //}
                                        //m_selfCollider.enabled = false;
                                        //m_animation.SetAnimation(0, m_info.moveBackwards.animation, true);
                                        //m_movement.MoveTowards(Vector2.one * -transform.localScale.x, m_currentBackMoveSpeed);
                                    }
                                    else
                                    {*/
                                        m_selfCollider.enabled = false;
                                        m_animation.SetAnimation(0, m_info.move.animation, true);
                                        m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_currentMoveSpeed);
                                    //}
                                }
                                else
                                {
                                    m_movement.Stop();
                                    if (m_mobileAttackCoroutine != null)
                                    {
                                        StopCoroutine(m_mobileAttackCoroutine);
                                        m_mobileAttackCoroutine = null;
                                    }
                                    if (m_backSensor.isDetecting || !m_backEdgeSensor.isDetecting)
                                    {
                                        m_selfCollider.enabled = true;
                                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                        m_stateHandle.SetState(State.Attacking);
                                        return;
                                    }
                                    var animation = UnityEngine.Random.Range(0, 100) > 2 ? m_info.idleAnimation : m_info.idle2Animation;
                                    if (m_animation.animationState.GetCurrent(0).IsComplete)
                                    {
                                        m_animation.SetAnimation(0, animation, true);
                                    }
                                }
                            }
                        }
                        else
                        {
                            m_turnState = State.ReevaluateSituation;
                            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
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

            if (m_enablePatience)
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
            m_selfCollider.enabled = false;
        }

        public void ResetAI()
        {
            m_selfCollider.enabled = false;
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            m_enablePatience = false;
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
            m_character.physics.UseStepClimb(false);
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation)
            {
                //m_flinchHandle.enabled = false;
                m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
            yield return new WaitForSeconds(timer);
            m_character.physics.UseStepClimb(true);
            //enabled = true;
            //m_flinchHandle.enabled = true;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }
    }
}
