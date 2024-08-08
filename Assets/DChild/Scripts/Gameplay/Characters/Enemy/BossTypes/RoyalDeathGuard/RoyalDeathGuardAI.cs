using DChild.Gameplay.Combat;
using Holysoft.Event;
using DChild.Gameplay.Characters.AI;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;
using DG.Tweening;
using DChild.Gameplay.Environment;
using UnityEngine.UIElements;
using System.Drawing.Text;
using DChild.Gameplay.Pooling;
using Doozy.Runtime.Reactor.Reactions;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Boss/RoyalDeathGuard")]
    public class RoyalDeathGuardAI : CombatAIBrain<RoyalDeathGuardAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField]
            private RoyalDeathGuardScytheProjectile.FlightInfo[] m_scytheThrowPatterns;
            public RoyalDeathGuardScytheProjectile.FlightInfo[] scytheThrowPatterns => m_scytheThrowPatterns;

            [SerializeField]
            private PhaseInfo<Phase> m_phaseInfo;
            public PhaseInfo<Phase> phaseInfo => m_phaseInfo;

            [SerializeField, BoxGroup("Consecutive Hits")]
            private Dictionary<int, float> m_consecutiveHitsToFlinchChancePair;
            public float GetFlinchChance(int hitCount) => m_consecutiveHitsToFlinchChancePair.TryGetValue(hitCount, out float value) ? value : 0;
            [SerializeField, BoxGroup("Consecutive Hits")]
            private float m_consecutiveHitInterval;
            public float consecutiveHitInterval => m_consecutiveHitInterval;
            [SerializeField, BoxGroup("Consecutive Hits")]
            private int m_maxConsecutiveHits;
            public float maxConsecutiveHits => m_maxConsecutiveHits;

            [SerializeField, TabGroup("Rage Quake")]
            private BasicAnimationInfo m_rageQuakeAnimation;
            public BasicAnimationInfo rageQuakeAnimation => m_rageQuakeAnimation;

            [SerializeField, BoxGroup("Movement")]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;

            [Title("Attack Behaviours")]
            [SerializeField, TabGroup("Attacks1", "Scythe Throw")]
            private SimpleAttackInfo m_scytheThrowAttack = new SimpleAttackInfo();
            public SimpleAttackInfo scytheThrowAttack => m_scytheThrowAttack;
            [SerializeField, TabGroup("Attacks1", "Scythe Throw")]
            private BasicAnimationInfo m_scytheThrowAnticipation;
            public BasicAnimationInfo scytheThrowAnticipation => m_scytheThrowAnticipation;
            [SerializeField, TabGroup("Attacks1", "Scythe Throw")]
            private BasicAnimationInfo m_scytheThrowThrow;
            public BasicAnimationInfo scytheThrowThrow => m_scytheThrowThrow;
            [SerializeField, TabGroup("Attacks1", "Scythe Throw")]
            private BasicAnimationInfo m_scytheThrowWaitForScythe;
            public BasicAnimationInfo scytheThrowWaitForScythe => m_scytheThrowWaitForScythe;
            [SerializeField, TabGroup("Attacks1", "Scythe Throw")]
            private BasicAnimationInfo m_scytheThrowCatch;
            public BasicAnimationInfo scytheThrowCatch => m_scytheThrowCatch;

            [SerializeField, TabGroup("Attacks1", "Scythe Swipe")]
            private SimpleAttackInfo m_scytheSwipeAttack = new SimpleAttackInfo();
            public SimpleAttackInfo scytheSwipeAttack => m_scytheSwipeAttack;
            [SerializeField, TabGroup("Attacks1", "Scythe Swipe")]
            private BasicAnimationInfo m_scytheSwipeAnticipation;
            public BasicAnimationInfo scytheSwipeAnticipation => m_scytheSwipeAnticipation;
            [SerializeField, TabGroup("Attacks1", "Scythe Swipe")]
            private AttackData m_scytheSwipeAttackData;
            public AttackData scytheSwipeAttackData => m_scytheSwipeAttackData;

            [SerializeField, TabGroup("Attacks1", "Scythe Smash")]
            private SimpleAttackInfo m_scytheSmashAttack = new SimpleAttackInfo();
            public SimpleAttackInfo scytheSmashAttack => m_scytheSmashAttack;
            [SerializeField, TabGroup("Attacks1", "Scythe Smash")]
            private BasicAnimationInfo m_scytheSmashAnticipation;
            public BasicAnimationInfo scytheSmashAnticipation => m_scytheSmashAnticipation;
            [SerializeField, TabGroup("Attacks1", "Scythe Smash")]
            private BasicAnimationInfo m_scytheSmashGroundLoop;
            public BasicAnimationInfo scytheSmashGroundLoop => m_scytheSmashGroundLoop;
            [SerializeField, TabGroup("Attacks1", "Scythe Smash")]
            private BasicAnimationInfo m_scytheSmashRemoveScytheFromGround;
            public BasicAnimationInfo scytheSmashRemoveScytheFromGround => m_scytheSmashRemoveScytheFromGround;
            [SerializeField, TabGroup("Attacks1", "Scythe Smash")]
            private AttackData m_scytheSmashAttackData;
            public AttackData scytheSmashAttackData => m_scytheSmashAttackData;

            [SerializeField, TabGroup("Attacks2", "Royal Guardian")]
            private BasicAnimationInfo m_royalGuardianShieldSummon = new SimpleAttackInfo();
            public BasicAnimationInfo royalGuardianShieldSummon => m_royalGuardianShieldSummon;
            [SerializeField, TabGroup("Attacks2", "Royal Guardian")]
            private BasicAnimationInfo m_royalGuardianAnticipation;
            public BasicAnimationInfo royalGuardianAnticipation => m_royalGuardianAnticipation;
            [SerializeField, TabGroup("Attacks2", "Royal Guardian")]
            private BasicAnimationInfo m_royalGuardianShieldSlam;
            public BasicAnimationInfo royalGuardianShieldSlam => m_royalGuardianShieldSlam;

            [SerializeField, TabGroup("Attacks2", "Scythe Swipe Two")]
            private SimpleAttackInfo m_scytheSwipeTwoAttack = new SimpleAttackInfo();
            public SimpleAttackInfo scytheSwipeTwoAttack => m_scytheSwipeTwoAttack;
            [SerializeField, TabGroup("Attacks2", "Scythe Swipe Two")]
            private BasicAnimationInfo m_scytheSwipeTwoAnticipation;
            public BasicAnimationInfo scytheSwipeTwoAnticipation => m_scytheSwipeTwoAnticipation;
            [SerializeField, TabGroup("Attacks2", "Scythe Swipe Two")]
            private AttackData m_scytheSwipeTwoAttackData;
            public AttackData scytheSwipeTwoAttackData => m_scytheSwipeTwoAttackData;

            [SerializeField, TabGroup("Attacks2", "Harvest")]
            private SimpleAttackInfo m_harvestAttack = new SimpleAttackInfo();
            public SimpleAttackInfo harvestAttack => m_harvestAttack;
            [SerializeField, TabGroup("Attacks2", "Harvest")]
            private BasicAnimationInfo m_harvestAnticipation;
            public BasicAnimationInfo harvestAnticipation => m_harvestAnticipation;
            [SerializeField, TabGroup("Attacks2", "Harvest")]
            private BasicAnimationInfo m_harvestPullNoHeal;
            public BasicAnimationInfo harvestPullNoHeal => m_harvestPullNoHeal;
            [SerializeField, TabGroup("Attacks2", "Harvest")]
            private BasicAnimationInfo m_harvestPullWithHeal;
            public BasicAnimationInfo harvestPullWithHeal => m_harvestPullWithHeal;
            [SerializeField, TabGroup("Attacks2", "Harvest")]
            private BasicAnimationInfo m_harvestScytheDrag;
            public BasicAnimationInfo harvestScytheDrag => m_harvestScytheDrag;
            [SerializeField, TabGroup("Attacks2", "Harvest")]
            private AttackData m_harvestAttackData;
            public AttackData harvestAttackData => m_harvestAttackData;
            [SerializeField, TabGroup("Attacks2", "Harvest")]
            private int m_harvestHealAmount;
            public int harvestHealAmount => m_harvestHealAmount;
            [SerializeField, TabGroup("Attacks2", "Harvest")]
            private float m_harvestChaseSpeed;
            public float harvestChaseSpeed => m_harvestChaseSpeed;
            [SerializeField, TabGroup("Attacks2", "Harvest")]
            private float m_harvestChaseDuration;
            public float harvestChaseDuration => m_harvestChaseDuration;

            [SerializeField, TabGroup("Attacks3", "Death Stench Wave")]
            private SimpleAttackInfo m_deathStenchWaveAttack = new SimpleAttackInfo();
            public SimpleAttackInfo deathStenchWaveAttack => m_deathStenchWaveAttack;
            [SerializeField, TabGroup("Attacks3", "Death Stench Wave")]
            private BasicAnimationInfo m_deathStenchWaveAnticipation;
            public BasicAnimationInfo deathStenchWaveAnticipation => m_deathStenchWaveAnticipation;
            [SerializeField, TabGroup("Attacks3", "Death Stench Wave")]
            private AttackData m_deathStenchWaveImpactAttackData;
            public AttackData deathStenchWaveImpactAttackData => m_deathStenchWaveImpactAttackData;

            [Title("Misc")]
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;
            [SerializeField]
            private float m_shortRangedAttackEvaluateDistance;
            public float shortRangedAttackEvaluateDistance => m_shortRangedAttackEvaluateDistance;
            [SerializeField]
            private float m_minMoveTime;
            public float minMoveTime => m_minMoveTime;
            [SerializeField]
            private float m_maxMoveTime;
            public float maxMoveTime => m_maxMoveTime;
            [SerializeField]
            private float m_moveAdjustmentTime;
            public float moveAdjustmentTime => m_moveAdjustmentTime;
            [SerializeField]
            private int m_numberOfHitsToRetreat;
            public int numberOfHitsToRetreat => m_numberOfHitsToRetreat;

            [Title("Animations")]
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_defeatAnimation;
            public BasicAnimationInfo defeatAnimation => m_defeatAnimation;
            [SerializeField]
            private BasicAnimationInfo m_defeat2Animation;
            public BasicAnimationInfo defeat2Animation => m_defeat2Animation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation;
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchBlackAnimation;
            public BasicAnimationInfo flinchBackAnimation => m_flinchBlackAnimation;

            [SerializeField]
            private BasicAnimationInfo m_floatAnimation;
            public BasicAnimationInfo floatAnimation => m_floatAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idle1Animation;
            public BasicAnimationInfo idle1Animation => m_idle1Animation;
            [SerializeField]
            private BasicAnimationInfo m_idle2Animation;
            public BasicAnimationInfo idle2Animation => m_idle2Animation;
            [SerializeField]
            private BasicAnimationInfo m_idleTransition1to2Animation;
            public BasicAnimationInfo idleTransition1to2Animation => m_idleTransition1to2Animation;
            [SerializeField]
            private BasicAnimationInfo m_idleTransition2to1Animation;
            public BasicAnimationInfo idleTransition2to1Animation => m_idleTransition2to1Animation;
            [SerializeField]
            private BasicAnimationInfo m_playerDetectAnimation;
            public BasicAnimationInfo playerDetectAnimation => m_playerDetectAnimation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation;
            public BasicAnimationInfo turnAnimation => m_turnAnimation;
            [SerializeField]
            private BasicAnimationInfo m_energyAbsorbAnimation;
            public BasicAnimationInfo energyAbsorbAnimation => m_energyAbsorbAnimation;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_rageQuakeAnimation.SetData(m_skeletonDataAsset);

                m_move.SetData(m_skeletonDataAsset);
                m_floatAnimation.SetData(m_skeletonDataAsset);

                //Attack Animations
                m_scytheThrowAttack.SetData(m_skeletonDataAsset);
                m_scytheSwipeAttack.SetData(m_skeletonDataAsset);
                m_scytheSmashAttack.SetData(m_skeletonDataAsset);
                m_royalGuardianShieldSummon.SetData(m_skeletonDataAsset);
                m_scytheSwipeTwoAttack.SetData(m_skeletonDataAsset);
                m_harvestAttack.SetData(m_skeletonDataAsset);
                m_deathStenchWaveAttack.SetData(m_skeletonDataAsset);

                //Attack Anticipation Animations
                m_scytheThrowAnticipation.SetData(m_skeletonDataAsset);
                m_scytheSwipeAnticipation.SetData(m_skeletonDataAsset);
                m_scytheSmashAnticipation.SetData(m_skeletonDataAsset);
                m_royalGuardianAnticipation.SetData(m_skeletonDataAsset);
                m_scytheSwipeTwoAnticipation.SetData(m_skeletonDataAsset);
                m_harvestAnticipation.SetData(m_skeletonDataAsset);
                m_deathStenchWaveAnticipation.SetData(m_skeletonDataAsset);

                //Other Attack Animations
                m_scytheThrowThrow.SetData(m_skeletonDataAsset);
                m_scytheThrowWaitForScythe.SetData(m_skeletonDataAsset);
                m_scytheThrowCatch.SetData(m_skeletonDataAsset);
                m_harvestPullNoHeal.SetData(m_skeletonDataAsset);
                m_harvestPullWithHeal.SetData(m_skeletonDataAsset);
                m_harvestScytheDrag.SetData(m_skeletonDataAsset);
                m_scytheSmashGroundLoop.SetData(m_skeletonDataAsset);
                m_scytheSmashRemoveScytheFromGround.SetData(m_skeletonDataAsset);
                m_royalGuardianShieldSlam.SetData(m_skeletonDataAsset);

                //Other Behavior Animations
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_defeatAnimation.SetData(m_skeletonDataAsset);
                m_defeat2Animation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_flinchBlackAnimation.SetData(m_skeletonDataAsset);
                m_idle1Animation.SetData(m_skeletonDataAsset);
                m_idle2Animation.SetData(m_skeletonDataAsset);
                m_idleTransition1to2Animation.SetData(m_skeletonDataAsset);
                m_idleTransition2to1Animation.SetData(m_skeletonDataAsset);
                m_playerDetectAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_energyAbsorbAnimation.SetData(m_skeletonDataAsset);
#endif
            }
        }

        [System.Serializable]
        public class PhaseInfo : IPhaseInfo
        {
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
            PreAttackMovement,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        private enum Mode
        {
            Normal,
            RoyalGuardian
        }

        private enum Attack
        {
            ScytheThrow,
            ScytheSwipe1,
            ScytheSwipe2,
            ScytheSmash,
            RoyalGuard1,
            RoyalGuard2,
            Harvest,
            DeathStenchWave,
            NullAttack,
        }

        public enum Phase
        {
            PhaseOne,
            PhaseTwo,
            PhaseThree,
            Wait,
        }

        [SerializeField, TabGroup("Reference")]
        private Boss m_boss;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        [SerializeField, TabGroup("Reference")]
        private Transform m_arenaCenter;
        [SerializeField, TabGroup("Reference")]
        private Attacker m_attacker;

        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private PathFinderAgent m_agent;
        [SerializeField, TabGroup("Modules")]
        private Health m_health;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_rightWallSensor;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_leftWallSensor;

        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_slashGroundFX;
        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_scytheSpinFX;

        [SerializeField, TabGroup("Hurtbox")]
        private Collider2D m_scytheSpinBB;
        [SerializeField, TabGroup("Hurtbox")]
        private Collider2D m_groundStabBB;
        [SerializeField, TabGroup("Hurtbox")]
        private Collider2D m_scytheStabBB;

        [SerializeField]
        private SpineEventListener m_spineListener;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        State m_turnState;
        [ShowInInspector]
        private PhaseHandle<Phase, PhaseInfo> m_phaseHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_longRangedAttackDecider;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_shortRangedAttackDecider;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_longRangedAttackDeciderWithAttackCounter;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_royalGuardianAttackDecider;

        [ShowInInspector]
        private RandomAttackDecider<Attack> m_currentAttackDecider;

        private Attack m_currentAttack;
        //private ProjectileLauncher m_projectileLauncher;

        [SerializeField, TabGroup("Projectile Spawn Points")]
        private Transform m_projectilePoint;

        [SerializeField, TabGroup("Attacks", "ScytheThrow")]
        private RoyalDeathGuardScytheProjectile m_scytheThrowProjectile;
        [SerializeField, TabGroup("Attacks", "Scythe Throw")]
        private Transform m_scytheThrowReleasePoint;
        [SerializeField, TabGroup("Attacks", "Scythe Throw")]
        private Transform m_scytheThrowTargetPoint;
        [SerializeField, TabGroup("Attacks", "Scythe Throw")]
        private RaySensor m_scytheThrowWallCheckSensor;
        [SerializeField, TabGroup("Attacks", "Scythe Throw")]
        private float m_scytheThrowReleaseHeight;
        [SerializeField, TabGroup("Attacks", "Scythe Throw")]
        private float m_scytheThrowTargetHeight;

        [SerializeField, TabGroup("Attacks", "Scythe Smash")]
        private RoyalDeathGuardScythSmashDeathStench m_scytheSmashDeathStench;
        [SerializeField, TabGroup("Attacks", "Scythe Smash")]
        private Transform m_scytheSmashDeathStenchSpawn;

        [SerializeField, TabGroup("Attacks", "Royal Guardian")]
        private RoyalDeathGuardShield m_royalGuardianShield;
        [SerializeField, TabGroup("Attacks", "Royal Guardian")]
        private RoyalDeathGuardRoyalGuardShieldSlam m_royalGuardianShieldSlam;
        [SerializeField, ReadOnly, TabGroup("Attacks", "Royal Guardian")]
        private bool m_royalGuardianShieldActive;

        [SerializeField, TabGroup("Attacks", "Death Stench Wave")]
        private RoyalDeathGuardDeathStenchWave m_deathStenchWave;

        [SerializeField, BoxGroup("Movement")]
        private float m_groundCombatHeight;
        [SerializeField, BoxGroup("Movement")]
        private Transform m_leftRetreatPoint;
        [SerializeField, BoxGroup("Movement")]
        private Transform m_rightRetreatPoint;
        [SerializeField, BoxGroup("Movement")]
        private float m_destinationDistanceTolerance;
        [SerializeField, BoxGroup("Movement")]
        private float m_dynamicMoveLeftMaxDistance;
        [SerializeField, BoxGroup("Movement")]
        private float m_dynamicMoveRightMaxtDistance;

        //Consecutive Hit variables
        [SerializeField, ReadOnly]
        private int m_consecutiveHitToFlinchCounter;
        [SerializeField, ReadOnly]
        private float m_consectiveHitTimer;
        [SerializeField, ReadOnly]
        private bool m_canTrackConsecutiveHits;
        [SerializeField, ReadOnly]
        private bool m_isFlinchForbidden;

        private int m_randomAttack;
        private float m_startGroundPos;
        private bool m_hasHealed;
        private PhaseInfo m_phaseInfo;
        [SerializeField, ReadOnly]
        private bool m_scriptedPhaseTwoAttackDone;
        [SerializeField, ReadOnly]
        private bool m_scriptedPhaseThreeAttackDone;
        [SerializeField, ReadOnly]
        private int m_retreatHitCounter;
        [SerializeField, ReadOnly]
        private bool m_canTrackRetreatHits;

        private string[] m_idleAnimationNames; //when trying to make dynamic idle but not pursued as of 7-25-24

        [SerializeField, ReadOnly]
        private int m_attackCounter;
        [SerializeField, ReadOnly]
        private Attack m_lastAttack;

        [SerializeField, BoxGroup("TESTING")]
        private bool m_testingMode;

        private void ApplyPhaseData(PhaseInfo obj)
        {
            m_phaseInfo = obj;
            UpdateAttackDeciderList();
        }

        private void ChangeState()
        {
            StopAllCoroutines();
            m_scytheSpinFX.Stop();
            m_animation.DisableRootMotion();
            m_animation.SetEmptyAnimation(0, 0);
            m_stateHandle.OverrideState(State.Phasing);
            m_phaseHandle.ApplyChange();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                if (m_stateHandle.currentState == State.Idle)
                {
                    base.SetTarget(damageable, m_target);
                    m_stateHandle.OverrideState(State.ReevaluateSituation);
                }
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

        private Vector2 GroundPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(m_projectilePoint.position, Vector2.down, 1000, DChildUtility.GetEnvironmentMask());
            return hit.point;
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            StopAllCoroutines();
            m_scytheSpinFX.Stop();
            m_agent.Stop();
            m_hitbox.Disable();
            if (!m_deathHandle.gameObject.activeSelf)
            {
                this.enabled = false;
                StartCoroutine(DefeatRoutine());
                StartCoroutine(DeathStickRoutine());
            }
            else
            {
                StartCoroutine(DeathStickRoutine());
            }
        }

        private void SetRandomIdleAnimation()
        {
            int value = Random.Range(0, m_idleAnimationNames.Length);
            string idleAnimationName = m_idleAnimationNames[value];
            m_animation.SetAnimation(0, idleAnimationName, true);
        }

        private IEnumerator DefeatRoutine()
        {
            m_animation.SetAnimation(0, m_info.defeatAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.defeatAnimation);
            this.gameObject.SetActive(false);
        }

        private IEnumerator DeathStickRoutine()
        {
            var yAxis = transform.position.y;
            while (true)
            {
                transform.position = new Vector2(transform.position.x, yAxis);
                yield return null;
            }
        }

        #region Modules
        private IEnumerator FlinchRoutine()
        {
            m_agent.Stop();
            var flinch = IsFacingTarget() ? m_info.flinchAnimation : m_info.flinchBackAnimation;
            m_animation.SetAnimation(0, flinch, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, flinch);
        }

        private IEnumerator ChangePhaseRoutine()
        {
            m_stateHandle.Wait(State.Attacking);

            m_consecutiveHitToFlinchCounter = 0;
            m_attackCounter = 0;
            m_retreatHitCounter = 0;
            m_currentAttackDecider.hasDecidedOnAttack = false;

            m_isFlinchForbidden = true;
            yield return FlinchRoutine();
            m_animation.SetAnimation(0, m_info.rageQuakeAnimation.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rageQuakeAnimation.animation);
            m_animation.SetAnimation(0, m_info.idle2Animation.animation, false);

            if(m_phaseHandle.currentPhase == Phase.PhaseTwo)
            {
                m_currentAttackDecider = new RandomAttackDecider<Attack>();
                m_lastAttack = Attack.NullAttack;
                m_currentAttackDecider.DecideOnAttack(Attack.RoyalGuard1);
                m_currentAttackDecider.hasDecidedOnAttack = true;
            }

            if(m_phaseHandle.currentPhase == Phase.PhaseThree)
            {
                m_currentAttackDecider = new RandomAttackDecider<Attack>();
                m_lastAttack = Attack.NullAttack;
                m_currentAttackDecider.DecideOnAttack(Attack.RoyalGuard2);
                m_currentAttackDecider.hasDecidedOnAttack = true;
            }

            m_stateHandle.ApplyQueuedState();
        }
        #endregion

        #region Attacks
        private void SetScytheThrowInfo()
        {
            RoyalDeathGuardScytheProjectile.FlightInfo chosenFlightInfo;
            bool willThrowScytheRight;

            if (transform.localScale.x < 0)
                willThrowScytheRight = false;
            else
                willThrowScytheRight = true;

            if (m_phaseHandle.currentPhase == Phase.PhaseOne)
            {
                //Pattern 1
                chosenFlightInfo = m_info.scytheThrowPatterns[0];          
            }
            else
            {
                //Patern 1 or Pattern 2
                int rand = Random.Range(0, 2);
                if (rand == 0)
                {
                    chosenFlightInfo = m_info.scytheThrowPatterns[0];
                }
                else
                {
                    chosenFlightInfo = m_info.scytheThrowPatterns[1];
                }
            }         

            Vector2 targetPointPosition;
            if(willThrowScytheRight)
                targetPointPosition = new Vector2(m_scytheThrowReleasePoint.position.x + chosenFlightInfo.distance, m_scytheThrowTargetHeight);
            else
                targetPointPosition = new Vector2(m_scytheThrowReleasePoint.position.x - chosenFlightInfo.distance, m_scytheThrowTargetHeight);

            //Manually rotating scytheThrow wall Sensor instead of putting in character sensor so that can cast only once
            if (transform.localScale.x < 0)
            {
                m_scytheThrowWallCheckSensor.transform.eulerAngles = new Vector3(0, 0, 180);
            }
            else
            {
                m_scytheThrowWallCheckSensor.transform.eulerAngles = Vector3.zero;
            }

            m_scytheThrowProjectile.SetFlightInfo(chosenFlightInfo);
            m_scytheThrowWallCheckSensor.multiRaycast.SetCastDistance(chosenFlightInfo.distance + 20);
            m_scytheThrowTargetPoint.position = targetPointPosition;

        }

        public void ThrowScythe()
        {
            //flip scythe
            if(transform.localScale.x < 1f)
            {
                Vector3 scale = new Vector3(-1f, 1, 1);
                m_scytheThrowProjectile.transform.localScale = scale;
            }
            m_scytheThrowProjectile.transform.position = m_scytheThrowReleasePoint.position;

            m_scytheThrowProjectile.ExecuteFlight(m_scytheThrowReleasePoint.position, m_scytheThrowTargetPoint.position);          
        }

        public void ReleaseScytheSmashDeathStench()
        {
            m_scytheSmashDeathStench.Execute();
        }

        public void RoyalGuardianDeathStenchProjectileRelease()
        {
            var target = m_targetInfo.transform;
            m_royalGuardianShieldSlam.Execute(target);
        }

        private IEnumerator SummonRoyalGuardianShieldRoutine()
        {
            m_animation.SetAnimation(0, m_info.royalGuardianAnticipation.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.royalGuardianAnticipation.animation);

            m_animation.SetAnimation(0, m_info.royalGuardianShieldSummon.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.royalGuardianShieldSummon.animation);

            m_royalGuardianShield.Spawn();

            m_royalGuardianShieldActive = true;
        }

        private void FacePlayerInstantly()
        {
            if (!IsFacingTarget())
            {
                m_turnHandle.ForceTurnImmidiately();
            }
        }

        public void HarvestHeal()
        {
            m_damageable.Heal(m_info.harvestHealAmount);
        }

        public void DeathStenchWaveExecute()
        {
            m_deathStenchWave.transform.position = m_arenaCenter.transform.position;                                                                                                                                                                                                                                  
            m_deathStenchWave.Execute();
        }
        private IEnumerator ScytheThrowRoutine()
        {
            m_stateHandle.Wait(State.PreAttackMovement);

            bool isReturning = false;
            bool timeToCatch = false;

            m_scytheThrowProjectile.ReturnToStart += OnReturnToStart;

            FacePlayerInstantly();
            SetScytheThrowInfo();

            m_scytheThrowWallCheckSensor.Cast();
            var hasHitWall = m_scytheThrowWallCheckSensor.isDetecting;
            if (hasHitWall)
            {
                m_currentAttackDecider.hasDecidedOnAttack = false;
                m_stateHandle.ApplyQueuedState();
                yield return null;
                yield break;
            }

            m_isFlinchForbidden = true;
            //Move up for scythe throw
            yield return ScytheThrowDynamicMoveRoutine(m_dynamicMoveLeftMaxDistance, m_dynamicMoveRightMaxtDistance, m_scytheThrowReleaseHeight);


            m_animation.SetAnimation(0, m_info.scytheThrowAnticipation.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.scytheThrowAnticipation.animation);

            ThrowScythe();

            m_animation.SetAnimation(0, m_info.scytheThrowWaitForScythe.animation, true);
            yield return new WaitForSeconds(0.75f); //smoothest looking timing with current animation 7/15/24

            m_animation.SetAnimation(0, m_info.scytheThrowCatch.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.scytheThrowCatch.animation);

            m_scytheThrowProjectile.ReturnToStart -= OnReturnToStart;

            if (!m_royalGuardianShieldActive)
            {
                m_attackCounter++;
            }

            m_animation.SetAnimation(0, m_info.floatAnimation.animation, true);
            yield return new WaitForSeconds(1f);

            //Move back to ground level
            yield return ReturnToGroundCombatHeight(m_dynamicMoveLeftMaxDistance, m_dynamicMoveRightMaxtDistance);

            m_isFlinchForbidden = false;
            m_lastAttack = Attack.ScytheThrow;
            m_currentAttackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            yield return null;

            void OnReturnToStart(object sender, EventActionArgs eventArgs)
            {
                isReturning = true;
            }
        }     

        private IEnumerator ScytheSwipeRoutine()
        {
            m_stateHandle.Wait(State.PreAttackMovement);

            m_attacker.SetData(m_info.scytheSwipeAttackData);

            m_agent.Stop();

            FacePlayerInstantly();

            m_animation.SetAnimation(0, m_info.scytheSwipeAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.scytheSwipeAttack.animation);

            m_attackCounter++;

            m_animation.SetAnimation(0, m_info.floatAnimation.animation, true);
            yield return new WaitForSeconds(1f);

            m_currentAttackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator ScytheSwipeTwoRoutine()
        {
            m_stateHandle.Wait(State.PreAttackMovement);

            m_attacker.SetData(m_info.scytheSwipeTwoAttackData);

            m_agent.Stop();

            FacePlayerInstantly();

            m_animation.EnableRootMotion(true, true);

            m_animation.SetAnimation(0, m_info.scytheSwipeTwoAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.scytheSwipeTwoAttack.animation);

            m_attackCounter++;

            m_animation.SetAnimation(0, m_info.floatAnimation.animation, true);
            yield return new WaitForSeconds(1f);

            m_animation.EnableRootMotion(false, false);
            m_currentAttackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator ScytheSmashRoutine()
        {
            m_stateHandle.Wait(State.PreAttackMovement);

            bool deathStenchDone = false;

            m_agent.Stop();
            FacePlayerInstantly();

            m_attacker.SetData(m_info.scytheSmashAttackData);

            m_scytheSmashDeathStench.Done += OnDeathStenchDone;

            m_scytheSmashDeathStench.transform.eulerAngles = transform.localScale.x < 1 ? new Vector3(0, 0, 180f) : Vector3.zero;

            m_animation.SetAnimation(0, m_info.scytheSmashAnticipation.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.scytheSmashAnticipation.animation);

            m_animation.SetAnimation(0, m_info.scytheSmashAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.scytheSmashAttack.animation);

            //scytheSmash Death Stench executed via animation event

            m_animation.SetAnimation(0, m_info.scytheSmashGroundLoop.animation, true);

            //loop anim while stuck on ground
            while (!deathStenchDone)
            {
                yield return null;
            }

            //anim taking scythe out
            m_animation.SetAnimation(0, m_info.scytheSmashRemoveScytheFromGround.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.scytheSmashRemoveScytheFromGround.animation);

            m_scytheSmashDeathStench.Done -= OnDeathStenchDone;

            m_attackCounter++;

            m_animation.SetAnimation(0, m_info.floatAnimation.animation, true);
            yield return new WaitForSeconds(1f);

            m_lastAttack = Attack.ScytheSmash;
            m_currentAttackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            yield return null;

            void OnDeathStenchDone(object sender, EventActionArgs eventArgs)
            {
                Debug.Log("Scythe Smash DeathStench Done ");
                deathStenchDone = true;
            }
        }

        private IEnumerator RoyalGuardianOneRoutine()
        {
            m_stateHandle.Wait(State.PreAttackMovement);

            if (!m_isFlinchForbidden)
            {
                m_isFlinchForbidden = false;
            }

            FacePlayerInstantly();

            if (!m_royalGuardianShieldActive)
            {
                m_agent.Stop();
                yield return SummonRoyalGuardianShieldRoutine();

                m_attackCounter = 0;
            }

            m_currentAttackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator RoyalGuardianTwoRoutine()
        {
            m_stateHandle.Wait(State.PreAttackMovement);

            if (!m_isFlinchForbidden)
            {
                m_isFlinchForbidden = false;
            }

            FacePlayerInstantly();

            if (!m_royalGuardianShieldActive)
            {
                m_agent.Stop();
                yield return SummonRoyalGuardianShieldRoutine();

                m_attackCounter = 0;
            }

            m_animation.SetAnimation(0, m_info.royalGuardianShieldSlam.animation, false); ;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.royalGuardianShieldSlam.animation);

            m_attackCounter = 0;

            m_currentAttackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator HarvestRoutine()
        {
            m_stateHandle.Wait(State.PreAttackMovement);

            m_attacker.SetData(m_info.harvestAttackData);

            bool willHeal = false;

            int harvestCounter = 0;

            m_animation.DisableRootMotion();

            FacePlayerInstantly();

            m_attacker.TargetDamaged += OnTargetDamagedByHarvest;

            //scythe drag
            //Set destination next to player and if possible gradually accelerate towards it
            while(harvestCounter < 2)
            {
                yield return HarvestChaseRoutine();

                m_animation.SetAnimation(0, m_info.harvestAnticipation.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.harvestAnticipation.animation);

                m_animation.SetAnimation(0, m_info.harvestAttack.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.harvestAttack.animation);

                //swap pull animation if willheal
                var pullAnimation = willHeal ? m_info.harvestPullWithHeal.animation : m_info.harvestPullNoHeal.animation;

                //play pull animation here
                m_animation.SetAnimation(0, pullAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, pullAnimation);

                harvestCounter++;
            }
            
            m_attacker.TargetDamaged -= OnTargetDamagedByHarvest;

            m_attackCounter++;

            m_animation.SetAnimation(0, m_info.floatAnimation.animation, true);
            yield return new WaitForSeconds(1f);

            m_currentAttackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            yield return null;

            void OnTargetDamagedByHarvest(object sender, CombatConclusionEventArgs eventArgs)
            {
                willHeal = true;
                harvestCounter = 3;               
                Debug.Log("Damaged by Harvest");
            }
        }

        private IEnumerator DeathStenchWaveRoutine()
        {
            m_stateHandle.Wait(State.PreAttackMovement);

            m_deathStenchWave.gameObject.SetActive(true);

            var center = new Vector2(m_arenaCenter.position.x, m_groundCombatHeight);

            yield return MoveIntoPositionRoutine(center, m_info.move.speed);

            m_isFlinchForbidden = true;

            m_animation.EnableRootMotion(true, true);
            m_animation.SetAnimation(0, m_info.deathStenchWaveAnticipation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathStenchWaveAnticipation);

            m_animation.SetAnimation(0, m_info.deathStenchWaveAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathStenchWaveAttack.animation);

            //release death stench wave via animation event
            m_animation.EnableRootMotion(false, false);

            m_attackCounter = 0;

            m_isFlinchForbidden = false;

            m_animation.SetAnimation(0, m_info.floatAnimation.animation, true);
            yield return new WaitForSeconds(1.5f);

            m_deathStenchWave.gameObject.SetActive(false);

            m_currentAttackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        #endregion

        #region Movement
        private IEnumerator MoveIntoPositionRoutine(Vector3 destination, float speed)
        {
            m_agent.Stop();
            m_agent.SetDestination(destination);

            bool hasReachedPosition = false;

            while (hasReachedPosition == false)
            {
                m_agent.Move(speed);

                FacePlayerInstantly();

                if (!IsFacing(destination))
                {
                    m_animation.SetAnimation(0, m_info.idle1Animation, true);
                }
                else
                {
                    m_animation.SetAnimation(0, m_info.move.animation, true);
                }

                if (Vector3.Distance(transform.position, destination) < m_destinationDistanceTolerance)
                {
                    hasReachedPosition = true;
                }
                yield return null;
            }

            FacePlayerInstantly();

            m_agent.Stop();
        }

        private IEnumerator ScytheThrowDynamicMoveRoutine(float leftDistance, float rightDistance, float yHeight)
        {
            var XRange = Random.Range(transform.position.x - leftDistance, transform.position.y + rightDistance);

            if (m_leftWallSensor.isDetecting || m_rightWallSensor.isDetecting)
            {
                if (m_leftWallSensor.isDetecting)
                {
                    XRange = Random.Range(transform.position.x, transform.position.y + rightDistance);
                }

                if (m_rightWallSensor.isDetecting)
                {
                    XRange = Random.Range(transform.position.x - leftDistance, transform.position.y);
                }
            }

            var groundPos = new Vector2(XRange, yHeight);

            yield return MoveIntoPositionRoutine(groundPos, m_info.move.speed);
        }

        private IEnumerator ReturnToGroundCombatHeight(float leftDistance, float rightDistance)
        {
            var XRange = Random.Range(transform.position.x - leftDistance, transform.position.y + rightDistance);

            if(m_leftWallSensor.isDetecting || m_rightWallSensor.isDetecting)
            {
                if (m_leftWallSensor.isDetecting)
                {
                    XRange = Random.Range(transform.position.x, transform.position.y + rightDistance);
                }

                if (m_rightWallSensor.isDetecting)
                {
                    XRange = Random.Range(transform.position.x - leftDistance, transform.position.y);
                }
            }
            
            var groundPos = new Vector2(XRange, m_groundCombatHeight);

            if (transform.position.y > m_groundCombatHeight || transform.position.y < m_groundCombatHeight)
            {
                yield return MoveIntoPositionRoutine(groundPos, m_info.move.speed);
            }
            yield return null;
        }

        private IEnumerator ChasePlayer(float speed)
        {
            m_stateHandle.Wait(State.ReevaluateSituation);

            Debug.Log("Chase Player");

            m_animation.DisableRootMotion();

            yield return AdjustPositionBeforeMoving(m_info.move.speed);

            var randomMoveTime = Random.Range(m_info.minMoveTime, m_info.maxMoveTime);

            while (randomMoveTime > 0)
            {
                FacePlayerInstantly();
                if (!IsFacingTarget())
                {
                    m_animation.SetAnimation(0, m_info.idle1Animation, true);
                }
                else
                {
                    m_animation.SetAnimation(0, m_info.move.animation, true);
                }

                if (IsTargetInRange(m_info.shortRangedAttackEvaluateDistance) || m_rightWallSensor.isDetecting || m_leftWallSensor.isDetecting)
                {
                    m_agent.Stop();
                    randomMoveTime = 0;
                }

                m_agent.SetDestination(new Vector2(m_targetInfo.position.x, m_groundCombatHeight));

                m_agent.Move(speed);
                
                randomMoveTime -= Time.deltaTime;
                yield return null;
            }

            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator RetreatFromPlayer(float speed)
        {
            m_stateHandle.Wait(State.ReevaluateSituation);

            m_animation.DisableRootMotion();

            Debug.Log("Retreat from Player");

            var randomMoveTime = Random.Range(m_info.minMoveTime, m_info.maxMoveTime);

            yield return AdjustPositionBeforeMoving(m_info.move.speed);

            Vector2 destination = SetRunawayPosition();
            m_agent.SetDestination(destination);

            while (randomMoveTime > 0)
            {
                FacePlayerInstantly();
                if (!IsFacing(destination))
                {
                    m_animation.SetAnimation(0, m_info.idle1Animation, true);
                }
                else
                {
                    m_animation.SetAnimation(0, m_info.move.animation, true);
                }

                m_agent.Move(speed);

                if(m_retreatHitCounter >= m_info.numberOfHitsToRetreat)
                {
                    if (m_rightWallSensor.isDetecting || m_leftWallSensor.isDetecting)
                    {
                        m_agent.Stop();
                        randomMoveTime = 0;
                        m_canTrackRetreatHits = true;
                        m_retreatHitCounter = 0;
                    }
                }
                else
                {
                    if (IsTargetInRange(m_info.shortRangedAttackEvaluateDistance) || m_rightWallSensor.isDetecting || m_leftWallSensor.isDetecting)
                    {
                        m_agent.Stop();
                        randomMoveTime = 0;
                    }
                }

                randomMoveTime -= Time.deltaTime;
                yield return null;
            }

            if(m_retreatHitCounter >= m_info.numberOfHitsToRetreat)
            {
                randomMoveTime = 0;
                m_canTrackRetreatHits = true;
                m_retreatHitCounter = 0;
            }

            m_agent.Stop();

            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator AdjustPositionBeforeMoving(float speed)
        {
            Vector2 destination = transform.position;

            //return to ground combat level if somehow ends up higher
            yield return ReturnToGroundCombatHeight(m_dynamicMoveLeftMaxDistance, m_dynamicMoveRightMaxtDistance);

            //Adjust position to left or right depending on wall sensor
            if (m_rightWallSensor.isDetecting || m_leftWallSensor.isDetecting)
            {
                float adjustmentTimer = m_info.moveAdjustmentTime;
                if (m_rightWallSensor.isDetecting) //move left a bit to stop detecting wall
                {
                    destination = new Vector2(m_leftRetreatPoint.position.x, m_groundCombatHeight);
                    m_agent.SetDestination(destination);
                    while (adjustmentTimer > 0)
                    {
                        m_agent.Move(speed);
                        adjustmentTimer -= Time.deltaTime;
                        yield return null;
                    }
                }

                if (m_leftWallSensor.isDetecting)
                {
                    destination = new Vector2(m_rightRetreatPoint.position.x, m_groundCombatHeight);
                    m_agent.SetDestination(destination);
                    while (adjustmentTimer > 0)
                    {
                        m_agent.Move(speed);
                        adjustmentTimer -= Time.deltaTime;
                        yield return null;
                    }
                }
            }

            m_agent.Stop();
        }

        private Vector2 SetRunawayPosition()
        {
            Vector2 destination = transform.position;

            int goRightDecider = Random.Range(0, 2);

            if(!m_rightWallSensor.isDetecting && !m_leftWallSensor.isDetecting)
            {
                destination = goRightDecider > 0 ? new Vector2(m_leftRetreatPoint.position.x, m_groundCombatHeight) : new Vector2(m_rightRetreatPoint.position.x, m_groundCombatHeight);
            }
            else if (m_leftWallSensor.isDetecting) //go right
            {
                destination = new Vector2(m_rightRetreatPoint.position.x, m_groundCombatHeight);
            }
            else if (m_rightWallSensor.isDetecting) //go left
            {
                destination = new Vector2(m_leftRetreatPoint.position.x, m_groundCombatHeight);
            }

            return destination;
        }

        private IEnumerator HarvestChaseRoutine()
        {
            m_agent.Stop();
            FacePlayerInstantly();

            m_animation.SetAnimation(0, m_info.harvestScytheDrag.animation, true);

            float timer = m_info.harvestChaseDuration;

            while (timer > 0)
            {
                
                Vector3 targetDestination = new Vector3(m_targetInfo.position.x, m_groundCombatHeight);
                if (Vector3.Distance(transform.position, targetDestination) < m_info.shortRangedAttackEvaluateDistance)
                {
                    m_agent.Stop();
                    timer = 0f;
                }
                m_agent.SetDestination(targetDestination);
                m_agent.Move(m_info.harvestChaseSpeed);
                yield return null;
            }

            m_agent.Stop();
        }
        #endregion

        private void UpdateAttackDeciderList()
        {
            m_royalGuardianAttackDecider.SetList(new AttackInfo<Attack>(Attack.DeathStenchWave, 0),
                                                 new AttackInfo<Attack>(Attack.ScytheThrow, 0));
            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseOne:
                    m_longRangedAttackDecider.SetList(new AttackInfo<Attack>(Attack.ScytheThrow, 0),
                                            new AttackInfo<Attack>(Attack.ScytheSmash, 0));
                    break;
                case Phase.PhaseTwo:
                    m_shortRangedAttackDecider.SetList(new AttackInfo<Attack>(Attack.ScytheSwipe2, 0),
                                                        new AttackInfo<Attack>(Attack.ScytheSwipe1, 0));
                    m_longRangedAttackDecider.SetList(new AttackInfo<Attack>(Attack.ScytheThrow, 0),
                                                       new AttackInfo<Attack>(Attack.ScytheSmash, 0),
                                                       new AttackInfo<Attack>(Attack.Harvest, 0));
                    break;
                case Phase.PhaseThree:
                    m_shortRangedAttackDecider.SetList(new AttackInfo<Attack>(Attack.ScytheSwipe2, 0),
                                                        new AttackInfo<Attack>(Attack.ScytheSwipe1, 0));
                    m_longRangedAttackDecider.SetList(new AttackInfo<Attack>(Attack.ScytheThrow, 0),
                                                       new AttackInfo<Attack>(Attack.ScytheSmash, 0),
                                                       new AttackInfo<Attack>(Attack.Harvest, 0));
                    m_longRangedAttackDeciderWithAttackCounter.SetList(new AttackInfo<Attack>(Attack.RoyalGuard2, 0), 
                                                        new AttackInfo<Attack>(Attack.DeathStenchWave, 0));
                    break;
            }
        }

        private IEnumerator ChooseAttack()
        {
            if(!m_currentAttackDecider.hasDecidedOnAttack)
            {
                do
                {
                    m_currentAttackDecider.DecideOnAttack();
                    yield return null;
                }
                while (m_currentAttackDecider.chosenAttack.attack == m_lastAttack);                   
            }          
        }

        public override void ApplyData()
        {
            base.ApplyData();
        }

        private void OnDamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
        {
            if (m_canTrackRetreatHits)
            {
                if (m_retreatHitCounter >= m_info.numberOfHitsToRetreat)
                {
                    m_canTrackRetreatHits = false;
                }

                m_retreatHitCounter++;
            }

            if(m_isFlinchForbidden)
            {
                m_canTrackConsecutiveHits = false;
            }
            else
            {
                m_canTrackConsecutiveHits = true;
            }

            if(m_canTrackConsecutiveHits)
            {
                m_consectiveHitTimer = m_info.consecutiveHitInterval;

                if (m_consecutiveHitToFlinchCounter >= m_info.maxConsecutiveHits)
                    m_consecutiveHitToFlinchCounter = 0;

                m_consecutiveHitToFlinchCounter++;

                float randomFlinchChanceValue = Random.Range(1, 100);

                if (randomFlinchChanceValue < m_info.GetFlinchChance(m_consecutiveHitToFlinchCounter))
                {
                    Debug.Log("Will Flinch");
                    ForcedFlinch();
                }
            }        
        }

        private IEnumerator ConsecutiveHitFlinchRoutine()
        {
            m_stateHandle.Wait(State.PreAttackMovement);
            yield return FlinchRoutine();

            //Set these values to force run away after flinch
            m_canTrackRetreatHits = false;
            m_retreatHitCounter = m_info.numberOfHitsToRetreat;

            m_currentAttackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
        }

        private void ForcedFlinch()
        {
            //StopCurrentBehaviour
            StopAllCoroutines();
            StartCoroutine(ConsecutiveHitFlinchRoutine());
        }

        protected override void Awake()
        {
            base.Awake();
            m_turnHandle.TurnDone += OnTurnDone;
            m_damageable.DamageTaken += OnDamageTaken;
            m_royalGuardianShield.Destroyed += OnRoyalGuardianShieldDestroyed;

            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            m_shortRangedAttackDecider = new RandomAttackDecider<Attack>();
            m_longRangedAttackDecider = new RandomAttackDecider<Attack>();
            m_royalGuardianAttackDecider = new RandomAttackDecider<Attack>();
            m_longRangedAttackDeciderWithAttackCounter = new RandomAttackDecider<Attack>();
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            UpdateAttackDeciderList();

            m_idleAnimationNames[0] = m_info.idle1Animation.animation;
            m_idleAnimationNames[1] = m_info.idle2Animation.animation;

        }

        private void OnRoyalGuardianShieldDestroyed(object sender, EventActionArgs eventArgs)
        {
            m_royalGuardianShieldActive = false;
        }

        protected override void Start()
        {
            base.Start();
            //m_spineListener.Subscribe(m_info.deathFXEvent, m_deathFX.Play);
            m_animation.DisableRootMotion();
            m_startGroundPos = GroundPosition().y;
            m_scytheSmashDeathStench.transform.position = m_scytheSmashDeathStenchSpawn.position;

            m_isFlinchForbidden = false;
            m_consecutiveHitToFlinchCounter = 0;
            m_consectiveHitTimer = m_info.consecutiveHitInterval;

            m_canTrackRetreatHits = true;

            m_phaseHandle = new PhaseHandle<Phase, PhaseInfo>();
            m_phaseHandle.Initialize(Phase.PhaseOne, m_info.phaseInfo, m_character, ChangeState, ApplyPhaseData);
            m_phaseHandle.ApplyChange();
        }

        private void Update()
        {
            m_consectiveHitTimer -= GameplaySystem.time.deltaTime;
            m_phaseHandle.MonitorPhase();

            //Consecutive Hit section handles counting down on consecutive hit interval
            if (m_canTrackConsecutiveHits)
            {
                if (m_consectiveHitTimer <= 0)
                {
                    m_canTrackConsecutiveHits = false;
                    m_consecutiveHitToFlinchCounter = 0;
                }
            }

            if (!m_canTrackConsecutiveHits)
            {
                m_consectiveHitTimer = m_info.consecutiveHitInterval;
            }

            //RDG Shield Active handles turning on hitbox if active and facing player
            if(m_royalGuardianShieldActive)
            {
                if (IsFacingTarget())
                {
                    m_hitbox.Disable();
                }
                else
                {
                    m_hitbox.Enable();
                }
            }
            else
            {
                m_hitbox.Enable();
            }

            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    // Starting State When Fight is not triggered
                    m_animation.SetAnimation(0, m_info.idle1Animation.animation, true);
                    break;
                case State.Phasing:
                    Debug.Log("Phase Time");
                    StartCoroutine(ChangePhaseRoutine());
                    break;
                case State.Turning:
                    Debug.Log("Turning Steet");
                    m_stateHandle.Wait(m_turnState);
                    StopAllCoroutines();
                    m_agent.Stop();
                    m_turnHandle.Execute(m_info.turnAnimation.animation, m_info.idle1Animation.animation);
                    break;
                case State.PreAttackMovement:
                    var decideRandomAction = Random.Range(0, 2);

                    if (m_canTrackRetreatHits)
                    {
                        if (decideRandomAction > 0)
                        {
                            StartCoroutine(ChasePlayer(m_info.move.speed));
                        }
                        else
                        {
                            StartCoroutine(RetreatFromPlayer(m_info.move.speed));
                        }
                    }
                    else
                    {
                        StartCoroutine(RetreatFromPlayer(m_info.move.speed));
                        m_canTrackRetreatHits = true;
                    }
                    

                    break;
                case State.Attacking:

                    StartCoroutine(ChooseAttack());

                    switch (m_currentAttackDecider.chosenAttack.attack)
                    {
                        case Attack.ScytheThrow:
                            StartCoroutine(ScytheThrowRoutine());
                            break;
                        case Attack.ScytheSwipe1:
                            StartCoroutine(ScytheSwipeRoutine());
                            break;
                        case Attack.ScytheSwipe2:
                            StartCoroutine(ScytheSwipeTwoRoutine());
                            break;
                        case Attack.ScytheSmash:
                            StartCoroutine(ScytheSmashRoutine());
                            break;
                        case Attack.RoyalGuard1:
                            StartCoroutine(RoyalGuardianOneRoutine());
                            break;
                        case Attack.RoyalGuard2:
                            StartCoroutine(RoyalGuardianTwoRoutine());
                            break;
                        case Attack.Harvest:
                            StartCoroutine(HarvestRoutine());
                            break;
                        case Attack.DeathStenchWave:
                            StartCoroutine(DeathStenchWaveRoutine());
                            break;
                    }                 
                    break;

                case State.ReevaluateSituation:
                    if (!m_testingMode)
                    {
                        if (!m_royalGuardianShieldActive)
                        {
                            switch (m_phaseHandle.currentPhase)
                            {
                                case Phase.PhaseOne:
                                    if (IsTargetInRange(m_info.shortRangedAttackEvaluateDistance))
                                    {
                                        m_currentAttackDecider = m_shortRangedAttackDecider;
                                        m_lastAttack = Attack.NullAttack;
                                        m_currentAttackDecider.DecideOnAttack(Attack.ScytheSwipe1);
                                        m_currentAttackDecider.hasDecidedOnAttack = true;
                                    }
                                    else
                                    {
                                        m_currentAttackDecider = m_longRangedAttackDecider;
                                        m_currentAttackDecider.hasDecidedOnAttack = false;
                                    }
                                    break;
                                case Phase.PhaseTwo:
                                    if (!m_scriptedPhaseTwoAttackDone)
                                    {
                                        if (m_health.currentValue <= m_health.maxValue * 0.6)
                                        {
                                            m_currentAttackDecider = m_shortRangedAttackDecider;
                                            m_lastAttack = Attack.NullAttack;
                                            m_currentAttackDecider.DecideOnAttack(Attack.RoyalGuard1);
                                            m_currentAttackDecider.hasDecidedOnAttack = true;
                                            m_scriptedPhaseTwoAttackDone = true;
                                            break;
                                        }
                                    }

                                    if (IsTargetInRange(m_info.shortRangedAttackEvaluateDistance))
                                    {
                                        if (m_attackCounter >= 5)
                                        {
                                            m_currentAttackDecider = m_shortRangedAttackDecider;
                                            m_lastAttack = Attack.NullAttack;
                                            m_currentAttackDecider.DecideOnAttack(Attack.RoyalGuard1);
                                            m_currentAttackDecider.hasDecidedOnAttack = true;
                                        }
                                        else
                                        {
                                            m_currentAttackDecider = m_shortRangedAttackDecider;
                                            m_currentAttackDecider.hasDecidedOnAttack = false;
                                        }
                                    }
                                    else
                                    {
                                        m_currentAttackDecider = m_longRangedAttackDecider;
                                        m_currentAttackDecider.hasDecidedOnAttack = false;
                                    }

                                    break;
                                case Phase.PhaseThree:
                                    if (!m_scriptedPhaseThreeAttackDone)
                                    {
                                        if (m_health.currentValue <= m_health.maxValue * 0.3)
                                        {
                                            m_currentAttackDecider = m_shortRangedAttackDecider;
                                            m_lastAttack = Attack.NullAttack;
                                            m_currentAttackDecider.DecideOnAttack(Attack.RoyalGuard2);
                                            m_currentAttackDecider.hasDecidedOnAttack = true;
                                            m_scriptedPhaseThreeAttackDone = true;
                                            break;
                                        }
                                    }

                                    if (IsTargetInRange(m_info.shortRangedAttackEvaluateDistance))
                                    {
                                        if (m_attackCounter >= 5)
                                        {
                                            m_currentAttackDecider = m_shortRangedAttackDecider;
                                            m_lastAttack = Attack.NullAttack;
                                            m_currentAttackDecider.DecideOnAttack(Attack.DeathStenchWave);
                                            m_currentAttackDecider.hasDecidedOnAttack = true;
                                        }
                                        else
                                        {
                                            m_currentAttackDecider = m_shortRangedAttackDecider;
                                            m_currentAttackDecider.hasDecidedOnAttack = false;
                                        }
                                    }
                                    else
                                    {
                                        if (m_attackCounter >= 5)
                                        {
                                            m_currentAttackDecider = m_longRangedAttackDeciderWithAttackCounter;
                                            m_currentAttackDecider.hasDecidedOnAttack = false;
                                        }
                                        else
                                        {
                                            m_currentAttackDecider = m_longRangedAttackDecider;
                                            m_currentAttackDecider.hasDecidedOnAttack = false;
                                        }

                                    }
                                    break;
                                default:
                                    break;
                            }

                        }
                        else
                        {
                            m_currentAttackDecider = m_royalGuardianAttackDecider;
                            m_currentAttackDecider.hasDecidedOnAttack = false;
                        }

                        m_stateHandle.SetState(State.Attacking);
                    }
                    else
                    {
                        //behaviour in testing mode
                        m_currentAttackDecider = m_longRangedAttackDecider;
                        if (IsFacingTarget())
                        {
                            m_stateHandle.SetState(State.Idle);
                        }  

                    }

                    break;
                case State.WaitBehaviourEnd:
                    return;
            }
        }

        [Button]
        private void ForceAttack(Attack attack)
        {
            m_stateHandle.Wait(State.Attacking);
            m_currentAttackDecider.SetList(new AttackInfo<Attack>(attack, 0));
            m_currentAttackDecider.DecideOnAttack(attack);
            m_currentAttackDecider.hasDecidedOnAttack = true;
            m_stateHandle.ApplyQueuedState();
        }

        [Button]
        private void GoDown(float leftDistance, float rightDistance)
        {
            StartCoroutine(ReturnToGroundCombatHeight(leftDistance, rightDistance));
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