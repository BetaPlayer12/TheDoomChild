using DChild;
using DChild.Gameplay;
using DChild.Gameplay.Characters;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using DChild.Temp;
using DG.Tweening;
using Holysoft.Event;
using Language.Lua;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using Spine.Unity.Examples;
using Spine.Unity.Modules;
using System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
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
            private List<float> m_phase2PatternCooldown;
            public List<float> phase2PatternCooldown => m_phase2PatternCooldown;
            [SerializeField, MinValue(0)]
            private float m_normalBladeCooldown;
            public float normalBladeCooldown => m_normalBladeCooldown;
            [SerializeField, MinValue(0)]
            private float m_alterBladeCooldown;
            public float alterBladeCooldown => m_alterBladeCooldown;
            [SerializeField, MinValue(0)]
            private float m_defaultIdleTime;
            public float defaultIdleTime => m_defaultIdleTime;
          

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
            [SerializeField, BoxGroup("Sword Change")]
            private BasicAnimationInfo m_swordChangeAnimationToGreen;
            public BasicAnimationInfo swordChangeAnimationToGreen => m_swordChangeAnimationToGreen;
            [SerializeField, BoxGroup("Sword Change")]
            private BasicAnimationInfo m_swordChangeAnimationToPurple;
            public BasicAnimationInfo swordChangeAnimationToPurple => m_swordChangeAnimationToPurple;
            [SerializeField, BoxGroup("Sword Change")]
            private BasicAnimationInfo m_swordChangeAnimationToRed;
            public BasicAnimationInfo swordChangeAnimationToRed => m_swordChangeAnimationToRed;
            [SerializeField, BoxGroup("Sword Change")]
            private BasicAnimationInfo m_swordChangeAnimationToNormal;
            public BasicAnimationInfo swordChangeAnimationToNormal => m_swordChangeAnimationToNormal;
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
            private float m_dualSwordComboAttackRange;
            public float dualSwordComboAttackRange => m_dualSwordComboAttackRange;
            [SerializeField]
            private float m_projectileWaveSlashAttackRange;
            public float projectileWaveSlashAttackRange => m_projectileWaveSlashAttackRange;
            [SerializeField]
            private float m_drillDashAttackRange;
            public float drillDashAttackRange => m_drillDashAttackRange;
            [SerializeField]
            private float m_evadeRangeToFunction;
            public float evadeRangeToFunction => m_evadeRangeToFunction;
            [SerializeField]
            private float m_dualSwordComboAttackRange2;
            public float dualSwordComboAttackRange2 => m_dualSwordComboAttackRange2;
            [SerializeField]
            private float m_projectileWaveSlashAttackRange2;
            public float projectileWaveSlashAttackRange2 => m_projectileWaveSlashAttackRange2;
            [SerializeField]
            private float m_scytheWaveAttackRange2;
            public float scytheWaveAttackRange2 => m_scytheWaveAttackRange2;
            [SerializeField]
            private float m_normalBladeCounter;
            public float normalBladeCounter => m_normalBladeCounter;
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
            [SerializeField]
            private int m_geyserBurstCD;
            public int geyserBurstCD => m_geyserBurstCD;

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
            private BasicAnimationInfo m_drillDashDiagonal;
            public BasicAnimationInfo drillDashDiagonal => m_drillDashDiagonal;
            [SerializeField]
            private BasicAnimationInfo m_airTodrillDashDiagonal;
            public BasicAnimationInfo airTodrillDashDiagonal => m_airTodrillDashDiagonal;
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
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_geyserStartNormal;
            public string geyserStartNormal => m_geyserStartNormal;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_geyserStartRed;
            public string geyserStartRed => m_geyserStartRed;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_geyserStartGreen;
            public string geyserStartGreen => m_geyserStartGreen;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_geyserStartPurple;
            public string geyserStartPurple => m_geyserStartPurple;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_drillDiagonalEvent;
            public string drillDiagonalEvent => m_drillDiagonalEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_rootStartEvent;
            public string rootStartEvent => m_rootStartEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_rootEndEvent;
            public string rootEndEvent => m_rootEndEvent;

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

                m_airTodrillDashDiagonal.SetData(m_skeletonDataAsset);
                m_drillDashDiagonal.SetData(m_skeletonDataAsset);
                m_swordChangeAnimationToGreen.SetData(m_skeletonDataAsset);
                m_swordChangeAnimationToNormal.SetData(m_skeletonDataAsset);
                m_swordChangeAnimationToPurple.SetData(m_skeletonDataAsset);
                m_swordChangeAnimationToRed.SetData(m_skeletonDataAsset);
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
            Phase1Pattern5,
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
       
        private string m_blinkAppearAnimation;
        private string m_blinkDisappearAnimation;
        private string m_drillMixAnimation;
        private string m_swordMixAnimation;
        #endregion  
        [ReadOnly]
        private readonly List<SwordState> m_usedSwordStates = new();
        private void ApplyPhaseData(PhaseInfo obj)
        {
            if (m_attackDecider != null)
            {
                UpdateAttackDeciderList();
            }
            base.ApplyData();
        }

        private void ChangeState()
        {
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
            /*if (m_alterBladeCoroutine == null && m_staggerCoroutine == null && m_drillDashCounterCoroutine == null && m_blinkCoroutine == null)
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
            }*/


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
            enabled = true;
            yield return null;
        }
        [SerializeField]
        private GameObject m_drillDamage;
        [SerializeField]
        private GameObject m_drillDamageDiagonal;
        private IEnumerator DrillDashCounterRoutine()
        {
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
                m_character.physics.SetVelocity(m_info.drillDashSpeed * transform.localScale.x, 0);
                m_drillDamage.SetActive(true);
                m_animation.SetAnimation(0, m_info.drillDash1Attack.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillDash1Attack.animation);
                m_animation.SetEmptyAnimation(4, 0);
                m_hitbox.Enable();
                m_movement.Stop();
                m_animation.SetAnimation(0, m_info.drillToGroundAnimation, false);
                m_drillDamage.SetActive(false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillToGroundAnimation);
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
        }

        private IEnumerator IntroRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            float walkDuration = 3f;
            float elapsedTime = 0f;
            m_movement.Stop();
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            //m_cinematic.PlayCinematic(1, false);
            m_animation.animationState.TimeScale = 1;
            m_hitbox.Enable();
            m_hitbox.SetInvulnerability(Invulnerability.None);
            
            m_animation.SetAnimation(0, m_info.walk.animation, true);
            while (elapsedTime < walkDuration)
            {
                Vector2 direction = new Vector2(
                    m_targetInfo.position.x - transform.position.x,
                    0f
                ).normalized;

                m_movement.MoveTowards(direction, m_info.walk.speed);

                if (!IsFacingTarget())
                {
                    CustomTurn();
                }

                elapsedTime += Time.deltaTime;

                yield return null;
            }
            yield return AlterBladeMonitorRoutine();
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator SmartChangePhaseRoutine()
        {  
            StopComboCounts();
            ResetCounterCounts();
            SetAIToPhasing();
            yield return null;
        }

        private void SetAIToPhasing()
        {
            m_phaseHandle.ApplyChange();
            m_animation.DisableRootMotion();
            m_stateHandle.OverrideState(State.Phasing);
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
            Debug.Log("changing routine");
            m_stateHandle.Wait(State.ReevaluateSituation); 
            m_drillDamage.SetActive(false);
            m_heavySwordStab.SetActive(false);
            m_swordStab.SetActive(false);
            m_twinSlash.SetActive(false);
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
            yield return new WaitForSeconds(m_info.phaseChangeToBlinkDelay);
            yield return AlterBladeMonitorRoutine();
            yield return BlinkRoutine(BlinkState.DisappearForward, BlinkState.AppearForward, new Vector2(25,0), m_info.midAirHeight, true, false, false);
            m_stateHandle.ApplyQueuedState();
           
            Debug.Log("changing routine done");
        }
        #region Attacks

        private void LaunchProjectile()
        {
            Debug.Log("launching");
            if (!IsFacingTarget())
                CustomTurn();
            switch (m_currentSwordState)
            {
                case SwordState.Normal:
                    m_projectileLauncher = new ProjectileLauncher(m_info.slashNormalProjectile.projectileInfo, m_projectilePoint);
                    break;
                case SwordState.BlackBlood:
                    m_projectileLauncher = new ProjectileLauncher(m_info.slashBlackbloodProjectile.projectileInfo, m_projectilePoint);
                    break;
                case SwordState.Poison:
                    m_projectileLauncher = new ProjectileLauncher(m_info.slashPoisonProjectile.projectileInfo, m_projectilePoint);
                    break;
                case SwordState.Acid:
                    m_projectileLauncher = new ProjectileLauncher(m_info.slashAcidProjectile.projectileInfo, m_projectilePoint);
                    break;
            }
                    m_projectileLauncher.AimAt(m_targetInfo.position);
            m_projectileLauncher.LaunchProjectile();
            StartCoroutine(ProjectileIKControlRoutine());
            Debug.Log("launching done");
        }

        private void LaunchScytheWave()
        {
            Debug.Log("launching wave");
            if (!IsFacingTarget())
                CustomTurn();

            var target = new Vector2(m_scytheWavePoint.position.x + (5 * transform.localScale.x), m_scytheWavePoint.position.y);
            m_scytheWaveLauncher.AimAt(target);
            m_scytheWaveLauncher.LaunchProjectile();
            Debug.Log("launching wave done");
        }



        private IEnumerator ChooseScytheWaveSpawn()
        {
            Debug.Log("choose wave spawn");
            var chosenPosition = GetPointFarthestFromPlayer(m_scytheWaveLeftSpawnPosition.position,  m_scytheWaveRightSpawnPosition.position);
            m_animation.SetAnimation(0, m_blinkDisappearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_blinkDisappearAnimation);      
            transform.position = chosenPosition;
            m_animation.SetAnimation(0, m_blinkAppearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_blinkAppearAnimation);
            if (!IsFacingTarget())
                CustomTurn();
            Debug.Log("choose wave spawn done");
        }

        private Vector2 GetPointFarthestFromPlayer(params Vector2[] options)
        {
            Debug.Log("GetPointFarthestFromPlayer");
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
            Debug.Log("GetPointFarthestFromPlayer done");
        }

        private IEnumerator ProjectileIKControlRoutine()
        {
            Debug.Log("controlroutine");
            m_targetIK.mode = SkeletonUtilityBone.Mode.Override;
            //m_slashIK.gameObject.SetActive(true);
            //m_targetIK.transform.position = m_targetInfo.position;
            m_targetIK.transform.LookAt(m_targetInfo.position);
            yield return new WaitUntil(() => m_animation.animationState.GetCurrent(0).IsComplete);
            m_targetIK.mode = SkeletonUtilityBone.Mode.Follow;
            //m_slashIK.gameObject.SetActive(false);
            yield return null;
            Debug.Log("controlroutine done");
        }

        private IEnumerator EvadePlayerRoutine()
        {
            float blinkCount = 0;
            float walkDuration = 2f;
            float elapsedTime = 0f;
            while (true)
            {
                if (IsTargetInRange(m_info.evadeRangeToFunction))
                {
                    
                    yield return BlinkOut(BlinkState.DisappearBackward);
                    if (blinkCount >= 2)
                    {
                        yield return BlinkIn(BlinkState.AppearForward, new Vector2(20, 0));
                        yield return DualSwordComboForEvadeRoutine();
                        Debug.Log("Blink count 2");
                        yield break;
                    }
                    yield return BlinkIn(BlinkState.AppearForward, new Vector2(20, 0));
                    m_animation.SetAnimation(0, m_info.walk.animation, true);
                    while (elapsedTime < walkDuration)
                    {
                        Vector2 direction = new Vector2(
                            m_targetInfo.position.x - transform.position.x,
                            0f
                        ).normalized;

                        m_movement.MoveTowards(direction, m_info.walk.speed);

                        if (!IsFacingTarget())
                        {
                            CustomTurn();
                        }

                        elapsedTime += Time.deltaTime;

                        yield return null;
                    }
                    blinkCount++;
                    yield return null;
                }
                else
                {
                    var chosenBehavior = UnityEngine.Random.Range(0, 2) == 1 ? 0 : 1;

                    switch (chosenBehavior)
                    {
                        case 0:
                            Debug.Log("scytheWave");
                            yield return ChooseScytheWaveSpawn();
                            m_animation.SetAnimation(0, m_info.scytheWaveAttack.animation, false);
                            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.scytheWaveAttack.animation);
                            break;
                        case 1:
                            Debug.Log("DualSwordCombo");
                            yield return BlinkRoutine(BlinkState.DisappearBackward, BlinkState.AppearForward, new Vector2(20, 0), m_info.midAirHeight, false, true, false);
                            yield return DualSwordComboForEvadeRoutine();
                            break;
                    }
                    yield break;
                }
                yield return null;
                Debug.Log("done while loop");
            }
            
        }
       

        private IEnumerator FakeBlinkRoutine()
        {
            Debug.Log("fake blink");
           
            switch (m_fakeBlinkCount)
            {
                case 0:
                    m_fakeBlinkCount++;
                    m_fakeBlinkChosenDrillDashBehavior = UnityEngine.Random.Range(0, 2);
                    yield return BlinkRoutine(BlinkState.DisappearBackward, BlinkState.AppearBackward, new Vector2(20,0), 0,true, false, false);
                    break;
                case 1:
                    yield return m_fakeBlinkChosenDrillDashBehavior == 1 ? DrillDashComboRoutine() : DrillDash2Routine();
                    break;
            }
            Debug.Log("fake blink done");
        }

        private IEnumerator DrillDash2AttackRoutine()
        {
            Debug.Log("drill2route");
            m_stateHandle.Wait(State.ReevaluateSituation);
            if (IsTargetInRange(m_info.drillDash1Attack.range))
            {
                m_animation.EnableRootMotion(false, false);
                var drillCount = 0;
                while (drillCount < 2)
                {
                    m_animation.SetAnimation(0, m_info.groundToDrillAnimation, false);
                    var waitTime = m_animation.animationState.GetCurrent(0).AnimationEnd * 0.75f;
                    yield return new WaitForSeconds(waitTime);
                    m_drillDamage.SetActive(true);
                    m_hitbox.Disable();
                    m_animation.SetAnimation(4, m_drillMixAnimation, false);
                    m_character.physics.SetVelocity(m_info.drillDashSpeed * transform.localScale.x, 0);
                    m_animation.SetAnimation(0, m_info.drillDash1Attack.animation, false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillDash1Attack.animation);
                    m_animation.SetEmptyAnimation(4, 0);
                    m_hitbox.Enable();
                    m_movement.Stop();
                    m_animation.SetAnimation(0, m_info.drillToGroundAnimation, false);
                    m_drillDamage.SetActive(false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillToGroundAnimation);
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    if (!IsFacingTarget())
                        CustomTurn();

                    drillCount++;
                    yield return null;
                }

                m_fakeBlinkCount = 0;
                m_hitbox.SetCanBlockDamageState(false);
            }
            m_animation.SetAnimation(0, m_info.groundToDrillAnimation, false);
            var waitTime2 = m_animation.animationState.GetCurrent(0).AnimationEnd * 0.75f;
            yield return new WaitForSeconds(waitTime2);
            m_drillDamage.SetActive(true);
            m_hitbox.Disable();
            m_animation.SetAnimation(4, m_drillMixAnimation, false);
            m_character.physics.SetVelocity(m_info.drillDashSpeed * transform.localScale.x, 0);
            m_animation.SetAnimation(0, m_info.drillDash1Attack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillDash1Attack.animation);
            m_animation.SetEmptyAnimation(4, 0);
            m_hitbox.Enable();
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.drillToGroundAnimation, false);
            m_drillDamage.SetActive(false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillToGroundAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            yield return new WaitForSeconds(2f);
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
        }//new drill dash routine 2x
        private IEnumerator DrillDash2Routine()
        {
            Debug.Log("drill2route");
            if (IsTargetInRange(m_info.drillDash1Attack.range))
            {
                m_animation.EnableRootMotion(false, false);
                var drillCount = 0;
                while (drillCount < 2)
                {
                    m_animation.SetAnimation(0, m_info.groundToDrillAnimation, false);
                    var waitTime = m_animation.animationState.GetCurrent(0).AnimationEnd * 0.75f;
                    yield return new WaitForSeconds(waitTime);
                    m_drillDamage.SetActive(true);
                    m_hitbox.Disable();
                    m_animation.SetAnimation(4, m_drillMixAnimation, false);
                    m_character.physics.SetVelocity(m_info.drillDashSpeed * transform.localScale.x, 0);
                    m_animation.SetAnimation(0, m_info.drillDash1Attack.animation, false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillDash1Attack.animation);
                    m_animation.SetEmptyAnimation(4, 0);
                    m_hitbox.Enable();
                    m_movement.Stop();
                    m_animation.SetAnimation(0, m_info.drillToGroundAnimation, false);
                    m_drillDamage.SetActive(false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillToGroundAnimation);
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    if (!IsFacingTarget())
                        CustomTurn();

                    drillCount++;
                    yield return null;
                }
                
                m_fakeBlinkCount = 0;
                m_hitbox.SetCanBlockDamageState(false);
            }
            m_animation.SetAnimation(0, m_info.groundToDrillAnimation, false);
            var waitTime2 = m_animation.animationState.GetCurrent(0).AnimationEnd * 0.75f;
            yield return new WaitForSeconds(waitTime2);
            m_drillDamage.SetActive(true);
            m_hitbox.Disable();
            m_animation.SetAnimation(4, m_drillMixAnimation, false);
            m_character.physics.SetVelocity(m_info.drillDashSpeed * transform.localScale.x, 0);
            m_animation.SetAnimation(0, m_info.drillDash1Attack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillDash1Attack.animation);
            m_animation.SetEmptyAnimation(4, 0);
            m_hitbox.Enable();
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.drillToGroundAnimation, false);
            m_drillDamage.SetActive(false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillToGroundAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            yield return new WaitForSeconds(2f);
            
        }
        [SerializeField]
        private Transform m_drillPoint;
        [SerializeField]
        private RaySensor m_drillPointSensor;
        private IEnumerator DrillDashComboRoutine()
        {
            yield return BlinkRoutineWithFakeBlink(BlinkState.DisappearUpward, BlinkState.AppearUpward, new Vector2(30,30) ,20,false, false, false);
            Vector3 targetPos = m_lastTargetPos;
            Vector3 drillDirection = (targetPos - transform.position).normalized;
            if (!IsFacing(targetPos))
                CustomTurn();
            m_animation.SetAnimation(0, m_info.airTodrillDashDiagonal.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.airTodrillDashDiagonal);
            m_animation.SetAnimation(0, m_info.drillDashDiagonal.animation, true);
            while (!m_groundSensor.isDetecting)
            {
                transform.position += drillDirection * 150f * Time.deltaTime;
                yield return null;
            }
            m_hitbox.Disable();

            m_animation.SetAnimation(0, m_info.drillToGroundAnimation.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillToGroundAnimation.animation);
            m_character.physics.simulateGravity = true;
            m_model.transform.rotation = Quaternion.identity;
            m_animation.SetEmptyAnimation(4, 0);
            m_hitbox.Enable();
            m_movement.Stop();
            m_animation.DisableRootMotion();
            m_animation.SetAnimation(0, m_info.drillToGroundAnimation, false);
            m_drillDamage.SetActive(false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillToGroundAnimation);
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
            var waitTime = m_animation.animationState.GetCurrent(0).AnimationEnd * 0.75f;
            m_drillDamage.SetActive(true);
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
            m_drillDamage.SetActive(false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillToGroundAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            StopComboCounts();

            m_fakeBlinkCount = 0;
            m_hitbox.SetCanBlockDamageState(false);


            Debug.Log("drilldashcombo done");
        }
        [SerializeField]
        private GameObject m_heavySwordStab;
        [SerializeField]
        private GameObject m_swordStab;
        [SerializeField]
        private GameObject m_twinSlash;

        [Button]
        private void TestDualSwordCombo()
        {
            StartCoroutine(DualSwordComboAttackPattern1());
        }
        private IEnumerator DualSwordComboForEvadeRoutine()
        {
            Debug.Log("DualSwordComboEvadeRoutine()");
            if (!IsFacingTarget())
                CustomTurn();
            if (!IsTargetInRange(m_info.dualSwordComboAttackRange))
            {
                yield return BlinkRoutine(BlinkState.DisappearForward, BlinkState.AppearForward, new Vector2(20,0), m_info.midAirHeight, false, false, false);
                Debug.Log("Not in range, before downward slash");
            }
            if (!IsFacingTarget())
                CustomTurn();
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.downwardSlash1Attack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.downwardSlash1Attack.animation);
            Debug.Log("Done DownWardSlash");
            if (IsTargetInRange(m_info.dualSwordComboAttackRange))
            {
                Debug.Log("in range after downwardslash attack");
                m_swordStab.SetActive(true);
                m_animation.SetAnimation(0, m_info.swordStabAttack.animation, false);
                yield return new WaitForSeconds(.1f);
                m_swordStab.SetActive(false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.swordStabAttack.animation);
                m_animation.SetAnimation(0, m_info.heavySwordStabAttack.animation, false);
                yield return new WaitForSeconds(.1f);
                m_heavySwordStab.SetActive(true);
                yield return new WaitForSeconds(.1f);
                m_heavySwordStab.SetActive(false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.heavySwordStabAttack.animation);
                if (IsTargetInRange(m_info.dualSwordComboAttackRange))
                {
                    m_animation.EnableRootMotion(true, false);
                    m_animation.SetAnimation(0, m_info.downwardSlash2Attack.animation, false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.downwardSlash2Attack.animation);
                    var animTwinSlash = m_animation.SetAnimation(0, m_info.twinSlash1Attack.animation, false);
                    yield return new WaitForSeconds(.2f);
                    m_twinSlash.SetActive(true);
                    yield return new WaitForSeconds(.1f);
                    m_twinSlash.SetActive(false);
                    yield return new WaitForSpineAnimationComplete(animTwinSlash);
                    
                }

            }
            else
            {
                m_animation.SetAnimation(0, m_info.idleCombatAnimation.animation, true);
                yield return new WaitForSeconds(m_info.defaultIdleTime);
                Debug.Log("Start of projectileWaveSlash");
                yield return ProjectileWaveSlashForDualSwordPattern();

            }
            m_animation.DisableAnimation();
            m_animation.SetAnimation(0, m_info.idleCombatAnimation.animation, true);
            yield return new WaitForSeconds(m_info.defaultIdleTime);
            Debug.Log("DualSwordComboEvadeRoutine(): Done");
        }
        public void EnableRootMotion()
        {
            m_animation.EnableRootMotion(true, false);
        }
        public void DisableRootMotion()
        {
            m_animation.DisableRootMotion();
        }
        private IEnumerator  DualSwordComboAttackPattern1()// 
        {
            Debug.Log("phase1pattern1");
            m_stateHandle.Wait(State.ReevaluateSituation);
            if (!IsFacingTarget())
                CustomTurn(); 
            if (!IsTargetInRange(m_info.dualSwordComboAttackRange))
            {
                yield return BlinkRoutine(BlinkState.DisappearForward, BlinkState.AppearForward, new Vector2(20,0), m_info.midAirHeight, false, false, false);
                Debug.Log("Not in range, before downward slash");
            }
            if (!IsFacingTarget())
                CustomTurn();
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.downwardSlash1Attack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.downwardSlash1Attack.animation);
            m_animation.DisableRootMotion();
            Debug.Log("Done DownWardSlash");
            if (IsTargetInRange(m_info.dualSwordComboAttackRange))
            {
                m_animation.EnableRootMotion(true, false);
                Debug.Log("in range after downwardslash attack");
                m_swordStab.SetActive(true);
                m_animation.SetAnimation(0, m_info.swordStabAttack.animation, false);
                yield return new WaitForSeconds(.1f);
                m_swordStab.SetActive(false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.swordStabAttack.animation);
                m_animation.SetAnimation(0, m_info.heavySwordStabAttack.animation, false);
                yield return new WaitForSeconds(.1f);
                m_heavySwordStab.SetActive(true);
                yield return new WaitForSeconds(.1f);
                m_heavySwordStab.SetActive(false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.heavySwordStabAttack.animation);
                m_animation.DisableRootMotion();
                if (IsTargetInRange(m_info.dualSwordComboAttackRange))
                {
                    m_animation.EnableRootMotion(true, false);
                    m_animation.SetAnimation(0, m_info.downwardSlash2Attack.animation, false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.downwardSlash2Attack.animation);
                    var animTwinSlash = m_animation.SetAnimation(0, m_info.twinSlash1Attack.animation, false);
                    yield return new WaitForSeconds(.2f);
                    m_twinSlash.SetActive(true);
                    yield return new WaitForSeconds(.1f);
                    m_twinSlash.SetActive(false);
                    yield return new WaitForSpineAnimationComplete(animTwinSlash);

                  
                }
                
            }
            else
            {
                m_animation.DisableRootMotion();
                m_animation.SetAnimation(0, m_info.idleCombatAnimation.animation, true);
                yield return new WaitForSeconds(m_info.defaultIdleTime);
                Debug.Log("Start of projectileWaveSlash");
                yield return ProjectileWaveSlashForDualSwordPattern();

            }
            m_animation.DisableRootMotion();
            m_animation.SetAnimation(0, m_info.idleCombatAnimation.animation, true);
            yield return new WaitForSeconds(m_info.defaultIdleTime);
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            Debug.Log("phase1pattern1 done");
        }
        private IEnumerator ProjectileWaveSlashForDualSwordPattern()//ProjectileWaveSlash
        {
            Debug.Log("phase1pattern2");
            float walkDuration = 3f;
            float elapsedTime = 0f;
            if (IsTargetInRange(m_info.downwardSlash1Attack.range))
            {
                yield return BlinkRoutine(BlinkState.DisappearForward, BlinkState.AppearForward, new Vector2(25,0), m_info.midAirHeight, false, false, false);
            }
            yield return ChooseScytheWaveSpawn();
            m_animation.SetAnimation(0, m_info.projectilWaveSlashGround1Attack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.projectilWaveSlashGround1Attack.animation);
            m_animation.SetAnimation(0, m_info.walk.animation, true);
            while (elapsedTime < walkDuration)
            {
                Vector2 direction = new Vector2(
                    m_targetInfo.position.x - transform.position.x,
                    0f
                ).normalized;

                m_movement.MoveTowards(direction, m_info.walk.speed);

                if (!IsFacingTarget())
                {
                    CustomTurn();
                }

                elapsedTime += Time.deltaTime;

                yield return null;
            }
            yield return ChooseScytheWaveSpawn();
            m_animation.SetAnimation(0, m_info.projectilWaveSlashGround1Attack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.projectilWaveSlashGround1Attack.animation);
            m_animation.SetAnimation(0, m_info.projectilWaveSlashGround2Attack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.projectilWaveSlashGround2Attack.animation);
            Debug.Log("phase1pattern2 done");
        }
        private IEnumerator ProjectileWaveSlashPhase1Pattern2()//ProjectileWaveSlash
        {
            Debug.Log("phase1pattern2");
            m_stateHandle.Wait(State.ReevaluateSituation);
            float walkDuration = 3f;
            float elapsedTime = 0f;
            if (IsTargetInRange(m_info.projectileWaveSlashAttackRange))
            {
                yield return BlinkRoutine(BlinkState.DisappearForward, BlinkState.AppearForward, new Vector2(25, 0), m_info.midAirHeight, false, false, false);
            }

            yield return ChooseScytheWaveSpawn();
            m_animation.SetAnimation(0, m_info.projectilWaveSlashGround1Attack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.projectilWaveSlashGround1Attack.animation);
            //walk to player 
            while (elapsedTime < walkDuration)
            {
                Vector2 direction = new Vector2(
                    m_targetInfo.position.x - transform.position.x,
                    0f
                ).normalized;

                m_movement.MoveTowards(direction, m_info.walk.speed);

                if (!IsFacingTarget())
                {
                    CustomTurn();
                }

                elapsedTime += Time.deltaTime;

                yield return null;
            }
            m_movement.Stop();
            yield return ChooseScytheWaveSpawn();
            m_animation.SetAnimation(0, m_info.projectilWaveSlashGround1Attack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.projectilWaveSlashGround1Attack.animation);
            m_animation.SetAnimation(0, m_info.projectilWaveSlashGround2Attack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.projectilWaveSlashGround2Attack.animation);
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            Debug.Log("phase1pattern2 done");
        }

        private IEnumerator DrillDashPhase1Pattern3()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            if (IsTargetInRange(m_info.drillDashAttackRange))
            {
                Debug.Log("Player is in range, go to next pattern");
                m_attackDecider.hasDecidedOnAttack = false;
                m_stateHandle.ApplyQueuedState();
                yield break;

            }
            yield return BlinkRoutine(BlinkState.DisappearForward, BlinkState.AppearForward, new Vector2(25, 0), m_info.midAirHeight, false, false, false);
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
            m_drillDamage.SetActive(false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillToGroundAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            yield return new WaitForSeconds(m_info.phase1Pattern3IdleTime);
            yield return EvadePlayerRoutine();
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            Debug.Log("phase1pattern3 done");
        }
        [SerializeField]
        private ParticleSystem m_drillDiagonalVFX;
        private void DrillDiagonalEvent()
        {
            m_drillDiagonalVFX.Play();
            m_drillDamageDiagonal.SetActive(true);
        }
        private void GeyserBurstSpawnEvent()
        {
            GameObject geyserToSpawn = null;
            m_canGeyserBurst = false;
            switch (m_currentSwordState)
            {
                case SwordState.BlackBlood:
                    geyserToSpawn = m_info.geyserRed;
                    break;
                case SwordState.Poison:
                    geyserToSpawn = m_info.geyserPurple;
                    break;
                case SwordState.Acid:
                    geyserToSpawn = m_info.geyserGreen;
                    break;
            }
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
        }
        private IEnumerator GeyserBurstPhase1Pattern4()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            Debug.Log("phase1pattern4");
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
                m_attackDecider.hasDecidedOnAttack = false;
                m_stateHandle.ApplyQueuedState();

            Debug.Log("phase1pattern4 done");
        }

        private void SpawnGeysers(Vector2[] patternLocations, GameObject geyser)
        {
            Debug.Log("spawngey");
            for (int i = 0; i < patternLocations.Length; i++)
            {
                var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(geyser, gameObject.scene);
                instance.SpawnAt(patternLocations[i], Quaternion.identity);
            }
            Debug.Log("spawngey done");
        }
        [SerializeField]
        private GameObject m_twinSlashMidAir;
        private IEnumerator DualSwordComboPhase2Pattern1()
        {
            Debug.Log("phase2pattern1");
            m_stateHandle.Wait(State.ReevaluateSituation);
            if(!IsTargetInRange(m_info.dualSwordComboAttackRange2))
                yield return BlinkRoutine(BlinkState.DisappearForward, BlinkState.AppearForward, new Vector2(20, 0), m_info.midAirHeight, false, false, false);
            m_animation.SetAnimation(0, m_info.downwardSlash1Attack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.downwardSlash1Attack.animation);
            if (!IsTargetInRange(m_info.dualSwordComboAttackRange2))
                yield return BlinkRoutine(BlinkState.DisappearForward, BlinkState.AppearForward, new Vector2(20, 0), m_info.midAirHeight, false, false, false);
            Debug.Log("in range after downwardslash attack");
            m_swordStab.SetActive(true);
            m_animation.SetAnimation(0, m_info.swordStabAttack.animation, false);
            yield return new WaitForSeconds(.1f);
            m_swordStab.SetActive(false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.swordStabAttack.animation);
            m_animation.SetAnimation(0, m_info.heavySwordStabAttack.animation, false);
            yield return new WaitForSeconds(.1f);
            m_heavySwordStab.SetActive(true);
            yield return new WaitForSeconds(.1f);
            m_heavySwordStab.SetActive(false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.heavySwordStabAttack.animation);
            if (!IsTargetInRange(m_info.dualSwordComboAttackRange2))
                yield return BlinkRoutine(BlinkState.DisappearForward, BlinkState.AppearForward, new Vector2(20, 0), m_info.midAirHeight, false, false, false);
            m_animation.SetAnimation(0, m_info.downwardSlash2Attack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.downwardSlash2Attack.animation);
            Debug.Log("DoneDownWardSlash2Attack");
            if (!m_targetInfo.isCharacterGrounded)
            {
                Debug.Log("target is not grounded");
                yield return BlinkRoutine(BlinkState.DisappearForward, BlinkState.AppearForward, new Vector2(20, 0), m_info.midAirHeight, false, false, false);
                m_animation.SetAnimation(0, m_info.twinSlash2Attack.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.twinSlash2Attack.animation);
            }
            else
            {
                Debug.Log("target is grounded");
                yield return BlinkRoutine(BlinkState.DisappearForward, BlinkState.AppearForward, new Vector2(20, 0), m_info.midAirHeight, false, false, false);    
                m_animation.SetAnimation(0, m_info.twinSlash1Attack.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.twinSlash1Attack.animation);
            }
            yield return new WaitUntil(() => m_groundSensor.isDetecting);
            Debug.Log("Done");
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator ProjectileWaveSlashPhase2Pattern2()
        {
            Debug.Log("phase2pattern2");
            m_stateHandle.Wait(State.ReevaluateSituation);
            if(IsTargetInRange(m_info.projectileWaveSlashAttackRange2))
                yield return BlinkRoutine(BlinkState.DisappearForward, BlinkState.AppearForward, new Vector2(25, 0), m_info.midAirHeight, false, false, false);

            yield return ChooseScytheWaveSpawn();
            m_animation.SetAnimation(0, m_info.projectilWaveSlashGround1Attack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.projectilWaveSlashGround1Attack.animation);
            m_animation.SetAnimation(0, m_info.projectilWaveSlashGround2Attack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.projectilWaveSlashGround2Attack.animation);
            //needs to be randomized
            var random = UnityEngine.Random.RandomRange(0, 2);
            switch (random)
            {
                case 0:
                    yield return ChooseScytheWaveSpawn();
                    m_animation.SetAnimation(0, m_info.projectilWaveSlashGround1Attack.animation, false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.projectilWaveSlashGround1Attack.animation);
                    break;
                case 1:
                    yield return ChooseScytheWaveSpawn();
                    m_animation.SetAnimation(0, m_info.scytheWaveAttack.animation, false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.scytheWaveAttack.animation);                           
                    break;
            }
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            Debug.Log("phase2pattern2 done");
        }

        private IEnumerator ScytheWavePhase2Pattern3()
        {
            Debug.Log("phase2pattern3");
            m_stateHandle.Wait(State.ReevaluateSituation);
            if (!IsTargetInRange(m_info.scytheWaveAttackRange2))
                yield return BlinkRoutine(BlinkState.DisappearBackward, BlinkState.AppearBackward, new Vector2(60, 0), m_info.midAirHeight, false, false, false);

            yield return ChooseScytheWaveSpawn();
            if (!IsFacingTarget())
                CustomTurn();

            m_animation.SetAnimation(0, m_info.scytheDoubleWaveAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.scytheDoubleWaveAttack.animation);
            m_animation.SetAnimation(0, m_info.idleCombatAnimation.animation, true);
            yield return new WaitForSeconds(m_info.defaultIdleTime); ;
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            Debug.Log("phase2pattern3Done");
        }

    private IEnumerator GeyserBurstPhase2Pattern5()
    {
        Debug.Log("phase2pattern5");
        m_stateHandle.Wait(State.ReevaluateSituation);
            //!if geyserburt counter == 0
                DrillDashComboRoutine();
            //else
            //check blade state
            //ifbladenormalState
            //choose another pattern
            //esle

                var isMidAir = UnityEngine.Random.Range(0, 2) == 1 ? true : false;
            m_phase2pattern5Count = m_phase2pattern5Count > 3 ? 3 : m_phase2pattern5Count;
            switch (m_phase2pattern5Count)
            {
                case 0:
                    m_phase2pattern5Count++;
                    enabled = true;
                    m_blinkCoroutine = StartCoroutine(BlinkRoutine(BlinkState.DisappearForward, isMidAir ? BlinkState.AppearUpward : BlinkState.AppearForward, new Vector2(40, 0), m_info.midAirHeight,true, false, isMidAir ? true : false));
                    yield return new WaitUntil(() => m_blinkCoroutine == null);
                    enabled = false;
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
                    enabled = true;
                    m_blinkCoroutine = StartCoroutine(BlinkRoutine(BlinkState.DisappearForward, BlinkState.AppearForward, new Vector2(15, 0), 0, false, false, false));
                    yield return new WaitUntil(() => m_blinkCoroutine == null);
                    enabled = false;
                    break;
                case 2:
                    m_phase2pattern5Count++;
                    if (isMidAir)
                    {
                        /*if (m_blinkCoroutine != null)
                            yield return new WaitUntil(() => m_blinkCoroutine == null);
*/
                        enabled = true;
                        m_blinkCoroutine = StartCoroutine(BlinkRoutine(BlinkState.DisappearUpward, BlinkState.AppearUpward, new Vector2(60, 0), 50, false, false, true));
                        yield return new WaitUntil(() => m_blinkCoroutine == null);
                        enabled = false;
                    }
                    else
                    {
                        m_phase2pattern5Count = 0;
                        if (IsTargetInRange(m_info.drillDash1Attack.range))
                        {
                           
                            m_animation.EnableRootMotion(false, false);
                            var drillCount = 0;
                            while (drillCount < 2)
                            {
                                m_animation.SetAnimation(0, m_info.groundToDrillAnimation, false);
                                var waitTime = m_animation.animationState.GetCurrent(0).AnimationEnd * 0.75f;
                                yield return new WaitForSeconds(waitTime);
                                m_drillDamage.SetActive(true);
                                m_hitbox.Disable();
                                m_animation.SetAnimation(4, m_drillMixAnimation, false);
                                m_character.physics.SetVelocity(m_info.drillDashSpeed * transform.localScale.x, 0);
                                m_animation.SetAnimation(0, m_info.drillDash1Attack.animation, false);
                                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillDash1Attack.animation);
                                m_animation.SetEmptyAnimation(4, 0);
                                m_hitbox.Enable();
                                m_movement.Stop();
                                m_animation.SetAnimation(0, m_info.drillToGroundAnimation, false);
                                m_drillDamage.SetActive(false);
                                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillToGroundAnimation);
                                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                                if (!IsFacingTarget())
                                    CustomTurn();

                                drillCount++;
                                yield return null;
                            }

                            m_fakeBlinkCount = 0;
                            m_fakeBlinkRoutine = null;
                            m_hitbox.SetCanBlockDamageState(false);
                            if (m_alterBladeCoroutine == null)
                                m_stateHandle.ApplyQueuedState();
                        }
                        else
                        {
                            /*if (m_blinkCoroutine != null)
                                yield return new WaitUntil(() => m_blinkCoroutine == null);
*/
                            enabled = true;
                            m_blinkCoroutine = StartCoroutine(BlinkRoutine(BlinkState.DisappearBackward, BlinkState.AppearBackward, new Vector2(50, 0), 0,false, false, false));
                            yield return new WaitUntil(() => m_blinkCoroutine == null);
                            enabled = false;
                        }
                    }
                    break;
                case 3:
                    m_phase2pattern5Count = 0;
                    m_drillDashComboCount++;
                    m_drillDashComboCount = m_drillDashComboCount > 1 ? 1 : m_drillDashComboCount;
                    switch (m_drillDashComboCount)
                    {
                        case 0:
                            m_drillDashComboCount++;
                            /*if (m_blinkCoroutine != null)
                                yield return new WaitUntil(() => m_blinkCoroutine == null);
*/
                            enabled = true;
                            yield return BlinkRoutine(BlinkState.DisappearUpward, BlinkState.AppearUpward, new Vector2(60, 0), 50,false, false, true);
                            yield return new WaitUntil(() => m_blinkCoroutine == null);
                            enabled = false;
                            break;
                        case 1:
                            m_drillDashComboCount = 0;
                            m_lastTargetPos = m_targetInfo.position;
                            m_hitbox.Disable();
                            m_animation.DisableRootMotion();
                            if (!IsFacingTarget())
                                CustomTurn();
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
                            while (time < .25f || !m_groundSensor.isDetecting)
                            {
                                m_character.physics.SetVelocity((m_character.facing == HorizontalDirection.Right ? m_info.drillDashSpeed : -m_info.drillDashSpeed) * m_model.transform.right);
                                time += Time.deltaTime;
                                m_groundSensor.multiRaycast.SetCastDistance(100);
                                yield return null;
                            }
                            m_groundSensor.multiRaycast.SetCastDistance(1);
                            m_character.physics.simulateGravity = true;
                            m_model.transform.rotation = Quaternion.identity;
                            m_animation.SetEmptyAnimation(4, 0);
                            m_hitbox.Enable();
                            m_movement.Stop();
                            m_animation.SetAnimation(0, m_info.drillToGroundAnimation, false);
                            m_drillDamage.SetActive(false);
                            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillToGroundAnimation);
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
                            var waitTime = m_animation.animationState.GetCurrent(0).AnimationEnd * 0.75f;
                            m_drillDamage.SetActive(true);
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
                            m_drillDamage.SetActive(false);
                            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.drillToGroundAnimation);
                            m_animation.SetAnimation(0, m_info.idleAnimation, true);
                            StopComboCounts();

                            m_fakeBlinkCount = 0;
                            m_fakeBlinkRoutine = null;
                            m_hitbox.SetCanBlockDamageState(false);
                            if (m_alterBladeCoroutine == null)
                                m_stateHandle.ApplyQueuedState();
                            break;
                    }
                    break;
            }
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            Debug.Log("phase2pattern5 done");
        }

        private IEnumerator FakeBlink1Pattern5Phase1()
        {
            Debug.Log("fake blink");
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_fakeBlinkChosenDrillDashBehavior = UnityEngine.Random.Range(0, 2);
           // yield return BlinkRoutine(BlinkState.DisappearBackward, BlinkState.AppearBackward, new Vector2(40, 0), 0, true, false, false);
           // yield return m_fakeBlinkChosenDrillDashBehavior == 1 ? DrillDashComboRoutine() : DrillDash2Routine();
            yield return DrillDashComboRoutine();
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            Debug.Log("fake blink done");
        }
        private IEnumerator FakeBlink2Pattern5Phase2()
        {
            Debug.Log("fake blink");
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_fakeBlinkChosenDrillDashBehavior = UnityEngine.Random.Range(0, 2);
            yield return BlinkRoutine(BlinkState.DisappearBackward, BlinkState.AppearBackward, new Vector2(50, 0), 0, true, false, false);
            yield return m_fakeBlinkChosenDrillDashBehavior == 1 ? DrillDashComboRoutine() : DualSwordComboForFakeBlinkPhase2();
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            Debug.Log("fake blink done");
        }
        private IEnumerator DualSwordComboForFakeBlinkPhase2()
        {
            Debug.Log("DualSwordComboForFakeBlinkPhase2");
            if (!IsTargetInRange(m_info.dualSwordComboAttackRange2))
                yield return BlinkRoutine(BlinkState.DisappearForward, BlinkState.AppearForward, new Vector2(20, 0), m_info.midAirHeight, false, false, false);
            m_animation.SetAnimation(0, m_info.downwardSlash1Attack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.downwardSlash1Attack.animation);
            if (!IsTargetInRange(m_info.dualSwordComboAttackRange2))
                yield return BlinkRoutine(BlinkState.DisappearForward, BlinkState.AppearForward, new Vector2(20, 0), m_info.midAirHeight, false, false, false);
            Debug.Log("in range after downwardslash attack");
            m_swordStab.SetActive(true);
            m_animation.SetAnimation(0, m_info.swordStabAttack.animation, false);
            yield return new WaitForSeconds(.1f);
            m_swordStab.SetActive(false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.swordStabAttack.animation);
            m_animation.SetAnimation(0, m_info.heavySwordStabAttack.animation, false);
            yield return new WaitForSeconds(.1f);
            m_heavySwordStab.SetActive(true);
            yield return new WaitForSeconds(.1f);
            m_heavySwordStab.SetActive(false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.heavySwordStabAttack.animation);
            if (!IsTargetInRange(m_info.dualSwordComboAttackRange2))
                yield return BlinkRoutine(BlinkState.DisappearForward, BlinkState.AppearForward, new Vector2(20, 0), m_info.midAirHeight, false, false, false);
            m_animation.SetAnimation(0, m_info.downwardSlash2Attack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.downwardSlash2Attack.animation);
            Debug.Log("DoneDownWardSlash2Attack");
            if (!m_targetInfo.isCharacterGrounded)
            {
                Debug.Log("target is not grounded");
                yield return BlinkRoutine(BlinkState.DisappearForward, BlinkState.AppearForward, new Vector2(20, 0), m_info.midAirHeight, false, false, false);
                m_animation.SetAnimation(0, m_info.twinSlash2Attack.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.twinSlash2Attack.animation);
            }
            else
            {
                Debug.Log("target is grounded");
                yield return BlinkRoutine(BlinkState.DisappearForward, BlinkState.AppearForward, new Vector2(20, 0), m_info.midAirHeight, false, false, false);
                m_animation.SetAnimation(0, m_info.twinSlash1Attack.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.twinSlash1Attack.animation);
            }
            yield return new WaitUntil(() => m_groundSensor.isDetecting);
            Debug.Log("Done");
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
                //m_movement.Stop();
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
        }

        private IEnumerator BlinkRoutineWithFakeBlink(BlinkState disappearState, BlinkState appearState, Vector2 positionOffset, float midAirHeight, bool fakeBlink, bool evadeBlink, bool isMidAir, bool oppositeSide = false)
        {
            Debug.Log("blinkroutine");
            m_blinkCount++;
            m_character.physics.SetVelocity(0f, 0f);
            m_drillDamage.SetActive(false);
            m_heavySwordStab.SetActive(false);
            m_swordStab.SetActive(false);
            m_twinSlash.SetActive(false);
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
            //var blinkWait = 0f;
            lastPos = new Vector2(m_targetInfo.position.x + (m_targetInfo.transform.GetComponent<Character>().facing == HorizontalDirection.Right ? -positionOffset.x : positionOffset.x), m_targetInfo.position.y + positionOffset.y);

            if (fakeBlink)
            {
                var blinkCount = 0;
                transform.position = lastPos;
                if (!IsFacingTarget())
                    CustomTurn();
                m_animation.SetAnimation(0, m_info.blinkAppearForwardAnimation.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.blinkAppearForwardAnimation.animation);
                m_animation.SetAnimation(0, m_info.blinkDisappearUpwardAnimation.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.blinkDisappearUpwardAnimation.animation);
                m_animation.SetAnimation(0, m_info.blinkFakeAnimation, true);
                while (blinkCount < m_info.fakeBlinkCount)
                {

                    transform.position = RandomTeleportPoint(transform.position);
                    if (!IsFacingTarget())
                        CustomTurn();
                    yield return new WaitForSeconds(0.3f);
                    Debug.Log(blinkCount++);
                    blinkCount++;
                    yield return null;
                }
                yield return new WaitForSeconds(0.3f);
                m_model.SetActive(false);
                yield return new WaitForSeconds(3f);
                Vector2 positionOffsetForFakeBlink = new Vector2(7.5f, 0f);
                lastPos = new Vector2(m_targetInfo.position.x + (m_targetInfo.transform.GetComponent<Character>().facing == HorizontalDirection.Right ? -positionOffsetForFakeBlink.x : positionOffsetForFakeBlink.x), m_targetInfo.position.y + positionOffsetForFakeBlink.y);
            }


            transform.position = lastPos;
            m_blinkFX.Play();
            yield return new WaitForSeconds(m_info.blinkDuration);
            m_model.SetActive(true);
            m_legCollider.enabled = true;
            m_bodyCollider.enabled = true;
            if (!IsFacingTarget())
                CustomTurn();
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


            yield return new WaitForSeconds(.1f);
            m_animation.SetAnimation(0, m_blinkAppearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_blinkAppearAnimation);
            m_animation.SetAnimation(0, m_info.idleCombatAnimation, true);
            if (!IsFacingTarget())
                CustomTurn();
            m_hitbox.SetCanBlockDamageState(false);
            Debug.Log("blinkroutine done");
        }

        private IEnumerator BlinkOut(BlinkState disappearState)
        {
            Debug.Log("blink out routine");
            m_blinkCount++;
            m_character.physics.SetVelocity(0f, 0f);
            m_drillDamage.SetActive(false);
            m_heavySwordStab.SetActive(false);
            m_swordStab.SetActive(false);
            m_twinSlash.SetActive(false);
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
            yield return null;
            Debug.Log("BlinkOutDone");
        }
        private IEnumerator BlinkIn( BlinkState appearState, Vector2 positionOffset)
        {
            Debug.Log("blinkIn");
            var lastPos = transform.position;
            lastPos = new Vector2(m_targetInfo.position.x + (m_targetInfo.transform.GetComponent<Character>().facing == HorizontalDirection.Right ? -positionOffset.x : positionOffset.x), m_targetInfo.position.y + positionOffset.y);
            transform.position = lastPos;
            m_blinkFX.Play();
            yield return new WaitForSeconds(m_info.blinkDuration);
            
            m_model.SetActive(true);
            if (!IsFacingTarget())
                CustomTurn();
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


            yield return new WaitForSeconds(.1f);
            m_legCollider.enabled = true;
            m_bodyCollider.enabled = true;
            m_character.physics.simulateGravity = true;
            m_animation.SetAnimation(0, m_blinkAppearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_blinkAppearAnimation);
            if (!m_groundSensor.isDetecting)
            {
                m_animation.SetAnimation(0, m_info.fallAnimation, true);
                yield return new WaitUntil(() => m_groundSensor.isDetecting);
                m_animation.SetAnimation(0, m_info.landAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.landAnimation);
            }
            m_hitbox.SetCanBlockDamageState(false);
            Debug.Log("blinkInroutine done");
        }
        private IEnumerator BlinkRoutine(BlinkState disappearState, BlinkState appearState, Vector2 positionOffset, float midAirHeight, bool fakeBlink, bool evadeBlink, bool isMidAir, bool oppositeSide = false)
        {
            Debug.Log("blinkroutine");
                m_blinkCount++;
                m_character.physics.SetVelocity(0f, 0f);
                m_drillDamage.SetActive(false);
                m_heavySwordStab.SetActive(false);
                m_swordStab.SetActive(false);
                m_twinSlash.SetActive(false);
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
                //var blinkWait = 0f;
                lastPos = new Vector2(m_targetInfo.position.x + (m_targetInfo.transform.GetComponent<Character>().facing == HorizontalDirection.Right ? -positionOffset.x : positionOffset.x), m_targetInfo.position.y + positionOffset.y);

            if (fakeBlink)
                {
                var blinkCount = 0;
                transform.position = lastPos;
                if (!IsFacingTarget())
                    CustomTurn();
                m_animation.SetAnimation(0, m_info.blinkAppearForwardAnimation.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.blinkAppearForwardAnimation.animation);   
                m_animation.SetAnimation(0, m_info.blinkDisappearUpwardAnimation.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.blinkDisappearUpwardAnimation.animation);
                m_animation.SetAnimation(0, m_info.blinkFakeAnimation, true);
                while (blinkCount < m_info.fakeBlinkCount)
                {

                    transform.position = RandomTeleportPoint(transform.position);                  
                    if (!IsFacingTarget())
                        CustomTurn();
                    yield return new WaitForSeconds(0.3f);
                    Debug.Log(blinkCount++);
                    blinkCount++;
                    yield return null;
                }
                yield return new WaitForSeconds(0.3f);      
                m_model.SetActive(false);
                yield return new WaitForSeconds(3f);
                Vector2 positionOffsetForFakeBlink = new Vector2(7.5f, 0f);
                lastPos = new Vector2(m_targetInfo.position.x + (m_targetInfo.transform.GetComponent<Character>().facing == HorizontalDirection.Right ? -positionOffsetForFakeBlink.x : positionOffsetForFakeBlink.x), m_targetInfo.position.y + positionOffsetForFakeBlink.y);
            }


                transform.position = lastPos;
                m_blinkFX.Play();
                yield return new WaitForSeconds(m_info.blinkDuration);
                m_character.physics.simulateGravity = true;
                m_model.SetActive(true);
                
            if (!IsFacingTarget())
                CustomTurn();
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
                

                yield return new WaitForSeconds(.1f);
            m_legCollider.enabled = true;
            m_bodyCollider.enabled = true;
            m_character.physics.simulateGravity = true;
            m_animation.SetAnimation(0, m_blinkAppearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_blinkAppearAnimation);   
            if (!m_groundSensor.isDetecting)
            {
                m_animation.SetAnimation(0, m_info.fallAnimation, true);
                yield return new WaitUntil(() => m_groundSensor.isDetecting);
                m_animation.SetAnimation(0, m_info.landAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.landAnimation);
            }
            m_hitbox.SetCanBlockDamageState(false);
            Debug.Log("blinkroutine done");
        }


        private Vector3 RandomTeleportPoint(Vector3 storedPos)
        {
            Debug.Log("randomtp");
            Vector3 randomPos = storedPos;
            while (Vector2.Distance(storedPos, randomPos) <= 50f)
            {
                randomPos = m_randomSpawnCollider.bounds.center + new Vector3(
               (UnityEngine.Random.value - 0.5f) * m_randomSpawnCollider.bounds.size.x,
               (UnityEngine.Random.value - 0.5f) * m_randomSpawnCollider.bounds.size.y,
               (UnityEngine.Random.value - 0.5f) * m_randomSpawnCollider.bounds.size.z);
            }
            return randomPos;
            Debug.Log("randomtp done");
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
        [SerializeField] private bool m_canUseAlternateSwordState = true;
        private int m_alterBladeCounterToActivate = 0;
        private readonly SwordState[] m_alternateSwordStates =
        {
        SwordState.BlackBlood,
        SwordState.Poison,
        SwordState.Acid
        };
        private SwordState GetNextRandomSwordState()
        {
            var availableStates = m_alternateSwordStates
                .Where(state => !m_usedSwordStates.Contains(state))
                .ToList();

            if (availableStates.Count == 0)
            {
                m_usedSwordStates.Clear();

                return SwordState.Normal;
            }

            var selectedState = availableStates[
                UnityEngine.Random.Range(0, availableStates.Count)
            ];

            m_usedSwordStates.Add(selectedState);

            return selectedState;
        }
        [SerializeField] private int m_alternateBladeAttackCounter = 3;

        private int m_currentAlternateBladeAttackCounter;
        private IEnumerator AlterBladeMonitorRoutine()
        {
            Debug.Log("altermonitor");
            m_stateHandle.Wait(State.Attacking);
            m_currentSwordState = GetNextRandomSwordState();
            yield return AlterBladeRoutine(m_currentSwordState);
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            Debug.Log("altermonitor done");
        }

        private IAIAnimationInfo animationChangeSwordString;
        private IEnumerator AlterBladeRoutine(SwordState swordState)
        {
            Debug.Log("alterblade");
            StopComboCounts();
            if (!m_groundSensor.isDetecting)
            {
                m_animation.DisableRootMotion();
                m_animation.SetAnimation(0, m_info.fallAnimation, true);
                yield return new WaitUntil(() => m_groundSensor.isDetecting);
                m_animation.SetAnimation(0, m_info.landAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.landAnimation);
            }
            m_animation.SetAnimation(0, m_info.idleCombatAnimation, true);
            switch (swordState)
            {
                case SwordState.Normal:
                    animationChangeSwordString = m_info.swordChangeAnimationToNormal;
                    m_swordMixAnimation = m_info.swordNormalMixAnimation.animation;
                    m_drillMixAnimation = m_info.drillNormalMixAnimation.animation;
                    m_projectileLauncher = new ProjectileLauncher(m_info.slashNormalProjectile.projectileInfo, m_projectilePoint);
                    m_scytheWaveLauncher = new ProjectileLauncher(m_info.scytheWaveNormalProjectile.projectileInfo, m_scytheWavePoint);
                    break;
                case SwordState.BlackBlood:
                    animationChangeSwordString = m_info.swordChangeAnimationToRed;
                    m_swordMixAnimation = m_info.swordRedMixAnimation.animation;
                    m_drillMixAnimation = m_info.drillRedMixAnimation.animation;
                    m_projectileLauncher = new ProjectileLauncher(m_info.slashBlackbloodProjectile.projectileInfo, m_projectilePoint);
                    m_scytheWaveLauncher = new ProjectileLauncher(m_info.scytheWaveBlackbloodProjectile.projectileInfo, m_scytheWavePoint);
                    break;
                case SwordState.Poison:
                    animationChangeSwordString = m_info.swordChangeAnimationToPurple;
                    m_swordMixAnimation = m_info.swordPurpleMixAnimation.animation;
                    m_drillMixAnimation = m_info.drillPurpleMixAnimation.animation;
                    m_projectileLauncher = new ProjectileLauncher(m_info.slashPoisonProjectile.projectileInfo, m_projectilePoint);
                    m_scytheWaveLauncher = new ProjectileLauncher(m_info.scytheWavePoisonProjectile.projectileInfo, m_scytheWavePoint);
                    break;
                case SwordState.Acid:
                    animationChangeSwordString = m_info.swordChangeAnimationToGreen;
                    m_swordMixAnimation = m_info.swordGreenMixAnimation.animation;
                    m_drillMixAnimation = m_info.drillGreenMixAnimation.animation;
                    m_projectileLauncher = new ProjectileLauncher(m_info.slashAcidProjectile.projectileInfo, m_projectilePoint);
                    m_scytheWaveLauncher = new ProjectileLauncher(m_info.scytheWaveAcidProjectile.projectileInfo, m_scytheWavePoint);
                    break;
            }
            m_animation.SetAnimation(0, animationChangeSwordString.animation, false);
            m_animation.SetAnimation(5, m_swordMixAnimation, false).MixBlend = MixBlend.First;
            yield return new WaitForAnimationComplete(m_animation.animationState, animationChangeSwordString.animation);
            m_animation.SetAnimation(0, m_info.idleCombatAnimation, true);
            Debug.Log("alterblade done");
        }
        #endregion 

        private void UpdateAttackDeciderList()
        {
            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseOne:
                    m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Phase1Pattern3, m_info.phase1Pattern1Range));
                    break;
                case Phase.PhaseTwo:
                    m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Phase1Pattern2, m_info.phase1Pattern1Range));
                    break;
                case Phase.Wait:
                    break;
            }
            
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private Vector2 GroundPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1000, DChildUtility.GetEnvironmentMask());
            return hit.point;
        }


      

        protected override void Awake()
        {

            // m_turnHandle.TurnDone += OnTurnDone;
            base.Awake();
            m_projectileLauncher = new ProjectileLauncher(m_info.slashNormalProjectile.projectileInfo, m_projectilePoint);
            m_scytheWaveLauncher = new ProjectileLauncher(m_info.scytheWaveNormalProjectile.projectileInfo, m_scytheWavePoint);
            m_attackDecider = new RandomAttackDecider<Attack>();
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            UpdateAttackDeciderList();
            
        }

        protected override void Start()
        {
            base.Start();
            m_spineListener.Subscribe(m_info.slashNormalProjectile.launchOnEvent, LaunchProjectile);
            m_spineListener.Subscribe(m_info.scytheWaveNormalProjectile.launchOnEvent, LaunchScytheWave);
            m_spineListener.Subscribe(m_info.geyserStartRed, GeyserBurstSpawnEvent);
            m_spineListener.Subscribe(m_info.geyserStartGreen, GeyserBurstSpawnEvent);
            m_spineListener.Subscribe(m_info.geyserStartPurple, GeyserBurstSpawnEvent);
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

        private int geyserBurstCurrentCount;
        private bool m_canGeyserBurst = true;
        private void Update()
        {
            m_phaseHandle.MonitorPhase();
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    break;
                case State.Intro:
                    StartCoroutine(IntroRoutine());
                    break;
                case State.Phasing:            
                    StartCoroutine(ChangePhaseRoutine());
                    break;
                #region Turning
                //case State.Turning:
                //    m_phaseHandle.allowPhaseChange = false;
                //    m_stateHandle.Wait(m_turnState);
                //    m_turnHandle.Execute();
                //    m_movement.Stop();
                //    break;
                #endregion
                case State.Attacking:
                    m_lastTargetPos = m_targetInfo.position;
                    StopAllCoroutines();
                    Debug.Log("CURRENT ATTACK PATTERN " + m_currentAttack);
                    
                    if (m_attackDecider.hasDecidedOnAttack == false)
                    {
                        m_attackDecider.DecideOnAttack();

                    }
                    switch (m_attackDecider.chosenAttack.attack)
                        {
                            case Attack.Phase1Pattern1:
                                StartCoroutine(DualSwordComboAttackPattern1());

                                break;
                            case Attack.Phase1Pattern2:
                                StartCoroutine(ProjectileWaveSlashPhase1Pattern2());
                                break;
                            case Attack.Phase1Pattern3:
                                StartCoroutine(DrillDashPhase1Pattern3());
                                break;
                            case Attack.Phase1Pattern4:
                            
                                if (m_currentSwordState != SwordState.Normal && m_canGeyserBurst)
                                {
                                    StartCoroutine(GeyserBurstPhase1Pattern4());

                                }
                                else
                                {
                                    m_attackDecider.hasDecidedOnAttack = false;
                                    m_stateHandle.ApplyQueuedState();
                                }
                                break;
                            case Attack.Phase1Pattern5:
                            StartCoroutine(FakeBlink1Pattern5Phase1());
                                break;
                            case Attack.Phase2Pattern1:
                                StartCoroutine(DualSwordComboPhase2Pattern1());
                                break;
                            case Attack.Phase2Pattern2:
                                StartCoroutine(ProjectileWaveSlashPhase2Pattern2()); ;
                                break;
                            case Attack.Phase2Pattern3:
                                StartCoroutine(ScytheWavePhase2Pattern3());
                                break;
                            case Attack.Phase2Pattern4:
                                if (m_patternCooldown[3] == m_info.phase2PatternCooldown[3])
                                {
                                    if (m_currentSwordState != SwordState.Normal)
                                    {
                                        StartCoroutine(GeyserBurstPhase1Pattern4());
                                    }
                                    else
                                    {

                                        StartCoroutine(DrillDashComboRoutine());
                                    }
                                }
                                else
                                {
                                    StartCoroutine(DrillDashComboRoutine());
                                }
                                break;
                            case Attack.Phase2Pattern5:
                                StartCoroutine(FakeBlink2Pattern5Phase2());
                                break;
                        }
                    break;
                case State.ReevaluateSituation:
                    if (m_targetInfo.isValid)
                    {
                        if (m_currentSwordState == SwordState.Normal)
                        {
                            m_alterBladeCounterToActivate++;

                            if (m_canUseAlternateSwordState &&
                                m_alterBladeCounterToActivate > m_info.normalBladeCounter)
                            {
                                m_alterBladeCounterToActivate = 0;

                                StartCoroutine(AlterBladeMonitorRoutine());
                            }
                        }
                        else
                        {
                            m_currentAlternateBladeAttackCounter++;

                            if (m_currentAlternateBladeAttackCounter > m_alternateBladeAttackCounter)
                            {
                                m_currentAlternateBladeAttackCounter = 0;

                                StartCoroutine(AlterBladeMonitorRoutine());
                            }
                        }
                        if (geyserBurstCurrentCount >= m_info.geyserBurstCD)
                        {
                            geyserBurstCurrentCount = 0;
                            m_canGeyserBurst = true;
                        }
                        else if (!m_canGeyserBurst)
                        {
                            geyserBurstCurrentCount++;
                        }
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