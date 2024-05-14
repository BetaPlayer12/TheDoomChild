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
using DG.Tweening;
using Unity.Mathematics;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Boss/TheOneFirstForm")]
    public class TheOneFirstFormAI : CombatAIBrain<TheOneFirstFormAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [TitleGroup("Phase Info")]

            [SerializeField]
            private PhaseInfo<Phase> m_phaseInfo;
            public PhaseInfo<Phase> phaseInfo => m_phaseInfo;

            [TitleGroup("Movement Behaviours")]
            [SerializeField]
            private MovementInfo m_walk = new MovementInfo();
            public MovementInfo walk => m_walk;

            [TitleGroup("Attack Behaviours")]

            [SerializeField, BoxGroup("Downward Slash 1")]
            private SimpleAttackInfo m_downwardSlash1Attack = new SimpleAttackInfo();
            public SimpleAttackInfo downwardSlash1Attack => m_downwardSlash1Attack;
            [SerializeField, BoxGroup("Downward Slash 2")]
            private SimpleAttackInfo m_downwardSlash2Attack = new SimpleAttackInfo();
            public SimpleAttackInfo downwardSlash2Attack => m_downwardSlash2Attack;
            [SerializeField, BoxGroup("Sword Stab")]
            private SimpleAttackInfo m_swordStabAttack = new SimpleAttackInfo();
            public SimpleAttackInfo swordStabAttack => m_swordStabAttack;
            [SerializeField, BoxGroup("Heavy Sword Stab")]
            private SimpleAttackInfo m_heavySwordStabAttack = new SimpleAttackInfo();
            public SimpleAttackInfo heavySwordStabAttack => m_heavySwordStabAttack;
            [SerializeField, BoxGroup("Twin Slash 1")]
            private SimpleAttackInfo m_twinSlash1Attack = new SimpleAttackInfo();
            public SimpleAttackInfo twinSlash1Attack => m_twinSlash1Attack;
            [SerializeField, BoxGroup("Twin Slash 2")]
            private SimpleAttackInfo m_twinSlash2Attack = new SimpleAttackInfo();
            public SimpleAttackInfo twinSlash2Attack => m_twinSlash2Attack;
            [SerializeField, BoxGroup("Drill Dash 1")]
            private SimpleAttackInfo m_drillDash1Attack = new SimpleAttackInfo();
            public SimpleAttackInfo drillDash1Attack => m_drillDash1Attack;
            [SerializeField, BoxGroup("Drill Dash 2")]
            private SimpleAttackInfo m_drillDash2Attack = new SimpleAttackInfo();
            public SimpleAttackInfo drillDash2Attack => m_drillDash2Attack;
            [SerializeField, BoxGroup("ProjectileWaveSlash")]
            private SimpleAttackInfo m_projectilWaveSlashGround1Attack = new SimpleAttackInfo();
            public SimpleAttackInfo projectilWaveSlashGround1Attack => m_projectilWaveSlashGround1Attack;
            [SerializeField, BoxGroup("ProjectileWaveSlash")]
            private SimpleAttackInfo m_projectilWaveSlashGround2Attack = new SimpleAttackInfo();
            public SimpleAttackInfo projectilWaveSlashGround2Attack => m_projectilWaveSlashGround2Attack;
            [SerializeField, BoxGroup("ProjectileWaveSlash")]
            private SimpleAttackInfo m_projectilWaveSlashMidAir1Attack = new SimpleAttackInfo();
            public SimpleAttackInfo projectilWaveSlashMidAir1Attack => m_projectilWaveSlashMidAir1Attack;
            [SerializeField, BoxGroup("ProjectileWaveSlash")]
            private SimpleAttackInfo m_projectilWaveSlashMidAir2Attack = new SimpleAttackInfo();
            public SimpleAttackInfo projectilWaveSlashMidAir2Attack => m_projectilWaveSlashMidAir2Attack;
            [SerializeField, BoxGroup("ProjectileWaveSlash")]
            private SimpleAttackInfo m_scytheWaveAttack = new SimpleAttackInfo();
            public SimpleAttackInfo scytheWaveAttack => m_scytheWaveAttack;
            [SerializeField, BoxGroup("ProjectileWaveSlash")]
            private SimpleAttackInfo m_scytheDoubleWaveAttack = new SimpleAttackInfo();
            public SimpleAttackInfo scytheDoubleWaveAttack => m_scytheDoubleWaveAttack;
            [SerializeField, BoxGroup("GeyserBurst")]
            private SimpleAttackInfo m_geyserBurstGreenAttack = new SimpleAttackInfo();
            public SimpleAttackInfo geyserBurstGreenAttack => m_geyserBurstGreenAttack;
            [SerializeField, BoxGroup("GeyserBurst")]
            private SimpleAttackInfo m_geyserBurstPurpleAttack = new SimpleAttackInfo();
            public SimpleAttackInfo geyserBurstPurpleAttack => m_geyserBurstPurpleAttack;
            [SerializeField, BoxGroup("GeyserBurst")]
            private SimpleAttackInfo m_geyserBurstRedAttack = new SimpleAttackInfo();
            public SimpleAttackInfo geyserBurstRedAttack => m_geyserBurstRedAttack;

            [TitleGroup("Attack Cooldown States")]
            [SerializeField, MinValue(0)]
            private List<float> m_phase1PatternCooldown;
            public List<float> phase1PatternCooldown => m_phase1PatternCooldown;
            [SerializeField, MinValue(0)]
            private List<float> m_phase2PatternCooldown;
            public List<float> phase2PatternCooldown => m_phase2PatternCooldown;
            [SerializeField, MinValue(0)]
            private float m_normalBladeCooldown;
            public float normalBladeCooldown => m_normalBladeCooldown;
            [SerializeField, MinValue(0)]
            private float m_alterBladeCooldown;
            public float alterBladeCooldown => m_alterBladeCooldown;
            //[SerializeField, MinValue(0), BoxGroup("Phase 1")]
            //private float m_phase1Pattern1CD;
            //public float phase1Pattern1CD => m_phase1Pattern1CD;
            //[SerializeField, MinValue(0), BoxGroup("Phase 1")]
            //private float m_phase1Pattern2CD;
            //public float phase1Pattern2CD => m_phase1Pattern2CD;
            //[SerializeField, MinValue(0), BoxGroup("Phase 1")]
            //private float m_phase1Pattern3CD;
            //public float phase1Pattern3CD => m_phase1Pattern3CD;
            //[SerializeField, MinValue(0), BoxGroup("Phase 1")]
            //private float m_phase1Pattern4CD;
            //public float phase1Pattern4CD => m_phase1Pattern4CD;

            [TitleGroup("Attack Colors")]
            [SerializeField]
            private BasicAnimationInfo m_drillNormalMixAnimation;
            public BasicAnimationInfo drillNormalMixAnimation => m_drillNormalMixAnimation;
            [SerializeField]
            private BasicAnimationInfo m_drillGreenMixAnimation;
            public BasicAnimationInfo drillGreenMixAnimation => m_drillGreenMixAnimation;
            [SerializeField]
            private BasicAnimationInfo m_drillPurpleMixAnimation;
            public BasicAnimationInfo drillPurpleMixAnimation => m_drillPurpleMixAnimation;
            [SerializeField]
            private BasicAnimationInfo m_drillRedMixAnimation;
            public BasicAnimationInfo drillRedMixAnimation => m_drillRedMixAnimation;
            [SerializeField]
            private BasicAnimationInfo m_swordNormalMixAnimation;
            public BasicAnimationInfo swordNormalMixAnimation => m_swordNormalMixAnimation;
            [SerializeField]
            private BasicAnimationInfo m_swordGreenMixAnimation;
            public BasicAnimationInfo swordGreenMixAnimation => m_swordGreenMixAnimation;
            [SerializeField]
            private BasicAnimationInfo m_swordPurpleMixAnimation;
            public BasicAnimationInfo swordPurpleMixAnimation => m_swordPurpleMixAnimation;
            [SerializeField]
            private BasicAnimationInfo m_swordRedMixAnimation;
            public BasicAnimationInfo swordRedMixAnimation => m_swordRedMixAnimation;

            [TitleGroup("Ability Behaviours")]
            [SerializeField, BoxGroup("Blink")]
            private float m_blinkDuration;
            public float blinkDuration => m_blinkDuration;
            [SerializeField, BoxGroup("Blink")]
            private float m_fakeBlinkCount;
            public float fakeBlinkCount => m_fakeBlinkCount;
            [SerializeField, BoxGroup("Blink")]
            private BasicAnimationInfo m_blinkAppearBackwardAnimation;
            public BasicAnimationInfo blinkAppearBackwardAnimation => m_blinkAppearBackwardAnimation;
            [SerializeField, BoxGroup("Blink")]
            private BasicAnimationInfo m_blinkAppearForwardAnimation;
            public BasicAnimationInfo blinkAppearForwardAnimation => m_blinkAppearForwardAnimation;
            [SerializeField, BoxGroup("Blink")]
            private BasicAnimationInfo m_blinkAppearUpwardAnimation;
            public BasicAnimationInfo blinkAppearUpwardAnimation => m_blinkAppearUpwardAnimation;
            [SerializeField, BoxGroup("Blink")]
            private BasicAnimationInfo m_blinkDisappearBackwardAnimation;
            public BasicAnimationInfo blinkDisappearBackwardAnimation => m_blinkDisappearBackwardAnimation;
            [SerializeField, BoxGroup("Blink")]
            private BasicAnimationInfo m_blinkDisappearForwardAnimation;
            public BasicAnimationInfo blinkDisappearForwardAnimation => m_blinkDisappearForwardAnimation;
            [SerializeField, BoxGroup("Blink")]
            private BasicAnimationInfo m_blinkDisappearUpwardAnimation;
            public BasicAnimationInfo blinkDisappearUpwardAnimation => m_blinkDisappearUpwardAnimation;
            [SerializeField, BoxGroup("Blink")]
            private BasicAnimationInfo m_blinkFakeAnimation;
            public BasicAnimationInfo blinkFakeAnimation => m_blinkFakeAnimation;
            [SerializeField, BoxGroup("Drill Dash")]
            private float m_drillDashSpeed;
            public float drillDashSpeed => m_drillDashSpeed;
            [SerializeField, BoxGroup("Sword Change")]
            private BasicAnimationInfo m_swordChangeAnimation;
            public BasicAnimationInfo swordChangeAnimation => m_swordChangeAnimation;
            [SerializeField, BoxGroup("Summon Swords")]
            private BasicAnimationInfo m_summonSwordsAnimation;
            public BasicAnimationInfo summonSwordsAnimation => m_summonSwordsAnimation;


            [TitleGroup("Pattern Ranges")]
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
            [SerializeField, BoxGroup("Phase 1")]
            private float m_phase2Pattern1Range;
            public float phase2Pattern1Range => m_phase2Pattern1Range;
            [SerializeField, BoxGroup("Phase 1")]
            private float m_phase2Pattern2Range;
            public float phase2Pattern2Range => m_phase2Pattern2Range;
            [SerializeField, BoxGroup("Phase 1")]
            private float m_phase2Pattern3Range;
            public float phase2Pattern3Range => m_phase2Pattern3Range;
            [SerializeField, BoxGroup("Phase 1")]
            private float m_phase2Pattern4Range;
            public float phase2Pattern4Range => m_phase2Pattern4Range;
            [SerializeField, BoxGroup("Phase 1")]
            private float m_phase2Pattern5Range;
            public float phase2Pattern5Range => m_phase2Pattern5Range;

            [TitleGroup("Misc")]
            [SerializeField]
            private float m_phaseChangeToBlinkDelay;
            public float phaseChangeToBlinkDelay => m_phaseChangeToBlinkDelay;
            [SerializeField]
            private float m_phase1Pattern2WalkTime;
            public float phase1Pattern2WalkTime => m_phase1Pattern2WalkTime;
            [SerializeField]
            private float m_phase1Pattern3IdleTime;
            public float phase1Pattern3IdleTime => m_phase1Pattern3IdleTime;
            [SerializeField]
            private float m_midAirHeight;
            public float midAirHeight => m_midAirHeight;
            [SerializeField]
            private int m_staggerHitCount;
            public int staggerHitCount => m_staggerHitCount;
            [SerializeField]
            private int m_drillDashHitCount;
            public int drillDashHitCount => m_drillDashHitCount;
            [SerializeField]
            private int m_fakeBlinkHitCount;
            public int fakeBlinkHitCount => m_fakeBlinkHitCount;

            [TitleGroup("Animations")]
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleCombatAnimation;
            public BasicAnimationInfo idleCombatAnimation => m_idleCombatAnimation;
            [SerializeField]
            private BasicAnimationInfo m_drillToGroundAnimation;
            public BasicAnimationInfo drillToGroundAnimation => m_drillToGroundAnimation;
            [SerializeField]
            private BasicAnimationInfo m_groundToDrillAnimation;
            public BasicAnimationInfo groundToDrillAnimation => m_groundToDrillAnimation;
            [SerializeField]
            private BasicAnimationInfo m_fallAnimation;
            public BasicAnimationInfo fallAnimation => m_fallAnimation;
            [SerializeField]
            private BasicAnimationInfo m_landAnimation;
            public BasicAnimationInfo landAnimation => m_landAnimation;
            [SerializeField]
            private BasicAnimationInfo m_staggerAnimation;
            public BasicAnimationInfo staggerAnimation => m_staggerAnimation;
            [SerializeField]
            private BasicAnimationInfo m_staggerWithKnockbackAnimation;
            public BasicAnimationInfo staggerWithKnockbackAnimation => m_staggerWithKnockbackAnimation;
            [SerializeField]
            private BasicAnimationInfo m_defStaggerWithKnockbackAnimation;
            public BasicAnimationInfo defStaggerWithKnockbackAnimation => m_defStaggerWithKnockbackAnimation;
            [SerializeField]
            private BasicAnimationInfo m_defeated1Animation;
            public BasicAnimationInfo defeated1Animation => m_defeated1Animation;
            [SerializeField]
            private BasicAnimationInfo m_defeated2Animation;
            public BasicAnimationInfo defeated2Animation => m_defeated2Animation;
            [SerializeField]
            private BasicAnimationInfo m_defeated3Animation;
            public BasicAnimationInfo defeated3Animation => m_defeated3Animation;
            [SerializeField]
            private BasicAnimationInfo m_defeated4Animation;
            public BasicAnimationInfo defeated4Animation => m_defeated4Animation;

            [Title("Projectiles")]
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;
            [SerializeField, BoxGroup("Slash Projectile")]
            private SimpleProjectileAttackInfo m_slashNormalProjectile;
            public SimpleProjectileAttackInfo slashNormalProjectile => m_slashNormalProjectile;
            [SerializeField, BoxGroup("Slash Projectile")]
            private SimpleProjectileAttackInfo m_slashBlackbloodProjectile;
            public SimpleProjectileAttackInfo slashBlackbloodProjectile => m_slashBlackbloodProjectile;
            [SerializeField, BoxGroup("Slash Projectile")]
            private SimpleProjectileAttackInfo m_slashPoisonProjectile;
            public SimpleProjectileAttackInfo slashPoisonProjectile => m_slashPoisonProjectile;
            [SerializeField, BoxGroup("Slash Projectile")]
            private SimpleProjectileAttackInfo m_slashAcidProjectile;
            public SimpleProjectileAttackInfo slashAcidProjectile => m_slashAcidProjectile;
            [SerializeField, BoxGroup("Scythe Wave Projectile")]
            private SimpleProjectileAttackInfo m_scytheWaveNormalProjectile;
            public SimpleProjectileAttackInfo scytheWaveNormalProjectile => m_scytheWaveNormalProjectile;
            [SerializeField, BoxGroup("Scythe Wave Projectile")]
            private SimpleProjectileAttackInfo m_scytheWaveBlackbloodProjectile;
            public SimpleProjectileAttackInfo scytheWaveBlackbloodProjectile => m_scytheWaveNormalProjectile;
            [SerializeField, BoxGroup("Scythe Wave Projectile")]
            private SimpleProjectileAttackInfo m_scytheWavePoisonProjectile;
            public SimpleProjectileAttackInfo scytheWavePoisonProjectile => m_scytheWavePoisonProjectile;
            [SerializeField, BoxGroup("Scythe Wave Projectile")]
            private SimpleProjectileAttackInfo m_scytheWaveAcidProjectile;
            public SimpleProjectileAttackInfo scytheWaveAcidProjectile => m_scytheWaveAcidProjectile;

            [Title("Spawnable Objects")]
            [SerializeField, BoxGroup("Geyser Prefabs")]
            private GameObject m_geyserGreen;
            public GameObject geyserGreen => m_geyserGreen;
            [SerializeField, BoxGroup("Geyser Prefabs")]
            private GameObject m_geyserPurple;
            public GameObject geyserPurple => m_geyserPurple;
            [SerializeField, BoxGroup("Geyser Prefabs")]
            private GameObject m_geyserRed;
            public GameObject geyserRed => m_geyserRed;

            [TitleGroup("FX")]
            [SerializeField]
            private GameObject m_fx;
            public GameObject fx => m_fx;

            [TitleGroup("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_dustLandEvent;
            public string dustLandEvent => m_dustLandEvent;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_walk.SetData(m_skeletonDataAsset);
                m_downwardSlash1Attack.SetData(m_skeletonDataAsset);
                m_downwardSlash2Attack.SetData(m_skeletonDataAsset);
                m_swordStabAttack.SetData(m_skeletonDataAsset);
                m_heavySwordStabAttack.SetData(m_skeletonDataAsset);
                m_twinSlash1Attack.SetData(m_skeletonDataAsset);
                m_twinSlash2Attack.SetData(m_skeletonDataAsset);
                m_drillDash1Attack.SetData(m_skeletonDataAsset);
                m_drillDash2Attack.SetData(m_skeletonDataAsset);
                m_projectilWaveSlashGround1Attack.SetData(m_skeletonDataAsset);
                m_projectilWaveSlashGround2Attack.SetData(m_skeletonDataAsset);
                m_projectilWaveSlashMidAir1Attack.SetData(m_skeletonDataAsset);
                m_projectilWaveSlashMidAir2Attack.SetData(m_skeletonDataAsset);
                m_scytheWaveAttack.SetData(m_skeletonDataAsset);
                m_scytheDoubleWaveAttack.SetData(m_skeletonDataAsset);
                m_geyserBurstGreenAttack.SetData(m_skeletonDataAsset);
                m_geyserBurstPurpleAttack.SetData(m_skeletonDataAsset);
                m_geyserBurstRedAttack.SetData(m_skeletonDataAsset);
                m_slashNormalProjectile.SetData(m_skeletonDataAsset);
                m_slashBlackbloodProjectile.SetData(m_skeletonDataAsset);
                m_slashPoisonProjectile.SetData(m_skeletonDataAsset);
                m_slashAcidProjectile.SetData(m_skeletonDataAsset);
                m_scytheWaveNormalProjectile.SetData(m_skeletonDataAsset);
                m_scytheWaveBlackbloodProjectile.SetData(m_skeletonDataAsset);
                m_scytheWavePoisonProjectile.SetData(m_skeletonDataAsset);
                m_scytheWaveAcidProjectile.SetData(m_skeletonDataAsset);


                m_drillNormalMixAnimation.SetData(m_skeletonDataAsset);
                m_drillGreenMixAnimation.SetData(m_skeletonDataAsset);
                m_drillPurpleMixAnimation.SetData(m_skeletonDataAsset);
                m_drillRedMixAnimation.SetData(m_skeletonDataAsset);
                m_swordNormalMixAnimation.SetData(m_skeletonDataAsset);
                m_swordGreenMixAnimation.SetData(m_skeletonDataAsset);
                m_swordPurpleMixAnimation.SetData(m_skeletonDataAsset);
                m_swordRedMixAnimation.SetData(m_skeletonDataAsset);
                m_blinkAppearBackwardAnimation.SetData(m_skeletonDataAsset);
                m_blinkAppearForwardAnimation.SetData(m_skeletonDataAsset);
                m_blinkAppearUpwardAnimation.SetData(m_skeletonDataAsset);
                m_blinkDisappearBackwardAnimation.SetData(m_skeletonDataAsset);
                m_blinkDisappearForwardAnimation.SetData(m_skeletonDataAsset);
                m_blinkDisappearUpwardAnimation.SetData(m_skeletonDataAsset);
                m_blinkFakeAnimation.SetData(m_skeletonDataAsset);
                m_swordChangeAnimation.SetData(m_skeletonDataAsset);
                m_summonSwordsAnimation.SetData(m_skeletonDataAsset);
                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_idleCombatAnimation.SetData(m_skeletonDataAsset);
                m_drillToGroundAnimation.SetData(m_skeletonDataAsset);
                m_groundToDrillAnimation.SetData(m_skeletonDataAsset);
                m_fallAnimation.SetData(m_skeletonDataAsset);
                m_landAnimation.SetData(m_skeletonDataAsset);
                m_staggerAnimation.SetData(m_skeletonDataAsset);
                m_staggerWithKnockbackAnimation.SetData(m_skeletonDataAsset);
                m_defStaggerWithKnockbackAnimation.SetData(m_skeletonDataAsset);
                m_defeated1Animation.SetData(m_skeletonDataAsset);
                m_defeated2Animation.SetData(m_skeletonDataAsset);
                m_defeated3Animation.SetData(m_skeletonDataAsset);
                m_defeated4Animation.SetData(m_skeletonDataAsset);

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
            //private List<float> m_patternCooldown;
            //public List<float> patternCooldown => m_patternCooldown;
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

        private enum SwordState
        {
            Normal,
            BlackBlood,
            Poison,
            Acid,
        }

        private enum BlinkState
        {
            AppearForward,
            AppearBackward,
            AppearUpward,
            DisappearForward,
            DisappearBackward,
            DisappearUpward,
        }

        //private enum Pattern
        //{
        //    AttackPattern1,
        //    AttackPattern2,
        //    AttackPattern3,
        //    WaitAttackEnd,
        //}

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
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_bodyCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_legCollider;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_model;
        //[SerializeField, TabGroup("Modules")]
        //private TransformTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Modules")]
        private TransformTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Hurtbox")]
        private Collider2D m_swordSlash1BB;

        [SerializeField, TabGroup("FX")]
        private ParticleFX m_blinkFX;

        [SerializeField, TabGroup("Spawn Points")]
        private Collider2D m_randomSpawnCollider;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_projectilePoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_scytheWavePoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_scytheWaveLeftSpawnPosition;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_scytheWaveRightSpawnPosition;
        [SerializeField, TabGroup("IK Control")]
        private SkeletonUtilityBone m_targetIK;
        [SerializeField, TabGroup("Geyser Pattern Spawn Points")]
        private Vector2[] m_geyserPatternOne;
        [SerializeField, TabGroup("Geyser Pattern Spawn Points")]
        private Vector2[] m_geyserPatternTwo;

        private ProjectileLauncher m_projectileLauncher;
        private ProjectileLauncher m_scytheWaveLauncher;

        [SerializeField]
        private SpineEventListener m_spineListener;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        State m_turnState;
        SwordState m_currentSwordState;
        SwordState m_cachedSwordState;
        [ShowInInspector]
        private PhaseHandle<Phase, PhaseInfo> m_phaseHandle;
        //[ShowInInspector]
        //private RandomAttackDecider<Pattern> m_patternDecider;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        //private Pattern m_chosenPattern;
        //private Pattern m_previousPattern;
        private Attack m_currentAttack;
        private float m_currentAttackRange;
        private int m_staggerCurrentHitCount;
        private int m_drillDashCurrentHitCount;
        private int m_fakeBlinkCurrentHitCount;

        private Coroutine m_currentAttackCoroutine;
        private Coroutine m_changePhaseCoroutine;
        private Coroutine m_blinkCoroutine;
        private Coroutine m_staggerCoroutine;
        private Coroutine m_drillDashCounterCoroutine;
        private Coroutine m_fakeBlinkRoutine;
        private Coroutine m_alterBladeMonitorCoroutine;
        private Coroutine m_alterBladeCoroutine;

        private Vector2 m_lastTargetPos;
        private float m_currentCooldown;
        private float m_pickedCooldown;
        private int m_blinkCount;
        private List<float> m_currentFullCooldown;
        private List<float> m_patternCooldown;

        #region PatternCounts
        private int m_phase2pattern1Count;
        private int m_phase2pattern2Count;
        private int m_phase2pattern5Count;
        private int m_fakeBlinkCount;
        private int m_fakeBlinkChosenDrillDashBehavior;
        private int m_drillDashComboCount;
        #endregion

        private bool m_isDetecting;

        #region Animation
        private string m_idleAnimation;
        private string m_blinkAppearAnimation;
        private string m_blinkDisappearAnimation;
        private string m_drillMixAnimation;
        private string m_swordMixAnimation;
        #endregion  

        private void ApplyPhaseData(PhaseInfo obj)
        {
            m_attackCache.Clear();
            m_attackRangeCache.Clear();
            if (m_patternCooldown.Count != 0)
                m_patternCooldown.Clear();
            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseOne:
                    m_idleAnimation = m_info.idleCombatAnimation.animation;
                    AddToAttackCache(Attack.Phase1Pattern1, Attack.Phase1Pattern2, Attack.Phase1Pattern3, Attack.Phase1Pattern4);
                    AddToRangeCache(m_info.phase1Pattern1Range, m_info.phase1Pattern2Range, m_info.phase1Pattern3Range, m_info.phase1Pattern4Range);
                    for (int i = 0; i < m_info.phase1PatternCooldown.Count; i++)
                        m_patternCooldown.Add(m_info.phase1PatternCooldown[i]);
                    break;
                case Phase.PhaseTwo:
                    m_idleAnimation = m_info.idleCombatAnimation.animation;
                    AddToAttackCache(Attack.Phase2Pattern1, Attack.Phase2Pattern2, Attack.Phase2Pattern3, Attack.Phase2Pattern4, Attack.Phase2Pattern5);
                    AddToRangeCache(m_info.phase2Pattern1Range, m_info.phase2Pattern2Range, m_info.phase2Pattern3Range, m_info.phase2Pattern4Range, m_info.phase2Pattern5Range);
                    for (int i = 0; i < m_info.phase2PatternCooldown.Count; i++)
                        m_patternCooldown.Add(m_info.phase2PatternCooldown[i]);
                    break;
            }
            m_attackUsed = new bool[m_attackCache.Count];
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
            //StopCurrentAttackRoutine();
            //SetAIToPhasing();
            StartCoroutine(SmartChangePhaseRoutine());
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
                    m_alterBladeMonitorCoroutine = StartCoroutine(AlterBladeMonitorRoutine());
                    m_stateHandle.OverrideState(State.Intro);
                    //GameEventMessage.SendEvent("Boss Encounter");
                }
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            if (m_currentAttackCoroutine == null)
            {
                if (m_stateHandle.currentState != State.Phasing)
                {
                    m_animation.animationState.TimeScale = 1f;
                    m_stateHandle.ApplyQueuedState();
                }
                m_phaseHandle.allowPhaseChange = true;
            }
        }

        private void OnDamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
        {
            if (m_alterBladeCoroutine == null && m_staggerCoroutine == null && m_drillDashCounterCoroutine == null && m_blinkCoroutine == null)
            {
                switch (m_phaseHandle.currentPhase)
                {
                    case Phase.PhaseOne:
                        var drillCounter = false;
                        if (m_staggerCurrentHitCount < m_info.staggerHitCount)
                            m_staggerCurrentHitCount++;
                        else
                        {
                            drillCounter = false;
                            m_hitbox.SetCanBlockDamageState(true);
                        }

                        if (m_drillDashCurrentHitCount < m_info.drillDashHitCount)
                            m_drillDashCurrentHitCount++;
                        else
                        {
                            drillCounter = true;
                            m_hitbox.SetCanBlockDamageState(true);
                        }

                        if (m_hitbox.canBlockDamage)
                        {
                            if (m_currentAttackCoroutine != null)
                            {
                                StopCoroutine(m_currentAttackCoroutine);
                                m_currentAttackCoroutine = null;
                                m_attackDecider.hasDecidedOnAttack = false;
                            }

                            StopComboCounts();
                            m_stateHandle.Wait(State.ReevaluateSituation);

                            switch (drillCounter)
                            {
                                case false:
                                    m_staggerCoroutine = StartCoroutine(StaggerRoutine());
                                    m_staggerCurrentHitCount = 0;
                                    break;
                                case true:
                                    m_drillDashCounterCoroutine = StartCoroutine(DrillDashCounterRoutine());
                                    m_drillDashCurrentHitCount = 0;
                                    break;
                            }

                        }
                        break;
                    case Phase.PhaseTwo:
                        if (m_fakeBlinkCurrentHitCount < m_info.fakeBlinkHitCount)
                            m_fakeBlinkCurrentHitCount++;
                        else
                        {
                            m_hitbox.SetCanBlockDamageState(true);
                        }

                        if (m_hitbox.canBlockDamage)
                        {
                            if (m_currentAttackCoroutine != null)
                            {
                                StopCoroutine(m_currentAttackCoroutine);
                                m_currentAttackCoroutine = null;
                                m_attackDecider.hasDecidedOnAttack = false;
                            }

                            StopComboCounts();
                            m_stateHandle.Wait(State.ReevaluateSituation);

                            m_fakeBlinkRoutine = StartCoroutine(FakeBlinkRoutine());
                            m_fakeBlinkCurrentHitCount = 0;
                        }
                        break;
                }
            }
        }

        private IEnumerator StaggerRoutine()
        {
            enabled = false;
            m_hitbox.Disable();
            if (!m_groundSensor.isDetecting)
            {
                m_animation.DisableRootMotion();
                m_animation.SetAnimation(0, m_info.fallAnimation, true);
                yield return new WaitUntil(() => m_groundSensor.isDetecting);
                m_animation.SetAnimation(0, m_info.landAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.landAnimation);
            }
            m_animation.EnableRootMotion(true, false);
            if (!IsFacingTarget())
                CustomTurn();

            m_animation.SetAnimation(0, m_info.staggerAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.staggerAnimation);
            m_hitbox.Enable();
            m_staggerCoroutine = null;
            m_hitbox.SetCanBlockDamageState(false);
            if (m_alterBladeCoroutine == null)
                m_stateHandle.ApplyQueuedState();
            yield return null;
            enabled = true;
        }
        [SerializeField]
        private GameObject m_drillDamage;
        private IEnumerator DrillDashCounterRoutine()
        {
            enabled = false;
            var drillCount = 0;
            if (!m_groundSensor.isDetecting)
            {
                m_animation.DisableRootMotion();
                m_animation.SetAnimation(0, m_info.fallAnimation, true);
                yield return new WaitUntil(() => m_groundSensor.isDetecting);
                m_animation.SetAnimation(0, m_info.landAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.landAnimation);
            }
            m_animation.EnableRootMotion(false, false);
            while (drillCount < 2)
            {
                m_animation.SetAnimation(0, m_info.groundToDrillAnimation, false);
                var waitTime = m_animation.animationState.GetCurrent(0).AnimationEnd * 0.75f;
                yield return new WaitForSeconds(waitTime);
                m_hitbox.Disable();
                m_animation.SetAnimation(4, m_drillMixAnimation, false);
                m_drillDamage.SetActive(true);
                m_character.physics.SetVelocity(m_info.drillDashSpeed * transform.localScale.x, 0);
                m_animation.SetAnimation(0, m_info.drillDash1Attack.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillDash1Attack.animation);
                m_animation.SetEmptyAnimation(4, 0);
                m_hitbox.Enable();
                m_movement.Stop();
                m_animation.SetAnimation(0, m_info.drillToGroundAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillToGroundAnimation);
                m_drillDamage.SetActive(false);
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                if (!IsFacingTarget())
                    CustomTurn();

                drillCount++;
                yield return null;
            }
            m_drillDashCounterCoroutine = null;
            m_hitbox.SetCanBlockDamageState(false);
            if (m_alterBladeCoroutine == null)
                m_stateHandle.ApplyQueuedState();
            yield return null;
            enabled = true;
        }

        private IEnumerator IntroRoutine()
        {
            m_stateHandle.Wait(State.Chasing);
            m_movement.Stop();
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            //m_cinematic.PlayCinematic(1, false);
            m_animation.animationState.TimeScale = 1;
            m_animation.EnableRootMotion(true, false);
            m_hitbox.Enable();
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator SmartChangePhaseRoutine()
        {
            yield return new WaitWhile(() => !m_phaseHandle.allowPhaseChange);
            StopCurrentBehaviorRoutine();
            StopComboCounts();
            ResetCounterCounts();
            SetAIToPhasing();
            yield return null;
        }

        private void SetAIToPhasing()
        {
            enabled = true;
            m_phaseHandle.ApplyChange();
            m_animation.DisableRootMotion();
            m_animation.SetEmptyAnimation(0, 0);
            m_stateHandle.OverrideState(State.Phasing);
        }

        private void StopCurrentBehaviorRoutine()
        {
            if (m_currentAttackCoroutine != null)
            {
                StopCoroutine(m_currentAttackCoroutine);
                m_currentAttackCoroutine = null;
                m_attackDecider.hasDecidedOnAttack = false;
            }
            if (m_staggerCoroutine != null)
            {
                StopCoroutine(m_staggerCoroutine);
                m_staggerCoroutine = null;
            }
            if (m_drillDashCounterCoroutine != null)
            {
                StopCoroutine(m_drillDashCounterCoroutine);
                m_drillDashCounterCoroutine = null;
            }
            if (m_fakeBlinkRoutine != null)
            {
                StopCoroutine(m_fakeBlinkRoutine);
                m_fakeBlinkRoutine = null;
            }
            if (m_blinkCoroutine != null)
            {
                StopCoroutine(m_blinkCoroutine);
                m_blinkCoroutine = null;
            }
            if (m_alterBladeMonitorCoroutine != null)
            {
                StopCoroutine(m_alterBladeMonitorCoroutine);
                m_alterBladeMonitorCoroutine = null;
            }
            if (m_alterBladeCoroutine != null)
            {
                StopCoroutine(m_alterBladeCoroutine);
                m_alterBladeCoroutine = null;
            }
        }

        private void StopComboCounts()
        {
            m_phase2pattern1Count = 0;
            m_phase2pattern2Count = 0;
            m_phase2pattern5Count = 0;
            m_fakeBlinkCount = 0;
            m_drillDashComboCount = 0;
        }

        private void ResetCounterCounts()
        {
            m_staggerCurrentHitCount = 0;
            m_drillDashCurrentHitCount = 0;
            m_fakeBlinkCurrentHitCount = 0;
        }

        private IEnumerator ChangePhaseRoutine()
        {
            enabled = false;
            m_stateHandle.Wait(State.Chasing);
            if (IsFacingTarget())
                CustomTurn();

            m_hitbox.Disable();
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.staggerAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.staggerAnimation);
            m_animation.SetAnimation(0, m_info.summonSwordsAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.summonSwordsAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_hitbox.Enable();
            m_hitbox.SetCanBlockDamageState(false);
            m_changePhaseCoroutine = null;
            yield return new WaitForSeconds(m_info.phaseChangeToBlinkDelay);
            m_alterBladeMonitorCoroutine = StartCoroutine(AlterBladeMonitorRoutine());
            m_blinkCoroutine = StartCoroutine(BlinkRoutine(BlinkState.DisappearForward, BlinkState.AppearForward, 25, m_info.midAirHeight, State.Chasing, true, false, false));
            yield return null;
            enabled = true;
        }
        #region Attacks

        private void LaunchProjectile()
        {
            if (!IsFacingTarget())
                CustomTurn();
            //switch (m_currentSwordState)
            //{
            //    case SwordState.Normal:
            //        m_projectileLauncher = new ProjectileLauncher(m_info.slashNormalProjectile.projectileInfo, m_projectilePoint);
            //        break;
            //    case SwordState.BlackBlood:
            //        m_projectileLauncher = new ProjectileLauncher(m_info.slashBlackbloodProjectile.projectileInfo, m_projectilePoint);
            //        break;
            //    case SwordState.Poison:
            //        m_projectileLauncher = new ProjectileLauncher(m_info.slashPoisonProjectile.projectileInfo, m_projectilePoint);
            //        break;
            //    case SwordState.Acid:
            //        m_projectileLauncher = new ProjectileLauncher(m_info.slashAcidProjectile.projectileInfo, m_projectilePoint);
            //        break;
            //}
            m_projectileLauncher.AimAt(m_targetInfo.position);
            m_projectileLauncher.LaunchProjectile();
            StartCoroutine(ProjectileIKControlRoutine());
        }

        private void LaunchScytheWave()
        {
            if (!IsFacingTarget())
                CustomTurn();

            var target = new Vector2(m_scytheWavePoint.position.x + (5 * transform.localScale.x), m_scytheWavePoint.position.y);
            m_scytheWaveLauncher.AimAt(target);
            m_scytheWaveLauncher.LaunchProjectile();
        }



        private IEnumerator ChooseScytheWaveSpawn()
        {           
            var chosenPosition = GetPointFarthestFromPlayer(m_scytheWaveLeftSpawnPosition.position,  m_scytheWaveRightSpawnPosition.position);

            m_animation.SetAnimation(0, m_blinkDisappearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_blinkDisappearAnimation);

            transform.position = chosenPosition;

            m_animation.SetAnimation(0, m_blinkAppearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_blinkAppearAnimation);

            yield return null;
        }

        private Vector2 GetPointFarthestFromPlayer(params Vector2[] options)
        {
            int farthestIndex = 0;
            float farthestDistance = Vector2.Distance(m_lastTargetPos, options[0]);
            for (int i = 1; i < options.Length; i++)
            {
                if(Vector2.Distance(m_lastTargetPos, options[i]) > farthestDistance)
                {
                    farthestIndex = i;
                    farthestDistance = Vector2.Distance(m_lastTargetPos, options[i]);
                }
            }

            return options[farthestIndex];
        }

        private IEnumerator ProjectileIKControlRoutine()
        {
            m_targetIK.mode = SkeletonUtilityBone.Mode.Override;
            //m_slashIK.gameObject.SetActive(true);
            //m_targetIK.transform.position = m_targetInfo.position;
            m_targetIK.transform.LookAt(m_targetInfo.position);
            yield return new WaitUntil(() => m_animation.animationState.GetCurrent(0).IsComplete);
            m_targetIK.mode = SkeletonUtilityBone.Mode.Follow;
            //m_slashIK.gameObject.SetActive(false);
            yield return null;
        }

        private IEnumerator EvadeRoutine()
        {
            m_animation.EnableRootMotion(true, false);
            if (/*IsTargetInRange(m_info.projectilWaveSlashGround1Attack.range) &&*/ m_blinkCount < 2)
            {
                m_blinkCount++;
                if (m_blinkCoroutine != null)
                    yield return new WaitUntil(() => m_blinkCoroutine == null);

                m_blinkCoroutine = StartCoroutine(BlinkRoutine(BlinkState.DisappearBackward, BlinkState.AppearBackward, 60, m_info.midAirHeight, State.Chasing, m_blinkCount == 1 ? true : false, true, false));
            }
            else
            {
                m_blinkCount = 0;
                var chosenBehavior = UnityEngine.Random.Range(0, 2) == 1 ? 0 : 1;

                switch (chosenBehavior)
                {
                    case 0:
                        yield return ChooseScytheWaveSpawn();
                        m_animation.SetAnimation(0, m_info.scytheWaveAttack.animation, false);
                        yield return new WaitForAnimationComplete(m_animation.animationState, m_info.scytheWaveAttack.animation);
                        m_attackDecider.hasDecidedOnAttack = false;
                        m_currentAttackCoroutine = null;
                        if (m_alterBladeCoroutine == null)
                        {
                            m_stateHandle.ApplyQueuedState();
                            enabled = true;
                        }
                        break;
                    case 1:
                        m_currentAttackCoroutine = StartCoroutine(Phase1Pattern1AttackRoutine());
                        break;
                }
            }
            //m_evadeCoroutine = null;
            yield return null;
        }

        private IEnumerator FakeBlinkRoutine()
        {
            if (m_currentAttackCoroutine == null)
            {
                switch (m_fakeBlinkCount)
                {
                    case 0:
                        m_fakeBlinkCount++;
                        m_fakeBlinkChosenDrillDashBehavior = UnityEngine.Random.Range(0, 2);
                        if (m_blinkCoroutine != null)
                            yield return new WaitUntil(() => m_blinkCoroutine == null);

                        m_blinkCoroutine = StartCoroutine(BlinkRoutine(BlinkState.DisappearBackward, BlinkState.AppearBackward, 50, 0, State.Chasing, true, false, false));
                        break;
                    case 1:
                        //m_fakeBlinkCount = 0;
                        //m_fakeBlinkRoutine = null;
                        //m_hitbox.SetCanBlockDamageState(false);

                        m_currentAttackCoroutine = StartCoroutine(m_fakeBlinkChosenDrillDashBehavior == 1 ? DrillDashComboRoutine() : DrillDash2Routine());
                        break;
                }
            }
            yield return null;
        }

        private IEnumerator DrillDash2Routine()
        {
            enabled = false;
            if (IsTargetInRange(m_info.drillDash1Attack.range))
            {
                //if (!m_groundSensor.isDetecting)
                //{
                //    m_animation.DisableRootMotion();
                //    m_animation.SetAnimation(0, m_info.fallAnimation, true);
                //    yield return new WaitUntil(() => m_groundSensor.isDetecting);
                //    m_animation.SetAnimation(0, m_info.landAnimation, false);
                //    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.landAnimation);
                //}
                m_animation.EnableRootMotion(false, false);
                var drillCount = 0;
                while (drillCount < 2)
                {
                    m_animation.SetAnimation(0, m_info.groundToDrillAnimation, false);
                    m_drillDamage.SetActive(true);
                    var waitTime = m_animation.animationState.GetCurrent(0).AnimationEnd * 0.75f;
                    yield return new WaitForSeconds(waitTime);
                    m_hitbox.Disable();
                    m_animation.SetAnimation(4, m_drillMixAnimation, false);
                    m_character.physics.SetVelocity(m_info.drillDashSpeed * transform.localScale.x, 0);
                    m_animation.SetAnimation(0, m_info.drillDash1Attack.animation, false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillDash1Attack.animation);
                    m_animation.SetEmptyAnimation(4, 0);
                    m_hitbox.Enable();
                    m_movement.Stop();
                    m_animation.SetAnimation(0, m_info.drillToGroundAnimation, false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillToGroundAnimation);
                    m_drillDamage.SetActive(false);
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    if (!IsFacingTarget())
                        CustomTurn();

                    drillCount++;
                    yield return null;
                }
                m_attackDecider.hasDecidedOnAttack = false;
                m_currentAttackCoroutine = null;
                m_fakeBlinkCount = 0;
                m_fakeBlinkRoutine = null;
                m_hitbox.SetCanBlockDamageState(false);
                if (m_alterBladeCoroutine == null)
                    m_stateHandle.ApplyQueuedState();
            }
            else
            {
                if (m_blinkCoroutine != null)
                    yield return new WaitUntil(() => m_blinkCoroutine == null);

                m_blinkCoroutine = StartCoroutine(BlinkRoutine(BlinkState.DisappearBackward, BlinkState.AppearBackward, 50, 0, State.Chasing, false, false, false));
            }
            yield return null;
            enabled = true;
        }
        
        private IEnumerator DrillDashComboRoutine()
        {
            enabled = false;
            m_drillDashComboCount = m_drillDashComboCount > 1 ? 1 : m_drillDashComboCount;
            switch (m_drillDashComboCount)
            {
                case 0:
                    m_drillDashComboCount++;
                    if (m_blinkCoroutine != null)
                        yield return new WaitUntil(() => m_blinkCoroutine == null);

                    m_blinkCoroutine = StartCoroutine(BlinkRoutine(BlinkState.DisappearUpward, BlinkState.AppearUpward, 60, 50, State.Chasing, false, false, true));
                    break;
                case 1:
                    m_drillDashComboCount = 0;
                    m_lastTargetPos = m_targetInfo.position;
                    m_hitbox.Disable();
                    m_animation.DisableRootMotion();
                    if (!IsFacingTarget())
                        CustomTurn();
                    //m_animation.SetAnimation(0, m_info.groundToDrillAnimation, false);
                    //var waitTime = m_animation.animationState.GetCurrent(0).AnimationEnd * 0.75f;
                    //yield return new WaitForSeconds(waitTime);
                    m_animation.SetAnimation(0, m_info.fallAnimation, true);
                    yield return new WaitForSeconds(0.25f);
                    m_character.physics.simulateGravity = false;
                    m_animation.SetAnimation(4, m_drillMixAnimation, false);
                    m_animation.SetAnimation(0, m_info.drillDash1Attack.animation, true);
                    m_drillDamage.SetActive(true);
                    Vector2 spitPos = transform.position;
                    Vector3 v_diff = (new Vector2(m_lastTargetPos.x, m_lastTargetPos.y - 2) - spitPos);
                    float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
                    m_model.transform.rotation = Quaternion.Euler(0f, 0f, (atan2 * Mathf.Rad2Deg) + (m_character.facing == HorizontalDirection.Right ? 0 : 180));

                    float time = 0;
                    while (time < .25f)
                    {
                        m_groundSensor.multiRaycast.SetCastDistance(25);
                        if (!m_groundSensor.isDetecting)
                        {
                            m_character.physics.SetVelocity((m_character.facing == HorizontalDirection.Right ? m_info.drillDashSpeed : -m_info.drillDashSpeed) * m_model.transform.right);
                            time += Time.deltaTime;
                            yield return null;
                        }
                        yield return null;
                    }
                    m_groundSensor.multiRaycast.SetCastDistance(1);
                    m_character.physics.simulateGravity = true;
                    m_model.transform.rotation = Quaternion.identity;
                    m_animation.SetEmptyAnimation(4, 0);
                    m_hitbox.Enable();
                    m_movement.Stop();
                    m_animation.SetAnimation(0, m_info.drillToGroundAnimation, false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillToGroundAnimation);
                    m_drillDamage.SetActive(false);
                    //m_animation.SetEmptyAnimation(0, 0);
                    if (!m_groundSensor.isDetecting)
                    {
                        m_animation.SetAnimation(0, m_info.fallAnimation, true);
                        yield return new WaitUntil(() => m_groundSensor.isDetecting);
                        m_animation.SetAnimation(0, m_info.landAnimation, false);
                        yield return new WaitForAnimationComplete(m_animation.animationState, m_info.landAnimation);
                    }
                    if (!IsFacingTarget())
                        CustomTurn();

                    m_animation.SetAnimation(0, m_info.groundToDrillAnimation, false);
                    m_drillDamage.SetActive(true);
                    var waitTime = m_animation.animationState.GetCurrent(0).AnimationEnd * 0.75f;
                    yield return new WaitForSeconds(waitTime);
                    m_hitbox.Disable();
                    m_animation.SetAnimation(4, m_drillMixAnimation, false);
                    m_character.physics.SetVelocity(m_info.drillDashSpeed * transform.localScale.x, 0);
                    m_animation.SetAnimation(0, m_info.drillDash1Attack.animation, false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillDash1Attack.animation);
                    m_animation.SetEmptyAnimation(4, 0);
                    m_hitbox.Enable();
                    m_movement.Stop();
                    m_animation.SetAnimation(0, m_info.drillToGroundAnimation, false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillToGroundAnimation);
                    m_drillDamage.SetActive(false);
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    StopComboCounts();
                    m_attackDecider.hasDecidedOnAttack = false;
                    m_currentAttackCoroutine = null;
                    m_fakeBlinkCount = 0;
                    m_fakeBlinkRoutine = null;
                    m_hitbox.SetCanBlockDamageState(false);
                    if (m_alterBladeCoroutine == null)
                        m_stateHandle.ApplyQueuedState();
                    break;
            }
            yield return null;
            enabled = true;
        }

        private IEnumerator Phase1Pattern1AttackRoutine()
        {
            m_animation.EnableRootMotion(true, false);
            if (IsTargetInRange(m_info.downwardSlash1Attack.range))
            {
                yield return ChooseScytheWaveSpawn();
                m_animation.SetAnimation(0, m_info.downwardSlash1Attack.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.downwardSlash1Attack.animation);

                if (!IsFacingTarget())
                    CustomTurn();

                for (int i = 0; i < 2; i++)
                {
                    switch (i)
                    {
                        case 0:
                            m_animation.SetAnimation(0, m_info.swordStabAttack.animation, false);
                            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.swordStabAttack.animation);
                            m_animation.SetAnimation(0, m_info.heavySwordStabAttack.animation, false);
                            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.heavySwordStabAttack.animation);
                            break;
                        case 1:
                            yield return ChooseScytheWaveSpawn();

                            m_animation.SetAnimation(0, m_info.downwardSlash2Attack.animation, false);
                            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.downwardSlash2Attack.animation);
                            m_animation.SetAnimation(0, m_info.twinSlash1Attack.animation, false);
                            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.twinSlash1Attack.animation);
                            m_attackDecider.hasDecidedOnAttack = false;
                            m_currentAttackCoroutine = null;
                            if (m_alterBladeCoroutine == null)
                            {
                                m_stateHandle.ApplyQueuedState();
                                enabled = true;
                            }

                            break;
                    }
                }
            }
            else
            {
                if (m_blinkCoroutine != null)
                    yield return new WaitUntil(() => m_blinkCoroutine == null);

                m_blinkCoroutine = StartCoroutine(BlinkRoutine(BlinkState.DisappearForward, BlinkState.AppearForward, 25, m_info.midAirHeight, State.Chasing, false, false, false));
            }
            yield return null;
        }

        private IEnumerator Phase1Pattern2AttackRoutine()
        {
            m_animation.EnableRootMotion(true, false);

            m_animation.SetAnimation(0, m_info.projectilWaveSlashGround1Attack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.projectilWaveSlashGround1Attack.animation);

            m_animation.SetAnimation(0, m_info.projectilWaveSlashGround2Attack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.projectilWaveSlashGround2Attack.animation);

            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            if (m_alterBladeCoroutine == null)
            {
                m_stateHandle.ApplyQueuedState();
                enabled = true;
            }

            yield return null;
        }

        private IEnumerator Phase1Pattern3AttackRoutine()
        {
            m_animation.EnableRootMotion(false, false);
            if (IsTargetInRange(m_info.drillDash1Attack.range))
            {
                m_animation.SetAnimation(0, m_info.groundToDrillAnimation, false);
                var waitTime = m_animation.animationState.GetCurrent(0).AnimationEnd * 0.75f;
                yield return new WaitForSeconds(waitTime);
                m_hitbox.Disable();
                m_drillDamage.SetActive(true);
                m_animation.SetAnimation(4, m_drillMixAnimation, false);
                m_character.physics.SetVelocity(m_info.drillDashSpeed * transform.localScale.x, 0);
                m_animation.SetAnimation(0, m_info.drillDash1Attack.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillDash1Attack.animation);
                m_animation.SetEmptyAnimation(4, 0);
                m_hitbox.Enable();
                m_movement.Stop();
                m_animation.SetAnimation(0, m_info.drillToGroundAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillToGroundAnimation);
                m_drillDamage.SetActive(false);
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                yield return new WaitForSeconds(m_info.phase1Pattern3IdleTime);

                m_currentAttackCoroutine = StartCoroutine(EvadeRoutine());
            }
            else
            {
                if (m_blinkCoroutine != null)
                    yield return new WaitUntil(() => m_blinkCoroutine == null);

                m_blinkCoroutine = StartCoroutine(BlinkRoutine(BlinkState.DisappearBackward, BlinkState.AppearBackward, 60, m_info.midAirHeight, State.Chasing, false, false, false));
            }
            yield return null;
        }

        private IEnumerator Phase1Pattern4AttackRoutine()
        {
            var geyserAnimation = "";
            GameObject geyserToSpawn = null;
            switch (m_currentSwordState)
            {
                case SwordState.BlackBlood:
                    geyserAnimation = m_info.geyserBurstRedAttack.animation;
                    geyserToSpawn = m_info.geyserRed;
                    break;
                case SwordState.Poison:
                    geyserAnimation = m_info.geyserBurstPurpleAttack.animation;
                    geyserToSpawn = m_info.geyserPurple;
                    break;
                case SwordState.Acid:
                    geyserAnimation = m_info.geyserBurstGreenAttack.animation;
                    geyserToSpawn = m_info.geyserGreen;
                    break;
            }

            m_animation.AddAnimation(0, geyserAnimation, false, 0);
            yield return new WaitForAnimationComplete(m_animation.animationState, geyserAnimation);

            int pattern = UnityEngine.Random.Range(0, 2);

            switch (pattern)
            {
                case 0:
                    {
                        SpawnGeysers(m_geyserPatternOne, geyserToSpawn);
                    }
                    break;
                case 1:
                    {
                        SpawnGeysers(m_geyserPatternTwo, geyserToSpawn);
                    }
                    break;
                default:
                    {
                        SpawnGeysers(m_geyserPatternTwo, geyserToSpawn);
                    }
                    break;
            }

            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            if (m_alterBladeCoroutine == null)
            {
                m_stateHandle.ApplyQueuedState();
                enabled = true;
            }
            yield return null;
        }

        private void SpawnGeysers(Vector2[] patternLocations, GameObject geyser)
        {
            for (int i = 0; i < patternLocations.Length; i++)
            {
                var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(geyser, gameObject.scene);
                instance.SpawnAt(patternLocations[i], Quaternion.identity);
            }
        }

        private IEnumerator Phase2Pattern1AttackRoutine()
        {
            m_animation.EnableRootMotion(true, m_phase2pattern1Count == 3 ? true : false);
            switch (m_phase2pattern1Count)
            {
                case 3:
                    bool playerIsGrouned = Mathf.Abs(m_targetInfo.position.y - GroundPosition().y) < 10f ? true : false;
                    if (!playerIsGrouned)
                    {
                        m_animation.SetAnimation(0, m_info.twinSlash2Attack.animation, false);
                        yield return new WaitForAnimationComplete(m_animation.animationState, m_info.twinSlash2Attack.animation);
                        m_animation.EnableRootMotion(true, false);
                        m_animation.AddAnimation(0, m_info.fallAnimation, true, 0);
                        yield return new WaitUntil(() => m_groundSensor.isDetecting);
                        m_animation.SetAnimation(0, m_info.landAnimation, false);
                        yield return new WaitForAnimationComplete(m_animation.animationState, m_info.landAnimation);
                    }
                    else
                    {
                        m_animation.SetAnimation(0, m_info.blinkDisappearBackwardAnimation, false);
                        yield return new WaitForAnimationComplete(m_animation.animationState, m_info.blinkDisappearBackwardAnimation);
                        m_animation.EnableRootMotion(true, false);
                        yield return new WaitUntil(() => m_groundSensor.isDetecting);
                        if (!IsFacingTarget())
                            CustomTurn();
                        transform.position = new Vector2(transform.position.x, GroundPosition().y);
                        m_animation.SetAnimation(0, m_info.blinkAppearBackwardAnimation, false);
                        yield return new WaitForAnimationComplete(m_animation.animationState, m_info.blinkAppearBackwardAnimation);
                        m_animation.SetAnimation(0, m_info.twinSlash1Attack.animation, false);
                        yield return new WaitForAnimationComplete(m_animation.animationState, m_info.twinSlash1Attack.animation);
                    }
                    StopComboCounts();
                    m_attackDecider.hasDecidedOnAttack = false;
                    m_currentAttackCoroutine = null;
                    if (m_alterBladeCoroutine == null)
                    {
                        m_stateHandle.ApplyQueuedState();
                        enabled = true;
                    }
                    yield return null;
                    break;
                default:
                    if (IsTargetInRange(m_info.downwardSlash1Attack.range))
                    {

                        if (!IsFacingTarget())
                            CustomTurn();

                        switch (m_phase2pattern1Count)
                        {
                            case 0:
                                yield return ChooseScytheWaveSpawn();
                                m_animation.SetAnimation(0, m_info.downwardSlash1Attack.animation, false);
                                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.downwardSlash1Attack.animation);
                                m_phase2pattern1Count++;
                                m_stateHandle.OverrideState(State.Attacking);
                                enabled = true;
                                break;
                            case 1:
                                m_animation.SetAnimation(0, m_info.swordStabAttack.animation, false);
                                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.swordStabAttack.animation);
                                m_animation.SetAnimation(0, m_info.heavySwordStabAttack.animation, false);
                                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.heavySwordStabAttack.animation);
                                m_phase2pattern1Count++;
                                m_stateHandle.OverrideState(State.Attacking);
                                enabled = true;
                                break;
                            case 2:
                                yield return ChooseScytheWaveSpawn();
                                m_animation.SetAnimation(0, m_info.downwardSlash2Attack.animation, false);
                                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.downwardSlash2Attack.animation);
                                m_phase2pattern1Count++;
                                m_blinkCoroutine = StartCoroutine(BlinkRoutine(BlinkState.DisappearUpward, BlinkState.AppearUpward, 25, m_info.midAirHeight, State.Chasing, false, false, true));
                                yield return null;
                                break;
                        }
                    }
                    else
                    {
                        if (m_blinkCoroutine != null)
                            yield return new WaitUntil(() => m_blinkCoroutine == null);

                        m_blinkCoroutine = StartCoroutine(BlinkRoutine(BlinkState.DisappearForward, BlinkState.AppearForward, 0, m_info.midAirHeight, State.Chasing, false, false, false));
                    }
                    break;
            }
            yield return null;
        }

        private IEnumerator Phase2Pattern2AttackRoutine()
        {
            Debug.Log("blinkers" + m_blinkCoroutine);
            switch (m_phase2pattern2Count)
            {
                case 0:
                    m_phase2pattern2Count++;
                    if (m_blinkCoroutine != null)
                        yield return new WaitUntil(() => m_blinkCoroutine == null);

                    m_blinkCoroutine = StartCoroutine(BlinkRoutine(BlinkState.DisappearBackward, BlinkState.AppearBackward, 60, m_info.midAirHeight, State.Chasing, false, false, false));
                    break;
                case 1:
                    m_phase2pattern2Count = 0;
                    m_animation.SetAnimation(0, m_info.projectilWaveSlashGround1Attack.animation, false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.projectilWaveSlashGround1Attack.animation);

                    yield return ChooseScytheWaveSpawn();
                    var randomAttackAnimation = UnityEngine.Random.Range(0, 2) == 1 ? m_info.projectilWaveSlashGround2Attack.animation : m_info.scytheWaveAttack.animation;

                    m_animation.SetAnimation(0, randomAttackAnimation, false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, randomAttackAnimation);

                    m_attackDecider.hasDecidedOnAttack = false;
                    m_currentAttackCoroutine = null;
                    if (m_alterBladeCoroutine == null)
                    {
                        m_stateHandle.ApplyQueuedState();
                        enabled = true;
                    }
                    break;
            }
            yield return null;
        }

        private IEnumerator Phase2Pattern3AttackRoutine()
        {
            if (IsTargetInRange(m_info.scytheWaveAttack.range))
            {
                yield return ChooseScytheWaveSpawn();
                m_animation.SetAnimation(0, m_info.scytheDoubleWaveAttack.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.scytheDoubleWaveAttack.animation);

                m_attackDecider.hasDecidedOnAttack = false;
                m_currentAttackCoroutine = null;
                if (m_alterBladeCoroutine == null)
                {
                    m_stateHandle.ApplyQueuedState();
                    enabled = true;
                }
            }
            else
            {
                if (m_blinkCoroutine != null)
                    yield return new WaitUntil(() => m_blinkCoroutine == null);

                m_blinkCoroutine = StartCoroutine(BlinkRoutine(BlinkState.DisappearBackward, BlinkState.AppearBackward, 60, m_info.midAirHeight, State.Chasing, false, false, false));
            }
            yield return null;
        }

        private IEnumerator Phase2Pattern5AttackRoutine()
        {
            var isMidAir = UnityEngine.Random.Range(0, 2) == 1 ? true : false;
            m_phase2pattern5Count = m_phase2pattern5Count > 3 ? 3 : m_phase2pattern5Count;
            switch (m_phase2pattern5Count)
            {
                case 0:
                    m_phase2pattern5Count++;
                    if (m_blinkCoroutine != null)
                        yield return new WaitUntil(() => m_blinkCoroutine == null);
                    m_blinkCoroutine = StartCoroutine(BlinkRoutine(BlinkState.DisappearForward, isMidAir ? BlinkState.AppearUpward : BlinkState.AppearForward, 40, m_info.midAirHeight, State.Chasing, true, false, isMidAir ? true : false));
                    break;
                case 1:
                    m_phase2pattern5Count++;
                    if (!m_groundSensor.isDetecting)
                    {
                        m_animation.DisableRootMotion();
                        m_animation.SetAnimation(0, m_info.fallAnimation, true);
                        yield return new WaitUntil(() => m_groundSensor.isDetecting);
                        m_animation.SetAnimation(0, m_info.landAnimation, false);
                        yield return new WaitForAnimationComplete(m_animation.animationState, m_info.landAnimation);
                    }

                    if (m_blinkCoroutine != null)
                        yield return new WaitUntil(() => m_blinkCoroutine == null);

                    m_blinkCoroutine = StartCoroutine(BlinkRoutine(BlinkState.DisappearForward, BlinkState.AppearForward, 15, 0, State.Chasing, false, false, false));
                    break;
                case 2:
                    m_phase2pattern5Count++;
                    if (isMidAir)
                    {
                        if (m_blinkCoroutine != null)
                            yield return new WaitUntil(() => m_blinkCoroutine == null);

                        m_blinkCoroutine = StartCoroutine(BlinkRoutine(BlinkState.DisappearUpward, BlinkState.AppearUpward, 60, 50, State.Chasing, false, false, true));
                    }
                    else
                    {
                        m_phase2pattern5Count = 0;
                        m_currentAttackCoroutine = StartCoroutine(DrillDash2Routine());
                    }
                    break;
                case 3:
                    m_phase2pattern5Count = 0;
                    m_drillDashComboCount++;
                    m_currentAttackCoroutine = StartCoroutine(DrillDashComboRoutine());
                    break;
            }
            yield return null;
        }
        #endregion

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            StopAllCoroutines();
            m_hitbox.Disable();
            m_animation.DisableRootMotion();
            m_movement.Stop();
            StartCoroutine(DefeatRoutine());
        }

        private IEnumerator DefeatRoutine()
        {
            m_animation.SetEmptyAnimation(4, 0);
            m_animation.SetEmptyAnimation(5, 0);
            m_animation.SetAnimation(0, m_info.defeated3Animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.defeated3Animation);
            m_animation.SetAnimation(0, m_info.blinkDisappearBackwardAnimation, false);
            m_isDetecting = false;
            enabled = false;
            yield return null;
        }

        #region Movement
        private void MoveToTarget(float targetRange)
        {
            if (!IsTargetInRange(targetRange) && m_groundSensor.isDetecting /*&& !m_wallSensor.isDetecting && m_edgeSensor.isDetecting*/)
            {
                m_animation.EnableRootMotion(false, false);
                m_animation.SetAnimation(0, m_info.walk.animation, true);
                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.walk.speed);
            }
            else
            {
                m_movement.Stop();
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
        }

        private IEnumerator BlinkRoutine(BlinkState disappearState, BlinkState appearState, float blinkDistance, float midAirHeight, State transitionState, bool fakeBlink, bool evadeBlink, bool isMidAir)
        {
            m_legCollider.enabled = false;
            m_hitbox.Disable();
            m_movement.Stop();
            m_character.physics.simulateGravity = false;
            m_bodyCollider.enabled = false;
            m_model.transform.rotation = Quaternion.identity; 
            switch (disappearState)
            {
                case BlinkState.DisappearForward:
                    m_blinkDisappearAnimation = m_info.blinkDisappearForwardAnimation.animation;
                    break;
                case BlinkState.DisappearBackward:
                    m_blinkDisappearAnimation = m_info.blinkDisappearBackwardAnimation.animation;
                    break;
                case BlinkState.DisappearUpward:
                    m_blinkDisappearAnimation = m_info.blinkDisappearUpwardAnimation.animation;
                    break;
            }

            m_animation.SetAnimation(0, m_blinkDisappearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_blinkDisappearAnimation);

            var lastPos = transform.position;
            var blinkWait = 0f;
            while (Mathf.Abs(lastPos.x - m_targetInfo.position.x) < blinkDistance
                || Mathf.Abs(lastPos.x - m_targetInfo.position.x) > blinkDistance + 5f
                || Vector2.Distance(lastPos, transform.position) <= 50f
                && blinkWait < 1.5f)
            {
                lastPos = new Vector2(RandomTeleportPoint(lastPos).x, GroundPosition(transform.position).y);
                blinkWait += Time.deltaTime;
                yield return null;
            }


            if (fakeBlink)
            {
                var blinkCount = 0;
                while (blinkCount < m_info.fakeBlinkCount)
                {
                    CustomTurn();
                    transform.position = RandomTeleportPoint(transform.position);
                    m_animation.SetAnimation(0, m_info.blinkFakeAnimation, true);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.blinkFakeAnimation);
                    blinkCount++;
                    yield return null;
                }
                m_model.SetActive(false);
            }

            transform.position = lastPos;

            m_blinkFX.Play();
            yield return new WaitForSeconds(m_info.blinkDuration);
            m_model.SetActive(true);
            m_legCollider.enabled = true;
            m_hitbox.Enable();
            m_character.physics.simulateGravity = true;
            m_bodyCollider.enabled = true;

            switch (appearState)
            {
                case BlinkState.AppearForward:
                    m_blinkAppearAnimation = m_info.blinkAppearForwardAnimation.animation;
                    break;
                case BlinkState.AppearBackward:
                    m_blinkAppearAnimation = m_info.blinkAppearBackwardAnimation.animation;
                    break;
                case BlinkState.AppearUpward:
                    m_blinkAppearAnimation = m_info.blinkAppearUpwardAnimation.animation;
                    break;
            }
            if (!IsFacingTarget())
                CustomTurn();

            if (isMidAir)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + midAirHeight);
                m_animation.EnableRootMotion(true, true);
            }
            else
                m_animation.DisableRootMotion();

            m_animation.SetAnimation(0, m_blinkAppearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_blinkAppearAnimation);
            m_blinkCoroutine = null;
            m_currentAttackCoroutine = null;
            //if (blinkDuration != 0)
            //    m_attackDecider.hasDecidedOnAttack = false;

            if (m_fakeBlinkRoutine != null)
            {
                m_fakeBlinkRoutine = StartCoroutine(FakeBlinkRoutine());
                yield return null;
            }

            if (m_alterBladeCoroutine == null)
            {
                switch (evadeBlink)
                {
                    case true:
                        //if (m_evadeCoroutine != null)
                        //    yield return new WaitUntil(() => m_evadeCoroutine == null);

                        m_animation.SetAnimation(0, m_idleAnimation, true);
                        m_currentAttackCoroutine = StartCoroutine(EvadeRoutine());
                        break;
                    case false:
                        m_stateHandle.OverrideState(transitionState);
                        enabled = true;
                        break;
                }
            }
            //else
            //{
            //    if (m_phaseHandle.currentPhase == Phase.PhaseTwo && m_fakeBlinkRoutine != null)
            //    {
            //    }
            //}
            yield return null;
        }

        private Vector3 RandomTeleportPoint(Vector3 storedPos)
        {
            Vector3 randomPos = storedPos;
            while (Vector2.Distance(storedPos, randomPos) <= 50f)
            {
                randomPos = m_randomSpawnCollider.bounds.center + new Vector3(
               (UnityEngine.Random.value - 0.5f) * m_randomSpawnCollider.bounds.size.x,
               (UnityEngine.Random.value - 0.5f) * m_randomSpawnCollider.bounds.size.y,
               (UnityEngine.Random.value - 0.5f) * m_randomSpawnCollider.bounds.size.z);
            }
            return randomPos;
        }

        private static ContactFilter2D m_contactFilter;
        private static RaycastHit2D[] m_hitResults;
        private static bool m_isInitialized;

        private static void Initialize()
        {
            if (m_isInitialized == false)
            {
                m_contactFilter.useLayerMask = true;
                m_contactFilter.SetLayerMask(DChildUtility.GetEnvironmentMask());
                //m_contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(DChildUtility.GetEnvironmentMask()));
                m_hitResults = new RaycastHit2D[16];
                m_isInitialized = true;
            }
        }

        public static RaycastHit2D[] Cast(Vector2 origin, Vector2 direction, float distance, bool ignoreTriggers, out int hitCount, bool debugMode = false)
        {
            Initialize();
            m_contactFilter.useTriggers = !ignoreTriggers;
            hitCount = Physics2D.Raycast(origin, direction, m_contactFilter, m_hitResults, distance);
#if UNITY_EDITOR
            if (debugMode)
            {
                if (hitCount > 0)
                {
                    Debug.DrawRay(origin, direction * m_hitResults[0].distance, Color.cyan, 1f);
                }
                else
                {
                    Debug.DrawRay(origin, direction * distance, Color.cyan, 1f);
                }
            }
#endif
            return m_hitResults;
        }
        #endregion

        #region Cooldown Monitors
        private IEnumerator CooldownMonitorRoutine(int chosenCD)
        {
            var fullCooldownTime = m_patternCooldown[chosenCD];
            while (m_patternCooldown[chosenCD] > 0)
            {
                m_patternCooldown[chosenCD] -= Time.deltaTime;
                yield return null;
            }
            m_patternCooldown[chosenCD] = fullCooldownTime;
            yield return null;
        }

        private IEnumerator AlterBladeMonitorRoutine()
        {
            while (true)
            {
                switch (m_currentSwordState)
                {
                    case SwordState.Normal:
                        yield return new WaitForSeconds(m_info.normalBladeCooldown);
                        while (m_currentSwordState == m_cachedSwordState || m_currentSwordState == SwordState.Normal)
                        {
                            var randomSwordState = UnityEngine.Random.Range(0, 3);
                            switch (randomSwordState)
                            {
                                case 0:
                                    m_currentSwordState = SwordState.BlackBlood;
                                    break;
                                case 1:
                                    m_currentSwordState = SwordState.Poison;
                                    break;
                                case 2:
                                    m_currentSwordState = SwordState.Acid;
                                    break;
                            }
                            yield return null;
                        }
                        m_cachedSwordState = m_currentSwordState;
                        break;
                    default:
                        yield return new WaitForSeconds(m_info.alterBladeCooldown);
                        m_currentSwordState = SwordState.Normal;
                        break;
                }
                if (m_alterBladeCoroutine == null)
                {
                    m_alterBladeCoroutine = StartCoroutine(AlterBladeRoutine(m_currentSwordState));
                }
                yield return null;
            }
        }

        private IEnumerator AlterBladeRoutine(SwordState swordState)
        {
            enabled = false;
            yield return new WaitUntil(() => m_currentAttackCoroutine == null && m_blinkCoroutine == null && m_changePhaseCoroutine == null && !m_hitbox.canBlockDamage);
            StopComboCounts();
            if (!m_groundSensor.isDetecting)
            {
                m_animation.DisableRootMotion();
                m_animation.SetAnimation(0, m_info.fallAnimation, true);
                yield return new WaitUntil(() => m_groundSensor.isDetecting);
                m_animation.SetAnimation(0, m_info.landAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.landAnimation);
            }
            m_animation.SetAnimation(0, m_idleAnimation, true);
            switch (swordState)
            {
                case SwordState.Normal:
                    m_swordMixAnimation = m_info.swordNormalMixAnimation.animation;
                    m_drillMixAnimation = m_info.drillNormalMixAnimation.animation;
                    m_projectileLauncher = new ProjectileLauncher(m_info.slashNormalProjectile.projectileInfo, m_projectilePoint);
                    m_scytheWaveLauncher = new ProjectileLauncher(m_info.scytheWaveNormalProjectile.projectileInfo, m_scytheWavePoint);
                    break;
                case SwordState.BlackBlood:
                    m_swordMixAnimation = m_info.swordRedMixAnimation.animation;
                    m_drillMixAnimation = m_info.drillRedMixAnimation.animation;
                    m_projectileLauncher = new ProjectileLauncher(m_info.slashBlackbloodProjectile.projectileInfo, m_projectilePoint);
                    m_scytheWaveLauncher = new ProjectileLauncher(m_info.scytheWaveBlackbloodProjectile.projectileInfo, m_scytheWavePoint);
                    break;
                case SwordState.Poison:
                    m_swordMixAnimation = m_info.swordPurpleMixAnimation.animation;
                    m_drillMixAnimation = m_info.drillPurpleMixAnimation.animation;
                    m_projectileLauncher = new ProjectileLauncher(m_info.slashPoisonProjectile.projectileInfo, m_projectilePoint);
                    m_scytheWaveLauncher = new ProjectileLauncher(m_info.scytheWavePoisonProjectile.projectileInfo, m_scytheWavePoint);
                    break;
                case SwordState.Acid:
                    m_swordMixAnimation = m_info.swordGreenMixAnimation.animation;
                    m_drillMixAnimation = m_info.drillGreenMixAnimation.animation;
                    m_projectileLauncher = new ProjectileLauncher(m_info.slashAcidProjectile.projectileInfo, m_projectilePoint);
                    m_scytheWaveLauncher = new ProjectileLauncher(m_info.scytheWaveAcidProjectile.projectileInfo, m_scytheWavePoint);
                    break;
            }
            m_animation.SetAnimation(0, m_info.swordChangeAnimation, false);
            m_animation.SetAnimation(5, m_swordMixAnimation, false).MixBlend = MixBlend.First;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.swordChangeAnimation);
            m_attackDecider.hasDecidedOnAttack = false;
            m_alterBladeCoroutine = null;
            m_stateHandle.OverrideState(State.Chasing);
            yield return null;
            enabled = true;
        }
        #endregion 

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Phase1Pattern1, m_info.phase1Pattern1Range)
                                    , new AttackInfo<Attack>(Attack.Phase1Pattern2, m_info.phase1Pattern2Range)
                                    , new AttackInfo<Attack>(Attack.Phase1Pattern3, m_info.phase1Pattern3Range)
                                    , new AttackInfo<Attack>(Attack.Phase1Pattern4, m_info.phase1Pattern4Range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private Vector2 GroundPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1000, DChildUtility.GetEnvironmentMask());
            return hit.point;
        }

        private void ChooseAttack()
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

        protected override void Awake()
        {
            base.Awake();
            m_turnHandle.TurnDone += OnTurnDone;
            m_damageable.DamageTaken += OnDamageTaken;
            //m_damageable.DamageTaken += OnDamageBlocked;
            //m_patternDecider = new RandomAttackDecider<Pattern>();
            m_projectileLauncher = new ProjectileLauncher(m_info.slashNormalProjectile.projectileInfo, m_projectilePoint);
            m_scytheWaveLauncher = new ProjectileLauncher(m_info.scytheWaveNormalProjectile.projectileInfo, m_scytheWavePoint);
            m_attackDecider = new RandomAttackDecider<Attack>();
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            UpdateAttackDeciderList();
            m_attackCache = new List<Attack>();
            AddToAttackCache(Attack.Phase1Pattern1, Attack.Phase1Pattern2, Attack.Phase1Pattern3, Attack.Phase1Pattern4);
            m_attackRangeCache = new List<float>();
            AddToRangeCache(m_info.phase1Pattern1Range, m_info.phase1Pattern2Range, m_info.phase1Pattern3Range, m_info.phase1Pattern4Range);
            m_attackUsed = new bool[m_attackCache.Count];
            m_currentFullCooldown = new List<float>();
            m_patternCooldown = new List<float>();
        }

        protected override void Start()
        {
            base.Start();
            m_spineListener.Subscribe(m_info.slashNormalProjectile.launchOnEvent, LaunchProjectile);
            m_spineListener.Subscribe(m_info.scytheWaveNormalProjectile.launchOnEvent, LaunchScytheWave);
            m_animation.DisableRootMotion();
            m_phaseHandle = new PhaseHandle<Phase, PhaseInfo>();
            m_phaseHandle.Initialize(Phase.PhaseOne, m_info.phaseInfo, m_character, ChangeState, ApplyPhaseData);
            m_phaseHandle.ApplyChange();

            m_currentSwordState = SwordState.Normal;
            m_cachedSwordState = SwordState.Normal;
            m_drillMixAnimation = m_info.drillNormalMixAnimation.animation;

            m_blinkDisappearAnimation = m_info.blinkDisappearForwardAnimation.animation;
            m_blinkAppearAnimation = m_info.blinkAppearForwardAnimation.animation;
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
                        //m_stateHandle.OverrideState(State.Chasing);
                    }
                    else
                    {
                        m_turnState = State.Intro;
                        m_stateHandle.SetState(State.Turning);
                    }
                    break;
                case State.Phasing:
                    if (m_changePhaseCoroutine == null)
                    {
                        m_changePhaseCoroutine = StartCoroutine(ChangePhaseRoutine());
                    }
                    break;
                case State.Turning:
                    m_phaseHandle.allowPhaseChange = false;
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute();
                    m_movement.Stop();
                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    m_lastTargetPos = m_targetInfo.position;

                    Debug.Log("CURRENT ATTACK PATTERN " + m_currentAttack);
                    switch (m_currentAttack)
                    {
                        case Attack.Phase1Pattern1:
                            m_currentAttackCoroutine = StartCoroutine(Phase1Pattern1AttackRoutine());
                            m_pickedCooldown = m_currentFullCooldown[0];
                            break;
                        case Attack.Phase1Pattern2:
                            m_currentAttackCoroutine = StartCoroutine(Phase1Pattern2AttackRoutine());
                            m_pickedCooldown = m_currentFullCooldown[1];
                            break;
                        case Attack.Phase1Pattern3:
                            m_currentAttackCoroutine = StartCoroutine(Phase1Pattern3AttackRoutine());
                            m_pickedCooldown = m_currentFullCooldown[2];
                            break;
                        case Attack.Phase1Pattern4:
                            if (m_patternCooldown[3] == m_info.phase1PatternCooldown[3] && m_currentSwordState != SwordState.Normal)
                            {
                                m_currentAttackCoroutine = StartCoroutine(Phase1Pattern4AttackRoutine());
                                m_pickedCooldown = m_currentFullCooldown[3];
                                StartCoroutine(CooldownMonitorRoutine(3));
                            }
                            else
                            {
                                m_attackDecider.hasDecidedOnAttack = false;
                                m_currentAttackCoroutine = null;
                                if (m_alterBladeCoroutine == null)
                                    m_stateHandle.ApplyQueuedState();
                            }
                            break;
                        case Attack.Phase2Pattern1:
                            m_currentAttackCoroutine = StartCoroutine(Phase2Pattern1AttackRoutine());
                            m_pickedCooldown = m_currentFullCooldown[0];
                            break;
                        case Attack.Phase2Pattern2:
                            m_currentAttackCoroutine = StartCoroutine(Phase2Pattern2AttackRoutine());
                            m_pickedCooldown = m_currentFullCooldown[1];
                            break;
                        case Attack.Phase2Pattern3:
                            m_currentAttackCoroutine = StartCoroutine(Phase2Pattern3AttackRoutine());
                            m_pickedCooldown = m_currentFullCooldown[2];
                            break;
                        case Attack.Phase2Pattern4:
                            if (m_patternCooldown[3] == m_info.phase2PatternCooldown[3])
                            {
                                if (m_currentSwordState != SwordState.Normal)
                                {
                                    m_currentAttackCoroutine = StartCoroutine(Phase1Pattern4AttackRoutine());
                                    m_pickedCooldown = m_currentFullCooldown[3];
                                    StartCoroutine(CooldownMonitorRoutine(3));
                                }
                                else
                                {
                                    //m_attackDecider.hasDecidedOnAttack = false;
                                    //m_currentAttackCoroutine = null;
                                    //if (m_alterBladeCoroutine == null)
                                    //    m_stateHandle.ApplyQueuedState();
                                    m_currentAttackCoroutine = StartCoroutine(DrillDashComboRoutine());
                                }
                            }
                            else
                            {
                                m_currentAttackCoroutine = StartCoroutine(DrillDashComboRoutine());
                            }
                            break;
                        case Attack.Phase2Pattern5:
                            m_currentAttackCoroutine = StartCoroutine(Phase2Pattern5AttackRoutine());
                            m_pickedCooldown = m_currentFullCooldown[4];
                            break;
                    }

                    enabled = false;
                    break;

                case State.Cooldown:
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        if (m_alterBladeCoroutine == null)
                            m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        m_animation.SetAnimation(0, m_idleAnimation, true).TimeScale = 1;
                    }

                    if (m_currentCooldown <= m_pickedCooldown)
                    {
                        m_currentCooldown += Time.deltaTime;
                    }
                    else
                    {
                        m_currentCooldown = 0;
                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                    }

                    break;

                case State.Chasing:
                    if (!m_hitbox.canBlockDamage)
                    {
                        if (IsFacingTarget())
                        {
                            ChooseAttack();
                            if (IsTargetInRange(m_currentAttackRange) && m_currentAttackCoroutine == null)
                            {
                                m_stateHandle.SetState(State.Attacking);
                            }
                            else
                            {
                                MoveToTarget(m_currentAttackRange);
                            }
                        }
                        else
                        {
                            m_turnState = State.Attacking;
                            if (m_alterBladeCoroutine == null)
                                m_stateHandle.SetState(State.Turning);
                        }
                    }
                    break;

                case State.ReevaluateSituation:
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