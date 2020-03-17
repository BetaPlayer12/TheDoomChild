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
using Spine.Unity.Modules;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Boss/MotherMantis")]
    public class MotherMantisAI : CombatAIBrain<MotherMantisAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField]
            private PhaseInfo<Phase> m_phaseInfo;
            public PhaseInfo<Phase> phaseInfo => m_phaseInfo;

            //Basic Behaviours
            [SerializeField]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;
            [SerializeField]
            private MovementInfo m_moveLowHP = new MovementInfo();
            public MovementInfo moveLowHP => m_moveLowHP;

            [Title("Attack Cooldown")]
            [SerializeField]
            private float m_universalAttackCD;
            public float universalAttackCD => m_universalAttackCD;
            [Title("Attack Chances")]
            [SerializeField]
            private float m_universalAttackChance;
            public float universalAttackChance => m_universalAttackChance;
            [SerializeField]
            private float m_meleeAttackChance;
            public float meleeAttackChance => m_meleeAttackChance;
            
            [Title("Attack Behaviours")]
            [SerializeField]
            private SimpleAttackInfo m_attack1 = new SimpleAttackInfo();
            public SimpleAttackInfo attack1 => m_attack1;
            [SerializeField]
            private SimpleAttackInfo m_attack2 = new SimpleAttackInfo();
            public SimpleAttackInfo attack2 => m_attack2;
            [SerializeField]
            private SimpleAttackInfo m_attack2StepBack = new SimpleAttackInfo();
            public SimpleAttackInfo attack2StepBack => m_attack2StepBack;
            [SerializeField]
            private SimpleAttackInfo m_attack3 = new SimpleAttackInfo();
            public SimpleAttackInfo attack3 => m_attack3;
            [SerializeField]
            private SimpleAttackInfo m_attack4 = new SimpleAttackInfo();
            public SimpleAttackInfo attack4 => m_attack4;
            [SerializeField]
            private SimpleAttackInfo m_attack4b = new SimpleAttackInfo();
            public SimpleAttackInfo attack4b => m_attack4b;
            [SerializeField]
            private SimpleAttackInfo m_attack5 = new SimpleAttackInfo();
            public SimpleAttackInfo attack5 => m_attack5;

            [Title("Spawned Objects")]
            [SerializeField]
            private GameObject m_larvaBulb;
            public GameObject larvaBulb => m_larvaBulb;
            
            [Title("Misc")]
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

            [Title("Animations")]
            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchBackAnimation;
            public string flinchBackAnimation => m_flinchBackAnimation;

            [Title("Projectiles")]
            //[Title("Events")]
            //[SerializeField, ValueDropdown("GetEvents")]
            //private string m_mantisEvent;
            //public string mantisEvent => m_mantisEvent;
            [SerializeField]
            private SimpleProjectileAttackInfo m_petalProjectile;
            public SimpleProjectileAttackInfo petalProjectile => m_petalProjectile;
            //[SerializeField]
            //private SimpleProjectileAttackInfo m_beeProjectile;
            //public SimpleProjectileAttackInfo beeProjectile => m_beeProjectile;
            //[SerializeField]
            //private GameObject m_burstGO;
            //public GameObject burstGO => m_burstGO;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_move.SetData(m_skeletonDataAsset);
                m_moveLowHP.SetData(m_skeletonDataAsset);
                //m_spearProjectile.SetData(m_skeletonDataAsset);
                //m_beeProjectile.SetData(m_skeletonDataAsset);
                //
                m_attack1.SetData(m_skeletonDataAsset);
                m_attack2.SetData(m_skeletonDataAsset);
                m_attack2StepBack.SetData(m_skeletonDataAsset);
                m_attack3.SetData(m_skeletonDataAsset);
                m_attack4.SetData(m_skeletonDataAsset);
                m_attack4b.SetData(m_skeletonDataAsset);
                m_attack5.SetData(m_skeletonDataAsset);
                m_petalProjectile.SetData(m_skeletonDataAsset);
#endif
            }
        }

        [System.Serializable]
        public class PhaseInfo : IPhaseInfo
        {
            [SerializeField]
            private float m_petalAmount;
            public float petalAmount => m_petalAmount;
            [SerializeField]
            private float m_droneStrikeSummonSpeed;
            public float droneStrikeSummonSpeed => m_droneStrikeSummonSpeed;
            [SerializeField]
            private int m_droneSummonAmmount;
            public int droneSummonAmmount => m_droneSummonAmmount;
            //[SerializeField]
            //private int m_tombSize;
            //public int tombSize => m_tombSize;
            //[SerializeField]
            //private int m_skeletonNum;
            //public int skeletonNum => m_skeletonNum;
            //[SerializeField, ValueDropdown("GetSkins")]
            //private string m_skin;
            //public string skin => m_skin;
            [SerializeField]
            private int m_phaseIndex;
            public int phaseIndex => m_phaseIndex;

            //[SerializeField, PreviewField]
            //protected SkeletonDataAsset m_skeletonDataAsset;

            //protected IEnumerable GetSkins()
            //{
            //    ValueDropdownList<string> list = new ValueDropdownList<string>();
            //    var reference = m_skeletonDataAsset.GetAnimationStateData().SkeletonData.Skins.ToArray();
            //    for (int i = 0; i < reference.Length; i++)
            //    {
            //        list.Add(reference[i].Name);
            //    }
            //    return list;
            //}
        }


        private enum State
        {
            Phasing,
            Intro,
            Idle,
            Flinch,
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
            Attack2,
            Attack2StepBack,
            Attack3,
            Attack4,
            Attack4b,
            Attack5,
            WaitAttackEnd,
        }

        public enum Phase
        {
            PhaseOne,
            PhaseTwo,
            PhaseThree,
            PhaseFour,
            Wait,
        }

        [SerializeField, TabGroup("Reference")]
        private Boss m_boss;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_bodyCollider;
        [SerializeField, TabGroup("Reference")]
        private Transform m_modelTransform;
        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        //[SerializeField, TabGroup("Modules")]
        //private PatrolHandle m_patrolHandle;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_petalStartFX;
        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_petalLoopFX;
        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_petalEndFX;
        [SerializeField]
        private SpineEventListener m_spineListener;

        private Transform m_stingerPos;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        State m_turnState;
        //[ShowInInspector]
        private PhaseHandle<Phase, PhaseInfo> m_phaseHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_previousAttack;
        private Attack m_chosenAttack;

        private ProjectileLauncher m_petalLauncher;
        private ProjectileLauncher m_spearLauncher;


        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_currentSpawnPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_petalProjectileSpawnPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_larvaSpawnPoint;

        private float m_groundPosition;
        private List<Vector2> m_targetPositions;

        private bool m_stickToGround;
        private float m_currentCD;

        //private Vector2 m_testTarget;


        private int m_currentPhaseIndex;
        private float m_currentPetalAmount;
        private float m_currentSummonSpeed;
        private int m_currentSummonAmmount;
        //private float m_currentDroneSummonSpeed;
        float m_currentRecoverTime;
        bool m_isPhasing;

        private string m_moveAnim;
        private float m_moveSpeed;

        private void ApplyPhaseData(PhaseInfo obj)
        {
            //Debug.Log("Change Phase");
            //m_currentTombVolleys = obj.tombVolley;
            //m_currentTombSize = obj.tombSize;
            //m_currentSkeletonSize = obj.skeletonNum;
            //m_currentSkin = obj.skin;
            m_currentPhaseIndex = obj.phaseIndex;
            m_currentPetalAmount = obj.petalAmount;
            m_currentSummonSpeed = obj.droneStrikeSummonSpeed;
            m_currentSummonAmmount = obj.droneSummonAmmount;
        }

        private void ChangeState()
        {
            StopAllCoroutines();
            m_stateHandle.OverrideState(State.Phasing);
        }

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            //m_stateHandle.ApplyQueuedState();
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.OverrideState(State.Cooldown);
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        //private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        //{
        //    StopAllCoroutines();
        //    m_stateHandle.OverrideState(State.WaitBehaviourEnd);
        //    //if (/*m_animation.GetCurrentAnimation(0).ToString() == m_info.spearThrowAttack.animation*/ m_currentPhaseIndex != 3)
        //    //{
        //    //    m_stateHandle.OverrideState(State.Fall);
        //    //}
        //    //else /*if (m_stateHandle.currentState != State.Fall)*/
        //    //{
        //    //    m_animation.SetAnimation(0, IsFacingTarget() ? m_info.stuckStateFlinchForwardAnimaation : m_info.stuckStateFlinchBackwardAnimation, false);
        //    //    m_stateHandle.OverrideState(State.Stucc);
        //    //}
        //}

        //private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        //{
        //    //m_stateHandle.OverrideState(State.Stucc);
        //    m_stateHandle.OverrideState(State.ReevaluateSituation);
        //}

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                m_stateHandle.OverrideState(State.Intro);
                GameEventMessage.SendEvent("Boss Encounter");

                //m_testTarget = m_targetInfo.position;
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_animation.animationState.TimeScale = 1f;
            if (!m_isPhasing)
                m_stateHandle.ApplyQueuedState();
        }

        private void CustomTurn()
        {
            if (!IsFacingTarget())
            {
                //m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
                m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
            }
        }

        private IEnumerator IntroRoutine()
        {
            //CustomTurn();
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.attack1.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack1.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            //m_stateHandle.SetState(State.ReevaluateSituation);
            yield return null;
        }

        private IEnumerator ChangePhaseRoutine()
        {
            m_phaseHandle.ApplyChange();
            m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            m_currentCD = 0;
            m_isPhasing = true;
            //m_stateHandle.Wait(State.ReevaluateSituation);
            m_animation.animationState.TimeScale = 1f;
            m_movement.Stop();
            m_bodyCollider.SetActive(false);
            m_hitbox.SetInvulnerability(true);
            m_animation.EnableRootMotion(true, false);
            //m_turnState = State.Phasing;
            var flinchAnim = IsFacingTarget() ? m_info.flinchAnimation : m_info.flinchBackAnimation;
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
            m_animation.SetAnimation(0, m_info.attack1.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack1.animation);
            m_isPhasing = false;
            m_bodyCollider.SetActive(true);
            m_hitbox.SetInvulnerability(false);
            m_animation.DisableRootMotion();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            yield return null;
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            //transform.position = new Vector2(transform.position.x, m_groundPosition);
            m_stickToGround = true;
            StartCoroutine(LeapStickToGroundRoutine(m_groundPosition));
            m_movement.Stop();
        }

        #region Attacks

        #region PetalAttack
        private void LaunchPetalProjectile(Vector2 target, Transform spawnPoint)
        {
            //if (!IsFacingTarget())
            //{
            //    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            //    m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
            //}
            m_petalLauncher = new ProjectileLauncher(m_info.petalProjectile.projectileInfo, spawnPoint);
            m_petalLauncher.AimAt(target);
            m_petalLauncher.LaunchProjectile();
        }

        private Vector2 CalculatePositions()
        {
            var target = m_targetInfo.position;
            var point = new Vector2(UnityEngine.Random.Range(-20, 20) + target.x, UnityEngine.Random.Range(-20, 20) + target.y); //Locked to Ground
            return point;
        }

        private IEnumerator PetalFXRoutine(Vector2 target)
        {
            m_petalStartFX.Play();
            yield return new WaitForSeconds(1.25f);
            m_petalEndFX.Play();
            for (int i = 0; i < m_currentPetalAmount; i++)
            {
                //if (IsFacing(m_targetInfo.position))
                //{
                //    m_targetPositions.Add(CalculatePositions());
                //}
                //var point = new Vector2(UnityEngine.Random.Range(-10, 10) + target.x, UnityEngine.Random.Range(-10, 10) + target.y); //Precise
                var xOffset = (m_targetPositions[i].x - target.x) * .2f;
                //var yOffset = point.y - target.y; //Precise
                var yOffset = (m_targetPositions[i].y - transform.position.y) * .2f; //Locked to Ground
                //m_currentSpawnPoint.position = new Vector2(UnityEngine.Random.Range(-5, 5) + m_petalProjectileSpawnPoint.position.x, UnityEngine.Random.Range(-5, 5) + m_petalProjectileSpawnPoint.position.y); //Random
                m_currentSpawnPoint.position = new Vector2(xOffset + m_petalProjectileSpawnPoint.position.x, yOffset + m_petalProjectileSpawnPoint.position.y); //In a straight path
                yield return new WaitForSeconds(.05f);
                LaunchPetalProjectile(m_targetPositions[i], m_currentSpawnPoint);
            }
            m_targetPositions.Clear();
            //LaunchPetalProjectile(target);
            yield return null;
        }

        private IEnumerator PetalLaunchRoutine()
        {
            m_stateHandle.Wait(State.Cooldown);
            //StartCoroutine(PetalFXRoutine());
            m_animation.SetAnimation(0, m_info.attack4.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack4.animation);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        #endregion

        #region LeapAttack
        private IEnumerator LeapStickToGroundRoutine(float groundPoint)
        {
            while (m_stickToGround)
            {
                //Debug.Log("Sticking to Ground");
                transform.position = new Vector2(transform.position.x, groundPoint);
                yield return null;
            }
        }

        private IEnumerator LeapAttackRoutine()
        {
            m_stateHandle.Wait(State.Cooldown);
            m_hitbox.SetInvulnerability(true);
            m_stickToGround = true;
            m_animation.SetAnimation(0, m_info.attack2.animation, false);
            yield return new WaitForSeconds(1.5f);
            transform.position = new Vector2(m_targetInfo.position.x, transform.position.y - 5);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack2.animation);
            m_hitbox.SetInvulnerability(false);
            m_stickToGround = false;
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        #endregion

        #region LarvaBulbAttack
        private IEnumerator SpawnLarvaRoutine()
        {
            yield return new WaitForSeconds(1f);
            var bulb = Instantiate(m_info.larvaBulb, m_larvaSpawnPoint.position, Quaternion.identity);
            yield return null;
        }

        private IEnumerator SpawnLarvaBulbRoutine()
        {
            m_stateHandle.Wait(State.Cooldown);
            m_animation.SetAnimation(0, m_info.attack3.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack3.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        #endregion

        #endregion
        #region Movement
        private void MoveToTarget()
        {
            if (!IsTargetInRange(m_info.attack1.range) && m_groundSensor.isDetecting /*&& !m_wallSensor.isDetecting && m_edgeSensor.isDetecting*/)
            {
                m_animation.EnableRootMotion(false, false);
                m_animation.SetAnimation(0, m_moveAnim, true);
                //m_movement.MoveTowards(m_targetInfo.position, m_info.move.speed * transform.localScale.x);
                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_moveSpeed);
            }
            else
            {
                m_movement.Stop();
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
        }
        #endregion

        private bool AllowAttack(int phaseIndex)
        {
            if (m_currentPhaseIndex >= phaseIndex)
            {
                return true;
            }
            else
            {
                m_attackDecider.hasDecidedOnAttack = false;
                m_stateHandle.OverrideState(State.ReevaluateSituation);
                return false;
            }
        }


        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Attack1, m_info.attack1.range),
                                    new AttackInfo<Attack>(Attack.Attack2, m_info.attack2.range),
                                    //new AttackInfo<Attack>(Attack.Attack2StepBack, m_info.attack2StepBack.range),
                                    new AttackInfo<Attack>(Attack.Attack3, m_info.attack3.range),
                                    new AttackInfo<Attack>(Attack.Attack4, m_info.attack4.range)/*,*/
                                    /*new AttackInfo<Attack>(Attack.Attack4b, m_info.attack4b.range),
                                    new AttackInfo<Attack>(Attack.Attack5, m_info.attack5.range)*/);
            m_attackDecider.hasDecidedOnAttack = false;
        }

        public override void ApplyData()
        {
            if (m_attackDecider != null)
            {
                UpdateAttackDeciderList();
            }
            //if (m_info != null)
            //{
            //    m_spineListener.Unsubcribe(m_info.spearProjectile.launchOnEvent, m_stingerLauncher.LaunchProjectile);
            //}
            //if (m_stingerLauncher != null)
            //{
            //    m_stingerLauncher.SetProjectile(m_info.spea.projectileInfo);
            //    m_spineListener.Subscribe(m_info.stingerProjectile.launchOnEvent, m_stingerLauncher.LaunchProjectile);
            //}
            base.ApplyData();
        }

        protected override void Awake()
        {
            base.Awake();
            //m_patrolHandle.TurnRequest += OnTurnRequest;
            //m_flinchHandle.FlinchStart += OnFlinchStart;
            //m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_attackHandle.AttackDone += OnAttackDone;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_attackDecider = new RandomAttackDecider<Attack>();
            UpdateAttackDeciderList();
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
        }

        protected override void Start()
        {
            base.Start();
            //m_flinchHandle.gameObject.SetActive(false);
            //m_spineListener.Subscribe(m_info.mantisEvent, LaunchProjectile);
            m_animation.DisableRootMotion();
            m_moveAnim = m_info.move.animation;
            m_moveSpeed = m_info.move.speed;
            m_targetPositions = new List<Vector2>();
            m_groundPosition = transform.position.y;

            m_phaseHandle = new PhaseHandle<Phase, PhaseInfo>();
            m_phaseHandle.Initialize(Phase.PhaseOne, m_info.phaseInfo, m_character, ChangeState, ApplyPhaseData);
            m_phaseHandle.ApplyChange();
        }

        private void Update()
        {
            m_phaseHandle.MonitorPhase();
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    break;
                case State.Intro:
                    if (IsFacingTarget())
                    {
                        StartCoroutine(IntroRoutine());
                    }
                    else
                    {
                        m_turnState = State.Intro;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    break;
                case State.Phasing:
                    //m_stateHandle.OverrideState(State.WaitBehaviourEnd);
                    StartCoroutine(ChangePhaseRoutine());
                    break;
                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    //m_animation.animationState.TimeScale = 2f;
                    m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                    m_movement.Stop();
                    break;
                case State.Attacking:
                    //StartCoroutine(TripleBeeDroneRoutine());
                    //MoveToAttackPosition(m_chosenAttack);
                    //m_stateHandle.Wait(State.ReevaluateSituation);

                    float chance = UnityEngine.Random.Range(0, 100);
                    if (chance < m_info.meleeAttackChance && IsTargetInRange(m_info.attack1.range))
                    {
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        m_attackHandle.ExecuteAttack(m_info.attack1.animation, m_info.idleAnimation);
                    }
                    else
                    {
                        //Debug.Log("Current Chance to Use Skill: " + chance);
                        //Debug.Log("Chance needed to Use Skill: " + m_info.universalAttackChance);
                        if (chance < m_info.universalAttackChance)
                        {
                            switch (m_attackDecider.chosenAttack.attack)
                            {
                                case Attack.Attack1:
                                    //m_animation.EnableRootMotion(true, false);
                                    m_attackHandle.ExecuteAttack(m_info.attack1.animation, m_info.idleAnimation);
                                    break;
                                case Attack.Attack2:
                                    //m_animation.EnableRootMotion(true, false);
                                    //m_attackHandle.ExecuteAttack(m_info.attack2.animation, m_info.idleAnimation);
                                    StartCoroutine(LeapAttackRoutine());
                                    StartCoroutine(LeapStickToGroundRoutine(m_groundPosition));
                                    break;
                                case Attack.Attack2StepBack:
                                    //m_animation.EnableRootMotion(true, false);
                                    m_attackHandle.ExecuteAttack(m_info.attack2StepBack.animation, m_info.idleAnimation);
                                    break;
                                case Attack.Attack3:
                                    //m_animation.EnableRootMotion(true, false);
                                    if (AllowAttack(3))
                                    {
                                        StartCoroutine(SpawnLarvaRoutine());
                                        StartCoroutine(SpawnLarvaBulbRoutine());
                                    }
                                    break;
                                case Attack.Attack4:
                                    //var target = m_targetInfo.position;
                                    if (AllowAttack(2))
                                    {
                                        for (int i = 0; i < m_currentPetalAmount; i++)
                                        {
                                            m_targetPositions.Add(CalculatePositions());
                                        }
                                        StartCoroutine(PetalFXRoutine(m_targetInfo.position));
                                        StartCoroutine(PetalLaunchRoutine());
                                    }
                                    break;
                                case Attack.Attack4b:
                                    //m_animation.EnableRootMotion(true, false);
                                    m_attackHandle.ExecuteAttack(m_info.attack4b.animation, m_info.idleAnimation);
                                    break;
                                case Attack.Attack5:
                                    //m_animation.EnableRootMotion(true, false);
                                    m_attackHandle.ExecuteAttack(m_info.attack5.animation, m_info.idleAnimation);
                                    break;
                            }
                        }
                        else
                        {
                            if (IsFacingTarget())
                            {
                                MoveToTarget();
                            }
                            else
                            {
                                m_turnState = State.Attacking;
                                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                                    m_stateHandle.SetState(State.Turning);
                            }
                        }
                    }

                    break;
                case State.Cooldown:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        if (!IsTargetInRange(m_info.targetDistanceTolerance))
                        {
                            MoveToTarget();
                        }
                        else
                        {
                            m_movement.Stop();
                            m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        }
                    }

                    if (m_currentCD <= m_info.universalAttackCD)
                    {
                        m_currentCD += Time.deltaTime;
                    }
                    else
                    {
                        m_attackDecider.hasDecidedOnAttack = false;
                        m_currentCD = 0;
                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                    }

                    break;

                case State.Chasing:

                    if (IsFacingTarget())
                    {
                        m_attackDecider.DecideOnAttack();
                        if (m_attackDecider.chosenAttack.attack == m_previousAttack)
                        {
                            m_attackDecider.hasDecidedOnAttack = false;
                        }
                        if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_attackDecider.chosenAttack.range) /*&& !m_wallSensor.allRaysDetecting*/)
                        {
                            StopAllCoroutines();
                            m_previousAttack = m_attackDecider.chosenAttack.attack;
                            //m_movement.Stop();
                            //m_animation.SetAnimation(0, m_info.idleAnimation, true);
                            m_stateHandle.SetState(State.Attacking);
                        }
                        else
                        {
                            m_attackDecider.hasDecidedOnAttack = false;
                            MoveToTarget();
                        }
                    }
                    else
                    {
                        m_turnState = State.ReevaluateSituation;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                            m_stateHandle.SetState(State.Turning);
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
            m_stickToGround = false;
            m_currentCD = 0;
        }
    }
}
