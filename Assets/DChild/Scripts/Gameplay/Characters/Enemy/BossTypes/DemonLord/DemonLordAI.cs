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
using DChild.Gameplay.Projectiles;
using DChild.Temp;
using System.Linq;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Boss/DemonLord")]
    public class DemonLordAI : CombatAIBrain<DemonLordAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField]
            private PhaseInfo<Phase> m_phaseInfo;
            public PhaseInfo<Phase> phaseInfo => m_phaseInfo;

            [SerializeField]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;
            [SerializeField]
            private MovementInfo m_lightningStepAway = new MovementInfo();
            public MovementInfo lightningStepAway => m_lightningStepAway;
            [SerializeField]
            private MovementInfo m_lightningStepMidair = new MovementInfo();
            public MovementInfo lightningStepMidair => m_lightningStepMidair;

            [Title("Attack Behaviours")]
            //[SerializeField, TabGroup("Ephemeral Arms")]
            //private SimpleAttackInfo m_ephemeralArmsSmashAttack = new SimpleAttackInfo();
            //public SimpleAttackInfo ephemeralArmsSmashAttack => m_ephemeralArmsSmashAttack;
            //[SerializeField, TabGroup("Ephemeral Arms")]
            //private SimpleAttackInfo m_ephemeralArmsComboAttack = new SimpleAttackInfo();
            //public SimpleAttackInfo ephemeralArmsComboAttack => m_ephemeralArmsComboAttack;
            //[SerializeField, TabGroup("Fireball Attacks")]
            //private float m_threeFireBallsCycle;
            //public float threeFireBallsCycle => m_threeFireBallsCycle;
            //[SerializeField, TabGroup("Fireball Attacks")]
            //private SimpleAttackInfo m_threeFireBallsAttack = new SimpleAttackInfo();
            //public SimpleAttackInfo threeFireBallsAttack => m_threeFireBallsAttack;
            //[SerializeField, TabGroup("Fireball Attacks")]
            //private BasicAnimationInfo m_threeFireBallsPreAnimation;
            //public BasicAnimationInfo threeFireBallsPreAnimation => m_threeFireBallsPreAnimation;
            //[SerializeField, TabGroup("Fireball Attacks")]
            //private BasicAnimationInfo m_threeFireBallsFireAnimation;
            //public BasicAnimationInfo threeFireBallsFireAnimation => m_threeFireBallsFireAnimation;
            //[SerializeField, TabGroup("Fireball Attacks")]
            //private SimpleAttackInfo m_flameWaveAttack = new SimpleAttackInfo();
            //public SimpleAttackInfo flameWaveAttack => m_flameWaveAttack;
            [SerializeField, TabGroup("Dragon Breath Attacks")]
            private BasicAnimationInfo m_DragonBreathsAnticipation = new BasicAnimationInfo();
            public BasicAnimationInfo dragonBreathAnticipation => m_DragonBreathsAnticipation;
            [SerializeField, TabGroup("Dragon Breath Attacks")]
            private BasicAnimationInfo m_DragonBreathsAttackLeft = new BasicAnimationInfo();
            public BasicAnimationInfo dragonBreathAttackLeft => m_DragonBreathsAttackLeft;
            [SerializeField, TabGroup("Dragon Breath Attacks")]
            private BasicAnimationInfo m_DragonBreathsAttackRight = new BasicAnimationInfo();
            public BasicAnimationInfo dragonBreathAttackRight => m_DragonBreathsAttackRight;
            [SerializeField, TabGroup("Summon Dragon Attacks")]
            private BasicAnimationInfo m_summonDragonAnticipation = new BasicAnimationInfo();
            public BasicAnimationInfo summonDragonAnticipation => m_summonDragonAnticipation;
            [SerializeField, TabGroup("Summon Dragon Attacks")]
            private BasicAnimationInfo m_summonDragonAttack = new BasicAnimationInfo();
            public BasicAnimationInfo summonDragonAttack => m_summonDragonAttack;
            [SerializeField, TabGroup("Ice Shard Attacks")]
            private BasicAnimationInfo m_iceShardAnticipation = new BasicAnimationInfo();
            public BasicAnimationInfo iceShardAnticipation => m_iceShardAnticipation;
            [SerializeField, TabGroup("Ice Shard Attacks")]
            private BasicAnimationInfo m_iceShardAttack = new BasicAnimationInfo();
            public BasicAnimationInfo iceShardAttack => m_iceShardAttack;
            [SerializeField, TabGroup("Electric Attacks")]
            private BasicAnimationInfo m_lightningOrbAttack = new BasicAnimationInfo();
            public BasicAnimationInfo lightningOrbAttack => m_lightningOrbAttack;
            [SerializeField, TabGroup("Electric Attacks")]
            private BasicAnimationInfo m_lightningOrbSummonAnticipation = new BasicAnimationInfo();
            public BasicAnimationInfo lightningOrbSummonAnticipation => m_lightningOrbSummonAnticipation;
            [SerializeField, TabGroup("Electric Attacks")]
            private BasicAnimationInfo m_lightningOrbSummonLoop = new BasicAnimationInfo();
            public BasicAnimationInfo lightningOrbSummonLoop => m_lightningOrbSummonLoop;


            //[SerializeField, TabGroup("Ice Attacks")]
            //private float m_rayOfFrostChargeDuration;
            //public float rayOfFrostChargeDuration => m_rayOfFrostChargeDuration;
            [SerializeField, TabGroup("Ice Attacks")]
            private SimpleAttackInfo m_rayOfFrostAttack = new SimpleAttackInfo();
            public SimpleAttackInfo rayOfFrostAttack => m_rayOfFrostAttack;
            //[SerializeField, TabGroup("Ice Attacks")]
            //private BasicAnimationInfo m_rayOfFrostChargeAnimation;
            //public BasicAnimationInfo rayOfFrostChargeAnimation => m_rayOfFrostChargeAnimation;
            //[SerializeField, TabGroup("Ice Attacks")]
            //private BasicAnimationInfo m_rayOfFrostFireAnimation;
            //public BasicAnimationInfo rayOfFrostFireAnimation => m_rayOfFrostFireAnimation;
            //[SerializeField, TabGroup("Ice Attacks")]
            //private BasicAnimationInfo m_rayOfFrostFireToIdleAnimation;
            //public BasicAnimationInfo rayOfFrostFireToIdleAnimation => m_rayOfFrostFireToIdleAnimation;
            //[SerializeField, TabGroup("Ice Attacks")]
            //private SimpleAttackInfo m_iceBombAttack = new SimpleAttackInfo();
            //public SimpleAttackInfo iceBombAttack => m_iceBombAttack;
            //[SerializeField, TabGroup("Ice Attacks")]
            //private BasicAnimationInfo m_IceBombThrowAnimation;
            //public BasicAnimationInfo IceBombThrowAnimation => m_IceBombThrowAnimation;
            //[SerializeField, TabGroup("Electric Attacks")]
            //private SimpleAttackInfo m_electricOrbAttack = new SimpleAttackInfo();
            //public SimpleAttackInfo electricOrbAttack => m_electricOrbAttack;
            [SerializeField, TabGroup("Electric Attacks")]
            private SimpleAttackInfo m_lightningGroundAttack = new SimpleAttackInfo();
            public SimpleAttackInfo lightningGroundAttack => m_lightningGroundAttack;
            [SerializeField, TabGroup("Electric Attacks")]
            private SimpleAttackInfo m_lightningStrikeAnticipation = new SimpleAttackInfo();
            public SimpleAttackInfo lightningStrikeAnticipation => m_lightningStrikeAnticipation;
            [SerializeField, TabGroup("Electric Attacks")]
            private SimpleAttackInfo m_lightningStrikeAttack = new SimpleAttackInfo();
            public SimpleAttackInfo lightningStrikeAttack => m_lightningStrikeAttack;

            [TitleGroup("Pattern Ranges")]
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;
            [SerializeField]
            private float m_targetGroundDistanceTolerance;
            public float targetGroundDistanceTolerance => m_targetGroundDistanceTolerance;
            [SerializeField]
            private float m_target1CHDistance;
            public float target1CHDistance => m_target1CHDistance;
            [SerializeField]
            private float m_target3CHDistance;
            public float target3CHDistance => m_target3CHDistance;
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
            [SerializeField, BoxGroup("Phase 2")] //Disabled
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
            [SerializeField, BoxGroup("Phase 3")]
            private float m_phase3Pattern1Range;
            public float phase3Pattern1Range => m_phase3Pattern1Range;
            [SerializeField, BoxGroup("Phase 3")]
            private float m_phase3Pattern2Range;
            public float phase3Pattern2Range => m_phase3Pattern2Range;
            //[SerializeField, BoxGroup("Phase 3")] //Disabled
            //private float m_phase3Pattern3Range;
            //public float phase3Pattern3Range => m_phase3Pattern3Range;
            //[SerializeField, BoxGroup("Phase 3")] //Disabled
            //private float m_phase3Pattern4Range;
            //public float phase3Pattern4Range => m_phase3Pattern4Range;
            [SerializeField, BoxGroup("Phase 3")]
            private float m_phase3Pattern5Range;
            public float phase3Pattern5Range => m_phase3Pattern5Range;
            [SerializeField, BoxGroup("Phase 3")]
            private float m_phase3Pattern6Range;
            public float phase3Pattern6Range => m_phase3Pattern6Range;

            [TitleGroup("Attack Cooldown States")]
            [SerializeField, MinValue(0)]
            private List<float> m_phase1PatternCooldown;
            public List<float> phase1PatternCooldown => m_phase1PatternCooldown;
            [SerializeField, MinValue(0)]
            private List<float> m_phase2PatternCooldown;
            public List<float> phase2PatternCooldown => m_phase2PatternCooldown;
            [SerializeField, MinValue(0)]
            private List<float> m_phase3PatternCooldown;
            public List<float> phase3PatternCooldown => m_phase3PatternCooldown;

            //[Title("Misc")]
            //[SerializeField]
            //private float m_targetDistanceTolerance;
            //public float targetDistanceTolerance => m_targetDistanceTolerance;

            [Title("Animations")]

            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation;
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation;
            public BasicAnimationInfo turnAnimation => m_turnAnimation;
            [SerializeField]
            private BasicAnimationInfo m_teleportDisapear;
            public BasicAnimationInfo teleportDisapear => m_teleportDisapear;
            [SerializeField]
            private BasicAnimationInfo m_teleportAppear;
            public BasicAnimationInfo teleportAppear => m_teleportAppear;
            [SerializeField]
            private BasicAnimationInfo m_rageAnimation;
            public BasicAnimationInfo rageAnimation => m_rageAnimation;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_iceShardCardinalProjectiles;
            public string iceShardCardinalProjectiles => m_iceShardCardinalProjectiles;

            [SerializeField, ValueDropdown("GetEvents")]
            private string m_iceShardDiagonalProjectiles;
            public string iceShardDiagonalProjectiles => m_iceShardDiagonalProjectiles;

            [SerializeField, ValueDropdown("GetEvents")]
            private string m_rayOfFrostBeam;
            public string rayOfFrostBeam => m_rayOfFrostBeam;

            [SerializeField, ValueDropdown("GetEvents")]
            private string m_dragonsBreath;
            public string dragonsBreath => m_dragonsBreath;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_lightningStrike;
            public string lightningStrike => m_lightningStrike;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_LightningOrbs;
            public string LightningOrbs => m_LightningOrbs;
            //[Title("Projectiles")]
            //[SerializeField, BoxGroup("RainProjectiles")]
            //private float m_rainProjectilesDuration;
            //public float rainProjectilesDuration => m_rainProjectilesDuration;
            [SerializeField, BoxGroup("FireBallProjectile")]
            private SimpleProjectileAttackInfo m_fireBallProjectile;
            public SimpleProjectileAttackInfo fireBallProjectile => m_fireBallProjectile;
            [SerializeField, BoxGroup("FireBallProjectile")]
            private float m_fireBallDelay;
            public float fireBallDelay => m_fireBallDelay;
            [SerializeField, BoxGroup("IceBombProjectile")]
            private SimpleProjectileAttackInfo m_iceBombProjectile;
            public SimpleProjectileAttackInfo iceBombProjectile => m_iceBombProjectile;
            [SerializeField, BoxGroup("IceBombProjectile")]
            private float m_iceBombDelay;
            public float iceBombDelay => m_iceBombDelay;
            [SerializeField, BoxGroup("ElectricOrbProjectile")]
            private SimpleProjectileAttackInfo m_electricOrbProjectile;
            public SimpleProjectileAttackInfo electricOrbProjectile => m_electricOrbProjectile;
            [SerializeField, BoxGroup("ElectricOrbProjectile")]
            private float m_electricOrbDelay;
            public float electricOrbDelay => m_electricOrbDelay;
            [SerializeField, BoxGroup("FlameWaveProjectile")]
            private SimpleProjectileAttackInfo m_flameWaveProjectile;
            public SimpleProjectileAttackInfo flameWaveProjectile => m_flameWaveProjectile;
            [SerializeField, BoxGroup("FlameWaveProjectile")]
            private float m_flameWaveDelay;
            public float flameWaveDelay => m_flameWaveDelay;
            [SerializeField, BoxGroup("LightningGroundProjectile")]
            private SimpleProjectileAttackInfo m_lightningGroundProjectile;
            public SimpleProjectileAttackInfo lightningGroundProjectile => m_lightningGroundProjectile;
            [SerializeField, BoxGroup("LightningGroundProjectile")]
            private float m_lightningGroundDelay;
            public float lightningGroundDelay => m_lightningGroundDelay;

            //[Title("Events")]
            //[SerializeField, ValueDropdown("GetEvents")]
            //private string m_deathFXEvent;
            //public string deathFXEvent => m_deathFXEvent;
            //[SerializeField, ValueDropdown("GetEvents")]
            //private string m_teleportFXEvent;
            //public string teleportFXEvent => m_teleportFXEvent;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_move.SetData(m_skeletonDataAsset);
                m_lightningStepAway.SetData(m_skeletonDataAsset);
                m_lightningStepMidair.SetData(m_skeletonDataAsset);
                m_rayOfFrostAttack.SetData(m_skeletonDataAsset);
                //m_ephemeralArmsSmashAttack.SetData(m_skeletonDataAsset);
                //m_ephemeralArmsComboAttack.SetData(m_skeletonDataAsset);
                //m_threeFireBallsAttack.SetData(m_skeletonDataAsset);
                //m_flameWaveAttack.SetData(m_skeletonDataAsset);
                //m_iceBombAttack.SetData(m_skeletonDataAsset);
                //m_electricOrbAttack.SetData(m_skeletonDataAsset);
                m_lightningGroundAttack.SetData(m_skeletonDataAsset);
                m_fireBallProjectile.SetData(m_skeletonDataAsset);
                m_iceBombProjectile.SetData(m_skeletonDataAsset);
                m_electricOrbProjectile.SetData(m_skeletonDataAsset);
                m_lightningOrbAttack.SetData(m_skeletonDataAsset);
                m_lightningOrbSummonAnticipation.SetData(m_skeletonDataAsset);
                m_lightningOrbSummonLoop.SetData(m_skeletonDataAsset);
                m_lightningStrikeAnticipation.SetData(m_skeletonDataAsset);
                m_lightningStrikeAttack.SetData(m_skeletonDataAsset);
                m_flameWaveProjectile.SetData(m_skeletonDataAsset);
              // m_lightningGroundProjectile.SetData(m_skeletonDataAsset);
                m_rageAnimation.SetData(m_skeletonDataAsset);

                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_DragonBreathsAnticipation.SetData(m_skeletonDataAsset);
                m_DragonBreathsAttackLeft.SetData(m_skeletonDataAsset);
                m_DragonBreathsAttackRight.SetData(m_skeletonDataAsset);
                m_summonDragonAnticipation.SetData(m_skeletonDataAsset);
                m_summonDragonAttack.SetData(m_skeletonDataAsset);
                m_iceShardAttack.SetData(m_skeletonDataAsset);
                m_iceShardAnticipation.SetData(m_skeletonDataAsset);
                //m_rayOfFrostChargeAnimation.SetData(m_skeletonDataAsset);
                //m_rayOfFrostFireAnimation.SetData(m_skeletonDataAsset);
                //m_rayOfFrostFireToIdleAnimation.SetData(m_skeletonDataAsset);

                //m_IceBombThrowAnimation.SetData(m_skeletonDataAsset);
                m_teleportDisapear.SetData(m_skeletonDataAsset);
                m_teleportAppear.SetData(m_skeletonDataAsset);
#endif
            }
        }

        [System.Serializable]
        public class PhaseInfo : IPhaseInfo
        {
            [SerializeField]
            private List<float> m_fullCooldown;
            public List<float> fullCooldown => m_fullCooldown;
            //[SerializeField]
            //private int m_hitCount;
            //public int hitCount => m_hitCount;
        }

        [System.Serializable]
        private class LightningStrikePositioningInfo
        {
            [SerializeField]
            private Transform m_lightningSpawnPoint;
            [SerializeField]
            private Transform m_castingPoint;

            public Vector3 lightningSpawnPosition => m_lightningSpawnPoint.position;
            public Vector3 castingPosition => m_castingPoint.position;
        }

        private enum State
        {
            Phasing,
            Intro,
            Idle,
            Turning,
            Attacking,
            Cooldown,
            Chasing,
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
            Phase3Pattern1,
            Phase3Pattern2,
            //Phase3Pattern3,
            //Phase3Pattern4,
            Phase3Pattern5,
            Phase3Pattern6,
            WaitAttackEnd,
        }

        //private enum FollowUpAttack
        //{
        //    EphemeralArmsSmash,
        //    EphemeralArmsCombo,
        //    ThreeFireBalls,
        //    FlameWave,
        //    RayOfFrost,
        //    IceBomb,
        //    ElectricOrb,
        //    LightningGround,
        //}

        //private enum Attack
        //{
        //    OrbSummonRainProjectiles,
        //    StaffPointRainProjectiles,
        //    SingleFleshSpike,
        //    EnragedMultipleFleshSpikes,
        //    FleshBomb,
        //    WaitAttackEnd,
        //}

        public enum Phase
        {
            PhaseOne,
            PhaseTwo,
            //PhaseThree,
            Wait,
        }
        [SerializeField, TabGroup("Reference")]
        private Boss m_boss;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        //[SerializeField, TabGroup("Reference")]
        //private DemonLordEphemeralArms m_ephemeralArms;
        [SerializeField, TabGroup("Reference")]
        private DemonLordEphemeralArms m_ephemeralArmsFront;
        [SerializeField, TabGroup("Reference")]
        private DemonLordEphemeralArms m_ephemeralArmsBack;
        [SerializeField, TabGroup("Reference")]
        private DemonLordBook m_book;
        [SerializeField, TabGroup("Reference")]
        private DragonsBreathFireController m_fireController;
        [SerializeField, TabGroup("Reference")]
        private DemonLordLightningStrike m_demonLordLightningStrike;
        [SerializeField, TabGroup("Reference")]
        private DemonLordIceShardSpell m_demonLordIceShardSpell;
        [SerializeField, TabGroup("Reference")]
        private DemonLordLightningConstellation m_demonLordLightningConstellation;

        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private PathFinderAgent m_agent;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandler;

        [SerializeField, TabGroup("Lazer")]
        private LineRenderer m_lineRenderer;
        //[SerializeField, TabGroup("Lazer")]
        //private LineRenderer m_telegraphLineRenderer;
        [SerializeField, TabGroup("Lazer")]
        private EdgeCollider2D m_edgeCollider;
        [SerializeField, TabGroup("Lazer")]
        private ParticleFX m_muzzleLoopFX;
        [SerializeField, TabGroup("Lazer")]
        private ParticleFX m_muzzleTelegraphFX;
        [SerializeField, TabGroup("Lazer")]
        private Transform m_beamFrontPoint;
        [SerializeField, TabGroup("Lazer")]
        private Transform m_beamBackPoint;
        private bool m_beamOn;
        private bool m_aimOn;
        [SerializeField, TabGroup("Lazer")]
        private SkeletonUtilityBone m_aimBone;
        [SerializeField, TabGroup("Lazer")]
        private float m_armRotationOffset;
        private List<Vector2> m_Points;
        private IEnumerator m_aimRoutine;
        [SerializeField, TabGroup("Lazer")]
        private GameObject m_rayOfFrostBeam;

        [SerializeField, TabGroup("FX")]
        private ParticleFX m_lightingStepAwayFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_lightingStepMidairFX;
        [SerializeField, TabGroup("FX")]
        private float m_lightingStepMidairFXYOFfset;
        [SerializeField, TabGroup("FX animator")]
        private Animator m_rayOfFrostAnimatorRight;
        [SerializeField, TabGroup("FX animator")]
        private Animator m_rayOfFrostAnimatorLeft;
        [SerializeField, TabGroup("FX animator")]
        private Animator m_dragonsBreathAnimator;
        //[SerializeField, TabGroup("FX animator")]
        //private Animator m_dragonsBreathAnimatorFxSide1;
        //[SerializeField, TabGroup("FX animator")]
        //private Animator m_dragonsBreathAnimatorFxSide2;


        [SerializeField]
        private SpineEventListener m_spineListener;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        State m_turnState;
        [ShowInInspector]
        private PhaseHandle<Phase, PhaseInfo> m_phaseHandle;
        //[ShowInInspector]
        //private RandomAttackDecider<Pattern> m_patternDecider;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_currentAttack;
        private Attack previousAttack;
        private float m_currentAttackRange;

        #region ProjectileLaunchers
        private ProjectileLauncher m_fireBallProjectileLauncher;
        private ProjectileLauncher m_iceBombProjectileLauncher;
        private ProjectileLauncher m_electricOrbProjectileLauncher;
        private ProjectileLauncher m_flameWaveProjectileLauncher;
        private ProjectileLauncher m_lightningGroundProjectileLauncher;
        #endregion

        private List<GameObject> m_electricOrbs;

        [SerializeField, TabGroup("Spawn Points")]
        private LightningStrikePositioningInfo[] m_lightningStrikePositioningInfos;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_fireBallSpawnPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_iceBombSpawnPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private List<Transform> m_iceShardsSpawnPointPattern1;
        [SerializeField, TabGroup("Spawn Points")]
        private List<Transform> m_iceShardsSpawnPointPattern2;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_electricOrbSpawnPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_flameWaveSpawnPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_lightningGroundSpawnPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private List<Transform> m_teleportSpawnPointsA;
        [SerializeField, TabGroup("Spawn Points")]
        private List<Transform> m_teleportSpawnPointsB;
        [SerializeField, TabGroup("Spawn Points")]
        private List<Transform> m_teleportSpawnPointsC;
        [SerializeField, TabGroup("Spawn Points")]
        private List<Transform> m_rayOfFrostPosition;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_rayOfFrostBeamPosition;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_dragonsBreathPoint;
        [SerializeField, TabGroup("Target Points")]
        private Transform m_fireBallTargetPoint;
        [SerializeField, TabGroup("Target Points")]
        private List<Transform> m_iceBombTargetPoints;
        [SerializeField, TabGroup("Target Points")]
        private List<Transform> m_electricOrbTargetPoints;
        [SerializeField, TabGroup("Target Points")]
        private List<Transform> m_LeftRightPointsCalculation;
        [SerializeField, TabGroup("Values for behaviours")]
        private int m_spawnPointIndexForTeleportPhase;
        [SerializeField, TabGroup("Values for behaviours")]
        private float m_distanceStoppingToleranceForRayFrost;
        private Dictionary<string, List<Transform>> m_listOfSpawnPoints = new Dictionary<string, List<Transform>>();

        private Vector2 m_lastTargetPos;
        private Vector2 m_lazerTargetPos;
        private float m_currentCooldown;
        private float m_pickedCooldown;
        private List<float> m_currentFullCooldown;
        private int[] m_patternAttackCount;
        private List<float> m_patternCooldown;
        //private int m_maxHitCount;
        //private int m_currentHitCount;

        private Coroutine m_currentAttackCoroutine;
        #region Lazer Coroutine
        private Coroutine m_lazerBeamCoroutine;
        private Coroutine m_lazerLookCoroutine;
        private Coroutine m_aimAtPlayerCoroutine;
        #endregion
        private bool m_isDetecting;
        [SerializeField]
        private bool m_startingPoint = true;
        [SerializeField]
        private bool m_IceShardPattern1Done = false;
        [SerializeField]
        private bool m_rayOfFrostBeamActivated = false;


        private string m_chosenPointNameForRayFrost;
 
        private void ApplyPhaseData(PhaseInfo obj)
        {
         
            if (m_patternCooldown.Count != 0)
                m_patternCooldown.Clear();
            UpdateAttackDeciderList();
            //m_maxHitCount = obj.hitCount;
            if (m_currentFullCooldown.Count != 0)
            {
                m_currentFullCooldown.Clear();
            }
            for (int i = 0; i < obj.fullCooldown.Count; i++)
            {
                m_currentFullCooldown.Add(obj.fullCooldown[i]);
            }
        }

        private void ChangeState()
        {
            if (m_currentAttackCoroutine != null)
            {
                StopCoroutine(m_currentAttackCoroutine);
                m_currentAttackCoroutine = null;
                m_attackDecider.hasDecidedOnAttack = false;
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
            enabled = true;
            StopAllCoroutines();
            UpdateAttackDeciderList();
            m_stateHandle.OverrideState(State.Phasing);
            m_animation.DisableRootMotion();
            m_animation.SetEmptyAnimation(0, 0);
            m_phaseHandle.ApplyChange();
        }

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null && m_stateHandle.currentState == State.Idle)
            {
                base.SetTarget(damageable, m_target);
                if (!m_isDetecting)
                {
                    m_isDetecting = true;
                    m_stateHandle.OverrideState(State.Intro);
                    //GameEventMessage.SendEvent("Boss Encounter");
                }
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            if (m_stateHandle.currentState != State.Phasing)
            {
                m_book.Idle(true);
                m_animation.animationState.TimeScale = 1f;
                m_stateHandle.ApplyQueuedState();
            }
        }

        //private IEnumerator IntroRoutine()
        //{
        //    m_stateHandle.Wait(State.ReevaluateSituation);
        //    m_agent.Stop();
        //    m_hitbox.SetInvulnerability(Invulnerability.MAX);
        //    yield return new WaitForSeconds(2);
        //    m_animation.SetAnimation(0, m_info.move.animation, true);
        //    yield return new WaitForSeconds(5);
        //    //GetComponentInChildren<MeshRenderer>().sortingOrder = 99;
        //    m_book.EphemeralArmsSmash(false);
        //    //m_ephemeralArms.EphemeralArmsSmash(false);
        //    m_ephemeralArmsFront.EphemeralArmsSmash(false);
        //    m_ephemeralArmsBack.EphemeralArmsSmash(false);
        //    m_animation.SetAnimation(0, m_info.ephemeralArmsSmashAttack.animation, false);
        //    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.ephemeralArmsSmashAttack.animation);
        //    m_book.Idle(true);
        //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
        //    m_hitbox.SetInvulnerability(Invulnerability.None);
        //    m_stateHandle.ApplyQueuedState();
        //    yield return null;
        //}

        //private void ResetCounts()
        //{
        //    m_currentHitCount = 0;
        //}

        private IEnumerator ChangePhaseRoutine()
        {
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            enabled = false;
            //m_hitbox.SetCanBlockDamageState(false);
            m_hitbox.Disable();
            //m_stateHandle.Wait(State.Chasing);
            //ResetCounts();

            m_animation.SetAnimation(0, m_info.rageAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rageAnimation);

            m_hitbox.Enable();
            m_stateHandle.ApplyQueuedState();
            m_hitbox.SetInvulnerability(Invulnerability.None);
            yield return null;
            enabled = true;
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            m_book.Flinch(false);
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            m_book.Idle(true);
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            m_book.Death(false);
            StopAllCoroutines();
            m_agent.Stop();
            m_isDetecting = false;
        }

        #region Attacks

        //private IEnumerator EphemeralArmsSmashAttackRoutine(FollowUpAttack attack)
        //{
        //    m_book.EphemeralArmsSmash(false);
        //    //m_ephemeralArms.EphemeralArmsSmash(false);
        //    m_ephemeralArmsFront.EphemeralArmsSmash(false);
        //    m_ephemeralArmsBack.EphemeralArmsSmash(false);
        //    m_animation.SetAnimation(0, m_info.ephemeralArmsSmashAttack.animation, false);
        //    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.ephemeralArmsSmashAttack.animation);
        //    //m_attackDecider.hasDecidedOnAttack = false;
        //    m_currentAttackCoroutine = null;
        //    //m_stateHandle.ApplyQueuedState();
        //    if (m_phaseHandle.currentPhase == Phase.PhaseThree)
        //    {
        //        if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) <= m_info.target3CHDistance)
        //        {
        //            attack = FollowUpAttack.EphemeralArmsCombo;
        //        }
        //    }
        //    //else
        //    //{
        //    //    if (attack == FollowUpAttack.ThreeFireBalls)
        //    //    {
        //    //        if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) <= m_info.target1CHDistance)
        //    //        {
        //    //            yield return null;
        //    //        }
        //    //    }
        //    //}
        //    switch (attack)
        //    {
        //        case FollowUpAttack.EphemeralArmsCombo:
        //            m_currentAttackCoroutine = StartCoroutine(EphemeralArmsComboAttackRoutine());
        //            break;
        //        //case FollowUpAttack.ThreeFireBalls:
        //        //    m_currentAttackCoroutine = StartCoroutine(ThreeFireBallsAttackRoutine());
        //        //    break; Removed
        //        case FollowUpAttack.FlameWave:
        //            m_currentAttackCoroutine = StartCoroutine(FlameWaveAttackRoutine());
        //            break;
        //        case FollowUpAttack.RayOfFrost:
        //            m_currentAttackCoroutine = StartCoroutine(RayOfFrostRoutine());
        //            break;
        //        //case FollowUpAttack.IceBomb:
        //        //    m_currentAttackCoroutine = StartCoroutine(IceBombAttackRoutine());
        //        //    break;
        //        case FollowUpAttack.ElectricOrb:
        //            m_currentAttackCoroutine = StartCoroutine(ElectricOrbAttackRoutine());
        //            break;
        //            //case FollowUpAttack.LightningGround:
        //            //    m_currentAttackCoroutine = StartCoroutine(LightningGroundAttackRoutine());
        //            //    break;

        //    }
        //    yield return null;
        //}

        //private IEnumerator EphemeralArmsComboAttackRoutine() // remove? 
        //{
        //    m_book.EphemeralArmsCombo(false);
        //    //m_ephemeralArms.EphemeralArmsCombo(false);
        //    m_ephemeralArmsFront.EphemeralArmsCombo(false);
        //    m_ephemeralArmsBack.EphemeralArmsCombo(false);
        //    m_animation.SetAnimation(0, m_info.ephemeralArmsComboAttack.animation, false);
        //    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.ephemeralArmsComboAttack.animation);
        //    m_book.Idle(true);
        //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
        //    m_attackDecider.hasDecidedOnAttack = false;
        //    m_currentAttackCoroutine = null;
        //    m_stateHandle.ApplyQueuedState();
        //    yield return null;
        //}

        //private IEnumerator ThreeFireBallsAttackRoutine() // remove 
        //{
        //    m_book.ThreeFireBallsPre(false);
        //    //m_ephemeralArms.ThreeFireBallsPre(false);
        //    m_ephemeralArmsFront.ThreeFireBallsPre(false);
        //    m_ephemeralArmsBack.ThreeFireBallsPre(false);
        //    m_animation.SetAnimation(0, m_info.threeFireBallsPreAnimation, false);
        //    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.threeFireBallsPreAnimation);
        //    m_book.ThreeFireBallsFire(false);
        //    //m_ephemeralArms.ThreeFireBallsFire(false);
        //    m_ephemeralArmsFront.ThreeFireBallsFire(false);
        //    m_ephemeralArmsBack.ThreeFireBallsFire(false);
        //    m_animation.SetAnimation(0, m_info.threeFireBallsFireAnimation, false);
        //    for (int i = 0; i < m_info.threeFireBallsCycle; i++)
        //    {
        //        m_fireBallProjectileLauncher.AimAt(m_targetInfo.position);
        //        yield return new WaitForSeconds(m_info.fireBallDelay);
        //        m_fireBallProjectileLauncher.LaunchProjectile();
        //        yield return null;
        //    }
        //    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.threeFireBallsFireAnimation);
        //    m_book.Idle(true);
        //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
        //    m_attackDecider.hasDecidedOnAttack = false;
        //    m_currentAttackCoroutine = null;
        //    m_stateHandle.ApplyQueuedState();
        //    yield return null;
        //}

        //private IEnumerator FlameWaveAttackRoutine()
        //{
        //    m_book.FlameWave(false);
        //    m_animation.SetAnimation(0, m_info.flameWaveAttack.animation, false);
        //    yield return new WaitForSeconds(m_info.flameWaveDelay);
        //    var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_info.flameWaveProjectile.projectileInfo.projectile);
        //    instance.transform.position = new Vector2(m_flameWaveSpawnPoint.position.x, GroundPosition(m_flameWaveSpawnPoint.position).y);

        //    m_flameWaveProjectileLauncher.LaunchProjectile(m_character.facing == HorizontalDirection.Right ? m_flameWaveSpawnPoint.right : -m_flameWaveSpawnPoint.right, instance.gameObject);
        //    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flameWaveAttack.animation);
        //    m_book.Idle(true);
        //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
        //    m_attackDecider.hasDecidedOnAttack = false;
        //    m_currentAttackCoroutine = null;
        //    m_stateHandle.ApplyQueuedState();
        //    yield return null;
        //}

        //private IEnumerator RayOfFrostAttackRoutine()
        //{
        //    m_lazerLookCoroutine = StartCoroutine(LazerLookRoutine());
        //    m_aimOn = true;
        //    //m_aimAtPlayerCoroutine = StartCoroutine(AimAtTargtRoutine());
        //    m_animation.EnableRootMotion(true, true);
        //    m_book.LightningMidair(false);
        //    m_animation.SetAnimation(0, m_info.lightningStepMidair.animation, false);
        //    m_lightingStepMidairFX.transform.position = new Vector2(transform.position.x, GroundPosition(m_lightingStepMidairFX.transform.position).y + m_lightingStepMidairFXYOFfset);
        //    m_lightingStepMidairFX.Play();
        //    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.lightningStepMidair.animation);
        //    m_animation.DisableRootMotion();
        //    m_book.RayOfFrost(false);
        //    m_ephemeralArmsFront.RayOfFrost(false);
        //    m_ephemeralArmsBack.RayOfFrost(false);
        //    m_animation.SetAnimation(0, m_info.rayOfFrostAttack.animation, false);
        //    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rayOfFrostAttack.animation);
        //    m_book.RayOfFrostCharge(true);
        //    m_ephemeralArmsFront.RayOfFrostCharge(true);
        //    m_ephemeralArmsBack.RayOfFrostCharge(true);
        //    m_animation.SetAnimation(0, m_info.rayOfFrostChargeAnimation, true);
        //    m_lineRenderer.gameObject.SetActive(true);
        //    m_beamOn = true;
        //    m_lazerBeamCoroutine = StartCoroutine(LazerBeamRoutine());
        //    yield return new WaitForSeconds(m_info.rayOfFrostChargeDuration);
        //    m_beamOn = false;
        //    m_lineRenderer.gameObject.SetActive(false);
        //    m_book.RayOfFrostFire(false);
        //    m_ephemeralArmsFront.RayOfFrostFire(false);
        //    m_ephemeralArmsBack.RayOfFrostFire(false);
        //    m_animation.SetAnimation(0, m_info.rayOfFrostFireAnimation, false);
        //    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rayOfFrostFireAnimation);
        //    StopCoroutine(m_lazerLookCoroutine);
        //    m_lazerLookCoroutine = null;
        //    m_aimOn = false;
        //    m_book.RayOfFrostFireToIdle(false);
        //    m_ephemeralArmsFront.RayOfFrostFireToIdle(false);
        //    m_ephemeralArmsBack.RayOfFrostFireToIdle(false);
        //    m_animation.SetAnimation(0, m_info.rayOfFrostFireToIdleAnimation, false);
        //    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rayOfFrostFireToIdleAnimation);
        //    m_book.Idle(true);
        //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
        //    m_attackDecider.hasDecidedOnAttack = false;
        //    m_currentAttackCoroutine = null;
        //    m_stateHandle.ApplyQueuedState();
        //    yield return null;
        //}
        private IEnumerator RayOfFrostRoutine()
        {
            var randIndexForRayOfFrost = UnityEngine.Random.Range(0, 2);
            var chosenPointForRayOfFrost = m_rayOfFrostPosition[randIndexForRayOfFrost];
            Debug.Log(chosenPointForRayOfFrost.name.ToString());
            m_chosenPointNameForRayFrost = chosenPointForRayOfFrost.name.ToString();

            while (Vector3.Distance(transform.position, chosenPointForRayOfFrost.position) > 0.3f)
            {
                var distanceCalculationDLordAndFrost = (chosenPointForRayOfFrost.position - transform.position).normalized;
                transform.position += m_info.move.speed * Time.deltaTime * distanceCalculationDLordAndFrost;
                Debug.Log("lean D ro");
                yield return null;
            }
            transform.position = chosenPointForRayOfFrost.position;
            m_agent.Stop();
            Debug.Log("Done transfer");
            if (IsFacingTarget() == false)
            {
                m_turnHandle.ExecuteWithAnimationByPass();
            }
            m_animation.DisableRootMotion();
            m_animation.SetAnimation(0, m_info.rayOfFrostAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rayOfFrostAttack.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        private void RayOfFrostBeamController()
        {
            RayOfFrostActivator(m_chosenPointNameForRayFrost);
        }

        private void RayOfFrostActivator(string chosenPointName)
        {
            var name = "Right";
            if (chosenPointName == name)
            {
                var rayOfFrost = "RayOfFrost";
                Debug.Log(rayOfFrost);
                m_rayOfFrostAnimatorRight.SetTrigger(rayOfFrost);

            }
            else
            {
                var rayOfFrostSide2 = "RayOfFrost";
                Debug.Log(rayOfFrostSide2);
                m_rayOfFrostAnimatorLeft.SetTrigger(rayOfFrostSide2);
            }
        }


        //private IEnumerator IceBombAttackRoutine() //remove 
        //{
        //    m_book.IceBomb(false);
        //    //m_ephemeralArms.IceBomb(false);
        //    m_ephemeralArmsFront.IceBomb(false);
        //    m_ephemeralArmsBack.IceBomb(false);
        //    m_animation.SetAnimation(0, m_info.iceBombAttack.animation, false);
        //    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.iceBombAttack.animation);
        //    m_book.IceBombThrow(false);
        //    //m_ephemeralArms.IceBombThrow(false);
        //    m_ephemeralArmsFront.IceBombThrow(false);
        //    m_ephemeralArmsBack.IceBombThrow(false);
        //    m_animation.SetAnimation(0, m_info.IceBombThrowAnimation, false);
        //    yield return new WaitForSeconds(m_info.iceBombDelay);
        //    for (int i = 0; i < m_iceBombTargetPoints.Count; i++)
        //    {
        //        var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_info.iceBombProjectile.projectileInfo.projectile);
        //        instance.transform.position = m_iceBombSpawnPoint.position;
        //        instance.GetComponent<DemonLordIceBomb>().SetTarget(m_iceBombTargetPoints[i]);

        //        m_iceBombProjectileLauncher.AimAt(m_iceBombTargetPoints[i].position);
        //        m_iceBombProjectileLauncher.LaunchProjectile(m_iceBombSpawnPoint.right, instance.gameObject);
        //    }
        //    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.IceBombThrowAnimation);
        //    m_book.Idle(true);
        //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
        //    m_attackDecider.hasDecidedOnAttack = false;
        //    m_currentAttackCoroutine = null;
        //    m_stateHandle.ApplyQueuedState();
        //    yield return null;

        //}

        //private IEnumerator ElectricOrbAttackRoutine()
        //{
        //    //m_ephemeralArms.ThreeFireBallsPre(false);
        //    m_ephemeralArmsFront.ThreeFireBallsPre(false);
        //    m_ephemeralArmsBack.ThreeFireBallsPre(false);
        //    m_animation.SetAnimation(0, m_info.electricOrbAttack.animation, false);
        //    for (int i = 0; i < m_electricOrbTargetPoints.Count; i++)
        //    {
        //        var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_info.electricOrbProjectile.projectileInfo.projectile);
        //        m_electricOrbs.Add(instance.gameObject);
        //        //instance.transform.position = m_electricOrbTargetPoints[i].position;
        //    }
        //    var timer = m_info.electricOrbDelay;
        //    while (timer > 0)
        //    {
        //        for (int i = 0; i < m_electricOrbTargetPoints.Count; i++)
        //        {
        //            m_electricOrbs[i].transform.position = m_electricOrbTargetPoints[i].position;
        //        }
        //        m_electricOrbSpawnPoint.position = m_targetInfo.position;
        //        timer -= Time.deltaTime;
        //        yield return null;
        //    }
        //    for (int i = 0; i < m_electricOrbTargetPoints.Count; i++)
        //    {
        //        //var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_info.electricOrbProjectile.projectileInfo.projectile);
        //        //instance.transform.position = m_electricOrbTargetPoints[i].position;
        //        m_electricOrbProjectileLauncher = new ProjectileLauncher(m_info.electricOrbProjectile.projectileInfo, m_electricOrbTargetPoints[i]);

        //        m_electricOrbs[i].GetComponent<Collider2D>().enabled = true;

        //        m_electricOrbProjectileLauncher.AimAt(m_targetInfo.position);
        //        m_electricOrbProjectileLauncher.LaunchProjectile(m_electricOrbTargetPoints[i].right, m_electricOrbs[i].gameObject);
        //    }
        //    m_electricOrbs.Clear();
        //    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.electricOrbAttack.animation);
        //    //m_ephemeralArms.EmptyAnimation();
        //    m_ephemeralArmsFront.EmptyAnimation();
        //    m_ephemeralArmsBack.EmptyAnimation();
        //    m_book.Idle(true);
        //    m_animation.SetAnimation(0, m_info.idleAnimation, true);
        //    m_attackDecider.hasDecidedOnAttack = false;
        //    m_currentAttackCoroutine = null;
        //    m_stateHandle.ApplyQueuedState();
        //    yield return null;
        //}

        private IEnumerator LightningGroundAttackRoutine() //remove?
        {
            m_book.LightningGround(false);
            //m_ephemeralArms.LightningGround(false);
            m_ephemeralArmsFront.LightningGround(false);
            m_ephemeralArmsBack.LightningGround(false);
            m_animation.SetAnimation(0, m_info.lightningGroundAttack.animation, false);
            yield return new WaitForSeconds(m_info.lightningGroundDelay);
            var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_info.lightningGroundProjectile.projectileInfo.projectile);
            instance.transform.position = new Vector2(m_lightningGroundSpawnPoint.position.x, GroundPosition(m_lightningGroundSpawnPoint.position).y);
            //m_lightningGroundProjectileLauncher.LaunchProjectile(i == 0 ? m_lightningGroundSpawnPoint.right : -m_lightningGroundSpawnPoint.right, instance.gameObject);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.lightningGroundAttack.animation);
            m_book.Idle(true);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        #region Lazer Attack
        private IEnumerator LazerLookRoutine()
        {
            while (true)
            {
                m_lazerTargetPos = LookPosition(m_character.facing == HorizontalDirection.Right ? m_beamFrontPoint : m_beamBackPoint/*m_beamPoint*/);
                yield return null;
            }
            yield return null;
        }

        public IEnumerator AimAtTargtRoutine()
        {
            m_aimBone.mode = SkeletonUtilityBone.Mode.Override;
            while (m_aimOn)
            {
                //m_aimBone.transform.position = new Vector2(m_targetInfo.position.x, m_targetInfo.position.y - 5f);
                Vector2 spitPos = m_aimBone.transform.position;
                Vector3 v_diff = (m_targetInfo.position - spitPos);
                float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
                m_aimBone.transform.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg);
                yield return null;
            }
            m_aimBone.mode = SkeletonUtilityBone.Mode.Follow;
            m_aimAtPlayerCoroutine = null;
            yield return null;
        }

        private IEnumerator AimRoutine()
        {
            while (true)
            {
                //m_telegraphLineRenderer.SetPosition(0, m_telegraphLineRenderer.transform.position);
                m_lineRenderer.SetPosition(0, m_lineRenderer.transform.position);
                m_lineRenderer.SetPosition(1, m_lineRenderer.transform.position);
                yield return null;
            }
        }

        private Vector2 ShotPosition()
        {
            m_lazerTargetPos = LookPosition(m_character.facing == HorizontalDirection.Right ? m_beamFrontPoint : m_beamBackPoint/*m_beamPoint*/);
            Vector2 startPoint = m_beamFrontPoint.position;
            Vector2 direction = (m_lazerTargetPos - startPoint).normalized;

            RaycastHit2D hit = Physics2D.Raycast(/*m_projectilePoint.position*/startPoint, direction, 1000, DChildUtility.GetEnvironmentMask());
            //Debug.DrawRay(startPoint, direction);
            return hit.point;
        }

        //private IEnumerator TelegraphLineRoutine()
        //{
        //    //float timer = 0;
        //    m_muzzleTelegraphFX.Play();
        //    m_telegraphLineRenderer.useWorldSpace = true;
        //    m_telegraphLineRenderer.SetPosition(1, ShotPosition());
        //    var timerOffset = m_telegraphLineRenderer.startWidth;
        //    while (m_telegraphLineRenderer.startWidth > 0)
        //    {
        //        m_telegraphLineRenderer.startWidth -= Time.deltaTime * timerOffset;
        //        yield return null;
        //    }
        //    yield return null;
        //}

        private IEnumerator LazerBeamRoutine()
        {
            if (!m_aimOn)
            {
                //StartCoroutine(TelegraphLineRoutine());
                StartCoroutine(m_aimRoutine);
            }

            yield return new WaitUntil(() => m_beamOn);
            StopCoroutine(m_aimRoutine);
            m_muzzleLoopFX.Play();
            m_muzzleTelegraphFX.Play();

            m_lineRenderer.useWorldSpace = true;
            while (m_beamOn)
            {
                m_muzzleTelegraphFX.transform.position = ShotPosition();

                m_lineRenderer.SetPosition(0, m_beamFrontPoint.position);
                m_lineRenderer.SetPosition(1, ShotPosition());
                for (int i = 0; i < m_lineRenderer.positionCount; i++)
                {
                    var pos = m_lineRenderer.GetPosition(i) - m_edgeCollider.transform.position;
                    pos = new Vector2(Mathf.Abs(pos.x), pos.y);
                    //if (i > 0)
                    //{
                    //    pos = pos * 0.7f;
                    //}
                    m_Points.Add(pos);
                }
                m_edgeCollider.points = m_Points.ToArray();
                m_Points.Clear();
                yield return null;
            }
            m_muzzleLoopFX.Stop();
            m_muzzleTelegraphFX.Stop();
            //yield return new WaitUntil(() => !m_beamOn);
            ResetLaser();
            m_lazerBeamCoroutine = null;
            yield return null;
        }

        private void ResetLaser()
        {
            //m_telegraphLineRenderer.useWorldSpace = false;
            m_lineRenderer.useWorldSpace = false;
            m_lineRenderer.SetPosition(0, Vector3.zero);
            m_lineRenderer.SetPosition(1, Vector3.zero);
            //m_telegraphLineRenderer.SetPosition(0, Vector3.zero);
            //m_telegraphLineRenderer.SetPosition(1, Vector3.zero);
            //m_telegraphLineRenderer.startWidth = 1;
            m_Points.Clear();
            for (int i = 0; i < m_lineRenderer.positionCount; i++)
            {
                m_Points.Add(Vector2.zero);
            }
            m_edgeCollider.points = m_Points.ToArray();
        }
        #endregion
        #endregion

        #region Movement
        private IEnumerator LightningStepRoutine()// remove 
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_animation.EnableRootMotion(true, true);
            m_animation.SetAnimation(0, m_info.lightningStepAway.animation, false);
            m_animation.AddAnimation(0, m_info.idleAnimation, true, 0);
            m_lightingStepAwayFX.Play();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.idleAnimation);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.lightningStepAway.animation);
            m_animation.DisableRootMotion();
            //m_animation.SetAnimation(0, m_info.idleAnimation, true).MixDuration = 0;
            //m_attackDecider.hasDecidedOnAttack = false;
            //m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator TeleportRoutine()
        {
            var spawnPointIndex1 = 0;
            var spawnPointIndex2 = 1;
            var spawnPointIndex3 = 2;
            m_stateHandle.Wait(State.ReevaluateSituation);
            var randomSpawnPoint = UnityEngine.Random.Range(0, m_listOfSpawnPoints.Count);
            Debug.Log(m_listOfSpawnPoints.Count.ToString());
            string[] patternKeys = new string[m_listOfSpawnPoints.Count];
            m_listOfSpawnPoints.Keys.CopyTo(patternKeys, 0);
            if (m_startingPoint)
            {
                StartCoroutine(TeleportPatterns(randomSpawnPoint, patternKeys, 1, 2));
                m_startingPoint = false;
            }
            else
            {
                if (m_spawnPointIndexForTeleportPhase == spawnPointIndex1)
                {
                    StartCoroutine(TeleportPatterns(randomSpawnPoint, patternKeys, 0, 1));
                }
                else if (m_spawnPointIndexForTeleportPhase == spawnPointIndex2)
                {
                    Debug.Log("spawn point B");
                    var randomSpawnPoint2 = UnityEngine.Random.Range(0, 4);// special case since the points is 4 
                    Debug.Log(randomSpawnPoint2);
                    StartCoroutine(TeleportPatterns(randomSpawnPoint, patternKeys, 1, 2));
                }
                else if (m_spawnPointIndexForTeleportPhase == spawnPointIndex3)
                {
                    StartCoroutine(TeleportPatterns(randomSpawnPoint, patternKeys, 2, 1));
                }
            }
            yield return null;
        }

        private IEnumerator TeleportPatterns(int randomSpawnPoint, string[] patternKeys, int spawnIndextoAvoid, int fallBackSpawnIndex)
        {
            m_spawnPointIndexForTeleportPhase = UnityEngine.Random.Range(0, m_listOfSpawnPoints.Count);
            m_spawnPointIndexForTeleportPhase = (m_spawnPointIndexForTeleportPhase == spawnIndextoAvoid) ? fallBackSpawnIndex : m_spawnPointIndexForTeleportPhase;
            var randomSpawnPointList = patternKeys[m_spawnPointIndexForTeleportPhase];
            List<Transform> randomSpawnPoints = m_listOfSpawnPoints[randomSpawnPointList];
            m_animation.EnableRootMotion(true, true);
            m_animation.SetAnimation(0, m_info.teleportDisapear.animation, false);
            var teleportPoint = randomSpawnPoints[randomSpawnPoint];
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.teleportDisapear.animation);
            transform.position = teleportPoint.position;
            if (IsFacingTarget() == false)
            {
                m_turnHandle.ExecuteWithAnimationByPass();
            }
            m_animation.SetAnimation(0, m_info.teleportAppear.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.teleportAppear.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_animation.DisableRootMotion();
            Debug.Log(randomSpawnPointList.ToString() + " spawn point list");
            Debug.Log(m_spawnPointIndexForTeleportPhase + " spawn point number");
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator DragonsBreathRoutine()
        {
            // get transform point of spawn pattern b top 
            // play animation var randIndexForRayOfFrost = UnityEngine.Random.Range(0, 2);
            var positionForDragonsBreath = m_dragonsBreathPoint;
            m_fireController.SetActiveDragonTrail(true);
            while (Vector3.Distance(transform.position, positionForDragonsBreath.position) > 0.2f)
            {
                var distanceCalcuOfTwoPosition = (positionForDragonsBreath.position - transform.position).normalized;
                transform.position += m_info.move.speed * Time.deltaTime * distanceCalcuOfTwoPosition;
                Debug.Log("lean D ro");
                yield return null;
            }
            Debug.Log("BOGA BOSS!");
            //var centerSpawnPoint = m_teleportSpawnPointsB[0];
            //transform.position = centerSpawnPoint.position;
            if (m_character.facing == HorizontalDirection.Left)
            {
              // m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);
                m_turnHandle.ExecuteWithAnimationByPass();
                do
                {

                    yield return null;
                }
                while (m_character.facing == HorizontalDirection.Left);

            }
            m_animation.SetAnimation(0, m_info.dragonBreathAnticipation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.dragonBreathAnticipation.animation);
            m_animation.SetAnimation(0, m_info.dragonBreathAttackRight, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.dragonBreathAttackRight.animation);
            m_animation.SetAnimation(0, m_info.dragonBreathAttackLeft, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.dragonBreathAttackLeft.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_fireController.StartDragonsRoutine();
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            Debug.Log("Done Dragons breath");
            yield return null;
        }


        private void DragonsBreathActivator()
        {
            m_fireController.StartDragonBreathRoutine();
        }

        private IEnumerator SummonDragonRoutine()
        {
            var positionForSummonDragon = m_dragonsBreathPoint;
            while (Vector3.Distance(transform.position, positionForSummonDragon.position) > m_distanceStoppingToleranceForRayFrost)
            {
                var CalculatedDistanceOfPositions = (positionForSummonDragon.position - transform.position).normalized;
                transform.position += m_info.move.speed * Time.deltaTime * CalculatedDistanceOfPositions;
                Debug.Log("papuntang langit boss");
                yield return null;
            }

            m_animation.SetAnimation(0, m_info.summonDragonAnticipation.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.summonDragonAnticipation.animation);
            m_animation.SetAnimation(0, m_info.summonDragonAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.summonDragonAttack.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            Debug.Log("Gwa laser done boss ");
            yield return null;
        }

        private IEnumerator DragonsBreathWithSummonDragon()
        {
            var positionForDragonsBreath = m_dragonsBreathPoint;
            // m_fireController.SetActiveDragonTrail(true);
            while (Vector3.Distance(transform.position, positionForDragonsBreath.position) > 0.2f)
            {
                var distanceCalcuOfTwoPosition = (positionForDragonsBreath.position - transform.position).normalized;
                transform.position += m_info.move.speed * Time.deltaTime * distanceCalcuOfTwoPosition;
                Debug.Log("lean D ro");
                yield return null;
            }
            // m_dragonsBreathAnimator.gameObject.SetActive(true);
            // m_fireController.SetActiveDragonTrail(true);
            Debug.Log("BOGA BOSS!");
            //var centerSpawnPoint = m_teleportSpawnPointsB[0];
            //transform.position = centerSpawnPoint.position;
            if (m_character.facing == HorizontalDirection.Left)
            {
                m_turnHandle.ExecuteWithAnimationByPass();
                do
                {

                    yield return null;
                }
                while (m_character.facing == HorizontalDirection.Left);

            }
            m_animation.SetAnimation(0, m_info.dragonBreathAnticipation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.dragonBreathAnticipation.animation);
            m_animation.SetAnimation(0, m_info.dragonBreathAttackRight, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.dragonBreathAttackRight.animation);
            m_animation.SetAnimation(0, m_info.dragonBreathAttackLeft, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.dragonBreathAttackLeft.animation);
           
            m_fireController.StartDragonsRoutine();

            m_animation.SetAnimation(0, m_info.summonDragonAnticipation.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.summonDragonAnticipation.animation);
            m_animation.SetAnimation(0, m_info.summonDragonAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.summonDragonAttack.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        private IEnumerator IceShardRoutine()
        {
            //any position? 
            m_animation.SetAnimation(0, m_info.iceShardAnticipation.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.iceShardAnticipation.animation);
            m_animation.SetAnimation(0, m_info.iceShardAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.iceShardAttack.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            Debug.Log("Gwa shard done boss?");
            yield return null;
        }
        private void IceShardSpawn()
        {
            Debug.Log("Spawns");
            if (m_IceShardPattern1Done == false)
            {
                IceShardSpell(transform.position, 0, 100);
                m_IceShardPattern1Done = true;
            }
            else
            {
                IceShardSpell(transform.position, 45, 100);
                m_IceShardPattern1Done = false;
            }

        }
        private void IceShardSpell(Vector3 position, float rotation, float speed)
        {
            m_demonLordIceShardSpell.LaunchIceShards(position, rotation, speed);
        }


        private bool m_isCastingSpellDone = false;
        private IEnumerator LightningOrbRoutine()
        {
            Debug.Log("HOLY WEEK SPECIAL BY TOTO DRAGONS CALL ALIMAE");
            m_demonLordLightningConstellation.SpellEnd += M_demonLordLightningConstellation_SpellEnd1;
            m_animation.SetAnimation(0, m_info.lightningOrbSummonAnticipation.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.lightningOrbSummonAnticipation.animation);
            m_animation.SetAnimation(0, m_info.lightningOrbSummonLoop, true);
            ActivateLightningOrb();
            while (m_isCastingSpellDone == false)
            {
                yield return null;
            }
            m_demonLordLightningConstellation.SpellEnd -= M_demonLordLightningConstellation_SpellEnd1;
            m_animation.SetAnimation(0, m_info.lightningOrbAttack, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.lightningOrbAttack.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            m_isCastingSpellDone = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();

            //m_animation.SetAnimation(0, m_info.lightningOrbSummonLoop, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.lightningOrbAttack.animation);
            yield return null;
        }

        private void M_demonLordLightningConstellation_SpellEnd1(object sender, EventActionArgs eventArgs)
        {
            m_isCastingSpellDone = true;
        }


        private void ActivateLightningOrb()
        {
            System.Random random = new System.Random();
            int randomValue = random.Next(0, (int)DemonLordLightningConstellation.Type._COUNT);
            DemonLordLightningConstellation.Type randomType = (DemonLordLightningConstellation.Type)randomValue;
            m_demonLordLightningConstellation.ExecuteSpell(randomType);
        }



        [SerializeField]
        private int m_lightningStrikeCounter;
        private IEnumerator LightningStrike()
        {
            var chosenlightingStrikePositioning = GetNearestLightingStrikePositioning(m_lightningStrikePositioningInfos);
            var ChosenPosition = chosenlightingStrikePositioning.castingPosition;
            yield return new WaitForSeconds(0.5f);
            while (Vector3.Distance(transform.position, ChosenPosition) > m_distanceStoppingToleranceForRayFrost)
            {
                var CalculatedDistanceOfPositions = (ChosenPosition - transform.position).normalized;
                transform.position += m_info.move.speed * Time.deltaTime * CalculatedDistanceOfPositions;
                Debug.Log("papuntang langit boss");
                yield return null;
            }
            if (IsFacingTarget() == false)
            {
               // m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);
                m_turnHandle.ExecuteWithAnimationByPass();
            }
         
            Debug.Log(chosenlightingStrikePositioning.ToString());
            //m_demonLordLightningStrike.LightningStrikeSpawnPosition(lightStrikeSpawnPoint, lightningStrikeObject);
            m_animation.SetAnimation(0, m_info.lightningStrikeAnticipation.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.lightningStrikeAnticipation.animation);
            m_demonLordLightningStrike.SetSpawnPosition(chosenlightingStrikePositioning.lightningSpawnPosition);
            LightingstrikeController();
            m_animation.SetAnimation(0, m_info.lightningStrikeAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.lightningStrikeAttack.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
       
            //m_attackDecider.hasDecidedOnAttack = false;
            //m_currentAttackCoroutine = null;
            //m_stateHandle.ApplyQueuedState();
            ////yield return new WaitForSeconds(1f);
            //GameplaySystem.playerManager.player.character.GetComponent<Damageable>().DamageTaken -= HitOnPlayer;         
            yield return null;
        }
     
        private IEnumerator ContinuousUpdate()
        {
            while (m_isPlayerHit == false && m_lightningStrikeCounter <3)
            {

                yield return StartCoroutine(LightningStrike());
                m_lightningStrikeCounter++;
            }
            //GameplaySystem.playerManager.player.character.GetComponent<Damageable>().DamageTaken -= HitOnPlayer;
            m_lightningStrikeCounter = 0;
            m_isPlayerHit = false;
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            //yield return new WaitForSeconds(1f);
            yield return null;
        }
        private LightningStrikePositioningInfo GetNearestLightingStrikePositioning(LightningStrikePositioningInfo[] options)
        {
            int nearestIndex = 0;
            float nearestDistance = Mathf.Abs(m_targetInfo.position.x - options[nearestIndex].lightningSpawnPosition.x);

            for (int i = 0; i < options.Length; i++)
            {
                var distance = Mathf.Abs(m_targetInfo.position.x - options[i].lightningSpawnPosition.x);
                if (distance < nearestDistance)
                {
                    nearestIndex = i;
                    nearestDistance = distance;
                }
            }
            Debug.Log(options);
            return options[nearestIndex];
        }

        private void LightingstrikeController()
        {
            m_demonLordLightningStrike.StrikeLightning();
        }
        #endregion

        //private void DecidedOnAttack(bool condition)
        //{
        //    m_attackDecider.hasDecidedOnAttack = condition;
        //}

        private void UpdateAttackDeciderList()
        {
            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseOne:
                    var dragonsBreath = new AttackInfo<Attack>(Attack.Phase1Pattern1, m_info.phase1Pattern1Range);
                    var iceShard = new AttackInfo<Attack>(Attack.Phase1Pattern2, m_info.phase1Pattern2Range);
                    var lightningStrike = new AttackInfo<Attack>(Attack.Phase1Pattern3, m_info.phase1Pattern3Range);
                    m_attackDecider.SetList(dragonsBreath, iceShard, lightningStrike);
                    break;
                case Phase.PhaseTwo:

                    dragonsBreath = new AttackInfo<Attack>(Attack.Phase1Pattern1, m_info.phase1Pattern1Range);
                    iceShard = new AttackInfo<Attack>(Attack.Phase1Pattern2, m_info.phase1Pattern2Range);
                    lightningStrike = new AttackInfo<Attack>(Attack.Phase1Pattern3, m_info.phase1Pattern3Range);
                    var summonDragonWithDragonsBreath = new AttackInfo<Attack>(Attack.Phase2Pattern1, m_info.phase2Pattern2Range);
                    var summonDragon = new AttackInfo<Attack>(Attack.Phase2Pattern2, m_info.phase2Pattern2Range);
                    var rayOfFrost = new AttackInfo<Attack>(Attack.Phase2Pattern3, m_info.phase2Pattern3Range);
                    var LightningOrb = new AttackInfo<Attack>(Attack.Phase2Pattern4, m_info.phase2Pattern4Range);

                    m_attackDecider.SetList(dragonsBreath, iceShard, lightningStrike, summonDragonWithDragonsBreath, summonDragon, rayOfFrost, LightningOrb);
                    break;
            }
            //m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Phase1Pattern1, m_info.phase1Pattern1Range),
            //                         new AttackInfo<Attack>(Attack.Phase1Pattern2, m_info.phase1Pattern2Range),
            //                         new AttackInfo<Attack>(Attack.Phase1Pattern3, m_info.phase1Pattern3Range),
            //                         new AttackInfo<Attack>(Attack.Phase1Pattern3, m_info.phase1Pattern4Range),
            //                         //new AttackInfo<Attack>(Attack.Phase2Pattern1, m_info.phase2Pattern1Range),
            //                         new AttackInfo<Attack>(Attack.Phase2Pattern2, m_info.phase2Pattern2Range),
            //                         new AttackInfo<Attack>(Attack.Phase2Pattern3, m_info.phase2Pattern3Range),
            //                         new AttackInfo<Attack>(Attack.Phase2Pattern4, m_info.phase2Pattern4Range),
            //                         new AttackInfo<Attack>(Attack.Phase2Pattern5, m_info.phase2Pattern5Range),
            //                         new AttackInfo<Attack>(Attack.Phase3Pattern1, m_info.phase3Pattern1Range),
            //                         new AttackInfo<Attack>(Attack.Phase3Pattern2, m_info.phase3Pattern2Range),
            //                         //new AttackInfo<Attack>(Attack.Phase3Pattern3, m_info.phase3Pattern3Range),
            //                         //new AttackInfo<Attack>(Attack.Phase3Pattern4, m_info.phase3Pattern4Range),
            //                         new AttackInfo<Attack>(Attack.Phase3Pattern5, m_info.phase3Pattern5Range),
            //                         new AttackInfo<Attack>(Attack.Phase3Pattern5, m_info.phase3Pattern6Range));
            m_attackDecider.hasDecidedOnAttack = false;

        }

        public override void ApplyData()
        {
            if (m_attackDecider != null)
            {
                UpdateAttackDeciderList();
            }
            base.ApplyData();
        }

        private void ChooseAttack()
        {
            if (!m_attackDecider.hasDecidedOnAttack)
            {
                m_attackDecider.DecideOnAttack();
            }
        }



        protected override void Awake()
        {
            //m_turnHandle.TurnDone += OnTurnDone;
            ////m_hitDetector.PlayerHit += AddHitCount;
            //m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            //m_flinchHandler.FlinchStart += OnFlinchStart;
            //m_flinchHandler.FlinchEnd += OnFlinchEnd;
            //m_fireBallProjectileLauncher = new ProjectileLauncher(m_info.fireBallProjectile.projectileInfo, m_fireBallSpawnPoint);
            //m_iceBombProjectileLauncher = new ProjectileLauncher(m_info.iceBombProjectile.projectileInfo, m_iceBombSpawnPoint);
            //m_electricOrbProjectileLauncher = new ProjectileLauncher(m_info.electricOrbProjectile.projectileInfo, m_electricOrbSpawnPoint);
            //m_flameWaveProjectileLauncher = new ProjectileLauncher(m_info.flameWaveProjectile.projectileInfo, m_flameWaveSpawnPoint);
            //m_lightningGroundProjectileLauncher = new ProjectileLauncher(m_info.lightningGroundProjectile.projectileInfo, m_lightningGroundSpawnPoint);
            //m_electricOrbs = new List<GameObject>();
            // m_patternAttackCount = new int[2];
            //m_patternDecider = new RandomAttackDecider<Pattern>();
            base.Awake();
            m_attackDecider = new RandomAttackDecider<Attack>();
            m_attackDecider.SetMaxRepeatAttack(1);
            //m_Points = new List<Vector2>();
            //for (int i = 0; i < 3; i++)
            //{
            //    m_attackDecider[i] = new RandomAttackDecider<Attack>();
            //}
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            m_currentFullCooldown = new List<float>();
            m_patternCooldown = new List<float>();

           
        }

        protected override void Start()
        {
            base.Start();

            ////m_spineListener.Subscribe(m_info.OrbSummonRainProjectile.launchOnEvent, m_deathFX.Play);
            m_spineListener.Subscribe(m_info.iceShardCardinalProjectiles, IceShardSpawn);
            m_spineListener.Subscribe(m_info.iceShardDiagonalProjectiles, IceShardSpawn);
            m_spineListener.Subscribe(m_info.rayOfFrostBeam, RayOfFrostBeamController);
            m_spineListener.Subscribe(m_info.dragonsBreath, DragonsBreathActivator);
            //m_spineListener.Subscribe(m_info.lightningStrike, LightingstrikeController);
            //m_animation.DisableRootMotion();

            m_phaseHandle = new PhaseHandle<Phase, PhaseInfo>();
            m_phaseHandle.Initialize(Phase.PhaseOne, m_info.phaseInfo, m_character, ChangeState, ApplyPhaseData);
        
            
            m_phaseHandle.ApplyChange();
            m_aimRoutine = AimRoutine();
            m_listOfSpawnPoints.Add("SpawnPointA", m_teleportSpawnPointsA);
            m_listOfSpawnPoints.Add("SpawnPointB", m_teleportSpawnPointsB);
            m_listOfSpawnPoints.Add("SpawnPointC", m_teleportSpawnPointsC);
            m_startingPoint = true;
         
        }
        [SerializeField]
        public bool m_isPlayerHit = false;
   
        private void HitOnPlayer(object sender, Damageable.DamageEventArgs eventArgs)
        {
            Debug.Log("On me");
            m_isPlayerHit = true;
        }
        private void Update()
        {
            m_phaseHandle.MonitorPhase();
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    m_book.Idle(true);
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    break;
                case State.Intro:
                    //if (IsFacingTarget())
                    //{
                    //    //StartCoroutine(IntroRoutine());
                    //    m_stateHandle.OverrideState(State.Attacking);
                    //}
                    //else
                    //{
                    //    m_turnState = State.Intro;
                    //    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                    //        m_stateHandle.SetState(State.Turning);
                    //}
                    m_stateHandle.OverrideState(State.Attacking);
                    break;
                case State.Phasing:
                    Debug.Log("Phase Time");
                    StartCoroutine(ChangePhaseRoutine());
                    break;
                //case State.Turning:
                //    Debug.Log("Turning Steet");
                //    m_stateHandle.Wait(m_turnState);
                //    StopAllCoroutines();
                //    m_book.Turn(false);
                //    m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);
                //    m_agent.Stop();
                //    m_stateHandle.OverrideState(State.Attacking);
                //    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    m_lastTargetPos = m_targetInfo.position;
                    m_agent.Stop();

                    m_attackDecider.DecideOnAttack();
                    m_currentAttack = m_attackDecider.chosenAttack.attack;
                    switch (m_currentAttack)
                    {

                        #region PHASE 1 ATTACKS
                        case Attack.Phase1Pattern1:
                            //if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) > m_info.target1CHDistance)
                            //{
                            //    m_currentAttackCoroutine = StartCoroutine(ThreeFireBallsAttackRoutine());
                            //}
                            //else
                            //{
                            //    m_currentAttackCoroutine = StartCoroutine(EphemeralArmsSmashAttackRoutine(FollowUpAttack.ThreeFireBalls));
                            //}
                            // m_currentAttackCoroutine = StartCoroutine(EphemeralArmsSmashAttackRoutine(FollowUpAttack.ElectricOrb));
                            //m_currentAttackCoroutine = StartCoroutine(DragonsBreathRoutine());

                            m_currentAttackCoroutine = StartCoroutine(DragonsBreathRoutine());
                            m_pickedCooldown = m_currentFullCooldown[0];
                            break;
                        case Attack.Phase1Pattern2:
                            //if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) > m_info.target1CHDistance)
                            //{
                            //    m_currentAttackCoroutine = StartCoroutine(IceBombAttackRoutine());
                            //}
                            //else
                            //{
                            //    m_currentAttackCoroutine = StartCoroutine(EphemeralArmsSmashAttackRoutine(FollowUpAttack.IceBomb));
                            //}
                            m_currentAttackCoroutine = StartCoroutine(IceShardRoutine());
                            m_pickedCooldown = m_currentFullCooldown[1];
                            break;
                        case Attack.Phase1Pattern3:
                            //if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) > m_info.target1CHDistance)
                            //{
                            //    m_currentAttackCoroutine = StartCoroutine(LightningStrike());
                            //}
                            //else
                            //{
                            //    m_currentAttackCoroutine = StartCoroutine(LightningStrike());
                            //}
                            
                            m_currentAttackCoroutine = StartCoroutine(ContinuousUpdate());
                            m_pickedCooldown = m_currentFullCooldown[2];
                            break;
                        //case Attack.Phase1Pattern4:
                        //    if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) > m_info.target1CHDistance)
                        //    {
                        //        m_currentAttackCoroutine = StartCoroutine(LightningOrbRoutine());
                        //    }
                        //    else
                        //    {
                        //        m_currentAttackCoroutine = StartCoroutine(IceShardRoutine());
                        //    }
                        //    m_pickedCooldown = m_currentFullCooldown[3];
                        //    break;
                        #endregion
                        #region PHASE 2 ATTACKS
                        case Attack.Phase2Pattern1:
                            //m_currentAttackCoroutine = StartCoroutine(TestingAttackRoutine());
                            if (m_phaseHandle.currentPhase == Phase.PhaseTwo)
                            {
                                StartCoroutine(DragonsBreathWithSummonDragon());
                                m_pickedCooldown = m_currentFullCooldown[0];
                            }
                            else
                            {
                                m_attackDecider.hasDecidedOnAttack = false;
                                m_stateHandle.ApplyQueuedState();
                            }
                            break;
                        case Attack.Phase2Pattern2:
                            //if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) > m_info.target1CHDistance)
                            //{
                            //    m_currentAttackCoroutine = StartCoroutine(IceBombAttackRoutine());
                            //}
                            //else
                            //{
                            //    m_currentAttackCoroutine = StartCoroutine(EphemeralArmsSmashAttackRoutine(FollowUpAttack.IceBomb));
                            //}

                            if (m_phaseHandle.currentPhase == Phase.PhaseTwo)
                            {
                                m_currentAttackCoroutine = StartCoroutine(RayOfFrostRoutine());
                                m_pickedCooldown = m_currentFullCooldown[1];
                            }
                            else
                            {
                                m_attackDecider.hasDecidedOnAttack = false;
                                m_stateHandle.ApplyQueuedState();
                            }
                            break;
                        case Attack.Phase2Pattern3:
                            //if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) > m_info.target1CHDistance)
                            //{
                            //    m_currentAttackCoroutine = StartCoroutine(SummonDragonRoutine());
                            //}
                            //else
                            //{
                            //    m_currentAttackCoroutine = StartCoroutine(EphemeralArmsSmashAttackRoutine(FollowUpAttack.ElectricOrb));
                            //}
                            if (m_phaseHandle.currentPhase == Phase.PhaseTwo)
                            {
                                m_currentAttackCoroutine = StartCoroutine(SummonDragonRoutine());
                                m_pickedCooldown = m_currentFullCooldown[2];
                            }
                            else
                            {
                                m_attackDecider.hasDecidedOnAttack = false;
                                m_stateHandle.ApplyQueuedState();
                            }

                            break;
                        case Attack.Phase2Pattern4:
                            //if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) > m_info.target1CHDistance)
                            //{
                            //    m_currentAttackCoroutine = StartCoroutine(FlameWaveAttackRoutine());
                            //}
                            //else
                            //{
                            //    m_currentAttackCoroutine = StartCoroutine(EphemeralArmsSmashAttackRoutine(FollowUpAttack.FlameWave));
                            //}
                            if (m_phaseHandle.currentPhase == Phase.PhaseTwo)
                            {
                                m_currentAttackCoroutine = StartCoroutine(LightningOrbRoutine());
                                m_pickedCooldown = m_currentFullCooldown[3];
                            }
                            else
                            {
                                m_attackDecider.hasDecidedOnAttack = false;
                                m_stateHandle.ApplyQueuedState();
                            }
                            break;
                            //case Attack.Phase2Pattern5:
                            //    m_currentAttackCoroutine = StartCoroutine(LightningOrbRoutine());// change  
                            //    m_pickedCooldown = m_currentFullCooldown[3];
                            //    break;
                            #endregion
                            #region PHASE 3 ATTACKS
                            //case Attack.Phase3Pattern1:
                            //    //if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) > m_info.target1CHDistance)
                            //    //{
                            //    //    m_currentAttackCoroutine = StartCoroutine(ThreeFireBallsAttackRoutine());
                            //    //}
                            //    //else
                            //    //{
                            //    //    m_currentAttackCoroutine = StartCoroutine(EphemeralArmsSmashAttackRoutine(FollowUpAttack.ThreeFireBalls));
                            //    //}
                            //    m_currentAttackCoroutine = StartCoroutine(EphemeralArmsSmashAttackRoutine(FollowUpAttack.FlameWave));
                            //    m_pickedCooldown = m_currentFullCooldown[0];
                            //    break;
                            //case Attack.Phase3Pattern2:
                            //    //if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) > m_info.target1CHDistance)
                            //    //{
                            //    //    m_currentAttackCoroutine = StartCoroutine(IceBombAttackRoutine());
                            //    //}
                            //    //else
                            //    //{
                            //    //    m_currentAttackCoroutine = StartCoroutine(EphemeralArmsSmashAttackRoutine(FollowUpAttack.IceBomb));
                            //    //}
                            //    m_currentAttackCoroutine = StartCoroutine(EphemeralArmsSmashAttackRoutine(FollowUpAttack.LightningGround));
                            //    m_pickedCooldown = m_currentFullCooldown[1];
                            //    break;
                            ////case Attack.Phase3Pattern3:
                            ////    //m_currentAttackCoroutine = StartCoroutine(TestingAttackRoutine());
                            ////    m_pickedCooldown = m_currentFullCooldown[2];
                            ////    break;
                            ////case Attack.Phase3Pattern4:
                            ////    //m_currentAttackCoroutine = StartCoroutine(TestingAttackRoutine());
                            ////    m_pickedCooldown = m_currentFullCooldown[3];
                            ////    break;
                            //case Attack.Phase3Pattern5:
                            //    m_currentAttackCoroutine = StartCoroutine(LightningGroundAttackRoutine());// change 
                            //    m_pickedCooldown = m_currentFullCooldown[4];
                            //    break;
                            //case Attack.Phase3Pattern6:
                            //    m_currentAttackCoroutine = StartCoroutine(RayOfFrostRoutine());
                            //    m_pickedCooldown = m_currentFullCooldown[5];
                            //    break;
                            #endregion
                    }
                    break;

                case State.Cooldown:
                    if (!IsFacingTarget())
                    {
                        m_agent.Stop();
                        m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);
                    }
                    else
                    {
                        m_book.Idle(true);
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        //  m_stateHandle.SetState(State.Attacking);
                    }

                    if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) <= m_info.target3CHDistance)
                    {
                        //StartCoroutine(LightningStepRoutine());// remove 
                        StartCoroutine(TeleportRoutine());
                        //return;
                        Debug.Log("STIP BAK");
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

                    break;
                //case State.Chasing:
                //    ChooseAttack();
                //    Debug.Log("Chasing Choose Attack");
                //    if (IsFacingTarget())
                //    {
                //        if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_currentAttackRange) && Mathf.Abs(GroundPosition(transform.position).y - m_character.centerMass.position.y) <= m_info.targetGroundDistanceTolerance)
                //        {
                //            m_stateHandle.SetState(State.Attacking);
                //        }
                //        else
                //        {
                //            m_animation.SetAnimation(0, m_info.move.animation, true);
                //            if (Mathf.Abs(GroundPosition(transform.position).y - m_character.centerMass.position.y) <= m_info.targetGroundDistanceTolerance)
                //            {
                //                m_agent.SetDestination(m_targetInfo.position);
                //            }
                //            else
                //            {
                //                m_agent.SetDestination(GroundPosition(transform.position));
                //            }
                //            m_agent.Move(m_info.move.speed);
                //        }
                //    }
                //    else
                //    {
                //        m_turnState = State.Chasing;
                //        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation /*&& m_animation.GetCurrentAnimation(0).ToString() != m_info.attackDaggersIdle.animation*/)
                //            m_stateHandle.SetState(State.Turning);
                //    }
                //    break;

                case State.ReevaluateSituation:
                    if (m_targetInfo.isValid)
                    {
                        m_stateHandle.SetState(State.Attacking);
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
            //m_stickToGround = false;
            //m_currentCD = 0;
        }

        public override void ReturnToSpawnPoint()
        {
        }

        protected override void OnForbidFromAttackTarget()
        {
        }
    }
}