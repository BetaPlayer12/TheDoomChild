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
using Spine.Unity.Modules;
using Spine.Unity.Examples;
using DChild.Gameplay.Pooling;
using UnityEngine.Playables;
using DChild.Temp;

namespace DChild.Gameplay.Characters.Enemies
{

    [AddComponentMenu("DChild/Gameplay/Enemies/Boss/TheOneSecondForm")]
    public class TheOneSecondFormAI : CombatAIBrain<TheOneSecondFormAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField]
            private PhaseInfo<Phase> m_phaseInfo;
            public PhaseInfo<Phase> phaseInfo => m_phaseInfo;

            [SerializeField]
            private int m_maxHitCount;
            public int maxhitcount => m_maxHitCount;
            [SerializeField]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;
            [SerializeField]
            private MovementInfo m_moveSlow = new MovementInfo();
            public MovementInfo moveSlow => m_moveSlow;
            [SerializeField]
            private MovementInfo m_runAway = new MovementInfo();
            public MovementInfo runAway => m_runAway;

            [SerializeField, MinValue(0), BoxGroup("ReevaluationInfo")]
            private float m_targetDistancetoleranceBehind = 1;
            public float targetDistancetoleranceBehind => m_targetDistancetoleranceBehind;
            [SerializeField, MinValue(0), BoxGroup("ReevaluationInfo")]
            private float m_targetDistancetolerance = 1;
            public float targetDistancetolerance => m_targetDistancetolerance;




            [SerializeField, BoxGroup("Tendril Climb")]
            private MovementInfo m_tendrilClimbUp = new MovementInfo();
            public MovementInfo tendrilClimbUp => m_tendrilClimbUp;
            [SerializeField, BoxGroup("Tendril Climb")]
            private MovementInfo m_tendrilClimbDown = new MovementInfo();
            public MovementInfo tendrilClimbDown => m_tendrilClimbDown;
            [SerializeField, BoxGroup("Cieling Climb")]
            private MovementInfo m_cielingClimbForward = new MovementInfo();
            public MovementInfo cielingClimbForward => m_cielingClimbForward;
            [SerializeField, BoxGroup("Cieling Climb")]
            private MovementInfo m_cielingClimbBackward = new MovementInfo();
            public MovementInfo cielingClimbBackward => m_cielingClimbBackward;

            [Title("Attack Behaviours")]
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;
            [SerializeField]
            private float m_jumpGravityScale;
            public float jumpGravityScale => m_jumpGravityScale;
            [SerializeField, BoxGroup("Tendril Whip")]
            private SimpleAttackInfo m_tendrilWhipAttack = new SimpleAttackInfo();
            public SimpleAttackInfo tendrilWhipAttack => m_tendrilWhipAttack;
            [SerializeField, BoxGroup("Tendril Whip")]
            private BasicAnimationInfo m_tendrilWhipAnticipationAnimation;
            public BasicAnimationInfo tendrilWhipAnticipationAnimation => m_tendrilWhipAnticipationAnimation;
            [SerializeField, BoxGroup("Tendril Whip")]
            private BasicAnimationInfo m_tendrilWhipBackAnimation;
            public BasicAnimationInfo tendrilWhipBackAnimation => m_tendrilWhipBackAnimation;
            [SerializeField, BoxGroup("Tendril Whip")]
            private BasicAnimationInfo m_tendrilWhipBackAnticipationAnimation;
            public BasicAnimationInfo tendrilWhipBackAnticipationAnimation => m_tendrilWhipBackAnticipationAnimation;
            [SerializeField, BoxGroup("Elemental Beam")]
            private float m_elementalBeamChargeDuration;
            public float elementalBeamChargeDuration => m_elementalBeamChargeDuration;
            [SerializeField, BoxGroup("Elemental Beam")]
            private SimpleAttackInfo m_elementalBeamAttack = new SimpleAttackInfo();
            public SimpleAttackInfo elementalBeamAttack => m_elementalBeamAttack;
            [SerializeField, BoxGroup("Elemental Beam")]
            private SimpleAttackInfo m_elementalBeamWallAttack = new SimpleAttackInfo();
            public SimpleAttackInfo elementalBeamWallAttack => m_elementalBeamWallAttack;
            [SerializeField, BoxGroup("Elemental Beam")]
            private SimpleAttackInfo m_elementalBeamCielingAttack = new SimpleAttackInfo();
            public SimpleAttackInfo elementalBeamCielingAttack => m_elementalBeamCielingAttack;
            [SerializeField, BoxGroup("Elemental Beam")]
            private BasicAnimationInfo m_elementalBeamChargeAnimation;
            public BasicAnimationInfo elementalBeamChargeAnimation => m_elementalBeamChargeAnimation;
            [SerializeField, BoxGroup("Elemental Beam")]
            private BasicAnimationInfo m_elementalBeamWallChargeAnimation;
            public BasicAnimationInfo elementalBeamWallChargeAnimation => m_elementalBeamWallChargeAnimation;
            [SerializeField, BoxGroup("Elemental Beam")]
            private BasicAnimationInfo m_elementalBeamCielingChargeAnimation;
            public BasicAnimationInfo elementalBeamCielingChargeAnimation => m_elementalBeamCielingChargeAnimation;
            [SerializeField, BoxGroup("Elemental Beam Flick")]
            private SimpleAttackInfo m_elementalBeamFlickAttack = new SimpleAttackInfo();
            public SimpleAttackInfo elementalBeamFlickAttack => m_elementalBeamFlickAttack;
            [SerializeField, BoxGroup("Elemental Beam Overload")]
            private SimpleAttackInfo m_elementalBeamOverloadAttack = new SimpleAttackInfo();
            public SimpleAttackInfo elementalBeamOverloadAttack => m_elementalBeamOverloadAttack;
            [SerializeField, BoxGroup("Jump Smash")]
            private SimpleAttackInfo m_jumpSmashAttack = new SimpleAttackInfo();
            public SimpleAttackInfo jumpSmashAttack => m_jumpSmashAttack;
            [SerializeField, BoxGroup("Jump Smash")]
            private BasicAnimationInfo m_jumpSmashStartAnimation;
            public BasicAnimationInfo jumpSmashStartAnimation => m_jumpSmashStartAnimation;
            [SerializeField, BoxGroup("Jump Smash")]
            private BasicAnimationInfo m_jumpSmashLandAnimation;
            public BasicAnimationInfo jumpSmashLandAnimation => m_jumpSmashLandAnimation;
            [SerializeField, BoxGroup("Jump Smash")]
            private BasicAnimationInfo m_jumpSmashWallStickAnimation;
            public BasicAnimationInfo jumpSmashWallStickAnimation => m_jumpSmashWallStickAnimation;
            [SerializeField, BoxGroup("Jump Smash")]
            private Vector2 m_posOffset;
            public Vector2 posOffset => m_posOffset;
            [SerializeField, BoxGroup("Jump Smash")]
            private Vector2 m_targetOffset;
            public Vector2 targetOffset => m_targetOffset;
            [SerializeField, BoxGroup("Tigrex Bite")]
            private SimpleAttackInfo m_tigrexBiteAttack = new SimpleAttackInfo();
            public SimpleAttackInfo tigrexBiteAttack => m_tigrexBiteAttack;
            [SerializeField, BoxGroup("Tigrex Bite")]
            private float m_tigrexBiteMoveSpeed;
            public float tigrexBiteMoveSpeed => m_tigrexBiteMoveSpeed;
            [SerializeField, BoxGroup("Tigrex Bite")]
            private float m_tigrexBiteDuration;
            public float tigrexBiteDuration => m_tigrexBiteDuration;
            [SerializeField, BoxGroup("Two Stomps")]
            private SimpleAttackInfo m_twoStompsAttack = new SimpleAttackInfo();
            public SimpleAttackInfo twoStompsAttack => m_twoStompsAttack;
            [SerializeField, BoxGroup("Two Stomps")]
            private float m_twoStompsDuration;
            public float twoStompsDuration => m_twoStompsDuration;

            [TitleGroup("Pattern Ranges")]
            [SerializeField]
            private float m_minRange;
            public float minRange => m_minRange;
            [SerializeField, BoxGroup("Phase 1")]
            private float m_phase1Pattern1Range;
            public float phase1Pattern1Range => m_phase1Pattern1Range;
            [SerializeField, BoxGroup("Phase 1")]
            private float m_phase1Pattern2Range;
            public float phase1Pattern2Range => m_phase1Pattern2Range;
            [SerializeField, BoxGroup("Phase 1")]
            private float m_phase1Pattern3Range;
            public float phase1Pattern3Range => m_phase1Pattern3Range;
            [SerializeField, BoxGroup("Phase 1")]
            private float m_phase1Pattern4Range;
            public float phase1Pattern4Range => m_phase1Pattern4Range;
            [SerializeField, BoxGroup("Phase 2")]
            private float m_phase2Pattern1Range;
            public float phase2Pattern1Range => m_phase2Pattern1Range;
            [SerializeField, BoxGroup("Phase 2")]
            private float m_phase2Pattern2Range;
            public float phase2Pattern2Range => m_phase2Pattern2Range;
            [SerializeField, BoxGroup("Phase 2")]
            private float m_phase2Pattern3Range;
            public float phase2Pattern3Range => m_phase2Pattern3Range;
            [SerializeField, BoxGroup("Phase 2")]
            private float m_phase2Pattern4Range;
            public float phase2Pattern4Range => m_phase2Pattern4Range;
            [SerializeField, BoxGroup("Phase 2")]
            private float m_phase2Pattern5Range;
            public float phase2Pattern5Range => m_phase2Pattern5Range;
            [SerializeField, BoxGroup("Phase 2")]
            private float m_phase2Pattern6Range;
            public float phase2Pattern6Range => m_phase2Pattern6Range;

            [TitleGroup("Attack Cooldown States")]
            [SerializeField, MinValue(0)]
            private List<float> m_phase1PatternCooldown;
            public List<float> phase1PatternCooldown => m_phase1PatternCooldown;
            [SerializeField, MinValue(0)]
            private List<float> m_phase2PatternCooldown;
            public List<float> phase2PatternCooldown => m_phase2PatternCooldown;
            [SerializeField, MinValue(0)]
            private int m_backAttackCooldown;
            public int backAttackCooldown => m_backAttackCooldown;

            [Title("Animations")]
            [SerializeField]
            private BasicAnimationInfo m_introAnimation;
            public BasicAnimationInfo introAnimation => m_introAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idle1Animation;
            public BasicAnimationInfo idle1Animation => m_idle1Animation;
            [SerializeField]
            private BasicAnimationInfo m_idle2Animation;
            public BasicAnimationInfo idle2Animation => m_idle2Animation;
            [SerializeField]
            private BasicAnimationInfo m_tendrilWallIdleAnimation;
            public BasicAnimationInfo tendrilWallIdleAnimation => m_tendrilWallIdleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation;
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_midAirFlinchAnimation;
            public BasicAnimationInfo midAirFlinchAnimation => m_midAirFlinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchToFallAnimation;
            public BasicAnimationInfo flinchToFallAnimation => m_flinchToFallAnimation;
            [SerializeField]
            private BasicAnimationInfo m_staggerAnimation;
            public BasicAnimationInfo staggerAnimation => m_staggerAnimation;
            [SerializeField]
            private BasicAnimationInfo m_staggerFallAnimation;
            public BasicAnimationInfo staggerFallAnimation => m_staggerFallAnimation;
            [SerializeField]
            private BasicAnimationInfo m_staggerLandAnimation;
            public BasicAnimationInfo staggerLandAnimation => m_staggerLandAnimation;
            [SerializeField]
            private BasicAnimationInfo m_standUpAnimation;
            public BasicAnimationInfo standUpAnimation => m_standUpAnimation;
            [SerializeField]
            private BasicAnimationInfo m_fallAnimation;
            public BasicAnimationInfo fallAnimation => m_fallAnimation;
            [SerializeField]
            private BasicAnimationInfo m_landAnimation;
            public BasicAnimationInfo landAnimation => m_landAnimation;
            [SerializeField]
            private BasicAnimationInfo m_dodgeAnimation;
            public BasicAnimationInfo dodgeAnimation => m_dodgeAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation;
            public BasicAnimationInfo turnAnimation => m_turnAnimation;
            [SerializeField]
            private BasicAnimationInfo m_phaseTransitionRoarAnimation;
            public BasicAnimationInfo phaseTransitionRoarAnimation => m_phaseTransitionRoarAnimation;

            [Title("Projectiles")]
            [SerializeField]
            private SimpleProjectileAttackInfo m_elementalOverloadProjectile;
            public SimpleProjectileAttackInfo elementalOverloadProjectile => m_elementalOverloadProjectile;
            [SerializeField]
            private float m_projectileGravityScale;
            public float projectileGravityScale => m_projectileGravityScale;

            //[Title("FX")]
            //[SerializeField]
            //private GameObject m_lightningBoltFX;
            //public GameObject lightningBoltFX => m_lightningBoltFX;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_beamOnEvent;
            public string beamOnEvent => m_beamOnEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_beamOffEvent;
            public string beamOffEvent => m_beamOffEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_mouthChargeEvent;
            public string mouthChargeEvent => m_mouthChargeEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_stomp1Event;
            public string stomp1Event => m_stomp1Event;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_stomp2Event;
            public string stomp2Event => m_stomp2Event;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_stoneGroundSpawnEvent;
            public string stoneGroundSpawnEvent => m_stoneGroundSpawnEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_stoneProjectileSpawnEvent;
            public string stoneProjectileSpawnEvent => m_stoneProjectileSpawnEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_stoneDustSpawnEvent;
            public string stoneDustSpawnEvent => m_stoneDustSpawnEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_taileKeyOnEvent;
            public string taileKeyOnEvent => m_taileKeyOnEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_taileKeyOffEvent;
            public string taileKeyOffEvent => m_taileKeyOffEvent;


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_move.SetData(m_skeletonDataAsset);
                m_moveSlow.SetData(m_skeletonDataAsset);
                m_runAway.SetData(m_skeletonDataAsset);
                m_tendrilClimbUp.SetData(m_skeletonDataAsset);
                m_tendrilClimbDown.SetData(m_skeletonDataAsset);
                m_cielingClimbForward.SetData(m_skeletonDataAsset);
                m_cielingClimbBackward.SetData(m_skeletonDataAsset);
                m_elementalOverloadProjectile.SetData(m_skeletonDataAsset);
                m_jumpSmashAttack.SetData(m_skeletonDataAsset);
                m_tendrilWhipAttack.SetData(m_skeletonDataAsset);
                m_elementalBeamAttack.SetData(m_skeletonDataAsset);
                m_elementalBeamWallAttack.SetData(m_skeletonDataAsset);
                m_elementalBeamCielingAttack.SetData(m_skeletonDataAsset);
                m_elementalBeamFlickAttack.SetData(m_skeletonDataAsset);
                m_elementalBeamOverloadAttack.SetData(m_skeletonDataAsset);
                m_tigrexBiteAttack.SetData(m_skeletonDataAsset);
                m_twoStompsAttack.SetData(m_skeletonDataAsset);

                m_tendrilWhipAnticipationAnimation.SetData(m_skeletonDataAsset);
                m_tendrilWhipBackAnimation.SetData(m_skeletonDataAsset);
                m_tendrilWhipBackAnticipationAnimation.SetData(m_skeletonDataAsset);
                m_elementalBeamChargeAnimation.SetData(m_skeletonDataAsset);
                m_elementalBeamWallChargeAnimation.SetData(m_skeletonDataAsset);
                m_elementalBeamCielingChargeAnimation.SetData(m_skeletonDataAsset);
                m_jumpSmashStartAnimation.SetData(m_skeletonDataAsset);
                m_jumpSmashLandAnimation.SetData(m_skeletonDataAsset);
                m_jumpSmashWallStickAnimation.SetData(m_skeletonDataAsset);
                m_introAnimation.SetData(m_skeletonDataAsset);
                m_idle1Animation.SetData(m_skeletonDataAsset);
                m_idle2Animation.SetData(m_skeletonDataAsset);
                m_tendrilWallIdleAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_midAirFlinchAnimation.SetData(m_skeletonDataAsset);
                m_flinchToFallAnimation.SetData(m_skeletonDataAsset);
                m_staggerAnimation.SetData(m_skeletonDataAsset);
                m_staggerFallAnimation.SetData(m_skeletonDataAsset);
                m_staggerLandAnimation.SetData(m_skeletonDataAsset);
                m_standUpAnimation.SetData(m_skeletonDataAsset);
                m_fallAnimation.SetData(m_skeletonDataAsset);
                m_landAnimation.SetData(m_skeletonDataAsset);
                m_dodgeAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_phaseTransitionRoarAnimation.SetData(m_skeletonDataAsset);

#endif
            }
        }

        [System.Serializable]
        public class PhaseInfo : IPhaseInfo
        {
            [SerializeField]
            private List<float> m_fullCooldown;
            public List<float> fullCooldown => m_fullCooldown;
            [SerializeField]
            private int m_hitCount;
            public int hitCount => m_hitCount;
            [SerializeField]
            private int m_attackCount;
            public int attackCount => m_attackCount;
        }


        private enum State
        {
            Phasing,
            Idle,
            Turning,
            Attacking,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            Phase1Pattern1,
            Phase1Pattern2,
            Phase1Pattern3,
            Phase1Pattern4,
            Phase2Pattern1,
            Phase2Pattern2,
            Phase2Pattern3,
            Phase2Pattern4,
            Phase2Pattern5,
            Phase2Pattern6,
            BackwardTendrilWhip,
            TwoStomp,
            TendrilWhip,
            TendrilClimbJumpSmash,
            ElementalBeamFlick,
            TendrilClimbElementalBeam,
            TigrexBiting,
            ElementalOverload,
            WaitAttackEnd,
        }
        private enum WallAttack
        {
            JumpAttack,
            ElementalBeam,
            WaitAttackEnd,
        }

        public enum Phase
        {
            PhaseOne,
            PhaseTwo,
            Wait,
        }

        /*        private bool[] m_attackUsed;
                private List<Attack> m_attackCache;
                private List<float> m_attackRangeCache;*/
        private Vector2 m_lastTargetPos;
        private Vector2 m_lazerTargetPos;
        private float m_currentCooldown;
        private float m_pickedCooldown;
        private float m_backCooldown;
        private List<float> m_currentFullCooldown;
        private int[] m_patternAttackCount;
        private List<float> m_patternCooldown;
        private int m_currentHitCount = 0;
        private int m_maxAttackCount;
        private int m_currentAttackCount;
        private float m_defaultGravityScale;
        #region Jump Settings
        //private Vector2 m_posOffset = new Vector2(1f, 0.4f);
        //private Vector2 m_targetOffset = new Vector2(1.6f, 1);
        #endregion

        [SerializeField, TabGroup("Reference")]
        private Boss m_boss;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;

        [SerializeField]
        private TheOneSecondFormElementalBeamAttack m_elementalBeamAttackHandle;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_backSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_wallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_cielingSensor;

        [SerializeField, TabGroup("Lazer")]
        private ParticleSystem m_muzzleLoopFX;
        [SerializeField, TabGroup("Lazer")]
        private ParticleSystem m_elementalBeamIndicatorFX;

        [SerializeField]
        private SpineEventListener m_spineListener;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        State m_turnState;
        [ShowInInspector]
        private PhaseHandle<Phase, PhaseInfo> m_phaseHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_BasicAttackDecider;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_ComplexeattackDecider;

        private RandomAttackDecider<Attack> m_currentAttackDecider;

        private Attack m_currentAttack;
        private float m_currentAttackRange;
        private BallisticProjectileLauncher m_projectileLauncher;

        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_projectilePoint;

        private bool canUseBackTendril => m_backCooldown <= 0;

        /*        private Coroutine m_roarCoroutine;
                private Coroutine m_currentAttackCoroutine;
                private Coroutine m_counterAttackCoroutine;
                private Coroutine m_backAttackCoroutine;
                private Coroutine m_backAttackCooldownCoroutine;
                #region Lazer Coroutine
                private Coroutine m_lazerBeamCoroutine;
                private Coroutine m_lazerLookCoroutine;
                private Coroutine m_aimAtPlayerCoroutine;
                #endregion*/
        private PhaseInfo m_currentPhaseInfo;
        #region Animations
        private string m_currentIdleAnimation;
        #endregion


        private void ApplyPhaseData(PhaseInfo obj)
        {
            /*            m_attackCache.Clear();
                        m_attackRangeCache.Clear();*/
            //if (m_patternCooldown.Count != 0)
            //    m_patternCooldown.Clear();
            //m_maxHitCount = obj.hitCount;
            //m_maxAttackCount = obj.attackCount;
            /*switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseOne:
                    //m_idleAnimation = m_info.idleCombatAnimation;
                    AddToAttackCache(Attack.Phase1Pattern1, Attack.Phase1Pattern2, Attack.Phase1Pattern3, Attack.Phase1Pattern4);
                    AddToRangeCache(m_info.phase1Pattern1Range, m_info.phase1Pattern2Range, m_info.phase1Pattern3Range, m_info.phase1Pattern4Range);
                    for (int i = 0; i < m_info.phase1PatternCooldown.Count; i++)
                        m_patternCooldown.Add(m_info.phase1PatternCooldown[i]);
                    break;
                case Phase.PhaseTwo:
                    //m_idleAnimation = m_info.idleCombatAnimation;
                    AddToAttackCache(Attack.Phase2Pattern1, Attack.Phase2Pattern2, Attack.Phase2Pattern3, Attack.Phase2Pattern4, Attack.Phase2Pattern5, Attack.Phase2Pattern6);
                    AddToRangeCache(m_info.phase2Pattern1Range, m_info.phase2Pattern2Range, m_info.phase2Pattern3Range, m_info.phase2Pattern4Range, m_info.phase2Pattern5Range, m_info.phase2Pattern6Range);
                    for (int i = 0; i < m_info.phase2PatternCooldown.Count; i++)
                        m_patternCooldown.Add(m_info.phase2PatternCooldown[i]);
                    break;
            }*/
            //m_attackUsed = new bool[m_attackCache.Count];
            //if (m_currentFullCooldown.Count != 0)
            //{
            //    m_currentFullCooldown.Clear();
            //}
            //for (int i = 0; i < obj.fullCooldown.Count; i++)
            //{
            //    m_currentFullCooldown.Add(obj.fullCooldown[i]);
            //}
            m_currentPhaseInfo = obj;
            UpdateAttackDeciderList();
        }

        private void ChangeState()
        {
            //StopCurrentAttackRoutine();
            //SetAIToPhasing();
            StartCoroutine(SmartChangePhaseRoutine());
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs)
        {
            if (m_stateHandle.currentState != State.Phasing /*&& !m_hasPhaseChanged*/)
            {
                m_stateHandle.OverrideState(State.Turning);
            }
        }

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null && m_stateHandle.currentState == State.Idle)
            {
                base.SetTarget(damageable, m_target);
                m_stateHandle.OverrideState(State.ReevaluateSituation);
            }
        }

        private void OnDamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
        {
            //if (m_groundSensor.isDetecting && m_roarCoroutine == null)
            //{
            //    switch (m_phaseHandle.currentPhase)
            //    {
            //        case Phase.PhaseOne:
            //            if (m_currentHitCount < m_maxHitCount)
            //                m_currentHitCount++;
            //            else
            //            {
            //                StopRoutines();
            //                m_stateHandle.Wait(State.ReevaluateSituation);

            //                m_counterAttackCoroutine = StartCoroutine(DodgeRoutine());
            //                m_currentHitCount = 0;
            //            }
            //            break;
            //        case Phase.PhaseTwo:
            //            if (m_currentHitCount < m_maxHitCount)
            //                m_currentHitCount++;
            //            else
            //            {
            //                StopRoutines();
            //                m_stateHandle.Wait(State.ReevaluateSituation);

            //                m_counterAttackCoroutine = StartCoroutine(DodgeRoutine());
            //                m_currentHitCount = 0;
            //            }
            //            break;
            //    }
            //}
            if (m_groundSensor.isDetecting)
            {

                if (m_currentHitCount < m_info.maxhitcount)
                {
                    m_currentHitCount++;
                    Debug.Log("current hit count" + m_currentHitCount);
                }
                else if (m_currentHitCount >= m_info.maxhitcount)
                {
                 
                   // StartCoroutine(DodgeRoutine());
                    m_currentHitCount = 0;
                    Debug.Log("current hit count" + m_currentHitCount);
                }
            }

        }
        private IEnumerator DodgeMovement()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            if (!IsFacingTarget())
                CustomTurn();

            m_muzzleLoopFX.Stop();
            //  ResetLaser();
            // m_aimOn = false;
            // m_beamOn = false;
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.dodgeAnimation, false);
            //m_animation.AddAnimation(0, m_info.idle1Animation, true, 0);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.dodgeAnimation);
           
        }
        private IEnumerator DodgeRoutine()
        {
           // StopAllCoroutines();
            yield return DodgeMovement();
            DoRandomIdleAnimation();
            m_stateHandle.ApplyQueuedState();

        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            if (m_stateHandle.currentState != State.Phasing /*&& !m_hasPhaseChanged*/)
            {
                m_animation.animationState.TimeScale = 1f;
                m_stateHandle.ApplyQueuedState();
            }
            m_phaseHandle.allowPhaseChange = true;
        }

        /*private IEnumerator IntroRoutine()
        {
            //gameObject.SetActive(true);
            //m_animation.EnableRootMotion(true, true);
            m_stateHandle.Wait(State.Chasing);
            m_movement.Stop();
            m_hitbox.Disable();
            m_animation.animationState.TimeScale = 1;
            yield return new WaitForSeconds(1.3f);
            m_animation.SetAnimation(0, RandomIdleAnimation(), true);
            m_hitbox.Enable();
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }*/

        private IEnumerator SmartChangePhaseRoutine()
        {
            yield return new WaitWhile(() => !m_phaseHandle.allowPhaseChange);
            //StopRoutines();
            SetAIToPhasing();
            m_muzzleLoopFX.Stop();
            // ResetLaser();
            yield return null;
        }

        private void SetAIToPhasing()
        {
            //m_hasPhaseChanged = true;
            m_stateHandle.OverrideState(State.Phasing);
            m_phaseHandle.ApplyChange();
            m_animation.DisableRootMotion();
            m_animation.SetEmptyAnimation(0, 0);
            // m_aimOn = false;
            //m_beamOn = false;
        }

        /* private void StopRoutines()
         {
             *//*if (m_currentAttackCoroutine != null)
             {
                 StopCoroutine(m_currentAttackCoroutine);
                 m_currentAttackCoroutine = null;
             }
             StopCoroutine(m_aimRoutine);
             if (m_aimAtPlayerCoroutine != null)
             {
                 StopCoroutine(m_aimAtPlayerCoroutine);
                 m_aimAtPlayerCoroutine = null;
             }
             if (m_lazerLookCoroutine != null)
             {
                 StopCoroutine(m_lazerLookCoroutine);
                 m_lazerLookCoroutine = null;
             }
             if (m_lazerBeamCoroutine != null)
             {
                 StopCoroutine(m_lazerBeamCoroutine);
                 m_lazerBeamCoroutine = null;
             }
             if (m_backAttackCoroutine != null)
             {
                 StopCoroutine(m_backAttackCoroutine);
                 m_backAttackCoroutine = null;
             }
             if (m_backAttackCooldownCoroutine != null)
             {
                 StopCoroutine(m_backAttackCooldownCoroutine);
                 m_backAttackCooldownCoroutine = null;
             }
             if (m_roarCoroutine != null)
             {
                 StopCoroutine(m_roarCoroutine);
                 m_roarCoroutine = null;
             }*//*
         }*/

        private void HandleCooldowns()
        {
            m_backCooldown -= GameplaySystem.time.deltaTime;

        }

        private IEnumerator ChangePhaseRoutine()
        {

            m_stateHandle.Wait(State.ReevaluateSituation);
            m_hitbox.Disable();
            //m_hasPhaseChanged = false;
            m_animation.SetAnimation(0, m_info.phaseTransitionRoarAnimation, false).MixDuration = 0;
            //yield return new WaitForSeconds(3.9f);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.phaseTransitionRoarAnimation);
            m_animation.SetAnimation(0, RandomIdleAnimation(), true);
            m_hitbox.Enable();
            m_stateHandle.ApplyQueuedState();
            yield return null;

        }

        #region Modules

        private void DoRandomIdleAnimation()
        {
            m_animation.SetAnimation(0, RandomIdleAnimation(), true);
        }

        private IEnumerator RoarBehaviour()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            yield return RoarRoutine();
            DoRandomIdleAnimation();
            m_hitbox.Enable();
            m_stateHandle.ApplyQueuedState();
        }
        private IEnumerator RoarRoutine()
        {
            m_currentAttackCount = 0;
            m_hitbox.Disable();
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.phaseTransitionRoarAnimation, false).MixDuration = 0;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.phaseTransitionRoarAnimation);

        }
        private IEnumerator TendrilWhipBackwardRoutine()
        {
            m_animation.SetAnimation(0, m_info.tendrilWhipBackAnticipationAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.tendrilWhipBackAnticipationAnimation);
            m_animation.SetAnimation(0, m_info.tendrilWhipBackAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.tendrilWhipBackAnimation);
            m_animation.SetAnimation(0, m_info.turnAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.turnAnimation);

        }
        private IEnumerator TwoStompsRoutine()
        {
            m_animation.SetAnimation(0, m_info.twoStompsAttack.animation, true);
            var timer = 0f;
            while (timer < m_info.twoStompsDuration)
            {
                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.move.speed);
                timer += Time.deltaTime;
                yield return null;
            }
        }
        private IEnumerator TendrilWhipRoutine()
        {
            m_animation.SetAnimation(0, m_info.tendrilWhipAnticipationAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.tendrilWhipAnticipationAnimation);
            m_animation.SetAnimation(0, m_info.tendrilWhipAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.tendrilWhipAttack.animation);
            
        }
        private IEnumerator TigrexBiteRoutine()
        {
            m_animation.SetAnimation(0, m_info.tigrexBiteAttack.animation, true);
            var timer = 0f;
            while (timer < m_info.tigrexBiteDuration)
            {
                if (m_wallSensor.isDetecting || (Vector2.Distance(transform.position, m_targetInfo.position) >= m_info.targetDistanceTolerance && !IsFacingTarget()))
                    CustomTurn();

                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.tigrexBiteMoveSpeed);
                timer += Time.deltaTime;
                yield return null;
            }
        }
        private IEnumerator ElementalBeamFlickRoutine()
        {
            m_animation.SetAnimation(0, m_info.elementalBeamChargeAnimation, true);
            m_muzzleLoopFX.Play();
            yield return new WaitForSeconds(m_info.elementalBeamChargeDuration);
            m_animation.SetAnimation(0, m_info.elementalBeamFlickAttack.animation, true);
            yield return ElementalBeamLazerRoutine();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.elementalBeamFlickAttack.animation);
            m_muzzleLoopFX.Stop();
        }
        private IEnumerator ElementalBeamRoutine()
        {
            m_animation.SetAnimation(0, m_info.elementalBeamWallChargeAnimation, true);
            m_muzzleLoopFX.Play();
            m_elementalBeamAttackHandle.EnableAimAtTarget(m_targetInfo.transform);
            m_elementalBeamIndicatorFX.Play();
            m_elementalBeamAttackHandle.ShowIndicator(m_info.elementalBeamChargeDuration);
            yield return new WaitForSeconds(m_info.elementalBeamChargeDuration);
            m_elementalBeamAttackHandle.HoldAim();
            m_animation.SetAnimation(0, m_info.elementalBeamWallAttack.animation, false);
            yield return ElementalBeamLazerRoutine();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.elementalBeamWallAttack.animation);
            m_muzzleLoopFX.Stop();
            m_elementalBeamAttackHandle.DisableAimAtTarget();

        }
        private IEnumerator TendrilClimbRoutine()
        {
            m_animation.EnableRootMotion(false, false);
            m_animation.SetAnimation(0, m_info.phaseTransitionRoarAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.phaseTransitionRoarAnimation);
            m_animation.SetAnimation(0, m_info.turnAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.turnAnimation);
            CustomTurn();
            m_animation.SetAnimation(0, m_info.move.animation, true);
            while (!m_wallSensor.isDetecting)
            {
                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.move.speed);
                yield return null;
            }
            CustomTurn();
            m_movement.Stop();
            m_character.physics.simulateGravity = false;
            m_animation.SetAnimation(0, m_info.tendrilClimbUp.animation, true);
            while (!m_cielingSensor.isDetecting)
            {
                m_character.physics.SetVelocity(0, m_info.tendrilClimbUp.speed);
                yield return null;
            }
            m_character.physics.SetVelocity(Vector2.zero);
        }

        /// <summary>
        /// Make Sure Tendril Climb Routine is use right before this
        /// </summary>
        /// <returns></returns>
        private IEnumerator JumpSmashRoutine()
        {
            m_character.physics.SetVelocity(BallisticVelocity(new Vector2(m_targetInfo.position.x, /*(transform.position.y - m_targetInfo.position.y)*/GroundPosition(m_targetInfo.position).y)));
            m_animation.SetAnimation(0, m_info.jumpSmashStartAnimation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.jumpSmashStartAnimation);
            yield return new WaitUntil(() => m_groundSensor.isDetecting);
            m_animation.SetAnimation(0, m_info.jumpSmashAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.jumpSmashAttack.animation);
            m_animation.SetAnimation(0, m_info.jumpSmashLandAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.jumpSmashLandAnimation);
        }

        private IEnumerator ElementalOverloadRoutine()
        {
            //m_lazerLookCoroutine = StartCoroutine(LazerLookRoutine());
            m_animation.SetAnimation(0, m_info.elementalBeamOverloadAttack.animation, false);
            //m_lazerBeamCoroutine = StartCoroutine(LazerBeamRoutine());
            //  yield return new WaitUntil(() => m_beamOn);
            // m_aimAtPlayerCoroutine = StartCoroutine(AimAtTargtRoutine());
            m_muzzleLoopFX.Play();
            yield return ElementalBeamLazerRoutine();
            m_muzzleLoopFX.Stop();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.elementalBeamOverloadAttack.animation);
            
            // StopCoroutine(m_lazerLookCoroutine);
            // m_lazerLookCoroutine = null;
            // StopCoroutine(m_aimAtPlayerCoroutine);
            // m_aimAtPlayerCoroutine = null;

        }

        #endregion

        #region Attack Patterns
        private IEnumerator TestRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            yield return new WaitForSeconds(1);
            m_currentAttackCount++;
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator ElementalOverloadAttack()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            yield return ElementalOverloadRoutine();
            m_currentAttackCount++;
            m_movement.Stop();
            DoRandomIdleAnimation();
            m_currentAttackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();

        }

        private IEnumerator TendrilClimbJumpSmashAttack()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            yield return TendrilClimbRoutine();
            yield return JumpSmashRoutine();
            m_currentAttackCount++;
            m_movement.Stop();
            m_currentAttackDecider.hasDecidedOnAttack = false;
            DoRandomIdleAnimation();
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator TendrilClimbAttack()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            yield return TendrilClimbRoutine();
            var Random = UnityEngine.Random.Range(0, 2);

            yield return ElementalBeamRoutine();


            yield return JumpSmashRoutine();
            m_currentAttackCount++;
            m_movement.Stop();
            m_currentAttackDecider.hasDecidedOnAttack = false;
            DoRandomIdleAnimation();
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator ElementalBeamFlickAttack()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            if (!IsFacingTarget())
                CustomTurn();
            yield return ElementalBeamFlickRoutine();
            m_currentAttackCount++;
            m_movement.Stop();
            m_animation.SetAnimation(0, RandomIdleAnimation(), true);
            m_currentAttackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            
        }

        private IEnumerator TigrexBiteAttack()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            yield return TigrexBiteRoutine();
            m_currentAttackCount++;
            m_movement.Stop();
            DoRandomIdleAnimation();
            m_currentAttackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();

        }

        private IEnumerator TendrilWhipBackwardAttack()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            yield return TendrilWhipBackwardRoutine();
            if (!IsFacingTarget())
                CustomTurn();
            DoRandomIdleAnimation();
            m_currentAttackCount++;
            m_backCooldown = m_info.backAttackCooldown;
            m_currentAttackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator TwoStompAttack()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            if (!IsFacingTarget())
                CustomTurn();
            
            yield return TwoStompsRoutine();

            m_movement.Stop();
            DoRandomIdleAnimation();
            m_currentAttackCount++;
            m_currentAttackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator TendrilWhipAttack()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            if (!IsFacingTarget())
                CustomTurn();
            yield return TendrilWhipRoutine();
            DoRandomIdleAnimation();
            m_currentAttackCount++;
            m_currentAttackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
        }

        #endregion



        #region Attacks
        private void LaunchProjectile()
        {
            //var target = new Vector2(m_projectilePoint.position.x + (5 * transform.localScale.x), m_projectilePoint.position.y);
            //m_projectileLauncher.AimAt(target);
            //m_projectileLauncher.LaunchProjectile();

            m_projectileLauncher.AimAt(m_targetInfo.position);
            m_projectileLauncher.LaunchBallisticProjectile(m_targetInfo.position);
        }

        private IEnumerator ElementalBeamLazerRoutine()
        {
            yield return new WaitForSpineEvent(m_animation.skeletonAnimation, m_info.beamOnEvent);

            m_muzzleLoopFX.Play();
            m_elementalBeamAttackHandle.ExecuteBeamAttack();

            yield return new WaitForSpineEvent(m_animation.skeletonAnimation, m_info.beamOffEvent);

            m_elementalBeamAttackHandle.StopBeam();

            //if (!m_aimOn)
            //{
            //    StartCoroutine(TelegraphLineRoutine());
            //    StartCoroutine(m_aimRoutine);
            //}

            //yield return new WaitUntil(() => m_beamOn);
            //StopCoroutine(m_aimRoutine);

            //m_lineRenderer.useWorldSpace = true;
            //while (m_beamOn)
            //{
            //    m_muzzleLoopFX.transform.position = ShotPosition();

            //    m_lineRenderer.SetPosition(0, m_beamFrontPoint.position);
            //    m_lineRenderer.SetPosition(1, ShotPosition());
            //    for (int i = 0; i < m_lineRenderer.positionCount; i++)
            //    {
            //        var pos = m_lineRenderer.GetPosition(i) - m_edgeCollider.transform.position;
            //        pos = new Vector2(Mathf.Abs(pos.x), pos.y);
            //        //if (i > 0)
            //        //{
            //        //    pos = pos * 0.7f;
            //        //}
            //        m_Points.Add(pos);
            //    }
            //    m_edgeCollider.points = m_Points.ToArray();
            //    m_Points.Clear();
            //    yield return null;
            //}
            //m_muzzleLoopFX.Stop();
            ////yield return new WaitUntil(() => !m_beamOn);
            //ResetLaser();
            ////m_lazerBeamCoroutine = null;
            //yield return null;
        }


        #endregion

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            StopAllCoroutines();
            m_movement.Stop();
        }

        #region Movement
        private IEnumerator TendrilClimbRoutine(WallAttack attack)
        {
            m_animation.EnableRootMotion(false, false);
            //m_stateHandle.Wait(State.Chasing);
            m_animation.SetAnimation(0, m_info.phaseTransitionRoarAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.phaseTransitionRoarAnimation);
            m_animation.SetAnimation(0, m_info.turnAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.turnAnimation);
            CustomTurn();
            m_animation.SetAnimation(0, m_info.move.animation, true);
            while (!m_wallSensor.isDetecting)
            {
                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.move.speed);
                yield return null;
            }
            CustomTurn();
            m_movement.Stop();
            m_character.physics.simulateGravity = false;
            m_animation.SetAnimation(0, m_info.tendrilClimbUp.animation, true);
            while (!m_cielingSensor.isDetecting)
            {
                m_character.physics.SetVelocity(0, m_info.tendrilClimbUp.speed);
                yield return null;
            }
            m_character.physics.SetVelocity(Vector2.zero);

            //switch (attack)
            //{
            //    case WallAttack.JumpAttack:
            //        m_animation.SetAnimation(0, m_info.jumpSmashWallStickAnimation, false);
            //        yield return new WaitForAnimationComplete(m_animation.animationState, m_info.jumpSmashWallStickAnimation);
            //        m_character.physics.SetVelocity(BallisticVelocity(new Vector2(m_targetInfo.position.x, (transform.position.y - m_targetInfo.position.y))));
            //        m_animation.SetAnimation(0, m_info.jumpSmashStartAnimation, false);
            //        //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.jumpSmashStartAnimation);
            //        yield return new WaitUntil(() => m_groundSensor.isDetecting);
            //        m_animation.SetAnimation(0, m_info.jumpSmashAttack.animation, false);
            //        yield return new WaitForAnimationComplete(m_animation.animationState, m_info.jumpSmashAttack.animation);
            //        m_animation.SetAnimation(0, m_info.jumpSmashLandAnimation, false);
            //        yield return new WaitForAnimationComplete(m_animation.animationState, m_info.jumpSmashLandAnimation);
            //        //m_stateHandle.ApplyQueuedState();
            //        break;
            //    case WallAttack.ElementalBeam:
            //        m_lazerLookCoroutine = StartCoroutine(LazerLookRoutine());
            //        m_aimOn = true;
            //        m_aimAtPlayerCoroutine = StartCoroutine(AimAtTargtRoutine());
            //        m_animation.SetAnimation(0, m_info.elementalBeamWallChargeAnimation, true);
            //        m_lazerBeamCoroutine = StartCoroutine(LazerBeamRoutine());
            //        yield return new WaitForSeconds(m_info.elementalBeamChargeDuration);
            //        m_animation.SetAnimation(0, m_info.elementalBeamWallAttack.animation, false);
            //        yield return new WaitForAnimationComplete(m_animation.animationState, m_info.elementalBeamWallAttack.animation);
            //        StopCoroutine(m_lazerLookCoroutine);
            //        m_lazerLookCoroutine = null;
            //        m_aimOn = false;
            //        m_character.physics.simulateGravity = true;
            //        m_animation.SetAnimation(0, m_info.fallAnimation, true);
            //        yield return new WaitUntil(() => m_groundSensor.isDetecting);
            //        m_animation.SetAnimation(0, m_info.landAnimation, false);
            //        yield return new WaitForAnimationComplete(m_animation.animationState, m_info.landAnimation);
            //        m_currentAttackCount++;
            //        m_movement.Stop();
            //        m_animation.SetAnimation(0, RandomIdleAnimation(), true);
            //        break;
            //}
            #region Experiment
            //m_lazerLookCoroutine = StartCoroutine(LazerLookRoutine());
            //m_aimOn = true;
            //m_aimAtPlayerCoroutine = StartCoroutine(AimAtTargtRoutine());
            m_animation.SetAnimation(0, m_info.jumpSmashWallStickAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.jumpSmashWallStickAnimation);
            m_animation.SetAnimation(0, m_info.elementalBeamWallChargeAnimation, true);
            // m_lazerBeamCoroutine = StartCoroutine(LazerBeamRoutine());
            yield return new WaitForSeconds(m_info.elementalBeamChargeDuration);
            m_animation.SetAnimation(0, m_info.elementalBeamWallAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.elementalBeamWallAttack.animation);
            // StopCoroutine(m_lazerLookCoroutine);
            //m_lazerLookCoroutine = null;
            // m_aimOn = false;
            m_character.physics.SetVelocity(BallisticVelocity(new Vector2(m_targetInfo.position.x, /*(transform.position.y - m_targetInfo.position.y)*/GroundPosition(m_targetInfo.position).y)));
            m_animation.SetAnimation(0, m_info.jumpSmashStartAnimation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.jumpSmashStartAnimation);
            yield return new WaitUntil(() => m_groundSensor.isDetecting);
            m_animation.SetAnimation(0, m_info.jumpSmashAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.jumpSmashAttack.animation);
            m_animation.SetAnimation(0, m_info.jumpSmashLandAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.jumpSmashLandAnimation);
            #endregion

            m_currentAttackCount++;
            m_currentAttackDecider.hasDecidedOnAttack = false;
            // m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private string RandomIdleAnimation()
        {
            m_currentIdleAnimation = UnityEngine.Random.Range(0, 2) == 1 ? m_info.idle1Animation.animation : m_info.idle2Animation.animation;
            return m_currentIdleAnimation;
        }

        public Vector2 BallisticVelocity(Vector2 target)
        {
            GetComponent<IsolatedCharacterPhysics2D>().simulateGravity = true;
            GetComponent<IsolatedCharacterPhysics2D>().gravity.gravityScale = m_info.jumpGravityScale;

            //var yOffset = m_character.centerMass.position.y + Mathf.Abs(m_character.centerMass.position.y - m_character.centerMass.position.y);
            var yOffset = target.y + Mathf.Abs(GroundPosition(target).y - m_character.centerMass.position.y);
            target = new Vector2(target.x, Mathf.Abs(target.x - m_character.centerMass.position.x) < 10 ? yOffset : target.y);
            var dir = (target - (Vector2)m_character.centerMass.position);
            //dir = new Vector2(dir.x, dir.y + (Mathf.Abs(target.y - m_spawnPoint.position.y) * 0.5f));
            Debug.Log("Ballistic Direction " + dir);
            var h = dir.y;
            dir.y = 0;
            var dist = dir.magnitude;
            dir.y = dist;
            dist += h;


            //var currentSpeed = Vector2.Distance(target, m_spawnPoint.position) < 10 ? m_speed : 1;

            var vel = (Mathf.Sqrt(dist * GetComponent<IsolatedCharacterPhysics2D>().gravity.gravityScale)) /** currentSpeed*/;
            Debug.Log("Velocity " + vel);
            var ballisticVel = (vel * new Vector2(dir.x * m_info.posOffset.x, dir.y * m_info.posOffset.y).normalized) * m_info.targetOffset.sqrMagnitude; //closest to accurate
            Debug.Log("Ballistic Velocity " + ballisticVel);
            return ballisticVel;
        }
        #endregion

        private void UpdateAttackDeciderList()
        {

            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseOne:
                    m_BasicAttackDecider.SetList(new AttackInfo<Attack>(Attack.TwoStomp, 0),
                                            new AttackInfo<Attack>(Attack.TendrilWhip, 0),
                                            new AttackInfo<Attack>(Attack.TendrilClimbJumpSmash, 0),
                                            new AttackInfo<Attack>(Attack.ElementalBeamFlick, 0),
                                            new AttackInfo<Attack>(Attack.TendrilClimbElementalBeam, 0));

                    m_ComplexeattackDecider.SetList(new AttackInfo<Attack>(Attack.TendrilClimbJumpSmash, 0),
                                           new AttackInfo<Attack>(Attack.ElementalBeamFlick, 0),
                                           new AttackInfo<Attack>(Attack.TendrilClimbElementalBeam, 0));
                    break;
                case Phase.PhaseTwo:
                    m_BasicAttackDecider.SetList(new AttackInfo<Attack>(Attack.TwoStomp, 0),
                                             new AttackInfo<Attack>(Attack.TendrilWhip, 0),
                                             new AttackInfo<Attack>(Attack.TendrilClimbJumpSmash, 0),
                                             new AttackInfo<Attack>(Attack.ElementalBeamFlick, 0),
                                             new AttackInfo<Attack>(Attack.TendrilClimbElementalBeam, 0));

                   m_ComplexeattackDecider.SetList(new AttackInfo<Attack>(Attack.TendrilClimbJumpSmash, 0),
                                         new AttackInfo<Attack>(Attack.ElementalBeamFlick, 0),
                                         new AttackInfo<Attack>(Attack.TendrilClimbElementalBeam, 0), 
                                         new AttackInfo<Attack>(Attack.TigrexBiting, 0),
                                         new AttackInfo<Attack>(Attack.ElementalOverload, 0));
                    break;
                case Phase.Wait:
                    break;
            }

            //m_currentAttackDecider.hasDecidedOnAttack = false;
        }

        public override void ApplyData()
        {
            if (m_BasicAttackDecider != null && m_ComplexeattackDecider != null)
            {
                UpdateAttackDeciderList();
            }
            base.ApplyData();
        }

        //private Vector2 WallPosition()
        //{
        //    var wristPoint = new Vector2(m_wristPoint.position.x, m_wristPoint.position.y + 2f);
        //    RaycastHit2D hit = Physics2D.Raycast(wristPoint, Vector2.right * transform.localScale.x, 1000, DChildUtility.GetEnvironmentMask());
        //    return hit.point;
        //}

        //private Vector2 GroundPosition()
        //{
        //    RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1000, DChildUtility.GetEnvironmentMask());
        //    return hit.point;
        //}

        /*private void ChooseAttack()
        {
            if (!m_attackDecider.hasDecidedOnAttack)
            {
                IsAllAttackComplete();
                for (int i = 0; i < m_attackCache.Count; i++)
                {
                    m_attackDecider.DecideOnAttack();
                    if (m_attackCache[i] != m_currentAttack && !m_attackUsed[i])
                    {
                        m_attackUsed[i] = true;
                        m_currentAttack = m_attackCache[i];
                        m_currentAttackRange = m_attackRangeCache[i];
                        return;
                    }
                }
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

        void AddToAttackCache(params Attack[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                m_attackCache.Add(list[i]);
            }
        }

        void AddToRangeCache(params float[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                m_attackRangeCache.Add(list[i]);
            }
        }

        void UpdateRangeCache(params float[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                m_attackRangeCache[i] = list[i];
            }
        }*/

        private bool IsTargetBehind(float distanceTolerance)
        {
            return !IsFacingTarget() && IsTargetWithin(distanceTolerance);
        }

        private bool IsTargetWithin(float distanceTolerance)
        {

            //var directionToTarget = (Vector3)m_targetInfo.position - m_character.centerMass.position;
            //return directionToTarget.magnitude <= distanceTolerance;

            var dasd = Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) <= distanceTolerance;
            return dasd;
        }


        protected override void Awake()
        {
            base.Awake();
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            m_projectileLauncher = new BallisticProjectileLauncher(m_info.elementalOverloadProjectile.projectileInfo, m_projectilePoint, m_info.projectileGravityScale, m_info.elementalOverloadProjectile.projectileInfo.speed);
            m_damageable.DamageTaken += OnDamageTaken;
            m_BasicAttackDecider = new RandomAttackDecider<Attack>();
            m_ComplexeattackDecider = new RandomAttackDecider<Attack>();
            m_currentAttackDecider = m_BasicAttackDecider;
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            UpdateAttackDeciderList();

            /* m_attackCache = new List<Attack>();
             AddToAttackCache(Attack.Phase1Pattern1, Attack.Phase1Pattern2, Attack.Phase1Pattern3, Attack.Phase1Pattern4, Attack.Phase2Pattern1, Attack.Phase2Pattern2, Attack.Phase2Pattern3, Attack.Phase2Pattern4, Attack.Phase2Pattern5, Attack.Phase2Pattern6);
             m_attackRangeCache = new List<float>();
             AddToRangeCache(m_info.phase1Pattern1Range, m_info.phase1Pattern2Range, m_info.phase1Pattern3Range, m_info.phase1Pattern4Range, m_info.phase2Pattern1Range, m_info.phase2Pattern2Range, m_info.phase2Pattern3Range, m_info.phase2Pattern4Range, m_info.phase2Pattern5Range, m_info.phase2Pattern6Range);
             m_attackUsed = new bool[m_attackCache.Count];*/
            m_currentFullCooldown = new List<float>();
            m_patternCooldown = new List<float>();
        }

        protected override void Start()
        {


            base.Start();
            m_spineListener.Subscribe(m_info.elementalOverloadProjectile.launchOnEvent, LaunchProjectile);
            m_animation.DisableRootMotion();

            m_phaseHandle = new PhaseHandle<Phase, PhaseInfo>();
            m_phaseHandle.Initialize(Phase.PhaseOne, m_info.phaseInfo, m_character, ChangeState, ApplyPhaseData);
            m_phaseHandle.ApplyChange();

            m_currentIdleAnimation = RandomIdleAnimation();
            m_defaultGravityScale = GetComponent<IsolatedCharacterPhysics2D>().gravity.gravityScale;
           // m_muzzleLoopFX.transform.SetParent(null);
            m_backCooldown = m_info.backAttackCooldown;
        }

        private void Update()
        {
            //if (!m_hasPhaseChanged && m_stateHandle.currentState != State.Phasing)
            //{
            //}
            m_phaseHandle.MonitorPhase();
            HandleCooldowns();
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    break;

                case State.Phasing:
                    StartCoroutine(ChangePhaseRoutine());
                    break;
                //case State.Turning:
                //    m_phaseHandle.allowPhaseChange = false;
                //    m_stateHandle.Wait(m_turnState);
                //    m_movement.Stop();
                //    if (IsTargetInRange(m_info.tendrilWhipAttack.range) && m_backCooldown >= m_info.backAttackCooldown)
                //    {
                //       // m_backAttackCoroutine = StartCoroutine(TendrilWhipBackwardRoutine());
                //    }
                //    else
                //    {
                //        m_turnHandle.Execute(m_info.turnAnimation.animation, RandomIdleAnimation());
                //    }
                //    break;
                case State.Attacking:
                    //m_stateHandle.Wait(m_currentAttackCount >= m_maxAttackCount ? State.Roar : State.Cooldown);
                    m_lastTargetPos = m_targetInfo.position;
                    m_currentIdleAnimation = RandomIdleAnimation();
                    StopAllCoroutines();
                    if (m_currentAttackDecider.hasDecidedOnAttack == false)
                    {
                        m_currentAttackDecider.DecideOnAttack();
                    }
                    switch (m_currentAttackDecider.chosenAttack.attack)
                    {

                        case Attack.BackwardTendrilWhip:
                            StartCoroutine(TendrilWhipBackwardAttack());
                            break;
                        case Attack.TwoStomp:
                            StartCoroutine(TwoStompAttack());
                            break;
                        case Attack.TendrilWhip:
                            StartCoroutine(TendrilWhipAttack());
                            break;
                        case Attack.TendrilClimbJumpSmash:
                            StartCoroutine(TendrilClimbJumpSmashAttack());
                            break;
                        case Attack.ElementalBeamFlick:
                            StartCoroutine(ElementalBeamFlickAttack());
                           break;
                        case Attack.TendrilClimbElementalBeam:
                            StartCoroutine(TendrilClimbAttack());
                            break;
                        case Attack.TigrexBiting:
                            StartCoroutine(TigrexBiteAttack());
                            break;
                        case Attack.ElementalOverload:
                            StartCoroutine(ElementalOverloadAttack());
                            break;
                    }
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    /*switch (m_currentAttack)
                    {
                        case Attack.Phase1Pattern1:
                            m_currentAttackCoroutine = StartCoroutine(TwoStompsRoutine());
                            m_pickedCooldown = m_currentFullCooldown[0];
                            break;
                        case Attack.Phase1Pattern2:
                            m_currentAttackCoroutine = StartCoroutine(TendrilWhipRoutine());
                            m_pickedCooldown = m_currentFullCooldown[1];
                            break;
                        case Attack.Phase1Pattern3:
                            m_currentAttackCoroutine = StartCoroutine(TendrilClimbRoutine(WallAttack.ElementalBeam));
                            m_pickedCooldown = m_currentFullCooldown[2];
                            break;
                        case Attack.Phase1Pattern4:
                            m_currentAttackCoroutine = StartCoroutine(ElementalBeamFlickRoutine());
                            m_pickedCooldown = m_currentFullCooldown[3];
                            break;
                        case Attack.Phase2Pattern1:
                            m_currentAttackCoroutine = StartCoroutine(TwoStompsRoutine());
                            m_pickedCooldown = m_currentFullCooldown[0];
                            break;
                        case Attack.Phase2Pattern2:
                            m_currentAttackCoroutine = StartCoroutine(TendrilWhipRoutine());
                            m_pickedCooldown = m_currentFullCooldown[1];
                            break;
                        case Attack.Phase2Pattern3:
                            m_currentAttackCoroutine = StartCoroutine(TendrilClimbRoutine(WallAttack.JumpAttack));
                            m_pickedCooldown = m_currentFullCooldown[2];
                            break;
                        case Attack.Phase2Pattern4:
                            m_currentAttackCoroutine = StartCoroutine(ElementalBeamFlickRoutine());
                            m_pickedCooldown = m_currentFullCooldown[3];
                            break;
                        case Attack.Phase2Pattern5:
                            m_currentAttackCoroutine = StartCoroutine(TigrexBiteRoutine());
                            m_pickedCooldown = m_currentFullCooldown[4];
                            break;
                        case Attack.Phase2Pattern6:
                            m_currentAttackCoroutine = StartCoroutine(ElementalOverloadRoutine());
                            m_pickedCooldown = m_currentFullCooldown[5];
                            break;
                    }*/
                    break;

                /*case State.Cooldown:
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        m_animation.SetAnimation(0, m_currentIdleAnimation, true);
                    }

                    if (m_currentCooldown <= m_pickedCooldown)
                    {
                        m_currentCooldown += Time.deltaTime;
                    }
                    else
                    {
                        m_currentCooldown = 0;
                        //m_stateHandle.OverrideState(State.ReevaluateSituation);
                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                    }

                    break;*/

                /*case State.Chasing:
                    *//*if (IsFacingTarget())
                    {
                        ChooseAttack();
                        Debug.Log(*//*"range for attack is " + IsTargetInRange(m_currentAttackRange) + *//*" attack coroutine is " + (m_currentAttackCoroutine != null));
                        if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_currentAttackRange) && m_currentAttackCoroutine == null)
                        {
                            m_stateHandle.SetState(State.Attacking);
                        }
                        else
                        {
                            if (m_groundSensor.isDetecting *//*&& !m_wallSensor.isDetecting && m_edgeSensor.isDetecting*//*)
                            {
                                m_animation.SetAnimation(0, m_info.move.animation, true);
                                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.move.speed);
                            }
                            else
                            {
                                m_movement.Stop();
                                m_animation.SetAnimation(0, m_currentIdleAnimation, true);
                            }
                        }
                    }
                    else
                    {
                        m_turnState = State.Chasing;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            m_stateHandle.SetState(State.Turning);
                    }*//*
                    break;*/

                case State.ReevaluateSituation:
                    if (m_currentAttackCount == 7)
                    {
                        StopAllCoroutines();
                        StartCoroutine(RoarBehaviour());
                        m_currentAttackCount = 0;
                        Debug.Log("roar");
                    }
                    else
                    {
                        if (IsTargetBehind(m_info.targetDistancetoleranceBehind))
                        {
                            if (canUseBackTendril)
                            {
                                m_stateHandle.SetState(State.Attacking);
                                m_currentAttackDecider.DecideOnAttack(Attack.BackwardTendrilWhip);
                            }
                            Debug.Log("Backward tendril");
                        }
                        else if (IsTargetWithin(m_info.targetDistancetolerance))
                        {
                            //m_stateHandle.SetState(State.Attacking);
                            m_stateHandle.SetState(State.Attacking);
                            m_currentAttackDecider = m_BasicAttackDecider;
                            m_currentAttackDecider.hasDecidedOnAttack = false;
                            //m_currentAttackDecider.DecideOnAttack(Attack.ElementalBeamFlick);
                            Debug.Log("Randomly Choose attack");
                        }
                        else
                        {
                            m_stateHandle.SetState(State.Attacking);
                            m_currentAttackDecider = m_ComplexeattackDecider;
                            m_currentAttackDecider.hasDecidedOnAttack = false;
                            Debug.Log("Randomly Choose range or movement attacks");
                        }
                        //StartCoroutine(TestRoutine());
                    }


                    /* if (m_targetInfo.isValid)
                     {
                         //m_stateHandle.SetState(State.Chasing);
                     }
                     else
                     {
                         m_stateHandle.SetState(State.Idle);
                     }*/
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }
        }

        protected override void OnTargetDisappeared()
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