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
using Doozy.Engine;

namespace DChild.Gameplay.Characters.Enemies
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
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
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

            [SerializeField, MinValue(0)]
            private float m_patience;
            public float patience => m_patience;
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

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
            private GameObject m_skeletonSpawnFX;
            public GameObject skeletonSpawnFX => m_skeletonSpawnFX;
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

        private enum Phase
        {
            Second,
            Third,
            Final,
            Wait,
        }

        [SerializeField, TabGroup("Reference")]
        private BasicHealth m_health;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        private float m_maxHealth;
        private float m_phaseHealth;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;

        [SerializeField]
        private SimpleTurnHandle m_turnHandle;
        [SerializeField]
        private MovementHandle2D m_movementHandle;
        [SerializeField]
        private PatrolHandle m_patrolHandle;
        [SerializeField]
        private AttackHandle m_attackHandle;
        [SerializeField]
        private DeathHandle m_deathHandle;
        [SerializeField]
        private State m_currentState;
        private State m_afterWaitForBehaviourState;
        [SpineEvent, SerializeField]
        private List<string> m_eventName;
        [SpineSkin, SerializeField]
        private List<string> m_skinName;
        private int m_chosenSkin;

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
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        //Patience Handler
        private bool m_burrowed;
        private bool m_waitRoutineEnd;
        private bool m_isPhaseChanging;

        private List<GameObject> m_skeletons;
        private List<GameObject> m_tombs;

        [SerializeField]
        private List<ParticleSystem> m_summonFX;
        [SerializeField]
        private ParticleSystem m_smokeFX;

        [SerializeField, TabGroup("Cannon Values")]
        private float m_speed;
        [SerializeField, TabGroup("Cannon Values")]
        private float m_gravityScale;
        [SerializeField, TabGroup("Cannon Values")]
        private Vector2 m_posOffset;
        [SerializeField, TabGroup("Cannon Values")]
        private float m_velOffset;
        [SerializeField, TabGroup("Cannon Values")]
        private Vector2 m_targetOffset;
        [SerializeField, TabGroup("Phase Values")]
        private List<int> m_tombVolleys;
        private int m_currentTombVolleys;
        [SerializeField, TabGroup("Phase Values")]
        private List<int> m_tombSize;
        private int m_currentTombSize;
        [SerializeField, TabGroup("Phase Values")]
        private List<int> m_skeletonSize;
        private int m_currentSkeletonSize;
        [SerializeField, TabGroup("Phase Values")]
        private List<int> m_summonThreshhold;
        private int m_currentSummonThreshhold;

        private float m_targetDistance;

        protected override void Start()
        {
            base.Start();
            m_burrowed = true;
            m_currentSkeletonSize = m_skeletonSize[0];
            m_currentSummonThreshhold = m_summonThreshhold[0];
            m_maxHealth = m_health.currentValue;
            m_phaseHealth = m_maxHealth;
            m_animation.skeletonAnimation.skeleton.SetSkin(m_skinName[0]);
            //Debug.Log(m_boneName.Count);
            //for (int i = 0; i < m_boneName.Count; i++)
            //{
            //    m_bone[i] = GetComponentInChildren<SkeletonAnimation>().Skeleton.FindBone(m_boneName[i]);
            //    Debug.Log(m_bone);
            //}
            m_info.seedSpitFX.GetComponent<IsolatedObjectPhysics2D>().gravity.gravityScale = m_gravityScale;
            GameplaySystem.SetBossHealth(m_character);
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
                GameEventMessage.SendEvent("Boss Encounter");
            }
        }

        public override void ApplyData()
        {
            base.ApplyData();
            if (m_attackDecider != null)
            {
                //Debug.Log("Update attack list trigger function");
                UpdateAttackDeciderList();
            }
        }
        private void UpdateAttackDeciderList()
        {
            //Debug.Log("Update attack list trigger");
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.GroundSlam, m_info.groundSlam.range),
                                    new AttackInfo<Attack>(Attack.SkeletonSummon, m_info.skeletonSummon.range),
                                    new AttackInfo<Attack>(Attack.Spit, m_info.spit.range),
                                    new AttackInfo<Attack>(Attack.Tomb, m_info.skeletonSummon.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        protected override void Awake()
        {
            base.Awake();
            m_patrolHandle.TurnRequest += OnTurnRequest;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();

            m_tombs = new List<GameObject>();
            m_skeletons = new List<GameObject>();

            if (m_animation.skeletonAnimation == null) return;
 
            m_animation.skeletonAnimation.AnimationState.Event += HandleEvent;
        }



        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            GameEventMessage.SendEvent("Boss Gone");
            base.OnDestroyed(sender, eventArgs);
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

        private Vector2 BallisticVel()
        {
            //TEST
            m_info.seedSpitFX.GetComponent<IsolatedObjectPhysics2D>().gravity.gravityScale = m_gravityScale;
            //

            //Speed: 1.5 | Gravity Scale: 2 | Pos Offset: (0.25, 0.1)
            //var dir = m_targetInfo.position - new Vector2(transform.position.x + (m_posOffset.x * transform.localScale.x), transform.position.y + m_posOffset.y);
            //var h = (dir.x * dir.y) * ((dir.x * dir.y) * m_velOffset)/*+ dir.x > 0 ? dir.y : -dir.y*/;
            //dir.y = 0;
            //var dist = dir.magnitude;
            //dir.y = dist;
            //dist += h;
            //var vel = Mathf.Sqrt(dist * (m_info.seedSpitFX.GetComponent<IsolatedObjectPhysics2D>().gravity.gravityScale /* * (dist * 0.1f)*/));
            ////vel *= m_speed;
            //Debug.Log("projectile direction: " + dir.x);
            //return vel * new Vector2(dir.x, /*dir.y*/ 0).normalized;

            m_targetDistance = Vector2.Distance(m_targetInfo.position, m_seedSpitTF.position);
            //Debug.Log("Target Distance: " + m_targetDistance);
            //Speed: 3 | Gravity Scale: 3 | Pos Offset: (1, 0.4)
            var dir = (m_targetInfo.position - new Vector2(m_seedSpitTF.position.x, m_seedSpitTF.position.y));
            var h = dir.y;
            dir.y = 0;
            var dist = dir.magnitude;
            dir.y = dist;
            dist += h;

            var currentSpeed = m_speed;
            //Debug.Log("current Speed: " + currentSpeed);

            var vel = Mathf.Sqrt(dist * m_info.seedSpitFX.GetComponent<IsolatedObjectPhysics2D>().gravity.gravityScale);
            return (vel * new Vector3(dir.x * m_posOffset.x, dir.y * m_posOffset.y).normalized) * m_targetOffset.sqrMagnitude; //closest to accurate
        }

        private float GroundDistance()
        {
            RaycastHit2D hit = Physics2D.Raycast(m_seedSpitTF.position, Vector2.down, 1000, LayerMask.GetMask("Environment"));
            if (hit.collider != null)
            {
                return hit.distance;
            }

            return 0;
        }

        private Phase PhaseHandler(float health)
        {
            if(m_health.currentValue < m_maxHealth * 0.75f && m_phaseHealth == m_maxHealth)
            {
                m_phaseHealth = m_maxHealth * 0.75f;
                return Phase.Second;
            }
            else if (m_health.currentValue < m_maxHealth * 0.50f && m_phaseHealth == m_maxHealth * 0.75f)
            {
                m_phaseHealth = m_maxHealth * 0.50f;
                return Phase.Third;
            }
            else if (m_health.currentValue < m_maxHealth * 0.25f && m_phaseHealth == m_maxHealth * 0.50f)
            {
                m_phaseHealth = 0;
                return Phase.Final;
            }
            else
            {
                return Phase.Wait;
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
            m_currentState = State.Chasing;
        }

        private IEnumerator GroundAttackRoutine()
        {
            m_waitRoutineEnd = true;
            m_animation.SetAnimation(0, m_info.groundSlam.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, AwakenedAncientAnimation.ANIMATION_GROUND_SLAM);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_waitRoutineEnd = false;
            m_currentState = State.ReevaluateSituation;
            yield return null;
        }

        private IEnumerator TombAttackRoutine(Vector3 target, int tombSize)
        {
            m_waitRoutineEnd = true;
            m_hitbox.SetInvulnerability(true);
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
            m_animation.SetAnimation(0, m_info.burrowAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.burrowAnimation);
            //Debug.Log("Summon Tombs");
            for (int i = 0; i < tombSize; i++)
            {
                GameObject tomb = Instantiate(m_info.tombAttackGO, new Vector2(target.x + UnityEngine.Random.Range(-10, 10), target.y - 2.5f), Quaternion.identity);
                tomb.GetComponent<TombAttack>().GetTarget(m_targetInfo, m_currentTombVolleys);
                m_tombs.Add(tomb);
            }
            //m_animation.SetAnimation(0, m_info.burrowIdleAnimation, true);
            //yield return null;
            //yield return new WaitForSeconds(5f);
            while (m_tombs.Count > 0)
            {
                for (int i = 0; i < m_tombs.Count; i++)
                {
                    if (m_tombs[i] == null)
                    {
                        m_tombs.RemoveAt(i);
                    }
                }
                yield return null;
            }
            m_animation.skeletonAnimation.skeleton.SetSkin(m_skinName[m_chosenSkin]);
            //Debug.Log("Waited for Tomb Attack to Finish");
            m_animation.SetAnimation(0, m_info.unburrowAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.unburrowAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_hitbox.SetInvulnerability(false);
            m_isPhaseChanging = false;
            m_waitRoutineEnd = false;
            m_currentState = State.ReevaluateSituation;
            yield return null;
            m_tombs.Clear();
        }

        private IEnumerator SkeletonSummonRoutine(int skeletonSize)
        {
            if (m_currentSkeletonSize > m_skeletons.Count)
            {
                m_waitRoutineEnd = true;
                m_animation.SetAnimation(0, m_info.skeletonSummon.animation, false);
                for (int i = 0; i < m_summonFX.Count; i++)
                {
                    Debug.Log("SUMMON FX " + i);
                    m_summonFX[i].Play();
                    var mainFx = m_summonFX[i].main;
                    mainFx.simulationSpeed = 2.5f;
                }
                yield return new WaitForAnimationComplete(m_animation.animationState, AwakenedAncientAnimation.ANIMATION_SPIT_SKELETON);
                for (int i = 0; i < m_summonFX.Count; i++)
                {
                    m_summonFX[i].Stop();
                }
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                m_waitRoutineEnd = false;
                m_currentState = State.ReevaluateSituation;
            }
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
                if (IsFacingTarget())
                {
                    GameObject obj = Instantiate(m_info.mouthSpitFX, m_seedSpitTF.position, Quaternion.identity);
                    obj.transform.localScale = new Vector3(obj.transform.localScale.x * transform.localScale.x, obj.transform.localScale.y, obj.transform.localScale.z);
                    obj.transform.parent = m_seedSpitTF;
                    obj.transform.localPosition = new Vector2(4, -1.5f);

                    //Shoot Spit
                    var target = m_targetInfo.position; //No Parabola
                                                        //var target = TargetParabola(); //With Parabola
                    target = new Vector2(target.x, target.y - 2);
                    Vector2 spitPos = new Vector2(transform.localScale.x < 0 ? m_seedSpitTF.position.x - 1.5f : m_seedSpitTF.position.x + 1.5f, m_seedSpitTF.position.y -0.75f);
                    Vector3 v_diff = (target - spitPos);
                    float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
                    //transform.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg);
                    //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    //GameObject shoot = Instantiate(m_info.seedSpitFX, spitPos, Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg)); //No Parabola
                    //shoot.GetComponent<Rigidbody2D>().AddForce((m_spitSpeed + (Vector2.Distance(target, transform.position) * 0.35f)) * shoot.transform.right, ForceMode2D.Impulse);

                    GameObject projectile = Instantiate(m_info.seedSpitFX, spitPos, Quaternion.identity);
                    //projectile.GetComponent<IsolatedObjectPhysics2D>().SetVelocity(BallisticVel());
                    projectile.GetComponent<IsolatedObjectPhysics2D>().AddForce(BallisticVel(), ForceMode2D.Impulse);
                }
                else
                {
                    m_waitRoutineEnd = false;
                    m_currentState = State.Turning;
                }
            }
            else if (e.Data.Name == m_eventName[3])
            {
                //Debug.Log(m_eventName[3]);

                GameObject obj = Instantiate(m_info.stompFX, new Vector2(m_stompTF.position.x + (0.5f * transform.localScale.x), transform.position.y - 1f), Quaternion.identity);
                GameObject obj2 = Instantiate(m_info.crawlingVineFX, new Vector2(m_stompTF.position.x + (0.5f * transform.localScale.x), transform.position.y), Quaternion.identity);
                obj2.transform.localScale = new Vector3(obj2.transform.localScale.x * transform.localScale.x, obj2.transform.localScale.y, obj2.transform.localScale.z);
                obj2.GetComponent<Rigidbody2D>().AddForce(new Vector2(m_vineCrawlSpeed * transform.localScale.x, 0), ForceMode2D.Impulse);
                //obj2.GetComponent<Rigidbody2D>().velocity = new Vector2(m_vineCrawlSpeed * transform.localScale.x, 0);
            }
            else if (e.Data.Name == m_eventName[4])
            {
                Debug.Log(m_eventName[4]);
            }
            else if (e.Data.Name == m_eventName[5])
            {
                //Debug.Log(m_eventName[5]);
                GameObject skeleton = Instantiate(m_info.skeletonGO, new Vector2(m_skeletonSpawnTF.position.x + /*(3 * transform.localScale.x)*/ +UnityEngine.Random.Range(-2, 2), m_skeletonSpawnTF.position.y), Quaternion.identity);
                skeleton.GetComponent<SkeletonSpawnAI>().SetDirection(transform.localScale.x);
                GameObject skeletonFX = Instantiate(m_info.skeletonSpawnFX, skeleton.transform.position, Quaternion.identity);
                m_skeletons.Add(skeleton);
            }
            else if (e.Data.Name == m_eventName[6])
            {
                //Debug.Log(m_eventName[5]);
                m_smokeFX.Play();
            }
        }

        private void Update()
        {
            if (!m_isPhaseChanging)
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
                            m_attackDecider.DecideOnAttack();
                            if (m_attackDecider.hasDecidedOnAttack)
                            {
                                switch (m_attackDecider.chosenAttack.attack)
                                {
                                    case Attack.GroundSlam:
                                        if (Wait() && !m_wallSensor.isDetecting)
                                        {
                                            //m_attackHandle.ExecuteAttack(m_info.groundSlam.animation);
                                            StartCoroutine(GroundAttackRoutine());
                                            WaitTillAttackEnd(Attack.GroundSlam);
                                        }
                                        break;
                                    case Attack.Spit:
                                        if (Wait() && !m_wallSensor.isDetecting)
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
                                        //if (Wait())
                                        //{
                                        //    if (Vector2.Distance(target, transform.position) >= m_info.groundSlam.range - 10)
                                        //    {
                                        //        StartCoroutine(TombAttackRoutine(target, m_tombSize[0]));
                                        //        WaitTillAttackEnd(Attack.Tomb);
                                        //    }
                                        //}
                                        break;
                                    case Attack.SkeletonSummon:
                                        if (Wait())
                                        {
                                            if (Vector2.Distance(target, transform.position) >= m_info.skeletonSummon.range - 10)
                                            {
                                                StartCoroutine(SkeletonSummonRoutine(m_currentSkeletonSize));
                                                WaitTillAttackEnd(Attack.SkeletonSummon);
                                            }
                                        }
                                        break;
                                }
                                m_attackDecider.hasDecidedOnAttack = false;
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

                                if (!m_wallSensor.isDetecting && m_groundSensor.allRaysDetecting)
                                {
                                    if (Wait())
                                    {
                                        m_animation.EnableRootMotion(true, false);
                                        m_animation.SetAnimation(0, m_info.moveAnimation, true);
                                    }
                                }
                                else
                                {
                                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
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
            }

            //m_animation.skeletonAnimation.initialSkinName = m_skinName[0];
            switch (PhaseHandler(m_health.currentValue))
            {
                case Phase.Second:
                    m_chosenSkin = 1;
                    m_currentTombSize = m_tombSize[0];
                    m_currentSkeletonSize = m_skeletonSize[1];
                    m_currentSummonThreshhold = m_summonThreshhold[1];
                    m_currentTombVolleys = m_tombVolleys[0];
                    if (!m_isPhaseChanging)
                    {
                        m_isPhaseChanging = true;
                        StartCoroutine(TombAttackRoutine(m_targetInfo.position, m_currentTombSize));
                    }
                    break;
                case Phase.Third:
                    m_chosenSkin = 2;
                    m_currentTombSize = m_tombSize[1];
                    m_currentSkeletonSize = m_skeletonSize[2];
                    m_currentSummonThreshhold = m_summonThreshhold[2];
                    m_currentTombVolleys = m_tombVolleys[1];
                    if (!m_isPhaseChanging)
                    {
                        m_isPhaseChanging = true;
                        StartCoroutine(TombAttackRoutine(m_targetInfo.position, m_currentTombSize));
                    }
                    break;
                case Phase.Final:
                    m_chosenSkin = 3;
                    m_currentTombSize = m_tombSize[2];
                    m_currentSkeletonSize = m_skeletonSize[3];
                    m_currentSummonThreshhold = m_summonThreshhold[3];
                    m_currentTombVolleys = m_tombVolleys[2];
                    if (!m_isPhaseChanging)
                    {
                        m_isPhaseChanging = true;
                        StartCoroutine(TombAttackRoutine(m_targetInfo.position, m_currentTombSize));
                    }
                    break;
                default:
                    break;
            }

            for (int i = 0; i < m_skeletons.Count; i++)
            {
                if (m_skeletons[i].activeSelf == false)
                {
                    m_skeletons.RemoveAt(i);
                }
            }
        }
    }
}