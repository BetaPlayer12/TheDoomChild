﻿using System;
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
            [SerializeField, TitleGroup("Attack Behaviours"), Range(0f, 1000f)]
            private float m_spikeSpitCount;
            public float spikeSpitCount => m_spikeSpitCount;
            [SerializeField, TitleGroup("Attack Behaviours"), Range(1, 10)]
            private List<int> m_spikeShowerCount;
            public List<int> spikeShowerCount => m_spikeShowerCount;
            [SerializeField, BoxGroup("HeavyGroundStab"), Space]
            private List<SimpleAttackInfo> m_heavyGroundStabLeftAttacks = new List<SimpleAttackInfo>();
            public List<SimpleAttackInfo> heavyGroundStabLeftAttacks => m_heavyGroundStabLeftAttacks;
            [SerializeField, BoxGroup("HeavyGroundStab"), Space]
            private List<SimpleAttackInfo> m_heavyGroundStabRightAttacks = new List<SimpleAttackInfo>();
            public List<SimpleAttackInfo> heavyGroundStabRightAttacks => m_heavyGroundStabRightAttacks;
            [SerializeField, BoxGroup("HeavyGroundStab"), ValueDropdown("GetAnimations")]
            private string m_heavyGroundStabAnticipationLeftAnimation;
            public string heavyGroundStabAnticipationLeftAnimation => m_heavyGroundStabAnticipationLeftAnimation;
            [SerializeField, BoxGroup("HeavyGroundStab"), ValueDropdown("GetAnimations")]
            private string m_heavyGroundStabAnticipationRightAnimation;
            public string heavyGroundStabAnticipationRightAnimation => m_heavyGroundStabAnticipationRightAnimation;
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
            [SerializeField, BoxGroup("KrakenRage"), ValueDropdown("GetAnimations")]
            private string m_krakenRageLoopAnimation;
            public string krakenRageLoopAnimation => m_krakenRageLoopAnimation;
            [SerializeField, BoxGroup("KrakenRage"), ValueDropdown("GetAnimations")]
            private string m_krakenRageEndAnimation;
            public string krakenRageEndAnimation => m_krakenRageEndAnimation;
            [SerializeField, BoxGroup("SpikeSpitter")]
            private List<SimpleAttackInfo> m_spikeSpitterAttacks = new List<SimpleAttackInfo>();
            public List<SimpleAttackInfo> spikeSpitterAttacks => m_spikeSpitterAttacks;
            [SerializeField, BoxGroup("SpikeSpitter"), ValueDropdown("GetAnimations")]
            private List<string> m_spikeSpitterExtendAnimations;
            public List<string> spikeSpitterExtendAnimations => m_spikeSpitterExtendAnimations;
            [SerializeField, BoxGroup("SpikeSpitter"), ValueDropdown("GetAnimations")]
            private List<string> m_spikeSpitterRetractAnimations;
            public List<string> spikeSpitterRetractAnimations => m_spikeSpitterRetractAnimations;
            //[SerializeField, BoxGroup("BodySlam")]
            //private SimpleAttackInfo m_bodySlamGroundNearAttack = new SimpleAttackInfo();
            //public SimpleAttackInfo bodySlamGroundNearAttack => m_bodySlamGroundNearAttack;
            //[SerializeField, BoxGroup("BodySlam")]
            //private SimpleAttackInfo m_bodySlamGroundFarAttack = new SimpleAttackInfo();
            //public SimpleAttackInfo bodySlamGroundFarAttack => m_bodySlamGroundFarAttack;

            [SerializeField, TitleGroup("Pattern Ranges")]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;
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
            [SerializeField, BoxGroup("Phase 3")]
            private float m_phase3Pattern1Range;
            public float phase3Pattern1Range => m_phase3Pattern1Range;
            [SerializeField, BoxGroup("Phase 3")]
            private float m_phase3Pattern2Range;
            public float phase3Pattern2Range => m_phase3Pattern2Range;
            [SerializeField, BoxGroup("Phase 3")]
            private float m_phase3Pattern3Range;
            public float phase3Pattern3Range => m_phase3Pattern3Range;
            [SerializeField, BoxGroup("Phase 3")]
            private float m_phase3Pattern4Range;
            public float phase3Pattern4Range => m_phase3Pattern4Range;
            [SerializeField, BoxGroup("Phase 3")]
            private float m_phase3Pattern5Range;
            public float phase3Pattern5Range => m_phase3Pattern5Range;
            [SerializeField, BoxGroup("Phase 3")]
            private float m_phase3Pattern6Range;
            public float phase3Pattern6Range => m_phase3Pattern6Range;
            [SerializeField, BoxGroup("Phase 3")]
            private float m_phase3Pattern7Range;
            public float phase3Pattern7Range => m_phase3Pattern7Range;

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
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchLeftAnimation;
            public string flinchLeftAnimation => m_flinchLeftAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchRightAnimation;
            public string flinchRightAnimation => m_flinchRightAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchColorMixAnimation;
            public string flinchColorMixAnimation => m_flinchColorMixAnimation;

            [TitleGroup("Hit Counts")]
            [SerializeField]
            private int m_grappleEvadeHitCount;
            public int grappleEvadeHitCount => m_grappleEvadeHitCount;

            [TitleGroup("Phase Behaviours")]
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_phase1MixAnimation;
            public string phase1MixAnimation => m_phase1MixAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_phase2MixAnimation;
            public string phase2MixAnimation => m_phase2MixAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_phase3MixAnimation;
            public string phase3MixAnimation => m_phase3MixAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_rageQuakePhase1ToPhase2Animation;
            public string rageQuakePhase1ToPhase2Animation => m_rageQuakePhase1ToPhase2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_rageQuakePhase2ToPhase3Animation;
            public string rageQuakePhase2ToPhase3Animation => m_rageQuakePhase2ToPhase3Animation;

            [TitleGroup("Wall Behaviours")]
            [SerializeField, ValueDropdown("GetAnimations")]
            private List<string> m_wallGrappleAnimations;
            public List<string> wallGrappleAnimations => m_wallGrappleAnimations;
            [SerializeField, ValueDropdown("GetAnimations")]
            private List<string> m_wallGrappleExtendAnimations;
            public List<string> wallGrappleExtendAnimations => m_wallGrappleExtendAnimations;
            [SerializeField, ValueDropdown("GetAnimations")]
            private List<string> m_wallGrappleRetractAnimations;
            public List<string> wallGrappleRetractAnimations => m_wallGrappleRetractAnimations;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_wallGrappleAllAnimation;
            public string wallGrappleAllAnimation => m_wallGrappleAllAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_wallGrappleAllExtendAnimation;
            public string wallGrappleAllExtendAnimation => m_wallGrappleAllExtendAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_wallGrappleAllRetractAnimation;
            public string wallGrappleAllRetractAnimation => m_wallGrappleAllRetractAnimation;

            //[TitleGroup("Misc")]
            //[SerializeField]

            [TitleGroup("Animations")]
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleMidAirAnimation;
            public string idleMidAirAnimation => m_idleMidAirAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleCielingAnimation;
            public string idleCielingAnimation => m_idleCielingAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleLeftWallAnimation;
            public string idleLeftWallAnimation => m_idleLeftWallAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleRightWallAnimation;
            public string idleRightWallAnimation => m_idleRightWallAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathSelfDestructAnimation;
            public string deathSelfDestructAnimation => m_deathSelfDestructAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_bodySlamStart;
            public string bodySlamStart => m_bodySlamStart;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_bodySlamLoop;
            public string bodySlamLoop => m_bodySlamLoop;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_bodySlamEnd;
            public string bodySlamEnd => m_bodySlamEnd;

            [Title("Projectiles")]
            [SerializeField]
            private SimpleProjectileAttackInfo m_ballisticProjectile;
            public SimpleProjectileAttackInfo ballisticProjectile => m_ballisticProjectile;
            [SerializeField]
            private SimpleProjectileAttackInfo m_airProjectile;
            public SimpleProjectileAttackInfo airProjectile => m_airProjectile;
            [SerializeField]
            private float m_projectileGravityScale;
            public float projectileGravityScale => m_projectileGravityScale;
            [SerializeField]
            private float m_launchDelay;
            public float launchDelay => m_launchDelay;

            [TitleGroup("FX")]
            [SerializeField]
            private GameObject m_fx;
            public GameObject fx => m_fx;

            [TitleGroup("Events")]
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
                for (int i = 0; i < m_heavyGroundStabRightAttacks.Count; i++)
                {
                    m_heavyGroundStabRightAttacks[i].SetData(m_skeletonDataAsset);
                }
                for (int i = 0; i < m_heavyGroundStabLeftAttacks.Count; i++)
                {
                    m_heavyGroundStabLeftAttacks[i].SetData(m_skeletonDataAsset);
                }
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
                m_airProjectile.SetData(m_skeletonDataAsset);
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
            //[SerializeField]
            //private List<float> m_patternCooldown;
            //public List<float> patternCooldown => m_patternCooldown;
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
            Phase2Pattern6,
            Phase3Pattern1,
            Phase3Pattern2,
            Phase3Pattern3,
            Phase3Pattern4,
            Phase3Pattern5,
            Phase3Pattern6,
            Phase3Pattern7,
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
        private List<Attack> m_attackCache;
        private List<float> m_attackRangeCache;

        [SerializeField, TabGroup("Reference")]
        private Boss m_boss;
        [SerializeField, TabGroup("Reference")]
        private Rigidbody2D m_rb2d;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
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
        //[SerializeField, TabGroup("Hurtbox")]
        //private Collider2D m_swordSlash1BB;

        //[SerializeField, TabGroup("FX")]
        //private ParticleFX m_earthShakerExplosionFX;

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
        //[SerializeField, TabGroup("Tentacle Points")]
        //private Joint2D m_targetChain;
        //private int m_tentacleTargetPointIndex;
        //private int m_wallGrappleDirectionIndex;
        private bool m_willGripWall;
        private bool m_willGripTarget;
        private bool m_willTentaspearChase;
        private bool m_willStickToWall;
        #endregion
        #region Spitter
        [SerializeField, TabGroup("Spitter")]
        private List<Transform> m_spitterPositions;
        #endregion
        //[SerializeField, TabGroup("Spawn Points")]
        //private Transform m_projectilePoint;
        //[SerializeField, TabGroup("Spawn Points")]
        //private Transform m_scytheWavePoint;
        //[SerializeField, TabGroup("IK Control")]
        //private SkeletonUtilityBone m_targetIK;

        private BallisticProjectileLauncher m_projectileLauncher;
        //private ProjectileLauncher m_scytheWaveLauncher;

        [SerializeField]
        private SpineEventListener m_spineListener;
        [SerializeField]
        private PhysicsMaterial2D m_physicsMat;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
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
        private int m_maxHitCount;
        private int m_currentHitCount;

        private Coroutine m_currentAttackCoroutine;
        private Coroutine m_changePhaseCoroutine;
        private Coroutine m_grappleEvadeCoroutine;
        private Coroutine m_grappleCoroutine;
        private Coroutine m_wreckingBallCoroutine;

        private Vector2 m_lastTargetPos;
        private float m_currentCooldown;
        private float m_pickedCooldown;
        private List<float> m_currentFullCooldown;
        private List<float> m_patternCooldown;

        //#region PatternCounts
        //private int m_phase2pattern1Count;
        //private int m_phase2pattern2Count;
        //private int m_phase2pattern5Count;
        //private int m_fakeBlinkCount;
        //private int m_fakeBlinkChosenDrillDashBehavior;
        //private int m_drillDashComboCount;
        //#endregion

        #region Animation
        private string m_idleAnimation;
        private List<string> m_wallGrappleAnimations;
        private List<string> m_wallGrappleExtendAnimations;
        private List<string> m_wallGrappleRetractAnimations;
        #endregion  

        private void ApplyPhaseData(PhaseInfo obj)
        {
            m_attackCache.Clear();
            m_attackRangeCache.Clear();
            if (m_patternCooldown.Count != 0)
                m_patternCooldown.Clear();
            m_maxHitCount = obj.hitCount;
            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseOne:
                    m_bodyCollider.size = new Vector2(40, 15);
                    m_hitbox.transform.localScale = new Vector3(0.75f, 0.75f, 1);
                    m_sensorResizer.localScale = new Vector3(0.75f, 0.75f, 1);
                    m_animation.SetAnimation(10, m_info.phase1MixAnimation, false);
                    AddToAttackCache(Attack.Phase1Pattern1, Attack.Phase1Pattern2, Attack.Phase1Pattern3, Attack.Phase1Pattern4);
                    AddToRangeCache(m_info.phase1Pattern1Range, m_info.phase1Pattern2Range, m_info.phase1Pattern3Range, m_info.phase1Pattern4Range);
                    for (int i = 0; i < m_info.phase1PatternCooldown.Count; i++)
                        m_patternCooldown.Add(m_info.phase1PatternCooldown[i]);
                    break;
                case Phase.PhaseTwo:
                    m_bodyCollider.size = new Vector2(50, 18);
                    m_hitbox.transform.localScale = new Vector3(1, 1, 1);
                    m_sensorResizer.localScale = new Vector3(1, 1, 1);
                    m_animation.SetAnimation(10, m_info.phase2MixAnimation, false);
                    AddToAttackCache(Attack.Phase2Pattern1, Attack.Phase2Pattern2, Attack.Phase2Pattern3, Attack.Phase2Pattern4, Attack.Phase2Pattern5, Attack.Phase2Pattern6);
                    AddToRangeCache(m_info.phase2Pattern1Range, m_info.phase2Pattern2Range, m_info.phase2Pattern3Range, m_info.phase2Pattern4Range, m_info.phase2Pattern5Range, m_info.phase2Pattern6Range);
                    for (int i = 0; i < m_info.phase2PatternCooldown.Count; i++)
                        m_patternCooldown.Add(m_info.phase2PatternCooldown[i]);
                    break;
                case Phase.PhaseThree:
                    m_bodyCollider.size = new Vector2(60, 20);
                    m_hitbox.transform.localScale = new Vector3(1.25f, 1.25f, 1);
                    m_sensorResizer.localScale = new Vector3(1.3f, 1.25f, 1);
                    m_animation.SetAnimation(10, m_info.phase3MixAnimation, false);
                    AddToAttackCache(Attack.Phase3Pattern1, Attack.Phase3Pattern2, Attack.Phase3Pattern3, Attack.Phase3Pattern4, Attack.Phase3Pattern5, Attack.Phase3Pattern6, Attack.Phase3Pattern7);
                    AddToRangeCache(m_info.phase3Pattern1Range, m_info.phase3Pattern2Range, m_info.phase3Pattern3Range, m_info.phase3Pattern4Range, m_info.phase3Pattern5Range, m_info.phase3Pattern6Range, m_info.phase3Pattern7Range);
                    for (int i = 0; i < m_info.phase3PatternCooldown.Count; i++)
                        m_patternCooldown.Add(m_info.phase3PatternCooldown[i]);
                    break;
            }
            m_bodyColliderCacheSize = m_bodyCollider.size;
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

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                m_stateHandle.OverrideState(State.Intro);
                GameEventMessage.SendEvent("Boss Encounter");
            }
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
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

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            m_flinchRighthHandle.gameObject.SetActive(false);
            m_flinchLeftHandle.gameObject.SetActive(false);
        }

        private void OnDamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
        {
            if (m_grappleEvadeCoroutine == null && m_wreckingBallCoroutine == null)
            {
                switch (m_phaseHandle.currentPhase)
                {
                    case Phase.PhaseOne:
                        if (m_currentHitCount < m_maxHitCount)
                            m_currentHitCount++;
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
                            if (m_grappleCoroutine != null)
                            {
                                StopCoroutine(m_grappleCoroutine);
                                m_grappleCoroutine = null;
                            }

                            m_stateHandle.Wait(State.ReevaluateSituation);

                            m_willStickToWall = false;
                            m_legCollider.enabled = true;
                            m_grappleEvadeCoroutine = StartCoroutine(GrappleRoutine(false, true, m_info.bodySlamCount/*, true*/));
                            m_currentHitCount = 0;

                        }
                        break;
                    default:
                        if (m_currentHitCount < m_maxHitCount)
                            m_currentHitCount++;
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
                            if (m_grappleCoroutine != null)
                            {
                                StopCoroutine(m_grappleCoroutine);
                                m_grappleCoroutine = null;
                            }

                            m_stateHandle.Wait(State.ReevaluateSituation);

                            m_willStickToWall = false;
                            m_legCollider.enabled = true;
                            m_wreckingBallCoroutine = StartCoroutine(WreckingBallRoutine(m_info.wreckingBallCount));
                            m_currentHitCount = 0;

                        }
                        break;
                }
            }
        }

        private IEnumerator GrappleRoutine(bool willTargetWall, bool willTargetSlam, int slamCount/*, bool randomGrapple*/)
        {
            enabled = true;
            m_animation.SetEmptyAnimation(3, 0);
            m_animation.SetEmptyAnimation(15, 0);
            m_animation.SetEmptyAnimation(27, 0);
            if (!m_groundSensor.isDetecting)
            {
                m_animation.DisableRootMotion();
                m_character.physics.simulateGravity = true;
                StartCoroutine(GrappleRetractRoutine(4));
                m_animation.SetAnimation(0, m_info.bodySlamStart, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bodySlamStart);
                while (!m_groundSensor.isDetecting)
                {
                    m_animation.SetAnimation(0, m_info.bodySlamLoop, true);
                    yield return null;
                }
                m_animation.SetAnimation(0, m_info.bodySlamEnd, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bodySlamEnd);
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
                m_bodyCollider.size = new Vector2(m_bodyCollider.size.y, m_bodyCollider.size.y);
                StartCoroutine(GrappleExtendRoutine(4));
                yield return new WaitForSeconds(3f);
                m_legCollider.enabled = willTargetWall ? false : true;
                if (willTargetSlam)
                {
                    m_willGripTarget = true;
                    //m_tentacleTargetPointIndex = m_wallGrappleDirectionIndex == 0 ? 6 : 0;
                    m_targetPosition.position = willTargetWall ? m_tentacleOverridePoints[UnityEngine.Random.Range(0, m_tentacleOverridePoints.Count)].position : new Vector3(m_targetInfo.position.x, GroundPosition(m_targetInfo.position).y);
                    AimAt(m_targetPosition.position);
                    m_animation.SetAnimation(27, m_info.wallGrappleExtendAnimations[m_info.wallGrappleExtendAnimations.Count - 1], false).TimeScale = m_info.tentacleSpeed;
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.wallGrappleExtendAnimations[m_info.wallGrappleExtendAnimations.Count - 1]);
                }
                StartCoroutine(GrappleRetractRoutine(4));
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
                m_character.physics.simulateGravity = true;
                yield return new WaitUntil(() => !m_willGripWall);
                switch (willTargetSlam)
                {
                    case true:
                        yield return new WaitUntil(() => !m_willStickToWall);
                        m_bodyCollider.size = m_bodyColliderCacheSize;
                        m_legCollider.enabled = true;
                        break;
                    case false:
                        m_bodyCollider.size = m_bodyColliderCacheSize;
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
                m_movement.Stop();
                m_animation.SetAnimation(0, m_info.bodySlamEnd, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bodySlamEnd);
                m_animation.SetEmptyAnimation(27, 0);
            }
            //m_hitbox.Enable();
            m_grappleEvadeCoroutine = null;
            m_hitbox.SetCanBlockDamageState(false);
            //TEMP
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            //TEMP
            m_stateHandle.ApplyQueuedState();
            yield return null;
            enabled = true;
        }

        private IEnumerator WreckingBallRoutine(int slamCount)
        {
            enabled = true;
            m_rb2d.sharedMaterial = m_physicsMat;
            m_animation.SetEmptyAnimation(3, 0);
            m_animation.SetEmptyAnimation(15, 0);
            m_animation.SetEmptyAnimation(27, 0);
            m_animation.DisableRootMotion();
            if (!m_groundSensor.isDetecting)
            {
                m_character.physics.simulateGravity = true;
                StartCoroutine(GrappleRetractRoutine(4));
                m_animation.SetAnimation(0, m_info.bodySlamStart, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bodySlamStart);
                while (!m_groundSensor.isDetecting)
                {
                    m_animation.SetAnimation(0, m_info.bodySlamLoop, true);
                    yield return null;
                }
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
            StartCoroutine(GrappleExtendRoutine(m_info.wallGrappleExtendAnimations.Count - 1));
            yield return new WaitForSeconds(3f);
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
                        m_movement.Stop();
                        if (targetID > m_tentacleOverridePoints.Count - 1)
                            targetID = 0;
                        target = m_tentacleOverridePoints[targetID].position;
                        AimAt(target);
                        targetID++;
                        bounceCount++;
                    }
                }
                yield return null;
            }
            m_willGripTarget = false;
            m_targetPosition.position = Vector2.zero;
            m_animation.DisableRootMotion();
            m_bodyCollider.size = m_bodyColliderCacheSize;
            m_legCollider.enabled = true;
            m_character.physics.simulateGravity = true;
            StartCoroutine(GrappleRetractRoutine(m_info.wallGrappleRetractAnimations.Count - 1));
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
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.bodySlamEnd, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bodySlamEnd);
            m_rb2d.sharedMaterial = null;
            //m_hitbox.Enable();
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

        private void SetAIToPhasing()
        {
            m_animation.SetEmptyAnimation(0, 0);
            m_animation.SetEmptyAnimation(3, 0);
            m_animation.SetEmptyAnimation(15, 0);
            m_animation.SetEmptyAnimation(27, 0);
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
            if (m_grappleCoroutine != null)
            {
                StopCoroutine(m_grappleCoroutine);
                m_grappleCoroutine = null;
            }
            if (m_grappleEvadeCoroutine != null)
            {
                StopCoroutine(m_grappleEvadeCoroutine);
                m_grappleEvadeCoroutine = null;
            }
            if (m_wreckingBallCoroutine != null)
            {
                StopCoroutine(m_wreckingBallCoroutine);
                m_wreckingBallCoroutine = null;
            }
        }

        private void ResetCounterCounts()
        {
            m_currentHitCount = 0;
        }

        private IEnumerator ChangePhaseRoutine()
        {
            enabled = false;
            m_animation.DisableRootMotion();
            m_character.physics.simulateGravity = true;
            StartCoroutine(GrappleRetractRoutine(m_info.wallGrappleRetractAnimations.Count - 1));
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
                enabled = false;
                m_movement.Stop();
                m_animation.SetEmptyAnimation(9, 0);
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
            m_hitbox.Disable();
            yield return new WaitUntil(() => m_groundSensor.isDetecting);
            var flinchAnimation = m_targetInfo.position.x > transform.position.x ? m_info.flinchLeftAnimation : m_info.flinchRightAnimation;
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, flinchAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, flinchAnimation);
            var rageAnim = "";
            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseTwo:
                    rageAnim = m_info.rageQuakePhase1ToPhase2Animation;
                    break;
                case Phase.PhaseThree:
                    rageAnim = m_info.rageQuakePhase2ToPhase3Animation;
                    break;
            }
            m_animation.SetAnimation(0, rageAnim, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, rageAnim);
            m_phaseHandle.ApplyChange();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_hitbox.Enable();
            m_hitbox.SetCanBlockDamageState(false);
            m_changePhaseCoroutine = null;
            m_stateHandle.OverrideState(State.Chasing);
            yield return null;
            enabled = true;
        }
        #region Attacks

        private void LaunchSingleProjectile()
        {
            var projectileInfo = m_willStickToWall ? m_info.airProjectile.projectileInfo : m_info.ballisticProjectile.projectileInfo;
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

            //StartCoroutine(DelayedProjectileLauncher(m_spitterPositions.Count, m_info.launchDelay));
        }

        private void LaunchMultiProjectile()
        {
            var projectileInfo = m_willStickToWall ? m_info.airProjectile.projectileInfo : m_info.ballisticProjectile.projectileInfo;
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

            //StartCoroutine(DelayedProjectileLauncher(m_spitterPositions.Count, m_info.launchDelay));
        }

        //private IEnumerator DelayedProjectileLauncher(int launchCounts, float launchDelay)
        //{
        //    yield return new WaitForSeconds(launchDelay);
        //    for (int i = 0; i < launchCounts; i++)
        //    {
        //        m_projectileLauncher = new BallisticProjectileLauncher(m_info.ballisticProjectile.projectileInfo, m_spitterPositions[i], m_info.projectileGravityScale, m_info.ballisticProjectile.projectileInfo.speed);
        //        m_projectileLauncher.LaunchBallisticProjectile(SpitterAimPosition(m_spitterPositions[i]));
        //    }
        //    yield return null;
        //}

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

        private IEnumerator Phase1Pattern1AttackRoutine()
        {
            m_animation.EnableRootMotion(true, false);
            var timer = 0f;
            while (timer <= m_info.crawlDuration /*&& !IsTargetInRange(m_info.heavyGroundStabRightAttacks[0].range - 5f)*/)
            {
                var tentaSpearCrawlAnimation = m_targetInfo.position.x > transform.position.x ? m_info.tentaSpearRightCrawl.animation : m_info.tentaSpearLeftCrawl.animation;
                m_animation.SetAnimation(0, tentaSpearCrawlAnimation, true);
                timer += Time.deltaTime;
                yield return null;
            }
            m_animation.SetEmptyAnimation(0, 0);

            if (IsTargetInRange(m_info.heavyGroundStabRightAttacks[0].range))
            {
                for (int i = 0; i < m_info.heavyGroundStabRightAttacks.Count; i++)
                {
                    m_lastTargetPos = m_targetInfo.position;
                    string heavyGroundStabAnticipation = m_lastTargetPos.x > transform.position.x ? m_info.heavyGroundStabAnticipationRightAnimation : m_info.heavyGroundStabAnticipationLeftAnimation;
                    m_animation.SetAnimation(0, heavyGroundStabAnticipation, false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, heavyGroundStabAnticipation);
                    m_stabHeadBone.mode = SkeletonUtilityBone.Mode.Override;
                    m_stabHeadBone.transform.position = new Vector2(m_lastTargetPos.x, GroundPosition(m_lastTargetPos).y);
                    var heavyGroundStabAttackAnimation = heavyGroundStabAnticipation == m_info.heavyGroundStabAnticipationRightAnimation ? m_info.heavyGroundStabRightAttacks[i].animation : m_info.heavyGroundStabLeftAttacks[i].animation;
                    m_animation.SetAnimation(0, heavyGroundStabAttackAnimation, false);
                    //yield return new WaitForSeconds(0.25f);
                    yield return new WaitForAnimationComplete(m_animation.animationState, heavyGroundStabAttackAnimation);
                    m_stabHeadBone.mode = SkeletonUtilityBone.Mode.Follow;
                }
                m_animation.DisableRootMotion();
                m_attackDecider.hasDecidedOnAttack = false;
                m_currentAttackCoroutine = null;
                m_stateHandle.ApplyQueuedState();
            }
            else
            {
                m_grappleCoroutine = StartCoroutine(GrappleRoutine(false, false, m_info.bodySlamCount/*, true*/));
            }
            yield return null;
        }

        private IEnumerator Phase1Pattern2AttackRoutine()
        {
            m_animation.EnableRootMotion(true, false);
            m_animation.AddAnimation(0, m_info.idleAnimation, true, 0);
            if (IsTargetInRange(m_info.spikeSpitterAttacks[0].range))
            {
                for (int i = 0; i < m_info.spikeSpitterAttacks.Count; i++)
                {
                    //if (!IsFacingTarget() && i == 0)
                    //    CustomTurn();
                    m_lastTargetPos = m_targetInfo.position;
                    m_animation.SetAnimation(0, m_info.spikeSpitterExtendAnimations[i], false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spikeSpitterExtendAnimations[i]);
                    //LaunchProjectile(i == 0 ? false : true);
                    m_animation.SetAnimation(0, m_info.spikeSpitterAttacks[i].animation, false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spikeSpitterAttacks[i].animation);
                    m_animation.SetAnimation(0, m_info.spikeSpitterRetractAnimations[i], false);
                    yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spikeSpitterRetractAnimations[i]);
                    //if (m_character.facing != HorizontalDirection.Right && i == 0)
                    //    CustomTurn();
                }
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

        private IEnumerator Phase1Pattern3And4AttackRoutine(bool spreadShot)
        {
            m_willStickToWall = true;
            m_animation.EnableRootMotion(true, false);
            m_animation.AddAnimation(0, m_info.idleAnimation, true, 0);
            RandomizeTentaclePosition();
            m_grappleCoroutine = StartCoroutine(GrappleRoutine(true, true, 1/*, true*/));
            yield return new WaitUntil(() => m_character.physics.simulateGravity);
            enabled = false;
            m_animation.EnableRootMotion(true, true);
            m_animation.SetAnimation(0, DynamicIdleAnimation(), true);
            var id = spreadShot ? 1 : 0;
            for (int i = 0; i < m_info.spikeSpitCount; i++)
            {
                //if (!IsFacingTarget())
                //    CustomTurn();
                m_lastTargetPos = m_targetInfo.position;
                m_animation.SetAnimation(15, m_info.spikeSpitterExtendAnimations[id], false);
                Debug.Log("Pattern 1 Phase 3 Shoot ");
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spikeSpitterExtendAnimations[id]);
                //LaunchProjectile(spreadShot);
                m_animation.SetAnimation(15, m_info.spikeSpitterAttacks[id].animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spikeSpitterAttacks[id].animation);
                m_animation.SetAnimation(15, m_info.spikeSpitterRetractAnimations[id], false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spikeSpitterRetractAnimations[id]);
            }
            m_animation.SetEmptyAnimation(3, 0);
            m_animation.SetEmptyAnimation(15, 0);
            m_animation.DisableRootMotion();
            m_animation.SetAnimation(0, m_info.bodySlamStart, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bodySlamStart);
            while (!m_groundSensor.isDetecting)
            {
                m_animation.SetAnimation(0, m_info.bodySlamLoop, true);
                yield return null;
            }
            //if (m_character.facing != HorizontalDirection.Right)
            //    CustomTurn();
            m_willStickToWall = false;
            //m_animation.DisableRootMotion();
            //m_attackDecider.hasDecidedOnAttack = false;
            //m_currentAttackCoroutine = null;
            //m_stateHandle.ApplyQueuedState();
            yield return null;
            enabled = true;
        }

        private IEnumerator Phase2Pattern1AttackRoutine()
        {
            if (IsTargetInRange(m_info.phase2Pattern1Range))
            {
                m_animation.EnableRootMotion(true, false);
                var timer = 0f;
                while (timer <= m_info.crawlDuration /*&& !IsTargetInRange(m_info.heavyGroundStabRightAttacks[0].range - 5f)*/)
                {
                    var tentaSpearCrawlAnimation = m_targetInfo.position.x > transform.position.x ? m_info.tentaSpearRightCrawl.animation : m_info.tentaSpearLeftCrawl.animation;
                    m_animation.SetAnimation(0, tentaSpearCrawlAnimation, true);
                    timer += Time.deltaTime;
                    yield return null;
                }
                m_animation.SetEmptyAnimation(0, 0);
                if (IsTargetInRange(m_info.heavyGroundStabLeftAttacks[0].range))
                {
                    var randomAttack = UnityEngine.Random.Range(0, 2) == 0 ? true : false;
                    m_currentAttackCoroutine = StartCoroutine(randomAttack ? HeavyGroundStabAttackRoutine() : HeavySpearStabAttackRoutine());
                }
                else
                {
                    m_grappleCoroutine = StartCoroutine(GrappleRoutine(false, true, 1/*, true*/));
                }
                m_animation.DisableRootMotion();
            }
            else
            {
                m_grappleCoroutine = StartCoroutine(GrappleRoutine(false, false, 1/*, true*/));
            }
            yield return null;
        }

        private IEnumerator HeavyGroundStabAttackRoutine()
        {
            for (int i = 0; i < m_info.heavyGroundStabRightAttacks.Count; i++)
            {
                m_lastTargetPos = m_targetInfo.position;
                string heavyGroundStabAnticipation = m_lastTargetPos.x > transform.position.x ? m_info.heavyGroundStabAnticipationRightAnimation : m_info.heavyGroundStabAnticipationLeftAnimation;
                m_animation.SetAnimation(0, heavyGroundStabAnticipation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, heavyGroundStabAnticipation);
                m_stabHeadBone.mode = SkeletonUtilityBone.Mode.Override;
                m_stabHeadBone.transform.position = new Vector2(m_lastTargetPos.x, GroundPosition(m_lastTargetPos).y);
                var heavyGroundStabAttackAnimation = heavyGroundStabAnticipation == m_info.heavyGroundStabAnticipationRightAnimation ? m_info.heavyGroundStabRightAttacks[i].animation : m_info.heavyGroundStabLeftAttacks[i].animation;
                m_animation.SetAnimation(0, heavyGroundStabAttackAnimation, false);
                //yield return new WaitForSeconds(0.25f);
                yield return new WaitForAnimationComplete(m_animation.animationState, heavyGroundStabAttackAnimation);
                m_stabHeadBone.mode = SkeletonUtilityBone.Mode.Follow;
            }
            m_animation.DisableRootMotion();
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator HeavySpearStabAttackRoutine()
        {
            //string heavyGroundStabAnticipation = m_targetInfo.position.x > transform.position.x ? m_info.heavySpearStabRightAttack.animation : m_info.heavySpearStabLeftAttack.animation;
            //m_animation.SetAnimation(0, heavyGroundStabAnticipation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, heavyGroundStabAnticipation);
            var heavySpearStabAttackAnimation = m_targetInfo.position.x > transform.position.x ? m_info.heavySpearStabRightAttack.animation : m_info.heavySpearStabLeftAttack.animation;
            m_animation.SetAnimation(0, heavySpearStabAttackAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, heavySpearStabAttackAnimation);
            m_animation.DisableRootMotion();
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator Phase3Pattern2AttackRoutine()
        {
            m_animation.EnableRootMotion(true, false);
            m_animation.AddAnimation(0, m_info.idleAnimation, true, 0);
            if (IsTargetInRange(m_info.spikeSpitterAttacks[0].range))
            {
                for (int i = 0; i < m_info.spikeSpitterAttacks.Count; i++)
                {
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

        private IEnumerator Phase3Pattern3AttackRoutine()
        {
            m_willStickToWall = true;
            m_animation.EnableRootMotion(true, false);
            m_animation.AddAnimation(0, m_info.idleAnimation, true, 0);
            RandomizeTentaclePosition();
            m_grappleCoroutine = StartCoroutine(GrappleRoutine(true, true, 1/*, true*/));
            yield return new WaitUntil(() => m_character.physics.simulateGravity);
            enabled = false;
            m_animation.EnableRootMotion(true, true);
            m_animation.SetAnimation(0, DynamicIdleAnimation(), true);
            for (int i = 0; i < m_info.spikeSpitterAttacks.Count; i++)
            {
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
            m_animation.SetEmptyAnimation(3, 0);
            m_animation.SetEmptyAnimation(15, 0);
            m_animation.DisableRootMotion();
            m_animation.SetAnimation(0, m_info.bodySlamStart, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bodySlamStart);
            while (!m_groundSensor.isDetecting)
            {
                m_animation.SetAnimation(0, m_info.bodySlamLoop, true);
                yield return null;
            }
            //if (m_character.facing != HorizontalDirection.Right)
            //    CustomTurn();
            m_willStickToWall = false;
            //m_animation.DisableRootMotion();
            //m_attackDecider.hasDecidedOnAttack = false;
            //m_currentAttackCoroutine = null;
            //m_stateHandle.ApplyQueuedState();
            yield return null;
            enabled = true;
        }

        private IEnumerator Phase3Pattern7AttackRoutine()
        {
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.krakenRageAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.krakenRageAttack.animation);
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
            m_animation.SetAnimation(0, m_info.krakenRageEndAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.krakenRageEndAnimation);
            m_animation.DisableRootMotion();
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
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
            m_character.physics.simulateGravity = true;
            StartCoroutine(DeathRoutine());
        }

        private IEnumerator DeathRoutine()
        {
            StartCoroutine(GrappleRetractRoutine(m_info.wallGrappleRetractAnimations.Count - 1));
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
                enabled = false;
                m_movement.Stop();
                m_animation.SetEmptyAnimation(9, 0);
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
            m_animation.SetEmptyAnimation(0, 0);
            m_animation.SetEmptyAnimation(3, 0);
            m_animation.SetEmptyAnimation(15, 0);
            m_animation.SetEmptyAnimation(27, 0);
            m_krakenRageBB.enabled = false;
            m_animation.SetAnimation(0, m_info.deathSelfDestructAnimation, false);
            yield return new WaitForSeconds(0.75f);
            m_selfDestructBB.enabled = true;
            yield return new WaitForSeconds(0.25f);
            m_selfDestructBB.enabled = false;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathSelfDestructAnimation);
            m_animation.SetAnimation(0, m_info.deathAnimation, true);
            enabled = false;
            yield return null;
        }

        #region Movement
        private void MoveToTarget(float targetRange)
        {
            var moveRight = m_willTentaspearChase ? m_info.tentaSpearRightCrawl : m_info.rightMove;
            var moveLeft = m_willTentaspearChase ? m_info.tentaSpearLeftCrawl : m_info.leftMove;
            if (!IsTargetInRange(targetRange) && m_groundSensor.isDetecting /*&& !m_wallSensor.isDetecting && m_edgeSensor.isDetecting*/)
            {
                m_animation.EnableRootMotion(true, false);
                m_animation.SetAnimation(0, m_targetInfo.position.x > transform.position.x ? moveRight.animation : moveLeft.animation, true);
                //m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.walk.speed);
            }
            else
            {
                m_movement.Stop();
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
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
                return m_groundSensor.isDetecting ? m_info.idleAnimation : m_info.idleMidAirAnimation;
            }
            return m_info.idleAnimation;
            //m_animation.SetEmptyAnimation(3, 0);
            //return m_groundSensor.isDetecting ? m_info.idleAnimation : m_info.idleMidAirAnimation;
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
                Debug.Log("Override Bones");
            }
            yield return new WaitUntil(() => !m_willGripWall && !m_willGripTarget);
            for (int i = 0; i < m_tentacleOverrideBones.Count; i++)
            {
                m_tentacleOverrideBones[i].mode = SkeletonUtilityBone.Mode.Follow;
            }
            yield return null;
        }

        private IEnumerator GrappleExtendRoutine(int tentaclesCount)
        {
            StartCoroutine(TentacleControlRoutine());
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
            yield return null;
        }

        private void RandomizeTentaclePosition()
        {
            for (int i = 0; i < m_tentacleOverridePoints.Count; i++)
            {
                for (int x = 0; x < m_tentacleOverridePoints.Count; x++)
                {
                    m_tentacleOverridePoints[i].position = RandomTentaclePointPosition(m_tentacleOverridePoints[i]);
                    if (Vector2.Distance(m_tentacleOverridePoints[i].position, m_tentacleOverridePoints[x].position) < 25f)
                    {
                        m_tentacleOverridePoints[i].position = RandomTentaclePointPosition(m_tentacleOverridePoints[i]);
                    }
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

        private Vector2 RandomTentaclePointPosition(Transform tentacle)
        {
            tentacle.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(m_info.grappleWidth.x, m_info.grappleWidth.y));
            int hitCount = 0;
            //RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down,  1000, DChildUtility.GetEnvironmentMask());
            RaycastHit2D[] hit = Cast(m_lastTargetPos, tentacle.right, 1000, true, out hitCount, true);
            Debug.DrawRay(tentacle.position, hit[0].point);
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

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Phase1Pattern1, m_info.phase1Pattern1Range)
                                    , new AttackInfo<Attack>(Attack.Phase1Pattern2, m_info.phase1Pattern2Range)
                                    , new AttackInfo<Attack>(Attack.Phase1Pattern3, m_info.phase1Pattern3Range)
                                    , new AttackInfo<Attack>(Attack.Phase1Pattern4, m_info.phase1Pattern4Range)
                                    , new AttackInfo<Attack>(Attack.Phase2Pattern2, m_info.phase2Pattern2Range)
                                    , new AttackInfo<Attack>(Attack.Phase2Pattern3, m_info.phase2Pattern3Range)
                                    , new AttackInfo<Attack>(Attack.Phase2Pattern4, m_info.phase2Pattern4Range)
                                    , new AttackInfo<Attack>(Attack.Phase2Pattern5, m_info.phase2Pattern5Range)
                                    , new AttackInfo<Attack>(Attack.Phase2Pattern6, m_info.phase2Pattern6Range)
                                    , new AttackInfo<Attack>(Attack.Phase3Pattern1, m_info.phase3Pattern1Range)
                                    , new AttackInfo<Attack>(Attack.Phase3Pattern2, m_info.phase3Pattern2Range)
                                    , new AttackInfo<Attack>(Attack.Phase3Pattern3, m_info.phase3Pattern3Range)
                                    , new AttackInfo<Attack>(Attack.Phase3Pattern4, m_info.phase3Pattern4Range)
                                    , new AttackInfo<Attack>(Attack.Phase3Pattern5, m_info.phase3Pattern5Range)
                                    , new AttackInfo<Attack>(Attack.Phase3Pattern6, m_info.phase3Pattern6Range)
                                    , new AttackInfo<Attack>(Attack.Phase3Pattern7, m_info.phase3Pattern7Range));
            m_attackDecider.hasDecidedOnAttack = false;
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
                        switch (m_currentAttack)
                        {
                            case Attack.Phase1Pattern1:
                                m_willTentaspearChase = true;
                                break;
                            case Attack.Phase1Pattern2:
                                m_willTentaspearChase = true;
                                break;
                            case Attack.Phase1Pattern3:
                                m_willTentaspearChase = true;
                                break;
                            case Attack.Phase1Pattern4:
                                m_willTentaspearChase = true;
                                break;
                            case Attack.Phase2Pattern1:
                                m_willTentaspearChase = true;
                                break;
                            case Attack.Phase2Pattern2:
                                m_willTentaspearChase = true;
                                break;
                            case Attack.Phase2Pattern3:
                                m_willTentaspearChase = true;
                                break;
                            case Attack.Phase2Pattern4:
                                m_willTentaspearChase = true;
                                break;
                            case Attack.Phase2Pattern5:
                                m_willTentaspearChase = true;
                                break;
                            case Attack.Phase2Pattern6:
                                m_willTentaspearChase = true;
                                break;
                            case Attack.Phase3Pattern1:
                                m_willTentaspearChase = true;
                                break;
                            case Attack.Phase3Pattern2:
                                m_willTentaspearChase = true;
                                break;
                            case Attack.Phase3Pattern3:
                                m_willTentaspearChase = true;
                                break;
                            case Attack.Phase3Pattern4:
                                m_willTentaspearChase = true;
                                break;
                            case Attack.Phase3Pattern5:
                                m_willTentaspearChase = true;
                                break;
                            case Attack.Phase3Pattern6:
                                m_willTentaspearChase = true;
                                break;
                            case Attack.Phase3Pattern7:
                                m_willTentaspearChase = true;
                                break;
                        }
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
            m_damageable.DamageTaken += OnDamageTaken;
            m_flinchRighthHandle.FlinchStart += OnFlinchStart;
            m_flinchLeftHandle.FlinchStart += OnFlinchStart;
            m_flinchRighthHandle.FlinchEnd += OnFlinchEnd;
            m_flinchLeftHandle.FlinchEnd += OnFlinchEnd;
            //m_damageable.DamageTaken += OnDamageBlocked;
            //m_patternDecider = new RandomAttackDecider<Pattern>();
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
            m_animation.DisableRootMotion();
            m_phaseHandle = new PhaseHandle<Phase, PhaseInfo>();
            m_phaseHandle.Initialize(Phase.PhaseOne, m_info.phaseInfo, m_character, ChangeState, ApplyPhaseData);
            m_phaseHandle.ApplyChange();
            m_spineListener.Subscribe(m_info.singleShotEvent, LaunchSingleProjectile);
            m_spineListener.Subscribe(m_info.multiShotEvent, LaunchMultiProjectile);
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
            m_phaseHandle.MonitorPhase();
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
                        m_changePhaseCoroutine = StartCoroutine(ChangePhaseRoutine());
                    }
                    if (!m_groundSensor.isDetecting)
                    {
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    }
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
                            m_currentAttackCoroutine = StartCoroutine(Phase1Pattern3And4AttackRoutine(false));
                            m_pickedCooldown = m_currentFullCooldown[2];
                            break;
                        case Attack.Phase1Pattern4:
                            m_currentAttackCoroutine = StartCoroutine(Phase1Pattern3And4AttackRoutine(true));
                            m_pickedCooldown = m_currentFullCooldown[3];
                            break;
                        case Attack.Phase2Pattern1:
                            m_currentAttackCoroutine = StartCoroutine(Phase2Pattern1AttackRoutine());
                            m_pickedCooldown = m_currentFullCooldown[0];
                            break;
                        case Attack.Phase2Pattern2:
                            m_currentAttackCoroutine = StartCoroutine(Phase1Pattern2AttackRoutine());
                            m_pickedCooldown = m_currentFullCooldown[1];
                            break;
                        case Attack.Phase2Pattern3:
                            m_currentAttackCoroutine = StartCoroutine(Phase1Pattern3And4AttackRoutine(false));
                            m_pickedCooldown = m_currentFullCooldown[2];
                            break;
                        case Attack.Phase2Pattern4:
                            m_currentAttackCoroutine = StartCoroutine(Phase1Pattern3And4AttackRoutine(true));
                            m_pickedCooldown = m_currentFullCooldown[3];
                            break;
                        case Attack.Phase2Pattern5:
                            m_currentAttackCoroutine = StartCoroutine(HeavyGroundStabAttackRoutine());
                            m_pickedCooldown = m_currentFullCooldown[4];
                            break;
                        case Attack.Phase2Pattern6:
                            m_currentAttackCoroutine = StartCoroutine(HeavySpearStabAttackRoutine());
                            m_pickedCooldown = m_currentFullCooldown[5];
                            break;
                        #region WIP ATTACK PATTERNS
                        case Attack.Phase3Pattern1:
                            if (IsTargetInRange(m_info.heavyGroundStabLeftAttacks[0].range))
                            {
                                m_currentAttackCoroutine = StartCoroutine(HeavySpearStabAttackRoutine());
                            }
                            else
                            {
                                m_currentAttackCoroutine = StartCoroutine(GrappleRoutine(false, true, m_info.bodySlamCount));
                            }
                            m_pickedCooldown = m_currentFullCooldown[0];
                            break;
                        case Attack.Phase3Pattern2:
                            m_currentAttackCoroutine = StartCoroutine(Phase3Pattern2AttackRoutine());
                            m_pickedCooldown = m_currentFullCooldown[1];
                            break;
                        case Attack.Phase3Pattern3:
                            m_currentAttackCoroutine = StartCoroutine(Phase3Pattern3AttackRoutine());
                            m_pickedCooldown = m_currentFullCooldown[2];
                            break;
                        case Attack.Phase3Pattern4:
                            m_currentAttackCoroutine = StartCoroutine(Phase1Pattern3And4AttackRoutine(true));
                            m_pickedCooldown = m_currentFullCooldown[3];
                            break;
                        case Attack.Phase3Pattern5:
                            m_currentAttackCoroutine = StartCoroutine(HeavyGroundStabAttackRoutine()); //LEAVE ALONE
                            m_pickedCooldown = m_currentFullCooldown[4];
                            break;
                        case Attack.Phase3Pattern6:
                            m_currentAttackCoroutine = StartCoroutine(HeavySpearStabAttackRoutine()); //LEAVE ALONE
                            m_pickedCooldown = m_currentFullCooldown[5];
                            break;
                        case Attack.Phase3Pattern7:
                            m_currentAttackCoroutine = StartCoroutine(Phase3Pattern7AttackRoutine());
                            m_pickedCooldown = m_currentFullCooldown[6];
                            break;
                            #endregion
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
                        //m_stateHandle.OverrideState(State.ReevaluateSituation);
                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                    }

                    break;

                case State.Chasing:
                    if (!m_hitbox.canBlockDamage)
                    {
                        ChooseAttack();
                        if (m_character.facing != HorizontalDirection.Right)
                            CustomTurn();
                        if (IsTargetInRange(m_currentAttackRange) && m_currentAttackCoroutine == null)
                        {
                            m_animation.SetEmptyAnimation(0, 0);
                            m_stateHandle.SetState(State.Attacking);
                        }
                        else
                        {
                            MoveToTarget(m_currentAttackRange);
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