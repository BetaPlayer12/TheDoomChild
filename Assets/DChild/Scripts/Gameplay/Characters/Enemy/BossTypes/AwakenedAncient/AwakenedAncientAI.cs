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
    public class AwakenedAncientAI : CombatAIBrain<AwakenedAncientAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Basic Behaviours
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_moveAnimation;
            public string moveAnimation => m_moveAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_burrowAnimation;
            public string burrowAnimation => m_burrowAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_burrowIdleAnimation;
            public string burrowIdleAnimation => m_burrowIdleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_unburrowAnimation;
            public string unburrowAnimation => m_unburrowAnimation;
            //

            //Attack Behaviours
            [SerializeField]
            private SimpleAttackInfo m_groundSlam = new SimpleAttackInfo();
            public SimpleAttackInfo groundSlam => m_groundSlam;
            [SerializeField]
            private SimpleAttackInfo m_spit = new SimpleAttackInfo();
            public SimpleAttackInfo spit => m_spit;
            [SerializeField]
            private SimpleAttackInfo m_skeletonSummon = new SimpleAttackInfo();
            public SimpleAttackInfo skeletonSummon => m_skeletonSummon;

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
                m_groundSlam.SetData(m_skeletonDataAsset);
                m_spit.SetData(m_skeletonDataAsset);
                m_skeletonSummon.SetData(m_skeletonDataAsset);
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
            GroundSlam,
            Spit,
            Tomb,
            SkeletonSummon,
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

        [SerializeField]
        private Transform m_footTF;
        [SerializeField]
        private Transform m_anticipationTF;
        [SerializeField]
        private Transform m_seedSpitTF;
        [SerializeField]
        private Transform m_stompTF;
        [SerializeField]
        private Transform m_skeletonSpawnTF;

        [SerializeField]
        private float m_spitSpeed;
        [SerializeField]
        private float m_vineCrawlSpeed;

        private Attack m_currentAttack;
        private Attack m_afterWaitForBehaviourAttack;

        //Patience Handler
        [SerializeField]
        private float m_patience;
        private float m_currentPatience;
        private bool m_enablePatience;
        private bool m_burrowed;
        private bool m_waitRoutineEnd;

        [SerializeField]
        private int m_skeletonSize;
        private GameObject[] m_skeletons;

        protected override void Start()
        {
            base.Start();
            m_burrowed = true;
            //Debug.Log(m_boneName.Count);
            //for (int i = 0; i < m_boneName.Count; i++)
            //{
            //    m_bone[i] = GetComponentInChildren<SkeletonAnimation>().Skeleton.FindBone(m_boneName[i]);
            //    Debug.Log(m_bone);
            //}
            m_skeletons = new GameObject[m_skeletonSize];
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

        private void InstantiateObject(GameObject m_object)
        {
            //Shoot Spit
            var target = m_targetInfo.position; //No Parabola
                                                //var target = TargetParabola(); //With Parabola
            target = new Vector2(target.x, target.y - 2);
            Vector2 spitPos = m_seedSpitTF.position;
            Vector3 v_diff = (target - spitPos);
            float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
            //transform.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg);
            //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            GameObject shoot = Instantiate(m_info.seedSpitFX, spitPos, Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg)); //No Parabola
                                                                                                                         //GameObject shoot = Instantiate(m_info.seedSpitFX, spitPos, Quaternion.identity); //With Parabola
            shoot.GetComponent<Rigidbody2D>().AddForce((m_spitSpeed + (Vector2.Distance(target, transform.position) * 0.35f)) * shoot.transform.right, ForceMode2D.Impulse);
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
            yield return new WaitForAnimationComplete(m_animation.animationState, AwakenedAncientAnimation.ANIMATION_TURN);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_waitRoutineEnd = false;
            yield return null;
            m_turnHandle.Execute();
        }

        private IEnumerator BurrowRoutine()
        {
            m_waitRoutineEnd = true;
            m_animation.SetAnimation(0, m_info.burrowAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, AwakenedAncientAnimation.ANIMATION_BURROW);
            m_animation.SetAnimation(0, m_info.burrowIdleAnimation, true);
            m_waitRoutineEnd = false;
            yield return null;
        }

        private IEnumerator UnburrowRoutine()
        {
            m_waitRoutineEnd = true;
            m_animation.SetAnimation(0, m_info.unburrowAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, AwakenedAncientAnimation.ANIMATION_UNBURROW);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_waitRoutineEnd = false;
            yield return null;
        }

        private IEnumerator GroundAttackRoutine()
        {
            m_waitRoutineEnd = true;
            m_animation.SetAnimation(0, m_info.groundSlam.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, AwakenedAncientAnimation.ANIMATION_GROUND_SLAM);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_waitRoutineEnd = false;
            yield return null;
        }

        private IEnumerator TombAttackRoutine(Vector3 target)
        {
            m_waitRoutineEnd = true;
            m_animation.SetAnimation(0, m_info.burrowAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, AwakenedAncientAnimation.ANIMATION_BURROW);
            //Debug.Log("Summon Tombs");
            for (int i = 0; i < 3; i++)
            {
                GameObject tomb = Instantiate(m_info.tombAttackGO, new Vector2(target.x + UnityEngine.Random.Range(-10, 10), target.y - 2.5f), Quaternion.identity);
                tomb.GetComponent<TombAttack>().GetTarget(m_targetInfo);
            }
            //m_animation.SetAnimation(0, m_info.burrowIdleAnimation, true);
            //yield return null;
            yield return new WaitForSeconds(5f);
            //Debug.Log("Waited seconds");
            m_animation.SetAnimation(0, m_info.unburrowAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, AwakenedAncientAnimation.ANIMATION_UNBURROW);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_waitRoutineEnd = false;
            yield return null;
        }

        private IEnumerator SkeletonSummonRoutine()
        {
            m_waitRoutineEnd = true;
            m_animation.SetAnimation(0, m_info.skeletonSummon.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, AwakenedAncientAnimation.ANIMATION_SPIT_SKELETON);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_waitRoutineEnd = false;
            yield return null;
        }

        void HandleEvent(TrackEntry trackEntry, Spine.Event e)
        {
            if (e.Data.Name == m_eventName[0])
            {
                //Debug.Log(m_eventName[0]);

                GameObject obj = Instantiate(m_info.footFX, /*new Vector2(m_footTF.position.x + (3.5f * transform.localScale.x), m_footTF.position.y)*/ m_footTF.position, Quaternion.identity);
                obj.transform.parent = m_footTF;
            }
            else if (e.Data.Name == m_eventName[1])
            {
                //Debug.Log(m_eventName[1]);

                GameObject obj = Instantiate(m_info.anticipationFX, new Vector2(m_seedSpitTF.position.x + (1 * transform.localScale.x), m_seedSpitTF.position.y + .25f), Quaternion.identity);
                obj.transform.localScale = new Vector3(obj.transform.localScale.x * transform.localScale.x, obj.transform.localScale.y, obj.transform.localScale.z);
                obj.transform.parent = m_seedSpitTF;
            }
            else if (e.Data.Name == m_eventName[2])
            {
                //Debug.Log(m_eventName[2]);
                GameObject obj = Instantiate(m_info.mouthSpitFX, m_seedSpitTF.position, Quaternion.identity);
                obj.transform.localScale = new Vector3(obj.transform.localScale.x * transform.localScale.x, obj.transform.localScale.y, obj.transform.localScale.z);
                obj.transform.parent = m_seedSpitTF;
                obj.transform.localPosition = new Vector2(4, -1.5f);

                //Shoot Spit
                var target = m_targetInfo.position; //No Parabola
                //var target = TargetParabola(); //With Parabola
                target = new Vector2(target.x, target.y - 2);
                Vector2 spitPos = m_seedSpitTF.position;
                Vector3 v_diff = (target - spitPos);
                float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
                //transform.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg);
                //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                GameObject shoot = Instantiate(m_info.seedSpitFX, spitPos, Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg)); //No Parabola
                //GameObject shoot = Instantiate(m_info.seedSpitFX, spitPos, Quaternion.identity); //With Parabola
                shoot.GetComponent<Rigidbody2D>().AddForce((m_spitSpeed + (Vector2.Distance(target, transform.position) * 0.35f)) * shoot.transform.right, ForceMode2D.Impulse);
            }
            else if (e.Data.Name == m_eventName[3])
            {
                //Debug.Log(m_eventName[3]);

                GameObject obj = Instantiate(m_info.stompFX, new Vector2(m_stompTF.position.x + (0.5f * transform.localScale.x), m_stompTF.position.y - 2.75f), Quaternion.identity);
                GameObject obj2 = Instantiate(m_info.crawlingVineFX, new Vector2(m_stompTF.position.x + (0.5f * transform.localScale.x), m_stompTF.position.y - 2.5f), Quaternion.identity);
                obj2.transform.localScale = new Vector3(obj2.transform.localScale.x * transform.localScale.x, obj2.transform.localScale.y, obj2.transform.localScale.z);
                obj2.GetComponent<Rigidbody2D>().AddForce(new Vector2(m_vineCrawlSpeed * transform.localScale.x, 0), ForceMode2D.Impulse);
            }
            else if (e.Data.Name == m_eventName[4])
            {
                Debug.Log(m_eventName[4]);
            }
            else if (e.Data.Name == m_eventName[5])
            {
                //Debug.Log(m_eventName[5]);
                for (int i = 0; i < 1; i++)
                {
                    GameObject skeleton = Instantiate(m_info.skeletonGO, new Vector2(m_skeletonSpawnTF.position.x + (3 * transform.localScale.x) + UnityEngine.Random.Range(-5, 5), m_skeletonSpawnTF.position.y), Quaternion.identity);
                    skeleton.GetComponent<SkeletonSpawnAI>().SetDirection(transform.localScale.x);
                    m_skeletons[i] = skeleton;
                }
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
                        if (m_burrowed)
                        {
                            StartCoroutine(UnburrowRoutine());
                            m_burrowed = false;
                        }
                        else
                        {
                            if (Wait())
                            {
                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                            }
                        }
                    }
                    else
                    {
                        //StartCoroutine(BurrowRoutine());
                        if (!m_burrowed)
                        {
                            StartCoroutine(BurrowRoutine());
                            m_burrowed = true;
                        }
                        else
                        {
                            if (Wait())
                            {
                                m_animation.SetAnimation(0, m_info.burrowIdleAnimation, true);
                            }
                        }
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
                            case Attack.GroundSlam:
                                if (Wait())
                                {
                                    //m_attackHandle.ExecuteAttack(m_info.groundSlam.animation);
                                    StartCoroutine(GroundAttackRoutine());
                                    WaitTillAttackEnd(Attack.GroundSlam);
                                }
                                break;
                            case Attack.Spit:
                                if (Wait())
                                {
                                    //m_attackHandle.ExecuteAttack(m_info.spit.animation);
                                    if (Vector2.Distance(target, transform.position) >= m_info.groundSlam.range - 15)
                                    {
                                        m_animation.SetAnimation(0, m_info.spit.animation, false);
                                        m_animation.AddAnimation(0, m_info.idleAnimation, true, 0);
                                        WaitTillAttackEnd(Attack.Spit);
                                    }
                                }
                                break;
                            case Attack.Tomb:
                                if (Wait())
                                {
                                    if (Vector2.Distance(target, transform.position) >= m_info.groundSlam.range - 10)
                                    {
                                        StartCoroutine(TombAttackRoutine(target));
                                        WaitTillAttackEnd(Attack.Tomb);
                                    }
                                }
                                break;
                            case Attack.SkeletonSummon:
                                if (Wait() && m_skeletons.Length == m_skeletonSize)
                                {
                                    if (Vector2.Distance(target, transform.position) >= m_info.skeletonSummon.range - 10)
                                    {
                                        StartCoroutine(SkeletonSummonRoutine());
                                        WaitTillAttackEnd(Attack.SkeletonSummon);
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
                        if (IsFacingTarget() && Vector2.Distance(target, transform.position) <= m_info.groundSlam.range)
                        {
                            m_currentState = State.Attacking;
                            m_movementHandle.Stop();
                        }
                        else if (IsFacingTarget() && Vector2.Distance(target, transform.position) >= m_info.groundSlam.range)
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