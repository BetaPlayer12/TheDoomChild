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
using DChild.Temp;
using Spine.Unity.Modules;
using Spine.Unity.Examples;
using DChild.Gameplay.Pooling;
using UnityEngine.Playables;
using DChild.Gameplay.Projectiles;
using Sirenix.Serialization;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Boss/CinderBolt")]
    public class CinderBoltAI : CombatAIBrain<CinderBoltAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField]
            private PhaseInfo<Phase> m_phaseInfo;
            public PhaseInfo<Phase> phaseInfo => m_phaseInfo;

            [SerializeField, BoxGroup("Movement")]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;
            [SerializeField, BoxGroup("Movement")]
            private MovementInfo m_overchargedMove = new MovementInfo();
            public MovementInfo overchargedMove => m_overchargedMove;

            [SerializeField, BoxGroup("Movement"), ValueDropdown("GetAnimations")]
            private string m_moveTurnAnimation;
            public string moveTurnAnimation => m_moveTurnAnimation;

            [SerializeField, Title("Hover"), BoxGroup("Movement")]
            private MovementInfo m_hoverUpward = new MovementInfo();
            public MovementInfo hoverUpward => m_hoverUpward;
            [SerializeField, Title("Hover"), BoxGroup("Movement")]
            private MovementInfo m_overchargedHoverUpward = new MovementInfo();
            public MovementInfo overchargedHoverUpward => m_overchargedHoverUpward;
            [SerializeField, Title("Hover"), BoxGroup("Movement")]
            private MovementInfo m_hoverBackward = new MovementInfo();
            public MovementInfo hoverBackward => m_hoverBackward;
            [SerializeField, Title("Hover"), BoxGroup("Movement")]
            private MovementInfo m_overchargedHoverBackward = new MovementInfo();
            public MovementInfo overchargedHoverBackward => m_overchargedHoverBackward;
            [SerializeField, Title("Hover"), BoxGroup("Movement")]
            private MovementInfo m_hoverDownward = new MovementInfo();
            public MovementInfo hoverDownward => m_hoverDownward;
            [SerializeField, Title("Hover"), BoxGroup("Movement")]
            private MovementInfo m_overchargedHoverDownward = new MovementInfo();
            public MovementInfo overchargedHoverDownward => m_overchargedHoverDownward;
            [SerializeField, Title("Hover"), BoxGroup("Movement")]
            private MovementInfo m_hoverForward = new MovementInfo();
            public MovementInfo hoverForward => m_hoverForward;
            [SerializeField, Title("Hover"), BoxGroup("Movement")]
            private MovementInfo m_overchargedHoverForward = new MovementInfo();
            public MovementInfo overchargedHoverForward => m_overchargedHoverForward;
            [SerializeField, Title("Long Dash"), BoxGroup("Movement")]
            private MovementInfo m_longDash = new MovementInfo();
            public MovementInfo longDash => m_longDash;
            [SerializeField, Title("Long Dash"), BoxGroup("Movement")]
            private MovementInfo m_overchargedLongDash = new MovementInfo();
            public MovementInfo overchargedLongDash => m_overchargedLongDash;
            [SerializeField, Title("Long Dash"), BoxGroup("Movement"), ValueDropdown("GetAnimations")]
            private string m_longDashBoosterChargeAnimation;
            public string longDashBoosterChargeAnimation => m_longDashBoosterChargeAnimation;
            [SerializeField, Title("Long Dash"), BoxGroup("Movement"), ValueDropdown("GetAnimations")]
            private string m_overchargedLongDashBoosterChargeAnimation;
            public string overchargedLongDashBoosterChargeAnimation => m_overchargedLongDashBoosterChargeAnimation;
            [SerializeField, Title("Long Dash"), BoxGroup("Movement"), ValueDropdown("GetAnimations")]
            private string m_longDashStopAnimation;
            public string longDashStopAnimation => m_longDashStopAnimation;
            /*[SerializeField, Title("Overcharged Long Dash"), BoxGroup("Movement"), ValueDropdown("GetAnimations")]
            private string m_overchargedLongDashStopAnimation;
            public string overchargedLongDashStopAnimation => m_overchargedLongDashStopAnimation;*/
            [SerializeField, Title("Short Dash"), BoxGroup("Movement")]
            private MovementInfo m_shortDash = new MovementInfo();
            public MovementInfo shortDash => m_shortDash;
            [SerializeField, Title("Short Dash"), BoxGroup("Movement")]
            private MovementInfo m_overchargedShortDash = new MovementInfo();
            public MovementInfo overchargedShortDash => m_overchargedShortDash;

            [SerializeField, Title("Straight Left and Uppercut"), BoxGroup("Attack")]
            private SimpleAttackInfo m_straightLeftAndUppercutAttack = new SimpleAttackInfo();
            public SimpleAttackInfo straightLeftAndUppercutAttack => m_straightLeftAndUppercutAttack;
            [SerializeField, Title("Straight Left and Uppercut"), BoxGroup("Attack")]
            private SimpleAttackInfo m_overchargedStraightLeftAndUppercutAttack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedStraightLeftAndUppercutAttack => m_overchargedStraightLeftAndUppercutAttack;
            [SerializeField, Title("Flame Thrower"), BoxGroup("Attack")]
            private SimpleAttackInfo m_flameThrowerAttack = new SimpleAttackInfo();
            public SimpleAttackInfo flameThrowerAttack => m_flameThrowerAttack;
            [SerializeField, Title("Flame Thrower"), BoxGroup("Attack")]
            private SimpleAttackInfo m_overchargedFlameThrowerAttack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedFlameThrowerAttack => m_overchargedFlameThrowerAttack;
            [SerializeField, Title("Flame Beam"), BoxGroup("Attack")]
            private SimpleAttackInfo m_flameBeamAttack = new SimpleAttackInfo();
            public SimpleAttackInfo flameBeamAttack => m_flameBeamAttack;
            [SerializeField, Title("Flame Beam"), BoxGroup("Attack")]
            private SimpleAttackInfo m_overchargedFlameBeamAttack = new SimpleAttackInfo();
            [SerializeField, Title("Long Dash"), BoxGroup("Attack")]
            private SimpleAttackInfo m_longDashAttack = new SimpleAttackInfo();
            public SimpleAttackInfo longDashAttack => m_longDashAttack;
            [SerializeField, Title("Long Dash"), BoxGroup("Attack")]
            private SimpleAttackInfo m_overchargedLongDashAttack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedLongDashAttack => m_overchargedLongDashAttack;
            public SimpleAttackInfo overchargedFlameBeamAttack => m_overchargedFlameBeamAttack;
            [SerializeField, Title("Punch"), BoxGroup("Attack")]
            private SimpleAttackInfo m_punchAttack = new SimpleAttackInfo();
            public SimpleAttackInfo punchAttack => m_punchAttack;
            [SerializeField, Title("Punch"), BoxGroup("Attack")]
            private SimpleAttackInfo m_overchargedPunchAttack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedPunchAttack => m_overchargedPunchAttack;
            [SerializeField, Title("Shotgun Blast"), BoxGroup("Attack")]
            private SimpleAttackInfo m_shotgunBlastFireAttack = new SimpleAttackInfo();
            public SimpleAttackInfo shotgunBlastFireAttack => m_shotgunBlastFireAttack;
            [SerializeField, Title("Shotgun Blast"), BoxGroup("Attack")]
            private SimpleAttackInfo m_overchargedShotgunBlastFireAttack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedShotgunBlastFireAttack => m_overchargedShotgunBlastFireAttack;
            [SerializeField, Title("Shotgun Blast"), BoxGroup("Attack"), ValueDropdown("GetAnimations")]
            private string m_shotgunBlastBackToIdleAnimation;
            public string shotgunBlastBackToIdleAnimation => m_shotgunBlastBackToIdleAnimation;
            [SerializeField, Title("Shotgun Blast"), BoxGroup("Attack"), ValueDropdown("GetAnimations")]
            private string m_shotgunBlastPreAnimation;
            public string shotgunBlastPreAnimation => m_shotgunBlastPreAnimation;
            [SerializeField, Title("Shotgun Blast"), BoxGroup("Attack")]
            private SimpleAttackInfo m_shotgunBlastRapidFireAttack = new SimpleAttackInfo();
            public SimpleAttackInfo shotgunBlastRapidFireAttack => m_shotgunBlastRapidFireAttack;
            [SerializeField, Title("Shotgun Blast"), BoxGroup("Attack")]
            private SimpleAttackInfo m_overchargedShotgunBlastRapidFireAttack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedShotgunBlastRapidFireAttack => m_overchargedShotgunBlastRapidFireAttack;
            [SerializeField, Title("Uppercut"), BoxGroup("Attack")]
            private SimpleAttackInfo m_uppercutAttack = new SimpleAttackInfo();
            public SimpleAttackInfo uppercutAttack => m_uppercutAttack;
            [SerializeField, Title("Uppercut"), BoxGroup("Attack")]
            private SimpleAttackInfo m_overchargedUppercutAttack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedUppercutAttack => m_overchargedUppercutAttack;
            [SerializeField, Title("Firebeam"), BoxGroup("Attack")]
            private SimpleAttackInfo m_firebeamAttack = new SimpleAttackInfo();
            public SimpleAttackInfo firebeamAttack => m_firebeamAttack;
            [SerializeField, Title("Meteor Smash"), BoxGroup("Attack")]
            private SimpleAttackInfo m_meteorAttack = new SimpleAttackInfo();
            public SimpleAttackInfo meteorAttack => m_meteorAttack;
            [SerializeField, Title("Spin Attack"), BoxGroup("Attack")]
            private SimpleAttackInfo m_spinAttack = new SimpleAttackInfo();
            public SimpleAttackInfo spinAttack => m_spinAttack;
            [SerializeField, Title("Short Dash"), BoxGroup("Attack")]
            private SimpleAttackInfo m_shortDashAttack = new SimpleAttackInfo();
            public SimpleAttackInfo shortDashAttack => m_shortDashAttack;

            [SerializeField, Title("Overcharged Punch Uppercut"), BoxGroup("Overcharged Attack")]
            private SimpleAttackInfo m_overchargedPunchUppercutAttack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedPunchUppercutAttack => m_overchargedPunchUppercutAttack;
            [SerializeField, Title("Overcharged Flamethrower1"), BoxGroup("Overcharged Attack")]
            private SimpleAttackInfo m_overchargedFlamethrower1Attack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedFlamethrower1Attack => m_overchargedFlamethrower1Attack;
            [SerializeField, Title("Overcharged Pre-Spin"), BoxGroup("Overcharged Attack")]
            private SimpleAttackInfo m_overchargedPreSpinAttack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedPreSpinAttack => m_overchargedPreSpinAttack;
            [SerializeField, Title("Overcharged Spin"), BoxGroup("Overcharged Attack")]
            private SimpleAttackInfo m_overchargedSpinAttack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedSpinAttack => m_overchargedSpinAttack;
            [SerializeField, Title("Overcharged Post-Spin"), BoxGroup("Overcharged Attack")]
            private SimpleAttackInfo m_overchargedPostSpinAttack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedPostSpinAttack => m_overchargedPostSpinAttack;
            [SerializeField, Title("Overcharged Firebeam"), BoxGroup("Overcharged Attack")]
            private SimpleAttackInfo m_overchargedFirebeamAttack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedFirebeamAttack => m_overchargedFirebeamAttack;
            [SerializeField, Title("Shotgun Blast"), BoxGroup("Overcharged Attack")]
            private SimpleAttackInfo m_overchargedShotgunBlastPreAnimation = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedShotgunBlastPreAnimation => m_overchargedShotgunBlastPreAnimation;
            [SerializeField, Title("Shotgun Blast"), BoxGroup("Overcharged Attack")]
            private SimpleAttackInfo m_overchargedShotgunBlastBackToIdleAnimation = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedShotgunBlastBackToIdleAnimation => m_overchargedShotgunBlastBackToIdleAnimation;


            [SerializeField, TabGroup("Phase 1"), BoxGroup("Pattern Ranges")]
            private float m_phase1Pattern1Range;
            public float phase1Pattern1Range => m_phase1Pattern1Range;
            [SerializeField, TabGroup("Phase 1"), BoxGroup("Pattern Ranges")]
            private float m_phase1Pattern2Range;
            public float phase1Pattern2Range => m_phase1Pattern2Range;
            [SerializeField, TabGroup("Phase 2"), BoxGroup("Pattern Ranges")]
            private float m_phase2Pattern1Range;
            public float phase2Pattern1Range => m_phase2Pattern1Range;
            [SerializeField, TabGroup("Phase 2"), BoxGroup("Pattern Ranges")]
            private float m_phase2Pattern2Range;
            public float phase2Pattern2Range => m_phase2Pattern2Range;
            [SerializeField, TabGroup("Phase 2"), BoxGroup("Pattern Ranges")]
            private float m_phase2Pattern3Range;
            public float phase2Pattern3Range => m_phase2Pattern3Range;
            [SerializeField, TabGroup("Phase 2"), BoxGroup("Pattern Ranges")]
            private float m_phase2Pattern4Range;
            public float phase2Pattern4Range => m_phase2Pattern4Range;


            [Title("Misc")]
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

            [SerializeField]
            private SimpleProjectileAttackInfo m_bulletProjectile;
            public SimpleProjectileAttackInfo bulletProjectile => m_bulletProjectile;
            [SerializeField]
            private SimpleProjectileAttackInfo m_overchargedBulletProjectile;
            public SimpleProjectileAttackInfo overchargedBulletProjectile => m_overchargedBulletProjectile;

            [Title("Animations")]
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_punchUppercut;
            public string punchUppercut => m_punchUppercut;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flamethower1;
            public string flamethrower1 => m_flamethower1;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_overchargedFlinchAnimation;
            public string overchargedFlinchAnimation => m_overchargedFlinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_hydraulicsAnimation;
            public string hydraulicsAnimation => m_hydraulicsAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_malfunctionStateAnimation;
            public string malfunctionStateAnimation => m_malfunctionStateAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_malfunctionStateIdleAnimation;
            public string malfunctionStateIdleAnimation => m_malfunctionStateIdleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_malfunctionRecoveryStateAnimation;
            public string malfunctionRecoveryStateAnimation => m_malfunctionRecoveryStateAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_spinAnimation;
            public string spinAnimation => m_spinAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_spinDropAnimation;
            public string spinDropAnimation => m_spinDropAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_spinPreAnimation;
            public string spinPreAnimation => m_spinPreAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_spinEndAnimation;
            public string spinEndAnimation => m_spinEndAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_overchargedSpinAnimation;
            public string overchargedSpinAnimation => m_overchargedSpinAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_overchargedSpinDropAnimation;
            public string overchargedSpinDropAnimation => m_overchargedSpinDropAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_overchargedSpinPreAnimation;
            public string overchargedSpinPreAnimation => m_overchargedSpinPreAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_overchargedSpinEndAnimation;
            public string overchargedSpinEndAnimation => m_overchargedSpinEndAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;
            /*[SerializeField, ValueDropdown("GetAnimations")]
            private string m_overchargedTurn;
            public string overchargedTurn => m_overchargedTurn;*/
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_overchargedIdle;
            public string overchargedIdle => m_overchargedIdle;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_punchUppercutEvent;
            public string punchUppercutEvent => m_punchUppercutEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_flamethrower1Event;
            public string flamethrower1Event => m_flamethrower1Event;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_overchargedPunchUppercutEvent;
            public string overchargedPunchUppercutEvent => m_overchargedPunchUppercutEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_overchargedFlamethrower1Event;
            public string overchargedFlamethrower1Event => m_overchargedFlamethrower1Event;
            [SerializeField]
            private CinderBoltHeatHandle.Config m_heatHandleConfiguration;
            public CinderBoltHeatHandle.Config heatHandleConfiguration => m_heatHandleConfiguration;
            /*
                        [Title("Damagetypes")]
                        [SerializeField]
                        private Dictionary<DamageType, int> m_damageType;
                        public Dictionary<DamageType, int> damageType => m_damageType;*/

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_move.SetData(m_skeletonDataAsset);
                m_hoverUpward.SetData(m_skeletonDataAsset);
                m_overchargedHoverUpward.SetData(m_skeletonDataAsset);
                m_hoverBackward.SetData(m_skeletonDataAsset);
                m_overchargedHoverBackward.SetData(m_skeletonDataAsset);
                m_hoverDownward.SetData(m_skeletonDataAsset);
                m_overchargedHoverDownward.SetData(m_skeletonDataAsset);
                m_hoverForward.SetData(m_skeletonDataAsset);
                m_overchargedHoverForward.SetData(m_skeletonDataAsset);
                m_longDash.SetData(m_skeletonDataAsset);
                m_overchargedLongDash.SetData(m_skeletonDataAsset);
                m_shortDash.SetData(m_skeletonDataAsset);
                m_straightLeftAndUppercutAttack.SetData(m_skeletonDataAsset);
                m_overchargedStraightLeftAndUppercutAttack.SetData(m_skeletonDataAsset);
                m_flameThrowerAttack.SetData(m_skeletonDataAsset);
                m_overchargedFlameThrowerAttack.SetData(m_skeletonDataAsset);
                m_flameBeamAttack.SetData(m_skeletonDataAsset);
                m_overchargedFlameBeamAttack.SetData(m_skeletonDataAsset);
                m_punchAttack.SetData(m_skeletonDataAsset);
                m_overchargedPunchAttack.SetData(m_skeletonDataAsset);
                m_shotgunBlastFireAttack.SetData(m_skeletonDataAsset);
                m_overchargedShotgunBlastFireAttack.SetData(m_skeletonDataAsset);
                m_shotgunBlastRapidFireAttack.SetData(m_skeletonDataAsset);
                m_overchargedShotgunBlastRapidFireAttack.SetData(m_skeletonDataAsset);
                m_uppercutAttack.SetData(m_skeletonDataAsset);
                m_overchargedUppercutAttack.SetData(m_skeletonDataAsset);
                m_firebeamAttack.SetData(m_skeletonDataAsset);
                m_meteorAttack.SetData(m_skeletonDataAsset);
                m_spinAttack.SetData(m_skeletonDataAsset);
                m_longDashAttack.SetData(m_skeletonDataAsset);
                m_overchargedLongDashAttack.SetData(m_skeletonDataAsset);
                m_shortDashAttack.SetData(m_skeletonDataAsset);
                m_overchargedShortDash.SetData(m_skeletonDataAsset);
                m_bulletProjectile.SetData(m_skeletonDataAsset);
                m_overchargedPunchUppercutAttack.SetData(m_skeletonDataAsset);
                m_overchargedFlamethrower1Attack.SetData(m_skeletonDataAsset);
                m_overchargedPreSpinAttack.SetData(m_skeletonDataAsset);
                m_overchargedSpinAttack.SetData(m_skeletonDataAsset);
                m_overchargedPostSpinAttack.SetData(m_skeletonDataAsset);
                m_overchargedFirebeamAttack.SetData(m_skeletonDataAsset);
                m_overchargedShotgunBlastPreAnimation.SetData(m_skeletonDataAsset);
                m_overchargedShotgunBlastFireAttack.SetData(m_skeletonDataAsset);
                m_overchargedShotgunBlastBackToIdleAnimation.SetData(m_skeletonDataAsset);
                m_overchargedBulletProjectile.SetData(m_skeletonDataAsset);
                m_overchargedMove.SetData(m_skeletonDataAsset);
#endif
            }
        }

        [System.Serializable]
        public class PhaseInfo : IPhaseInfo
        {
            [SerializeField]
            private int m_attackCount;
            public int attackCount => m_attackCount;
            [SerializeField]
            private List<int> m_patternCount;
            public List<int> patternCount => m_patternCount;
            [SerializeField]
            private int m_phaseIndex;
            public int phaseIndex => m_phaseIndex;
        }


        private enum State
        {
            Phasing,
            Intro,
            Idle,
            Malfunction,
            Turning,
            Attacking,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Pattern
        {
            Phase1Pattern1,
            Phase1Pattern2,
            Phase2Pattern1,
            Phase2Pattern2,
            Phase2Pattern3,
            Phase2Pattern4,
            WaitAttackEnd,
        }
        private enum Attack
        {
            PunchUppercut,
            LongDash,
            ShotgunBlast,
            SpinAttack,
            MeteorSmash,
            Flamethrower1,
            Flamethrower2,
            Firebeam,
            ShortDash,
            WaitAttackEnd,
        }

        public enum Phase
        {
            PhaseOne,
            PhaseTwo,
            Wait,
        }

        private bool[] m_attackUsed;
        private List<Attack> m_attackCache;
        private List<float> m_attackRangeCache;

        [SerializeField, TabGroup("Reference")]
        private Boss m_boss;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Modules")]
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private Health m_health;/*
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;*/
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;

        [SerializeField, TabGroup("FX")]
        private ParticleFX m_flamethrower1FX;
        /*[SerializeField, TabGroup("FX")]
        private ParticleFX m_firebeamFX;*/
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_firebeamAnticipationFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_spinAttackFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_longDashFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_shortDashFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_flamethrower2FX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_meteorSmashFX;
        [SerializeField, TabGroup("FX")]
        private GameObject m_runeShieldFX;
        [SerializeField, TabGroup("FX")]
        private GameObject m_runeShieldBreakFX;
        [SerializeField, TabGroup("FX")]
        private GameObject m_meteorSmashTrailFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_steamMalfAndOver;
        [SerializeField, TabGroup("FX")]
        private GameObject m_steamThrustFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_boosterChargeFX;
        [SerializeField, TabGroup("FX")]
        private GameObject RecoveryFX;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_projectilePoints;
        [SerializeField, TabGroup("Spawn Points")]
        private GameObject m_flamethrower1SpawnPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private GameObject m_flamethrower2SpawnPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private GameObject m_rightHandSpawnPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private GameObject m_leftHandSpawnPoint;

        [SerializeField, TabGroup("Spawn Points")]
        private List<Transform> m_firebeamTransformPoints;

        [SerializeField]
        private SpineEventListener m_spineListener;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        State m_turnState;
        [ShowInInspector]
        private PhaseHandle<Phase, PhaseInfo> m_phaseHandle;
        private bool m_phase1Done;
        private bool m_phase2Done;
        [ShowInInspector]
        private RandomAttackDecider<Pattern> m_patternDecider;
        private Pattern m_chosenPattern;
        private Pattern m_previousPattern;
        [ShowInInspector]
        private RandomAttackDecider<Attack>[] m_attackDecider;
        private Attack m_currentAttack;
        [ShowInInspector]
        private float m_currentAttackRange;
        private ProjectileLauncher m_projectileLauncher;
        private ProjectileLauncher m_overchargeProjectileLauncher;

        private int m_currentPhaseIndex;
        private float m_attackCount;
        private float[] m_patternCount;
        private int m_hitCount;
        private bool m_hasPhaseChanged;
        private Coroutine m_currentAttackCoroutine = null;
        [SerializeField, TabGroup("Attack Colliders")]
        private Collider2D m_punchAttackCollider, m_overchargedPunchAttackCollider;
        [SerializeField, TabGroup("Attack Colliders")]
        private Collider2D m_flamethrower1Collider, m_overchargedFlamethrower1Collider;
        [SerializeField, TabGroup("Attack Colliders")]
        private Collider2D m_firebeamCollider, m_overchargedFirebeamCollider;
        [SerializeField, TabGroup("Attack Colliders")]
        private Collider2D m_longDashCollider, m_overchargedLongDashCollider;
        [SerializeField, TabGroup("Attack Colliders")]
        private Collider2D m_flamethrower2Colliders, m_overchargedFlamethrower2Colliders;
        [SerializeField, TabGroup("Attack Colliders")]
        private Collider2D m_meteorSmashCollider, m_overchargedMeteorSmashCollider;
        [SerializeField, TabGroup("Attack Colliders")]
        private List<Collider2D> m_spinAttackCollider, m_overchargedSpinAttackCollider;
        [SerializeField, TabGroup("Attack Colliders")]
        private List<Collider2D> m_recoveryDamageCollider;

        private Coroutine m_changeLocationRoutine;
        private bool m_isDetecting;
        /*private float leftPos;
        private float rightPos;
        private float counter;*/

        [SerializeField, TabGroup("Laser")]
        private LineRenderer m_lineRenderer; //renderers
        [SerializeField, TabGroup("Laser")]
        private LineRenderer m_telegraphLineRenderer; //renderers
        [SerializeField, TabGroup("Laser")]
        private EdgeCollider2D m_edgeCollider;
        [SerializeField, TabGroup("Laser")]
        private EdgeCollider2D m_overchargeEdgeCollider; //damage
        [SerializeField, TabGroup("Laser")]
        private GameObject m_muzzleFXGO; //to be deleted
        [SerializeField, TabGroup("Laser")]
        private Transform m_laserOrigin;  //origin
        [SerializeField, TabGroup("Laser")]
        private ParticleFX m_muzzleLoopFX; //impact
        [SerializeField, TabGroup("Laser")]
        private ParticleFX m_laserOriginMuzzleFX; //origin muzzle
        [SerializeField, TabGroup("Laser")]
        private ParticleFX m_muzzleTelegraphFX; //telegraphline
        [SerializeField, TabGroup("Laser")]
        private float m_laserDuration; // laser duration

        private Vector2 m_laserTargetPos;
        [SerializeField]
        private List<Vector2> m_Points;
        private IEnumerator m_aimRoutine;
        [SerializeField]
        private bool m_beamOn;
        private bool m_aimOn;

        private Coroutine m_laserBeamCoroutine;
        private Coroutine m_laserLookCoroutine;

        private SimpleAttackProjectile m_projectile;
        [SerializeField]
        private bool m_isRaging = false;
        private float m_runeDuration;
        private bool m_hasMalfactioned = false;
        [SerializeField]
        private bool m_hasRune = false;
        [SerializeField]
        private CinderBoltHeatHandle m_heatHandler;

        /*[SerializeField]
        private Dictionary<DamageType, int> m_heatHandle;
        public Dictionary<DamageType, int> heatHandle => m_heatHandle;*/

        private void ApplyPhaseData(PhaseInfo obj)
        {
            /*m_attackCache.Clear();
            m_attackRangeCache.Clear();*/
            GetComponent<Damageable>().DamageTaken += CinderBoltAI_DamageTaken;
            m_currentPhaseIndex = obj.phaseIndex;
            for (int i = 0; i < m_patternCount.Length; i++)
            {
                m_patternCount[i] = obj.patternCount[i];
            }
            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseOne:
                    m_patternDecider.SetList(new AttackInfo<Pattern>(Pattern.Phase1Pattern1, m_info.targetDistanceTolerance),
                                             new AttackInfo<Pattern>(Pattern.Phase1Pattern2, m_info.targetDistanceTolerance)/*,
                                     new AttackInfo<Pattern>(Pattern.Phase2Pattern1, m_info.targetDistanceTolerance),
                                     new AttackInfo<Pattern>(Pattern.Phase2Pattern2, m_info.targetDistanceTolerance),
                                     new AttackInfo<Pattern>(Pattern.Phase2Pattern3, m_info.targetDistanceTolerance),
                                     new AttackInfo<Pattern>(Pattern.Phase2Pattern4, m_info.targetDistanceTolerance)*/);
                    m_attackDecider[1].SetList(new AttackInfo<Attack>(Attack.PunchUppercut, m_info.punchAttack.range),
                                               new AttackInfo<Attack>(Attack.Flamethrower1, m_info.flameThrowerAttack.range));
                    m_attackDecider[2].SetList(new AttackInfo<Attack>(Attack.PunchUppercut, m_info.punchAttack.range),
                                               new AttackInfo<Attack>(Attack.Flamethrower1, m_info.flameThrowerAttack.range),
                                               new AttackInfo<Attack>(Attack.SpinAttack, m_info.spinAttack.range),
                                               new AttackInfo<Attack>(Attack.Firebeam, m_info.firebeamAttack.range),
                                               new AttackInfo<Attack>(Attack.LongDash, m_info.longDashAttack.range));
                    break;
                case Phase.PhaseTwo:
                    m_patternDecider.SetList(new AttackInfo<Pattern>(Pattern.Phase2Pattern1, m_info.targetDistanceTolerance),
                                             new AttackInfo<Pattern>(Pattern.Phase2Pattern2, m_info.targetDistanceTolerance),
                                             new AttackInfo<Pattern>(Pattern.Phase2Pattern3, m_info.targetDistanceTolerance),
                                             new AttackInfo<Pattern>(Pattern.Phase2Pattern4, m_info.targetDistanceTolerance));
                    m_attackDecider[3].SetList(new AttackInfo<Attack>(Attack.PunchUppercut, m_info.punchAttack.range),
                                               new AttackInfo<Attack>(Attack.ShotgunBlast, m_info.shotgunBlastFireAttack.range),
                                               new AttackInfo<Attack>(Attack.Flamethrower1, m_info.flameThrowerAttack.range),
                                               new AttackInfo<Attack>(Attack.Flamethrower2, m_info.flameThrowerAttack.range), //change to flamethrower2 range
                                               new AttackInfo<Attack>(Attack.SpinAttack, m_info.spinAttack.range),
                                               new AttackInfo<Attack>(Attack.ShortDash, m_info.shortDashAttack.range));
                    m_attackDecider[4].SetList(new AttackInfo<Attack>(Attack.PunchUppercut, m_info.punchAttack.range),
                                               new AttackInfo<Attack>(Attack.ShotgunBlast, m_info.shotgunBlastFireAttack.range),
                                               new AttackInfo<Attack>(Attack.SpinAttack, m_info.spinAttack.range),
                                               new AttackInfo<Attack>(Attack.LongDash, m_info.longDashAttack.range));
                    m_attackDecider[5].SetList(new AttackInfo<Attack>(Attack.SpinAttack, m_info.spinAttack.range),
                                               new AttackInfo<Attack>(Attack.MeteorSmash, m_info.meteorAttack.range),
                                               new AttackInfo<Attack>(Attack.Firebeam, m_info.firebeamAttack.range));
                    m_attackDecider[6].SetList(new AttackInfo<Attack>(Attack.Flamethrower2, m_info.flameThrowerAttack.range), //change to flamethrower2 range
                                               new AttackInfo<Attack>(Attack.Firebeam, m_info.firebeamAttack.range));
                    break;
            }
            DecidedOnAttack(false);
            /*switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseOne:
                    AddToAttackCache(Attack.PunchUppercut, Attack.Flamethrower1, Attack.SpinAttack, Attack.Firebeam, Attack.LongDash);
                    AddToRangeCache(m_info.punchAttack.range, m_info.flameThrowerAttack.range, m_info.spinAttack.range, m_info.firebeamAttack.range, m_info.longDashAttack.range);
                    break;
                case Phase.PhaseTwo:
                    AddToAttackCache(Attack.PunchUppercut, Attack.Flamethrower1, Attack.SpinAttack, Attack.Firebeam, Attack.Flamethrower2, Attack.MeteorSmash, Attack.ShotgunBlast, Attack.ShortDash, Attack.LongDash);
                    AddToRangeCache(m_info.punchAttack.range, m_info.flameThrowerAttack.range, m_info.spinAttack.range, m_info.firebeamAttack.range, m_info.meteorAttack.range, m_info.shotgunBlastFireAttack.range, m_info.shortDashAttack.range, m_info.longDashAttack.range);
                    break;
            }*/

        }

        private void CinderBoltAI_DamageTaken1(object sender, Damageable.DamageEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }

        private void ChangeState()
        {
            if (!m_hasPhaseChanged && m_changeLocationRoutine == null)
            {
                //m_hitbox.SetInvulnerability(Invulnerability.None);
                /*StopAllCoroutines();*/
                m_stateHandle.OverrideState(State.Phasing);
                m_hasPhaseChanged = true;
                //UpdateAttackDeciderList();
                m_phaseHandle.ApplyChange();
                m_animation.DisableRootMotion();
                m_animation.SetEmptyAnimation(0, 0);
            }
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs)
        {
            if (m_stateHandle.currentState != State.Phasing)
            {
                m_stateHandle.OverrideState(State.Turning);
            }
        }

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                if (!m_isDetecting)
                {
                    m_isDetecting = true;
                }
                m_stateHandle.OverrideState(State.Intro);
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            if (m_stateHandle.currentState != State.Phasing)
            {
                m_animation.animationState.TimeScale = 1f;
                m_stateHandle.ApplyQueuedState();
            }
            m_phaseHandle.allowPhaseChange = true;
        }

        private IEnumerator IntroRoutine()
        {
            enabled = false;
            m_stateHandle.Wait(State.Chasing);
            m_movement.Stop();/*
            m_punchAttacker.SetDamageModifier(1);
            m_flamethrower1.SetDamageModifier(1);
            m_flamethrower2.SetDamageModifier(1);
            m_firebeam.SetDamageModifier(1);
            m_longD.SetDamageModifier(1);
            m_meteor.SetDamageModifier(1);
            m_shotG.SetDamageModifier(1);*/
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_stateHandle.ApplyQueuedState();
            yield return null;
            enabled = true;
        }

        private IEnumerator ChangePhaseRoutine()
        {
            m_stateHandle.Wait(State.Chasing);
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            StartCoroutine(OnFirstRuneShieldRoutine());
            m_hasPhaseChanged = false;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        private IEnumerator OnFirstRuneShieldRoutine()
        {
            m_hasRune = false;
            m_runeDuration = 10;
            m_runeShieldFX.SetActive(true);
            yield return new WaitForSeconds(m_runeDuration);
            m_runeShieldFX.SetActive(false);
            m_runeShieldBreakFX.SetActive(true);
            yield return new WaitForSeconds(1f);
            m_runeShieldBreakFX.SetActive(false);
            yield return null;
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            m_movement.Stop();

            m_punchAttacker.SetActive(true);
            m_punchAttacker2.SetActive(true);
            m_overchargedPunchAttacker.SetActive(false);
            m_overchargedPunchAttacker2.SetActive(false);
            m_flamethrower1.SetActive(true);
            m_overchargedFlamethrower1.SetActive(false);
            m_firebeam.SetActive(true);
            m_longD.SetActive(true);
            m_overchargedLongD.SetActive(false);
            m_shotG.SetActive(true);
            m_meteor.SetActive(true);
            m_overchargedMeteor.SetActive(false);
            m_flamethrower2.SetActive(true);
            m_overchargedFlamethrower2.SetActive(false);
            m_steamMalfAndOver.Play();
            m_movement.Stop();
            m_firebeamAnticipationFX.Stop();
            m_flamethrower1FX.Stop();
            m_flamethrower2FX.Stop();
            m_laserOriginMuzzleFX.Stop();
            m_longDashFX.Stop();
            m_meteorSmashFX.Stop();
            m_meteorSmashTrailFX.SetActive(false);
            m_muzzleLoopFX.Stop();
            m_shortDashFX.Stop();
            m_spinAttackFX.Stop();
            m_punchAttackCollider.enabled = false;
            m_punchAttackCollider.enabled = false;
            m_flamethrower2Colliders.enabled = false;
            m_firebeamCollider.enabled = false;
            m_longDashCollider.enabled = false;
            m_flamethrower2Colliders.enabled = false;
            m_meteorSmashCollider.enabled = false;
            m_spinAttackCollider[0].enabled = false;
            m_spinAttackCollider[1].enabled = false;
            StopAllCoroutines();
            m_movement.Stop();
            m_isDetecting = false;
        }

        /*private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            m_hitCount++;
            m_flamethrower1FX.Stop();
            m_firebeamAnticipationFX.Stop();
            //m_firebeamFX.Stop();
            if (m_hitCount == 5 && m_phaseHandle.currentPhase == Phase.PhaseOne)
            {
                StopAllCoroutines();
                m_hitCount = 0;
            }
        }*/

        /*private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            //m_stateHandle.OverrideState(State.ReevaluateSituation);
        }*/
        /*private IEnumerator PunchUppercutDecider()
        {
            yield return new WaitForSeconds(0.5f);
            m_stateHandle.Wait(State.Chasing);
            if (m_isRaging)
            {
                StartCoroutine(OverchargedPunchAttackRoutine());
            }
            else
            {
                StartCoroutine(PunchAttackRoutine());
            }
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            m_punchAttackCollider.enabled = false;
            yield return null;
        }
        private IEnumerator Flamethrower1Decider()
        {
            yield return new WaitForSeconds(0.5f);
            m_stateHandle.Wait(State.Chasing);
            if (m_isRaging)
            {
                StartCoroutine(Flamethrower1Routine());
            }
            else
            {
                StartCoroutine(Flamethrower1Routine());
            }
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            m_punchAttackCollider.enabled = false;
            yield return null;
        }
        private IEnumerator SpinAttackDecider()
        {
            yield return new WaitForSeconds(0.5f);
            m_stateHandle.Wait(State.Chasing);
            if (m_isRaging)
            {
                StartCoroutine(SpinAttackRoutine());
            }
            else
            {
                StartCoroutine(SpinAttackRoutine());
            }
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            m_punchAttackCollider.enabled = false;
            yield return null;
        }private IEnumerator FirebeamAttackDecider()
        {
            yield return new WaitForSeconds(0.5f);
            m_stateHandle.Wait(State.Chasing);
            if (m_isRaging)
            {
                StartCoroutine(FirebeamLaserRoutine());
            }
            else
            {
                StartCoroutine(FirebeamLaserRoutine());
            }
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            m_punchAttackCollider.enabled = false;
            yield return null;
        }*/
        #region Overcharged Attacks
        [SerializeField, TabGroup("Attackers")]
        private GameObject m_punchAttacker;
        [SerializeField, TabGroup("Attackers")]
        private GameObject m_punchAttacker2;
        [SerializeField, TabGroup("Attackers")]
        private GameObject m_flamethrower1;
        [SerializeField, TabGroup("Attackers")]
        private GameObject m_flamethrower2;
        [SerializeField, TabGroup("Attackers")]
        private GameObject m_firebeam;
        [SerializeField, TabGroup("Attackers")]
        private GameObject m_spinAttacker;
        [SerializeField, TabGroup("Attackers")]
        private GameObject m_longD;
        [SerializeField, TabGroup("Attackers")]
        private GameObject m_meteor;
        [SerializeField, TabGroup("Attackers")]
        private GameObject m_shotG;
        [SerializeField, TabGroup("Attackers")]
        private GameObject m_overchargedPunchAttacker;
        [SerializeField, TabGroup("Attackers")]
        private GameObject m_overchargedPunchAttacker2;
        [SerializeField, TabGroup("Attackers")]
        private GameObject m_overchargedFlamethrower1;
        [SerializeField, TabGroup("Attackers")]
        private GameObject m_overchargedFlamethrower2;
        [SerializeField, TabGroup("Attackers")]
        private GameObject m_overchargedFirebeam;
        [SerializeField, TabGroup("Attackers")]
        private GameObject m_overchargedSpinAttacker;
        [SerializeField, TabGroup("Attackers")]
        private GameObject m_overchargedLongD;
        [SerializeField, TabGroup("Attackers")]
        private GameObject m_overchargedMeteor;
        [SerializeField, TabGroup("Attackers")]
        private GameObject m_overchargedShotG;
        private IEnumerator OverchargedPunchAttackRoutine()
        {
            enabled = false;/*
            m_punchAttacker.enabled = false;
            m_punchAttacker2.enabled = false;
            m_overchargedPunchAttacker.enabled = true;
            m_overchargedPunchAttacker2.enabled = true;*/
            m_runeDuration = 5;
            Vector2 targetPoint = m_targetInfo.position;
            var direction = (targetPoint - (Vector2)transform.position).normalized;
            while (Vector2.Distance(transform.position, targetPoint) > m_info.punchAttack.range)
            {
                m_animation.SetAnimation(0, m_info.overchargedHoverForward, true);
                m_movement.MoveTowards(direction, m_info.move.speed * 2);
                yield return null;
            }
            m_steamThrustFX.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            //m_stateHandle.Wait(State.Chasing);
            m_movement.Stop();
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.overchargedPunchUppercutAttack, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.overchargedPunchUppercutAttack);
            m_overchargedPunchAttackCollider.enabled = false;
            m_animation.SetAnimation(0, m_info.overchargedIdle, true);
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            m_overchargedPunchAttackCollider.enabled = false;
            yield return null;
            enabled = true;
        }
        private IEnumerator OverchargedFlamethrower1Routine()
        {
            enabled = false;/*
            m_flamethrower1.enabled = false;
            m_overchargedFlamethrower1.enabled = true;*/
            m_runeDuration = 5;
            Vector2 targetPoint = m_targetInfo.position;
            var direction = (targetPoint - (Vector2)transform.position).normalized;
            while (Vector2.Distance(transform.position, targetPoint) > m_info.flameThrowerAttack.range)
            {
                m_animation.SetAnimation(0, m_info.overchargedHoverForward, true);
                m_movement.MoveTowards(direction, m_info.move.speed * 2);
                yield return null;
            }
            m_steamThrustFX.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            //m_stateHandle.Wait(State.Chasing);
            m_movement.Stop();
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.overchargedFlamethrower1Attack.animation, false);
            yield return new WaitForSeconds(0.4f);
            m_flamethrower1FX.Play();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.overchargedFlamethrower1Attack.animation);
            m_flamethrower1FX.Stop();
            m_animation.SetAnimation(0, m_info.overchargedIdle, true);
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
            enabled = true;
        }
        private IEnumerator OverchargedSpinAttackRoutine()
        {
            enabled = false;/*
            m_punchAttacker.enabled = false;
            m_overchargedPunchAttacker.enabled = true;*/
            m_steamThrustFX.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            //m_stateHandle.Wait(State.Chasing);
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.overchargedPreSpinAttack, false);
            yield return new WaitForSeconds(0.5f);
            m_animation.SetAnimation(0, m_info.overchargedSpinAttack, true);
            OverchargeSpinColliders(true);
            var m_followElapsedTime = 0f;
            var m_followDuration = 10f;
            while (m_followElapsedTime < m_followDuration)
            {
                m_movement.MoveTowards(new Vector2(m_targetInfo.position.x - transform.position.x, 0).normalized, (m_info.move.speed * 2));
                m_followElapsedTime += Time.deltaTime;
                yield return null;
            }
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.overchargedSpinEndAnimation, false);
            OverchargeSpinColliders(false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.overchargedSpinEndAnimation);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_animation.SetAnimation(0, m_info.overchargedIdle, true);
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
            enabled = true;
        }
        private IEnumerator OverchargedFirebeamRoutine()
        {
            enabled = false;/*
            m_firebeam.enabled = true;*/
            yield return new WaitForSeconds(0.5f);
            m_runeDuration = 5;
            //m_stateHandle.Wait(State.Chasing);
            /*if (m_phaseHandle.currentPhase == Phase.PhaseTwo)
            {
                StartCoroutine(RuneShieldRoutine());
            }*/
            int closestPointIndex = 0;
            float closestDistance = Vector2.Distance(m_firebeamTransformPoints[closestPointIndex].position, m_targetInfo.position);
            for (int i = 0; i < m_firebeamTransformPoints.Count; i++)
            {
                float distance = Vector2.Distance(m_firebeamTransformPoints[i].position, m_targetInfo.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPointIndex = i;
                }
            }
            Vector2 targetPoint = m_firebeamTransformPoints[closestPointIndex].position;
            var direction = (targetPoint - (Vector2)transform.position).normalized;
            while (Vector2.Distance(transform.position, targetPoint) > 1f)
            {
                // Move towards the target point
                m_animation.SetAnimation(0, m_info.overchargedMove, true);
                m_movement.MoveTowards(direction, m_info.move.speed * 2);
                yield return null;
            }
            m_steamThrustFX.SetActive(false);
            m_movement.Stop();
            for (int i = 1; i < m_firebeamTransformPoints.Count; i++)
            {
                if ((closestPointIndex + 1) % 2 == 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    m_character.SetFacing(HorizontalDirection.Left);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    m_character.SetFacing(HorizontalDirection.Right);
                }
            }
            m_animation.SetAnimation(0, m_info.overchargedFirebeamAttack, false);
            //yield return new WaitForSeconds(1f);
            //m_firebeamCollider.enabled = true;
            StartCoroutine(FirebeamLaserRoutine());
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.overchargedFirebeamAttack);
            yield return new WaitForSeconds(1f);
            //m_firebeamCollider.enabled = false;
            m_animation.SetAnimation(0, m_info.overchargedIdle, true);
            DecidedOnAttack(false);
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
            enabled = true;
        }
        private IEnumerator OverchargedShortDash()
        {
            enabled = false;/*
            m_longD.enabled = false;
            m_overchargedLongD.enabled = true;*/
            var targetPos = m_targetInfo.position.x;
            m_steamThrustFX.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            //m_stateHandle.Wait(State.Chasing);
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.overchargedShortDash, false);
            m_shortDashFX.Play();
            m_movement.MoveTowards(new Vector2(targetPos - transform.position.x, 0), m_info.shortDash.speed * 2);
            m_overchargedLongDashCollider.enabled = true;
            yield return new WaitForSeconds(0.5f);
            m_movement.Stop();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.overchargedShortDash);
            m_overchargedLongDashCollider.enabled = false;
            m_shortDashFX.Stop();
            m_hitbox.SetInvulnerability(Invulnerability.None);
            yield return new WaitForSeconds(.5f);
            if (!IsFacingTarget())
            {
                transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
                m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
            }
            var randomAttackDecider = UnityEngine.Random.Range(0, 3);
            switch (randomAttackDecider)
            {
                case 0:
                    Vector2 targetPoint = m_targetInfo.position;
                    var direction = (targetPoint - (Vector2)transform.position).normalized;
                    while (Vector2.Distance(transform.position, targetPoint) > m_info.punchAttack.range)
                    {
                        m_animation.SetAnimation(0, m_info.overchargedHoverForward, true);
                        m_movement.MoveTowards(direction, m_info.move.speed * 2);
                        yield return null;
                    }
                    m_steamThrustFX.SetActive(false);
                    yield return new WaitForSeconds(0.5f);
                    //m_stateHandle.Wait(State.Chasing);
                    m_movement.Stop();
                    m_animation.EnableRootMotion(true, false);
                    m_animation.SetAnimation(0, m_info.overchargedPunchUppercutAttack, false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.overchargedPunchUppercutAttack);
                    m_overchargedPunchAttackCollider.enabled = false;
                    m_animation.SetAnimation(0, m_info.overchargedIdle, true);
                    DecidedOnAttack(false);
                    m_animation.DisableRootMotion();
                    m_currentAttackCoroutine = null;
                    m_stateHandle.ApplyQueuedState();
                    m_overchargedPunchAttackCollider.enabled = false;
                    break;
                case 1:
                    Vector2 target = m_targetInfo.position;
                    var dir = (target - (Vector2)transform.position).normalized;
                    while (Vector2.Distance(transform.position, target) > m_info.flameThrowerAttack.range)
                    {
                        m_animation.SetAnimation(0, m_info.overchargedHoverForward, true);
                        m_movement.MoveTowards(dir, m_info.move.speed * 2);
                        yield return null;
                    }
                    m_steamThrustFX.SetActive(false);
                    yield return new WaitForSeconds(0.5f);
                    //m_stateHandle.Wait(State.Chasing);
                    m_movement.Stop();
                    m_animation.EnableRootMotion(true, false);
                    m_animation.SetAnimation(0, m_info.overchargedFlamethrower1Attack.animation, false);
                    yield return new WaitForSeconds(0.4f);
                    m_flamethrower1FX.Play();
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.overchargedFlamethrower1Attack.animation);
                    m_flamethrower1FX.Stop();
                    m_animation.SetAnimation(0, m_info.overchargedIdle, true);
                    DecidedOnAttack(false);
                    m_animation.DisableRootMotion();
                    m_currentAttackCoroutine = null;
                    m_stateHandle.ApplyQueuedState();
                    break;
                case 2:
                    Vector2 targetP = m_targetInfo.position;
                    var directiooon = (targetP - (Vector2)transform.position).normalized;
                    m_steamThrustFX.SetActive(true);
                    while (Vector2.Distance(transform.position, targetP) > m_info.punchAttack.range + 10f)
                    {
                        m_animation.SetAnimation(0, m_info.move, true);
                        m_movement.MoveTowards(directiooon, m_info.move.speed);
                        yield return null;
                    }
                    m_movement.Stop();
                    m_animation.SetAnimation(0, m_info.overchargedShotgunBlastPreAnimation, false);
                    Vector2 spitPos = m_projectilePoints.transform.position;
                    Vector3 v_diff = (targetP - spitPos);
                    float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
                    var aimRotation = atan2 * Mathf.Rad2Deg;
                    aimRotation -= 5f;
                    ProjectileLaunchHandle overchargeLaunchHandle = new ProjectileLaunchHandle();

                    //m_projectileLauncher.AimAt(m_targetInfo.position);
                    m_animation.SetAnimation(0, m_info.overchargedShotgunBlastFireAttack, false);
                    //m_projectileLauncher.LaunchProjectile();
                    yield return new WaitForSeconds(1f);
                    for (int i = 0; i < 3; i++)
                    {
                        m_projectilePoints.transform.rotation = Quaternion.Euler(0f, 0f, aimRotation);
                        Debug.Log("To");
                        var spawnDirection = m_projectilePoints.transform.right;
                        Debug.Log("mm");
                        overchargeLaunchHandle.Launch(m_info.overchargedBulletProjectile.projectileInfo.projectile, m_projectilePoints.transform.position, spawnDirection, m_info.overchargedBulletProjectile.projectileInfo.speed);
                        Debug.Log("i");
                        aimRotation += 5f;
                    }
                    yield return new WaitForSeconds(1f);
                    m_animation.SetAnimation(0, m_info.overchargedShotgunBlastBackToIdleAnimation, false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.overchargedShotgunBlastBackToIdleAnimation);
                    m_animation.SetAnimation(0, m_info.overchargedIdle, true);
                    DecidedOnAttack(false);
                    m_currentAttackCoroutine = null;
                    m_stateHandle.ApplyQueuedState();
                    break;
            }
            yield return null;
            enabled = true;
        }
        private IEnumerator OverchargedLongDash()
        {
            enabled = false;/*
            m_longD.enabled = false;
            m_overchargedLongD.enabled = true;*/
            var targetPos = m_targetInfo.position.x;
            m_steamThrustFX.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            m_movement.Stop();
            m_boosterChargeFX.Play();
            yield return new WaitForSeconds(2f);
            m_animation.SetAnimation(0, m_info.overchargedLongDash, false);
            m_longDashFX.Play();
            m_movement.MoveTowards(new Vector2(targetPos - transform.position.x, 0), m_info.longDash.speed * 2);
            m_overchargedLongDashCollider.enabled = true;
            m_animation.SetAnimation(0, m_info.longDashStopAnimation, false);
            yield return new WaitForSeconds(0.3f);
            m_movement.Stop();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.longDashStopAnimation);
            m_overchargedLongDashCollider.enabled = false;
            m_longDashFX.Stop();
            m_hitbox.SetInvulnerability(Invulnerability.None);
            yield return new WaitForSeconds(.5f);
            if (!IsFacingTarget())
            {
                transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
                m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
            }
            StartCoroutine(OverchargedPunchAttackRoutine());
            yield return null;
            enabled = true;
        }
        private IEnumerator OverchargedShotgunBlastRoutine()
        {
            enabled = false;/*
            m_shotG.enabled = true;*/
            m_steamThrustFX.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            m_runeDuration = 5;
            //m_stateHandle.Wait(State.Chasing);
            Vector2 targetPoint = m_targetInfo.position;
            var direction = (targetPoint - (Vector2)transform.position).normalized;
            m_steamThrustFX.SetActive(true);
            while (Vector2.Distance(transform.position, targetPoint) > m_info.punchAttack.range + 10f)
            {
                m_animation.SetAnimation(0, m_info.move, true);
                m_movement.MoveTowards(direction, m_info.move.speed);
                yield return null;
            }
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.overchargedShotgunBlastPreAnimation, false);
            Vector2 spitPos = m_projectilePoints.transform.position;
            Vector3 v_diff = (targetPoint - spitPos);
            float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
            var aimRotation = atan2 * Mathf.Rad2Deg;
            aimRotation -= 5f;
            ProjectileLaunchHandle overchargeLaunchHandle = new ProjectileLaunchHandle();
            
            //m_projectileLauncher.AimAt(m_targetInfo.position);
            m_animation.SetAnimation(0, m_info.overchargedShotgunBlastFireAttack, false);
            //m_projectileLauncher.LaunchProjectile();
            yield return new WaitForSeconds(1f);
            for (int i = 0; i < 3; i++)
            {
                m_projectilePoints.transform.rotation = Quaternion.Euler(0f, 0f, aimRotation);
                Debug.Log("To");
                var spawnDirection = m_projectilePoints.transform.right;
                Debug.Log("mm");

                overchargeLaunchHandle.Launch(m_info.overchargedBulletProjectile.projectileInfo.projectile, m_projectilePoints.transform.position, spawnDirection, m_info.overchargedBulletProjectile.projectileInfo.speed);
                Debug.Log("i");
                aimRotation += 5f;
            }
            yield return new WaitForSeconds(1f);
            m_animation.SetAnimation(0, m_info.overchargedShotgunBlastBackToIdleAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.overchargedShotgunBlastBackToIdleAnimation);
            m_animation.SetAnimation(0, m_info.overchargedIdle, true);
            DecidedOnAttack(false);
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
            enabled = true;
        }
        private IEnumerator OverchargedFlamethrower2Routine()
        {
            enabled = false;/*
            m_flamethrower2.enabled = false;
            m_overchargedFlamethrower2.enabled = true;*/
            m_steamThrustFX.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            m_runeDuration = 5;
            //m_stateHandle.Wait(State.Chasing);
            m_movement.Stop();
            Vector2 targetPoint = new Vector2(transform.position.x + 10f, m_firebeamTransformPoints[1].position.y + 20f);
            var direction = (targetPoint - (Vector2)transform.position).normalized;
            m_animation.SetAnimation(0, m_info.overchargedMove, true);
            while (Vector2.Distance(transform.position, targetPoint) > 10f)
            {
                m_movement.MoveTowards(direction, m_info.move.speed * 2);
                yield return null;
            }
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.overchargedHoverDownward, true);
            yield return new WaitForSeconds(0.5f);
            m_animation.SetAnimation(0, m_info.overchargedMove, true);
            m_overchargedFlamethrower2Colliders.enabled = true;
            m_flamethrower2FX.Play();
            var m_followElapsedTime = 0f;
            var m_followDuration = 10f;
            while (m_followElapsedTime < m_followDuration)
            {
                m_movement.MoveTowards(new Vector2((m_targetInfo.position.x + (m_character.facing == HorizontalDirection.Left ? 10f : -10f)) - transform.position.x, 0).normalized, m_info.move.speed * 2);
                m_followElapsedTime += Time.deltaTime;
                if (!IsFacingTarget())
                {
                    CustomTurn();
                }
                yield return null;
            }
            m_movement.Stop();
            m_overchargedFlamethrower2Colliders.enabled = false;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.overchargedMove);
            m_flamethrower2FX.Stop();
            m_animation.SetAnimation(0, m_info.overchargedIdle, true);
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
            enabled = true;
        }
        private IEnumerator OverchargedMeteorSmashRoutine()
        {
            enabled = false;/*
            m_meteor.enabled = false;
            m_overchargedMeteor.enabled = true;*/
            m_steamThrustFX.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            m_runeDuration = 5;
            //m_stateHandle.Wait(State.Chasing);
            m_movement.Stop();
            Vector2 targetPoint = new Vector2(m_targetInfo.position.x, m_firebeamTransformPoints[1].position.y + 20f);
            var direction = (targetPoint - (Vector2)transform.position).normalized;
            while (Vector2.Distance(transform.position, targetPoint) > 10f)
            {
                m_movement.MoveTowards(direction, m_info.move.speed * 2);
                yield return null;
            }
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.overchargedSpinPreAnimation, false);
            yield return new WaitForSeconds(0.5f);
            m_overchargedMeteorSmashCollider.enabled = true;
            m_animation.SetAnimation(0, m_info.overchargedSpinAttack, true);
            m_meteorSmashTrailFX.SetActive(true);
            m_meteorSmashFX.Play();
            Vector2 targetPointBelow = new Vector2(transform.position.x, m_firebeamTransformPoints[2].position.y - 10f);
            var directionVertical = (targetPointBelow - (Vector2)transform.position).normalized;
            while (Vector2.Distance(transform.position, targetPointBelow) > 10f)
            {
                m_movement.MoveTowards(directionVertical, m_info.move.speed * 6f);
                yield return null;
            }
            m_movement.Stop();
            m_meteorSmashTrailFX.SetActive(false);
            m_animation.SetAnimation(0, m_info.overchargedSpinEndAnimation, false);
            m_meteorSmashFX.Stop();
            if (m_targetInfo.isCharacterGrounded)
            {
                StartCoroutine(OverchargedSpinAttackRoutine());
            }
            //StartCoroutine(RuneShieldRoutine());
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.overchargedSpinEndAnimation);
            m_overchargedMeteorSmashCollider.enabled = false;
            m_animation.SetAnimation(0, m_info.overchargedIdle, true);
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
            enabled = true;
        }
        #endregion
        #region Normal Attacks
        private IEnumerator PunchAttackRoutine()
        {
            enabled = false;/*
            m_punchAttacker.enabled = true;
            m_punchAttacker2.enabled = true;
            m_overchargedPunchAttacker.enabled = false;
            m_overchargedPunchAttacker2.enabled = false;*/
            //yield return new WaitForSeconds(0.5f);
            m_runeDuration = 5;
            Vector2 targetPoint = m_targetInfo.position;
            var direction = (targetPoint - (Vector2)transform.position).normalized;
            while (Vector2.Distance(transform.position, targetPoint) > m_info.punchAttack.range)
            {
                m_animation.SetAnimation(0, m_info.move, true);
                m_movement.MoveTowards(direction, m_info.move.speed);
                yield return null;
            }
            m_steamThrustFX.SetActive(false);
            //m_stateHandle.Wait(State.Chasing);
            m_movement.Stop();
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.punchUppercut, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.punchUppercut);
            m_punchAttackCollider.enabled = false;
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            m_punchAttackCollider.enabled = false;
            yield return null;
            enabled = true;
        }
        private IEnumerator Flamethrower1Routine()
        {
            enabled = false;/*
            m_flamethrower1.enabled = true;
            m_overchargedFlamethrower1.enabled = false;*/
            //yield return new WaitForSeconds(0.5f);
            //m_stateHandle.Wait(State.Chasing);
            m_runeDuration = 5;
            Vector2 targetPoint = m_targetInfo.position;
            var direction = (targetPoint - (Vector2)transform.position).normalized;
            while (Vector2.Distance(transform.position, targetPoint) > m_info.punchAttack.range)
            {
                m_animation.SetAnimation(0, m_info.move, true);
                m_movement.MoveTowards(direction, m_info.move.speed);
                yield return null;
            }
            m_steamThrustFX.SetActive(false);
            m_movement.Stop();
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.flameThrowerAttack.animation, false);
            yield return new WaitForSeconds(0.4f);
            m_flamethrower1FX.Play();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flameThrowerAttack.animation);
            m_flamethrower1FX.Stop();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            enabled = true;
            yield return null;
        }
        private bool SpinColliders(bool isDone)
        {
            for (int i = 0; i < m_spinAttackCollider.Count; i++)
            {
                m_spinAttackCollider[i].enabled = isDone;
            }
            return isDone;
        }
        private bool OverchargeSpinColliders(bool isDone)
        {
            for (int i = 0; i < m_overchargedSpinAttackCollider.Count; i++)
            {
                m_overchargedSpinAttackCollider[i].enabled = isDone;
            }
            return isDone;
        }
        private IEnumerator SpinAttackRoutine()
        {
            enabled = false;/*
            m_punchAttacker.enabled = true;
            m_overchargedPunchAttacker.enabled = false;*/
            /*Vector2 targetPoint = m_targetInfo.position;
            var direction = (targetPoint - (Vector2)transform.position).normalized;
            m_steamThrustFX.SetActive(true);
            while (Vector2.Distance(transform.position, targetPoint) > m_info.spinAttack.range)
            {
                m_animation.SetAnimation(0, m_info.move, true);
                m_movement.MoveTowards(direction, m_info.move.speed);
                yield return null;
            }*/
            m_steamThrustFX.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            //m_stateHandle.Wait(State.Chasing);
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.spinPreAnimation, false);
            yield return new WaitForSeconds(0.5f);
            m_animation.SetAnimation(0, m_info.spinAttack, true);
            SpinColliders(true);
            var m_followElapsedTime = 0f;
            var m_followDuration = 10f;
            while (m_followElapsedTime < m_followDuration)
            {
                m_movement.MoveTowards(new Vector2(m_targetInfo.position.x - transform.position.x, 0).normalized, m_info.move.speed);
                m_followElapsedTime += Time.deltaTime;
                yield return null;
            }
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.spinEndAnimation, false);
            SpinColliders(false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spinEndAnimation);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
            enabled = true;
        }
        #region Laser Coroutine
        private IEnumerator FirebeamLaserRoutine()
        {
            yield return new WaitForSeconds(.1f);
            m_laserLookCoroutine = StartCoroutine(LaserLookRoutine());
            m_aimOn = true;
            m_laserBeamCoroutine = StartCoroutine(LaserBeamRoutine());

            yield return new WaitForSeconds(1f);
            m_aimOn = false;
            m_beamOn = true;
            yield return new WaitForSeconds(m_laserDuration);
            m_beamOn = false;
            yield return new WaitUntil(() => m_laserBeamCoroutine == null);
            //m_projectile.CallPoolRequest();
            yield return null;
        }
        private IEnumerator LaserLookRoutine()
        {
            while (true)
            {
                m_laserTargetPos = LookPosition(m_laserOrigin);
                yield return null;
            }
        }
        private IEnumerator AimRoutine()
        {
            while (true)
            {
                m_telegraphLineRenderer.SetPosition(0, m_telegraphLineRenderer.transform.position);
                m_lineRenderer.SetPosition(0, m_lineRenderer.transform.position);
                m_lineRenderer.SetPosition(1, m_lineRenderer.transform.position);
                yield return null;
            }
        }
        private Vector2 ShotPosition()
        {
            m_laserTargetPos = LookPosition(m_laserOrigin);
            Vector2 startPoint = m_laserOrigin.position;

            /*var direction = m_laserOrigin.right;*/
            var direction = m_character.facing == HorizontalDirection.Right ? Vector2.right : Vector2.left;
            //transform.localScale = GetComponentInParent<Character>().transform.localScale;

            var contactFilter = new ContactFilter2D();
            contactFilter.useTriggers = false;
            RaycastHit2D hit = Physics2D.Raycast(/*m_projectilePoint.position*/startPoint, direction, 1000, DChildUtility.GetEnvironmentMask());
            //Debug.DrawRay(startPoint, direction);
            //Debug.Log(hit)
            return hit.point;
        }
        private IEnumerator TelegraphLineRoutine()
        {
            //float timer = 0;
            //m_muzzleTelegraphFX.Play();
            m_telegraphLineRenderer.useWorldSpace = true;
            var timerOffset = m_telegraphLineRenderer.startWidth;
            m_telegraphLineRenderer.SetPosition(1, ShotPosition());
            //m_telegraphLineRenderer.startWidth -= Time.deltaTime * timerOffset;
            yield return null;
        }
        private IEnumerator LaserBeamRoutine()
        {
            if (m_aimOn)
            {
                StartCoroutine(TelegraphLineRoutine());
                StartCoroutine(m_aimRoutine);
            }

            yield return new WaitUntil(() => m_beamOn);
            if (!m_isRaging)
            {
                m_firebeamCollider.enabled = true;
            }
            else
            {
                m_overchargedFirebeamCollider.enabled = true;
            }
            StopCoroutine(m_aimRoutine);
            m_laserOriginMuzzleFX.Play();
            //m_muzzleFXGO.SetActive(true);
            m_muzzleLoopFX.Play();

            m_lineRenderer.useWorldSpace = true;
            while (m_beamOn)
            {
                m_muzzleLoopFX.transform.position = ShotPosition();

                m_lineRenderer.SetPosition(0, m_laserOrigin.position);
                m_lineRenderer.SetPosition(1, ShotPosition());
                //Debug.Log(m_lineRenderer.positionCount);
                for (int i = 0; i < m_lineRenderer.positionCount; i++)
                {
                    var pos = Vector3.zero;
                    if (!m_isRaging)
                    {
                        pos = m_lineRenderer.GetPosition(i) - m_edgeCollider.transform.position;
                    }
                    else
                    {
                        pos = m_lineRenderer.GetPosition(i) - m_overchargeEdgeCollider.transform.position;
                    }
                    pos.x *= (int)m_character.facing;
                    pos = new Vector2(pos.x, pos.y);
                    //if (i > 0)
                    //{
                    //    pos = pos * 0.7f;
                    //}
                    m_Points.Add(pos);
                }
                if (!m_isRaging)
                {
                    m_edgeCollider.points = m_Points.ToArray();
                }
                else
                {
                    m_overchargeEdgeCollider.points = m_Points.ToArray();
                }
                m_Points.Clear();
                yield return null;
            }
            if (!m_isRaging)
            {
                m_firebeamCollider.enabled = false;
            }
            else
            {
                m_overchargedFirebeamCollider.enabled = false;
            }

            m_laserOriginMuzzleFX.Stop();
            m_muzzleLoopFX.Stop();
            //m_muzzleFXGO.SetActive(false);
            //yield return new WaitUntil(() => !m_beamOn);
            ResetLaser();
            m_laserBeamCoroutine = null;
            yield return null;
        }
        private void ResetLaser()
        {
            m_telegraphLineRenderer.useWorldSpace = false;
            m_lineRenderer.useWorldSpace = false;
            m_lineRenderer.SetPosition(0, Vector3.zero);
            m_lineRenderer.SetPosition(1, Vector3.zero);
            m_telegraphLineRenderer.SetPosition(0, Vector3.zero);
            m_telegraphLineRenderer.SetPosition(1, Vector3.zero);
            m_telegraphLineRenderer.startWidth = 30;
            m_Points.Clear();
            /*for (int i = 0; i < m_lineRenderer.positionCount; i++)
            {
                m_Points.Add(Vector2.zero);
            }*/
            if (!m_isRaging)
            {
                m_edgeCollider.points = m_Points.ToArray();
            }
            else
            {
                m_overchargeEdgeCollider.points = m_Points.ToArray();
            }
        }
        protected new Vector2 LookPosition(Transform startPoint)
        {
            int hitCount = 0;
            //RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down,  1000, DChildUtility.GetEnvironmentMask());
            RaycastHit2D[] hit = Cast(startPoint.position, startPoint.right, 1000, true, out hitCount, true);
            Debug.DrawRay(startPoint.position, hit[0].point);
            //var hitPos = (new Vector2(m_projectilePoint.position.x, Vector2.down.y) * hit[0].distance);
            //return hitPos;
            return hit[0].point;
        }
        #endregion
        private IEnumerator FirebeamRoutine()
        {
            enabled = false;/*
            m_firebeam.enabled = true;*/
            yield return new WaitForSeconds(0.5f);
            m_runeDuration = 5;
            //m_stateHandle.Wait(State.Chasing);
            /*if (m_phaseHandle.currentPhase == Phase.PhaseTwo)
            {
                StartCoroutine(RuneShieldRoutine());
            }*/
            int closestPointIndex = 0;
            float closestDistance = Vector2.Distance(m_firebeamTransformPoints[closestPointIndex].position, m_targetInfo.position);
            for (int i = 0; i < m_firebeamTransformPoints.Count; i++)
            {
                float distance = Vector2.Distance(m_firebeamTransformPoints[i].position, m_targetInfo.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPointIndex = i;
                }
            }
            Vector2 targetPoint = m_firebeamTransformPoints[closestPointIndex].position;
            var direction = (targetPoint - (Vector2)transform.position).normalized;
            while (Vector2.Distance(transform.position, targetPoint) > 1f)
            {
                // Move towards the target point
                m_animation.SetAnimation(0, m_info.move, true);
                m_movement.MoveTowards(direction, m_info.move.speed);
                yield return null;
            }
            m_steamThrustFX.SetActive(false);
            m_movement.Stop();
            for (int i = 1; i < m_firebeamTransformPoints.Count; i++)
            {
                if ((closestPointIndex + 1) % 2 == 0)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    m_character.SetFacing(HorizontalDirection.Left);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    m_character.SetFacing(HorizontalDirection.Right);
                }
            }
            m_animation.SetAnimation(0, m_info.firebeamAttack, false);
            yield return new WaitForSeconds(0.25f);
            StartCoroutine(FirebeamLaserRoutine());
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.firebeamAttack);
            //m_firebeamCollider.enabled = false;
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            DecidedOnAttack(false);
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
            enabled = true;
        }
        private IEnumerator ShortDash()
        {
            enabled = false;/*
            m_longD.enabled = true;
            m_overchargedLongD.enabled = false;*/
            var targetPos = m_targetInfo.position.x;
            m_steamThrustFX.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            //m_stateHandle.Wait(State.Chasing);
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.shortDash, false);
            m_shortDashFX.Play();
            m_movement.MoveTowards(new Vector2(targetPos - transform.position.x, 0).normalized, m_info.shortDash.speed);
            m_longDashCollider.enabled = true;
            yield return new WaitForSeconds(1f);
            m_movement.Stop();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.shortDash);
            m_longDashCollider.enabled = false;
            m_shortDashFX.Stop();
            m_hitbox.SetInvulnerability(Invulnerability.None);
            yield return new WaitForSeconds(.5f);
            if (!IsFacingTarget())
            {
                transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
                m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
            }
            var randomAttackDecider = UnityEngine.Random.Range(0, 3);
            switch (randomAttackDecider)
            {
                case 0:
                    Vector2 targetPoint = m_targetInfo.position;
                    var direction = (targetPoint - (Vector2)transform.position).normalized;
                    while (Vector2.Distance(transform.position, targetPoint) > m_info.punchAttack.range)
                    {
                        m_animation.SetAnimation(0, m_info.move, true);
                        m_movement.MoveTowards(direction, m_info.move.speed);
                        yield return null;
                    }
                    m_steamThrustFX.SetActive(false);
                    //m_stateHandle.Wait(State.Chasing);
                    m_movement.Stop();
                    m_animation.EnableRootMotion(true, false);
                    m_animation.SetAnimation(0, m_info.punchUppercut, false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.punchUppercut);
                    m_punchAttackCollider.enabled = false;
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    DecidedOnAttack(false);
                    m_animation.DisableRootMotion();
                    m_currentAttackCoroutine = null;
                    m_stateHandle.ApplyQueuedState();
                    m_punchAttackCollider.enabled = false;
                    break;
                case 1:
                    Vector2 target = m_targetInfo.position;
                    var dir = (target - (Vector2)transform.position).normalized;
                    while (Vector2.Distance(transform.position, target) > m_info.punchAttack.range)
                    {
                        m_animation.SetAnimation(0, m_info.move, true);
                        m_movement.MoveTowards(dir, m_info.move.speed);
                        yield return null;
                    }
                    m_steamThrustFX.SetActive(false);
                    m_movement.Stop();
                    m_animation.EnableRootMotion(true, false);
                    m_animation.SetAnimation(0, m_info.flameThrowerAttack.animation, false);
                    yield return new WaitForSeconds(0.4f);
                    m_flamethrower1FX.Play();
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flameThrowerAttack.animation);
                    m_flamethrower1FX.Stop();
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    DecidedOnAttack(false);
                    m_animation.DisableRootMotion();
                    m_currentAttackCoroutine = null;
                    m_stateHandle.ApplyQueuedState();
                    break;
                case 2:
                    Vector2 targetP = m_targetInfo.position;
                    var directiooon = (targetP - (Vector2)transform.position).normalized;
                    m_steamThrustFX.SetActive(true);
                    while (Vector2.Distance(transform.position, targetP) > m_info.punchAttack.range + 40f)
                    {
                        m_animation.SetAnimation(0, m_info.move, true);
                        m_movement.MoveTowards(directiooon, m_info.move.speed);
                        yield return null;
                    }
                    m_steamThrustFX.SetActive(false);
                    m_movement.Stop();
                    m_animation.SetAnimation(0, m_info.shotgunBlastPreAnimation, false);
                    Vector2 spitPos = m_projectilePoints.transform.position;
                    Vector3 v_diff = (targetP - spitPos);
                    float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
                    var aimRotation = atan2 * Mathf.Rad2Deg;
                    aimRotation -= 5f;
                    ProjectileLaunchHandle launchHandle = new ProjectileLaunchHandle();
                    m_animation.SetAnimation(0, m_info.shotgunBlastFireAttack, false);
                    yield return new WaitForSeconds(0.5f);
                    for (int i = 0; i < 3; i++)
                    {
                        m_projectilePoints.transform.rotation = Quaternion.Euler(0f, 0f, aimRotation);
                        var spawnDirection = m_projectilePoints.transform.right;
                        launchHandle.Launch(m_info.bulletProjectile.projectileInfo.projectile, m_projectilePoints.transform.position, spawnDirection, m_info.bulletProjectile.projectileInfo.speed);
                        aimRotation += 5f;
                    }
                    //m_projectileLauncher.LaunchProjectile();
                    yield return new WaitForSeconds(0.5f);
                    m_animation.SetAnimation(0, m_info.shotgunBlastBackToIdleAnimation, false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.shotgunBlastBackToIdleAnimation);
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    DecidedOnAttack(false);
                    m_currentAttackCoroutine = null;
                    m_stateHandle.ApplyQueuedState();
                    break;
            }
            yield return null;
            enabled = true;
        }
        private IEnumerator LongDash()
        {
            enabled = false;/*
            m_longD.enabled = true;
            m_overchargedLongD.enabled = false;*/
            var targetPos = m_targetInfo.position.x;
            m_steamThrustFX.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            m_movement.Stop();
            m_boosterChargeFX.Play();
            yield return new WaitForSeconds(2f);
            //m_stateHandle.Wait(State.Chasing);
            m_animation.SetAnimation(0, m_info.longDashAttack, false);
            m_longDashFX.Play();
            m_movement.MoveTowards(new Vector2(targetPos - transform.position.x, 0), m_info.longDash.speed);
            m_longDashCollider.enabled = true;
            m_animation.SetAnimation(0, m_info.longDashStopAnimation, false);
            yield return new WaitForSeconds(0.8f);
            m_movement.Stop();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.longDashStopAnimation);
            m_longDashCollider.enabled = false;
            m_longDashFX.Stop();
            m_hitbox.SetInvulnerability(Invulnerability.None);
            yield return new WaitForSeconds(.5f);
            if (!IsFacingTarget())
            {
                /*m_animation.SetAnimation(0, m_info.turnAnimation, false);*/
                //CustomTurn();
                transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
                m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
            }
            Vector2 targetPoint = m_targetInfo.position;
            var direction = (targetPoint - (Vector2)transform.position).normalized;
            while (Vector2.Distance(transform.position, targetPoint) > m_info.punchAttack.range)
            {
                m_animation.SetAnimation(0, m_info.move, true);
                m_movement.MoveTowards(direction, m_info.move.speed);
                yield return null;
            }
            //m_stateHandle.Wait(State.Chasing);
            m_movement.Stop();
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.punchUppercut, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.punchUppercut);
            m_punchAttackCollider.enabled = false;
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            m_punchAttackCollider.enabled = false;
            yield return null;
            enabled = true;
        }
        private IEnumerator ShotgunBlastRoutine()
        {
            enabled = false;/*
            m_shotG.enabled = true;*/
            //m_steamThrustFX.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            m_runeDuration = 5;
            Vector2 targetPoint = m_targetInfo.position;
            var direction = (targetPoint - (Vector2)transform.position).normalized;
            m_steamThrustFX.SetActive(true);
            while (Vector2.Distance(transform.position, targetPoint) > m_info.punchAttack.range + 40f)
            {
                m_animation.SetAnimation(0, m_info.move, true);
                m_movement.MoveTowards(direction, m_info.move.speed);
                yield return null;
            }
            //m_stateHandle.Wait(State.Chasing);
            m_steamThrustFX.SetActive(false);
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.shotgunBlastPreAnimation, false);
            Vector2 spitPos = m_projectilePoints.transform.position;
            Vector3 v_diff = (targetPoint - spitPos);
            float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
            var aimRotation = atan2 * Mathf.Rad2Deg;
            aimRotation -= 5f;
            ProjectileLaunchHandle launchHandle = new ProjectileLaunchHandle();
            //yield return new WaitForSeconds(1.5f);
            //m_projectileLauncher.AimAt(m_targetInfo.position);
            m_animation.SetAnimation(0, m_info.shotgunBlastFireAttack, false);
            yield return new WaitForSeconds(0.5f);
            for (int i = 0; i < 3; i++)
            {
                m_projectilePoints.transform.rotation = Quaternion.Euler(0f, 0f, aimRotation);
                Debug.Log("Lea");
                var spawnDirection = m_projectilePoints.transform.right;
                Debug.Log("nd");
                launchHandle.Launch(m_info.bulletProjectile.projectileInfo.projectile, m_projectilePoints.transform.position, spawnDirection, m_info.bulletProjectile.projectileInfo.speed);
                Debug.Log("ro");
                aimRotation += 5f;
            }
            //m_projectileLauncher.LaunchProjectile();
            yield return new WaitForSeconds(0.5f);
            m_animation.SetAnimation(0, m_info.shotgunBlastBackToIdleAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.shotgunBlastBackToIdleAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            DecidedOnAttack(false);
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
            enabled = true;
        }
        private IEnumerator MeteorSmashRoutine()
        {
            enabled = false;/*
            m_meteor.enabled = true;
            m_overchargedMeteor.enabled = false;*/
            m_steamThrustFX.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            m_runeDuration = 5;
            //m_stateHandle.Wait(State.Chasing);
            m_movement.Stop();
            Vector2 targetPoint = new Vector2(m_targetInfo.position.x, m_firebeamTransformPoints[1].position.y + 20f);
            var direction = (targetPoint - (Vector2)transform.position).normalized;
            while (Vector2.Distance(transform.position, targetPoint) > 10f)
            {
                m_movement.MoveTowards(direction, m_info.move.speed);
                yield return null;
            }
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.spinPreAnimation, false);
            yield return new WaitForSeconds(0.5f);
            m_meteorSmashCollider.enabled = true;
            m_animation.SetAnimation(0, m_info.spinAttack, true);
            m_meteorSmashTrailFX.SetActive(true);
            m_meteorSmashFX.Play();
            Vector2 targetPointBelow = new Vector2(transform.position.x, m_firebeamTransformPoints[2].position.y - 10f);
            var directionVertical = (targetPointBelow - (Vector2)transform.position).normalized;
            while (Vector2.Distance(transform.position, targetPointBelow) > 10f)
            {
                m_movement.MoveTowards(directionVertical, m_info.move.speed * 4f);
                yield return null;
            }
            m_movement.Stop();
            m_meteorSmashTrailFX.SetActive(false);
            m_animation.SetAnimation(0, m_info.spinEndAnimation, false);
            m_meteorSmashFX.Stop();
            if (m_targetInfo.isCharacterGrounded)
            {
                StartCoroutine(SpinAttackRoutine());
            }
            //StartCoroutine(RuneShieldRoutine());
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spinEndAnimation);
            m_meteorSmashCollider.enabled = false;
            m_animation.SetAnimation(0, m_info.idleAnimation, true);

            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
            enabled = true;
        }
        private IEnumerator Flamethrower2Routine()
        {
            enabled = false;/*
            m_flamethrower2.enabled = true;
            m_overchargedFlamethrower2.enabled = false;*/
            m_steamThrustFX.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            m_runeDuration = 5;
            //m_stateHandle.Wait(State.Chasing);
            m_movement.Stop();
            Vector2 targetPoint = new Vector2(transform.position.x + 10f, m_firebeamTransformPoints[1].position.y + 20f);
            var direction = (targetPoint - (Vector2)transform.position).normalized;
            while (Vector2.Distance(transform.position, targetPoint) > 10f)
            {
                m_movement.MoveTowards(direction, m_info.move.speed);
                yield return null;
            }
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.hoverDownward, true);
            yield return new WaitForSeconds(0.5f);
            m_animation.SetAnimation(0, m_info.move, true);
            m_flamethrower2Colliders.enabled = true;
            m_flamethrower2FX.Play();
            var m_followElapsedTime = 0f;
            var m_followDuration = 10f;
            while (m_followElapsedTime < m_followDuration)
            {
                m_movement.MoveTowards(new Vector2((m_targetInfo.position.x + (m_character.facing == HorizontalDirection.Left? 10f : -10f)) - transform.position.x, 0).normalized, m_info.move.speed);
                m_followElapsedTime += Time.deltaTime;
                if (!IsFacingTarget())
                {
                    CustomTurn();
                }
                yield return null;
            }
            m_movement.Stop();
            m_flamethrower2Colliders.enabled = false;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.move);
            m_flamethrower2FX.Stop();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
            enabled = true;
        }
        #endregion

        #region Movement
        private void MoveToTarget(float targetRange)
        {
            enabled = false;
            if (!IsTargetInRange(targetRange))
            {
                m_animation.SetAnimation(0, m_info.move, true);
                m_movement.MoveTowards(Vector2.right * transform.localScale.x, m_info.move.speed);
                /*if (transform.position.y + 5 < m_targetInfo.position.y)
                {
                    m_movement.Stop();
                    m_movement.MoveTowards(Vector2.up * transform.localScale.y, m_info.move.speed);
                }
                if (m_targetInfo.position.y < -100)
                {
                    m_movement.Stop();
                    m_movement.MoveTowards(Vector2.down * transform.localScale.y, m_info.move.speed);
                    if (transform.position.y < -127)
                    {
                        //m_movement.Stop();
                        m_movement.MoveTowards(Vector2.right * transform.localScale.x, m_info.move.speed);
                    }
                }*/
            }
            else
            {
                //m_movement.Stop();
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
            enabled = true;
        }
        #endregion

        private bool AllowAttack(int phaseIndex, State state)
        {
            if (m_currentPhaseIndex >= phaseIndex)
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
            for (int i = 0; i < m_attackDecider.Length; i++)
            {
                m_attackDecider[i].hasDecidedOnAttack = condition;
            }
        }

        private void UpdateAttackDeciderList()
        {
            /*m_patternDecider.SetList(new AttackInfo<Pattern>(Pattern.Phase1Pattern1, m_info.targetDistanceTolerance),
                             new AttackInfo<Pattern>(Pattern.Phase1Pattern2, m_info.targetDistanceTolerance),
                             new AttackInfo<Pattern>(Pattern.Phase2Pattern1, m_info.targetDistanceTolerance),
                             new AttackInfo<Pattern>(Pattern.Phase2Pattern2, m_info.targetDistanceTolerance),
                             new AttackInfo<Pattern>(Pattern.Phase2Pattern3, m_info.targetDistanceTolerance),
                             new AttackInfo<Pattern>(Pattern.Phase2Pattern4, m_info.targetDistanceTolerance));
            m_attackDecider[1].SetList(new AttackInfo<Attack>(Attack.PunchUppercut, m_info.punchAttack.range),
                            new AttackInfo<Attack>(Attack.Flamethrower1, m_info.flameThrowerAttack.range));
            m_attackDecider[2].SetList(new AttackInfo<Attack>(Attack.PunchUppercut, m_info.punchAttack.range),
                                     new AttackInfo<Attack>(Attack.Flamethrower1, m_info.flameThrowerAttack.range),
                                     new AttackInfo<Attack>(Attack.SpinAttack, m_info.spinAttack.range),
                                     new AttackInfo<Attack>(Attack.Firebeam, m_info.firebeamAttack.range),
                                     new AttackInfo<Attack>(Attack.LongDash, m_info.longDashAttack.range));
            m_attackDecider[3].SetList(new AttackInfo<Attack>(Attack.PunchUppercut, m_info.punchAttack.range),
                             new AttackInfo<Attack>(Attack.ShotgunBlast, m_info.shotgunBlastFireAttack.range),
                             new AttackInfo<Attack>(Attack.Flamethrower1, m_info.flameThrowerAttack.range),
                             new AttackInfo<Attack>(Attack.Flamethrower2, m_info.flameThrowerAttack.range), //change to flamethrower2 range
                             new AttackInfo<Attack>(Attack.SpinAttack, m_info.spinAttack.range),
                             new AttackInfo<Attack>(Attack.ShortDash, m_info.shortDashAttack.range));
            m_attackDecider[4].SetList(new AttackInfo<Attack>(Attack.PunchUppercut, m_info.punchAttack.range),
                                     new AttackInfo<Attack>(Attack.ShotgunBlast, m_info.shotgunBlastFireAttack.range),
                                     new AttackInfo<Attack>(Attack.SpinAttack, m_info.spinAttack.range),
                                     new AttackInfo<Attack>(Attack.LongDash, m_info.longDashAttack.range));
            m_attackDecider[5].SetList(new AttackInfo<Attack>(Attack.SpinAttack, m_info.spinAttack.range),
                                     new AttackInfo<Attack>(Attack.MeteorSmash, m_info.meteorAttack.range),
                                     new AttackInfo<Attack>(Attack.Firebeam, m_info.firebeamAttack.range));
            m_attackDecider[6].SetList(new AttackInfo<Attack>(Attack.Flamethrower2, m_info.flameThrowerAttack.range), //change to flamethrower2 range
                                     new AttackInfo<Attack>(Attack.Firebeam, m_info.firebeamAttack.range));*/
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

        private void ExecuteAttack(int patternIndex)
        {
            //m_patternCount = new float[m_currentPhaseIndex == 1 ? 2 : 4];
            /*if (m_attackCount < m_patternCount[patternIndex])
            {*/
            enabled = false;
            ChooseAttack(patternIndex);
            if (IsTargetInRange(m_patternDecider.chosenAttack.range))
            {
                m_stateHandle.Wait(State.Attacking);
                switch (m_currentAttack)
                {
                    case Attack.PunchUppercut:
                        m_stateHandle.Wait(State.Chasing);
                        Debug.Log("Cinder used Punch Attack");
                        if (patternIndex == 1 || patternIndex == 2 || patternIndex == 3 || patternIndex == 4)
                        {
                            if (m_phaseHandle.currentPhase == Phase.PhaseOne || m_phaseHandle.currentPhase == Phase.PhaseTwo)
                            {
                                if (!m_isRaging)
                                {
                                    m_currentAttackCoroutine = StartCoroutine(PunchAttackRoutine());
                                }
                                else
                                {
                                    m_currentAttackCoroutine = StartCoroutine(OverchargedPunchAttackRoutine());
                                }
                            }
                            Debug.Log("Punch Attack finished or cancelled");
                            break;
                        }
                        else
                        {
                            DecidedOnAttack(false);
                            m_stateHandle.ApplyQueuedState();
                        }
                        enabled = true;
                        break;
                    case Attack.Flamethrower1:
                        m_stateHandle.Wait(State.Chasing);
                        Debug.Log("Cinder used Flamethrower1");
                        if (patternIndex == 1 || patternIndex == 2 || patternIndex == 3)
                        {

                            if (m_phaseHandle.currentPhase == Phase.PhaseOne || m_phaseHandle.currentPhase == Phase.PhaseTwo)
                            {
                                if (!m_isRaging)
                                {
                                    m_currentAttackCoroutine = StartCoroutine(Flamethrower1Routine());
                                }
                                else
                                {
                                    m_currentAttackCoroutine = StartCoroutine(OverchargedFlamethrower1Routine());
                                }
                            }
                            Debug.Log("Flamethrower1 finished or cancelled");
                            break;
                        }
                        else
                        {
                            DecidedOnAttack(false);
                            m_stateHandle.ApplyQueuedState();
                        }
                        enabled = true;
                        break;
                    case Attack.LongDash:
                        m_stateHandle.Wait(State.Chasing);
                        Debug.Log("Cinder used Long Dash");
                        if (patternIndex == 2 || patternIndex == 4)
                        {
                            if (m_phaseHandle.currentPhase == Phase.PhaseOne || m_phaseHandle.currentPhase == Phase.PhaseTwo)
                            {
                                if (!m_isRaging)
                                {
                                    m_currentAttackCoroutine = StartCoroutine(LongDash());
                                }
                                else
                                {
                                    m_currentAttackCoroutine = StartCoroutine(OverchargedLongDash());
                                }
                            }
                            Debug.Log("Long Dash finished or cancelled");
                            break;
                        }
                        else
                        {
                            DecidedOnAttack(false);
                            m_stateHandle.ApplyQueuedState();
                        }
                        enabled = true;
                        break;
                    case Attack.ShortDash:
                        m_stateHandle.Wait(State.Chasing);
                        Debug.Log("Cinder used Short Dash");
                        if (patternIndex == 3)
                        {
                            if (m_phaseHandle.currentPhase == Phase.PhaseTwo)
                            {
                                if (!m_isRaging)
                                {
                                    m_currentAttackCoroutine = StartCoroutine(ShortDash());
                                }
                                else
                                {
                                    m_currentAttackCoroutine = StartCoroutine(OverchargedShortDash());
                                }
                            }
                            Debug.Log("Short Dash finished or cancelled");
                            break;
                        }
                        else
                        {
                            DecidedOnAttack(false);
                            m_stateHandle.ApplyQueuedState();
                        }
                        enabled = true;
                        break;
                    case Attack.SpinAttack:
                        m_stateHandle.Wait(State.Chasing);
                        Debug.Log("Cinder used Spin Attack");
                        if (patternIndex == 2 || patternIndex == 3 || patternIndex == 4 || patternIndex == 5)
                        {
                            if (m_phaseHandle.currentPhase == Phase.PhaseOne || m_phaseHandle.currentPhase == Phase.PhaseTwo)
                            {
                                if (!m_isRaging)
                                {
                                    m_currentAttackCoroutine = StartCoroutine(SpinAttackRoutine());
                                }
                                else
                                {
                                    m_currentAttackCoroutine = StartCoroutine(OverchargedSpinAttackRoutine());
                                }
                            }
                            Debug.Log("Spin Attack finished or cancelled");
                            break;
                        }
                        else
                        {
                            DecidedOnAttack(false);
                            m_stateHandle.ApplyQueuedState();
                        }
                        enabled = true;
                        break;
                    case Attack.Firebeam:
                        m_stateHandle.Wait(State.Chasing);
                        Debug.Log("Cinder used Firebeam");
                        if (patternIndex == 2 || patternIndex == 5 || patternIndex == 6)
                        {
                            if (m_phaseHandle.currentPhase == Phase.PhaseOne || m_phaseHandle.currentPhase == Phase.PhaseTwo)
                            {
                                if (!m_isRaging)
                                {
                                    m_currentAttackCoroutine = StartCoroutine(FirebeamRoutine());
                                }
                                else
                                {
                                    m_currentAttackCoroutine = StartCoroutine(OverchargedFirebeamRoutine());
                                }
                            }
                            Debug.Log("Firebeam finished or cancelled");
                            break;
                        }
                        else
                        {
                            DecidedOnAttack(false);
                            m_stateHandle.ApplyQueuedState();
                        }
                        enabled = true;
                        break;
                    case Attack.ShotgunBlast:
                        m_stateHandle.Wait(State.Chasing);
                        Debug.Log("Cinder used Shotgun Blast");
                        if (patternIndex == 3 || patternIndex == 4)
                        {
                            if (m_phaseHandle.currentPhase == Phase.PhaseTwo)
                            {
                                if (!m_isRaging)
                                {
                                    m_currentAttackCoroutine = StartCoroutine(ShotgunBlastRoutine());
                                }
                                else
                                {
                                    m_currentAttackCoroutine = StartCoroutine(OverchargedShotgunBlastRoutine());
                                }
                            }
                            Debug.Log("Shotgun Blast finished or cancelled");
                            break;
                        }
                        else
                        {
                            DecidedOnAttack(false);
                            m_stateHandle.ApplyQueuedState();
                        }
                        enabled = true;
                        break;
                    case Attack.Flamethrower2:
                        m_stateHandle.Wait(State.Chasing);
                        Debug.Log("Cinder used Flamethrower2");
                        if (patternIndex == 3 || patternIndex == 6)
                        {
                            if (m_phaseHandle.currentPhase == Phase.PhaseTwo)
                            {
                                if (!m_isRaging)
                                {
                                    m_currentAttackCoroutine = StartCoroutine(Flamethrower2Routine());
                                }
                                else
                                {
                                    m_currentAttackCoroutine = StartCoroutine(OverchargedFlamethrower2Routine());
                                }
                            }
                            Debug.Log("Flamethrower2 finished or cancelled");
                            break;
                        }
                        else
                        {
                            DecidedOnAttack(false);
                            m_stateHandle.ApplyQueuedState();
                        }
                        enabled = true;
                        break;
                    case Attack.MeteorSmash:
                        m_stateHandle.Wait(State.Chasing);
                        Debug.Log("Cinder used Meteor Smash");
                        if (patternIndex == 5)
                        {
                            if (m_phaseHandle.currentPhase == Phase.PhaseTwo)
                            {
                                if (!m_isRaging)
                                {
                                    m_currentAttackCoroutine = StartCoroutine(MeteorSmashRoutine());
                                }
                                else
                                {
                                    m_currentAttackCoroutine = StartCoroutine(OverchargedMeteorSmashRoutine());
                                }
                            }
                            Debug.Log("Meteor Smash finished or cancelled");
                            break;
                        }
                        else
                        {
                            DecidedOnAttack(false);
                            m_stateHandle.ApplyQueuedState();
                        }
                        enabled = true;
                        break;
                }
            }
            else
            {
                m_steamThrustFX.SetActive(true);
                enabled = false;
                MoveToTarget(m_currentAttackRange);
                enabled = true;
                //m_steamThrustFX.SetActive(false);
            }
            //}
        }

        private void ChooseAttack(int patternIndex)
        {
            if (!m_attackDecider[patternIndex].hasDecidedOnAttack)
            {
                IsAllAttackComplete();
                for (int i = 0; i < m_attackCache.Count; i++)
                {
                    m_attackDecider[patternIndex].DecideOnAttack();
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
        }
        private void AddToAttackAndRangeCaches(params object[] args)
        {
            if (args.Length % 2 != 0)
            {
                return;
            }

            for (int i = 0; i < args.Length; i += 2)
            {
                if (args[i] is Attack attack && args[i + 1] is float range)
                {
                    AddToAttackCache(attack);
                    AddToRangeCache(range);
                }
            }
        }
        protected override void Awake()
        {
            base.Awake();
            /*m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;*/
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_patternDecider = new RandomAttackDecider<Pattern>();
            m_attackDecider = new RandomAttackDecider<Attack>[9];
            m_heatHandler.SetConfiguration(m_info.heatHandleConfiguration);
            m_projectile = GetComponent<SimpleAttackProjectile>();
            m_projectileLauncher = new ProjectileLauncher(m_info.bulletProjectile.projectileInfo, m_projectilePoints);
            m_overchargeProjectileLauncher = new ProjectileLauncher(m_info.overchargedBulletProjectile.projectileInfo, m_projectilePoints);
            for (int i = 0; i < m_attackDecider.Length; i++)
            {
                m_attackDecider[i] = new RandomAttackDecider<Attack>();
            }
            m_patternCount = new float[2];
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            //UpdateAttackDeciderList();
            m_attackCache = new List<Attack>();
            AddToAttackCache(Attack.Firebeam, Attack.LongDash, Attack.Flamethrower1, Attack.Flamethrower2, Attack.MeteorSmash, Attack.PunchUppercut, Attack.ShotgunBlast, Attack.SpinAttack, Attack.ShortDash);
            m_attackRangeCache = new List<float>();
            AddToRangeCache(m_info.firebeamAttack.range, m_info.longDashAttack.range, m_info.flameBeamAttack.range, m_info.flameBeamAttack.range, m_info.meteorAttack.range, m_info.punchAttack.range, m_info.shotgunBlastFireAttack.range, m_info.spinAttack.range, m_info.shortDashAttack.range);

            /*AddToAttackAndRangeCaches(
                Attack.Firebeam, m_info.firebeamAttack.range,
                Attack.LongDash, m_info.longDashAttack.range,
                Attack.Flamethrower1, m_info.flameThrowerAttack.range,
                Attack.Flamethrower2, m_info.flameThrowerAttack.range,
                Attack.MeteorSmash, m_info.meteorAttack.range,
                Attack.PunchUppercut, m_info.punchAttack.range,
                Attack.ShotgunBlast, m_info.shotgunBlastFireAttack.range,
                Attack.SpinAttack, m_info.spinAttack.range,
                Attack.ShortDash, m_info.shortDashAttack.range
            );*/
            m_attackUsed = new bool[m_attackCache.Count];
        }

        protected override void Start()
        {
            base.Start();
            m_aimRoutine = AimRoutine();
            m_spineListener.Subscribe(m_info.punchUppercutEvent, PunchAttack);
            m_spineListener.Subscribe(m_info.flamethrower1Event, Flamethrower1Attack);
            m_spineListener.Subscribe(m_info.overchargedPunchUppercutEvent, OvercahrgedPunchAttack);
            m_spineListener.Subscribe(m_info.overchargedFlamethrower1Event, OverchargedFlamethrower1Attack);
            m_animation.DisableRootMotion();
            m_phaseHandle = new PhaseHandle<Phase, PhaseInfo>();
            m_phaseHandle.Initialize(Phase.PhaseOne, m_info.phaseInfo, m_character, ChangeState, ApplyPhaseData);
            m_phaseHandle.ApplyChange();
            //m_patternCount = new float[m_currentPhaseIndex == 1? 2 : 4];
        }

        private void PunchAttack()
        {
            if (!m_isRaging)
            {
                m_punchAttackCollider.enabled = true;
            }
            //throw new System.NotImplementedException();
        }
        private void Flamethrower1Attack()
        {
            if (!m_isRaging)
            {
                m_flamethrower1Collider.enabled = true;
            }
        }
        private void OvercahrgedPunchAttack()
        {
            if (m_isRaging)
            {
                m_overchargedPunchAttackCollider.enabled = true;
            }
            //throw new System.NotImplementedException();
        }
        private void OverchargedFlamethrower1Attack()
        {
            if (m_isRaging)
            {
                m_overchargedFlamethrower1Collider.enabled = true;
            }
        }
        private int counter;
        [SerializeField]
        private float timeLeft = 0;
        private IEnumerator CounterForRageRoutine()
        {
            var duration = 10f;
            while (duration > timeLeft && checker)
            {
                timeLeft += Time.deltaTime;
                if (timeLeft >= duration)
                {
                    timeLeft = 0f;
                    checker = false;
                }
                if(timeLeft == 0)
                {
                    m_heatHandler.HandleDamageTaken(DamageType.Fire);
                }
                yield return null;
            }
            yield return null;
        }
        private IEnumerator OnRageCounter()
        {
            /*m_punchAttacker.SetDamageModifier(2);
            m_flamethrower1.SetDamageModifier(2);
            m_flamethrower2.SetDamageModifier(2);
            m_firebeam.SetDamageModifier(2);
            m_longD.SetDamageModifier(2);
            m_meteor.SetDamageModifier(2);
            m_shotG.SetDamageModifier(2);*/
            m_hitbox.SetInvulnerability(Invulnerability.None);
            var elapsedTime = 0f;
            var rageDuration = 10f;
            while (m_isRaging && rageDuration > elapsedTime)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime >= rageDuration)
                {
                    elapsedTime = 0f;
                    m_isRaging = false;
                    m_hasMalfactioned = true;
                    m_steamMalfAndOver.Stop();
                    m_heatHandler.ResetHeat();/*
                    m_punchAttacker.SetDamageModifier(1);
                    m_flamethrower1.SetDamageModifier(1);
                    m_flamethrower2.SetDamageModifier(1);
                    m_firebeam.SetDamageModifier(1);
                    m_longD.SetDamageModifier(1);
                    m_meteor.SetDamageModifier(1);
                    m_shotG.SetDamageModifier(1);*/
                    /*damage = 0;
                    ligthVisuals.GetComponent<CinderBoltHeatLightsReaction>().HandleReaction(damage);*/
                }
                yield return null;
            }
            yield return null;
        }
        private IEnumerator OnMlfunctionedRoutine()
        {
            enabled = false;
            m_stateHandle.Wait(State.Chasing);
            yield return new WaitForSeconds(0.5f);
            m_punchAttacker.SetActive(true);
            m_punchAttacker2.SetActive(true);
            m_overchargedPunchAttacker.SetActive(false);
            m_overchargedPunchAttacker2.SetActive(false);
            m_flamethrower1.SetActive(true);
            m_overchargedFlamethrower1.SetActive(false);
            m_firebeam.SetActive(true);
            m_overchargedFirebeam.SetActive(false);
            m_spinAttacker.SetActive(true);
            m_overchargedSpinAttacker.SetActive(false);
            m_longD.SetActive(true);
            m_overchargedLongD.SetActive(false);
            m_shotG.SetActive(true);
            m_overchargedShotG.SetActive(true);
            m_meteor.SetActive(true);
            m_overchargedMeteor.SetActive(false);
            m_flamethrower2.SetActive(true);
            m_overchargedFlamethrower2.SetActive(false);
            m_hasMalfactioned = false;
            m_steamMalfAndOver.Play();
            m_movement.Stop();
            m_firebeamAnticipationFX.Stop();
            m_muzzleLoopFX.Stop();
            m_laserOriginMuzzleFX.Stop();
            m_flamethrower1FX.Stop();
            m_flamethrower2FX.Stop();
            m_laserOriginMuzzleFX.Stop();
            m_muzzleTelegraphFX.Stop();
            m_longDashFX.Stop();
            m_meteorSmashFX.Stop();
            m_meteorSmashTrailFX.SetActive(false);
            m_muzzleLoopFX.Stop();
            m_shortDashFX.Stop();
            m_spinAttackFX.Stop();
            m_punchAttackCollider.enabled = false;
            m_flamethrower2Colliders.enabled = false;
            m_firebeamCollider.enabled = false;
            m_overchargedFirebeamCollider.enabled = false;
            m_longDashCollider.enabled = false;
            m_flamethrower2Colliders.enabled = false;
            m_meteorSmashCollider.enabled = false;
            m_spinAttackCollider[0].enabled = false;
            m_spinAttackCollider[1].enabled = false;
            m_animation.SetAnimation(0, m_info.malfunctionStateAnimation, false);
            yield return new WaitForSeconds(1.5f);
            m_animation.SetAnimation(0, m_info.malfunctionStateIdleAnimation, true);
            yield return new WaitForSeconds(5f);
            m_animation.SetAnimation(0, m_info.malfunctionRecoveryStateAnimation, false);
            yield return new WaitForSeconds(1f);
            RecoveryFX.SetActive(true);
            m_recoveryDamageCollider[0].enabled = true;
            m_recoveryDamageCollider[1].enabled = true;
            yield return new WaitForSeconds(1f);
            RecoveryFX.SetActive(false);/*
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.malfunctionRecoveryStateAnimation);*/
            m_recoveryDamageCollider[0].enabled = false;
            m_recoveryDamageCollider[1].enabled = false;
            m_steamMalfAndOver.Stop();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_heatHandler.ResetHeat();
            //GetComponent<CinderBoltHeatGauge>().AddHeat(0);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            DecidedOnAttack(false);
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
            enabled = true;
        }
        private IEnumerator OnRuneShieldRoutine()
        {
            m_runeDuration = m_phaseHandle.currentPhase == Phase.PhaseOne ? 5 : 8;
            m_runeShieldFX.SetActive(true);
            yield return new WaitForSeconds(m_runeDuration);
            m_runeShieldFX.SetActive(false);
            m_runeShieldBreakFX.SetActive(true);
            yield return new WaitForSeconds(1f);
            m_runeShieldBreakFX.SetActive(false);
            m_hasRune = false;
            yield return null;
        }
        public GameObject ligthVisuals;
        public bool checker = false;
        [SerializeField]
        private BasicAttackResistance m_basicAttackResistance;
        [SerializeField]
        private AttackResistanceData m_attackResistanceData;
        private void CinderBoltAI_DamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
        {
            Debug.Log("Heat Gauge value is: " + GetComponent<CinderBoltHeatGauge>().currentValue);
            if (eventArgs.type == DamageType.Fire)
            {
                checker = true;
                counter += 1;
                if (counter == 2)
                {
                    m_hasRune = true;
                    counter = 0;
                }
                if (m_hasRune)
                {
                    m_basicAttackResistance.SetData(m_attackResistanceData);
                }
                else
                {
                    m_basicAttackResistance.ClearResistance();
                }
                StartCoroutine(CounterForRageRoutine());
            }
            //throw new NotImplementedException();
        }
        private void Update()
        {
            ligthVisuals.GetComponent<CinderBoltHeatLightsReaction>().HandleReaction(GetComponent<CinderBoltHeatGauge>().currentValue);
            if (GetComponent<CinderBoltHeatGauge>().currentValue == 90)
            {
                m_isRaging = true;
            }
            if (m_isRaging)
            {
                m_punchAttacker.SetActive(false);
                m_punchAttacker2.SetActive(false);
                m_overchargedPunchAttacker.SetActive(true);
                m_overchargedPunchAttacker2.SetActive(true);
                m_flamethrower1.SetActive(false);
                m_overchargedFlamethrower1.SetActive(true);
                m_spinAttacker.SetActive(false);
                m_overchargedSpinAttacker.SetActive(true);
                m_firebeam.SetActive(false);
                m_overchargedFirebeam.SetActive(true);
                m_longD.SetActive(false);
                m_overchargedLongD.SetActive(true);
                m_shotG.SetActive(true);
                m_overchargedShotG.SetActive(true);
                m_meteor.SetActive(false);
                m_overchargedMeteor.SetActive(true);
                m_flamethrower2.SetActive(false);
                m_overchargedFlamethrower2.SetActive(true);
                m_steamMalfAndOver.Play();
                StartCoroutine(OnRageCounter());
            }
            if (m_hasMalfactioned)
            {
                StopAllCoroutines();
                StartCoroutine(OnMlfunctionedRoutine());
                return;
            }
            if (m_hasRune)
            {
                StartCoroutine(OnRuneShieldRoutine());
            }
            //m_basicAttackResistance.SetData(m_attackResistanceData);
            m_phaseHandle.MonitorPhase();
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    //m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    break;
                case State.Intro:
                    /*if (IsFacingTarget())
                    {*/
                    //enabled = true;
                    StartCoroutine(IntroRoutine());
                    /*}
                    else
                    {
                        CustomTurn();
                    }*/
                    break;
                /*case State.Malfunction:
                    StartCoroutine(OnMlfunctionedRoutine());
                    enabled = true;
                    break;*/
                case State.Phasing:
                    StartCoroutine(ChangePhaseRoutine());
                    break;
                case State.Turning:
                    m_phaseHandle.allowPhaseChange = false;
                    m_stateHandle.Wait(m_turnState);
                    if (!m_isRaging)
                    {
                        m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                    }
                    else
                    {
                        m_turnHandle.Execute(m_info.turnAnimation, m_info.overchargedIdle);
                    }
                    m_movement.Stop();
                    break;
                case State.Attacking:
                    m_hitbox.SetInvulnerability(Invulnerability.None);
                    if (IsFacingTarget())
                    {
                        switch (m_chosenPattern)
                        {
                            case Pattern.Phase1Pattern1:
                                if (m_phaseHandle.currentPhase == Phase.PhaseOne)
                                {
                                    Debug.Log("Phase 1 Pattern 1");
                                    UpdateRangeCache(m_info.punchAttack.range, m_info.flameThrowerAttack.range);
                                    ExecuteAttack(1);
                                }
                                break;
                            case Pattern.Phase1Pattern2:
                                if (m_phaseHandle.currentPhase == Phase.PhaseOne)
                                {
                                    Debug.Log("Phase 1 Pattern 2");
                                    UpdateRangeCache(m_info.punchAttack.range, m_info.flameThrowerAttack.range, m_info.spinAttack.range, m_info.firebeamAttack.range, m_info.longDashAttack.range);
                                    ExecuteAttack(2);
                                }
                                break;
                            case Pattern.Phase2Pattern1:
                                if (m_phaseHandle.currentPhase == Phase.PhaseTwo)
                                {
                                    Debug.Log("Phase 2 Pattern 1");
                                    UpdateRangeCache(m_info.punchAttack.range, m_info.flameThrowerAttack.range, m_info.spinAttack.range, m_info.shotgunBlastFireAttack.range, m_info.flameThrowerAttack.range, m_info.shortDashAttack.range);//change the flamethrower to flamethrower2range
                                    ExecuteAttack(3);
                                }
                                break;
                            case Pattern.Phase2Pattern2:
                                if (m_phaseHandle.currentPhase == Phase.PhaseTwo)
                                {
                                    Debug.Log("Phase 2 Pattern 2");
                                    UpdateRangeCache(m_info.punchAttack.range, m_info.spinAttack.range, m_info.shotgunBlastFireAttack.range, m_info.longDashAttack.range);
                                    ExecuteAttack(4);
                                }
                                break;
                            case Pattern.Phase2Pattern3:
                                if (m_phaseHandle.currentPhase == Phase.PhaseTwo)
                                {
                                    Debug.Log("Phase 2 Pattern 3");
                                    UpdateRangeCache(m_info.spinAttack.range, m_info.firebeamAttack.range, m_info.meteorAttack.range);
                                    ExecuteAttack(5);
                                }
                                break;
                            case Pattern.Phase2Pattern4:
                                if (m_phaseHandle.currentPhase == Phase.PhaseTwo)
                                {
                                    Debug.Log("Phase 2 Pattern 4");
                                    UpdateRangeCache(m_info.firebeamAttack.range, m_info.flameThrowerAttack.range);//change the flamethrower to flamethrower2range
                                    ExecuteAttack(6);
                                }
                                break;
                        }
                    }
                    else
                    {
                        m_turnState = State.Attacking;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation/* || m_animation.GetCurrentAnimation(0).ToString() != m_info.overchargedTurn*/)
                            m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.Chasing:
                    enabled = false;
                    //m_attackCount = 0;
                    /*if (IsFacingTarget())
                    {*/
                    //DecidedOnAttack(false);
                    m_patternDecider.DecideOnAttack();
                    m_chosenPattern = m_patternDecider.chosenAttack.attack;
                    if (m_chosenPattern == m_previousPattern)
                    {
                        m_patternDecider.hasDecidedOnAttack = false;
                    }
                    if (m_patternDecider.hasDecidedOnAttack)
                    {
                        m_previousPattern = m_chosenPattern;
                        m_stateHandle.SetState(State.Attacking);
                    }
                    /*}
                    else
                    {
                        CustomTurn();
                        MoveToTarget(m_patternDecider.chosenAttack.range);
                    }*/
                    enabled = true;
                    break;

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
        }

        public override void ReturnToSpawnPoint()
        {
        }

        protected override void OnForbidFromAttackTarget()
        {
        }
    }
}