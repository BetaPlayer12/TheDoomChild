using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

namespace DChild.Gameplay.Characters.Enemies
{

    [AddComponentMenu("DChild/Gameplay/Enemies/Boss/FrankyAI")]
    public class FrankyAI : CombatAIBrain<FrankyAI.Info>
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

            [Title("Attack Behaviours")]
            #region ShoulderBash
            [SerializeField, Range(0, 100)]
            private float m_shoulderBashReelSpeed;
            public float shoulderBashReelSpeed => m_shoulderBashReelSpeed;
            [SerializeField, Range(0, 500)]
            private float m_punchSpeed;
            public float punchSpeed => m_punchSpeed;
            [Title("Attack Behaviours")]
            [SerializeField]
            private Vector2 m_shoulderBashVelocity;
            public Vector2 shoulderBashVelocity => m_shoulderBashVelocity;
            [SerializeField]
            private SimpleAttackInfo m_shoulderBashAttack = new SimpleAttackInfo();
            public SimpleAttackInfo shoulderBashAttack => m_shoulderBashAttack;

            [SerializeField]
            private BasicAnimationInfo m_shoulderBashLoopAnimation;
            public BasicAnimationInfo shoulderBashLoopAnimation => m_shoulderBashLoopAnimation;
            [SerializeField]
            private BasicAnimationInfo m_shoulderBashEndAnimation;
            public BasicAnimationInfo shoulderBashEndAnimation => m_shoulderBashEndAnimation;
            [SerializeField]
            private BasicAnimationInfo m_shoulderBashAnimation;
            public BasicAnimationInfo shoulderBashAnimation => m_shoulderBashAnimation;
            [SerializeField]
            private BasicAnimationInfo m_electricStomp;
            public BasicAnimationInfo electricStomp => m_electricStomp;
            #endregion
            #region PunchCombo
            [SerializeField]
            private SimpleAttackInfo m_punchComboAttack;
            public SimpleAttackInfo punchComboAttack => m_punchComboAttack;

            [SerializeField]
            private BasicAnimationInfo m_punchComboAnimation;
            public BasicAnimationInfo punchComboAnimation => m_punchComboAnimation;
            #endregion
            #region ChainFistPunch
            [SerializeField]
            private float m_punchVelocity;
            public float punchVelocity => m_punchVelocity;
            [SerializeField]
            private BasicAnimationInfo m_chainFistPunchAttackAnticipation;
            public BasicAnimationInfo chainFistAttackAnticipation => m_chainFistPunchAttackAnticipation;
            [SerializeField]
            private SimpleAttackInfo m_chainFistPunchAttack = new SimpleAttackInfo();
            public SimpleAttackInfo chainFistPunchAttack => m_chainFistPunchAttack;
            [SerializeField]
            private BasicAnimationInfo m_chainFistPunchUpperAnimation;
            public BasicAnimationInfo chainFistPunchUpperAnimation => m_chainFistPunchUpperAnimation;
            #endregion
            #region LeapAttack
            [SerializeField]
            private MovementInfo m_leapAttackStartAnimation;
            public MovementInfo leapAttackStartAnimation => m_leapAttackStartAnimation;
            [SerializeField]
            private BasicAnimationInfo m_leapAnticipation;
            public BasicAnimationInfo leapAnticipation => m_leapAnticipation;
            [SerializeField]
            private BasicAnimationInfo m_leapLoopAnimation;
            public BasicAnimationInfo leapLoopAnimation => m_leapLoopAnimation;
            [SerializeField]
            private BasicAnimationInfo m_leapLoopAnimation2;
            public BasicAnimationInfo leapLoopAnimation2 => m_leapLoopAnimation2;
            [SerializeField]
            private BasicAnimationInfo m_leapAttackEndAnimation;
            public BasicAnimationInfo leapAttackEndAnimation => m_leapAttackEndAnimation;
            [SerializeField, TabGroup("Leap Attack Values")]
            private float m_leapVelocity;
            public float leapVelocity => m_leapVelocity;
            [SerializeField, MinValue(0), TabGroup("Leap Attack Values")]
            private float m_leapTime;
            public float leapTime => m_leapTime;
            [SerializeField, TabGroup("Leap Attack Values")]
            private float m_transitionStart;
            public float transitionStart => m_transitionStart;
            #endregion


            #region ChainBash
            [SerializeField]
            private BasicAnimationInfo m_chainBashNearEdge;
            public BasicAnimationInfo chainBashNearEdge => m_chainBashNearEdge;
            [SerializeField]
            private BasicAnimationInfo m_chainBashMiddle;
            public BasicAnimationInfo chainBashMiddle => m_chainBashMiddle;
            [SerializeField]
            private BasicAnimationInfo m_chainBashPullingLoop;
            public BasicAnimationInfo chainBashPullingLoop => m_chainBashPullingLoop;
            [SerializeField]
            private BasicAnimationInfo m_chainBashPullingOut;
            public BasicAnimationInfo chainBashPullingOut => m_chainBashPullingOut;
            [SerializeField]
            private BasicAnimationInfo m_chainBashIILightningReleaseStart;
            public BasicAnimationInfo chainBashIILightningReleaseStart => m_chainBashIILightningReleaseStart;
            [SerializeField]
            private BasicAnimationInfo m_chainBashIILightningReleaseLoop;
            public BasicAnimationInfo chainBashIILightningReleaseLoop => m_chainBashIILightningReleaseLoop;
            [SerializeField]
            private BasicAnimationInfo m_chainBashIILightningReleaseEnd;
            public BasicAnimationInfo chainBashIILightningReleaseEnd => m_chainBashIILightningReleaseEnd;
            [SerializeField]
            private float m_chainBashDuration;
            public float ChainBashDuration => m_chainBashDuration;
            #endregion
            #region RunAttack
            [SerializeField]
            private SimpleAttackInfo m_runAttack = new SimpleAttackInfo();
            public SimpleAttackInfo runAttack => m_runAttack;
            [SerializeField]
            private BasicAnimationInfo m_runAttackStartAnimation;
            public BasicAnimationInfo runAttackStartAnimation => m_runAttackStartAnimation;
            [SerializeField]
            private BasicAnimationInfo m_runAttackAnimation;
            public BasicAnimationInfo runAttackAnimation => m_runAttackAnimation;
            [SerializeField]
            private BasicAnimationInfo m_runAttackEndAnimation;
            public BasicAnimationInfo runAttackEndAnimation => m_runAttackEndAnimation;
            //[SerializeField, TabGroup("Run Attack Values")]
            //private float m_runAttackDistance;
            //public float runAttackDistance => m_runAttackDistance;
            [SerializeField, TabGroup("Run Attack Values")]
            private float m_runAttackSpeed;
            public float runAttackSpeed => m_runAttackSpeed;
            #endregion

            #region ChainShock
            [SerializeField]
            private SimpleAttackInfo m_chainShockAttack = new SimpleAttackInfo();
            public SimpleAttackInfo chainShockAttack => m_chainShockAttack;
            [SerializeField]
            private BasicAnimationInfo m_chainShockLoopAnimation;
            public BasicAnimationInfo chainShockLoopAnimation => m_chainShockLoopAnimation;
            [SerializeField]
            private BasicAnimationInfo m_chainShockEndAnimation;
            public BasicAnimationInfo chainShockEndAnimation => m_chainShockEndAnimation;
            [SerializeField]
            private float m_shockTime;
            public float shockTime => m_shockTime;
            #endregion

            #region ShockRampage
            [SerializeField]
            private SimpleAttackInfo m_shockRampageAttack = new SimpleAttackInfo();
            public SimpleAttackInfo shockRampageAttack => m_shockRampageAttack;

            #endregion

            #region PhaseDischarge
            [SerializeField]
            private SimpleAttackInfo m_phaseDischarge = new SimpleAttackInfo();
            public SimpleAttackInfo phaseDischarge => m_phaseDischarge;

            #endregion

            [SerializeField]
            private SimpleAttackInfo m_lightningStompAttack = new SimpleAttackInfo();
            public SimpleAttackInfo lightningStompAttack => m_lightningStompAttack;

            [SerializeField, TabGroup("Phase 1"), BoxGroup("Pattern Ranges")]
            private float m_phase1Pattern1Range;
            public float phase1Pattern1Range => m_phase1Pattern1Range;
            [SerializeField, TabGroup("Phase 1"), BoxGroup("Pattern Ranges")]
            private float m_phase2Pattern1Range;
            public float phase2Pattern1Range => m_phase2Pattern1Range;
            [SerializeField, TabGroup("Phase 2"), BoxGroup("Pattern Ranges")]
            private float m_phase2Pattern2Range;
            public float phase2Pattern2Range => m_phase2Pattern2Range;
            [SerializeField, TabGroup("Phase 2"), BoxGroup("Pattern Ranges")]
            private float m_phase3Pattern1Range;
            public float phase3Pattern1Range => m_phase3Pattern1Range;
            [SerializeField, TabGroup("Phase 2"), BoxGroup("Pattern Ranges")]
            private float m_phase3Pattern2Range;
            public float phase3Pattern2Range => m_phase3Pattern2Range;

            [Title("Misc")]
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;
            [Title("Misc")]
            [SerializeField]
            private float m_walkSpeed;
            public float walkSpeed => m_walkSpeed;

            [Title("Animations")]
            [SerializeField]
            private BasicAnimationInfo m_introAnimation;
            public BasicAnimationInfo introAnimation => m_introAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idle2Animation;
            public BasicAnimationInfo idle2Animation => m_idle2Animation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation;
            public BasicAnimationInfo turnAnimation => m_turnAnimation;
            [SerializeField]
            private BasicAnimationInfo m_roarAnimation;
            public BasicAnimationInfo roarAnimation => m_roarAnimation;
            [SerializeField]
            private BasicAnimationInfo m_hookTravelLoopAnimation;
            public BasicAnimationInfo hookTravelLoopAnimation => m_hookTravelLoopAnimation;
            [SerializeField]
            private BasicAnimationInfo m_hookBackLoopAnimation;
            public BasicAnimationInfo hookBackLoopAnimation => m_hookBackLoopAnimation;

            [Title("Projectiles")]
            [SerializeField]
            private SimpleProjectileAttackInfo m_stompProjectile;
            public SimpleProjectileAttackInfo stompProjectile => m_stompProjectile;

            [Title("FX")]
            [SerializeField]
            private GameObject m_lightningBoltFX;
            public GameObject lightningBoltFX => m_lightningBoltFX;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_phaseEvent;
            public string phaseEvent => m_phaseEvent;
            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_shockRampageColliderEventOn;
            public string shockRampageColliderEventOn => m_shockRampageColliderEventOn;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_shockRampageColliderEventOff;
            public string shockRampageColliderEventOff => m_shockRampageColliderEventOff;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_stopRoarEvent;
            public string stopRoarEvent => m_stopRoarEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_stompEvent;
            public string stompEvent => m_stompEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_leapEvent;
            public string leapEvent => m_leapEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_aimChainBash;
            public string aimChainBash => m_aimChainBash;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_shoulderBashEventAttack;
            public string shoulderBashEventAttack => m_shoulderBashEventAttack;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_chainedBashCharge;
            public string chainedBashCharge => m_chainedBashCharge;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_chainedBashChargeContactWall;
            public string chainedBashChargeContactWall => m_chainedBashChargeContactWall;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_chainedBashHitboxOff;
            public string chainedBashHitboxOff => m_chainedBashHitboxOff;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_chainedBashHitboxOn;
            public string chainedBashHitboxOn => m_chainedBashHitboxOn;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_chainedBashImpact;
            public string chainedBashImpact => m_chainedBashImpact;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_chainedBashPullSuccess;
            public string chainedBashPullSuccess => m_chainedBashPullSuccess;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_chainedBashRootStart;
            public string chainedBashRootStart => m_chainedBashRootStart;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_chainBashLightningRelease;
            public string chainBashLightningRelease => m_chainBashLightningRelease;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_frankyGustWindReleaseFX;
            public string frankyGustWindReleaseFX => m_frankyGustWindReleaseFX;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_shoulderBashColliderOff;
            public string shoulderBashColliderOff => m_shoulderBashColliderOff;


            public override void Initialize()
            {
#if UNITY_EDITOR
                m_move.SetData(m_skeletonDataAsset);
                m_shoulderBashAttack.SetData(m_skeletonDataAsset);
                m_punchComboAttack.SetData(m_skeletonDataAsset);
                m_chainFistPunchAttack.SetData(m_skeletonDataAsset);
                m_chainFistPunchAttackAnticipation.SetData(m_skeletonDataAsset);
                m_leapAttackStartAnimation.SetData(m_skeletonDataAsset);
                m_chainShockAttack.SetData(m_skeletonDataAsset);
                m_lightningStompAttack.SetData(m_skeletonDataAsset);
                m_stompProjectile.SetData(m_skeletonDataAsset);
                m_runAttack.SetData(m_skeletonDataAsset);
                electricStomp.SetData(m_skeletonDataAsset);
                m_shockRampageAttack.SetData(m_skeletonDataAsset);
                m_phaseDischarge.SetData(m_skeletonDataAsset);
                m_shoulderBashLoopAnimation.SetData(m_skeletonDataAsset);
                m_shoulderBashEndAnimation.SetData(m_skeletonDataAsset);
                m_shoulderBashAnimation.SetData(m_skeletonDataAsset);
                m_punchComboAnimation.SetData(m_skeletonDataAsset);
                m_chainFistPunchUpperAnimation.SetData(m_skeletonDataAsset);
                m_leapAnticipation.SetData(m_skeletonDataAsset);
                m_leapLoopAnimation.SetData(m_skeletonDataAsset);
                m_leapLoopAnimation2.SetData(m_skeletonDataAsset);
                //m_leapTransitionAnimation.SetData(m_skeletonDataAsset);
                m_leapAttackEndAnimation.SetData(m_skeletonDataAsset);
                m_runAttackStartAnimation.SetData(m_skeletonDataAsset);
                m_runAttackAnimation.SetData(m_skeletonDataAsset);
                m_runAttackEndAnimation.SetData(m_skeletonDataAsset);
                m_chainShockLoopAnimation.SetData(m_skeletonDataAsset);
                m_chainShockEndAnimation.SetData(m_skeletonDataAsset);
                m_introAnimation.SetData(m_skeletonDataAsset);
                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_idle2Animation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_roarAnimation.SetData(m_skeletonDataAsset);
                m_hookTravelLoopAnimation.SetData(m_skeletonDataAsset);
                m_hookBackLoopAnimation.SetData(m_skeletonDataAsset);
                m_chainBashNearEdge.SetData(m_skeletonDataAsset);
                m_chainBashMiddle.SetData(m_skeletonDataAsset);
                m_chainBashPullingLoop.SetData(m_skeletonDataAsset);
                m_chainBashPullingOut.SetData(m_skeletonDataAsset);
                m_chainBashIILightningReleaseEnd.SetData(m_skeletonDataAsset);
                m_chainBashIILightningReleaseLoop.SetData(m_skeletonDataAsset);
                m_chainBashIILightningReleaseStart.SetData(m_skeletonDataAsset);

#endif
            }
        }

        [System.Serializable]
        public class PhaseInfo : IPhaseInfo
        {
            [SerializeField]
            private List<float> m_patternCount;
            public List<float> patternCount => m_patternCount;
            [SerializeField]
            private int m_phaseIndex;
            public int phaseIndex => m_phaseIndex;
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
            ChainFist,
            PunchCombo,
            LeapAttack,
            ShoulderBash,
            RunningAttack,
            PhaseDischarge1,
            ChainedBash1,
            PhaseDischarge2,
            ShockRampage,
            ChainedBash2,
            ElectricStomp,
            WaitAttackEnd,
        }

        private enum Attack
        {
            Phase1Pattern1,
            Phase2Pattern1,
            Phase2Pattern2,
            Phase3Pattern1,
            Phase3Pattern2,
            WaitAttackEnd
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
        private GameObject m_spriteMask;
        [SerializeField, TabGroup("Reference")]
        private Animator m_glowAnimFranky;
        /*[SerializeField, TabGroup("Reference")]
        private Collider2D m_aoeBB;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_punchBB;*/
        [SerializeField, TabGroup("Colliders")]
        private Collider2D m_punchLeftComboBB;
        [SerializeField, TabGroup("Colliders")]
        private Collider2D m_punchRightComboBB;
        [SerializeField, TabGroup("Colliders")]
        private Collider2D m_shoulderBashBB;
        [SerializeField, TabGroup("Colliders")]
        private Collider2D m_leapAttackBB;
        [SerializeField, TabGroup("Colliders")]
        private Collider2D m_chainFistBB;
        [SerializeField, TabGroup("Colliders")]
        private Collider2D[] m_chainBashBB;
        [SerializeField, TabGroup("Colliders")]
        private Collider2D m_runningAttackBB;
        [SerializeField, TabGroup("Colliders")]
        private Collider2D m_shockRampageBB;
        [SerializeField, TabGroup("Colliders")]
        private Collider2D m_BodyLightningCollider;
        [SerializeField, TabGroup("Colliders")]
        private Collider2D m_bodyCollider;
        [SerializeField, TabGroup("Colliders")]
        private Collider2D m_bodyColliderForChainBash;
        [SerializeField, TabGroup("EnvironmentColliders")]
        private GameObject[] m_arenaPlayerDetectorColliders;
        /*[SerializeField, TabGroup("Reference")]
        private Collider2D m_punchComboLastHitBB;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_runningAttackBB;
        [SerializeField, TabGroup("Reference")]
        private Collider2D m_shockRampageBB;*/
        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        //[SerializeField, TabGroup("Cinematic")]
        //private PlayableDirector m_director;
        //[SerializeField, TabGroup("Cinematic")]
        //private PlayableAsset m_bossCapsuleIdleCinematic;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Attackers")]
        private AttackData[] m_attackDataDefault;
        [SerializeField, TabGroup("Attackers")]
        private AttackData[] m_attackDataDefault2;
        [SerializeField, TabGroup("Attackers")]
        private AttackData[] m_attackDataBuffed;
        [SerializeField, TabGroup("Attackers")]
        private AttackData[] m_attackDataBuffed2;
        [SerializeField, TabGroup("Attackers")]
        private Attacker[] m_attackerDischarge1;
        [SerializeField, TabGroup("Attackers")]
        private Attacker[] m_attackerDisharge2;
        [SerializeField, TabGroup("Attackers")]
        private Attacker m_punchComboAttacker;
        [SerializeField, TabGroup("Attackers")]
        private Attacker m_chainBashAttacker;
        [SerializeField, TabGroup("Attackers")]
        private Attacker m_punchComboLastHitAttacker;
        [SerializeField, TabGroup("Attackers")]
        private Attacker m_LeapAttackAttacker;
        [SerializeField, TabGroup("Attackers")]
        private Attacker m_chainFistAttacker;
        [SerializeField, TabGroup("Attackers")]
        private Attacker m_chainedBashAttacker;
        [SerializeField, TabGroup("Attackers")]
        private Attacker m_runningAttack;
        [SerializeField, TabGroup("Attackers")]
        private Attacker m_shockRampage;
        [SerializeField, TabGroup("Effects")]
        private GameObject m_wallStickStartFX;
        [SerializeField, TabGroup("Effects")]
        private GameObject m_wallStickStartFXEnvironmentLeft;
        [SerializeField, TabGroup("Effects")]
        private GameObject m_wallStickStartFXEnvironmentRight;
        [SerializeField, TabGroup("Effects")]
        private GameObject m_wallStickStartFXEnvironmentLeftWall;
        [SerializeField, TabGroup("Effects")]
        private GameObject m_wallStickStartFXEnvironmentRightWall;
        [SerializeField, TabGroup("Effects")]
        private GameObject m_wallStickLoopFX;
        [SerializeField, TabGroup("Effects")]
        private GameObject m_wallStickEndFX;
        [SerializeField, TabGroup("Effects")]
        private ParticleSystem m_gustWindVFX;
        [SerializeField, TabGroup("Effects")]
        private GameObject m_leapFX;
        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_orbLightningFX;
        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_stompFX;
        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_bodyLightningFX;
        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_phase3FX;
        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_buffedEffects;
        List<GameObject> m_lightningBoltEffects;
        [SerializeField]
        private SpineEventListener m_spineListener;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        State m_turnState;
        [ShowInInspector]
        private PhaseHandle<Phase, PhaseInfo> m_phaseHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        [SerializeField]
        private Attack m_currentAttack;
        private float m_currentAttackRange;
        private ProjectileLauncher m_projectileLauncher;

        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_headPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_wristPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_wallPosPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_fistPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_fistRefPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_projectilePoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_wallRunPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_CenterOfTheArena;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_chainBashStarting;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_nearEdgePositionLeft;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_nearEdgePositionRight;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_nearEdgePositionLeftRampage;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_nearEdgePositionRightRampage;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_centerPositionLeft;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_centerPositionRight;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_chainBashIK;
        [SerializeField, TabGroup("Chain")]
        private BoxCollider2D m_chainHurtBox;
        [SerializeField, TabGroup("Chain")]
        private BoxCollider2D m_leapHurtBox;

        private int m_currentPhaseIndex;
        private int m_buffedAttackCount;
        private float m_attackCount;
        private float[] m_patternCount;
        private float m_currentLeapDuration;
        private bool m_rangeAttack;
        private bool m_stickToGround;
        private bool m_stickToWall;
        [SerializeField]
        private bool m_isBuffed;
        private bool m_playerIsHitFromPunchCombo;
        private bool m_hasChosenAttack;
        private bool m_hasPhaseChanged;
        private Coroutine m_currentAttackCoroutine;
        private Coroutine m_leapRoutine;
        private int m_attackSpecialAttackLimit;

        public event EventAction<EventActionArgs> PhaseDischargeAction;
        public event EventAction<EventActionArgs> ElectricPushLeft;
        public event EventAction<EventActionArgs> ElectricPushRight;


        private bool m_isDetecting;

        private void ApplyPhaseData(PhaseInfo obj)
        {
            if(m_attackDecider != null)
            {
                UpdateAttackDeciderList();
            }
            base.ApplyData();
        }

        private void ChangeState()
        {
            m_stateHandle.OverrideState(State.Phasing);
            m_hasPhaseChanged = false;
            m_animation.SetEmptyAnimation(0, 0);
            m_phaseHandle.ApplyChange();
            //StartCoroutine(SmartChangePhaseRoutine());
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
                }
            }
        }

        private void PlayerTakenDamge(object sender, Damageable.DamageEventArgs eventArgs)
        {
            m_playerIsHitFromPunchCombo = true;
        }
        private IEnumerator IntroRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_movement.Stop();
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_spriteMask.SetActive(false);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        private IEnumerator ChangePhaseRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            ChangePhaseDeactivator();
            m_movement.Stop();
            m_hitbox.SetInvulnerability(Invulnerability.MAX);  
            m_animation.SetAnimation(0, m_info.roarAnimation, false);
            m_isBuffed = false;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.roarAnimation.animation);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_hasPhaseChanged = false;
            DecidedOnAttack(false);     
            m_buffedEffects.Play();
            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseTwo:
                    yield return PhaseDistarge1();
                    Debug.Log("Phase Change to two");
                    break;
                case Phase.PhaseThree:
                    yield return PhaseDistarge2();
                    Debug.Log("Phase change to three");
                    break;

            }
            m_buffedEffects.Stop();
           
            m_stateHandle.ApplyQueuedState();
        }
        private void LaunchProjectile()
        {
            m_stompFX.Play();
            var target = new Vector2(m_projectilePoint.position.x + (5 * transform.localScale.x), m_projectilePoint.position.y);
            m_projectileLauncher.AimAt(target);
            m_projectileLauncher.LaunchProjectile();
        }

        private IEnumerator StickToGroundRoutine(float groundPoint)
        {
            m_stickToGround = true;
            while (m_stickToGround)
            {
                transform.position = new Vector2(transform.position.x, groundPoint);
                yield return null;
            }
        }
        #region Attacks
        private IEnumerator PunchCombo()
        {
            while (Vector2.Distance(transform.position, m_targetInfo.position) > 15f)
            {
                m_animation.SetAnimation(0, m_info.move.animation, true);
                m_movement.MoveTowards(new Vector2(m_targetInfo.position.x - transform.position.x, 0f).normalized, m_info.walkSpeed);
                if (!IsFacingTarget())
                {
                    CustomTurn();
                }
                yield return null;
            }
            m_animation.SetAnimation(0, m_info.punchComboAttack, false);
            m_targetInfo.GetTargetDamagable().DamageTaken += PlayerDamagedPunchCombo;
            m_punchLeftComboBB.enabled = true;
            m_punchRightComboBB.enabled = true;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.punchComboAttack);
            m_punchLeftComboBB.enabled = false;
            m_punchRightComboBB.enabled = false;
            /*if (!m_hitByPunchCombo)
            {
                m_animation.SetAnimation(0, m_info.idle2Animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.idle2Animation);
            }
            else
            {*/
            yield return ShoulderBash();
            //}
            if (!m_isBuffed)
            {
                m_attackCounter++;
            }
            yield return null;
        }
        private IEnumerator PunchComboPhase3()
        {
            m_animation.SetAnimation(0, m_info.runAttackStartAnimation.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.runAttackStartAnimation);
            while (Vector2.Distance(transform.position, m_targetInfo.position) > 15f)
            {
                m_animation.SetAnimation(0, m_info.runAttackAnimation, true);
                m_movement.MoveTowards(new Vector2(m_targetInfo.position.x - transform.position.x, 0f).normalized, m_info.runAttackSpeed);
                if (!IsFacingTarget())
                {
                    CustomTurn();
                }
                yield return null;
            }
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.runAttackEndAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.runAttackEndAnimation);
            m_animation.SetAnimation(0, m_info.punchComboAttack, false);
            m_targetInfo.GetTargetDamagable().DamageTaken += PlayerDamagedPunchCombo;
            m_punchLeftComboBB.enabled = true;
            m_punchRightComboBB.enabled = true;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.punchComboAttack);
            m_punchLeftComboBB.enabled = false;
            m_punchRightComboBB.enabled = false;
            /*if (!m_hitByPunchCombo)
            {
                m_animation.SetAnimation(0, m_info.idle2Animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.idle2Animation);
            }
            else
            {*/
            yield return ShoulderBashPhase3();
            //}
            if (!m_isBuffed)
            {
                m_attackCounter++;
            }
            yield return null;
        }

        private IEnumerator ChainFist()
        {
            m_animation.SetAnimation(0, m_info.chainFistAttackAnticipation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.chainFistAttackAnticipation);
            m_animation.SetAnimation(0, m_info.chainFistPunchAttack, false);
            m_chainFistBB.enabled = true;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.chainFistPunchAttack);
            m_chainFistBB.enabled = false;
            /*m_animation.SetAnimation(0, m_info.idle2Animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.idle2Animation);*/
            if (!m_isBuffed)
            {
                m_attackCounter++;
            }
            yield return null;
        }
        [Button]
        private void ShoulderBashStart()
        {
            StartCoroutine(ShoulderBash());
        }
        private IEnumerator ShoulderBash()
        {
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            m_animation.SetAnimation(0, m_info.move.animation, true);
            while (Vector2.Distance(transform.position, m_targetInfo.position) > 15f)
            {
                m_movement.MoveTowards(new Vector2(m_targetInfo.position.x - transform.position.x, 0f).normalized, m_info.walkSpeed);
                if (!IsFacingTarget())
                {
                    CustomTurn();
                }
                yield return null;
            }
            //animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.shoulderBashAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.shoulderBashAnimation);
           // DisableShoulderBashCollider();
           //_animation.EnableRootMotion(false, false);
           //_animation.DisableRootMotion();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            yield return new WaitForSeconds(0.3f);
            m_attackCounter++;
        }
        private IEnumerator ShoulderBashPhase3()
        {
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            m_animation.SetAnimation(0, m_info.move.animation, true);
            while (Vector2.Distance(transform.position, m_targetInfo.position) > 15f)
            {

                m_movement.MoveTowards(new Vector2(m_targetInfo.position.x - transform.position.x, 0f).normalized, m_info.walkSpeed);
                yield return null;
            }
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.shoulderBashAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.shoulderBashAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_attackCounter++;
        }
        private void ActivateShoulderBashCollider()
        {
            m_shoulderBashBB.enabled = true;
        }
        private void DisableShoulderBashCollider()
        {
            m_shoulderBashBB.enabled = false;
        }
        private IEnumerator LeapAttack()
        {
            if(!IsFacingTarget()){ CustomTurn(); }
            m_animation.SetAnimation(0, m_info.leapAnticipation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.leapAnticipation);
            yield return new WaitForFixedUpdate();
            if (!IsFacingTarget()) { CustomTurn(); }
            m_animation.SetAnimation(0, m_info.leapAttackStartAnimation, false);
            if(Vector2.Distance(transform.position, m_targetInfo.position) > 20f)
                m_movement.MoveTowards(new Vector2(m_targetInfo.position.x - transform.position.x, 0).normalized, m_info.leapAttackStartAnimation.speed);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.leapAttackStartAnimation);
            m_animation.SetAnimation(0, m_info.idle2Animation, true);
            m_gustWindVFX.Stop();
            yield return new WaitForSeconds(0.5f);
           
            yield return new WaitForFixedUpdate();
            if (!IsFacingTarget()) { CustomTurn(); }
            m_animation.SetAnimation(0, m_info.leapAttackStartAnimation, false);
            if(Vector2.Distance(transform.position, m_targetInfo.position) > 20f)
                m_movement.MoveTowards(new Vector2(m_targetInfo.position.x - transform.position.x, 0).normalized, m_info.leapAttackStartAnimation.speed);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.leapAttackStartAnimation);
            m_animation.SetAnimation(0, m_info.idle2Animation, true);
            m_gustWindVFX.Stop();
            yield return new WaitForSeconds(0.5f);
            yield return new WaitForFixedUpdate();
            if (!IsFacingTarget()) { CustomTurn(); }
            
            m_animation.SetAnimation(0, m_info.leapAttackStartAnimation, false);
            if (Vector2.Distance(transform.position, m_targetInfo.position) > 20f)
                m_movement.MoveTowards(new Vector2(m_targetInfo.position.x - transform.position.x, 0).normalized, m_info.leapAttackStartAnimation.speed);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.leapAttackStartAnimation);
            m_animation.SetAnimation(0, m_info.leapAttackEndAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.leapAttackEndAnimation);
            m_animation.SetAnimation(0, m_info.idle2Animation, true);

            m_leapAttackBB.enabled = false;
            if(m_phaseHandle.currentPhase == Phase.PhaseOne)
            {
                m_attackCounter = 0;
            }
            else
            {
                if (!m_isBuffed)
                {
                    m_attackCounter++;
                }
            }
            yield return null;
        }
        private void ChangePhaseDeactivator()
        {
            //fx
            m_chainHandVFX.Stop();
            m_chainHandVFX2.Stop();
            m_wallImpactVFX.Stop();
            m_gustWindVFX.Stop();
            m_bodyLightningFX.Stop();
            m_phase3FX.Stop();


            //collider
            m_chainFistBB.enabled = false;
            m_leapAttackBB.enabled = false;
            m_punchLeftComboBB.enabled = false;
            m_punchRightComboBB.enabled = false;
            m_runningAttackBB.enabled = false;
            m_shockRampageBB.enabled = false;
            m_shoulderBashBB.enabled = false;
            m_chainBashChargeFistCollider.enabled = false;
            m_BodyLightningCollider.enabled = false;
            m_leapAttackBB.enabled = false;
            for (int i = 0; i < m_chainBashBB.Length; i++)
            {
                m_chainBashBB[i].enabled = false;
            }
        }
        private IEnumerator PhaseDistarge1()
        {
           for (int i = 0; i < m_attackerDischarge1.Length; i++)
            {
                m_attackerDischarge1[i].SetData(m_attackDataBuffed[i]);

            }
            if (!IsFacing(m_CenterOfTheArena.position)) { CustomTurn(); }
            m_animation.SetAnimation(0, m_info.runAttackStartAnimation.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.runAttackStartAnimation);
            while (Vector2.Distance(transform.position, m_CenterOfTheArena.position) > 2f)
            {
                //Debug.Log(Vector2.Distance(transform.position, m_CenterOfTheArena.position));
                if (!IsFacing(m_CenterOfTheArena.position)) { CustomTurn(); }
                m_animation.SetAnimation(0, m_info.runAttackAnimation.animation, true);
                m_movement.MoveTowards(new Vector2(m_CenterOfTheArena.position.x - transform.position.x, 0f).normalized, m_info.runAttackSpeed);
                yield return null;
            }
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.runAttackEndAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.runAttackEndAnimation);
            m_animation.SetAnimation(0, m_info.phaseDischarge, false);
            yield return new WaitForSeconds(0.8f);
            m_bodyLightningFX.Play();
            m_orbLightningFX.Play();
            m_BodyLightningCollider.enabled = true;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.phaseDischarge);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_BodyLightningCollider.enabled = false;
            m_orbLightningFX.Stop();
            m_glowAnimFranky.SetBool("isRaging", true);
            m_attackCounter = 0;
            m_isBuffed = true;
            /*m_animation.SetAnimation(0, m_info.idle2Animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.idle2Animation);*/
            var random = UnityEngine.Random.RandomRange(0, 2);
            if(random == 0)
            {
                if (!IsFacingTarget()) { CustomTurn(); }
                yield return ChainBash();
                yield return PunchCombo();
            }
            else
            {
                if (!IsFacingTarget()) { CustomTurn(); }
                yield return ChainFist();
                yield return PunchCombo();
            }
            for (int i = 0; i < m_attackerDischarge1.Length; i++)
            {
                m_attackerDischarge1[i].SetData(m_attackDataDefault[i]);

            }
            m_glowAnimFranky.SetBool("isRaging", false);
            m_bodyLightningFX.Stop();
            m_isBuffed = false;
            yield return null;
        }
        private IEnumerator PhaseDistarge2()
        {
            for (int i = 0; i < m_attackerDisharge2.Length; i++)
            {
                m_attackerDisharge2[i].SetData(m_attackDataBuffed2[i]);

            }
            m_animation.SetAnimation(0, m_info.runAttackStartAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.runAttackStartAnimation);
            while (Vector2.Distance(transform.position, m_CenterOfTheArena.position) > 2f)
            {
                if (!IsFacing(m_CenterOfTheArena.position)) { CustomTurn(); }
                //Debug.Log(Vector2.Distance(transform.position, m_CenterOfTheArena.position));
                m_animation.SetAnimation(0, m_info.runAttackAnimation.animation, true);
                m_movement.MoveTowards(new Vector2(m_CenterOfTheArena.position.x - transform.position.x, 0f).normalized, m_info.runAttackSpeed);
                yield return null;
            }
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.runAttackEndAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.runAttackEndAnimation);
            m_animation.SetAnimation(0, m_info.phaseDischarge, false);
            yield return new WaitForSeconds(1f);
            PhaseDischargeAction?.Invoke(this, EventActionArgs.Empty);
            m_orbLightningFX.Play();
            m_bodyLightningFX.Play();
            m_BodyLightningCollider.enabled = true; 
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.phaseDischarge);
            m_orbLightningFX.Stop();
            m_BodyLightningCollider.enabled = false;
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_glowAnimFranky.SetBool("isRaging", true);
            
            m_attackCounter = 0;
            m_isBuffed = true;
            /*m_animation.SetAnimation(0, m_info.idle2Animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.idle2Animation);*/
            var random = UnityEngine.Random.RandomRange(0, 3);
            if (random == 0)
            {
                if (!IsFacingTarget()) { CustomTurn(); }
                yield return ChainBashII();
                yield return LeapAttack();
            }
            else if (random == 1)
            {
                if (!IsFacingTarget()) { CustomTurn(); }
                yield return ElectricStomp();
                yield return PunchCombo();
            }
            else
            {
                yield return RunningAttack();
                yield return ShockRampage();
            }
            for (int i = 0; i < m_attackerDisharge2.Length; i++)
            {
                m_attackerDisharge2[i].SetData(m_attackDataDefault2[i]);

            }
            m_glowAnimFranky.SetBool("isRaging", false);
            m_bodyLightningFX.Stop();
            m_isBuffed = false;
            yield return null;
        }
        private GameObject SpawnFX(GameObject fxPrefab, Transform spawnPos, bool flipObject = false)
        {
            Quaternion rotationHandler = fxPrefab.transform.rotation;
            Vector2 spawnObjectPos =  new Vector2(spawnPos.position.x + 4.5f, spawnPos.position.y);
            if (flipObject)
            {
                rotationHandler = Quaternion.Euler(0, 0, -fxPrefab.transform.eulerAngles.z);
                spawnObjectPos = new Vector2(spawnPos.position.x - 4.5f, spawnPos.position.y);
            }
            
            var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(fxPrefab, gameObject.scene);
            instance.SpawnAt(spawnObjectPos, rotationHandler);
            return instance.gameObject;
        }
       
        

        [Button]
        public void ChainBash1Test()
        {
            StartCoroutine(ChainBash());
        }
        [Button]
        public void ChainBash2Test()
        {
            StartCoroutine(ChainBashII());
        }
        private GameObject m_wallImpactStartVFX;
        private Transform m_runTowardsPositionAfterCheck;
        private void ChainBashImpactVFX()
        {
            bool reverseSpawnRotation = false;
            if(m_character.facing == HorizontalDirection.Left)
            {
                reverseSpawnRotation = true;
                m_wallStickStartFXEnvironmentLeft.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                m_wallStickStartFXEnvironmentRight.GetComponent<ParticleSystem>().Play();
            }
                //m_wallImpactStartVFX = SpawnFX(m_wallStickStartFX.gameObject, m_chainBashIK, reverseSpawnRotation);
            


        }
        private void ChainBashOnHitBoxEvent()
        {
            m_chainBashBB[0].enabled = true;
            m_bodyCollider.enabled = false;
        }
        private void ChainBashOffHitBoxEvent()
        {
            m_chainBashBB[0].enabled = false;
            m_bodyColliderForChainBash.enabled = false;
            m_bodyCollider.enabled = true;
        }
        private void ChainBashRootStartEvent()
        {
            m_chainHandVFX.Play();
            m_bodyColliderForChainBash.enabled = true;
            m_animation.EnableRootMotion(true, false);
        }
        private void ChainBashImpactPunch()
        {
            m_chainHandVFX.Stop();
            m_chainHandVFX2.Play();
            if (m_character.facing == HorizontalDirection.Left)
            {
                
                m_wallStickStartFXEnvironmentLeftWall.GetComponent<ParticleSystem>().Play();

            }
            else
            {
                m_wallStickStartFXEnvironmentRightWall.GetComponent<ParticleSystem>().Play();
            }
            m_chainBashChargeFistCollider.enabled = false;
        }
        private void ChainBashCharge()
        {
            m_chainBashChargeFistCollider.enabled = true;
        }   
        
        public void ChainBashDone()
        {
            m_animation.DisableRootMotion();
            m_chainHandVFX2.Stop();
        }

        private void GustWindVFX()
        {
            m_gustWindVFX.Play();
        }
        private void ShockRampageColliderOff()
        {
            m_shockRampageBB.enabled = false;
        }
        private void ShockRampageColliderOn()
        {
            m_shockRampageBB.enabled = true;
        }
        public void ChainBashIILightningReleaseEvent()
        {
            if (m_character.facing == HorizontalDirection.Left)
            {
                ElectricPushLeft?.Invoke(this, EventActionArgs.Empty);
            }
            else
            {
                ElectricPushRight?.Invoke(this, EventActionArgs.Empty);
            }
        }
        private FrankyPlayerDetector.PlayerPosition m_currentPlayerPosition;
        private Transform m_sideToFace;
        IAIAnimationInfo animationNameChain;
        [SerializeField]
        private ParticleSystem m_chainHandVFX;
        [SerializeField]
        private ParticleSystem m_chainHandVFX2;
        [SerializeField]
        Rigidbody2D m_rigidbody2D;
        [SerializeField]
        private Collider2D m_chainBashChargeFistCollider;
        [SerializeField]
        private ParticleSystem m_wallImpactVFX;
        private IEnumerator ChainBash()
        {
           // m_spineListener.Subscribe(m_info.chainedBashChargeContactWall, ChainBashImpactVFX);
            FrankyPlayerDetector.OnPlayerEnteredArea += FrankyPlayerDetector_OnPlayerEnteredArea;
            yield return new WaitForSeconds(0.3f);
           
            for (int i = 0; i < m_arenaPlayerDetectorColliders.Length; i++)
            {
                m_arenaPlayerDetectorColliders[i].gameObject.SetActive(true);
            }
            yield return new WaitForFixedUpdate();
            
            var randomNumber = UnityEngine.Random.Range(0, 2);
            Debug.Log(randomNumber.ToString());
            Debug.Log(m_currentPlayerPosition);         
            switch (m_currentPlayerPosition)
            {
                case FrankyPlayerDetector.PlayerPosition.Left:
                    var randomPositionLeft = randomNumber == 0 ? m_centerPositionRight : m_nearEdgePositionRight;
                    animationNameChain = randomNumber == 0 ? m_info.chainBashMiddle : m_info.chainBashNearEdge;
                    m_runTowardsPositionAfterCheck = randomPositionLeft;      
                    m_sideToFace = m_arenaLeftSide;
                    Debug.Log(randomPositionLeft.ToString());
                    Debug.Log(animationNameChain);
                    break;
                case FrankyPlayerDetector.PlayerPosition.Right:
                    var randomPositionRight = randomNumber == 0 ? m_centerPositionLeft : m_nearEdgePositionLeft;
                    animationNameChain = randomNumber == 0 ? m_info.chainBashMiddle : m_info.chainBashNearEdge;
                    m_runTowardsPositionAfterCheck = randomPositionRight;
                    m_sideToFace = m_arenaRightSide;
                    Debug.Log(randomPositionRight.ToString());
                    Debug.Log(animationNameChain);
                    break;
            }
            FrankyPlayerDetector.OnPlayerEnteredArea -= FrankyPlayerDetector_OnPlayerEnteredArea;
            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < m_arenaPlayerDetectorColliders.Length; i++)
            {
                m_arenaPlayerDetectorColliders[i].gameObject.SetActive(false);
            }
            Debug.Log(m_currentPlayerPosition);
            if (!IsFacing(m_runTowardsPositionAfterCheck.position)) { CustomTurn(); }

            
            m_animation.SetAnimation(0, m_info.runAttackStartAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.runAttackStartAnimation);
            while (Vector2.Distance(transform.position, m_runTowardsPositionAfterCheck.position) > 2f)
            {
                m_animation.SetAnimation(0,m_info.runAttackAnimation.animation,true);
                Vector2 direction = new Vector2( m_runTowardsPositionAfterCheck.position.x - transform.position.x,0f ).normalized;
                transform.position +=(Vector3)(direction * m_info.move.speed * 1f * Time.deltaTime);
                yield return null;
            }
            // snap exactly to target
 
            yield return new WaitForFixedUpdate();
            m_animation.SetAnimation(0, m_info.runAttackEndAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.runAttackEndAnimation);
            transform.position = new Vector2(m_runTowardsPositionAfterCheck.position.x, transform.position.y);
            if (!IsFacing(m_sideToFace.position)) { CustomTurn(); }
            m_animation.EnableRootMotion(true, false);
            yield return new WaitForFixedUpdate();
            m_animation.SetAnimation(0, animationNameChain, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, animationNameChain);
            yield return new WaitForFixedUpdate();
            m_animation.SetAnimation(0, m_info.chainBashPullingLoop, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.chainBashPullingLoop);
            m_animation.SetAnimation(0, m_info.chainBashPullingOut, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.chainBashPullingOut);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            #region old attack
            //m_puchWallLoopFX.Play();
            //m_animation.SetAnimation(0, m_info.chainBash1AnimationEnd.animation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.chainBash1AnimationEnd.animation);
            //m_puchWallEndFX.Play();
            //m_punchBoneRayCast.gameObject.SetActive(true);
            //m_animation.SetAnimation(0, m_info.chainBash1AnimationStart.animation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.chainBash1AnimationStart.animation);
            //m_chainBashBB[0].enabled = true;




            //Debug.Log("before");

            ////RaycastHit2D hit = Physics2D.Raycast(m_chainBashStarting.position, Vector2.right * transform.localScale.x, 1000, DChildUtility.GetEnvironmentMask());
            ///* Debug.Log("After");

            // fistBone.enabled = true;
            // fistBone.mode = SkeletonUtilityBone.Mode.Override;
            // Vector2 targetPos = hit.point;
            // m_fistPoint.position = targetPos;
            // m_wallPosPoint.position = new Vector2(targetPos.x, targetPos.y);
            // m_wallPosPoint.localScale = m_character.facing == HorizontalDirection.Right? new Vector3(2, 2, 2) : new Vector3(-2, 2, 2);
            // m_wallPosPoint.gameObject.SetActive(true);
            // Vector2 fxPos = new Vector2(targetPos.x *//*+ (8* transform.localScale.x)*//*, targetPos.y);
            // var startFX = SpawnFX(m_wallStickStartFX, fxPos);
            // yield return new WaitForSeconds(1f);
            // startFX.SetActive(false);
            // var loopFX = SpawnFX(m_wallStickLoopFX, fxPos);
            // yield return new WaitForSeconds(0.5f);*/
            //m_animation.SetAnimation(0, m_info.chainBash1AnimationLoop, true);
            //yield return new WaitForSeconds(0.1f);
            //while (!m_punchBoneRayCast.allRaysDetecting)
            //{
            //    float direction = Mathf.Sign(m_punchMoveBone.transform.lossyScale.x);
            //    m_punchMoveBone.transform.position +=Vector3.right * direction * m_info.punchSpeed * Time.deltaTime;
            //    Debug.Log("looping bone");
            //    yield return null;
            //}
            //m_puchWallLoopFX.Play();
            //Debug.Log("after");
            //m_chainBashBB[1].enabled = true;
            //while (!m_bodyRaySensor.allRaysDetecting)
            //{
            //    m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.shoulderBashReelSpeed * Time.deltaTime);
            //    yield return new WaitForSeconds(1f);
            //    yield return null;
            //}
            //////Destroy(loopFX.gameObject);
            //////loopFX.gameObject.SetActive(false); 
            ////m_wallPosPoint.gameObject.SetActive(false);
            ////m_chainBashBB[0].enabled = false;
            ////m_chainBashBB[1].enabled = false;
            ////var bashLoopAnim = m_animation.SetAnimation(0, m_info.chainBash1AnimationLoop, true);
            ////yield return new WaitForSeconds(m_info.ChainBashDuration);
            ////yield return new WaitForSpineAnimationComplete(bashLoopAnim);
            ////m_animation.SetAnimation(0, m_info.chainBash1AnimationEnd, false);
            ////yield return new WaitForAnimationComplete(m_animation.animationState, m_info.chainBash1AnimationEnd);
            ////m_fistPoint.GetComponent<SkeletonUtilityBone>().mode = SkeletonUtilityBone.Mode.Follow;
            ////m_fistPoint.GetComponent<SkeletonUtilityBone>().enabled = false;
            ////m_wallPosPoint.localPosition = Vector2.zero;
            ///*m_animation.SetAnimation(0, m_info.idleAnimation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.idleAnimation);*/
            //yield return new WaitForSeconds(10f);// for testing only 
            //CustomTurn();\

            #endregion
        }

        private void FrankyPlayerDetector_OnPlayerEnteredArea(FrankyPlayerDetector.PlayerPosition playerPos)
        {
            m_currentPlayerPosition = playerPos;
           
            Debug.Log("Player pos: "+ playerPos.ToString());
        }

        [SerializeField]
        private Transform m_arenaLeftSide;
        [SerializeField]
        private Transform m_arenaRightSide;
        private IEnumerator RunningAttack()
        {
            var runTowards = m_character.facing == HorizontalDirection.Left? m_arenaLeftSide : m_arenaRightSide;
            if(Vector2.Distance(transform.position, runTowards.position) < 10)
            {
                CustomTurn();
                runTowards = m_character.facing == HorizontalDirection.Left? m_arenaLeftSide : m_arenaRightSide;
            }
            m_runningAttackBB.enabled = true;
            m_animation.SetAnimation(0, m_info.runAttackStartAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.runAttackStartAnimation);
            m_animation.SetAnimation(0, m_info.runAttackAnimation, true);
            while(Vector2.Distance(transform.position, runTowards.position) > 15f)
            {
                m_movement.MoveTowards(new Vector2(runTowards.position.x - transform.position.x, 0f).normalized, m_info.walkSpeed * 5);
                yield return null;
            }
            m_movement.MoveTowards(new Vector2(runTowards.position.x - transform.position.x, 0f).normalized, m_info.walkSpeed * 1);
            m_animation.SetAnimation(0, m_info.runAttackEndAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.runAttackEndAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_movement.Stop();
            m_runningAttackBB.enabled = false;
            /*if (m_phaseHandle.currentPhase == Phase.PhaseThree)
            {
                if (m_isBuffed)
                {
                    yield return ShockRampage();
                }
            }*/

            if (!IsFacingTarget()) { CustomTurn(); }
            m_attackCounter++;
            yield return null;
        }
        [Button]
        private void ShockRampageTest()
        {
            StartCoroutine(ShockRampage());
        }

        private IEnumerator ShockRampage()
        {
            m_spineListener.Subscribe(m_info.shockRampageColliderEventOn, ShockRampageColliderOn);
            m_spineListener.Subscribe(m_info.shockRampageColliderEventOff, ShockRampageColliderOff);
            m_bodyCollider.enabled = false;
            FrankyPlayerDetector.OnPlayerEnteredArea += FrankyPlayerDetector_OnPlayerEnteredArea;
            yield return new WaitForSeconds(0.3f);

            for (int i = 0; i < m_arenaPlayerDetectorColliders.Length; i++)
            {
                m_arenaPlayerDetectorColliders[i].gameObject.SetActive(true);
            }
            yield return new WaitForFixedUpdate();

            switch (m_currentPlayerPosition)
            {
                case FrankyPlayerDetector.PlayerPosition.Left:
                    var PositionRight = m_nearEdgePositionRightRampage;
                    m_runTowardsPositionAfterCheck = PositionRight;
                    m_sideToFace = m_arenaLeftSide;
                    break;
                case FrankyPlayerDetector.PlayerPosition.Right:
                    var PositionLeft = m_nearEdgePositionLeftRampage;
                    m_runTowardsPositionAfterCheck = PositionLeft;
                    m_sideToFace = m_arenaRightSide;
                    break;
            }
            FrankyPlayerDetector.OnPlayerEnteredArea -= FrankyPlayerDetector_OnPlayerEnteredArea;
            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < m_arenaPlayerDetectorColliders.Length; i++)
            {
                m_arenaPlayerDetectorColliders[i].gameObject.SetActive(false);
            }
            Debug.Log(m_currentPlayerPosition);
            if (!IsFacing(m_runTowardsPositionAfterCheck.position)) { CustomTurn(); }
            m_animation.SetAnimation(0, m_info.runAttackStartAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.runAttackStartAnimation);
            while (Vector2.Distance(transform.position, m_runTowardsPositionAfterCheck.position) > 2f)
            {
                m_animation.SetAnimation(0, m_info.runAttackAnimation.animation, true);
                Vector2 direction = new Vector2(m_runTowardsPositionAfterCheck.position.x - transform.position.x, 0).normalized;
                transform.position += (Vector3)(direction * m_info.move.speed * 1f * Time.deltaTime);
                yield return null;
            }
            if (!IsFacing(m_sideToFace.position)) { CustomTurn(); }

            m_animation.SetAnimation(0, m_info.shockRampageAttack, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.shockRampageAttack);
            m_bodyCollider.enabled = true;

        }
        private IEnumerator ChainBashII()
        {
           // m_spineListener.Subscribe(m_info.chainedBashChargeContactWall, ChainBashImpactVFX);
            FrankyPlayerDetector.OnPlayerEnteredArea += FrankyPlayerDetector_OnPlayerEnteredArea;
            yield return new WaitForSeconds(0.3f);

            for (int i = 0; i < m_arenaPlayerDetectorColliders.Length; i++)
            {
                m_arenaPlayerDetectorColliders[i].gameObject.SetActive(true);
            }
            yield return new WaitForFixedUpdate();
            m_chainBashBB[0].enabled = true;
            var randomNumber = UnityEngine.Random.Range(0, 2);
            Debug.Log(randomNumber.ToString());
            Debug.Log(m_currentPlayerPosition);
            switch (m_currentPlayerPosition)
            {
                case FrankyPlayerDetector.PlayerPosition.Left:
                    var randomPositionLeft = randomNumber == 0 ? m_centerPositionRight : m_nearEdgePositionRight;
                    animationNameChain = randomNumber == 0 ? m_info.chainBashMiddle : m_info.chainBashNearEdge;
                    m_runTowardsPositionAfterCheck = randomPositionLeft;
                    m_sideToFace = m_arenaLeftSide;
                    Debug.Log(randomPositionLeft.ToString());
                    Debug.Log(animationNameChain.ToString());
                    break;
                case FrankyPlayerDetector.PlayerPosition.Right:
                    var randomPositionRight = randomNumber == 0 ? m_centerPositionLeft : m_nearEdgePositionLeft;
                    animationNameChain = randomNumber == 0 ? m_info.chainBashMiddle : m_info.chainBashNearEdge;
                    m_runTowardsPositionAfterCheck = randomPositionRight;
                    m_sideToFace = m_arenaRightSide;
                    Debug.Log(randomPositionRight.ToString());
                    Debug.Log(animationNameChain.ToString());
                    break;
            }
            FrankyPlayerDetector.OnPlayerEnteredArea -= FrankyPlayerDetector_OnPlayerEnteredArea;
            yield return new WaitForSeconds(0.2f);
            for (int i = 0; i < m_arenaPlayerDetectorColliders.Length; i++)
            {
                m_arenaPlayerDetectorColliders[i].gameObject.SetActive(false);
            }
            Debug.Log(m_currentPlayerPosition);
            if (!IsFacing(m_runTowardsPositionAfterCheck.position)) { CustomTurn(); }


            m_animation.SetAnimation(0, m_info.runAttackStartAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.runAttackStartAnimation);
            while (Vector2.Distance(transform.position, m_runTowardsPositionAfterCheck.position) > 2f)
            {
                m_animation.SetAnimation(0,m_info.runAttackAnimation.animation,true);
                Vector2 direction = new Vector2(m_runTowardsPositionAfterCheck.position.x - transform.position.x,0f).normalized;
                transform.position +=(Vector3)(direction * m_info.move.speed * 1f * Time.deltaTime);
                yield return null;
            }

            // snap exactly to target

            yield return new WaitForFixedUpdate();
            m_animation.SetAnimation(0, m_info.runAttackEndAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.runAttackEndAnimation);
            m_animation.EnableRootMotion(true, false); 
            if (!IsFacing(m_sideToFace.position)) { CustomTurn(); }
            transform.position = new Vector2(m_runTowardsPositionAfterCheck.position.x,transform.position.y);
            yield return new WaitForFixedUpdate();
            m_animation.SetAnimation(0, animationNameChain, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, animationNameChain);     
            m_animation.SetAnimation(0, m_info.chainBashIILightningReleaseStart, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.chainBashIILightningReleaseStart);
            m_animation.SetAnimation(0, m_info.chainBashIILightningReleaseLoop, true);
            yield return new WaitForSeconds(3f);
            m_animation.SetAnimation(0, m_info.chainBashIILightningReleaseEnd, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.chainBashIILightningReleaseEnd);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
        }
       
        private IEnumerator ElectricStomp()
        {
            if (!IsFacingTarget()) { CustomTurn(); }
            m_animation.SetAnimation(0, m_info.electricStomp, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.electricStomp);
            if (!m_isBuffed)
            {
                m_attackCounter++;
            }
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            yield return null;
        }
        #endregion
        #region Patterns
        private IEnumerator Phase1Pattern1Routine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            if(m_attackCounter >= 5)
            {
                yield return LeapAttack();
            }
            while(Vector2.Distance(transform.position, m_targetInfo.position) > 15f)
            {
                m_animation.SetAnimation(0, m_info.move.animation, true);
                m_movement.MoveTowards(new Vector2(m_targetInfo.position.x - transform.position.x, 0f).normalized, m_info.walkSpeed);
                if (!IsFacingTarget())
                {
                    CustomTurn();
                }
                yield return null;
            }
            m_movement.Stop();
            var random = UnityEngine.Random.RandomRange(0, 3);
            if(random == 0)
            {
                if (!IsFacingTarget()){ CustomTurn();}
                yield return PunchCombo();
            }
            else if(random == 2)
            {
                if (!IsFacingTarget()) { CustomTurn(); }
                yield return ChainFist();
            }
            else
            {
                if (!IsFacingTarget()) { CustomTurn(); }
                yield return ShoulderBash();
            }
            //m_animation.SetAnimation(0, m_info.idleAnimation, true);
            DecidedOnAttack(false);
            //animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
        }
        private IEnumerator Phase2Pattern1Routine()
        {
            yield return PhaseDistarge1(); 
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
        }
        private IEnumerator Phase2Pattern2Routine()
        {
            if (m_attackCounter >= 10)
            {
                yield return PhaseDistarge1();
            }
            else
            {
                if (Vector2.Distance(transform.position, m_targetInfo.position) > 20f)
                {
                    var random = UnityEngine.Random.RandomRange(0, 2);
                    if (random == 0)
                    {
                        if (!IsFacingTarget()) { CustomTurn(); }
                        yield return RunningAttack();
                    }
                    else
                    {
                        yield return ChainBash();
                    }
                }
                else
                {
                    var random = UnityEngine.Random.RandomRange(0, 3);
                    if (random == 0)
                    {
                        yield return PunchCombo();
                    }
                    else if (random == 1)
                    {
                        yield return ChainFist();
                    }
                    else
                    {
                        yield return LeapAttack();
                    }
                }
                yield return null;
            }
            //m_animation.SetAnimation(0, m_info.idleAnimation, true);
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        private IEnumerator Phase3Pattern1Routine()
        {
            yield return PhaseDistarge2();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        private IEnumerator Phase3Pattern2Routine()
        {
            if(m_attackCounter >= 12)
            {
                yield return PhaseDistarge2();
                yield return null;
            }
            else
            {
                if (Vector2.Distance(transform.position, m_targetInfo.position) > 20f)
                {
                    var random = UnityEngine.Random.RandomRange(0, 3);
                    if (random == 0)
                    {
                        if (!IsFacingTarget()) { CustomTurn(); }
                        yield return RunningAttack();
                    }
                    else if( random == 1)
                    {
                        yield return ChainBashII();
                    }
                    else
                    {
                        yield return ElectricStomp();
                    }
                }
                else
                {
                    var random = UnityEngine.Random.RandomRange(0, 4);
                    if (random == 0)
                    {
                        if (!IsFacingTarget()) { CustomTurn(); }
                        yield return PunchComboPhase3();
                    }
                    else if (random == 1)
                    {
                        if (!IsFacingTarget()) { CustomTurn(); }
                        yield return ChainFist();
                    }
                    else if (random == 2)
                    {
                        yield return LeapAttack();
                    }
                    else
                    {
                        yield return ElectricStomp();
                    }
                }
            }
            //m_animation.SetAnimation(0, m_info.idleAnimation, true);
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        #endregion

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            StopAllCoroutines();
            m_bodyLightningFX.Stop();
            m_chainHandVFX.Stop();
            m_chainHandVFX2.Stop();
            m_gustWindVFX.Stop();
            m_wallStickLoopFX.GetComponent<FX>().Stop();
            m_phase3FX.Stop();
            StartCoroutine(StickToGroundRoutine(GroundPosition().y));
            for (int i = 0; i < m_lightningBoltEffects.Count; i++)
            {
                Destroy(m_lightningBoltEffects[i]);
            }
            m_lightningBoltEffects.Clear();
            m_chainHurtBox.gameObject.SetActive(false);
            //m_deathFX.Play();
            m_movement.Stop();
            m_isDetecting = false;
        }
        private void DecidedOnAttack(bool condition)
        {
            m_attackDecider.hasDecidedOnAttack = condition;
        }

        private void UpdateAttackDeciderList()
        {
            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseOne:
                    m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Phase1Pattern1, m_info.phase1Pattern1Range));
                    break;
                case Phase.PhaseTwo:
                    m_attackDecider.SetList(/*new AttackInfo<Attack>(Attack.Phase2Pattern1, m_info.phase2Pattern1Range),*/
                        (new AttackInfo<Attack>(Attack.Phase2Pattern2, m_info.phase2Pattern2Range)));
                    break;
                case Phase.PhaseThree:
                    m_attackDecider.SetList(/*new AttackInfo<Attack>(*//*Attack.Phase3Pattern1, m_info.phase3Pattern1Range),*/
                        (new AttackInfo<Attack>(Attack.Phase3Pattern2, m_info.phase3Pattern2Range)));
                    break;
            }
            DecidedOnAttack(false);
        }

        public override void ApplyData()
        {
            if (m_attackDecider != null)
            {
                UpdateAttackDeciderList();
            }
            base.ApplyData();
        }

        private Vector2 WallPosition()
        {
            var wristPoint = new Vector2(m_wristPoint.position.x, m_wristPoint.position.y + 2f);
            RaycastHit2D hit = Physics2D.Raycast(wristPoint, Vector2.right * transform.localScale.x, 1000, DChildUtility.GetEnvironmentMask());
            return hit.point;
        }

        private Vector2 GroundPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1000, DChildUtility.GetEnvironmentMask());
            return hit.point;
        }

        protected override void Awake()
        {
            base.Awake();
            //m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            m_projectileLauncher = new ProjectileLauncher(m_info.stompProjectile.projectileInfo, m_projectilePoint);
            //m_patternDecider = new RandomAttackDecider<Pattern>();
            m_lightningBoltEffects = new List<GameObject>();
            m_attackDecider = new RandomAttackDecider<Attack>();
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            var asdas = m_chainBashAttacker.GetBaseDamage().value;
            Debug.Log(asdas);
            UpdateAttackDeciderList();
        }

        private void PhaseFX()
        {
            //m_aoeBB.enabled = true;
            m_orbLightningFX.Play();
            m_bodyLightningFX.Play();
            if (m_currentPhaseIndex == 3)
            {
                m_phase3FX.Play();
            }
        }

        private void PhaseFXStop()
        {

            //m_aoeBB.enabled = false;
            m_orbLightningFX.Stop();
        }
        private IEnumerator LeapColliderController()
        {
            m_leapAttackBB.enabled = true;
            yield return new WaitForSeconds(0.4f);
            m_leapAttackBB.enabled = false;
        }

        private void LeapEvent()
        {
            //if (!IsFacingTarget()) { CustomTurn(); }
            var fxPool = GameSystem.poolManager.GetPool<FXPool>().GetOrCreateItem(m_leapFX);
            fxPool.Play();
            StartCoroutine(LeapColliderController());
            fxPool.transform.position = new Vector2(transform.position.x + (23f * transform.localScale.x), transform.position.y - 1.5f);
        }
        [SerializeField]
        private int m_attackCounter;
        private bool m_hitByPunchCombo;
        private void PlayerDamagedPunchCombo(object sender, Damageable.DamageEventArgs eventArgs)
        {
            m_hitByPunchCombo = true;
           // m_attackCounter++;
        }
        protected override void Start()
        {
            base.Start();
            m_spineListener.Subscribe(m_info.phaseEvent, PhaseFX);
            m_spineListener.Subscribe(m_info.leapEvent, LeapEvent);
            m_spineListener.Subscribe(m_info.stopRoarEvent, PhaseFXStop);
            m_spineListener.Subscribe(m_info.stompEvent, LaunchProjectile);
            m_spineListener.Subscribe(m_info.shoulderBashEventAttack, ActivateShoulderBashCollider);
            m_spineListener.Subscribe(m_info.chainedBashCharge, ChainBashCharge);
            m_spineListener.Subscribe(m_info.chainedBashChargeContactWall, ChainBashImpactVFX);
            m_spineListener.Subscribe(m_info.chainedBashHitboxOff, ChainBashOffHitBoxEvent);
            m_spineListener.Subscribe(m_info.chainedBashHitboxOn, ChainBashOnHitBoxEvent);
            m_spineListener.Subscribe(m_info.chainedBashImpact, ChainBashImpactPunch);
            m_spineListener.Subscribe(m_info.chainedBashPullSuccess, ChainBashDone);
            m_spineListener.Subscribe(m_info.chainedBashRootStart, ChainBashRootStartEvent);
            m_spineListener.Subscribe(m_info.chainBashLightningRelease, ChainBashIILightningReleaseEvent);
            m_spineListener.Subscribe(m_info.shockRampageColliderEventOn, ShockRampageColliderOn);
            m_spineListener.Subscribe(m_info.shockRampageColliderEventOff, ShockRampageColliderOff);
            m_spineListener.Subscribe(m_info.frankyGustWindReleaseFX, GustWindVFX);
            m_spineListener.Subscribe(m_info.shoulderBashColliderOff, DisableShoulderBashCollider);

            //m_spineListener.Subscribe(m_info.aimChainBash, AimChainBash);
            // m_animation.DisableRootMotion();
            m_fistRefPoint.GetComponent<CircleCollider2D>().enabled = false;
            m_phaseHandle = new PhaseHandle<Phase, PhaseInfo>();
            m_phaseHandle.Initialize(Phase.PhaseOne, m_info.phaseInfo, m_character, ChangeState, ApplyPhaseData);
            m_phaseHandle.ApplyChange();
        }
        [SerializeField]
        private Material m_tommi;
        [SerializeField]
        private float m_glowBrightness;
        private float healthy = 0.5f;
        private void OnOrOffDamagetModifier(AttackData[] currentDamage, float percentage)
        {
           // var asdda = m_chainedBashAttacker.
            for (int i = 0; i < currentDamage.Length; i++)
            {
                m_chainBashAttacker.SetData(currentDamage[i]);
                var value = currentDamage[i].info.damage.value;
                Debug.Log(value);
                var adsaddsa = m_chainedBashAttacker.GetBaseDamage();
                adsaddsa.value = value;
                m_chainedBashAttacker.SetDamage(adsaddsa);
            }
        }
        private void Update()
        {
            //if (m_isBuffed)
            //{
            //    m_tommi.SetFloat("_Color_Brightness", m_glowBrightness);
            //    m_buffedEffects.Play();
            //   // StartCoroutine(OnOrOffDamagetModifier(1.1f));
            //}
            //else
            //{
            //    m_tommi.SetFloat("_Color_Brightness", healthy);
            //    m_buffedEffects.Stop();
            //  //  StartCoroutine(OnOrOffDamagetModifier(1f));
            //}
            m_phaseHandle.MonitorPhase();
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    break;
                case State.Intro:
                    if (!IsFacingTarget()) { CustomTurn(); }
                    StartCoroutine(IntroRoutine());
                    break;
                case State.Phasing:
                    StopAllCoroutines();
                    m_animation.DisableRootMotion();
                    m_movement.Stop();
                    StartCoroutine(ChangePhaseRoutine());
                    break;
                case State.Turning:
                    Debug.Log("Turning");
                    m_phaseHandle.allowPhaseChange = false;
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idleAnimation.animation);

                    m_movement.Stop();
                    break;
                case State.Attacking:
                    m_hitbox.SetInvulnerability(Invulnerability.None);
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    if(m_attackDecider.hasDecidedOnAttack == false)
                    {
                        m_attackDecider.DecideOnAttack();
                    }
                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.Phase1Pattern1:
                            StartCoroutine(Phase1Pattern1Routine());
                            break;
                        case Attack.Phase2Pattern1:
                            StartCoroutine(Phase2Pattern1Routine());
                            break;
                        case Attack.Phase2Pattern2:
                            StartCoroutine(Phase2Pattern2Routine());
                            break;
                        case Attack.Phase3Pattern1:
                            StartCoroutine(Phase3Pattern1Routine());
                            break;
                        case Attack.Phase3Pattern2:
                            StartCoroutine(Phase3Pattern2Routine());
                            break;
                    }
                    break;

                case State.Chasing:
                    m_stateHandle.SetState(State.Attacking);
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
            m_stickToGround = false;
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