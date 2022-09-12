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
using DChild.Gameplay.Physics;

namespace DChild.Gameplay.Characters.Enemies
{
    public class SkeletonSpawnAI : CombatAIBrain<SkeletonSpawnAI.Info>
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
            [SerializeField]
            private SimpleAttackInfo m_attack3 = new SimpleAttackInfo();
            public SimpleAttackInfo attack3 => m_attack3;
            [SerializeField]
            private SimpleAttackInfo m_runAttack = new SimpleAttackInfo();
            public SimpleAttackInfo runAttack => m_runAttack;
            [SerializeField]
            private SimpleAttackInfo m_runAttack2 = new SimpleAttackInfo();
            public SimpleAttackInfo runAttack2 => m_runAttack2;
            //

            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

            [SerializeField]
            private float m_jumpSpeed;
            public float jumpSpeed => m_jumpSpeed;
            [SerializeField]
            private float m_jumpX;
            public float jumpX => m_jumpX;
            [SerializeField]
            private float m_jumpY;
            public float jumpY => m_jumpY;

            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idle1Animation;
            public string idle1Animation => m_idle1Animation;
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
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_fallToRunAnimation;
            public string fallToRunAnimation => m_fallToRunAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_jumpFromAwakenedAnimation;
            public string jumpFromAwakenedAnimation => m_jumpFromAwakenedAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_jumpFromAwakenedAnimation2;
            public string jumpFromAwakenedAnimation2 => m_jumpFromAwakenedAnimation2;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_jumpFromAwakenedAnimation3;
            public string jumpFromAwakenedAnimation3 => m_jumpFromAwakenedAnimation3;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_jumpUpmAwakenedAnimation;
            public string jumpUpmAwakenedAnimation => m_jumpUpmAwakenedAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_fallFromAwakenedAnimation;
            public string fallFromAwakenedAnimation => m_fallFromAwakenedAnimation;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_rightFootEvent;
            public string rightFootEvent => m_rightFootEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_leftFootEvent;
            public string leftFootEvent => m_leftFootEvent;



            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_run.SetData(m_skeletonDataAsset);
                m_attack1.SetData(m_skeletonDataAsset);
                m_attack2.SetData(m_skeletonDataAsset);
                m_attack3.SetData(m_skeletonDataAsset);
                m_runAttack.SetData(m_skeletonDataAsset);
                m_runAttack2.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Idle,
            Turning,
            Attacking,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            Attack1,
            Attack2,
            Attack3,
            RunAttack,
            [HideInInspector]
            _COUNT
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;

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
        private FlinchHandler m_flinchHandler;

        private bool m_spawnDone;
        //private bool m_isRunAttacking;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        [Title("For Testing Purposes")]
        [SerializeField]
        private GameObject m_testTarget;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            StopAllCoroutines();
            m_animation.EnableRootMotion(false, false);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
            }
        }

        public void AddTarget(GameObject target)
        {
            m_testTarget = target;
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            //m_character.SetFacing(m_character.facing == HorizontalDirection.Left  ? HorizontalDirection.Right : HorizontalDirection.Left);
            m_stateHandle.ApplyQueuedState();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            m_animation.SetAnimation(0, m_info.idle1Animation, true);
            m_spawnDone = true;
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            StopAllCoroutines();
            m_movement.Stop();
            GetComponentInChildren<Hitbox>().gameObject.SetActive(false);
        }

        public void SetDirection(float direction)
        {
            transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);
            GetComponent<Character>().SetFacing(direction > 0 ? HorizontalDirection.Right : HorizontalDirection.Left);
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
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack1, m_info.attack1.range),
                                    new AttackInfo<Attack>(Attack.Attack2, m_info.attack2.range),
                                    new AttackInfo<Attack>(Attack.Attack3, m_info.attack3.range),
                                    new AttackInfo<Attack>(Attack.RunAttack, m_info.runAttack.range)/**/);
            m_attackDecider.hasDecidedOnAttack = false;
        }

        public IEnumerator Die()
        {
            StopAllCoroutines();
            m_spawnDone = false;
            GetComponentInChildren<Hitbox>().gameObject.SetActive(false);
            m_animation.SetAnimation(0, m_info.deathAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathAnimation);
            Destroy(this.gameObject);
            yield return null;
        }

        private IEnumerator SpawnRoutine()
        {
            m_animation.EnableRootMotion(false, false);
            GetComponent<Rigidbody2D>().AddForce(m_info.jumpSpeed * new Vector2(m_info.jumpX * transform.localScale.x, m_info.jumpY), ForceMode2D.Impulse);
            m_animation.SetAnimation(0, m_info.jumpFromAwakenedAnimation3, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.jumpFromAwakenedAnimation3);
            m_animation.SetAnimation(0, m_info.jumpUpmAwakenedAnimation, true);
            yield return new WaitUntil(() => m_groundSensor.isDetecting);
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.fallFromAwakenedAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, SkeletonSpawnAnimation.ANIMATION_FALL_FROM);
            m_animation.SetAnimation(0, m_info.idle1Animation, true);
            if (m_targetInfo.isValid)
            {
                m_stateHandle.SetState(State.Chasing);
            }
            m_spawnDone = true;
            yield return null;
        }

        protected override void Start()
        {
            base.Start();
            //m_enableChase = true;
            //m_spineEventListener.Subscribe(m_info.leftFootEvent, LeftFootAudio);
            //m_spineEventListener.Subscribe(m_info.rightFootEvent, RightFootAudio);

            if (m_testTarget != null)
            {
                SetTarget(m_testTarget.GetComponent<Damageable>(), m_testTarget.GetComponent<Character>());
            }
            StartCoroutine(SpawnRoutine());
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            m_attackDecider = new RandomAttackDecider<Attack>();
            m_flinchHandler.FlinchStart += OnFlinchStart;
            m_flinchHandler.FlinchEnd += OnFlinchEnd;
            UpdateAttackDeciderList();
        }

        private void Update()
        {
            if (m_spawnDone)
            {
                switch (m_stateHandle.currentState)
                {
                    case State.Idle:
                        m_animation.SetAnimation(0, m_info.idle1Animation, true);
                        break;
                    case State.Turning:
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        m_movement.Stop();
                        m_animation.SetAnimation(0, m_info.idle1Animation, true).MixDuration = 0.05f;
                        m_turnHandle.Execute(m_info.turnAnimation, m_info.idle1Animation);
                        break;
                    case State.Attacking:
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        

                        switch (m_attackDecider.chosenAttack.attack)
                        {
                            case Attack.Attack1:
                                m_animation.EnableRootMotion(true, false);
                                m_attackHandle.ExecuteAttack(m_info.attack1.animation, m_info.idle1Animation);
                                break;
                            case Attack.Attack2:
                                m_animation.EnableRootMotion(true, false);
                                m_attackHandle.ExecuteAttack(m_info.attack2.animation, m_info.idle1Animation);
                                break;
                            case Attack.Attack3:
                                m_animation.EnableRootMotion(true, false);
                                m_attackHandle.ExecuteAttack(m_info.attack3.animation, m_info.idle1Animation);
                                break;
                            case Attack.RunAttack:
                                m_animation.EnableRootMotion(false, false);
                                GetComponent<IsolatedPhysics2D>().AddForce((Vector2.right * transform.localScale.x) * 15, ForceMode2D.Impulse);
                                m_attackHandle.ExecuteAttack(m_info.runAttack.animation, m_info.idle1Animation);
                                break;
                        }
                        m_attackDecider.hasDecidedOnAttack = false;

                        break;
                    case State.Chasing:
                        {
                            if (IsFacingTarget())
                            {
                                if (!m_wallSensor.isDetecting && m_groundSensor.allRaysDetecting)
                                {
                                    m_attackDecider.DecideOnAttack();
                                    if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range))
                                    {
                                        m_movement.Stop();
                                        m_animation.SetAnimation(0, m_info.idle1Animation, true).MixDuration = 0.05f;
                                        m_stateHandle.SetState(State.Attacking);
                                    }
                                    else
                                    {
                                        m_animation.EnableRootMotion(false, false);
                                        //m_animation.SetAnimation(0, m_info.run.animation, true);
                                        //m_movement.MoveTowards(m_targetInfo.position, m_info.run.speed * transform.localScale.x);
                                        if (!IsTargetInRange(m_info.targetDistanceTolerance))
                                        {
                                            m_animation.SetAnimation(0, m_info.run.animation, true).MixDuration = 0.05f;
                                            m_movement.MoveTowards(m_targetInfo.position, m_info.run.speed * transform.localScale.x);
                                        }
                                        else
                                        {
                                            m_animation.SetAnimation(0, m_info.move.animation, true).MixDuration = 0.05f;
                                            m_movement.MoveTowards(m_targetInfo.position, m_info.move.speed * transform.localScale.x);
                                        }
                                    }
                                }
                                else
                                {
                                    m_animation.SetAnimation(0, m_info.idle1Animation, true).MixDuration = 0.05f;
                                }
                            }
                            else
                            {
                                if(m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
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

                m_wallSensor.transform.localScale = new Vector3(transform.localScale.x, m_wallSensor.transform.localScale.y, m_wallSensor.transform.localScale.z);
                m_groundSensor.transform.localScale = new Vector3(transform.localScale.x, m_groundSensor.transform.localScale.y, m_groundSensor.transform.localScale.z);
            }
        }

        protected override void OnTargetDisappeared()
        {

        }

        public void ResetAI()
        {
        }

        public override void ReturnToSpawnPoint()
        {
        }

        protected override void OnForbidFromAttackTarget()
        {
        }
    }
}
