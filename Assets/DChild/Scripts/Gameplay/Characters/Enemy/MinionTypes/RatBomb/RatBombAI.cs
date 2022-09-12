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
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/RatBomb")]
    public class RatBombAI : CombatAIBrain<RatBombAI.Info>, IKnockbackable
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
            private SimpleAttackInfo m_attackMelee = new SimpleAttackInfo();
            public SimpleAttackInfo attackMelee => m_attackMelee;
            [SerializeField]
            private SimpleAttackInfo m_attackExplode = new SimpleAttackInfo();
            public SimpleAttackInfo attackExplode => m_attackExplode;
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

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_explodeEvent;
            public string explodeEvent => m_explodeEvent;

            [SerializeField]
            private float m_explodeDelay;
            public float explodeDelay => m_explodeDelay;


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_attackMelee.SetData(m_skeletonDataAsset);
                m_attackExplode.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Detect,
            Idle,
            Turning,
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
        private IsolatedCharacterPhysics2D m_characterPhysics;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_explodeBB;
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
        
        private float m_currentRunAttackDuration;
        private float m_currentTimeScale;
        private bool m_isDetecting;
        private Vector2 m_startPoint;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_edgeSensor;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_explodeFX;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;

        private State m_turnState;
        private Vector2 m_targetLastPos;
        private Coroutine m_randomTurnRoutine;


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
                if (m_stateHandle.currentState != State.Chasing && !m_isDetecting)
                {
                    m_isDetecting = true;
                    m_stateHandle.SetState(State.Detect);
                }
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            base.OnDestroyed(sender, eventArgs);
            m_animation.SetEmptyAnimation(0, 0);
            m_characterPhysics.UseStepClimb(true);
            m_movement.Stop();
            StartCoroutine(AttackExplodeRoutine());
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.deathAnimation)
            //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
            //m_stateHandle.OverrideState(State.ReevaluateSituation);
            StartCoroutine(AttackExplodeRoutine());
        }


        private bool IsTargetLastPosInRange(float distance) => Vector2.Distance(m_targetLastPos, m_character.centerMass.position) <= distance;

        private IEnumerator DetectRoutine()
        {
            //m_animation.SetAnimation(0, m_info.detectAnimation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.detectAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_targetLastPos = m_targetInfo.position;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
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
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                    m_stateHandle.SetState(State.Turning);
                yield return null;
            }
        }

        private IEnumerator AttackExplodeRoutine()
        {
            //m_animation.SetAnimation(0, m_info.idle2Animation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.idle2Animation);
            m_animation.SetAnimation(0, m_info.attackExplode.animation, false);
            yield return new WaitForSeconds(.45f);
            m_movement.Stop();
            m_hitbox.gameObject.SetActive(false);
            m_explodeFX.Play();
            m_explodeBB.enabled = true;
            yield return new WaitForSeconds(.25f);
            m_explodeBB.enabled = false;
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attackExplode.animation);
            yield return new WaitForSeconds(5);
            m_hitbox.gameObject.SetActive(true);
            transform.gameObject.SetActive(false);
            //m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        protected override void Start()
        {
            base.Start();
            m_currentTimeScale = UnityEngine.Random.Range(1.0f, 2.0f);

            m_randomTurnRoutine = StartCoroutine(RandomTurnRoutine());
            m_startPoint = transform.position;
            //m_spineEventListener.Subscribe(m_info.explodeEvent, m_explodeFX.Play);
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            //m_flinchHandle.FlinchStart += OnFlinchStart;
            //m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
        }


        private void Update()
        {
            switch (m_stateHandle.currentState)
            {
                case State.Detect:
                    m_movement.Stop();
                    StopCoroutine(m_randomTurnRoutine);
                    if (/*IsFacing(m_targetLastPos)*/IsFacingTarget())
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
                    m_movement.Stop();
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                    break;

                case State.Chasing:
                    {
                        if (/*IsFacing(m_targetLastPos)*/IsFacingTarget())
                        {
                            if (IsTargetInRange(m_info.attackExplode.range) 
                                || m_wallSensor.allRaysDetecting 
                                || IsTargetInRange(m_info.attackExplode.range))
                            {
                                m_stateHandle.Wait(State.ReevaluateSituation);
                                m_movement.Stop();
                                StartCoroutine(AttackExplodeRoutine());
                            }
                            else
                            {
                                m_animation.EnableRootMotion(false, false);
                                if (m_groundSensor.isDetecting && m_edgeSensor.isDetecting)
                                {
                                    //var distance = Vector2.Distance(m_targetLastPos, transform.position);
                                    m_animation.SetAnimation(0, m_info.move.animation, true).TimeScale = m_currentTimeScale;
                                    m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.move.speed);
                                }
                                else
                                {
                                    m_movement.Stop();
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
                        m_stateHandle.SetState(State.Idle);
                    }
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Idle);
            m_isDetecting = false;
        }

        public void ResetAI()
        {
            m_targetInfo.Set(null, null);
            m_isDetecting = false;
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
            m_stateHandle.Wait(State.ReevaluateSituation);
            StartCoroutine(KnockbackRoutine(resumeAIDelay));
        }

        private IEnumerator KnockbackRoutine(float timer)
        {
            //enabled = false;
            //m_flinchHandle.m_autoFlinch = false;
            m_animation.DisableRootMotion();
            m_characterPhysics.UseStepClimb(false);
            //m_flinchHandle.enabled = false;
            //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
            //m_animation.SetAnimation(0, m_info.idleAnimation, true);
            yield return new WaitForSeconds(timer);
            m_characterPhysics.UseStepClimb(true);
            //enabled = true;
            //m_flinchHandle.enabled = true;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }
    }
}
