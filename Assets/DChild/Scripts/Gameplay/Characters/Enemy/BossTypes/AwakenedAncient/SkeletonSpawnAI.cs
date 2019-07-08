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
            private string m_moveAnimation;
            public string moveAnimation => m_moveAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_runAnimation;
            public string runAnimation => m_runAnimation;
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

            //

            [SerializeField]
            private GameObject m_footFX;
            public GameObject footFX => m_footFX;
            [SerializeField]
            private GameObject m_anticipationFX;
            public GameObject anticipationFX => m_anticipationFX;
            [SerializeField]
            private GameObject m_seedSpitFX;
            public GameObject seedSpitFX => m_seedSpitFX;
            [SerializeField]
            private GameObject m_mouthSpitFX;
            public GameObject mouthSpitFX => m_mouthSpitFX;
            [SerializeField]
            private GameObject m_stompFX;
            public GameObject stompFX => m_stompFX;
            [SerializeField]
            private GameObject m_crawlingVineFX;
            public GameObject crawlingVineFX => m_crawlingVineFX;
            [SerializeField]
            private GameObject m_tombAttackGO;
            public GameObject tombAttackGO => m_tombAttackGO;
            [SerializeField]
            private GameObject m_skeletonGO;
            public GameObject skeletonGO => m_skeletonGO;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_attack1.SetData(m_skeletonDataAsset);
                m_attack2.SetData(m_skeletonDataAsset);
                m_attack3.SetData(m_skeletonDataAsset);
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

        //Patience Handler
        [SerializeField]
        private float m_patience;
        private float m_currentPatience;
        private bool m_enablePatience;
        private bool m_burrowed;
        private bool m_waitRoutineEnd;

        private float m_maxRange;
        private List<float> m_attackRanges;

        protected override void Start()
        {
            base.Start();
            m_attackRanges = new List<float>();
            m_attackRanges.Add(m_info.attack1.range);
            m_attackRanges.Add(m_info.attack2.range);
            m_attackRanges.Add(m_info.attack3.range);
            Debug.Log("attack Ranges COunt: " + m_attackRanges.Count);
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
                if (!m_burrowed)
                {
                    m_currentState = State.Chasing;
                    m_currentPatience = 0;
                    m_enablePatience = false;
                }
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

            if (m_animation.skeletonAnimation == null) return;

            m_animation.skeletonAnimation.AnimationState.Event += HandleEvent;
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
            yield return new WaitForAnimationComplete(m_animation.animationState, SkeletonSpawnAnimation.ANIMATION_TURN);
            m_animation.SetAnimation(0, m_info.idle1Animation, true);
            m_waitRoutineEnd = false;
            yield return null;
            m_turnHandle.Execute();
        }

        private IEnumerator Attack1Routine()
        {
            m_waitRoutineEnd = true;
            m_animation.SetAnimation(0, m_info.attack1.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, SkeletonSpawnAnimation.ANIMATION_ATTACK1);
            m_animation.SetAnimation(0, m_info.idle1Animation, true);
            m_waitRoutineEnd = false;
            yield return null;
        }

        private IEnumerator Attack2Routine()
        {
            m_waitRoutineEnd = true;
            m_animation.SetAnimation(0, m_info.attack2.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, SkeletonSpawnAnimation.ANIMATION_ATTACK2);
            m_animation.SetAnimation(0, m_info.idle1Animation, true);
            m_waitRoutineEnd = false;
            yield return null;
        }

        private IEnumerator Attack3Routine()
        {
            m_waitRoutineEnd = true;
            m_animation.SetAnimation(0, m_info.attack3.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, SkeletonSpawnAnimation.ANIMATION_ATTACK3);
            m_animation.SetAnimation(0, m_info.idle1Animation, true);
            m_waitRoutineEnd = false;
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
                    if (m_targetInfo.isValid)
                    {
                        //StartCoroutine(UnburrowRoutine());
                        Debug.Log("Doing IDLE");
                        m_animation.SetAnimation(0, m_info.idle1Animation, true);
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
                                if (Wait())
                                {
                                    //m_attackHandle.ExecuteAttack(m_info.groundSlam.animation);
                                    if (Vector2.Distance(target, transform.position) >= m_info.attack2.range)
                                    {
                                        StartCoroutine(Attack1Routine());
                                        WaitTillAttackEnd(Attack.Attack1);
                                    }
                                }
                                break;
                            case Attack.Attack2:
                                if (Wait())
                                {
                                    //m_attackHandle.ExecuteAttack(m_info.spit.animation);
                                    if (Vector2.Distance(target, transform.position) >= m_info.attack2.range)
                                    {
                                        StartCoroutine(Attack2Routine());
                                        WaitTillAttackEnd(Attack.Attack2);
                                    }
                                }
                                break;
                            case Attack.Attack3:
                                if (Wait())
                                {
                                    if (Vector2.Distance(target, transform.position) >= m_info.attack3.range)
                                    {
                                        StartCoroutine(Attack3Routine());
                                        WaitTillAttackEnd(Attack.Attack3);
                                    }
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
                            m_movementHandle.Stop();
                        }
                        else if (IsFacingTarget() && Vector2.Distance(target, transform.position) >= m_maxRange)
                        {
                            if (Wait())
                            {
                                m_animation.EnableRootMotion(true, true);
                                m_animation.SetAnimation(0, m_info.moveAnimation, true);
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

            if (m_enablePatience)
            {
                Patience();
            }
        }
    }
}