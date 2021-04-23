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
            [SerializeField]
            private PhaseInfo<Phase> m_phaseInfo;
            public PhaseInfo<Phase> phaseInfo => m_phaseInfo;

            [Title("Animations")]
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
            private string m_screamAnimation;
            public string screamAnimation => m_screamAnimation;
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

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_footEvent;
            public string footEvent => m_footEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_saAnticipationEvent;
            public string saAnticipationEvent => m_saAnticipationEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_seedSpitEvent;
            public string seedSpitEvent => m_seedSpitEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_stompEvent;
            public string stompEvent => m_stompEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_skeletonAnticipationEvent;
            public string skeletonAnticipationEvent => m_skeletonAnticipationEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_spawnSkeletonEvent;
            public string spawnSkeletonEvent => m_spawnSkeletonEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_smokeEvent;
            public string smokeEvent => m_smokeEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_screamStartEvent;
            public string screamStartEvent => m_screamStartEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_screamEndEvent;
            public string screamEndEvent => m_screamEndEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_frontLeftFoodAudioEvent;
            public string frontLeftFoodAudioEvent => m_frontLeftFoodAudioEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_backLeftFoodAudioEvent;
            public string backLeftFoodAudioEvent => m_backLeftFoodAudioEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_frontRightFoodAudioEvent;
            public string frontRightFoodAudioEvent => m_frontRightFoodAudioEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_backRightFoodAudioEvent;
            public string backRightFoodAudioEvent => m_backRightFoodAudioEvent;

            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

            [Title("Attack Behaviours")]
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


            [Title("Prefabs and Shit")]
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

        [System.Serializable]
        public class PhaseInfo : IPhaseInfo
        {
            [SerializeField]
            private int m_tombVolley;
            public int tombVolley => m_tombVolley;
            [SerializeField]
            private int m_tombSize;
            public int tombSize => m_tombSize;
            [SerializeField]
            private int m_skeletonNum;
            public int skeletonNum => m_skeletonNum;
            [SerializeField, ValueDropdown("GetSkins")]
            private string m_skin;
            public string skin => m_skin;
            [SerializeField]
            private int m_phaseIndex;
            public int phaseIndex => m_phaseIndex;

            [SerializeField, PreviewField]
            protected SkeletonDataAsset m_skeletonDataAsset;

            protected IEnumerable GetSkins()
            {
                ValueDropdownList<string> list = new ValueDropdownList<string>();
                var reference = m_skeletonDataAsset.GetAnimationStateData().SkeletonData.Skins.ToArray();
                for (int i = 0; i < reference.Length; i++)
                {
                    list.Add(reference[i].Name);
                }
                return list;
            }
        }

        private enum State
        {
            Idle,
            Turning,
            Attacking,
            Chasing,
            Phasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            GroundSlam,
            Spit,
            SkeletonSummon,
            WaitAttackEnd,
        }

        public enum Phase
        {
            First,
            Second,
            Third,
            Final,
            Wait,
        }

        [SerializeField, TabGroup("Reference")]
        private Boss m_boss;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;

        [SerializeField]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField]
        private MovementHandle2D m_movementHandle;
        [SerializeField]
        private AttackHandle m_attackHandle;
        [SerializeField]
        private DeathHandle m_deathHandle;

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
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private PhaseHandle<Phase, PhaseInfo> m_phaseHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;

        private bool m_burrowed;

        private List<SkeletonSpawnAI> m_skeletons;
        private List<GameObject> m_tombs;
        private List<GameObject> m_tombSouls;

        [SerializeField]
        private List<ParticleSystem> m_summonFX;
        [SerializeField]
        private ParticleSystem m_smokeFX;
        [SerializeField]
        private ParticleSystem m_screamFX;
        [SerializeField]
        private ParticleSystem m_screamSpitFX;
        [SerializeField]
        private ParticleSystem m_footFX;

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

        private int m_currentTombVolleys;
        private int m_currentTombSize;
        private int m_currentSkeletonSize;
        private int m_currentSummonThreshhold;
        private string m_currentSkin;
        private int m_currentPhaseIndex;

        private float m_targetDistance;

        protected override void Start()
        {
            base.Start();
            m_burrowed = true;

            m_spineEventListener.Subscribe(m_info.footEvent, m_footFX.Play);
            m_spineEventListener.Subscribe(m_info.saAnticipationEvent, AnticipationPlay);
            m_spineEventListener.Subscribe(m_info.seedSpitEvent, SeedSpit);
            m_spineEventListener.Subscribe(m_info.stompEvent, Stomp);
            m_spineEventListener.Subscribe(m_info.skeletonAnticipationEvent, SpawnSkeletonFX);
            m_spineEventListener.Subscribe(m_info.spawnSkeletonEvent, SpawnSkeleton);
            m_spineEventListener.Subscribe(m_info.smokeEvent, m_smokeFX.Play);
            m_spineEventListener.Subscribe(m_info.screamStartEvent, m_screamSpitFX.Play);
            m_spineEventListener.Subscribe(m_info.screamEndEvent, m_screamSpitFX.Stop);

            m_phaseHandle = new PhaseHandle<Phase, PhaseInfo>();
            m_phaseHandle.Initialize(Phase.First, m_info.phaseInfo, m_character, ChangeState, ApplyPhaseData);
            m_phaseHandle.ApplyChange();

            m_animation.skeletonAnimation.skeleton.SetSkin(m_currentSkin);

            m_info.seedSpitFX.GetComponent<IsolatedObjectPhysics2D>().gravity.gravityScale = m_gravityScale;
            //GameplaySystem.SetBossHealth(m_character);
        }

        private void ApplyPhaseData(PhaseInfo obj)
        {
            Debug.Log("Change Phase");
            m_currentTombVolleys = obj.tombVolley;
            m_currentTombSize = obj.tombSize;
            m_currentSkeletonSize = obj.skeletonNum;
            m_currentSkin = obj.skin;
            m_currentPhaseIndex = obj.phaseIndex;

        }

        private void ChangeState()
        {
            StopAllCoroutines();
            m_stateHandle.OverrideState(State.Phasing);
        }

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
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
                                    new AttackInfo<Attack>(Attack.Spit, m_info.spit.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        protected override void Awake()
        {
            base.Awake();
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();

            m_tombs = new List<GameObject>();
            //m_tombSouls = new List<GameObject>();
            m_skeletons = new List<SkeletonSpawnAI>();
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            GameEventMessage.SendEvent("Boss Gone");
            base.OnDestroyed(sender, eventArgs);
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
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
            target = new Vector2(target.x, target.y - 2);
            Vector2 spitPos = m_seedSpitTF.position;
            Vector3 v_diff = (target - spitPos);
            float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
            GameObject shoot = Instantiate(m_info.seedSpitFX, spitPos, Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg)); //No Parabola
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
            m_info.seedSpitFX.GetComponent<IsolatedObjectPhysics2D>().gravity.gravityScale = m_gravityScale;

            m_targetDistance = Vector2.Distance(m_targetInfo.position, m_seedSpitTF.position);
            var dir = (m_targetInfo.position - new Vector2(m_seedSpitTF.position.x, m_seedSpitTF.position.y));
            var h = dir.y;
            dir.y = 0;
            var dist = dir.magnitude;
            dir.y = dist;
            dist += h;

            var currentSpeed = m_speed;

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

        private IEnumerator BurrowRoutine()
        {
            m_animation.SetAnimation(0, m_info.burrowAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.burrowAnimation);
            m_animation.SetAnimation(0, m_info.burrowIdleAnimation, true);
            yield return null;
        }

        private IEnumerator UnburrowRoutine()
        {
            m_animation.SetAnimation(0, m_info.unburrowAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.unburrowAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            yield return null;
            m_stateHandle.OverrideState(State.Chasing);
        }

        private IEnumerator GroundAttackRoutine()
        {
            m_animation.SetAnimation(0, m_info.groundSlam.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.groundSlam.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        private IEnumerator ChangePhaseRoutine()
        {
            m_phaseHandle.ApplyChange();
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            while (m_skeletons.Count > 0)
            {
                for (int i = 0; i < m_skeletons.Count; i++)
                {
                    StartCoroutine(m_skeletons[i].GetComponent<SkeletonSpawnAI>().Die());
                    m_skeletons.RemoveAt(i);
                }
                yield return null;
            }
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
            m_animation.SetAnimation(0, m_info.screamAnimation, false);
            yield return new WaitForSeconds(.5f);
            m_screamFX.Play();
            m_boss.SendPhaseTriggered(m_currentPhaseIndex);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.screamAnimation);

            yield return TombAttackRoutine(/*m_targetInfo.position*/);
        }

        private IEnumerator TombAttackRoutine(/*Vector3 target*/)
        {
            m_animation.SetAnimation(0, m_info.burrowAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.burrowAnimation);
            for (int i = 0; i < m_currentTombSize; i++)
            {
                GameObject tomb = Instantiate(m_info.tombAttackGO, new Vector2(m_targetInfo.position.x + (UnityEngine.Random.Range(0, 1) == 0 ? UnityEngine.Random.Range(5, 10) : UnityEngine.Random.Range(-10, -5)), m_targetInfo.position.y - 2.5f), Quaternion.identity);
                var tombAttack = tomb.GetComponent<TombAttack>();
                tombAttack.GetTarget(m_targetInfo, m_currentTombVolleys, i);
                m_tombs.Add(tomb);
            }
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
            m_animation.skeletonAnimation.skeleton.SetSkin(m_currentSkin);
            m_animation.SetAnimation(0, m_info.unburrowAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.unburrowAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_tombs.Clear();
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        private void AnticipationPlay()
        {
            GameObject obj = Instantiate(m_info.anticipationFX, new Vector2(m_seedSpitTF.position.x + (1 * transform.localScale.x), m_seedSpitTF.position.y + .25f), Quaternion.identity);
            obj.transform.localScale = new Vector3(obj.transform.localScale.x * transform.localScale.x, obj.transform.localScale.y, obj.transform.localScale.z);
            obj.transform.parent = m_seedSpitTF;
        }

        private void SeedSpit()
        {
            if (IsFacingTarget())
            {
                GameObject obj = Instantiate(m_info.mouthSpitFX, m_seedSpitTF.position, Quaternion.identity);
                obj.transform.localScale = new Vector3(obj.transform.localScale.x * transform.localScale.x, obj.transform.localScale.y, obj.transform.localScale.z);
                obj.transform.parent = m_seedSpitTF;
                obj.transform.localPosition = new Vector2(4, -1.5f);

                //Shoot Spit
                var target = m_targetInfo.position;
                target = new Vector2(target.x, target.y - 2);
                Vector2 spitPos = new Vector2(transform.localScale.x < 0 ? m_seedSpitTF.position.x - 1.5f : m_seedSpitTF.position.x + 1.5f, m_seedSpitTF.position.y - 0.75f);
                Vector3 v_diff = (target - spitPos);
                float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);

                GameObject projectile = Instantiate(m_info.seedSpitFX, spitPos, Quaternion.identity);
                projectile.GetComponent<IsolatedObjectPhysics2D>().AddForce(BallisticVel(), ForceMode2D.Impulse);
            }
            else
            {
                m_stateHandle.OverrideState(State.Turning);
            }
        }

        private void Stomp()
        {
            GameObject obj = Instantiate(m_info.stompFX, new Vector2(m_stompTF.position.x + (0.5f * transform.localScale.x), transform.position.y - 1f), Quaternion.identity);
            GameObject obj2 = Instantiate(m_info.crawlingVineFX, new Vector2(m_stompTF.position.x + (0.5f * transform.localScale.x), transform.position.y), Quaternion.identity);
            obj2.transform.localScale = new Vector3(obj2.transform.localScale.x * transform.localScale.x, obj2.transform.localScale.y, obj2.transform.localScale.z);
            obj2.GetComponent<Rigidbody2D>().AddForce(new Vector2(m_vineCrawlSpeed * transform.localScale.x, 0), ForceMode2D.Impulse);
        }

        private void SpawnSkeletonFX()
        {
            for (int i = 0; i < m_summonFX.Count; i++)
            {
                m_summonFX[i].Play();
                var mainFx = m_summonFX[i].main;
                mainFx.simulationSpeed = 2.5f;
            }
        }

        private void SpawnSkeleton()
        {
            for (int i = 0; i < m_summonFX.Count; i++)
            {
                m_summonFX[i].Stop();
            }
            GameObject skeleton = Instantiate(m_info.skeletonGO, new Vector2(m_skeletonSpawnTF.position.x + /*(3 * transform.localScale.x)*/ +UnityEngine.Random.Range(-2, 2), m_skeletonSpawnTF.position.y), Quaternion.identity);
            var skeletonAI = skeleton.GetComponent<SkeletonSpawnAI>();
            skeletonAI.SetDirection(transform.localScale.x);
            skeletonAI.AddTarget(m_targetInfo.transform.gameObject);
            GameObject skeletonFX = Instantiate(m_info.skeletonSpawnFX, skeleton.transform.position, Quaternion.identity);
            m_skeletons.Add(skeletonAI);
        }

        private void Update()
        {
            m_phaseHandle.MonitorPhase();

            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    if (m_targetInfo.isValid)
                    {
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
                        ;
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
                case State.Phasing:
                    Debug.Log("Phasing");
                    m_stateHandle.OverrideState(State.WaitBehaviourEnd);
                    StartCoroutine(ChangePhaseRoutine());
                    break;
                case State.Turning:
                    if (Wait() /*&& !m_waitRoutineEnd*/)
                    {
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        m_movementHandle.Stop();
                        m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                    }
                    break;
                case State.Attacking:
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
                            case Attack.SkeletonSummon:
                                if (Wait())
                                {
                                    if (Vector2.Distance(target, transform.position) >= m_info.skeletonSummon.range - 10 && m_currentSkeletonSize > m_skeletons.Count)
                                    {
                                        //StartCoroutine(SkeletonSummonRoutine(m_currentSkeletonSize));
                                        m_animation.SetAnimation(0, m_info.skeletonSummon.animation, false);
                                        WaitTillAttackEnd(Attack.SkeletonSummon);
                                    }
                                }
                                break;
                        }
                        m_attackDecider.hasDecidedOnAttack = false;
                    }
                    break;
                case State.Chasing:
                    if (IsFacingTarget() && Vector2.Distance(m_targetInfo.position, transform.position) <= m_info.groundSlam.range)
                    {
                        m_stateHandle.SetState(State.Attacking);
                        m_movementHandle.Stop();
                    }
                    else if (IsFacingTarget() && Vector2.Distance(m_targetInfo.position, transform.position) >= m_info.groundSlam.range)
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
                        m_stateHandle.SetState(State.Turning);
                    }
                    //Play Animation
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

            for (int i = 0; i < m_skeletons.Count; i++)
            {
                if (m_skeletons[i].gameObject.activeSelf == false)
                {
                    m_skeletons.RemoveAt(i);
                }
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Idle);
        }

        protected override void OnBecomePassive()
        {
            throw new NotImplementedException();
        }
    }
}