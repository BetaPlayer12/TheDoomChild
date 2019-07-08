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
    public class MeleeBeeDroneAI : CombatAIBrain<MeleeBeeDroneAI.Info>
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
            //

            //Attack Behaviours
            [SerializeField]
            private SimpleAttackInfo m_stingerdive = new SimpleAttackInfo();
            public SimpleAttackInfo stingerdive => m_stingerdive;
            [SerializeField]
            private SimpleAttackInfo m_rapidSting = new SimpleAttackInfo();
            public SimpleAttackInfo rapidSting => m_rapidSting;
            //

            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idle2Animation;
            public string idle2Animation => m_idle2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_damageAnimation;
            public string damageAnimation => m_damageAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flightAnimation;
            public string flightAnimation => m_flightAnimation;
            //

            [SerializeField]
            private GameObject m_stingerGO;
            public GameObject stingerGO => m_stingerGO;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_patrol.SetData(m_skeletonDataAsset);
                m_move.SetData(m_skeletonDataAsset);
                m_stingerdive.SetData(m_skeletonDataAsset);
                m_rapidSting.SetData(m_skeletonDataAsset);
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
            StingerDive,
            RapidSting,
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

        [SerializeField]
        private Transform m_stingerPos;
        [SerializeField]
        private float m_stingerSpeed;

        //Patience Handler
        [SerializeField]
        private float m_patience;
        private float m_currentPatience;
        private bool m_enablePatience;
        private bool m_waitRoutineEnd;

        private float m_maxRange;
        private List<float> m_attackRanges;

        protected override void Start()
        {
            base.Start();
            m_attackRanges = new List<float>();
            m_attackRanges.Add(m_info.stingerdive.range);
            m_attackRanges.Add(m_info.rapidSting.range);
            Debug.Log("attack Ranges COunt: "+m_attackRanges.Count);
            for (int i = 0; i < m_attackRanges.Count; i++)
            {
                if (m_maxRange < m_attackRanges[i])
                {
                    m_maxRange = m_attackRanges[i];
                }
            }

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
                m_currentPatience = 0;
                m_enablePatience = false;
            }
            else
            {
                m_enablePatience = true;
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

        //Patience Handler
        private void Patience()
        {
            if (m_currentPatience < m_patience)
            {
                m_currentPatience += Time.deltaTime;
            }
            else
            {
                //m_targetInfo = null;
                base.SetTarget(null, null);
                m_enablePatience = false;
                m_currentState = State.Idle;
            }
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
            yield return new WaitForAnimationComplete(m_animation.animationState, MeleeBeeDroneAnimation.ANIMATION_TURN);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_waitRoutineEnd = false;
            yield return null;
            m_turnHandle.Execute();
        }

        private IEnumerator StingerDiveRoutine()
        {
            m_waitRoutineEnd = true;
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.stingerdive.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, MeleeBeeDroneAnimation.ANIMATION_STINGERDIVE);
            m_animation.EnableRootMotion(false, false);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_waitRoutineEnd = false;
            yield return null;
        }

        private IEnumerator RapidStingRoutine()
        {
            m_waitRoutineEnd = true;
            transform.SetParent(m_targetInfo.transform);
            GetComponent<Rigidbody2D>().isKinematic = true;
            m_animation.DisableRootMotion();
            m_animation.SetAnimation(0, m_info.rapidSting.animation, false);
            //yield return new WaitForSeconds(2);
            //transform.SetParent(m_targetInfo.)
            yield return new WaitForAnimationComplete(m_animation.animationState, MeleeBeeDroneAnimation.ANIMATION_RAPIDSTING);
            transform.SetParent(null);
            GetComponent<Rigidbody2D>().isKinematic = false;
            m_animation.EnableRootMotion(false, false);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_waitRoutineEnd = false;
            yield return null;
        }

        void HandleEvent(TrackEntry trackEntry, Spine.Event e)
        {
            if (e.Data.Name == m_eventName[0])
            {

            }
        }

        private void Update()
        {
            switch (m_currentState)
            {
                case State.Idle:
                    //Add actual CharacterInfo Later
                    if (m_targetInfo.isValid)
                    {
                        //StartCoroutine(UnburrowRoutine());
                        Debug.Log("Doing IDLE");
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    }
                    else
                    {
                        //PATROL
                        Debug.Log("Doing PATROL");
                        m_animation.SetAnimation(0, m_info.patrol.animation, true);
                        var characterInfo = new PatrolHandle.CharacterInfo(m_character.transform.position, m_character.facing);
                        m_patrolHandle.Patrol(m_movementHandle, m_info.patrol.speed, characterInfo);
                    }
                    break;
                case State.Turning:
                    if (Wait() && !m_waitRoutineEnd)
                    {
                        Debug.Log("Doing TURN");
                        StartCoroutine(TurnRoutine());
                        WaitTillBehaviourEnd(State.ReevaluateSituation);
                    }
                    break;
                case State.Attacking:
                    if (!m_waitRoutineEnd)
                    {
                        Debug.Log("Doing ATTACK");
                        var target = m_targetInfo.position;
                        target = new Vector2(m_targetInfo.position.x, m_targetInfo.position.y - 0.5f);
                        Array values = Enum.GetValues(typeof(Attack));
                        var random = new System.Random();
                        m_currentAttack = (Attack)values.GetValue(random.Next(values.Length));
                        switch (m_currentAttack)
                        {
                            case Attack.StingerDive:
                                //if (Wait())
                                //{
                                //}
                                if (Vector2.Distance(target, transform.position) <= m_info.stingerdive.range)
                                {
                                    StartCoroutine(StingerDiveRoutine());
                                    WaitTillAttackEnd(Attack.StingerDive);
                                }
                                break;
                            case Attack.RapidSting:
                                //if (Wait())
                                //{
                                //}
                                if (Vector2.Distance(target, transform.position) <= m_info.rapidSting.range)
                                {
                                    StartCoroutine(RapidStingRoutine());
                                    WaitTillAttackEnd(Attack.RapidSting);
                                }
                                break;
                        }
                    }
                    break;
                case State.Chasing:
                    if (!m_waitRoutineEnd)
                    {
                        var target = m_targetInfo.position;
                        target = new Vector2(m_targetInfo.position.x, m_targetInfo.position.y - 0.5f);
                        Debug.Log("Doing CHASE");
                        //Put Target Destination
                        if (IsFacingTarget() && Vector2.Distance(target, transform.position) <= m_maxRange)
                        {
                            m_currentState = State.Attacking;
                            m_movementHandle.Stop();
                        }
                        else if (IsFacingTarget() && Vector2.Distance(target, transform.position) >= m_maxRange)
                        {
                            if (Wait())
                            {
                                m_animation.EnableRootMotion(false, false);
                                m_animation.SetAnimation(0, m_info.flightAnimation, true);
                                //m_movementHandle.MoveTowards(target, m_info.move.speed);
                                MoveTo(target, m_info.move.speed);
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
                        Debug.Log("Doing REEVALUATION");
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

            if (m_enablePatience)
            {
                Patience();
            }
        }
    }
}
