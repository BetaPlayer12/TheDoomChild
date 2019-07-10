using System;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Refactor.DChild.Gameplay.Characters.AI;
using UnityEngine;
using Spine;
using Spine.Unity;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using DChild;
using DChild.Gameplay.Characters.Enemies;

namespace Refactor.DChild.Gameplay.Characters.Enemies
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
            //

            //Basic Animation Behaviours
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
            //

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

            //

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
            WaitAttackEnd,
        }

        [SerializeField]
        private SimpleTurnHandle m_turnHandle;
        [SerializeField]
        private MovementHandle2D m_movementHandle;
        [SerializeField]
        private PatrolHandle m_patrolHandle;
        [SerializeField]
        private AttackHandle m_attackHandle;
        [SerializeField]
        private State m_currentState;
        private State m_afterWaitForBehaviourState;
        [SpineEvent, SerializeField]
        private List<string> m_eventName;

        private Attack m_currentAttack;
        private Attack m_afterWaitForBehaviourAttack;
        
        private bool m_hasTarget;
        private bool m_waitRoutineEnd;

        private float m_maxRange;
        private List<float> m_attackRanges;

        [SerializeField]
        private float m_jumpSpeed;
        [SerializeField]
        private float jumpX, jumpY;
        [SerializeField]
        private BoxCollider2D m_box2D;

        protected override void Start()
        {
            base.Start();
            m_attackRanges = new List<float>();
            m_attackRanges.Add(m_info.attack1.range);
            m_attackRanges.Add(m_info.attack2.range);
            m_attackRanges.Add(m_info.attack3.range);
            //m_attackRanges.Add(m_info.runAttack.range);
            Debug.Log("attack Ranges COunt: " + m_attackRanges.Count);
            for (int i = 0; i < m_attackRanges.Count; i++)
            {
                if (m_maxRange < m_attackRanges[i])
                {
                    m_maxRange = m_attackRanges[i];
                }
            }

            StartCoroutine(SpawnRoutine());

            var skeletonAnimation = GetComponentInChildren<SkeletonAnimation>();

            if (skeletonAnimation == null) return;

            skeletonAnimation.AnimationState.Event += HandleEvent;
        }

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_currentState = State.ReevaluateSituation;
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs)
        {
            WaitTillBehaviourEnd(State.ReevaluateSituation);
            m_turnHandle.Execute();
        }

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            //base.SetTarget(damageable, m_target);
            //m_currentState = State.Chasing;
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                m_currentState = State.Chasing;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_currentState = m_afterWaitForBehaviourState;
        }

        private void WaitTillBehaviourEnd(State nextState)
        {
            m_currentState = State.WaitBehaviourEnd;
            m_afterWaitForBehaviourState = nextState;
        }

        private void WaitTillAttackEnd(Attack nextAttack)
        {
            m_currentAttack = Attack.WaitAttackEnd;
            m_afterWaitForBehaviourAttack = nextAttack;
        }

        public void MoveTo(Vector2 target, float speed)
        {
            var direction = (target - (Vector2)transform.position).normalized;
            GetComponent<IsolatedPhysics2D>().SetVelocity(direction * speed);
        }

        public void MoveOnGround(Vector2 target, float speed)
        {
            //var direction = (target - (Vector2)transform.position).normalized;
            //m_physics.SetVelocity((Mathf.Sign(direction.x) * moveGroundDirection) * speed);
            if (target.x > transform.position.x)
            {
                GetComponent<IsolatedPhysics2D>().SetVelocity(Vector2.right * speed);
            }
            else
            {
                GetComponent<IsolatedPhysics2D>().SetVelocity(Vector2.left * speed);
            }
        }

        public void SetDirection(float direction)
        {
            transform.localScale = new Vector3(direction, transform.localScale.y, transform.localScale.z);
        }

        public bool Wait()
        {
            if (m_animation.GetCurrentAnimation(0).ToString() != "Idle")
            {
                return m_animation.skeletonAnimation.AnimationState.GetCurrent(0).IsComplete;
            }
            else
            {
                return true;
            }
        }

        private IEnumerator TurnRoutine()
        {
            m_waitRoutineEnd = true;
            m_animation.SetAnimation(0, m_info.turnAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, SkeletonSpawnAnimation.ANIMATION_TURN);
            m_animation.SetAnimation(0, m_info.idle1Animation, true);
            m_waitRoutineEnd = false;
            yield return null;
            m_turnHandle.Execute();
        }

        private IEnumerator SpawnRoutine()
        {
            m_waitRoutineEnd = true;
            m_animation.EnableRootMotion(false, false);
            GetComponent<Rigidbody2D>().AddForce(m_jumpSpeed * new Vector2(jumpX * transform.localScale.x, jumpY), ForceMode2D.Impulse);
            m_animation.SetAnimation(0, m_info.jumpFromAwakenedAnimation3, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, SkeletonSpawnAnimation.ANIMATION_JUMP_FROM3);
            m_animation.SetAnimation(0, m_info.jumpUpmAwakenedAnimation, true);
            yield return new WaitUntil(() => GetComponent<IsolatedCharacterPhysics2D>().onWalkableGround);
            m_animation.SetAnimation(0, m_info.fallFromAwakenedAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, SkeletonSpawnAnimation.ANIMATION_FALL_FROM);
            m_animation.SetAnimation(0, m_info.idle1Animation, true);
            m_box2D.offset = new Vector2(0, 10);
            m_box2D.size = new Vector2(50, 25);
            m_waitRoutineEnd = false;
            yield return null;
        }

        private IEnumerator Attack1Routine()
        {
            m_waitRoutineEnd = true;
            m_animation.SetAnimation(0, m_info.attack1.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, SkeletonSpawnAnimation.ANIMATION_ATTACK1);
            m_animation.SetAnimation(0, m_info.idle1Animation, true);
            m_waitRoutineEnd = false;
            m_currentState = State.ReevaluateSituation;
            yield return null;
        }

        private IEnumerator Attack2Routine()
        {
            m_waitRoutineEnd = true;
            m_animation.SetAnimation(0, m_info.attack2.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, SkeletonSpawnAnimation.ANIMATION_ATTACK2);
            m_animation.SetAnimation(0, m_info.idle1Animation, true);
            m_waitRoutineEnd = false;
            m_currentState = State.ReevaluateSituation;
            yield return null;
        }

        private IEnumerator Attack3Routine()
        {
            m_waitRoutineEnd = true;
            m_animation.SetAnimation(0, m_info.attack3.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, SkeletonSpawnAnimation.ANIMATION_ATTACK3);
            m_animation.SetAnimation(0, m_info.idle1Animation, true);
            m_waitRoutineEnd = false;
            m_currentState = State.ReevaluateSituation;
            yield return null;
        }

        private IEnumerator RunAttackRoutine()
        {
            m_waitRoutineEnd = true;
            //MoveOnGround(m_targetInfo.position, m_info.run.speed);
            GetComponent<IsolatedPhysics2D>().AddForce((Vector2.right * transform.localScale.x) * 15, ForceMode2D.Impulse);
            m_animation.SetAnimation(0, m_info.runAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, SkeletonSpawnAnimation.ANIMATION_RUN2);
            m_animation.SetAnimation(0, m_info.idle1Animation, true);
            m_movementHandle.Stop();
            m_waitRoutineEnd = false;
            m_currentState = State.ReevaluateSituation;
            yield return null;
        }

        void HandleEvent(TrackEntry trackEntry, Spine.Event e)
        {
        }

        private void Update()
        {
            switch (m_currentState)
            {
                case State.Idle:
                    //Add actual CharacterInfo Later
                    if (!m_waitRoutineEnd)
                    {
                        m_animation.SetAnimation(0, m_info.idle1Animation, true);
                    }
                    break;
                case State.Turning:
                    if (!m_waitRoutineEnd)
                    {
                        StartCoroutine(TurnRoutine());
                        WaitTillBehaviourEnd(State.ReevaluateSituation);
                    }
                    break;
                case State.Attacking:
                    if (!m_waitRoutineEnd)
                    {
                        var target = m_targetInfo.position;
                        Array values = Enum.GetValues(typeof(Attack));
                        var random = new System.Random();
                        m_currentAttack = (Attack)values.GetValue(random.Next(values.Length));
                        switch (m_currentAttack)
                        {
                            case Attack.Attack1:
                                //m_attackHandle.ExecuteAttack(m_info.groundSlam.animation);
                                if (Vector2.Distance(target, transform.position) < m_info.attack1.range)
                                {
                                    StartCoroutine(Attack1Routine());
                                    WaitTillAttackEnd(Attack.Attack1);
                                }
                                break;
                            case Attack.Attack2:
                                //m_attackHandle.ExecuteAttack(m_info.spit.animation);
                                if (Vector2.Distance(target, transform.position) < m_info.attack2.range)
                                {
                                    StartCoroutine(Attack2Routine());
                                    WaitTillAttackEnd(Attack.Attack2);
                                }
                                break;
                            case Attack.Attack3:
                                if (Vector2.Distance(target, transform.position) < m_info.attack3.range)
                                {
                                    StartCoroutine(Attack3Routine());
                                    WaitTillAttackEnd(Attack.Attack3);
                                }
                                break;
                            case Attack.RunAttack:
                                if (Vector2.Distance(target, transform.position) < m_info.runAttack.range)
                                {
                                    StartCoroutine(RunAttackRoutine());
                                    WaitTillAttackEnd(Attack.RunAttack);
                                }
                                break;
                        }
                    }
                    break;
                case State.Chasing:
                    if (!m_waitRoutineEnd)
                    {
                        var target = m_targetInfo.position;
                        //Put Target Destination
                        if (IsFacingTarget() && Vector2.Distance(target, transform.position) <= m_maxRange)
                        {
                            m_currentState = State.Attacking;
                            m_animation.SetAnimation(0, m_info.idle1Animation, true);
                            m_movementHandle.Stop();
                        }
                        else if (IsFacingTarget() && Vector2.Distance(target, transform.position) >= m_maxRange)
                        {
                            if (Wait())
                            {
                                m_animation.EnableRootMotion(false, false);
                                m_animation.SetAnimation(0, m_info.run.animation, true);
                                //m_animation.SetAnimation(1, m_info.runAttack.animation, true);
                                //m_movementHandle.MoveTowards(target, m_info.run.speed);
                                MoveOnGround(target, m_info.run.speed);
                            }
                        }
                        else
                        {
                            m_currentState = State.Turning;
                            m_movementHandle.Stop();
                            //m_turnHandle.Execute();
                        }
                        //Play Animation
                    }
                    break;
                case State.ReevaluateSituation:
                    //How far is target, is it worth it to chase or go back to patrol
                    if (!m_waitRoutineEnd)
                    {
                        if (m_targetInfo.isValid)
                        {
                            m_currentState = State.Chasing;
                        }
                        else
                        {
                            m_currentState = State.Idle;
                        }
                    }
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }

            //if (Input.GetKeyDown(KeyCode.Z))
            //{
            //    StartCoroutine(SpawnRoutine());
            //}
        }
    }
}