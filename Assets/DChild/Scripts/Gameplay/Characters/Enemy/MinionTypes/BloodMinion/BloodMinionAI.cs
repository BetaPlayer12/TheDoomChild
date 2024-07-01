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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/BloodMinion")]
    public class BloodMinionAI : CombatAIBrain<BloodMinionAI.Info>, IResetableAIBrain
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Basic Behaviours
            [SerializeField]
            private MovementInfo m_submergeMove = new MovementInfo();
            public MovementInfo submergeMove => m_submergeMove;

            //Attack Behaviours
            [SerializeField]
            private SimpleAttackInfo m_attack1 = new SimpleAttackInfo();
            public SimpleAttackInfo attack1 => m_attack1;
            [SerializeField, MinValue(0)]
            private float m_attackHitboxDelay;
            public float attackHitboxDelay => m_attackHitboxDelay;
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
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation;
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinch2Animation;
            public BasicAnimationInfo flinch2Animation => m_flinch2Animation;
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_imerseAnimation;
            public BasicAnimationInfo imerseAnimation => m_imerseAnimation;
            [SerializeField]
            private BasicAnimationInfo m_retreatAnimation;
            public BasicAnimationInfo retreatAnimation => m_retreatAnimation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation;
            public BasicAnimationInfo turnAnimation => m_turnAnimation;
            [SerializeField]
            private BasicAnimationInfo m_submergAnimation;
            public BasicAnimationInfo submergAnimation => m_submergAnimation;
            [SerializeField]
            private BasicAnimationInfo m_submergIdleAnimation;
            public BasicAnimationInfo submergIdleAnimation => m_submergIdleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_submergeTurnAnimation;
            public BasicAnimationInfo submergeTurnAnimation => m_submergeTurnAnimation;


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_submergeMove.SetData(m_skeletonDataAsset);
                m_attack1.SetData(m_skeletonDataAsset);

                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_flinch2Animation.SetData(m_skeletonDataAsset);
                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_imerseAnimation.SetData(m_skeletonDataAsset);
                m_retreatAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_submergAnimation.SetData(m_skeletonDataAsset);
                m_submergIdleAnimation.SetData(m_skeletonDataAsset);
                m_submergeTurnAnimation.SetData(m_skeletonDataAsset);
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
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            Attack1,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private GameObject m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private Transform m_flinchFXFollower;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
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
        private bool m_isSubmerged;
        private Vector2 m_startPoint;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;

        [SerializeField, TabGroup("FX")]
        private ParticleSystem m_deathFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_flinchFX;
        [SerializeField, TabGroup("FX")]
        private ParticleSystemRenderer m_flinchFXRenderer;
        [SerializeField, TabGroup("FX")]
        private ParticleSystem m_slashFX;

        [SerializeField, TabGroup("Hurtbox")]
        private GameObject m_attackBB;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private State m_turnState;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            //m_animation.DisableRootMotion();
            m_flinchHandle.m_autoFlinch = true;
            m_isSubmerged = false;
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.SetState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable);
                m_selfCollider.SetActive(false);
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
            m_deathFX.gameObject.SetActive(true);
            m_deathFX.Play();
            base.OnDestroyed(sender, eventArgs);
            m_attackBB.SetActive(false);

            m_movement.Stop();
            m_selfCollider.SetActive(false);
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_targetInfo.isValid)
            {
                if (m_flinchHandle.m_autoFlinch)
                {
                    m_flinchFX.gameObject.SetActive(true);
                    m_attackBB.SetActive(false);
                    m_selfCollider.SetActive(false);
                    StopAllCoroutines();
                    if (!IsFacingTarget())
                    {
                        CustomTurn();
                        m_flinchHandle.SetAnimation(m_info.flinchAnimation.animation);
                    }
                    else
                    {
                        m_flinchHandle.SetAnimation(m_info.flinch2Animation.animation);
                    }
                    var flinchFX = Instantiate(m_flinchFX.gameObject, m_flinchFX.transform.position, Quaternion.identity);
                    flinchFX.transform.GetChild(0).GetComponent<ParticleSystemRenderer>().flip = transform.position.x > m_targetInfo.position.x ? Vector3.zero : Vector3.right;
                    flinchFX.GetComponent<Transform>().localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                    flinchFX.GetComponent<ParticleFX>().Play();

                    //m_flinchFXRenderer.flip = transform.position.x > m_targetInfo.position.x ?  Vector3.zero : Vector3.right;
                    //m_flinchFX.Play();

                    m_animation.SetAnimation(0, IsFacingTarget() ? m_info.flinchAnimation : m_info.flinch2Animation, false);
                    m_animation.AddAnimation(0, m_info.idleAnimation.animation, false, 0.2f).TimeScale = 20;
                    m_stateHandle.OverrideState(State.WaitBehaviourEnd);
                }
            }
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            if (m_flinchHandle.m_autoFlinch)
            {
                m_flinchFX.gameObject.SetActive(false);
                //m_flinchFXFollower.SetParent(this.transform);
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation.animation)
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

        public void ResetAI()
        {
            m_selfCollider.SetActive(false);
            m_targetInfo.Set(null, null);
            m_flinchHandle.m_autoFlinch = true;
            m_isDetecting = false;
            m_enablePatience = false;
            m_stateHandle.OverrideState(State.Patrol);
            enabled = true;
        }

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack1, m_info.attack1.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator AttackRoutine()
        {
            m_flinchHandle.enabled = true;
            m_attackBB.SetActive(true);
            m_selfCollider.SetActive(false);
            m_isSubmerged = false;
            m_hitbox.Enable();
            AIBrainUtility.SetAnimation(m_animation, 0, m_info.imerseAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.imerseAnimation);
            m_hitbox.Disable();
            m_slashFX.gameObject.SetActive(true);
            m_slashFX.GetComponent<ParticleSystemRenderer>().flip = m_character.facing == HorizontalDirection.Right ? Vector3.zero : Vector3.right;
            m_slashFX.Play();
            AIBrainUtility.SetAnimation(m_animation, 0, m_info.attack1, false);
            yield return new WaitForSeconds(m_info.attackHitboxDelay);
            m_hitbox.Enable();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack1.animation);
            m_attackBB.SetActive(false);
            m_selfCollider.SetActive(true);
            m_slashFX.gameObject.SetActive(false);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator RetreatRoutine()
        {
            m_hitbox.Disable();
            m_animation.EnableRootMotion(true, false);
            m_flinchHandle.enabled = false;
            AIBrainUtility.SetAnimation(m_animation, 0, m_info.retreatAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.retreatAnimation);
            AIBrainUtility.SetAnimation(m_animation, 0, m_info.submergIdleAnimation, true);
            //m_hitbox.gameObject.SetActive(true);

            m_isSubmerged = true;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator DetectRoutine()
        {
            m_hitbox.Enable();
            m_flinchHandle.enabled = true;
            AIBrainUtility.SetAnimation(m_animation, 0, m_info.imerseAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.imerseAnimation);
            AIBrainUtility.SetAnimation(m_animation, 0, m_info.idleAnimation, true);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        private IEnumerator SubmerseRoutine()
        {
            AIBrainUtility.SetAnimation(m_animation, 0, m_info.submergAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.submergAnimation);
            AIBrainUtility.SetAnimation(m_animation, 0, m_info.submergIdleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator ImerseRoutine()
        {
            m_hitbox.Enable();
            m_flinchHandle.enabled = true;
            m_animation.SetAnimation(0, m_info.imerseAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.imerseAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        protected override void Start()
        {
            base.Start();
            m_currentTimeScale = UnityEngine.Random.Range(1.0f, 2.0f);
            m_currentFullCD = UnityEngine.Random.Range(m_info.attackCD * .5f, m_info.attackCD * 2f);
            m_hitbox.Disable();
            m_startPoint = transform.position;
            m_deathFX.gameObject.SetActive(false);
            m_slashFX.gameObject.SetActive(false);
            m_flinchFX.gameObject.SetActive(false);
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

                case State.Patrol:
                    m_groundSensor.Cast();
                    m_wallSensor.Cast();
                    if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting)
                    {
                        m_turnState = State.ReevaluateSituation;
                        m_isSubmerged = true;
                        m_animation.EnableRootMotion(true, false);
                        m_animation.SetAnimation(0, m_info.submergeMove.animation, true);
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.centerMass.position, m_character.facing);
                        m_patrolHandle.Patrol(m_movement, m_info.submergeMove.speed, characterInfo);
                    }
                    else
                    {
                        m_movement.Stop();
                        m_animation.SetAnimation(0, m_info.submergIdleAnimation, true);
                    }
                    break;

                case State.Turning:
                    m_movement.Stop();
                    m_stateHandle.Wait(m_turnState);
                    m_animation.DisableRootMotion();
                    m_turnHandle.Execute(m_isSubmerged ? m_info.submergeTurnAnimation.animation : m_info.turnAnimation.animation, m_isSubmerged ? m_info.submergIdleAnimation.animation : m_info.idleAnimation.animation);
                    m_animation.animationState.GetCurrent(0).MixDuration = 0;
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    m_hitbox.enabled = true;
                    m_flinchHandle.m_autoFlinch = false;

                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.Attack1:
                            m_animation.EnableRootMotion(true, false);
                            //m_attackHandle.ExecuteAttack(m_info.attack1.animation, m_info.idleAnimation);
                            StartCoroutine(AttackRoutine());
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
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        if (!m_isSubmerged)
                        {
                            m_stateHandle.Wait(State.Cooldown);
                            StartCoroutine(RetreatRoutine());
                        }
                    }

                    if (m_currentCD <= m_currentFullCD)
                    {
                        m_currentCD += Time.deltaTime;
                    }
                    else
                    {
                        m_currentCD = 0;
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        StartCoroutine(SubmerseRoutine());
                    }

                    break;
                case State.Chasing:
                    {
                        if (IsFacingTarget())
                        {
                            m_attackDecider.DecideOnAttack();
                            if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range))
                            {
                                m_movement.Stop();
                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                m_groundSensor.Cast();
                                m_wallSensor.Cast();
                                m_edgeSensor.Cast();
                                if (!m_wallSensor.isDetecting && m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                                {
                                    //var distance = Vector2.Distance(m_targetInfo.position, transform.position);
                                    m_animation.EnableRootMotion(true, false);
                                    m_selfCollider.SetActive(false);
                                    m_animation.SetAnimation(0, m_info.submergeMove.animation, true).TimeScale = m_currentTimeScale;
                                    //m_movement.MoveTowards(Vector2.one * transform.localScale.x, distance >= m_info.targetDistanceTolerance ? m_info.move.speed : m_info.patrol.speed);
                                }
                                else
                                {
                                    m_movement.Stop();
                                    m_selfCollider.SetActive(true);
                                    if (m_animation.animationState.GetCurrent(0).Animation.ToString() == m_info.submergeMove.animation)
                                    {
                                        m_isSubmerged = false;
                                        m_hitbox.Enable();
                                        m_animation.SetAnimation(0, m_info.imerseAnimation, false);
                                    }
                                    else
                                    {
                                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                                            m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                    }
                                }
                            }
                        }
                        else
                        {
                            m_turnState = State.ReevaluateSituation;
                            m_isSubmerged = m_animation.animationState.GetCurrent(0).Animation.ToString() == m_info.idleAnimation.animation ? false : true;
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
            m_selfCollider.SetActive(false);
        }

        public override void ReturnToSpawnPoint()
        {
            transform.position = m_startPoint;
        }

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
            m_stateHandle.OverrideState(State.Patrol);
        }
    }
}
