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

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Boss/KingPus")]
    public class KingPusAI : CombatAIBrain<KingPusAI.Info>
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
            private float m_crawlDuration;
            public float crawlDuration => m_crawlDuration;
            [SerializeField]
            private MovementInfo m_leftMove = new MovementInfo();
            public MovementInfo leftMove => m_leftMove;
            [SerializeField]
            private MovementInfo m_rightMove = new MovementInfo();
            public MovementInfo rightMove => m_rightMove;
            [SerializeField]
            private MovementInfo m_tentaSpearLeftCrawl = new MovementInfo();
            public MovementInfo tentaSpearLeftCrawl => m_tentaSpearLeftCrawl;
            [SerializeField]
            private MovementInfo m_tentaSpearRightCrawl = new MovementInfo();
            public MovementInfo tentaSpearRightCrawl => m_tentaSpearRightCrawl;
            [SerializeField]
            private BasicAnimationInfo m_tentaSpearCrawlLeftAnticipationAnimation;
            public BasicAnimationInfo tentaSpearCrawlLeftAnticipationAnimation => m_tentaSpearCrawlLeftAnticipationAnimation;
            [SerializeField]
            private BasicAnimationInfo m_tentaSpearCrawlRightAnticipationAnimation;
            public BasicAnimationInfo tentaSpearCrawlRightAnticipationAnimation => m_tentaSpearCrawlRightAnticipationAnimation;

            [SerializeField, TitleGroup("Attack Behaviours"), Range(0.1f, 10f)]
            private float m_tentacleSpeed;
            public float tentacleSpeed => m_tentacleSpeed;
            [SerializeField, TitleGroup("Attack Behaviours"), Range(0f, 1000f)]
            private float m_toTargetRetractSpeed;
            public float toTargetRetractSpeed => m_toTargetRetractSpeed;
            [SerializeField, TitleGroup("Attack Behaviours"), Range(0f, 1000f)]
            private float m_wreckingBallSpeed;
            public float wreckingBallSpeed => m_wreckingBallSpeed;
            [SerializeField, TitleGroup("Attack Behaviours"), MinMaxSlider(30, 150)]
            private Vector2 m_grappleWidth = new Vector2(30, 150);
            public Vector2 grappleWidth => m_grappleWidth;
            [SerializeField, TitleGroup("Attack Behaviours"), Range(1, 10)]
            private int m_bodySlamCount;
            public int bodySlamCount => m_bodySlamCount;
            [SerializeField, TitleGroup("Attack Behaviours"), Range(1, 10)]
            private int m_wreckingBallCount;
            public int wreckingBallCount => m_wreckingBallCount;
            [SerializeField, TitleGroup("Attack Behaviours"), Range(1, 10)]
            private int m_groundStabCount;
            public int groundStabCount => m_groundStabCount;
            [SerializeField, TitleGroup("Attack Behaviours"), Range(0.1f, 10f)]
            private float m_groundStabStuckDuration;
            public float groundStabStuckDuration => m_groundStabStuckDuration;
            [SerializeField, TitleGroup("Attack Behaviours"), Range(0f, 1000f)]
            private float m_spikeSpitCount;
            public float spikeSpitCount => m_spikeSpitCount;
            [SerializeField, TitleGroup("Attack Behaviours"), Range(1, 10)]
            private List<int> m_spikeShowerCount;
            public List<int> spikeShowerCount => m_spikeShowerCount;
            [SerializeField, BoxGroup("HeavyGroundStab"), Space]
            private SimpleAttackInfo m_heavyGroundStabLeftAttack = new SimpleAttackInfo();
            public SimpleAttackInfo heavyGroundStabLeftAttack => m_heavyGroundStabLeftAttack;
            [SerializeField, BoxGroup("HeavyGroundStab"), Space]
            private SimpleAttackInfo m_heavyGroundStabRightAttack = new SimpleAttackInfo();
            public SimpleAttackInfo heavyGroundStabRightAttack => m_heavyGroundStabRightAttack;
            [SerializeField, BoxGroup("HeavyGroundStab")]
            private BasicAnimationInfo m_heavyGroundStabLoopLeftAnimation;
            public BasicAnimationInfo heavyGroundStabLoopLeftAnimation => m_heavyGroundStabLoopLeftAnimation;
            [SerializeField, BoxGroup("HeavyGroundStab")]
            private BasicAnimationInfo m_heavyGroundStabLoopRightAnimation;
            public BasicAnimationInfo heavyGroundStabLoopRightAnimation => m_heavyGroundStabLoopRightAnimation;
            [SerializeField, BoxGroup("HeavyGroundStab")]
            private BasicAnimationInfo m_heavyGroundStabAnticipationLeftAnimation;
            public BasicAnimationInfo heavyGroundStabAnticipationLeftAnimation => m_heavyGroundStabAnticipationLeftAnimation;
            [SerializeField, BoxGroup("HeavyGroundStab")]
            private BasicAnimationInfo m_heavyGroundStabAnticipationLoopLeftAnimation;
            public BasicAnimationInfo heavyGroundStabAnticipationLoopLeftAnimation => m_heavyGroundStabAnticipationLoopLeftAnimation;
            [SerializeField, BoxGroup("HeavyGroundStab")]
            private BasicAnimationInfo m_heavyGroundStabAnticipationRightAnimation;
            public BasicAnimationInfo heavyGroundStabAnticipationRightAnimation => m_heavyGroundStabAnticipationRightAnimation;
            [SerializeField, BoxGroup("HeavyGroundStab")]
            private BasicAnimationInfo m_heavyGroundStabAnticipationLoopRightAnimation;
            public BasicAnimationInfo heavyGroundStabAnticipationLoopRightAnimation => m_heavyGroundStabAnticipationLoopRightAnimation;
            [SerializeField, BoxGroup("HeavyGroundStab")]
            private BasicAnimationInfo m_heavyGroundStabReturnLeftAnimation;
            public BasicAnimationInfo heavyGroundStabReturnLeftAnimation => m_heavyGroundStabReturnLeftAnimation;
            [SerializeField, BoxGroup("HeavyGroundStab")]
            private BasicAnimationInfo m_heavyGroundStabReturnRightAnimation;
            public BasicAnimationInfo heavyGroundStabReturnRightAnimation => m_heavyGroundStabReturnRightAnimation;
            [SerializeField, BoxGroup("HeavySpearStab")]
            private SimpleAttackInfo m_heavySpearStabLeftAttack = new SimpleAttackInfo();
            public SimpleAttackInfo heavySpearStabLeftAttack => m_heavySpearStabLeftAttack;
            [SerializeField, BoxGroup("HeavySpearStab")]
            private SimpleAttackInfo m_heavySpearStabRightAttack = new SimpleAttackInfo();
            public SimpleAttackInfo heavySpearStabRightAttack => m_heavySpearStabRightAttack;
            [SerializeField, BoxGroup("KrakenRage")]
            private float m_krakenRageDuration;
            public float krakenRageDuration => m_krakenRageDuration;
            [SerializeField, BoxGroup("KrakenRage")]
            private SimpleAttackInfo m_krakenRageAttack = new SimpleAttackInfo();
            public SimpleAttackInfo krakenRageAttack => m_krakenRageAttack;
            [SerializeField, BoxGroup("KrakenRage")]
            private BasicAnimationInfo m_krakenRageLoopAnimation;
            public BasicAnimationInfo krakenRageLoopAnimation => m_krakenRageLoopAnimation;
            [SerializeField, BoxGroup("KrakenRage")]
            private BasicAnimationInfo m_krakenRageEndAnimation;
            public BasicAnimationInfo krakenRageEndAnimation => m_krakenRageEndAnimation;
            [SerializeField, BoxGroup("SpikeSpitter")]
            private List<SimpleAttackInfo> m_spikeSpitterAttacks = new List<SimpleAttackInfo>();
            public List<SimpleAttackInfo> spikeSpitterAttacks => m_spikeSpitterAttacks;
            [SerializeField, BoxGroup("SpikeSpitter")]
            private List<string> m_spikeSpitterExtendAnimations;
            public List<string> spikeSpitterExtendAnimations => m_spikeSpitterExtendAnimations;
            [SerializeField, BoxGroup("SpikeSpitter")]
            private List<string> m_spikeSpitterRetractAnimations;
            public List<string> spikeSpitterRetractAnimations => m_spikeSpitterRetractAnimations;

            [SerializeField, TitleGroup("Pattern Ranges")]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;
            //[SerializeField, BoxGroup("Phase 1")]
            //private float m_phase1Pattern1Range;
            //public float phase1Pattern1Range => m_phase1Pattern1Range;
            //[SerializeField, BoxGroup("Phase 1")]
            //private float m_phase1Pattern2Range;
            //public float phase1Pattern2Range => m_phase1Pattern2Range;
            //[SerializeField, BoxGroup("Phase 1")]
            //private float m_phase1Pattern3Range;
            //public float phase1Pattern3Range => m_phase1Pattern3Range;
            //[SerializeField, BoxGroup("Phase 1")]
            //private float m_phase1Pattern4Range;
            //public float phase1Pattern4Range => m_phase1Pattern4Range;
            //[SerializeField, BoxGroup("Phase 2")]
            //private float m_phase2Pattern1Range;
            //public float phase2Pattern1Range => m_phase2Pattern1Range;
            //[SerializeField, BoxGroup("Phase 2")]
            //private float m_phase2Pattern2Range;
            //public float phase2Pattern2Range => m_phase2Pattern2Range;
            //[SerializeField, BoxGroup("Phase 2")]
            //private float m_phase2Pattern3Range;
            //public float phase2Pattern3Range => m_phase2Pattern3Range;
            //[SerializeField, BoxGroup("Phase 2")]
            //private float m_phase2Pattern4Range;
            //public float phase2Pattern4Range => m_phase2Pattern4Range;
            //[SerializeField, BoxGroup("Phase 2")]
            //private float m_phase2Pattern5Range;
            //public float phase2Pattern5Range => m_phase2Pattern5Range;
            //[SerializeField, BoxGroup("Phase 2")]
            //private float m_phase2Pattern6Range;
            //public float phase2Pattern6Range => m_phase2Pattern6Range;
            //[SerializeField, BoxGroup("Phase 3")]
            //private float m_phase3Pattern1Range;
            //public float phase3Pattern1Range => m_phase3Pattern1Range;
            //[SerializeField, BoxGroup("Phase 3")]
            //private float m_phase3Pattern2Range;
            //public float phase3Pattern2Range => m_phase3Pattern2Range;
            //[SerializeField, BoxGroup("Phase 3")]
            //private float m_phase3Pattern3Range;
            //public float phase3Pattern3Range => m_phase3Pattern3Range;
            //[SerializeField, BoxGroup("Phase 3")]
            //private float m_phase3Pattern4Range;
            //public float phase3Pattern4Range => m_phase3Pattern4Range;
            //[SerializeField, BoxGroup("Phase 3")]
            //private float m_phase3Pattern5Range;
            //public float phase3Pattern5Range => m_phase3Pattern5Range;
            //[SerializeField, BoxGroup("Phase 3")]
            //private float m_phase3Pattern6Range;
            //public float phase3Pattern6Range => m_phase3Pattern6Range;
            //[SerializeField, BoxGroup("Phase 3")]
            //private float m_phase3Pattern7Range;
            //public float phase3Pattern7Range => m_phase3Pattern7Range;

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

            [TitleGroup("Flinch Behaviours")]
            [SerializeField]
            private BasicAnimationInfo m_flinchLeftAnimation;
            public BasicAnimationInfo flinchLeftAnimation => m_flinchLeftAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchRightAnimation;
            public BasicAnimationInfo flinchRightAnimation => m_flinchRightAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchColorMixAnimation;
            public BasicAnimationInfo flinchColorMixAnimation => m_flinchColorMixAnimation;

            [TitleGroup("Hit Counts")]
            [SerializeField]
            private int m_grappleEvadeHitCount;
            public int grappleEvadeHitCount => m_grappleEvadeHitCount;

            [TitleGroup("Phase Behaviours")]
            [SerializeField]
            private BasicAnimationInfo m_phase1MixAnimation;
            public BasicAnimationInfo phase1MixAnimation => m_phase1MixAnimation;
            [SerializeField]
            private BasicAnimationInfo m_phase2MixAnimation;
            public BasicAnimationInfo phase2MixAnimation => m_phase2MixAnimation;
            [SerializeField]
            private BasicAnimationInfo m_phase3MixAnimation;
            public BasicAnimationInfo phase3MixAnimation => m_phase3MixAnimation;
            [SerializeField]
            private BasicAnimationInfo m_rageQuakePhase1ToPhase2Animation;
            public BasicAnimationInfo rageQuakePhase1ToPhase2Animation => m_rageQuakePhase1ToPhase2Animation;
            [SerializeField]
            private BasicAnimationInfo m_rageQuakePhase2ToPhase3Animation;
            public BasicAnimationInfo rageQuakePhase2ToPhase3Animation => m_rageQuakePhase2ToPhase3Animation;

            [TitleGroup("Wall Behaviours")]
            [SerializeField]
            private List<string> m_wallGrappleAnimations;
            public List<string> wallGrappleAnimations => m_wallGrappleAnimations;
            [SerializeField]
            private List<string> m_wallGrappleExtendAnimations;
            public List<string> wallGrappleExtendAnimations => m_wallGrappleExtendAnimations;
            [SerializeField]
            private List<string> m_wallGrappleRetractAnimations;
            public List<string> wallGrappleRetractAnimations => m_wallGrappleRetractAnimations;
            [SerializeField]
            private BasicAnimationInfo m_wallGrappleAllAnimation;
            public BasicAnimationInfo wallGrappleAllAnimation => m_wallGrappleAllAnimation;
            [SerializeField]
            private BasicAnimationInfo m_wallGrappleAllExtendAnimation;
            public BasicAnimationInfo wallGrappleAllExtendAnimation => m_wallGrappleAllExtendAnimation;
            [SerializeField]
            private BasicAnimationInfo m_wallGrappleAllRetractAnimation;
            public BasicAnimationInfo wallGrappleAllRetractAnimation => m_wallGrappleAllRetractAnimation;

            [TitleGroup("Animations")]
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleMidAirAnimation;
            public BasicAnimationInfo idleMidAirAnimation => m_idleMidAirAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleCielingAnimation;
            public BasicAnimationInfo idleCielingAnimation => m_idleCielingAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleLeftWallAnimation;
            public BasicAnimationInfo idleLeftWallAnimation => m_idleLeftWallAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleRightWallAnimation;
            public BasicAnimationInfo idleRightWallAnimation => m_idleRightWallAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathSelfDestructAnimation;
            public BasicAnimationInfo deathSelfDestructAnimation => m_deathSelfDestructAnimation;
            [SerializeField]
            private BasicAnimationInfo m_fakeDeathAnimation;
            [SerializeField]
            public BasicAnimationInfo fakeDeathAnimation => m_fakeDeathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_bodySlamStart;
            public BasicAnimationInfo bodySlamStart => m_bodySlamStart;
            [SerializeField]
            private BasicAnimationInfo m_bodySlamLoop;
            public BasicAnimationInfo bodySlamLoop => m_bodySlamLoop;
            [SerializeField]
            private BasicAnimationInfo m_bodySlamEnd;
            public BasicAnimationInfo bodySlamEnd => m_bodySlamEnd;
            [SerializeField]
            private BasicAnimationInfo m_cannibalizationAnimation;
            public BasicAnimationInfo cannibalizationAnimation => m_cannibalizationAnimation;

            [Title("Projectiles")]
            [SerializeField, TitleGroup("Grounded")]
            private SimpleProjectileAttackInfo m_ballisticProjectile;
            public SimpleProjectileAttackInfo ballisticProjectile => m_ballisticProjectile;
            [SerializeField, TitleGroup("Grounded")]
            private SimpleProjectileAttackInfo m_ballisticPhase2Projectile;
            public SimpleProjectileAttackInfo ballisticPhase2Projectile => m_ballisticPhase2Projectile;
            [SerializeField, TitleGroup("Grounded")]
            private SimpleProjectileAttackInfo m_ballisticPhase3Projectile;
            public SimpleProjectileAttackInfo ballisticPhase3Projectile => m_ballisticPhase3Projectile;
            [SerializeField, TitleGroup("MidAir")]
            private SimpleProjectileAttackInfo m_airProjectile;
            public SimpleProjectileAttackInfo airProjectile => m_airProjectile;
            [SerializeField, TitleGroup("MidAir")]
            private SimpleProjectileAttackInfo m_airPhase2Projectile;
            public SimpleProjectileAttackInfo airPhase2Projectile => m_airPhase2Projectile;
            [SerializeField, TitleGroup("MidAir")]
            private SimpleProjectileAttackInfo m_airPhase3Projectile;
            public SimpleProjectileAttackInfo airPhase3Projectile => m_airPhase3Projectile;
            [SerializeField]
            private float m_projectileGravityScale;
            public float projectileGravityScale => m_projectileGravityScale;
            [SerializeField]
            private float m_launchDelay;
            public float launchDelay => m_launchDelay;
            [SerializeField, TitleGroup("Spitter")]
            private List<float> m_spittersRotationOffset;
            public List<float> spittersRotationOffset => m_spittersRotationOffset;
            [SerializeField, TitleGroup("Spitter"), MinMaxSlider(30, 150)]
            private Vector2 m_spittersRotationWidth = new Vector2(30, 150);
            public Vector2 spittersRotationWidth => m_spittersRotationWidth;

            [TitleGroup("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_moveEvent;
            public string moveEvent => m_moveEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_stopEvent;
            public string stopEvent => m_stopEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_singleShotEvent;
            public string singleShotEvent => m_singleShotEvent;
            [TitleGroup("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_multiShotEvent;
            public string multiShotEvent => m_multiShotEvent;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_tentaSpearLeftCrawl.SetData(m_skeletonDataAsset);
                m_tentaSpearRightCrawl.SetData(m_skeletonDataAsset);
                m_leftMove.SetData(m_skeletonDataAsset);
                m_rightMove.SetData(m_skeletonDataAsset);
                m_heavyGroundStabRightAttack.SetData(m_skeletonDataAsset);
                m_heavyGroundStabLeftAttack.SetData(m_skeletonDataAsset);
                m_heavySpearStabLeftAttack.SetData(m_skeletonDataAsset);
                m_heavySpearStabRightAttack.SetData(m_skeletonDataAsset);
                m_krakenRageAttack.SetData(m_skeletonDataAsset);
                for (int i = 0; i < m_spikeSpitterAttacks.Count; i++)
                {
                    m_spikeSpitterAttacks[i].SetData(m_skeletonDataAsset);
                }
                //m_bodySlamGroundNearAttack.SetData(m_skeletonDataAsset);
                //m_bodySlamGroundFarAttack.SetData(m_skeletonDataAsset);
                m_ballisticProjectile.SetData(m_skeletonDataAsset);
                m_ballisticPhase2Projectile.SetData(m_skeletonDataAsset);
                m_ballisticPhase3Projectile.SetData(m_skeletonDataAsset);
                m_airProjectile.SetData(m_skeletonDataAsset);
                m_airPhase2Projectile.SetData(m_skeletonDataAsset);
                m_airPhase3Projectile.SetData(m_skeletonDataAsset);

                m_tentaSpearCrawlLeftAnticipationAnimation.SetData(m_skeletonDataAsset);
                m_tentaSpearCrawlRightAnticipationAnimation.SetData(m_skeletonDataAsset);
                m_heavyGroundStabLoopLeftAnimation.SetData(m_skeletonDataAsset);
                m_heavyGroundStabLoopRightAnimation.SetData(m_skeletonDataAsset);
                m_heavyGroundStabAnticipationLeftAnimation.SetData(m_skeletonDataAsset);
                m_heavyGroundStabAnticipationLoopLeftAnimation.SetData(m_skeletonDataAsset);
                m_heavyGroundStabAnticipationRightAnimation.SetData(m_skeletonDataAsset);
                m_heavyGroundStabAnticipationLoopRightAnimation.SetData(m_skeletonDataAsset);
                m_heavyGroundStabReturnLeftAnimation.SetData(m_skeletonDataAsset);
                m_heavyGroundStabReturnRightAnimation.SetData(m_skeletonDataAsset);

                m_krakenRageLoopAnimation.SetData(m_skeletonDataAsset);
                m_krakenRageEndAnimation.SetData(m_skeletonDataAsset);

                m_flinchLeftAnimation.SetData(m_skeletonDataAsset);
                m_flinchRightAnimation.SetData(m_skeletonDataAsset);
                m_flinchColorMixAnimation.SetData(m_skeletonDataAsset);
                m_phase1MixAnimation.SetData(m_skeletonDataAsset);
                m_phase2MixAnimation.SetData(m_skeletonDataAsset);
                m_phase3MixAnimation.SetData(m_skeletonDataAsset);
                m_rageQuakePhase1ToPhase2Animation.SetData(m_skeletonDataAsset);
                m_rageQuakePhase2ToPhase3Animation.SetData(m_skeletonDataAsset);

                m_wallGrappleAllAnimation.SetData(m_skeletonDataAsset);
                m_wallGrappleAllExtendAnimation.SetData(m_skeletonDataAsset);
                m_wallGrappleAllExtendAnimation.SetData(m_skeletonDataAsset);
                m_wallGrappleAllRetractAnimation.SetData(m_skeletonDataAsset);

                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_idleMidAirAnimation.SetData(m_skeletonDataAsset);
                m_idleCielingAnimation.SetData(m_skeletonDataAsset);
                m_idleLeftWallAnimation.SetData(m_skeletonDataAsset);
                m_idleRightWallAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_deathSelfDestructAnimation.SetData(m_skeletonDataAsset);
                m_bodySlamStart.SetData(m_skeletonDataAsset);
                m_bodySlamLoop.SetData(m_skeletonDataAsset);
                m_bodySlamEnd.SetData(m_skeletonDataAsset);

                m_fakeDeathAnimation.SetData(m_skeletonDataAsset);
                m_cannibalizationAnimation.SetData(m_skeletonDataAsset);

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
            private int m_slamCount;
            public int slamCount => m_slamCount;
            [SerializeField]
            private float m_shortRangeAttackDistance;
            public float shortRangeAttackDistance => m_shortRangeAttackDistance;

            [SerializeField, BoxGroup("Attack Range")]
            private float m_tentaspearCrawlRange;
            public float tentaspearCrawlRange => m_tentaspearCrawlRange;
            [SerializeField, BoxGroup("Attack Range")]
            private float m_spikeShower1Range;
            public float spikeShower1Range => m_spikeShower1Range;
            [SerializeField, BoxGroup("Attack Range")]
            private float m_spikeSpit1Range;
            public float spikeSpit1Range => m_spikeSpit1Range;
            [SerializeField, BoxGroup("Attack Range")]
            private float m_spikeSpit2Range;
            public float spikeSpit2Range => m_spikeSpit2Range;

            [SerializeField, BoxGroup("Attack Range")]
            private float m_heavyGroundStabRange;
            public float heavyGroundStabRange => m_heavyGroundStabRange;

            [TabGroup("Not Phase Specific")]
            [SerializeField]
            private float m_heavySpearStabRange;
            public float heavySpearStabRange => m_heavySpearStabRange;

            [SerializeField]
            private ParticleSystem.MinMaxCurve m_crawlFXSize;
            public ParticleSystem.MinMaxCurve crawlFXSize => m_crawlFXSize;
            [SerializeField]
            private ParticleSystem.MinMaxCurve m_tentaSpearCrawlFXSize;
            public ParticleSystem.MinMaxCurve tentaSpearCrawlFXSize => m_tentaSpearCrawlFXSize;
            [SerializeField]
            private ParticleSystem.MinMaxCurve m_heavyGroundStabFXSize;
            public ParticleSystem.MinMaxCurve heavyGroundStabFXSize => m_heavyGroundStabFXSize;
            [SerializeField]
            private ParticleSystem.MinMaxCurve m_stabSlashFXSize;
            public ParticleSystem.MinMaxCurve stabSlashFXSize => m_stabSlashFXSize;
            [SerializeField]
            private ParticleSystem.MinMaxCurve m_krakenFXSize;
            public ParticleSystem.MinMaxCurve krakenFXSize => m_krakenFXSize;
            [SerializeField]
            private ParticleSystem.MinMaxCurve m_bodySlamFXSize;
            public ParticleSystem.MinMaxCurve bodySlamFXSize => m_bodySlamFXSize;
            [SerializeField]
            private ParticleSystem.MinMaxCurve m_healFXSize;
            public ParticleSystem.MinMaxCurve healFXSize => m_healFXSize;
            [SerializeField]
            private ParticleSystem.MinMaxCurve m_deathFXSize;
            public ParticleSystem.MinMaxCurve deathFXSize => m_deathFXSize;
        }

        private enum State
        {
            Phasing,
            Intro,
            Idle,
            Attacking,
            Cooldown,
            Chasing,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Attack
        {
            TentaspearCrawl,
            SpikeShower1,
            SpikeSpit1,
            SpikeSpit2,
            SpikeSpit1ToSpikeSpit2,
            HeavyGroundStab,
            HeavySpearStab,
            SpikeShower1toSpikeShower2,
            KrakenRage,
            BodySlam,
            WreckingBall,
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

        private bool[] m_attackUsed;
        private List<Attack> m_currentAttackCache;
        private List<Attack> m_shortRangedAttackCache;
        private List<Attack> m_longRangedAttackCache;

        private List<float> m_currentAttackRangeCache;
        private List<float> m_shortRangedAttackRangeCache;
        private List<float> m_longRangedAttackRangeCache;

        [SerializeField, TabGroup("Reference")]
        private Boss m_boss;
        [SerializeField, TabGroup("Reference")]
        private Rigidbody2D m_rb2d;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private CapsuleCollider2D m_hitboxCollider;
        [SerializeField, TabGroup("Reference")]
        private CapsuleCollider2D m_bodyCollider;
        private Vector2 m_bodyColliderCacheSize;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_legCollider;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_model;
        [SerializeField, TabGroup("FlinchHandles")]
        private FlinchHandler m_flinchRighthHandle;
        [SerializeField, TabGroup("FlinchHandles")]
        private FlinchHandler m_flinchLeftHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_leftWallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_rightWallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_cielingSensor;
        [SerializeField, TabGroup("Sensors")]
        private Transform m_sensorResizer;
        [SerializeField, TabGroup("Hurtbox")]
        private Collider2D m_krakenRageBB;
        [SerializeField, TabGroup("Hurtbox")]
        private Collider2D m_selfDestructBB;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_crawlFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_tentaSpearCrawlFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_heavyGroundStabFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_stabSlashFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_krakenFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_bodySlamFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_healFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_deathFX;

        private Health m_health;
        [SerializeField]
        private KingPusDamageable m_kingPusDamageable;

        #region TentaclVariables
        [SerializeField, TabGroup("Tentacle Points")]
        private List<SkeletonUtilityBone> m_tentacleOverrideBones;
        [SerializeField, TabGroup("Tentacle Points")]
        private List<Transform> m_tentacleOverridePoints;
        [SerializeField, TabGroup("Tentacle Points")]
        private List<SpringJoint2D> m_chains;
        [SerializeField, TabGroup("Tentacle Points")]
        private Transform m_targetLooker;
        [SerializeField, TabGroup("Tentacle Points")]
        private Transform m_targetPosition;
        [SerializeField, TabGroup("Tentacle Points")]
        private Transform m_mapCenter;
        private List<Vector2> m_defaultTentacleOverridePointPositions;
        [SerializeField, TabGroup("Tentacle Points")]
        private SkeletonUtilityBone m_stabHeadBone;
        private bool m_willGripWall;
        private bool m_willGripTarget;
        private bool m_willStickToWall;
        #endregion
        #region Spitter
        [SerializeField, TabGroup("Spitter")]
        private List<Transform> m_spitterPositions;
        [SerializeField, TabGroup("Spitter")]
        private List<SkeletonUtilityBone> m_spitterBone;
        #endregion

        private BallisticProjectileLauncher m_projectileLauncher;
        private ProjectileInfo m_airProjectileInfo;
        private ProjectileInfo m_ballisticProjectileInfo;

        [SerializeField]
        private SpineEventListener m_spineListener;
        [SerializeField]
        private PhysicsMaterial2D m_physicsMat;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private PhaseHandle<Phase, PhaseInfo> m_phaseHandle;
        private bool m_phase1Done;
        private bool m_phase2Done;
        private bool m_phase3Done;
        private bool m_phase4Done;
        private bool m_canUpdateStats;
        private PhaseInfo m_currentPhase;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_currentAttack;
        private float m_currentAttackRange;
        private int m_maxHitCount;
        private int m_currentHitCount;
        private int m_slamCount;
        private int m_currentSlamCount;
        private float m_shortRangeAttackDistance;

        #region Attack Coroutines
        private Coroutine m_currentAttackCoroutine;
        private Coroutine m_stabCoroutine;
        private Coroutine m_stabIKControlCoroutine;
        private Coroutine m_projectilePositionCheckerCoroutine;
        #endregion

        #region OnDamageTaken Coroutines
        private Coroutine m_changePhaseCoroutine;
        private Coroutine m_wreckingBallCoroutine;
        #endregion

        #region Grapple Coroutines
        private Coroutine m_grappleEvadeCoroutine;
        private Coroutine m_grappleCoroutine;
        private Coroutine m_grappleExtendCoroutine;
        private Coroutine m_grappleRetractCoroutine;
        private Coroutine m_tentacleControlCoroutine;
        private Coroutine m_dynamicIdleCoroutine;
        #endregion

        #region Coroutine Stoppers
        private Coroutine m_attackCoroutineStopper;
        private Coroutine m_allCoroutineStopper;
        #endregion

        private Vector2 m_lastTargetPos;
        private float m_currentCooldown;
        private float m_pickedCooldown;
        private List<float> m_currentFullCooldown;
        private List<float> m_patternCooldown;

        #region Attack Ranges
        private float m_currentGroundStabRange;
        #endregion

        #region Animation
        private string m_idleAnimation;
        private List<string> m_wallGrappleAnimations;
        private List<string> m_wallGrappleExtendAnimations;
        private List<string> m_wallGrappleRetractAnimations;
        #endregion
        private bool m_isDetecting;

        public event EventAction<EventActionArgs> BodySlamDone;
        public event EventAction<EventActionArgs> WreckingBallDone;
        public event EventAction<EventActionArgs> PhaseChangeStart;
        public event EventAction<EventActionArgs> PhaseChangeDone;

        #region Phasing
        private void ApplyPhaseData(PhaseInfo obj)
        {
            m_currentPhase = obj;
            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseOne:
                    if (!m_phase1Done && !m_phase2Done && !m_phase3Done && !m_phase4Done)
                    {
                        ClearAllAttackCaches();
                        ClearAllRangeCaches();

                        if (m_patternCooldown.Count != 0)
                            m_patternCooldown.Clear();

                        m_phase1Done = true;
                        m_canUpdateStats = true;
                        m_bodyCollider.size = new Vector2(40, 15);
                        m_hitbox.transform.localScale = new Vector3(0.75f, 0.75f, 1);
                        m_sensorResizer.localScale = new Vector3(0.75f, 0.75f, 1);
                        m_currentGroundStabRange = m_info.heavyGroundStabRightAttack.range;
                        m_animation.SetAnimation(10, m_info.phase1MixAnimation, false);
                        //AddToAttackCache(Attack.TentaspearCrawl, Attack.SpikeShower1, Attack.SpikeSpit1, Attack.SpikeSpit2);
                        AddToAttackCache(m_shortRangedAttackCache, Attack.HeavySpearStab, Attack.TentaspearCrawl);
                        AddToAttackCache(m_longRangedAttackCache, Attack.SpikeSpit1, Attack.SpikeShower1, Attack.TentaspearCrawl);
                        AddToRangeCache(m_shortRangedAttackRangeCache, obj.heavySpearStabRange, obj.tentaspearCrawlRange);
                        AddToRangeCache(m_longRangedAttackRangeCache, obj.spikeSpit1Range, obj.heavySpearStabRange, obj.tentaspearCrawlRange);
                        for (int i = 0; i < m_info.phase1PatternCooldown.Count; i++)
                            m_patternCooldown.Add(m_info.phase1PatternCooldown[i]);
                        m_airProjectileInfo = m_info.airProjectile.projectileInfo;
                        m_ballisticProjectileInfo = m_info.ballisticProjectile.projectileInfo;
                    }
                    break;
                case Phase.PhaseTwo:
                    if (m_phase1Done && !m_phase2Done && !m_phase3Done && !m_phase4Done)
                    {
                        ClearAllAttackCaches();
                        ClearAllRangeCaches();
                        if (m_patternCooldown.Count != 0)
                            m_patternCooldown.Clear();

                        m_phase2Done = true;
                        m_canUpdateStats = true;
                        m_bodyCollider.size = new Vector2(50, 18);
                        m_hitbox.transform.localScale = new Vector3(1, 1, 1);
                        m_sensorResizer.localScale = new Vector3(1, 1, 1);
                        m_currentGroundStabRange = m_info.heavyGroundStabRightAttack.range + 10;
                        //m_animation.SetAnimation(10, m_info.phase2MixAnimation, false);
                        //AddToAttackCache(Attack.TentaspearCrawl, Attack.SpikeShower1, Attack.SpikeSpit1ToSpikeSpit2, Attack.SpikeSpit2, Attack.HeavyGroundStab, Attack.HeavySpearStab);
                        AddToAttackCache(m_shortRangedAttackCache, Attack.HeavySpearStab, Attack.TentaspearCrawl, Attack.HeavyGroundStab);
                        AddToAttackCache(m_longRangedAttackCache, Attack.SpikeSpit2, Attack.SpikeShower1, Attack.TentaspearCrawl, Attack.BodySlam);
                        //AddToRangeCache(m_info.phase2Pattern1Range, m_info.phase2Pattern2Range, m_info.phase2Pattern3Range, m_info.phase2Pattern4Range, m_info.phase2Pattern5Range, m_info.phase2Pattern6Range);
                        AddToRangeCache(m_shortRangedAttackRangeCache, obj.heavySpearStabRange, obj.tentaspearCrawlRange, obj.heavyGroundStabRange);
                        AddToRangeCache(m_longRangedAttackRangeCache, obj.spikeSpit2Range, obj.spikeShower1Range, obj.tentaspearCrawlRange, obj.shortRangeAttackDistance);
                        for (int i = 0; i < m_info.phase2PatternCooldown.Count; i++)
                            m_patternCooldown.Add(m_info.phase2PatternCooldown[i]);
                        m_airProjectileInfo = m_info.airPhase2Projectile.projectileInfo;
                        m_ballisticProjectileInfo = m_info.ballisticPhase2Projectile.projectileInfo;
                        PhaseChangeDone?.Invoke(this, new EventActionArgs());
                    }
                    break;
                case Phase.PhaseThree:
                    if (m_phase1Done && m_phase2Done && !m_phase3Done && !m_phase4Done)
                    {
                        ClearAllAttackCaches();
                        ClearAllRangeCaches();
                        if (m_patternCooldown.Count != 0)
                            m_patternCooldown.Clear();

                        m_phase3Done = true;
                        m_canUpdateStats = true;
                        m_bodyCollider.size = new Vector2(60, 20);
                        m_hitbox.transform.localScale = new Vector3(1.25f, 1.25f, 1);
                        m_sensorResizer.localScale = new Vector3(1.3f, 1.25f, 1);
                        m_currentGroundStabRange = m_info.heavyGroundStabRightAttack.range + 20;
                        //m_animation.SetAnimation(10, m_info.phase3MixAnimation, false);
                        //AddToAttackCache(Attack.TentaspearCrawl, Attack.SpikeShower1toSpikeShower2, Attack.SpikeSpit1ToSpikeSpit2, Attack.SpikeSpit2, Attack.HeavyGroundStab, Attack.HeavySpearStab, Attack.KrakenRage);
                        AddToAttackCache(m_shortRangedAttackCache, Attack.HeavySpearStab, Attack.TentaspearCrawl);
                        AddToAttackCache(m_longRangedAttackCache, Attack.SpikeSpit1ToSpikeSpit2, Attack.BodySlam, Attack.TentaspearCrawl, Attack.HeavyGroundStab);
                        //AddToRangeCache(m_info.phase3Pattern1Range, m_info.phase3Pattern2Range, m_info.phase3Pattern3Range, m_info.phase3Pattern4Range, m_info.phase3Pattern5Range, m_info.phase3Pattern6Range, m_info.phase3Pattern7Range);
                        AddToRangeCache(m_shortRangedAttackRangeCache, obj.heavySpearStabRange, obj.tentaspearCrawlRange);
                        AddToRangeCache(m_longRangedAttackRangeCache, obj.spikeSpit1Range, obj.shortRangeAttackDistance, obj.tentaspearCrawlRange, obj.heavyGroundStabRange);
                        for (int i = 0; i < m_info.phase3PatternCooldown.Count; i++)
                            m_patternCooldown.Add(m_info.phase3PatternCooldown[i]);
                        m_airProjectileInfo = m_info.airPhase3Projectile.projectileInfo;
                        m_ballisticProjectileInfo = m_info.ballisticPhase3Projectile.projectileInfo;
                        PhaseChangeDone?.Invoke(this, new EventActionArgs());
                    }
                    break;
                case Phase.PhaseFour:
                    if (m_phase1Done && m_phase2Done && m_phase3Done && !m_phase4Done)
                    {
                        Debug.Log("Phase Four Stats Applied");
                        ClearAllAttackCaches();
                        ClearAllRangeCaches();
                        if (m_patternCooldown.Count != 0)
                            m_patternCooldown.Clear();

                        m_phase4Done = true;
                    }
                    break;
            }
            if (m_canUpdateStats)
            {
                m_maxHitCount = obj.hitCount;
                m_slamCount = obj.slamCount;
                m_shortRangeAttackDistance = obj.shortRangeAttackDistance;

                m_bodyColliderCacheSize = m_bodyCollider.size;
                m_attackUsed = new bool[m_currentAttackCache.Count];
                if (m_currentFullCooldown.Count != 0)
                {
                    m_currentFullCooldown.Clear();
                }
                for (int i = 0; i < obj.fullCooldown.Count; i++)
                {
                    m_currentFullCooldown.Add(obj.fullCooldown[i]);
                }
                var crawlFXMain = m_crawlFX.GetComponent<ParticleSystem>().main;
                crawlFXMain.startSize = obj.crawlFXSize;
                //var tentaSpearCrawlFXMain = m_tentaSpearCrawlFX.GetComponent<ParticleSystem>().main;
                //tentaSpearCrawlFXMain.startSize = obj.tentaSpearCrawlFXSize;
                var heavyGroundStabFXMain = m_heavyGroundStabFX.GetComponent<ParticleSystem>().main;
                heavyGroundStabFXMain.startSize = obj.heavyGroundStabFXSize;
                ParticleSystem[] stabSlashFX = m_stabSlashFX.GetComponentsInChildren<ParticleSystem>();
                for (int i = 0; i < stabSlashFX.Length; i++)
                {
                    var stabSlashFXMain = stabSlashFX[i].main;
                    stabSlashFXMain.startSize = stabSlashFXMain.startSize.constant * obj.stabSlashFXSize.constant;
                }
                ParticleSystem[] krakenFX = m_krakenFX.GetComponentsInChildren<ParticleSystem>();
                for (int i = 0; i < krakenFX.Length; i++)
                {
                    var krakenFXMain = krakenFX[i].main;
                    krakenFXMain.startSize = krakenFXMain.startSize.constant * obj.krakenFXSize.constant;
                }
                ParticleSystem[] bodySlamFX = m_bodySlamFX.GetComponentsInChildren<ParticleSystem>();
                for (int i = 0; i < bodySlamFX.Length; i++)
                {
                    var bodySlamFXMain = bodySlamFX[i].main;
                    bodySlamFXMain.startSize = bodySlamFXMain.startSize.constant * obj.bodySlamFXSize.constant;
                }
                ParticleSystem[] healFX = m_healFX.GetComponentsInChildren<ParticleSystem>();
                for (int i = 0; i < healFX.Length; i++)
                {
                    var healFXMain = healFX[i].main;
                    healFXMain.startSize = healFXMain.startSize.constant * obj.healFXSize.constant;
                }
                ParticleSystem[] deathFX = m_deathFX.GetComponentsInChildren<ParticleSystem>();
                for (int i = 0; i < deathFX.Length; i++)
                {
                    var deathFXMain = deathFX[i].main;
                    deathFXMain.startSize = deathFXMain.startSize.constant * obj.deathFXSize.constant;
                }
                m_canUpdateStats = false;
            }
        }

        private void ClearAllAttackCaches()
        {
            m_currentAttackCache.Clear();
            m_longRangedAttackCache.Clear();
            m_shortRangedAttackCache.Clear();
        }

        private void ClearAllRangeCaches()
        {
            m_currentAttackRangeCache.Clear();
            m_longRangedAttackCache.Clear();
            m_shortRangedAttackRangeCache.Clear();
        }

        private void SetCurrentAttackCache(List<Attack> attackCacheToUse)
        {
            m_currentAttackCache = attackCacheToUse;
            m_attackUsed = new bool[m_currentAttackCache.Count];
        }

        private void SetCurrentAttackRangeCache(List<float> attackRangeCacheToUse)
        {
            m_currentAttackRangeCache = attackRangeCacheToUse;
        }

        private void ChangeState()
        {
            Debug.Log("ChangeState for King Pus");

            StartCoroutine(SmartChangePhaseRoutine());
        }


        private void OnChangePhaseTime(object sender, EventActionArgs eventArgs)
        {
            m_hitbox.Disable();
            StartCoroutine(SmartChangePhaseRoutine());
        }

        private IEnumerator SmartChangePhaseRoutine()
        {
            //yield return new WaitWhile(() => !m_phaseHandle.allowPhaseChange);
            Debug.Log("Smart Phase Change for King Pus");

            m_phaseHandle.ApplyChange();
            m_rb2d.isKinematic = false;
            m_rb2d.useFullKinematicContacts = false;
            m_movement.Stop();
            m_rb2d.sharedMaterial = null;
            m_willStickToWall = false;
            m_bodyCollider.size = m_bodyColliderCacheSize;
            m_legCollider.enabled = true;
            m_willGripTarget = false;
            m_willGripWall = false;
            m_hitbox.SetCanBlockDamageState(false);
            m_animation.DisableRootMotion();
            StopCurrentBehaviorRoutine();
            ResetCounterCounts();
            SetAIToPhasing();
            yield return null;
        }

        private IEnumerator ChangePhaseRoutine()
        {
            PhaseChangeStart?.Invoke(this, new EventActionArgs());
            enabled = false;

            m_hitbox.Disable();

            m_stabSlashFX.Stop();
            m_krakenFX.Stop();

            m_animation.DisableRootMotion();
            m_character.physics.simulateGravity = true;
            m_grappleRetractCoroutine = StartCoroutine(GrappleRetractRoutine(m_info.wallGrappleRetractAnimations.Count - 1));
            if (!m_character.physics.inContactWithGround)
            {
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.wallGrappleRetractAnimations[0]);
                m_animation.SetAnimation(0, m_info.bodySlamStart, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bodySlamStart);
                while (!m_groundSensor.isDetecting)
                {
                    m_animation.SetAnimation(0, m_info.bodySlamLoop, true);
                    yield return null;
                }
                m_bodySlamFX.Play();
                m_animation.SetAnimation(0, m_info.bodySlamEnd, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bodySlamEnd);
                enabled = false;
                m_movement.Stop();
                m_animation.SetEmptyAnimation(9, 0);
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
            //m_hitbox.Disable();
            yield return new WaitUntil(() => m_groundSensor.isDetecting);
            var flinchAnimation = m_targetInfo.position.x > transform.position.x ? m_info.flinchLeftAnimation : m_info.flinchRightAnimation;
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, flinchAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, flinchAnimation);
            if (m_phaseHandle.currentPhase != Phase.PhaseThree)
            {
                m_animation.SetAnimation(0, m_info.fakeDeathAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.fakeDeathAnimation);
            }
            var rageAnim = "";
            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseTwo:
                    rageAnim = m_info.rageQuakePhase1ToPhase2Animation.animation;
                    break;
                case Phase.PhaseThree:
                    rageAnim = m_info.rageQuakePhase2ToPhase3Animation.animation;
                    break;
                default:
                    rageAnim = m_info.rageQuakePhase2ToPhase3Animation.animation;
                    break;
            }
            m_animation.EnableRootMotion(true, true);
            m_crawlFX.Play();
            m_animation.SetAnimation(0, rageAnim, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, rageAnim);
            m_animation.DisableRootMotion();
            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseOne:
                    m_animation.SetAnimation(10, m_info.phase2MixAnimation, false);
                    break;
                case Phase.PhaseTwo:
                    m_animation.SetAnimation(10, m_info.phase3MixAnimation, false);
                    break;
                case Phase.PhaseThree:
                    m_animation.SetAnimation(10, m_info.phase3MixAnimation, false);
                    break;
            }

            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseOne:
                    m_phaseHandle.SetPhase(Phase.PhaseTwo);
                    m_hitboxCollider.offset = new Vector2(2.37f, 2.46f);
                    m_hitboxCollider.size = new Vector2(18.09f, 10.83f);
                    break;
                case Phase.PhaseTwo:
                    m_phaseHandle.SetPhase(Phase.PhaseThree);
                    m_hitboxCollider.offset = new Vector2(3.19f, 1.58f);
                    m_hitboxCollider.size = new Vector2(41.23f, 17.41f);
                    break;
                case Phase.PhaseThree:
                    m_phaseHandle.SetPhase(Phase.PhaseFour);
                    //die here
                    break;
            }

            //m_phaseHandle.ApplyChange();
            PhaseChangeDone?.Invoke(this, new EventActionArgs());
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_hitbox.Enable();
            m_hitbox.SetCanBlockDamageState(false);
            m_changePhaseCoroutine = null;
            m_stateHandle.OverrideState(State.Chasing);
            yield return null;

            enabled = true;
        }


        private void SetAIToPhasing()
        {
            StopAnimations();
            m_stateHandle.OverrideState(State.Phasing);
        }

        #endregion

        #region Attacks

        private void LaunchSingleProjectile()
        {
            var projectileInfo = m_willStickToWall ? m_airProjectileInfo : m_ballisticProjectileInfo;

            m_projectileLauncher = new BallisticProjectileLauncher(projectileInfo, m_spitterPositions[0], m_info.projectileGravityScale, projectileInfo.speed);
            m_projectileLauncher.AimAt(m_targetInfo.position);
            switch (m_willStickToWall)
            {
                case true:
                    m_projectileLauncher.LaunchProjectile();
                    break;
                case false:
                    m_projectileLauncher.LaunchBallisticProjectile(m_targetInfo.position);
                    break;
            }
        }

        private IEnumerator ProjectilePositionCheckerRoutine()
        {
            while (true)
            {
                for (int x = 0; x < m_spitterBone.Count; x++)
                {
                    Vector2 spitPos = m_spitterBone[0].transform.position;
                    Vector3 v_diff = (m_targetInfo.position - spitPos);
                    float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
                    m_spitterBone[x].transform.rotation = Quaternion.Euler(0f, 0f, (atan2 * Mathf.Rad2Deg) + m_info.spittersRotationOffset[x]);
                }
                yield return null;
            }
        }

        private void LaunchMultiProjectile()
        {
            var projectileInfo = m_willStickToWall ? m_airProjectileInfo : m_ballisticProjectileInfo;

            for (int i = 0; i < m_spitterPositions.Count; i++)
            {
                m_projectileLauncher = new BallisticProjectileLauncher(projectileInfo, m_spitterPositions[i], m_info.projectileGravityScale, projectileInfo.speed);
                m_projectileLauncher.AimAt(SpitterAimPosition(m_spitterPositions[i]));
                switch (m_willStickToWall)
                {
                    case true:
                        m_projectileLauncher.LaunchProjectile();
                        break;
                    case false:
                        //m_projectileLauncher = new BallisticProjectileLauncher(projectileInfo, m_spitterPositions[i], m_info.projectileGravityScale, projectileInfo.speed);
                        m_projectileLauncher.LaunchBallisticProjectile(SpitterAimPosition(m_spitterPositions[i]));
                        break;
                }
            }
        }

        private Vector2 SpitterAimPosition(Transform spitter)
        {
            int hitCount = 0;
            //RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down,  1000, DChildUtility.GetEnvironmentMask());
            RaycastHit2D[] hit = Cast(spitter.position, spitter.right, 1000, true, out hitCount, true);
            Debug.DrawRay(spitter.position, hit[0].point);
            //var hitPos = (new Vector2(m_projectilePoint.position.x, Vector2.down.y) * hit[0].distance);
            //return hitPos;
            return hit[0].point;
        }

        private IEnumerator WreckingBallRoutine(int slamCount)
        {
            enabled = true;

            StartCoroutine(AttackCoroutineStopper(m_stabCoroutine));
            m_stateHandle.Wait(State.ReevaluateSituation);

            StopAnimations();
            m_rb2d.sharedMaterial = m_physicsMat;
            m_animation.DisableRootMotion();
            m_crawlFX.Stop();
            m_stabSlashFX.Stop();
            m_krakenFX.Stop();
            if (!m_groundSensor.isDetecting)
            {
                m_character.physics.simulateGravity = true;
                m_grappleRetractCoroutine = StartCoroutine(GrappleRetractRoutine(4));
                m_animation.SetAnimation(0, m_info.bodySlamStart, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bodySlamStart);
                while (!m_groundSensor.isDetecting)
                {
                    m_animation.SetAnimation(0, m_info.bodySlamLoop, true);
                    yield return null;
                }
                m_bodySlamFX.Play();
                m_animation.SetAnimation(0, m_info.bodySlamEnd, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bodySlamEnd);
                //yield return new WaitUntil(() => m_groundSensor.isDetecting);
                m_movement.Stop();
                m_animation.SetEmptyAnimation(9, 0);
            }
            m_animation.AddAnimation(0, m_info.idleAnimation, true, 0);
            CalculateWallGrapple(false);
            m_character.physics.simulateGravity = false;
            m_movement.Stop();
            m_bodyCollider.size = new Vector2(m_bodyCollider.size.y, m_bodyCollider.size.y);
            m_grappleExtendCoroutine = StartCoroutine(GrappleExtendRoutine(m_info.wallGrappleExtendAnimations.Count - 1));
            yield return new WaitForSeconds(3f);
            m_rb2d.isKinematic = true;
            m_rb2d.useFullKinematicContacts = true;
            m_legCollider.enabled = false;
            var targetID = UnityEngine.Random.Range(0, m_tentacleOverridePoints.Count - 1);
            var target = m_tentacleOverridePoints[targetID].position;
            AimAt(target);
            var bounceCount = 0;
            while (bounceCount < slamCount)
            {
                m_character.physics.SetVelocity(m_targetLooker.right * m_info.wreckingBallSpeed);
                if (m_groundSensor.isDetecting || m_cielingSensor.isDetecting || m_rightWallSensor.isDetecting || m_leftWallSensor.isDetecting)
                {
                    if (Vector2.Distance(transform.position, target) < 25f)
                    {
                        m_animation.EnableRootMotion(true, true);
                        m_movement.Stop();
                        if (targetID > m_tentacleOverridePoints.Count - 1)
                            targetID = 0;
                        target = m_tentacleOverridePoints[targetID].position;
                        AimAt(target);
                        targetID++;
                        bounceCount++;
                        m_animation.DisableRootMotion();
                    }
                }
                yield return null;
            }
            m_animation.EnableRootMotion(true, true);
            m_movement.Stop();
            m_rb2d.isKinematic = false;
            m_rb2d.useFullKinematicContacts = false;
            m_willGripTarget = false;
            m_targetPosition.position = Vector2.zero;
            m_animation.DisableRootMotion();
            m_legCollider.enabled = true;
            m_character.physics.simulateGravity = true;
            m_grappleRetractCoroutine = StartCoroutine(GrappleRetractRoutine(m_info.wallGrappleRetractAnimations.Count - 1));
            m_movement.Stop();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.wallGrappleRetractAnimations[0]);
            if (!m_groundSensor.isDetecting)
            {
                m_animation.SetAnimation(0, m_info.bodySlamStart, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bodySlamStart);
                while (!m_groundSensor.isDetecting)
                {
                    m_animation.SetAnimation(0, m_info.bodySlamLoop, true);
                    yield return null;
                }
            }
            m_bodyCollider.size = m_bodyColliderCacheSize;
            m_movement.Stop();
            m_bodySlamFX.Play();
            m_animation.SetAnimation(0, m_info.bodySlamEnd, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bodySlamEnd);
            BodySlamDone?.Invoke(this, new EventActionArgs());
            WreckingBallDone?.Invoke(this, new EventActionArgs());
            m_rb2d.sharedMaterial = null;
            m_wreckingBallCoroutine = null;
            m_hitbox.SetCanBlockDamageState(false);
            //TEMP
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            //TEMP
            m_stateHandle.ApplyQueuedState();
            yield return null;
            enabled = true;
        }

        private IEnumerator HeavyGroundStabIKControlRoutine()
        {
            while (true)
            {
                m_stabHeadBone.transform.position = new Vector2(m_lastTargetPos.x, GroundPosition(m_lastTargetPos).y);
                yield return null;
            }
        }

        private IEnumerator HeavySpearStabAttackRoutine()
        {
            //string heavyGroundStabAnticipation = m_targetInfo.position.x > transform.position.x ? m_info.heavySpearStabRightAttack.animation : m_info.heavySpearStabLeftAttack.animation;
            //m_animation.SetAnimation(0, heavyGroundStabAnticipation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, heavyGroundStabAnticipation);
            var heavySpearStabAttackAnimation = m_targetInfo.position.x > transform.position.x ? m_info.heavySpearStabRightAttack.animation : m_info.heavySpearStabLeftAttack.animation;
            m_stabSlashFX.transform.rotation = Quaternion.Euler(0, 0, heavySpearStabAttackAnimation == m_info.heavySpearStabRightAttack.animation ? 0 : 180);
            m_stabSlashFX.Play();
            m_animation.SetAnimation(0, heavySpearStabAttackAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, heavySpearStabAttackAnimation);
            m_animation.DisableRootMotion();
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator HeavyGroundStabAttackRoutine()
        {
            for (int i = 0; i < m_info.groundStabCount; i++)
            {
                var heavyGroundStabAnticipation = m_targetInfo.position.x > transform.position.x ? m_info.heavyGroundStabAnticipationRightAnimation : m_info.heavyGroundStabAnticipationLeftAnimation;
                m_animation.SetAnimation(30, heavyGroundStabAnticipation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, heavyGroundStabAnticipation);
                m_lastTargetPos = m_targetInfo.position;
                var heavyGroundStabLoopAnticipation = "";
                while (!IsTargetInRange(m_currentGroundStabRange))
                {
                    m_lastTargetPos = m_targetInfo.position;
                    heavyGroundStabLoopAnticipation = m_lastTargetPos.x > transform.position.x ? m_info.heavyGroundStabAnticipationLoopRightAnimation.animation : m_info.heavyGroundStabAnticipationLoopLeftAnimation.animation;
                    m_animation.SetAnimation(30, heavyGroundStabLoopAnticipation, true);
                    MoveToTarget(m_currentGroundStabRange, false);
                    yield return null;
                }
                m_movement.Stop();
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
                m_stabHeadBone.mode = SkeletonUtilityBone.Mode.Override;
                m_stabIKControlCoroutine = StartCoroutine(HeavyGroundStabIKControlRoutine());
                //m_stabHeadBone.transform.position = new Vector2(m_lastTargetPos.x, GroundPosition(m_lastTargetPos).y);
                var heavyGroundStabAttackAnimation = m_lastTargetPos.x > transform.position.x ? m_info.heavyGroundStabRightAttack.animation : m_info.heavyGroundStabLeftAttack.animation;
                m_animation.SetAnimation(30, heavyGroundStabAttackAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, heavyGroundStabAttackAnimation);
                m_heavyGroundStabFX.Play();
                //m_stabHeadBone.transform.position = new Vector2(m_lastTargetPos.x, GroundPosition(m_lastTargetPos).y);
                var heavyGroundStabStuckAnimation = m_lastTargetPos.x > transform.position.x ? m_info.heavyGroundStabLoopRightAnimation : m_info.heavyGroundStabLoopLeftAnimation;
                m_animation.SetAnimation(30, heavyGroundStabStuckAnimation, true);
                yield return new WaitForSeconds(m_info.groundStabStuckDuration);
                StopCoroutine(m_stabIKControlCoroutine);
                m_stabIKControlCoroutine = null;
                var heavyGroundStabReturnAnimation = m_lastTargetPos.x > transform.position.x ? m_info.heavyGroundStabReturnRightAnimation : m_info.heavyGroundStabReturnLeftAnimation;
                m_animation.SetAnimation(30, heavyGroundStabReturnAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, heavyGroundStabReturnAnimation);
                m_stabHeadBone.mode = SkeletonUtilityBone.Mode.Follow;
            }
            m_animation.SetEmptyAnimation(30, 0);
            m_animation.DisableRootMotion();
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stabCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        #endregion

        #region Attack Patterns
        private IEnumerator TentaspearCrawlAttackFullRoutine()
        {
            var timer = 0f;
            var tentacipationAnimation = m_targetInfo.position.x > transform.position.x ? m_info.tentaSpearCrawlRightAnticipationAnimation : m_info.tentaSpearCrawlLeftAnticipationAnimation;
            if (!IsTargetInRange(m_info.heavyGroundStabRightAttack.range))
            {
                m_animation.SetAnimation(0, tentacipationAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, tentacipationAnimation);
            }
            while (timer <= m_info.crawlDuration && !IsTargetInRange(m_info.heavyGroundStabRightAttack.range))
            {
                MoveToTarget(m_info.heavyGroundStabRightAttack.range, true);
                timer += Time.deltaTime;
                yield return null;
            }
            m_animation.SetEmptyAnimation(0, 0);

            if (IsTargetInRange(m_currentPhase.heavyGroundStabRange))
            {
                m_currentAttackCoroutine = null;
                m_stabCoroutine = StartCoroutine(HeavyGroundStabAttackRoutine());
            }
            else
            {
                //m_grappleCoroutine = StartCoroutine(GrappleRoutine(false, false, m_info.bodySlamCount/*, true*/));
            }
            yield return null;
        }

        private IEnumerator SpikeShowerOneFullAttackRoutine()
        {
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(30, m_info.idleAnimation.animation, true, 0);
            for (int i = 0; i < m_spitterBone.Count; i++)
            {
                m_spitterBone[i].mode = SkeletonUtilityBone.Mode.Override;
            }
            if (IsTargetInRange(m_info.spikeSpitterAttacks[0].range))
            {
                for (int i = 0; i < m_info.spikeSpitterAttacks.Count; i++)
                {
                    m_projectilePositionCheckerCoroutine = StartCoroutine(ProjectilePositionCheckerRoutine());

                    m_lastTargetPos = m_targetInfo.position;
                    m_animation.SetAnimation(0, m_info.spikeSpitterExtendAnimations[i], false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spikeSpitterExtendAnimations[i]);
                    m_animation.SetAnimation(0, m_info.spikeSpitterAttacks[i].animation, false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spikeSpitterAttacks[i].animation);
                    m_animation.SetAnimation(0, m_info.spikeSpitterRetractAnimations[i], false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spikeSpitterRetractAnimations[i]);
                }
                for (int i = 0; i < m_spitterBone.Count; i++)
                {
                    m_spitterBone[i].mode = SkeletonUtilityBone.Mode.Follow;
                }
                m_animation.SetEmptyAnimation(30, 0);
                m_animation.DisableRootMotion();
                m_attackDecider.hasDecidedOnAttack = false;
                m_currentAttackCoroutine = null;
                m_stateHandle.ApplyQueuedState();
            }
            else
            {
                m_grappleCoroutine = StartCoroutine(GrappleRoutine(false, true, m_info.bodySlamCount/*, true*/));
            }
            yield return null;
        }

        private IEnumerator SpikeSpitAttackFullRoutine(bool spreadShot)
        {
            m_willStickToWall = true;
            m_animation.EnableRootMotion(true, false);
            m_animation.AddAnimation(0, m_info.idleAnimation, true, 0);
            RandomizeTentaclePosition();
            m_grappleCoroutine = StartCoroutine(GrappleRoutine(true, true, 1/*, true*/));
            yield return new WaitUntil(() => m_character.physics.simulateGravity);
            m_hitbox.Enable();
            m_animation.EnableRootMotion(true, true);
            m_rb2d.isKinematic = false;
            m_rb2d.useFullKinematicContacts = false;
            m_movement.Stop();
            //m_animation.SetAnimation(0, DynamicIdleAnimation(), true);
            m_dynamicIdleCoroutine = StartCoroutine(DynamicIdleRoutine());
            m_animation.SetAnimation(30, m_info.idleAnimation.animation, true, 0);
            for (int i = 0; i < m_spitterBone.Count; i++)
            {
                m_spitterBone[i].mode = SkeletonUtilityBone.Mode.Override;
            }
            var id = spreadShot ? 1 : 0;
            m_animation.SetAnimation(15, m_info.spikeSpitterExtendAnimations[id], false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spikeSpitterExtendAnimations[id]);
            for (int i = 0; i < m_info.spikeSpitCount; i++)
            {
                m_projectilePositionCheckerCoroutine = StartCoroutine(ProjectilePositionCheckerRoutine());

                m_lastTargetPos = m_targetInfo.position;
                //LaunchProjectile(spreadShot);
                m_animation.SetAnimation(15, m_info.spikeSpitterAttacks[id].animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spikeSpitterAttacks[id].animation);
                m_animation.SetAnimation(15, m_info.idleAnimation, true);
            }
            m_animation.SetAnimation(15, m_info.spikeSpitterRetractAnimations[id], false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spikeSpitterRetractAnimations[id]);
            if (m_dynamicIdleCoroutine != null)
            {
                StopCoroutine(m_dynamicIdleCoroutine);
                m_dynamicIdleCoroutine = null;
            }
            m_animation.SetEmptyAnimation(3, 0);
            m_animation.SetEmptyAnimation(15, 0);
            m_animation.SetEmptyAnimation(30, 0);
            m_animation.DisableRootMotion();
            m_animation.SetAnimation(0, m_info.bodySlamStart, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bodySlamStart);
            for (int i = 0; i < m_spitterBone.Count; i++)
            {
                m_spitterBone[i].mode = SkeletonUtilityBone.Mode.Follow;
            }
            while (!m_groundSensor.isDetecting)
            {
                m_animation.SetAnimation(0, m_info.bodySlamLoop, true);
                yield return null;
            }
            //if (m_character.facing != HorizontalDirection.Right)
            //    CustomTurn();
            m_willStickToWall = false;
            yield return null;
        }

        //private IEnumerator Phase2Pattern1AttackRoutine()
        //{
        //    if (IsTargetInRange(m_info.phase2Pattern1Range))
        //    {
        //        var timer = 0f;
        //        var tentacipationAnimation = m_targetInfo.position.x > transform.position.x ? m_info.tentaSpearCrawlRightAnticipationAnimation : m_info.tentaSpearCrawlLeftAnticipationAnimation;
        //        if (!IsTargetInRange(m_info.heavyGroundStabRightAttack.range))
        //        {
        //            m_animation.SetAnimation(0, tentacipationAnimation, false);
        //            yield return new WaitForAnimationComplete(m_animation.animationState, tentacipationAnimation);
        //        }
        //        while (timer <= m_info.crawlDuration && !IsTargetInRange(m_info.heavySpearStabRightAttack.range))
        //        {
        //            MoveToTarget(m_info.heavySpearStabRightAttack.range, true);
        //            timer += Time.deltaTime;
        //            yield return null;
        //        }
        //        m_animation.SetEmptyAnimation(0, 0);

        //        if (IsTargetInRange(m_info.heavyGroundStabRightAttack.range))
        //        {
        //            var randomAttack = UnityEngine.Random.Range(0, 2) == 0 ? true : false;
        //            m_stabCoroutine = StartCoroutine(randomAttack ? HeavyGroundStabAttackRoutine() : HeavySpearStabAttackRoutine());
        //        }
        //        else
        //        {
        //            m_grappleCoroutine = StartCoroutine(GrappleRoutine(false, true, 1/*, true*/));
        //        }
        //        m_animation.DisableRootMotion();
        //    }
        //    else
        //    {
        //        m_grappleCoroutine = StartCoroutine(GrappleRoutine(false, false, 1/*, true*/));
        //    }
        //    yield return null;
        //}

        private IEnumerator SpikeShowerOneToSpikeShowerTwoFullAttackRoutine()
        {
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(30, m_info.idleAnimation.animation, true, 0);
            for (int i = 0; i < m_spitterBone.Count; i++)
            {
                m_spitterBone[i].mode = SkeletonUtilityBone.Mode.Override;
            }

            if (IsTargetInRange(m_info.spikeSpitterAttacks[0].range))
            {
                for (int i = 0; i < m_info.spikeSpitterAttacks.Count; i++)
                {
                    m_projectilePositionCheckerCoroutine = StartCoroutine(ProjectilePositionCheckerRoutine());

                    m_animation.SetAnimation(0, m_info.spikeSpitterExtendAnimations[i], false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spikeSpitterExtendAnimations[i]);
                    m_lastTargetPos = m_targetInfo.position;
                    for (int x = 0; x < m_info.spikeShowerCount[i]; x++)
                    {
                        m_animation.SetAnimation(0, m_info.spikeSpitterAttacks[i].animation, false);
                        yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spikeSpitterAttacks[i].animation);
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                        m_lastTargetPos = m_targetInfo.position;
                    }
                    m_animation.SetAnimation(0, m_info.spikeSpitterRetractAnimations[i], false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spikeSpitterRetractAnimations[i]);
                }
                for (int i = 0; i < m_spitterBone.Count; i++)
                {
                    m_spitterBone[i].mode = SkeletonUtilityBone.Mode.Follow;
                }
                m_animation.SetEmptyAnimation(30, 0);
                m_animation.DisableRootMotion();
                m_attackDecider.hasDecidedOnAttack = false;
                m_currentAttackCoroutine = null;
                m_stateHandle.ApplyQueuedState();
            }
            else
            {
                m_grappleCoroutine = StartCoroutine(GrappleRoutine(false, true, m_info.bodySlamCount/*, true*/));
            }
            yield return null;
        }

        private IEnumerator SpikeSpitOneToSpikeSpitTwoFullAttackRoutine()
        {
            m_willStickToWall = true;
            m_animation.EnableRootMotion(true, false);
            m_animation.AddAnimation(0, m_info.idleAnimation, true, 0);
            RandomizeTentaclePosition();
            m_grappleCoroutine = StartCoroutine(GrappleRoutine(true, true, 1));
            yield return new WaitUntil(() => m_character.physics.simulateGravity);
            m_animation.EnableRootMotion(true, true);
            m_hitbox.Enable();
            m_rb2d.isKinematic = false;
            m_rb2d.useFullKinematicContacts = false;
            m_movement.Stop();
            m_dynamicIdleCoroutine = StartCoroutine(DynamicIdleRoutine());
            m_animation.SetAnimation(30, m_info.idleAnimation.animation, true, 0);
            for (int i = 0; i < m_spitterBone.Count; i++)
            {
                m_spitterBone[i].mode = SkeletonUtilityBone.Mode.Override;
            }
            for (int i = 0; i < m_info.spikeSpitterAttacks.Count; i++)
            {
                m_projectilePositionCheckerCoroutine = StartCoroutine(ProjectilePositionCheckerRoutine());

                m_animation.SetAnimation(15, m_info.spikeSpitterExtendAnimations[i], false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spikeSpitterExtendAnimations[i]);
                m_lastTargetPos = m_targetInfo.position;
                for (int x = 0; x < m_info.spikeShowerCount[i]; x++)
                {
                    m_animation.SetAnimation(15, m_info.spikeSpitterAttacks[i].animation, false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spikeSpitterAttacks[i].animation);
                    m_animation.SetAnimation(15, m_info.idleAnimation, true);
                    m_lastTargetPos = m_targetInfo.position;
                }
                m_animation.SetAnimation(15, m_info.spikeSpitterRetractAnimations[i], false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spikeSpitterRetractAnimations[i]);
            }
            if (m_dynamicIdleCoroutine != null)
            {
                StopCoroutine(m_dynamicIdleCoroutine);
                m_dynamicIdleCoroutine = null;
            }
            m_animation.SetEmptyAnimation(3, 0);
            m_animation.SetEmptyAnimation(15, 0);
            m_animation.SetEmptyAnimation(30, 0);
            m_animation.DisableRootMotion();
            m_animation.SetAnimation(0, m_info.bodySlamStart, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bodySlamStart);
            for (int i = 0; i < m_spitterBone.Count; i++)
            {
                m_spitterBone[i].mode = SkeletonUtilityBone.Mode.Follow;
            }
            while (!m_groundSensor.isDetecting)
            {
                m_animation.SetAnimation(0, m_info.bodySlamLoop, true);
                yield return null;
            }
            m_willStickToWall = false;

            yield return null;
       
            //yield return SpikeSpitTwoFullAttackRoutine(true);
        }

        private IEnumerator KrakenRageFullAttackRoutine()
        {
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.krakenRageAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.krakenRageAttack.animation);
            m_krakenFX.Play();
            m_animation.SetAnimation(0, m_info.krakenRageLoopAnimation, true);
            m_krakenRageBB.enabled = true;
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.krakenRageAttack.animation);
            var timer = 0f;
            while (timer < m_info.krakenRageDuration)
            {
                timer += Time.deltaTime;
                yield return null;
            }
            m_krakenRageBB.enabled = false;
            m_krakenFX.Stop();
            m_animation.SetAnimation(0, m_info.krakenRageEndAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.krakenRageEndAnimation);
            m_animation.DisableRootMotion();
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        #endregion

        #region Movement

        private IEnumerator GrappleRoutine(bool willTargetWall, bool willTargetSlam, int slamCount/*, bool randomGrapple*/)
        {
            enabled = true;

            m_attackCoroutineStopper = StartCoroutine(AttackCoroutineStopper(m_stabCoroutine));
            m_stateHandle.Wait(State.ReevaluateSituation);

            StopAnimations();
            m_crawlFX.Stop();
            m_stabSlashFX.Stop();
            m_krakenFX.Stop();
            if (!m_groundSensor.isDetecting)
            {
                m_animation.DisableRootMotion();
                m_character.physics.simulateGravity = true;
                m_grappleRetractCoroutine = StartCoroutine(GrappleRetractRoutine(4));
                m_animation.SetAnimation(0, m_info.bodySlamStart, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bodySlamStart);
                while (!m_groundSensor.isDetecting)
                {
                    m_animation.SetAnimation(0, m_info.bodySlamLoop, true);
                    yield return null;
                }
                m_bodySlamFX.Play();
                m_animation.SetAnimation(0, m_info.bodySlamEnd, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bodySlamEnd);
                if(m_phaseHandle.currentPhase > Phase.PhaseOne)
                    BodySlamDone?.Invoke(this, new EventActionArgs());

                DamageSelfFromBodySlam();
                //yield return new WaitUntil(() => m_groundSensor.isDetecting);
                m_movement.Stop();
                m_animation.SetEmptyAnimation(27, 0);
            }
            m_animation.AddAnimation(0, m_info.idleAnimation, true, 0);
            for (int i = 0; i < slamCount; i++)
            {
                m_animation.DisableRootMotion();
                CalculateWallGrapple(true);
                m_character.physics.simulateGravity = false;
                m_movement.Stop();
                //m_bodyCollider.size = new Vector2(m_bodyCollider.size.y, m_bodyCollider.size.y);
                m_grappleExtendCoroutine = StartCoroutine(GrappleExtendRoutine(4));
                yield return new WaitForSeconds(3f);
                if (willTargetWall)
                {
                    //hitbox.Disable();
                    m_rb2d.isKinematic = true;
                    m_rb2d.useFullKinematicContacts = true;
                }
                m_legCollider.enabled = false;
                if (willTargetSlam)
                {
                    m_willGripTarget = true;
                    //m_tentacleTargetPointIndex = m_wallGrappleDirectionIndex == 0 ? 6 : 0;
                    m_targetPosition.position = willTargetWall ? m_tentacleOverridePoints[UnityEngine.Random.Range(0, m_tentacleOverridePoints.Count)].position : new Vector3(m_targetInfo.position.x, GroundPosition(m_targetInfo.position).y);
                    AimAt(m_targetPosition.position);
                    m_animation.SetAnimation(27, m_info.wallGrappleExtendAnimations[m_info.wallGrappleExtendAnimations.Count - 1], false).TimeScale = m_info.tentacleSpeed;
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.wallGrappleExtendAnimations[m_info.wallGrappleExtendAnimations.Count - 1]);
                }
                m_grappleRetractCoroutine = StartCoroutine(GrappleRetractRoutine(4));
                if (willTargetSlam)
                {
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.wallGrappleRetractAnimations[0]);
                    m_animation.SetAnimation(27, m_info.wallGrappleAnimations[m_info.wallGrappleAnimations.Count - 1], false).MixDuration = 0;
                    while (willTargetWall ? !m_groundSensor.isDetecting && !m_cielingSensor.isDetecting && !m_rightWallSensor.isDetecting && !m_leftWallSensor.isDetecting : !m_groundSensor.isDetecting)
                    {
                        m_character.physics.SetVelocity(m_targetLooker.right * m_info.toTargetRetractSpeed);
                        yield return null;
                    }
                    //yield return new WaitUntil(() => m_groundSensor.isDetecting);
                    m_willGripTarget = false;
                    m_targetPosition.position = Vector2.zero;
                    //m_targetChain.gameObject.SetActive(false);
                    m_animation.SetAnimation(27, m_info.wallGrappleRetractAnimations[m_info.wallGrappleRetractAnimations.Count - 1], false).TimeScale = m_info.tentacleSpeed;
                }
                //m_animation.SetEmptyAnimation(0, 0);
                //m_animation.SetAnimation(0, m_info.idleAnimation, true);
                if (willTargetWall)
                {
                    //m_hitbox.Enable();
                    m_rb2d.isKinematic = false;
                    m_rb2d.useFullKinematicContacts = false;
                }
                m_character.physics.simulateGravity = true;
                yield return new WaitUntil(() => !m_willGripWall);
                switch (willTargetSlam)
                {
                    case true:
                        yield return new WaitUntil(() => !m_willStickToWall);
                        //m_bodyCollider.size = m_bodyColliderCacheSize;
                        m_legCollider.enabled = true;
                        break;
                    case false:
                        //m_bodyCollider.size = m_bodyColliderCacheSize;
                        m_legCollider.enabled = true;
                        m_animation.SetAnimation(0, m_info.bodySlamStart, false);
                        yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bodySlamStart);
                        while (!m_groundSensor.isDetecting)
                        {
                            m_animation.SetAnimation(0, m_info.bodySlamLoop, true);
                            yield return null;
                        }
                        break;
                }

                m_bodySlamFX.Play();
                m_movement.Stop();
                m_animation.SetAnimation(0, m_info.bodySlamEnd, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bodySlamEnd);
                if (m_phaseHandle.currentPhase > Phase.PhaseOne)
                    BodySlamDone?.Invoke(this, new EventActionArgs());

                DamageSelfFromBodySlam();

                m_animation.SetEmptyAnimation(27, 0);
            }
            StopCoroutine(m_attackCoroutineStopper);
            m_attackCoroutineStopper = null;
            m_grappleEvadeCoroutine = null;
            //m_hitbox.SetCanBlockDamageState(false);
            //TEMP
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            //TEMP
            m_stateHandle.ApplyQueuedState();
            ResetCounterCounts();
            yield return null;
            enabled = true;
        }

        private IEnumerator FallFromMidairOrStickingRoutine()
        {
            m_character.physics.simulateGravity = true;
            yield return new WaitUntil(() => !m_willGripWall);

            m_legCollider.enabled = true;
            m_animation.SetAnimation(0, m_info.bodySlamStart, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bodySlamStart);
            while (!m_groundSensor.isDetecting)
            {
                m_animation.SetAnimation(0, m_info.bodySlamLoop, true);
                yield return null;
            }

            m_bodySlamFX.Play();
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.bodySlamEnd, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bodySlamEnd);
        }

        private void DamageSelfFromBodySlam()
        {
            m_damageable.TakeDamage(10, DamageType.True);
        }

        private void MoveToTarget(float targetRange, bool willTentaSpearChase)
        {
            var moveRight = willTentaSpearChase ? m_info.tentaSpearRightCrawl : m_info.rightMove;
            var moveLeft = willTentaSpearChase ? m_info.tentaSpearLeftCrawl : m_info.leftMove;
            if (!IsTargetInRange(targetRange) && m_groundSensor.isDetecting /*&& !m_wallSensor.isDetecting && m_edgeSensor.isDetecting*/)
            {
                m_animation.EnableRootMotion(willTentaSpearChase ? false : true, false);
                m_animation.SetAnimation(0, m_targetInfo.position.x > transform.position.x ? moveRight.animation : moveLeft.animation, true);
                //m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.walk.speed);
            }
            else
            {
                m_movement.Stop();
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
        }

        private void EventMove()
        {
            m_animation.DisableRootMotion();
            //m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_targetInfo.position.x > transform.position.x ? m_info.tentaSpearRightCrawl.speed : -m_info.tentaSpearRightCrawl.speed);
            m_rb2d.AddForce(new Vector2(m_targetInfo.position.x > transform.position.x ? m_info.tentaSpearRightCrawl.speed : -m_info.tentaSpearRightCrawl.speed, m_character.physics.velocity.y), ForceMode2D.Impulse);
        }

        private void EventStop()
        {
            m_animation.EnableRootMotion(true, false);
            m_movement.Stop();
        }

        private string DynamicIdleAnimation()
        {
            if (m_leftWallSensor.isDetecting)
            {
                m_animation.SetAnimation(3, m_info.idleLeftWallAnimation, true);
            }
            else if (m_rightWallSensor.isDetecting)
            {
                m_animation.SetAnimation(3, m_info.idleRightWallAnimation, true);
            }
            else if (m_cielingSensor.isDetecting)
            {
                m_animation.SetAnimation(3, m_info.idleCielingAnimation, true);
            }
            else
            {
                m_animation.SetEmptyAnimation(3, 0);
                return m_groundSensor.isDetecting ? m_info.idleAnimation.animation : m_info.idleMidAirAnimation.animation;
            }
            return m_info.idleAnimation.animation;
            //m_animation.SetEmptyAnimation(3, 0);
            //return m_groundSensor.isDetecting ? m_info.idleAnimation : m_info.idleMidAirAnimation;
        }

        private IEnumerator DynamicIdleRoutine()
        {
            var timer = 1f;
            while (timer > 0f)
            {
                timer -= Time.deltaTime;
                m_animation.SetAnimation(0, DynamicIdleAnimation(), true);
                yield return null;
            }
        }

        private void CalculateWallGrapple(bool randomized)
        {
            m_lastTargetPos = m_targetInfo.position;
            ResetTentaclePosition();
            //RandomizeTentaclePosition();
            switch (randomized)
            {
                case true:
                    RandomizeTentaclePosition();
                    break;
                case false:
                    for (int i = 0; i < m_tentacleOverridePoints.Count; i++)
                    {
                        m_tentacleOverridePoints[i].position = m_defaultTentacleOverridePointPositions[i];
                    }
                    break;
            }
        }

        private IEnumerator TentacleControlRoutine()
        {
            m_willGripWall = true;
            for (int i = 0; i < m_tentacleOverrideBones.Count; i++)
            {
                m_tentacleOverrideBones[i].mode = SkeletonUtilityBone.Mode.Override;
            }
            yield return new WaitUntil(() => !m_willGripWall && !m_willGripTarget);
            for (int i = 0; i < m_tentacleOverrideBones.Count; i++)
            {
                m_tentacleOverrideBones[i].mode = SkeletonUtilityBone.Mode.Follow;
            }
            m_tentacleControlCoroutine = null;
            yield return null;
        }

        private IEnumerator GrappleExtendRoutine(int tentaclesCount)
        {
            m_tentacleControlCoroutine = StartCoroutine(TentacleControlRoutine());
            for (int i = 0; i < tentaclesCount; i++)
            {
                m_animation.SetAnimation(i + 20, m_info.wallGrappleExtendAnimations[i], false).TimeScale = m_info.tentacleSpeed;
            }
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.wallGrappleExtendAnimations[0]);
            for (int i = 0; i < tentaclesCount; i++)
            {
                m_animation.SetAnimation(i + 20, m_info.wallGrappleAnimations[i], true).TimeScale = 5f;
            }
            for (int i = 0; i < tentaclesCount; i++)
            {
                m_chains[i].gameObject.SetActive(true);
            }
            m_grappleExtendCoroutine = null;
            yield return null;
        }

        private IEnumerator GrappleRetractRoutine(int tentaclesCount)
        {
            for (int i = 0; i < tentaclesCount; i++)
            {
                m_chains[i].gameObject.SetActive(false);
                m_chains[i].transform.localPosition = Vector2.zero;
            }
            for (int i = 0; i < tentaclesCount; i++)
            {
                m_animation.SetAnimation(i + 20, m_info.wallGrappleRetractAnimations[i], false).TimeScale = m_info.tentacleSpeed;
            }
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.wallGrappleRetractAnimations[0]);
            for (int i = 0; i < tentaclesCount; i++)
            {
                m_animation.SetEmptyAnimation(i + 20, 0);
            }
            for (int i = 0; i < m_tentacleOverridePoints.Count; i++)
            {
                m_tentacleOverridePoints[i].position = m_defaultTentacleOverridePointPositions[i];
            }
            ResetTentaclePosition();
            m_willGripWall = false;
            m_grappleRetractCoroutine = null;
            yield return null;
        }

        private void RandomizeTentaclePosition()
        {
            for (int i = 0; i < m_tentacleOverridePoints.Count; i++)
            {
                for (int x = 0; x < m_tentacleOverridePoints.Count; x++)
                {
                    m_tentacleOverridePoints[i].position = RandomTentaclePointPosition(/*m_tentacleOverridePoints[i]*/);
                    m_tentacleOverridePoints[i].position = new Vector2(m_tentacleOverridePoints[i].position.x + (UnityEngine.Random.Range(0, 2) == 1 ? 25 : -25), m_tentacleOverridePoints[i].position.y);
                }
            }
        }

        private void ResetTentaclePosition()
        {
            for (int i = 0; i < m_tentacleOverridePoints.Count; i++)
            {
                m_tentacleOverridePoints[i].position = Vector2.zero;
            }
        }

        private Vector2 RandomTentaclePointPosition(/*Transform tentacle*/)
        {
            /*tentacle.rotation*/
            m_mapCenter.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(m_info.grappleWidth.x, m_info.grappleWidth.y));
            int hitCount = 0;
            //RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down,  1000, DChildUtility.GetEnvironmentMask());
            RaycastHit2D[] hit = Cast(/*m_lastTargetPos*/m_mapCenter.position, /*tentacle.right*/m_mapCenter.right, 1000, true, out hitCount, true);
            Debug.DrawRay(/*tentacle.position*/m_mapCenter.position, hit[0].point);
            //var hitPos = (new Vector2(m_projectilePoint.position.x, Vector2.down.y) * hit[0].distance);
            //return hitPos;
            return hit[0].point;
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
        #endregion

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

        private void StopAnimations()
        {
            m_animation.SetEmptyAnimation(0, 0);
            m_animation.SetEmptyAnimation(3, 0);
            m_animation.SetEmptyAnimation(15, 0);
            m_animation.SetEmptyAnimation(27, 0);
            m_animation.SetEmptyAnimation(30, 0);
        }

        private void StopCurrentBehaviorRoutine()
        {
            if (m_currentAttackCoroutine != null)
            {
                StopCoroutine(m_currentAttackCoroutine);
                m_currentAttackCoroutine = null;
                m_attackDecider.hasDecidedOnAttack = false;
            }
            if (m_stabCoroutine != null)
            {
                StopCoroutine(m_stabCoroutine);
                m_stabCoroutine = null;
            }
            if (m_stabIKControlCoroutine != null)
            {
                StopCoroutine(m_stabIKControlCoroutine);
                m_stabIKControlCoroutine = null;
            }
            if (m_projectilePositionCheckerCoroutine != null)
            {
                StopCoroutine(m_projectilePositionCheckerCoroutine);
                m_projectilePositionCheckerCoroutine = null;
            }
            if (m_grappleCoroutine != null)
            {
                StopCoroutine(m_grappleCoroutine);
                m_grappleCoroutine = null;
                //StartCoroutine(GrappleRetractRoutine(m_info.wallGrappleRetractAnimations.Count - 1));
            }
            if (m_grappleExtendCoroutine != null)
            {
                StopCoroutine(m_grappleExtendCoroutine);
                m_grappleExtendCoroutine = null;
                //StartCoroutine(GrappleRetractRoutine(m_info.wallGrappleRetractAnimations.Count - 1));
            }
            if (m_grappleRetractCoroutine != null)
            {
                StopCoroutine(m_grappleRetractCoroutine);
                m_grappleRetractCoroutine = null;
                //StartCoroutine(GrappleRetractRoutine(m_info.wallGrappleRetractAnimations.Count - 1));
            }
            if (m_grappleEvadeCoroutine != null)
            {
                StopCoroutine(m_grappleEvadeCoroutine);
                m_grappleEvadeCoroutine = null;
                //StartCoroutine(GrappleRetractRoutine(m_info.wallGrappleRetractAnimations.Count - 1));
            }
            if (m_tentacleControlCoroutine != null)
            {
                StopCoroutine(m_tentacleControlCoroutine);
                m_tentacleControlCoroutine = null;
                //StartCoroutine(GrappleRetractRoutine(m_info.wallGrappleRetractAnimations.Count - 1));
            }
            if (m_wreckingBallCoroutine != null)
            {
                StopCoroutine(m_wreckingBallCoroutine);
                m_wreckingBallCoroutine = null;
            }
            if (m_attackCoroutineStopper != null)
            {
                StopCoroutine(m_attackCoroutineStopper);
                m_attackCoroutineStopper = null;
            }
            if (m_allCoroutineStopper != null)
            {
                StopCoroutine(m_allCoroutineStopper);
                m_allCoroutineStopper = null;
            }
            if (m_dynamicIdleCoroutine != null)
            {
                StopCoroutine(m_dynamicIdleCoroutine);
                m_dynamicIdleCoroutine = null;
            }
        }

        private void ResetCounterCounts()
        {
            m_currentHitCount = 0;
        }

        private IEnumerator AllCoroutinesStopper()
        {
            while (m_changePhaseCoroutine == null)
            {
                StopCurrentBehaviorRoutine();
                yield return null;
            }
            yield return null;
        }


        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
        }

        private void AllahuAkbar()
        {
            m_rb2d.isKinematic = false;
            m_rb2d.useFullKinematicContacts = false;
            StopAllCoroutines();
            StopCurrentBehaviorRoutine();
            StopAnimations();
            m_hitbox.Disable();
            m_animation.DisableRootMotion();
            m_movement.Stop();
            m_character.physics.simulateGravity = true;
            m_crawlFX.Stop();
            m_krakenFX.Stop();

            Debug.Log("Allahu Akbar!");

            StartCoroutine(DeathRoutine());
        }

        private IEnumerator DeathRoutine()
        {
            PhaseChangeStart?.Invoke(this, new EventActionArgs());
            enabled = false;
            m_grappleRetractCoroutine = StartCoroutine(GrappleRetractRoutine(m_info.wallGrappleRetractAnimations.Count - 1));
            if (!m_character.physics.inContactWithGround)
            {
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.wallGrappleRetractAnimations[0]);
                m_animation.SetAnimation(0, m_info.bodySlamStart, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bodySlamStart);
                while (!m_groundSensor.isDetecting)
                {
                    m_animation.SetAnimation(0, m_info.bodySlamLoop, true);
                    yield return null;
                }
                m_animation.SetAnimation(0, m_info.bodySlamEnd, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bodySlamEnd);
                m_movement.Stop();
                m_animation.SetEmptyAnimation(9, 0);
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
            m_animation.SetEmptyAnimation(0, 0);
            m_animation.SetEmptyAnimation(3, 0);
            m_animation.SetEmptyAnimation(15, 0);
            m_animation.SetEmptyAnimation(27, 0);
            m_krakenRageBB.enabled = false;
            m_deathFX.Play();
            m_animation.SetAnimation(0, m_info.deathSelfDestructAnimation, false);
            yield return new WaitForSeconds(0.75f);
            m_health.SetHealthPercentage(0.01f);
            m_selfDestructBB.enabled = true;
            yield return new WaitForSeconds(0.25f);
            m_selfDestructBB.enabled = false;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathSelfDestructAnimation);
            m_damageable.KillSelf();
            m_animation.SetAnimation(0, m_info.deathAnimation, true);
            m_isDetecting = false;
            //enabled = false;
            yield return null;
        }

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.TentaspearCrawl, m_currentPhase.tentaspearCrawlRange)
                                    , new AttackInfo<Attack>(Attack.SpikeShower1, m_currentPhase.spikeShower1Range)
                                    , new AttackInfo<Attack>(Attack.SpikeSpit1, m_currentPhase.spikeSpit1Range)
                                    , new AttackInfo<Attack>(Attack.SpikeSpit2, m_currentPhase.spikeSpit2Range)
                                    , new AttackInfo<Attack>(Attack.SpikeSpit1ToSpikeSpit2, m_currentPhase.spikeSpit1Range)
                                    , new AttackInfo<Attack>(Attack.HeavyGroundStab, m_currentPhase.heavyGroundStabRange)
                                    , new AttackInfo<Attack>(Attack.HeavySpearStab, m_currentPhase.heavySpearStabRange)
                                    , new AttackInfo<Attack>(Attack.SpikeShower1toSpikeShower2, m_currentPhase.spikeShower1Range)
                                    , new AttackInfo<Attack>(Attack.KrakenRage, m_currentPhase.shortRangeAttackDistance));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private void ChooseAttack()
        {
            if (!m_attackDecider.hasDecidedOnAttack)
            {
                IsAllAttackComplete();
                for (int i = 0; i < m_currentAttackCache.Count; i++)
                {
                    m_attackDecider.DecideOnAttack();
                    if (m_currentAttackCache[i] != m_currentAttack && !m_attackUsed[i])
                    {
                        m_attackUsed[i] = true;
                        m_currentAttack = m_currentAttackCache[i];
                        m_currentAttackRange = m_currentAttackRangeCache[i];
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

        void AddToAttackCache(List<Attack> cache, params Attack[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                cache.Add(list[i]);
            }
        }

        void AddToRangeCache(List<float> cache, params float[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                cache.Add(list[i]);
            }
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if(m_groundSensor.isDetecting)
            {
                if (m_targetInfo.position.x > transform.position.x)
                {
                    m_flinchRighthHandle.gameObject.SetActive(true);
                }
                else
                {
                    m_flinchLeftHandle.gameObject.SetActive(true);
                }
            }
            else
            {
                StartCoroutine(FallFromMidairOrStickingRoutine());
            }
            
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            m_flinchRighthHandle.gameObject.SetActive(false);
            m_flinchLeftHandle.gameObject.SetActive(false);
        }

        private void OnDamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
        {
            if (m_changePhaseCoroutine == null && m_grappleEvadeCoroutine == null && m_wreckingBallCoroutine == null && enabled)
            {
                switch (m_phaseHandle.currentPhase)
                {
                    case Phase.PhaseOne:
                        if (m_currentHitCount < m_maxHitCount)
                            m_currentHitCount++;
                        else
                        {
                            if (m_grappleCoroutine != null)
                            {
                                StopCoroutine(m_grappleCoroutine);
                                m_grappleCoroutine = null;
                            }

                            if (m_currentAttackCoroutine != null)
                            {
                                StopCoroutine(m_currentAttackCoroutine);
                                m_currentAttackCoroutine = null;
                                m_attackDecider.hasDecidedOnAttack = false;
                            }

                            StartCoroutine(GrappleRoutine(false, false, m_info.bodySlamCount));
                        }

                        if (m_hitbox.canBlockDamage)
                        {
                            if (m_grappleCoroutine != null)
                            {
                                StopCoroutine(m_grappleCoroutine);
                                m_grappleCoroutine = null;
                            }

                            if (m_currentAttackCoroutine != null)
                            {
                                StopCoroutine(m_currentAttackCoroutine);
                                m_currentAttackCoroutine = null;
                                m_attackDecider.hasDecidedOnAttack = false;
                            }

                            //m_stateHandle.Wait(State.ReevaluateSituation);

                            m_hitbox.Enable();
                            m_rb2d.isKinematic = false;
                            m_rb2d.useFullKinematicContacts = false;
                            m_willStickToWall = false;
                            m_legCollider.enabled = true;

                            m_grappleEvadeCoroutine = StartCoroutine(GrappleRoutine(false, true, m_info.bodySlamCount/*, true*/));
                            //m_wreckingBallCoroutine = StartCoroutine(WreckingBallRoutine(m_info.wreckingBallCount));
                            m_currentHitCount = 0;

                            //StartCoroutine(AttackCoroutineStopper());

                        }
                        break;
                    default:
                        if (m_currentHitCount < m_maxHitCount)
                            m_currentHitCount++;

                        if (m_hitbox.canBlockDamage)
                        {
                            if (m_grappleCoroutine != null)
                            {
                                StopCoroutine(m_grappleCoroutine);
                                m_grappleCoroutine = null;
                            }

                            if (m_currentAttackCoroutine != null)
                            {
                                StopCoroutine(m_currentAttackCoroutine);
                                m_currentAttackCoroutine = null;
                                m_attackDecider.hasDecidedOnAttack = false;
                            }

                            //m_stateHandle.Wait(State.ReevaluateSituation);

                            m_hitbox.Enable();
                            m_rb2d.isKinematic = false;
                            m_rb2d.useFullKinematicContacts = false;
                            m_willStickToWall = false;
                            m_legCollider.enabled = true;

                            m_wreckingBallCoroutine = StartCoroutine(WreckingBallRoutine(m_info.wreckingBallCount));
                            m_currentHitCount = 0;

                            //StartCoroutine(AttackCoroutineStopper());

                        }
                        break;
                }
            }
        }

        private IEnumerator AttackCoroutineStopper(Coroutine attackCoroutine)
        {
            //Debug.Log("Entered AttackCoroutineStopper");
            while (true /*|| m_wreckingBallCoroutine != null*/)
            {
                //Debug.Log("Checking KingPus Attack Routines");
                if (attackCoroutine != null)
                {
                    //Debug.Log("CURRENT ATACK OF KING PUS NOT NULL");
                    StopCoroutine(attackCoroutine);
                    attackCoroutine = null;
                    if (m_stabIKControlCoroutine != null)
                    {
                        StopCoroutine(m_stabIKControlCoroutine);
                        m_stabIKControlCoroutine = null;
                    }
                    if (m_projectilePositionCheckerCoroutine != null)
                    {
                        StopCoroutine(m_projectilePositionCheckerCoroutine);
                        m_projectilePositionCheckerCoroutine = null;
                    }
                    m_stateHandle.Wait(State.ReevaluateSituation);
                }
                yield return null;
            }
            //yield return null;
        }

        public void AimAt(Vector2 target)
        {
            Vector3 v_diff = (target - (Vector2)m_targetLooker.position);
            float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
            m_targetLooker.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg);
        }

        private void FixedUpdate()
        {
            if (m_willGripWall)
            {
                for (int i = 0; i < m_tentacleOverrideBones.Count; i++)
                {
                    if (m_tentacleOverrideBones[i].transform.position != m_tentacleOverridePoints[i].position)
                    {
                        m_tentacleOverrideBones[i].transform.position = m_tentacleOverridePoints[i].position;
                    }
                }
            }
            if (m_willGripTarget)
            {
                m_tentacleOverrideBones[m_tentacleOverrideBones.Count - 1].transform.position = m_targetPosition.position;
                //m_animation.SetAnimation(0, DynamicIdleAnimation(), true);
            }
            if (m_willGripWall || m_willGripTarget)
            {
                if (!m_groundSensor.isDetecting)
                {
                    m_animation.SetAnimation(0, m_info.idleMidAirAnimation, true);
                }
            }
        }

        private void Update()
        {
            //m_phaseHandle.MonitorPhase();
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    break;
                case State.Intro:
                    StartCoroutine(IntroRoutine());
                    //m_stateHandle.OverrideState(State.Chasing);
                    break;
                case State.Phasing:
                    if (m_changePhaseCoroutine == null)
                    {
                        Debug.Log("Current Phase " + m_phaseHandle.currentPhase);
                        if(m_phaseHandle.currentPhase == Phase.PhaseThree)
                        {
                            m_stateHandle.Wait(State.ReevaluateSituation);
                            AllahuAkbar();
                        }
                        else
                        {
                            m_changePhaseCoroutine = StartCoroutine(ChangePhaseRoutine());
                        }
                    }
                    if (!m_groundSensor.isDetecting)
                    {
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    }
                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    m_lastTargetPos = m_targetInfo.position;

                    switch (m_currentAttack)
                    {
                        case Attack.TentaspearCrawl:
                            m_currentAttackCoroutine = StartCoroutine(TentaspearCrawlAttackFullRoutine());
                            m_pickedCooldown = m_currentFullCooldown[0];
                            break;
                        case Attack.SpikeShower1:
                            m_currentAttackCoroutine = StartCoroutine(SpikeShowerOneFullAttackRoutine());
                            m_pickedCooldown = m_currentFullCooldown[1];
                            break;
                        case Attack.SpikeSpit1:
                            m_currentAttackCoroutine = StartCoroutine(SpikeSpitAttackFullRoutine(false));
                            m_pickedCooldown = m_currentFullCooldown[2];
                            break;
                        case Attack.SpikeSpit2:
                            m_currentAttackCoroutine = StartCoroutine(SpikeSpitAttackFullRoutine(true));
                            m_pickedCooldown = m_currentFullCooldown[3];
                            break;
                        case Attack.SpikeSpit1ToSpikeSpit2:
                            m_currentAttackCoroutine = StartCoroutine(SpikeSpitOneToSpikeSpitTwoFullAttackRoutine());
                            m_pickedCooldown = m_currentFullCooldown[2];
                            break;
                        case Attack.HeavyGroundStab:
                            m_currentAttackCoroutine = StartCoroutine(HeavyGroundStabAttackRoutine());
                            m_pickedCooldown = m_currentFullCooldown[4];
                            break;
                        case Attack.HeavySpearStab:
                            m_currentAttackCoroutine = StartCoroutine(HeavySpearStabAttackRoutine());
                            m_pickedCooldown = m_currentFullCooldown[5];
                            break;
                        case Attack.SpikeShower1toSpikeShower2:
                            m_currentAttackCoroutine = StartCoroutine(SpikeShowerOneToSpikeShowerTwoFullAttackRoutine());
                            m_pickedCooldown = m_currentFullCooldown[1];
                            break;
                        case Attack.KrakenRage:
                            m_currentAttackCoroutine = StartCoroutine(KrakenRageFullAttackRoutine());
                            m_pickedCooldown = m_currentFullCooldown[1];
                            break;
                        case Attack.BodySlam:
                            m_currentAttackCoroutine = StartCoroutine(GrappleRoutine(false, true, m_slamCount));
                            m_pickedCooldown = m_currentFullCooldown[0];
                            break;
                        case Attack.WreckingBall:
                            m_currentAttackCoroutine = StartCoroutine(WreckingBallRoutine(m_slamCount));
                            m_pickedCooldown = m_currentFullCooldown[0];
                            break;
                        case Attack.WaitAttackEnd:
                            break;
                        default: //for testing
                            //m_currentAttackCoroutine = StartCoroutine(KrakenRageFullAttackRoutine());
                            //m_pickedCooldown = m_currentFullCooldown[0];
                            break;
                    }

                    break;

                case State.Cooldown:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);

                    if (m_currentCooldown <= m_pickedCooldown)
                    {
                        m_currentCooldown += Time.deltaTime;
                    }
                    else
                    {
                        m_currentCooldown = 0;
                        m_animation.DisableRootMotion();
                        m_crawlFX.Play();
                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                    }

                    break;

                case State.Chasing:
                    if (!m_hitbox.canBlockDamage)
                    {
                        //ChooseAttack();
                        //if (m_character.facing != HorizontalDirection.Right)
                        //    CustomTurn();
                        //if (IsTargetInRange(m_currentAttackRange) && m_currentAttackCoroutine == null)
                        //{
                        //    m_animation.SetEmptyAnimation(0, 0);
                        //    m_stateHandle.SetState(State.Attacking);
                        //}
                        //else
                        //{
                        //    MoveToTarget(m_currentAttackRange, false);
                        //}
                        if (m_character.facing != HorizontalDirection.Right)
                            CustomTurn();
                        if (IsTargetInRange(m_shortRangeAttackDistance))
                        {
                            SetCurrentAttackCache(m_shortRangedAttackCache);
                            SetCurrentAttackRangeCache(m_shortRangedAttackRangeCache);
                        }
                        else
                        {
                            MoveToTarget(m_shortRangeAttackDistance, false);

                            if (IsTargetInRange(m_shortRangeAttackDistance))
                            {
                                SetCurrentAttackCache(m_shortRangedAttackCache);
                                SetCurrentAttackRangeCache(m_shortRangedAttackRangeCache);
                            }
                            else
                            {
                                SetCurrentAttackCache(m_longRangedAttackCache);
                                SetCurrentAttackRangeCache(m_longRangedAttackRangeCache);
                            }
                        }
                        ChooseAttack();
                        m_animation.SetEmptyAnimation(0, 0);
                        m_stateHandle.SetState(State.Attacking);
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
                    //switch (m_phaseHandle.currentPhase)
                    //{
                    //    case Phase.PhaseOne:
                    //        /*
                    //         * Check if boss is flinched, if on wall or ceiling do spike spit 1
                    //         * else for both conditions do crawl for 4 seconds towards location of player
                    //         * after crawling check distance of player and decide to do short ranged or long ranged attack
                    //         * short: heavy spear stab, tentaspear crawl
                    //         * long: spike spit 1, spike shower 1, tentaspear crawl
                    //         */

                    //        break;
                    //    case Phase.PhaseTwo:
                    //        /*
                    //        * Check if boss is flinched, if on wall or ceiling do spike spit 2
                    //        * else for both conditions check if hp is at 50% and do body slam 
                    //        * if not do crawl for 2 seconds towards location of player
                    //        * after crawling check distance of player and decide to do short ranged or long ranged attack
                    //        * short: heavy ground stab, tentaspear crawl, heavy spear stab
                    //        * long: spike spit 2, spike shower 1, tentaspear crawl, body slam (pus blob drop by extension)
                    //        */

                    //        break;
                    //    case Phase.PhaseThree:
                    //        /*
                    //        * Check if boss is flinched, if on wall or ceiling do spike spit 2
                    //        * else for both conditions check if hp is at 75%, 50%, or 25% and do wrecking ball 
                    //        * if not do crawl for 1 seconds towards location of player
                    //        * after crawling check distance of player and decide to do short ranged or long ranged attack
                    //        * short: tentaspear crawl, heavy spear stab
                    //        * long:  spike shower 1 (followed by spike shower 2), tentaspear crawl, body slam (pus blob drop by extension), heavy ground stab
                    //        */

                    //        break;
                    //    case Phase.PhaseFour:
                    //        break;
                    //    default:
                    //        m_stateHandle.SetState(State.Idle);
                    //        break;
                    //}
                    //break;
                case State.WaitBehaviourEnd:
                    return;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_damageable.DamageTaken += OnDamageTaken;
            m_flinchRighthHandle.FlinchStart += OnFlinchStart;
            m_flinchLeftHandle.FlinchStart += OnFlinchStart;
            m_flinchRighthHandle.FlinchEnd += OnFlinchEnd;
            m_flinchLeftHandle.FlinchEnd += OnFlinchEnd;
            m_health = GetComponentInChildren<Health>();
            m_attackDecider = new RandomAttackDecider<Attack>();
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            //m_currentAttackCache = new List<Attack>();
            //AddToAttackCache(Attack.TentaspearCrawl, Attack.SpikeShower1, Attack.SpikeSpit1, Attack.SpikeSpit2);
            //m_currentAttackRangeCache = new List<float>();
            m_currentFullCooldown = new List<float>();
            m_patternCooldown = new List<float>();
            m_currentAttackCache = new List<Attack>();
            m_shortRangedAttackCache = new List<Attack>();
            m_longRangedAttackCache = new List<Attack> ();
            m_shortRangedAttackRangeCache = new List<float> ();
            m_longRangedAttackRangeCache = new List<float> ();

            AddToAttackCache(m_shortRangedAttackCache, Attack.HeavySpearStab, Attack.TentaspearCrawl);
            AddToAttackCache(m_longRangedAttackCache, Attack.SpikeSpit1, Attack.SpikeShower1, Attack.TentaspearCrawl);
            AddToRangeCache(m_shortRangedAttackRangeCache, 100, 50);
            AddToRangeCache(m_longRangedAttackRangeCache, 100, 100, 100);

            SetCurrentAttackCache(m_shortRangedAttackCache);
            SetCurrentAttackRangeCache(m_shortRangedAttackRangeCache);
            m_attackUsed = new bool[m_currentAttackCache.Count]; 
        }

        protected override void Start()
        {
            base.Start();
            m_animation.DisableRootMotion();
            m_phaseHandle = new PhaseHandle<Phase, PhaseInfo>();
            m_phaseHandle.Initialize(Phase.PhaseOne, m_info.phaseInfo, m_character, ChangeState, ApplyPhaseData);
            m_phaseHandle.ApplyChange();
            UpdateAttackDeciderList();
            m_spineListener.Subscribe(m_info.singleShotEvent, LaunchSingleProjectile);
            m_spineListener.Subscribe(m_info.multiShotEvent, LaunchMultiProjectile);
            m_spineListener.Subscribe(m_info.moveEvent, EventMove);
            m_spineListener.Subscribe(m_info.stopEvent, EventStop);
            m_crawlFX.Play();
            for (int i = 0; i < m_chains.Count; i++)
            {
                m_chains[i].gameObject.SetActive(false);
                m_chains[i].transform.localPosition = Vector2.zero;
            }
            m_defaultTentacleOverridePointPositions = new List<Vector2>();
            for (int i = 0; i < m_tentacleOverridePoints.Count; i++)
            {
                m_defaultTentacleOverridePointPositions.Add(m_tentacleOverridePoints[i].position);
            }

            m_kingPusDamageable.PhaseChangeTime += OnChangePhaseTime;
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