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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/MushroomGiant")]
    public class MushroomGiantAI : CombatAIBrain<MushroomGiantAI.Info>
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
            private SimpleAttackInfo m_attack1 = new SimpleAttackInfo();
            public SimpleAttackInfo attack1 => m_attack1;
            [SerializeField]
            private SimpleAttackInfo m_attack2 = new SimpleAttackInfo();
            public SimpleAttackInfo attack2 => m_attack2;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_attack2_loop;
            public string attack2_loop => m_attack2_loop;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_attack2_loop2;
            public string attack2_loop2 => m_attack2_loop2;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_attack2_bounce;
            public string attack2_bounce => m_attack2_bounce;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_attack2_end;
            public string attack2_end => m_attack2_end;
            [SerializeField]
            private float m_attack2AirTime;
            public float attack2AirTime => m_attack2AirTime;
            [SerializeField]
            private Vector2 m_attack2Velocity;
            public Vector2 attack2Velocity => m_attack2Velocity;
            [SerializeField]
            private Vector2 m_attack2Bounce2Velocity;
            public Vector2 attack2Bounce2Velocity => m_attack2Bounce2Velocity;
            [SerializeField]
            private float m_attackCD;
            public float attackCD => m_attackCD;
            //[SerializeField]
            //private float m_attack2ThonkTime;
            //public float attack2ThonkTime => m_attack2ThonkTime;
            //[SerializeField]
            //private float m_attack2DropSpeed;
            //public float attack2DropSpeed => m_attack2DropSpeed;
            //[SerializeField]
            //private float m_attack2OverheadOffset;
            //public float attack2OverheadOffset => m_attack2OverheadOffset;
            [SerializeField]
            private SimpleAttackInfo m_attack3 = new SimpleAttackInfo();
            public SimpleAttackInfo attack3 => m_attack3;
            //

            //[SerializeField, ValueDropdown("GetAnimations")]
            //private string m_detectAnimation;
            //public string detectAnimation => m_detectAnimation;
            [SerializeField, MinValue(0)]
            private float m_detectionTime;
            public float detectionTime => m_detectionTime;
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
            private BasicAnimationInfo m_flinchAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;
            //[SerializeField, ValueDropdown("GetAnimations")]
            //private string m_turnAnimation;
            //public string turnAnimation => m_turnAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo deathAnimation => m_deathAnimation;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_breathEvent;
            public string breathEvent => m_breathEvent;


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_attack1.SetData(m_skeletonDataAsset);
                m_attack2.SetData(m_skeletonDataAsset);
                //m_attack2_upward.SetData(m_skeletonDataAsset);
                m_attack3.SetData(m_skeletonDataAsset);

                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Detect,
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
            //Attack2Downward,
            //Attack2Upward,
            Attack3,
            [HideInInspector]
            _COUNT
        }
        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_attackBB;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_bodyCollider;
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
        //Patience Handler
        private float m_currentPatience;
        private float m_currentCD;
        private float m_currentFullCD;
        private float m_currentTimeScale;
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
        private ParticleFX m_breathFX;
        //[SerializeField, TabGroup("Territory")]
        //private Collider2D m_territoryCollider;
        //[SerializeField, TabGroup("Renderer")]
        //private MeshRenderer m_mRendererer;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private State m_turnState;
        private Coroutine m_bbRoutine;


        //[SerializeField]
        //private AudioSource m_Audiosource;
        //[SerializeField]
        //private AudioClip m_AttackClip;
        //[SerializeField]
        //private AudioClip m_DeadClip;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            GetComponent<IsolatedCharacterPhysics2D>().UseStepClimb(true);
            m_breathFX.gameObject.GetComponent<ParticleSystem>().Stop();
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
                m_currentCD = 0;
                m_enablePatience = false;
                m_animation.animationState.TimeScale = 1f;
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
            base.OnDestroyed(sender, eventArgs);
            
            StopAllCoroutines();
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.deathAnimation, false);
            m_movement.Stop();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                //m_mRendererer.material.SetFloat("Highlight", 1);
                StopAllCoroutines();
                //m_hitbox.SetInvulnerability(false);
                //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                m_stateHandle.Wait(m_targetInfo.isValid ? State.Cooldown : State.ReevaluateSituation);
            }
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation.animation)
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
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack3, m_info.attack1.range),/**/
                                    //new AttackInfo<Attack>(Attack.Attack1, m_info.attack3.range),
                                    new AttackInfo<Attack>(Attack.Attack2, m_info.attack2.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {
            m_animation.SetAnimation(0, m_info.attack3.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack3.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            //yield return new WaitForSeconds(m_info.detectionTime);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }


        private IEnumerator Attack2Routine()
        {
            //m_stateHandle.Wait(State.ReevaluateSituation);
            m_movement.Stop();
            m_selfCollider.enabled = false;
            m_animation.SetAnimation(0, m_info.attack2.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack2.animation);
            m_animation.SetAnimation(0, m_info.attack2_loop, true);
            var bounceDir = new Vector2(m_info.attack2Velocity.x * transform.localScale.x, m_info.attack2Velocity.y);
            m_character.physics.SetVelocity(bounceDir);
            m_flinchHandle.gameObject.SetActive(false);
            m_bodyCollider.enabled = false;
            //m_hitbox.SetInvulnerability(true);
            //m_hitbox.gameObject.SetActive(false);
            yield return new WaitForSeconds(m_info.attack2AirTime);
            yield return new WaitUntil(() => m_groundSensor.isDetecting);
            m_animation.SetAnimation(0, m_info.attack2_bounce, false);
            m_bodyCollider.enabled = true;
            yield return new WaitForSeconds(.75f);
            m_character.physics.SetVelocity(new Vector2(m_info.attack2Bounce2Velocity.x * transform.localScale.x, m_info.attack2Bounce2Velocity.y));
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack2_bounce);
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.attack2_end, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack2_end);
            if (!m_wallSensor.isDetecting)
            {
                GetComponent<IsolatedCharacterPhysics2D>().UseStepClimb(true);
            }
            m_flinchHandle.gameObject.SetActive(true);
            m_flinchHandle.m_autoFlinch = true;
            //m_hitbox.SetInvulnerability(false);
            m_attackDecider.hasDecidedOnAttack = false;
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }


        protected override void Start()
        {
            base.Start();
            m_spineEventListener.Subscribe(m_info.breathEvent, PoisonBreath);
            m_currentTimeScale = UnityEngine.Random.Range(1.0f, 2.0f);
            m_currentFullCD = UnityEngine.Random.Range(m_info.attackCD * .5f, m_info.attackCD * 2f);
            //GameplaySystem.SetBossHealth(m_character);
            m_startpoint = transform.position;
        }

        private void PoisonBreath()
        {
            if (m_stateHandle.currentState != State.Detect)
            {
                m_breathFX.Play();
                m_bbRoutine = StartCoroutine(PoisonBreathBBRoutine());
            }
        }

        private IEnumerator PoisonBreathBBRoutine()
        {
            yield return new WaitForSeconds(1f);
            m_attackBB.SetActive(true);
            m_bbRoutine = null;
            yield return null;
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
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();
        }


        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Edge Sensor is " + m_edgeSensor.isDetecting);
            //Debug.Log("Highlight is: " + m_mRendererer.material.GetFloat("Highlight"));
            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    m_movement.Stop();
                    m_flinchHandle.m_autoFlinch = false;
                    if (IsFacingTarget())
                    {
                        //m_stateHandle.Wait(State.ReevaluateSituation);
                        StartCoroutine(DetectRoutine());
                    }
                    else
                    {
                        m_turnState = State.Detect;
                        //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.Patrol:
                    if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting)
                    {
                        m_turnState = State.ReevaluateSituation;
                        m_animation.EnableRootMotion(true, false);
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
                    m_turnHandle.Execute();
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);

                    switch (m_attackDecider.chosenAttack.attack)
                    {
                    //    case Attack.Attack1:
                    //        m_animation.EnableRootMotion(true, false);
                    //        m_attackHandle.ExecuteAttack(m_info.attack1.animation, m_info.idleAnimation.animation);
                    //        break;
                        case Attack.Attack2:
                            m_animation.EnableRootMotion(false, false);
                            //m_attackHandle.ExecuteAttack(m_info.attack2_downward.animation, m_info.idleAnimation);
                            StartCoroutine(Attack2Routine());
                            break;
                        case Attack.Attack3:
                            m_animation.EnableRootMotion(true, false);
                            m_attackHandle.ExecuteAttack(m_info.attack3.animation, m_info.idleAnimation.animation);
                            break;
                    }
                    m_attackDecider.hasDecidedOnAttack = false;

                    break;

                case State.Cooldown:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                    m_attackBB.SetActive(false);
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
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
                case State.Chasing:
                    {
                        m_flinchHandle.m_autoFlinch = false;
                        if (IsFacingTarget())
                        {
                            m_attackDecider.DecideOnAttack();
                            if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range) && !m_wallSensor.allRaysDetecting)
                            {
                                GetComponent<IsolatedCharacterPhysics2D>().UseStepClimb(true);
                                m_movement.Stop();
                                m_selfCollider.enabled = true;
                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                                {
                                    GetComponent<IsolatedCharacterPhysics2D>().UseStepClimb(true);
                                    m_animation.EnableRootMotion(true, false);
                                    m_selfCollider.enabled = false;
                                    m_animation.SetAnimation(0, m_info.move.animation, true).TimeScale = m_currentTimeScale;
                                    //m_movement.MoveTowards(m_targetInfo.position, m_info.move.speed * transform.localScale.x);
                                }
                                else
                                {
                                    GetComponent<IsolatedCharacterPhysics2D>().UseStepClimb(false);
                                    m_movement.Stop();
                                    m_selfCollider.enabled = true;
                                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                }
                            }
                        }
                        else
                        {
                            m_turnState = State.ReevaluateSituation;
                            //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
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
            m_currentCD = 0;
            m_enablePatience = false;
            m_isDetecting = false;
            m_selfCollider.enabled = false;
        }

        public void ResetAI()
        {
            m_selfCollider.enabled = false;
            m_targetInfo.Set(null, null);
            m_flinchHandle.m_autoFlinch = true;
            m_isDetecting = false;
            m_enablePatience = false;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            enabled = true;
        }

        public override void ReturnToSpawnPoint()
        {
            transform.position = m_startpoint;
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }
    }
}
