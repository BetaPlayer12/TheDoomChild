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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/SavepointMimic")]
    public class SavepointMimicAI : CombatAIBrain<SavepointMimicAI.Info>, IResetableAIBrain, IKnockbackable
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
           

            //Attack Behaviours
            [SerializeField]
            private SimpleAttackInfo m_attack = new SimpleAttackInfo();
            public SimpleAttackInfo attack => m_attack;
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
            private BasicAnimationInfo m_idleAggroAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo idleAggroAnimation => m_idleAggroAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleUnAggroAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo idleUnAggroAnimation => m_idleUnAggroAnimation;
            [SerializeField]
            private BasicAnimationInfo m_UnAggroAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo UnAggroAnimation => m_UnAggroAnimation;
            [SerializeField]
            private BasicAnimationInfo m_detectAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo detectAnimation => m_detectAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo turnAnimation => m_turnAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation = new BasicAnimationInfo();
            public BasicAnimationInfo deathAnimation => m_deathAnimation;

            public override void Initialize()
            {
#if UNITY_EDITOR
                
                m_attack.SetData(m_skeletonDataAsset);

                m_idleAggroAnimation.SetData(m_skeletonDataAsset);
                m_idleUnAggroAnimation.SetData(m_skeletonDataAsset);
                m_UnAggroAnimation.SetData(m_skeletonDataAsset);
                m_detectAnimation.SetData(m_skeletonDataAsset);
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
            Turning,
            Attacking,
            Chasing,
            Cooldown,
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

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private Rigidbody2D m_rb2d;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_selfCollider;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_shadow;
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
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private State m_turnState;
        private int m_detectlimit;
        private int m_detectcount;


        //[SerializeField]
        //private AudioSource m_Audiosource;
        //[SerializeField]
        //private AudioClip m_AttackClip;
        //[SerializeField]
        //private AudioClip m_DeadClip;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_character.physics.UseStepClimb(true);
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
        public void InteractDetect()
        {
            m_detectlimit = UnityEngine.Random.Range(2, 5);
            m_detectcount = 0;
            m_movement.Stop();
            m_stateHandle.Wait(State.ReevaluateSituation);
            StartCoroutine(DetectRoutine());
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
                m_stateHandle.Wait(State.Idle);
                m_selfCollider.enabled = false;
                m_targetInfo.Set(null, null);
                m_isDetecting = false;
                m_enablePatience = false;
                m_hitbox.gameObject.SetActive(false);
                StartCoroutine(SinkRoutine());
            }
        }

        private IEnumerator SinkRoutine()
        {
            m_shadow.SetActive(false);
            m_animation.SetAnimation(0, m_info.UnAggroAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.UnAggroAnimation);
            m_animation.SetAnimation(0, m_info.idleUnAggroAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
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
            m_animation.EnableRootMotion(true, false);
            StopAllCoroutines();
            m_character.physics.UseStepClimb(false);
            base.OnDestroyed(sender, eventArgs);
            
            m_rb2d.sharedMaterial = null;
            m_movement.Stop();
            m_selfCollider.enabled = false;
            m_shadow.SetActive(false);
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            Debug.Log("DO THE RAWR");
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.attack.animation)
            {
                m_rb2d.sharedMaterial = null;
                m_character.physics.UseStepClimb(false);
                StopAllCoroutines();
                m_selfCollider.enabled = false;
                m_animation.SetAnimation(0, m_info.idleAggroAnimation, false);
                StartCoroutine(FlinchRoutine());
                m_stateHandle.Wait(m_targetInfo.isValid ? State.Cooldown : State.ReevaluateSituation);
            }
        }

        private IEnumerator FlinchRoutine()
        {
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
            m_animation.SetAnimation(0, m_info.idleAggroAnimation, false);
            m_character.physics.UseStepClimb(true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.attack.animation)
            //{
            //    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.death1Animation || m_animation.GetCurrentAnimation(0).ToString() != m_info.death2Animation)
            //        m_animation.SetAnimation(0, m_info.idleAnimation, true);
            //    m_stateHandle.OverrideState(State.ReevaluateSituation);
            //}
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
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack, m_info.attack.range)/**/);
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private IEnumerator DetectRoutine()
        {
            m_shadow.SetActive(true);
            m_animation.SetAnimation(0, m_info.detectAnimation, false);
            yield return new WaitForSeconds(.25f);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_hitbox.gameObject.SetActive(true);
            m_animation.SetAnimation(0, m_info.idleAggroAnimation, true);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        private IEnumerator AttackRoutine()
        {
            m_selfCollider.enabled = false;
            m_animation.SetAnimation(0, m_info.attack.animation, false);
            m_animation.EnableRootMotion(true, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack.animation);
            m_rb2d.sharedMaterial = null;
            m_animation.SetAnimation(0, m_info.idleAggroAnimation, true);
            m_selfCollider.enabled = true;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        protected override void Start()
        {
            base.Start();
            m_hitbox.gameObject.SetActive(false);
            m_currentTimeScale = UnityEngine.Random.Range(1.0f, 2.0f);
            m_currentFullCD = UnityEngine.Random.Range(m_info.attackCD * .5f, m_info.attackCD * 2f);

            m_animation.SetAnimation(0, m_info.idleUnAggroAnimation, false);
            m_startPoint = transform.position;
            //m_spineEventListener.Subscribe(m_info.explodeEvent, m_explodeFX.Play);
        }

        protected override void Awake()
        {
            base.Awake();
            
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            var deathAnim = m_info.deathAnimation;
            m_deathHandle.SetAnimation(deathAnim.animation);
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();
            m_detectlimit = UnityEngine.Random.Range(1, 5);
            m_detectcount = 0;

        }


        private void Update()
        {
            //Debug.Log("Wall Sensor is " + m_wallSensor.isDetecting);
            //Debug.Log("Edge Sensor is " + m_edgeSensor.isDetecting);
            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    if(m_detectlimit == m_detectcount)
                    {
                        m_detectlimit = UnityEngine.Random.Range(1, 5);
                        m_detectcount = 0;
                        m_movement.Stop();
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        StartCoroutine(DetectRoutine());
                    }
                    else
                    {
                        m_detectcount = m_detectcount + 1;
                        m_stateHandle.SetState(State.Idle);
                        m_stateHandle.OverrideState(State.Idle);
                        m_currentPatience = 0;
                        m_enablePatience = false;
                        m_isDetecting = false;
                        m_selfCollider.enabled = false;
                    }
                    
                    break;

                case State.Idle:
                    m_shadow.SetActive(false);
                    break;

               

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_animation.animationState.GetCurrent(0).MixDuration = 0;
                    var turnAnim = m_info.turnAnimation;
                    m_turnHandle.Execute(turnAnim.animation, m_info.idleAggroAnimation.animation);
                    break;

                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);


                    m_animation.EnableRootMotion(true, true);
                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.Attack:
                            //m_attackHandle.ExecuteAttack(m_info.attack.animation, m_info.idleAnimation);
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
                        if (m_animation.animationState.GetCurrent(0).IsComplete)
                        {
                            m_animation.SetAnimation(0, m_info.idleAggroAnimation, true);
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
                            if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range)  && m_groundSensor.isDetecting)
                            {
                                m_character.physics.UseStepClimb(false);
                                m_movement.Stop();
                                m_animation.SetAnimation(0, m_info.idleAggroAnimation, true);
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                m_animation.EnableRootMotion(true, false);
                               
                                    m_movement.Stop();
                                    m_selfCollider.enabled = true;
                                    if (m_animation.animationState.GetCurrent(0).IsComplete)
                                    {
                                        m_character.physics.UseStepClimb(false);
                                        m_animation.SetAnimation(0, m_info.idleAggroAnimation, true);
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
            m_stateHandle.OverrideState(State.Idle);
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

        protected override void OnForbidFromAttackTarget()
        {
            ResetAI();
        }

        public override void ReturnToSpawnPoint()
        {
            transform.position = m_startPoint;
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
                m_animation.SetAnimation(0, m_info.flinchAnimation.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation.animation);
                m_animation.SetAnimation(0, m_info.idleAggroAnimation.animation, true);
            }
            yield return new WaitForSeconds(timer);
            //enabled = true;
            //m_flinchHandle.enabled = true;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }
    }
}
