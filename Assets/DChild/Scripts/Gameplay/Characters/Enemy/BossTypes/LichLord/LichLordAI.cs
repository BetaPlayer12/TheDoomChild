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
using Spine.Unity.Examples;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Boss/LichLord")]
    public class LichLordAI : CombatAIBrain<LichLordAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField]
            private PhaseInfo<Phase> m_phaseInfo;
            public PhaseInfo<Phase> phaseInfo => m_phaseInfo;

            [SerializeField]
            private MovementInfo m_move1 = new MovementInfo();
            public MovementInfo move1 => m_move1;

            [SerializeField]
            private MovementInfo m_move2= new MovementInfo();
            public MovementInfo move2 => m_move2;


            [Title("Attack Behaviours")]
            [SerializeField, TabGroup("Phase 1")]
            private SimpleAttackInfo m_ghostOrbAttack = new SimpleAttackInfo();
            public SimpleAttackInfo ghostOrbAttack => m_ghostOrbAttack;
            [SerializeField, TabGroup("Phase 1")]
            private SimpleAttackInfo m_skeletalArmAttack = new SimpleAttackInfo();
            public SimpleAttackInfo skeletalArmAttack => m_skeletalArmAttack;
            [SerializeField, TabGroup("Phase 2")]
            private SimpleAttackInfo m_summonTotemAttack = new SimpleAttackInfo();
            public SimpleAttackInfo summonTotemAttack => m_summonTotemAttack;
            [SerializeField, TabGroup("Phase 2"), ValueDropdown("GetAnimations")]
            private string m_appearAnimation;
            public string appearAnimation => m_appearAnimation;
            [SerializeField, TabGroup("Phase 2"), ValueDropdown("GetAnimations")]
            private string m_vanishAnimation;
            public string vanishAnimation => m_vanishAnimation;
            [SerializeField, TabGroup("Phase 3")]
            private SimpleAttackInfo m_mapCurseAttack = new SimpleAttackInfo();
            public SimpleAttackInfo mapCurseAttack => m_mapCurseAttack;


            [Title("Misc")]
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

            [Title("Animations")]
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idle1Animation;
            public string idle1Animation => m_idle1Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idle2Animation;
            public string idle2Animation => m_idle2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;

            [Title("Summoned Assets")]
            [SerializeField]
            private GameObject m_skeletalArm;
            public GameObject skeletalArm => m_skeletalArm;
            [SerializeField]
            private GameObject m_totem;
            public GameObject totem => m_totem;

            [Title("Projectiles")]
            [SerializeField]
            private SimpleProjectileAttackInfo m_ghostOrbProjectile;
            public SimpleProjectileAttackInfo ghostOrbProjectile => m_ghostOrbProjectile;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_lichOrbStartFXEvent;
            public string lichOrbStartFXEvent => m_lichOrbStartFXEvent;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_move1.SetData(m_skeletonDataAsset);
                m_move2.SetData(m_skeletonDataAsset);
                m_ghostOrbAttack.SetData(m_skeletonDataAsset);
                m_skeletalArmAttack.SetData(m_skeletonDataAsset);
                m_summonTotemAttack.SetData(m_skeletonDataAsset);
                m_mapCurseAttack.SetData(m_skeletonDataAsset);
                m_ghostOrbProjectile.SetData(m_skeletonDataAsset);
#endif
            }
        }

        [System.Serializable]
        public class PhaseInfo : IPhaseInfo
        {
            [SerializeField]
            private int m_attackCount;
            public int attackCount => m_attackCount;
            //[SerializeField]
            //private List<int> m_patternAttackCount;
            //public List<int> patternAttackCount => m_patternAttackCount;
        }


        private enum State
        {
            Phasing,
            Intro,
            Idle,
            Turning,
            Attacking,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Pattern
        {
            AttackPattern1,
            AttackPattern2,
            //AttackPattern3,
            WaitAttackEnd,
        }

        private enum Attack
        {
            GhostOrb,
            SkeletalArm,
            SummonTotem,
            MapCurse,
            WaitAttackEnd,
        }

        public enum Phase
        {
            PhaseOne,
            PhaseTwo,
            PhaseThree,
            Wait,
        }

        private bool[] m_attackUsed;
        private List<Pattern> m_attackCache;

        [SerializeField, TabGroup("Reference")]
        private Boss m_boss;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;

        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private PathFinderAgent m_agent;
        [SerializeField, TabGroup("Modules")]
        private Health m_health;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_ghostOrbStartFX;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField]
        private Transform m_lichLordArmTF;

        //[SerializeField, TabGroup("Effects")]
        //private ParticleFX m_deathFX;
        //[SerializeField, TabGroup("Effects")]
        //private ParticleFX m_slashGroundFX;
        //[SerializeField, TabGroup("Effects")]
        //private ParticleFX m_scytheSpinFX;

        [SerializeField]
        private SpineEventListener m_spineListener;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        State m_turnState;
        [ShowInInspector]
        private PhaseHandle<Phase, PhaseInfo> m_phaseHandle;
        [ShowInInspector]
        private RandomAttackDecider<Pattern> m_patternDecider;
        private Pattern m_currentPattern;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_currentAttack;
        private ProjectileLauncher m_projectileLauncher;

        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_projectilePoint;

        private int m_hitCount;
        private bool m_hasPhaseChanged;
        private PhaseInfo m_phaseInfo;

        private void ApplyPhaseData(PhaseInfo obj)
        {
            m_phaseInfo = obj;
        }

        private void ChangeState()
        {
            if (!m_hasPhaseChanged)
            {
                m_hasPhaseChanged = true;
                StopAllCoroutines();
                m_animation.DisableRootMotion();
                m_animation.SetEmptyAnimation(0, 0);
                m_stateHandle.OverrideState(State.Phasing);
                m_phaseHandle.ApplyChange();
            }
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                m_stateHandle.OverrideState(State.Intro);
                GameEventMessage.SendEvent("Boss Encounter");
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            if (m_stateHandle.currentState != State.Phasing)
            {
                m_animation.animationState.TimeScale = 1f;
                m_stateHandle.ApplyQueuedState();
            }
        }

        private void CustomTurn()
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
        }

        private IEnumerator IntroRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_agent.Stop();
            m_hitbox.SetInvulnerability(true);
            CustomTurn();
            m_animation.SetAnimation(0, m_info.idle2Animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.idle2Animation);
            m_animation.SetAnimation(0, m_info.idle1Animation, true);
            m_hitbox.SetInvulnerability(false);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator ChangePhaseRoutine()
        {
            m_hitbox.SetInvulnerability(false);
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private Vector2 GroundPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down, 1000, LayerMask.GetMask("Environment"));
            return hit.point;
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            StopAllCoroutines();
            m_agent.Stop();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            //StopAllCoroutines();
            //m_attackDecider.hasDecidedOnAttack = false;
            //m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            m_hitCount++;
            if (m_hitCount == 5)
            {
                StopAllCoroutines();
                m_hitCount = 0;
                StartCoroutine(HollowFormRoutine());
            }
            Debug.Log("Got HIT");
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            //m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        #region Attacks
        //Attack Routines
        private IEnumerator GhostOrbAttackRoutine()
        {
            m_animation.SetAnimation(0, m_info.ghostOrbAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.ghostOrbAttack.animation);
            var randomAttack = UnityEngine.Random.Range(0, 2);
            m_animation.SetAnimation(0, randomAttack == 1 ? m_info.idle1Animation : m_info.idle2Animation, false);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private void LaunchOrb()
        {
            //for (int i = 0; i < 3; i++)
            //{
            //    m_projectileLauncher.AimAt(target);
            //    m_projectileLauncher.LaunchProjectile();
            //}
            StartCoroutine(LaunchOrbRoutine());
        }

        private IEnumerator LaunchOrbRoutine()
        {
            var numberOfProjectiles = 16;
            float angleStep = 360f / numberOfProjectiles;
            float angle = 0f;

            var target = new Vector2(m_targetInfo.position.x, m_targetInfo.position.y);
            for (int z = 0; z < numberOfProjectiles; z++)
            {
                Vector2 startPoint = new Vector2(m_projectilePoint.position.x, m_projectilePoint.position.y);
                float projectileDirXposition = startPoint.x + Mathf.Sin((angle * Mathf.PI) / 180) * 5;
                float projectileDirYposition = startPoint.y + Mathf.Cos((angle * Mathf.PI) / 180) * 5;

                Vector2 projectileVector = new Vector2(projectileDirXposition, projectileDirYposition);
                Vector2 projectileMoveDirection = (projectileVector - startPoint).normalized * m_info.ghostOrbProjectile.projectileInfo.speed;
                if (z >= 4 && z <= 8)
                {

                    GameObject projectile = m_info.ghostOrbProjectile.projectileInfo.projectile;
                    var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(projectile);
                    instance.transform.position = m_projectilePoint.position;
                    instance.transform.localScale = new Vector3(1, m_targetInfo.position.x > m_projectilePoint.position.x ? 1 : -1, 1);
                    var component = instance.GetComponent<Projectile>();
                    component.ResetState();
                    component.GetComponent<Rigidbody2D>().velocity = projectileMoveDirection;
                    Vector2 v = component.GetComponent<Rigidbody2D>().velocity;
                    var projRotation = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
                    component.transform.rotation = Quaternion.AngleAxis(projRotation, Vector3.forward);

                    yield return new WaitForSeconds(.1f);
                }
                angle += m_projectilePoint.position.x < target.x ? angleStep : -angleStep;
            }
            yield return null;
        }

        private IEnumerator SkeletalArmRoutine()
        {
            var randomAttack = UnityEngine.Random.Range(0, 2);
            var skeletamArmPos = new Vector2(m_targetInfo.position.x + (randomAttack == 1 ? 20 : -20), GroundPosition().y - 3);
            m_lichLordArmTF.position = skeletamArmPos;
            m_lichLordArmTF.GetComponentInChildren<SkeletonAnimation>().state.SetAnimation(0, "Phase_1_Arm_Attack", false);
            m_lichLordArmTF.localScale = new Vector3(m_targetInfo.position.x > m_lichLordArmTF.position.x ? 1 : -1, 1, 1);
            m_animation.SetAnimation(0, m_info.skeletalArmAttack.animation, false);
            yield return new WaitForSeconds(.1f);
            m_lichLordArmTF.GetComponentInChildren<SkeletonRenderer>().maskInteraction = SpriteMaskInteraction.None;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.skeletalArmAttack.animation);
            m_animation.SetAnimation(0, randomAttack == 1 ? m_info.idle1Animation : m_info.idle2Animation, false);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator SummonTotem()
        {
            var randomAttack = UnityEngine.Random.Range(0, 2);
            var totemPos = new Vector2(m_targetInfo.position.x + (randomAttack == 1 ? 10 : -10), GroundPosition().y);
            var totem = Instantiate(m_info.totem, totemPos, Quaternion.identity);
            m_animation.SetAnimation(0, m_info.summonTotemAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.summonTotemAttack.animation);
            m_animation.SetAnimation(0, randomAttack == 1 ? m_info.idle1Animation : m_info.idle2Animation, false);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator MapCurse()
        {
            m_animation.SetAnimation(0, m_info.mapCurseAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.mapCurseAttack.animation);
            var randomAttack = UnityEngine.Random.Range(0, 2);
            m_animation.SetAnimation(0, randomAttack == 1 ? m_info.idle1Animation : m_info.idle2Animation, false);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        #endregion

        #region Movement
        private void DynamicMovement(Vector2 target)
        {
            if (IsFacingTarget())
            {
                var velocityX = GetComponent<IsolatedPhysics2D>().velocity.x;
                var velocityY = GetComponent<IsolatedPhysics2D>().velocity.y;
                //Debug.Log("Read Dynamic Movements " + velocityX + " " + velocityY);
                m_agent.SetDestination(target);
                m_agent.Move(m_info.move1.speed);

                m_animation.SetAnimation(0, m_info.move1.animation, true);
            }
            else
            {
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation && GetComponent<IsolatedPhysics2D>().velocity.y <= 0 && m_stateHandle.currentState != State.Phasing)
                {
                    m_turnState = State.Attacking;
                    m_stateHandle.OverrideState(State.Turning);
                }
            }
        }

        private IEnumerator HollowFormRoutine()
        {
            m_hitbox.gameObject.SetActive(false);
            m_animation.SetAnimation(0, m_info.vanishAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.vanishAnimation);
            m_animation.SetAnimation(0, m_info.appearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.appearAnimation);
            m_animation.SetAnimation(0, m_info.idle1Animation, true);
            m_hitbox.gameObject.SetActive(true);
            m_stateHandle.OverrideState(State.Chasing);
            yield return null;
        }
        #endregion

        private bool AllowAttack(Phase phase, State state)
        {
            if (m_phaseHandle.currentPhase >= phase)
            {
                return true;
            }
            else
            {
                DecidedOnAttack(false);
                m_stateHandle.OverrideState(state);
                return false;
            }
        }

        private void DecidedOnAttack(bool condition)
        {
            m_patternDecider.hasDecidedOnAttack = condition;
            m_attackDecider.hasDecidedOnAttack = condition;
        }

        private void UpdateAttackDeciderList()
        {
            m_patternDecider.SetList(new AttackInfo<Pattern>(Pattern.AttackPattern1, m_info.targetDistanceTolerance),
                                     new AttackInfo<Pattern>(Pattern.AttackPattern2, m_info.targetDistanceTolerance));
            DecidedOnAttack(false);
        }

        public override void ApplyData()
        {
            if (m_patternDecider != null)
            {
                UpdateAttackDeciderList();
            }
            base.ApplyData();
        }

        private void ChoosePattern()
        {
            if (!m_patternDecider.hasDecidedOnAttack)
            {
                IsAllAttackComplete();
                for (int i = 0; i < m_attackCache.Count; i++)
                {
                    m_patternDecider.DecideOnAttack();
                    if (m_attackCache[i] != m_currentPattern && !m_attackUsed[i])
                    {
                        m_attackUsed[i] = true;
                        m_currentPattern = m_attackCache[i];
                        return;
                    }
                }
            }
        }

        private void ExecuteAttack(Attack m_attack)
        {
            switch (m_attack)
            {
                case Attack.GhostOrb:
                    StartCoroutine(GhostOrbAttackRoutine());
                    break;
                case Attack.SkeletalArm:
                    StartCoroutine(SkeletalArmRoutine());
                    //StartCoroutine(Attack2Routine());
                    break;
                case Attack.SummonTotem:
                    StartCoroutine(SummonTotem());
                    //StartCoroutine(Attack3Routine());
                    break;
                case Attack.MapCurse:
                    StartCoroutine(MapCurse());
                    //StartCoroutine(Attack4Routine());
                    break;
            }
        }

        private void IsAllAttackComplete()
        {
            for (int i = 0; i < m_attackUsed.Length; ++i)
            {
                if (!m_attackUsed[i])
                {
                    return;
                }
            }
            for (int i = 0; i < m_attackUsed.Length; ++i)
            {
                m_attackUsed[i] = false;
            }
        }

        void AddToAttackCache(params Pattern[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                m_attackCache.Add(list[i]);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_projectileLauncher = new ProjectileLauncher(m_info.ghostOrbProjectile.projectileInfo, m_projectilePoint);
            m_patternDecider = new RandomAttackDecider<Pattern>();
            m_attackDecider = new RandomAttackDecider<Attack>();

            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            UpdateAttackDeciderList();

            m_attackCache = new List<Pattern>();
            AddToAttackCache(Pattern.AttackPattern1, Pattern.AttackPattern2/*, Pattern.AttackPattern3*/);
            m_attackUsed = new bool[m_attackCache.Count];
        }

        protected override void Start()
        {
            base.Start();
            m_spineListener.Subscribe(m_info.lichOrbStartFXEvent, m_ghostOrbStartFX.Play);
            m_spineListener.Subscribe(m_info.ghostOrbProjectile.launchOnEvent, LaunchOrb);
            m_animation.DisableRootMotion();

            m_phaseHandle = new PhaseHandle<Phase, PhaseInfo>();
            m_phaseHandle.Initialize(Phase.PhaseOne, m_info.phaseInfo, m_character, ChangeState, ApplyPhaseData);
            m_phaseHandle.ApplyChange();

            m_lichLordArmTF.SetParent(null);
            m_lichLordArmTF.GetComponentInChildren<SkeletonRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
        }

        private void Update()
        {
            if (!m_hasPhaseChanged)
            {
                m_phaseHandle.MonitorPhase();
            }
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idle1Animation, true);
                    break;
                case State.Intro:
                    StartCoroutine(IntroRoutine());
                    //if (IsFacingTarget())
                    //{
                    //    StartCoroutine(IntroRoutine());
                    //    //m_stateHandle.OverrideState(State.ReevaluateSituation);
                    //}
                    //else
                    //{
                    //    m_turnState = State.Intro;
                    //    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                    //        m_stateHandle.SetState(State.Turning);
                    //}
                    break;
                case State.Phasing:
                    Debug.Log("Phase Time");
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    StartCoroutine(ChangePhaseRoutine());
                    break;
                case State.Turning:
                    Debug.Log("Turning Steet");
                    m_stateHandle.Wait(m_turnState);
                    StopAllCoroutines();
                    m_agent.Stop();
                    m_turnHandle.Execute(m_info.turnAnimation, m_info.idle1Animation);
                    //m_animation.animationState.GetCurrent(0).MixDuration = 1;
                    //m_movement.Stop();
                    break;
                case State.Attacking:
                    //m_stateHandle.Wait(State.Attacking);
                    if (IsTargetInRange(m_info.targetDistanceTolerance))
                    {
                        m_stateHandle.Wait(State.ReevaluateSituation);
                        m_agent.Stop();
                        var randomFacing = UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1;
                        var target = new Vector2(m_targetInfo.position.x, GroundPosition().y /*m_startGroundPos*/);
                        switch (m_currentPattern)
                        {
                            case Pattern.AttackPattern1:
                                switch (m_phaseHandle.currentPhase)
                                {
                                    case Phase.PhaseOne:
                                        ExecuteAttack(Attack.GhostOrb);
                                        break;
                                    case Phase.PhaseTwo:
                                        ExecuteAttack(Attack.SummonTotem);
                                        break;
                                    case Phase.PhaseThree:
                                        ExecuteAttack(Attack.MapCurse);
                                        break;
                                }
                                ///////
                                //m_stateHandle.OverrideState(State.ReevaluateSituation);
                                break;
                            case Pattern.AttackPattern2:
                                switch (m_phaseHandle.currentPhase)
                                {
                                    case Phase.PhaseOne:
                                        ExecuteAttack(Attack.SkeletalArm);
                                        break;
                                    case Phase.PhaseTwo:
                                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                                        break;
                                    case Phase.PhaseThree:
                                        ExecuteAttack(Attack.SummonTotem);
                                        break;
                                }
                                ///////
                                //m_stateHandle.OverrideState(State.ReevaluateSituation);
                                break;
                        }
                    }
                    else
                    {
                        DynamicMovement(m_targetInfo.position);
                    }
                    break;

                case State.Chasing:
                    if (IsFacingTarget())
                    {
                        DecidedOnAttack(false);
                        ChoosePattern();
                        if (m_patternDecider.hasDecidedOnAttack )
                        {
                            m_stateHandle.SetState(State.Attacking);
                        }
                    }
                    else
                    {
                        m_turnState = State.Chasing;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation /*&& m_animation.GetCurrentAnimation(0).ToString() != m_info.attackDaggersIdle.animation*/)
                            m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.ReevaluateSituation:
                    m_stateHandle.SetState(State.Chasing);
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }
        }

        protected override void OnTargetDisappeared()
        {
            //m_stickToGround = false;
            //m_currentCD = 0;
        }
    }
}