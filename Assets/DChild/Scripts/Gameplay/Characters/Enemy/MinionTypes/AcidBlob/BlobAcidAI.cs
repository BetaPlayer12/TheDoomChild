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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/BlobAcid")]
    public class BlobAcidAI : CombatAIBrain<BlobAcidAI.Info>
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
            private SimpleAttackInfo m_attack = new SimpleAttackInfo();
            public SimpleAttackInfo attack => m_attack;
            [SerializeField]
            private float m_attackCD;
            public float attackCD => m_attackCD;
            //

            [SerializeField, MinValue(0)]
            private Vector2 m_spawnVelocity;
            public Vector2 spawnVelocity => m_spawnVelocity;
            [SerializeField, MinValue(0)]
            private float m_patience;
            public float patience => m_patience;
            [SerializeField, MinValue(0)]
            private float m_expandSpeed;
            public float expandSpeed => m_expandSpeed;

            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;


            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_spawnAnimation;
            public string spawnAnimation => m_spawnAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_sleepAnimation;
            public string sleepAnimation => m_sleepAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_detectAnimation;
            public string detectAnimation => m_detectAnimation;
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
            private string m_rotateToEdgeAnimation;
            public string rotateToEdgeAnimation => m_rotateToEdgeAnimation;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_smokeEvent;
            public string smokeEvent => m_smokeEvent;


            //[SerializeField]
            //private GameObject m_spitProjectile;
            //public GameObject spitProjectile => m_spitProjectile;


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_attack.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Spawn,
            Sleep,
            Patrol,
            Detect,
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

        [SerializeField, TabGroup("Modules")]
        private TransformTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Modules")]
        private PlatformPatrol m_platformPatrol;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        //Patience Handler
        private float m_currentPatience;
        private bool m_enablePatience;

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_attackbb;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_aggroSensor;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private Transform m_model;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_smokeFX;
        [SerializeField]
        private bool m_willPatrol;

        private float m_targetDistance;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private State m_turnState;

        private bool m_isDetecting;
        private bool m_isRotating;
        private float m_currentCD;
        private float m_currentScale;
        private Coroutine m_attackRoutine;

        protected override void Start()
        {
            base.Start();
            transform.localScale = m_willPatrol ? Vector3.one : Vector3.zero;
            m_selfCollider.SetActive(false);
            m_spineEventListener.Subscribe(m_info.smokeEvent, SmokeFX);
        }

        private void SmokeFX()
        {
            m_smokeFX.Play();
            m_attackRoutine = StartCoroutine(AttackbbRoutine());

        }

        private IEnumerator AttackbbRoutine()
        {
            m_attackbb.enabled = true;
            m_attackbb.transform.localScale = Vector3.one;
            while (m_attackbb.transform.localScale.x <= 2f)
            {
                var offset = Time.deltaTime * .5f;
                m_attackbb.transform.localScale += new Vector3(m_attackbb.transform.localScale.x * offset, m_attackbb.transform.localScale.y * offset);
                yield return null;
            }
            //yield return new WaitForSeconds(1f);
            m_attackbb.enabled = false;
            yield return null;
        }

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_animation.animationState.TimeScale = 1;
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public void SetTargetInfo(AITargetInfo targetInfo)
        {
            m_enablePatience = true;
            m_targetInfo = targetInfo;
        }

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                m_selfCollider.SetActive(true);
                m_currentPatience = 0;
                m_enablePatience = false;
                //StopCoroutine(PatienceRoutine()); //for latur
                if (m_stateHandle.currentState != State.Chasing && !m_isDetecting)
                {
                    m_isDetecting = true;
                    m_stateHandle.SetState(State.Detect);
                }
                if (transform.localRotation.z != 0)
                {
                    transform.localRotation = Quaternion.Euler(Vector3.zero);
                    GetComponent<IsolatedPhysics2D>().simulateGravity = true;
                    m_animation.DisableRootMotion();
                }
            }
            else
            {
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
                m_stateHandle.SetState(State.Sleep);
            }
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            base.OnDestroyed(sender, eventArgs);
            if (m_attackRoutine != null)
            {
                StopCoroutine(m_attackRoutine);
            }
            m_movement.Stop();
        }

        private void CustomTurn()
        {
            if (!IsFacingTarget())
            {
                transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
                m_character.SetFacing(transform.localScale.x > 0 ? HorizontalDirection.Right : HorizontalDirection.Left);
            }
        }

        private IEnumerator SpawnRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_animation.DisableRootMotion();
            m_isDetecting = true;
            m_animation.SetAnimation(0, m_info.sleepAnimation, true);
            while (m_currentScale <= 1)
            {
                m_currentScale += Time.deltaTime * m_info.expandSpeed;
                transform.localScale = new Vector3(m_currentScale, m_currentScale, 1);
                yield return null;
            }
            CustomTurn();
            m_currentScale = 0;
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.sleepAnimation);
            m_animation.SetAnimation(0, m_info.detectAnimation, false).TimeScale = 3f;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_animation.SetAnimation(0, m_info.spawnAnimation, false);
            yield return new WaitForSeconds(0.1f);
            m_character.physics.SetVelocity(new Vector2(m_info.spawnVelocity.x * transform.localScale.x, m_info.spawnVelocity.y));
            yield return new WaitForSeconds(0.25f);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spawnAnimation);
            yield return new WaitUntil(() => m_groundSensor.isDetecting);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator DetectRoutine()
        {
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator RotateToEdgeRoutine()
        {
            m_animation.DisableRootMotion();
            m_character.physics.simulateGravity = false;
            m_character.physics.SetVelocity(Vector2.zero);
            //m_model.localScale = new Vector2(-m_model.localScale.x, m_model.localScale.y);
            m_animation.SetAnimation(0, m_info.rotateToEdgeAnimation, true);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rotateToEdgeAnimation);
            GetComponentInChildren<SkeletonAnimation>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            m_platformPatrol.ExecuteAutoRotate();
            yield return new WaitForSeconds(.05f);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            //m_model.localScale = new Vector2(-m_model.localScale.x, m_model.localScale.y);
            m_isRotating = false;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private void OnRotateToEdge(object sender, EventActionArgs eventArgs)
        {
            if (!m_isRotating)
            {
                m_isRotating = true;
                m_stateHandle.Wait(State.Patrol);
                StartCoroutine(RotateToEdgeRoutine());
            }
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idleAnimation)
            {
                m_flinchHandle.m_autoFlinch = true;
                StopAllCoroutines();
                //m_animation.SetAnimation(0, m_info.damageAnimation, false);
                m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            }
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                m_flinchHandle.m_autoFlinch = false;
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation)
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
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

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack, m_info.attack.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        protected override void Awake()
        {
            base.Awake();
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            if (!m_willPatrol)
            {
                m_flinchHandle.FlinchStart += OnFlinchStart;
                m_flinchHandle.FlinchEnd += OnFlinchEnd;
            }
            else
            {
                m_flinchHandle.gameObject.SetActive(false);
                m_platformPatrol.TurnRequest += OnTurnRequest;
                m_platformPatrol.RotateToEdge += OnRotateToEdge;
                m_aggroSensor.enabled = false;
            }
            m_stateHandle = new StateHandle<State>(m_willPatrol ? State.Patrol : State.Spawn, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            //m_projectileLauncher = new ProjectileLauncher(m_info.projectile.projectileInfo, m_throwPoint);
            UpdateAttackDeciderList();
        }


        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Ground Sensor is " + m_groundSensor.isDetecting);
            switch (m_stateHandle.currentState)
            {
                case State.Spawn:
                    m_movement.Stop();
                    //m_character.physics.simulateGravity = true;
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    StartCoroutine(SpawnRoutine());
                    break;

                case State.Detect:
                    //m_character.physics.simulateGravity = true;
                    m_movement.Stop();
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    StartCoroutine(DetectRoutine());
                    break;

                case State.Patrol:
                    GetComponentInChildren<SkeletonAnimation>().maskInteraction = SpriteMaskInteraction.None;
                    m_animation.SetAnimation(0, m_info.patrol.animation, true).TimeScale = 1f;
                    var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);

                    if (transform.eulerAngles.z == 0 || transform.eulerAngles.z == 180)
                    {
                        //m_platformPatrol.Patrol(m_movement, m_info.move.speed, characterInfo);
                        var moveSpeedX = transform.localEulerAngles.z != 180 ? m_info.move.speed : -m_info.move.speed;
                        m_character.physics.SetVelocity(moveSpeedX * transform.localScale.x, 0);
                    }
                    else
                    {
                        var moveSpeedY = m_info.move.speed * GetComponent<IsolatedCharacterPhysics2D>().moveAlongGround.y;
                        m_character.physics.SetVelocity(0, moveSpeedY * transform.localScale.x);
                    }
                    break;

                case State.Sleep:
                    m_animation.SetAnimation(0, m_info.sleepAnimation, true);
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute();
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);

                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.Attack:
                            m_animation.EnableRootMotion(false, false);
                            m_animation.animationState.TimeScale = .5f;
                            m_attackHandle.ExecuteAttack(m_info.attack.animation, m_info.idleAnimation);
                            break;
                    }
                    m_attackDecider.hasDecidedOnAttack = false;

                    break;

                case State.Cooldown:
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        m_stateHandle.SetState(State.Turning);
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
                        if (IsFacingTarget())
                        {
                            m_attackDecider.DecideOnAttack();
                            if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range) && !m_wallSensor.allRaysDetecting)
                            {
                                m_movement.Stop();
                                m_animation.SetAnimation(0, m_info.idleAnimation, false);
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting /*&& m_edgeSensor.isDetecting*/)
                                {
                                    m_animation.EnableRootMotion(false, false);
                                    m_animation.SetAnimation(0, m_info.move.animation, true);
                                    m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.move.speed);
                                }
                                else
                                {
                                    m_attackDecider.hasDecidedOnAttack = false;
                                    m_movement.Stop();
                                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                }
                            }
                        }
                        else
                        {
                            m_turnState = State.ReevaluateSituation;
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
                        m_stateHandle.SetState(State.Sleep);
                    }
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }

            if (m_enablePatience)
            {
                Patience();
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Sleep);
            m_currentPatience = 0;
            m_enablePatience = false;
            m_isDetecting = false;
            m_selfCollider.SetActive(false);
        }

        public void ResetAI()
        {
            m_selfCollider.SetActive(false);
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
            m_enablePatience = false;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            enabled = true;
        }

        protected override void OnBecomePassive()
        {
            ResetAI();
        }
    }
}
