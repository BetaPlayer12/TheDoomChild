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

            [SerializeField, TitleGroup("Attack Behaviours")]
            private float m_retractPullSpeed;
            public float retractPullSpeed => m_retractPullSpeed;
            [SerializeField, BoxGroup("HeavyGroundStab"), Space]
            private SimpleAttackInfo m_heavyGroundStab1LeftAttack = new SimpleAttackInfo();
            public SimpleAttackInfo heavyGroundStab1LeftAttack => m_heavyGroundStab1LeftAttack;
            [SerializeField, BoxGroup("HeavyGroundStab")]
            private SimpleAttackInfo m_heavyGroundStab1RightAttack = new SimpleAttackInfo();
            public SimpleAttackInfo heavyGroundStab1RightAttack => m_heavyGroundStab1RightAttack;
            [SerializeField, BoxGroup("HeavyGroundStab")]
            private SimpleAttackInfo m_heavyGroundStab2LeftAttack = new SimpleAttackInfo();
            public SimpleAttackInfo heavyGroundStab2LeftAttack => m_heavyGroundStab2LeftAttack;
            [SerializeField, BoxGroup("HeavyGroundStab")]
            private SimpleAttackInfo m_heavyGroundStab2RightAttack = new SimpleAttackInfo();
            public SimpleAttackInfo heavyGroundStab2RightAttack => m_heavyGroundStab2RightAttack;
            [SerializeField, BoxGroup("HeavyGroundStab")]
            private SimpleAttackInfo m_heavyGroundStab3LeftAttack = new SimpleAttackInfo();
            public SimpleAttackInfo heavyGroundStab3LeftAttack => m_heavyGroundStab3LeftAttack;
            [SerializeField, BoxGroup("HeavyGroundStab")]
            private SimpleAttackInfo m_heavyGroundStab3RightAttack = new SimpleAttackInfo();
            public SimpleAttackInfo heavyGroundStab3RightAttack => m_heavyGroundStab3RightAttack;
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
            private SimpleAttackInfo m_krakenRageAttack = new SimpleAttackInfo();
            public SimpleAttackInfo krakenRageAttackn => m_krakenRageAttack;
            [SerializeField, BoxGroup("KrakenRage"), ValueDropdown("GetAnimations")]
            private string m_krakenRageLoopAnimation;
            public string krakenRageLoopAnimation => m_krakenRageLoopAnimation;
            [SerializeField, BoxGroup("KrakenRage"), ValueDropdown("GetAnimations")]
            private string m_krakenRageEndAnimation;
            public string krakenRageEndAnimation => m_krakenRageEndAnimation;
            [SerializeField, BoxGroup("SpikeSpitter")]
            private SimpleAttackInfo m_spikeSpitter1Attack = new SimpleAttackInfo();
            public SimpleAttackInfo spikeSpitter1Attack => m_spikeSpitter1Attack;
            [SerializeField, BoxGroup("SpikeSpitter"), ValueDropdown("GetAnimations")]
            private string m_spikeSpitter1ExtendAnimation;
            public string spikeSpitter1ExtendAnimation => m_spikeSpitter1ExtendAnimation;
            [SerializeField, BoxGroup("SpikeSpitter"), ValueDropdown("GetAnimations")]
            private string m_spikeSpitter1ExtractAnimation;
            public string spikeSpitter1ExtractAnimation => m_spikeSpitter1ExtractAnimation;
            [SerializeField, BoxGroup("SpikeSpitter")]
            private SimpleAttackInfo m_spikeSpitter2Attack = new SimpleAttackInfo();
            public SimpleAttackInfo spikeSpitter2Attack => m_spikeSpitter2Attack;
            [SerializeField, BoxGroup("SpikeSpitter"), ValueDropdown("GetAnimations")]
            private string m_spikeSpitter2ExtendAnimation;
            public string spikeSpitter2ExtendAnimation => m_spikeSpitter2ExtendAnimation;
            [SerializeField, BoxGroup("SpikeSpitter"), ValueDropdown("GetAnimations")]
            private string m_spikeSpitter2ExtractAnimation;
            public string spikeSpitter2ExtractAnimation => m_spikeSpitter2ExtractAnimation;
            [SerializeField, BoxGroup("BodySlam")]
            private SimpleAttackInfo m_bodySlamGroundNearAttack = new SimpleAttackInfo();
            public SimpleAttackInfo bodySlamGroundNearAttack => m_bodySlamGroundNearAttack;
            [SerializeField, BoxGroup("BodySlam")]
            private SimpleAttackInfo m_bodySlamGroundFarAttack = new SimpleAttackInfo();
            public SimpleAttackInfo bodySlamGroundFarAttack => m_bodySlamGroundFarAttack;
            
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

            //[Title("Projectiles")]
            //[SerializeField]
            //private SimpleProjectileAttackInfo m_slashProjectile;
            //public SimpleProjectileAttackInfo slashProjectile => m_slashProjectile;
            //[SerializeField]
            //private SimpleProjectileAttackInfo m_scytheWaveProjectile;
            //public SimpleProjectileAttackInfo scytheWaveProjectile => m_scytheWaveProjectile;

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
                m_tentaSpearLeftCrawl.SetData(m_skeletonDataAsset);
                m_tentaSpearRightCrawl.SetData(m_skeletonDataAsset);
                m_leftMove.SetData(m_skeletonDataAsset);
                m_rightMove.SetData(m_skeletonDataAsset);
                m_heavyGroundStab1LeftAttack.SetData(m_skeletonDataAsset);
                m_heavyGroundStab1RightAttack.SetData(m_skeletonDataAsset);
                m_heavyGroundStab2LeftAttack.SetData(m_skeletonDataAsset);
                m_heavyGroundStab2RightAttack.SetData(m_skeletonDataAsset);
                m_heavyGroundStab3LeftAttack.SetData(m_skeletonDataAsset);
                m_heavyGroundStab3RightAttack.SetData(m_skeletonDataAsset);
                m_heavySpearStabLeftAttack.SetData(m_skeletonDataAsset);
                m_heavySpearStabRightAttack.SetData(m_skeletonDataAsset);
                m_krakenRageAttack.SetData(m_skeletonDataAsset);
                m_spikeSpitter1Attack.SetData(m_skeletonDataAsset);
                m_spikeSpitter2Attack.SetData(m_skeletonDataAsset);
                m_bodySlamGroundNearAttack.SetData(m_skeletonDataAsset);
                m_bodySlamGroundFarAttack.SetData(m_skeletonDataAsset);
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
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_bodyCollider;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_legCollider;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_model;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        //[SerializeField, TabGroup("Hurtbox")]
        //private Collider2D m_swordSlash1BB;

        //[SerializeField, TabGroup("FX")]
        //private ParticleFX m_earthShakerExplosionFX;

        #region TentaclVariables
        [SerializeField, TabGroup("Tentacle Points")]
        private List<SkeletonUtilityBone> m_tentacleOverrideBones;
        [SerializeField, TabGroup("Tentacle Points")]
        private List<Transform> m_tentacleOverridePoints;
        private int m_wallGrappleDirectionIndex;
        private bool m_willGripWall;
        #endregion
        //[SerializeField, TabGroup("Spawn Points")]
        //private Transform m_projectilePoint;
        //[SerializeField, TabGroup("Spawn Points")]
        //private Transform m_scytheWavePoint;
        //[SerializeField, TabGroup("IK Control")]
        //private SkeletonUtilityBone m_targetIK;

        //private ProjectileLauncher m_projectileLauncher;
        //private ProjectileLauncher m_scytheWaveLauncher;

        [SerializeField]
        private SpineEventListener m_spineListener;

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
        private int m_grappleEvadeCurrentHitCount;

        private Coroutine m_currentAttackCoroutine;
        private Coroutine m_changePhaseCoroutine;
        private Coroutine m_grappleEvadeCoroutine;

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
            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseOne:
                    AddToAttackCache(Attack.Phase1Pattern1, Attack.Phase1Pattern2, Attack.Phase1Pattern3, Attack.Phase1Pattern4);
                    AddToRangeCache(m_info.phase1Pattern1Range, m_info.phase1Pattern2Range, m_info.phase1Pattern3Range, m_info.phase1Pattern4Range);
                    for (int i = 0; i < m_info.phase1PatternCooldown.Count; i++)
                        m_patternCooldown.Add(m_info.phase1PatternCooldown[i]);
                    break;
                case Phase.PhaseTwo:
                    AddToAttackCache(Attack.Phase2Pattern1, Attack.Phase2Pattern2, Attack.Phase2Pattern3, Attack.Phase2Pattern4, Attack.Phase2Pattern5, Attack.Phase2Pattern6);
                    AddToRangeCache(m_info.phase2Pattern1Range, m_info.phase2Pattern2Range, m_info.phase2Pattern3Range, m_info.phase2Pattern4Range, m_info.phase2Pattern5Range, m_info.phase2Pattern6Range);
                    for (int i = 0; i < m_info.phase2PatternCooldown.Count; i++)
                        m_patternCooldown.Add(m_info.phase2PatternCooldown[i]);
                    break;
                case Phase.PhaseThree:
                    AddToAttackCache(Attack.Phase3Pattern1, Attack.Phase3Pattern2, Attack.Phase3Pattern3, Attack.Phase3Pattern4, Attack.Phase3Pattern5, Attack.Phase3Pattern6, Attack.Phase3Pattern7);
                    AddToRangeCache(m_info.phase3Pattern1Range, m_info.phase3Pattern2Range, m_info.phase3Pattern3Range, m_info.phase3Pattern4Range, m_info.phase3Pattern5Range, m_info.phase3Pattern7Range);
                    for (int i = 0; i < m_info.phase3PatternCooldown.Count; i++)
                        m_patternCooldown.Add(m_info.phase3PatternCooldown[i]);
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

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                m_stateHandle.OverrideState(State.Intro);
                GameEventMessage.SendEvent("Boss Encounter");
            }
        }

        private void OnDamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
        {
            if (m_grappleEvadeCoroutine == null)
            {
                switch (m_phaseHandle.currentPhase)
                {
                    case Phase.PhaseOne:
                        if (m_grappleEvadeCurrentHitCount < m_info.grappleEvadeHitCount)
                            m_grappleEvadeCurrentHitCount++;
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
                            
                            m_stateHandle.Wait(State.ReevaluateSituation);

                            m_grappleEvadeCoroutine = StartCoroutine(GrappleEvadeRoutine());
                            m_grappleEvadeCurrentHitCount = 0;

                        }
                        break;
                    case Phase.PhaseTwo:
                        if (m_grappleEvadeCurrentHitCount < m_info.grappleEvadeHitCount)
                            m_grappleEvadeCurrentHitCount++;
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

                            m_stateHandle.Wait(State.ReevaluateSituation);

                            m_grappleEvadeCoroutine = StartCoroutine(GrappleEvadeRoutine());
                            m_grappleEvadeCurrentHitCount = 0;

                        }
                        break;
                }
            }
        }

        private IEnumerator TentacleControlRoutine()
        {
            for (int i = 0; i < m_tentacleOverrideBones.Count; i++)
            {
                m_tentacleOverrideBones[i].mode = SkeletonUtilityBone.Mode.Override;
                Debug.Log("Override Bones");
            }
            while (m_willGripWall)
            {
                for (int i = 0; i < m_tentacleOverrideBones.Count; i++)
                {
                    if (m_tentacleOverrideBones[i].transform.position != m_tentacleOverridePoints[i].position)
                    {
                        m_tentacleOverrideBones[i].transform.position = m_tentacleOverridePoints[i].position;
                    }
                }
                m_animation.SetAnimation(0, DynamicIdleAnimation(), true);
                Debug.Log("Override BonePosition");
                yield return null;
            }
            yield return new WaitUntil(() => !m_willGripWall);
            for (int i = 0; i < m_tentacleOverrideBones.Count; i++)
            {
                m_tentacleOverrideBones[i].mode = SkeletonUtilityBone.Mode.Follow;
            }
            yield return null;
        }

        private IEnumerator GrappleEvadeRoutine()
        {
            CalculateWallGrapple();
            //m_hitbox.Disable();
            m_animation.DisableRootMotion();
            m_character.physics.simulateGravity = false;
            m_willGripWall = true;
            StartCoroutine(TentacleControlRoutine());
            for (int i = 0; i < 4; i++)
            {
                m_animation.SetAnimation(i + 5, m_info.wallGrappleExtendAnimations[i + m_wallGrappleDirectionIndex], false);
            }
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.wallGrappleExtendAnimations[m_wallGrappleDirectionIndex]);
            for (int i = 0; i < 4; i++)
            {
                m_animation.SetAnimation(i + 5, m_info.wallGrappleAnimations[i + m_wallGrappleDirectionIndex], false);
            }
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.wallGrappleAnimations[m_wallGrappleDirectionIndex]);
            while (true)
            {
                //Vector2.MoveTowards(transform.position, m_tentaclePoints[m_wallGrappleDirectionIndex].position, m_info.retractPullSpeed * Time.deltaTime);
                yield return null;
            }
            for (int i = 0; i < 4; i++)
            {
                m_animation.SetAnimation(i + 5, m_info.wallGrappleRetractAnimations[i + m_wallGrappleDirectionIndex], false);
            }
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.wallGrappleRetractAnimations[m_wallGrappleDirectionIndex]);
            m_willGripWall = false;
            //if (!m_groundSensor.isDetecting)
            //{
            //}
            //m_animation.EnableRootMotion(true, false);
            //if (!IsFacingTarget())
            //    CustomTurn();

            m_animation.SetAnimation(0, m_info.bodySlamGroundFarAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.bodySlamGroundFarAttack.animation);
            m_character.physics.simulateGravity = true;
            //m_hitbox.Enable();
            m_grappleEvadeCoroutine = null;
            m_hitbox.SetCanBlockDamageState(false);
            //TEMP
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            //TEMP
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private void CustomTurn()
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
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
            ResetCounterCounts();
            SetAIToPhasing();
            yield return null;
        }

        private void SetAIToPhasing()
        {
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
        }

        private void ResetCounterCounts()
        {
            m_grappleEvadeCurrentHitCount = 0;
        }

        private IEnumerator ChangePhaseRoutine()
        {
            m_stateHandle.Wait(State.Chasing);
            if (IsFacingTarget())
                CustomTurn();

            m_hitbox.Disable();
            var flinchAnimation = m_targetInfo.position.x > transform.position.x ? m_info.flinchLeftAnimation : m_info.flinchRightAnimation;
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, flinchAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, flinchAnimation);
            m_hitbox.Enable();
            m_hitbox.SetCanBlockDamageState(false);
            m_changePhaseCoroutine = null;
            yield return null;
        }
        #region Attacks

        //private void LaunchProjectile()
        //{
        //    if (!IsFacingTarget())
        //        CustomTurn();

        //    m_projectileLauncher.AimAt(m_targetInfo.position);
        //    m_projectileLauncher.LaunchProjectile();
        //    StartCoroutine(ProjectileIKControlRoutine());
        //}

        private IEnumerator Phase1Pattern1AttackRoutine()
        {
            m_animation.EnableRootMotion(true, false);
            //yield return new Wait
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
        }

        #region Movement
        private void MoveToTarget(float targetRange)
        {
            if (!IsTargetInRange(targetRange) && m_groundSensor.isDetecting /*&& !m_wallSensor.isDetecting && m_edgeSensor.isDetecting*/)
            {
                m_animation.EnableRootMotion(true, false);
                m_animation.SetAnimation(0, m_character.facing == HorizontalDirection.Right ? m_info.tentaSpearRightCrawl.animation : m_info.tentaSpearLeftCrawl.animation, true);
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
            return m_groundSensor.isDetecting ? m_info.idleAnimation : m_info.idleMidAirAnimation;
        }

        private void CalculateWallGrapple()
        {
            if (m_targetInfo.position.x > transform.position.x)
            {
                m_wallGrappleDirectionIndex = 4;
            }
            else
            {
                m_wallGrappleDirectionIndex = 0;
            }
        }

        private Vector2 GroundPosition(Vector2 startPoint)
        {
            int hitCount = 0;
            //RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down,  1000, DChildUtility.GetEnvironmentMask());
            RaycastHit2D[] hit = Cast(startPoint, Vector2.down, 1000, true, out hitCount, true);
            Debug.DrawRay(startPoint, hit[0].point);
            //var hitPos = (new Vector2(m_projectilePoint.position.x, Vector2.down.y) * hit[0].distance);
            //return hitPos;
            return hit[0].point;
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
            m_damageable.DamageTaken += OnDamageTaken;
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
                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    m_lastTargetPos = m_targetInfo.position;

                    Debug.Log("CURRENT ATTACK PATTERN " + m_currentAttack);
                    switch (m_currentAttack)
                    {
                        case Attack.Phase1Pattern1:
                            m_currentAttackCoroutine = StartCoroutine(GrappleEvadeRoutine());
                            m_pickedCooldown = m_currentFullCooldown[0];
                            break;
                        case Attack.Phase1Pattern2:
                            m_currentAttackCoroutine = StartCoroutine(GrappleEvadeRoutine());
                            m_pickedCooldown = m_currentFullCooldown[1];
                            break;
                        case Attack.Phase1Pattern3:
                            m_currentAttackCoroutine = StartCoroutine(GrappleEvadeRoutine());
                            m_pickedCooldown = m_currentFullCooldown[2];
                            break;
                        case Attack.Phase1Pattern4:
                            m_currentAttackCoroutine = StartCoroutine(GrappleEvadeRoutine());
                            m_pickedCooldown = m_currentFullCooldown[3];
                            break;
                        //case Attack.Phase2Pattern1:
                        //    m_currentAttackCoroutine = StartCoroutine(Phase2Pattern1AttackRoutine());
                        //    m_pickedCooldown = m_currentFullCooldown[0];
                        //    break;
                        //case Attack.Phase2Pattern2:
                        //    m_currentAttackCoroutine = StartCoroutine(Phase2Pattern2AttackRoutine());
                        //    m_pickedCooldown = m_currentFullCooldown[1];
                        //    break;
                        //case Attack.Phase2Pattern3:
                        //    m_currentAttackCoroutine = StartCoroutine(Phase2Pattern3AttackRoutine());
                        //    m_pickedCooldown = m_currentFullCooldown[2];
                        //    break;
                        //case Attack.Phase2Pattern4:
                        //    if (m_patternCooldown[3] == m_info.phase2PatternCooldown[3])
                        //    {
                        //        if (m_currentSwordState != SwordState.Normal)
                        //        {
                        //            m_currentAttackCoroutine = StartCoroutine(Phase1Pattern4AttackRoutine());
                        //            m_pickedCooldown = m_currentFullCooldown[3];
                        //            StartCoroutine(CooldownMonitorRoutine(3));
                        //        }
                        //        else
                        //        {
                        //            //m_attackDecider.hasDecidedOnAttack = false;
                        //            //m_currentAttackCoroutine = null;
                        //            //if (m_alterBladeCoroutine == null)
                        //            //    m_stateHandle.ApplyQueuedState();
                        //            m_currentAttackCoroutine = StartCoroutine(DrillDashComboRoutine());
                        //        }
                        //    }
                        //    else
                        //    {
                        //        m_currentAttackCoroutine = StartCoroutine(DrillDashComboRoutine());
                        //    }
                        //    break;
                        //case Attack.Phase2Pattern5:
                        //    m_currentAttackCoroutine = StartCoroutine(Phase2Pattern5AttackRoutine());
                        //    m_pickedCooldown = m_currentFullCooldown[4];
                        //    break;
                    }

                    break;

                case State.Cooldown:
                    m_animation.SetAnimation(0, m_idleAnimation, true).TimeScale = 1;

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
                        if (IsTargetInRange(m_currentAttackRange) && m_currentAttackCoroutine == null)
                        {
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

            m_idleAnimation = m_groundSensor.isDetecting ? m_info.idleAnimation : m_info.idleMidAirAnimation;
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