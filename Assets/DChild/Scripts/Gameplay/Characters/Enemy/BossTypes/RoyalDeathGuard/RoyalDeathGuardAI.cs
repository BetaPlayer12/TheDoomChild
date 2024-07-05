using DChild.Gameplay.Combat;
using Holysoft.Event;
using DChild.Gameplay.Characters.AI;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Spine.Unity;

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

            [SerializeField, Min(0.1f), BoxGroup("Consecutive Hits")]
            private float m_consecutiveHitToFlinchInterval;
            public float consecutiveHitToFlinchInterval => m_consecutiveHitToFlinchInterval;
            [SerializeField, BoxGroup("Consecutive Hits")]
            private Dictionary<int, float> m_consecutiveHitsToFlinchChancePair;
            public float GetFlinchChance(int hitCount) => m_consecutiveHitsToFlinchChancePair.TryGetValue(hitCount, out float value) ? value : 0;

            [SerializeField, TabGroup("Rage Quake")]
            private BasicAnimationInfo m_rageQuakeAnimation;
            public BasicAnimationInfo rageQuakeAnimation => m_rageQuakeAnimation;

            [SerializeField]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;

            [SerializeField]
            private MovementInfo m_move2 = new MovementInfo();
            public MovementInfo move2 => m_move2;
            [SerializeField]
            private MovementInfo m_floatMove = new MovementInfo();
            public MovementInfo floatMove => m_floatMove;

            [Title("Attack Behaviours")]
            [SerializeField, TabGroup("Attacks1", "Scythe Throw")]
            private SimpleAttackInfo m_scytheThrowAttack = new SimpleAttackInfo();
            public SimpleAttackInfo scytheThrowAttack => m_scytheThrowAttack;
            [SerializeField, TabGroup("Attacks1", "Scythe Throw")]
            private BasicAnimationInfo m_scytheThrowAnticipation;
            public BasicAnimationInfo scytheThrowAnticipation => m_scytheThrowAnticipation;

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
            private AttackData m_scytheSmashAttackData;
            public AttackData scytheSmashAttackData => m_scytheSmashAttackData;

            [SerializeField, TabGroup("Attacks2", "Royal Guardian One")]
            private SimpleAttackInfo m_royalGuardianOneAttack = new SimpleAttackInfo();
            public SimpleAttackInfo royalGuardianOneAttack => m_royalGuardianOneAttack;
            [SerializeField, TabGroup("Attacks2", "Royal Guardian One")]
            private BasicAnimationInfo m_royalGuardianOneAnticipation;
            public BasicAnimationInfo royalGuardianOneAnticipation => m_royalGuardianOneAnticipation;

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
            private AttackData m_harvestAttackData;
            public AttackData harvestAttackData => m_harvestAttackData;

            [SerializeField, TabGroup("Attacks3", "Royal Guardian Two")]
            private SimpleAttackInfo m_royalGuardianTwoAttack = new SimpleAttackInfo();
            public SimpleAttackInfo royalGuardianTwoAttack => m_royalGuardianTwoAttack;
            [SerializeField, TabGroup("Attacks3", "Royal Guardian Two")]
            private BasicAnimationInfo m_royalGuardianTwoAnticipation;
            public BasicAnimationInfo royalGuardianTwoAnticipation => m_royalGuardianTwoAnticipation;

            [SerializeField, TabGroup("Attacks3", "Death Stench Wave")]
            private SimpleAttackInfo m_deathStenchWaveAttack = new SimpleAttackInfo();
            public SimpleAttackInfo deathStenchWaveAttack => m_deathStenchWaveAttack;
            [SerializeField, TabGroup("Attacks3", "Death Stench Wave")]
            private BasicAnimationInfo m_deathStenchWaveAnticipation;
            public BasicAnimationInfo m_deathStenchWaveAttackAnticipation => m_deathStenchWaveAttackAnticipation;
            [SerializeField, TabGroup("Attacks3", "Death Stench Wave")]
            private AttackData m_deathStenchWaveAttackData;
            public AttackData deathStenchWaveAttackData => m_deathStenchWaveAttackData;

            [Title("Misc")]
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

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
            public BasicAnimationInfo flinchBlackAnimation => m_flinchBlackAnimation;

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
                m_move2.SetData(m_skeletonDataAsset);
                m_floatMove.SetData(m_skeletonDataAsset);

                //Attack Animations
                m_scytheThrowAttack.SetData(m_skeletonDataAsset);
                m_scytheSwipeAttack.SetData(m_skeletonDataAsset);
                m_scytheSmashAttack.SetData(m_skeletonDataAsset);
                m_royalGuardianOneAttack.SetData(m_skeletonDataAsset);
                m_scytheSwipeTwoAttack.SetData(m_skeletonDataAsset);
                m_harvestAttack.SetData(m_skeletonDataAsset);
                m_royalGuardianTwoAttack.SetData(m_skeletonDataAsset);
                m_deathStenchWaveAttack.SetData(m_skeletonDataAsset);

                //Attack Anticipation Animations
                m_scytheThrowAnticipation.SetData(m_skeletonDataAsset);
                m_scytheSwipeAnticipation.SetData(m_skeletonDataAsset);
                m_scytheSmashAnticipation.SetData(m_skeletonDataAsset);
                m_royalGuardianOneAnticipation.SetData(m_skeletonDataAsset);
                m_scytheSwipeTwoAnticipation.SetData(m_skeletonDataAsset);
                m_harvestAnticipation.SetData(m_skeletonDataAsset);
                m_royalGuardianTwoAnticipation.SetData(m_skeletonDataAsset);
                m_deathStenchWaveAnticipation.SetData(m_skeletonDataAsset);

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
        private RandomAttackDecider<Attack> m_shortRangedAttackCountBasedAttackDecider;

        [ShowInInspector]
        private RandomAttackDecider<Attack> m_currentAttackDecider;

        private Attack m_currentAttack;
        //private ProjectileLauncher m_projectileLauncher;

        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_projectilePoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_scytheThrowPoint;

        [SerializeField, TabGroup("Projectiles")]
        private RoyalDeathGuardScytheProjectile m_scytheThrowProjectile;

        private int m_consecutiveHitToFlinchCounter;
        private float m_consectiveHitTimer;
        private bool m_willTrackConsecutiveHits;

        private int m_randomAttack;
        private float m_startGroundPos;
        private bool m_hasHealed;
        private PhaseInfo m_phaseInfo;

        [SerializeField, ReadOnly]
        private int m_attackCounter;

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
            var flinch = IsFacingTarget() ? m_info.flinchAnimation : m_info.flinchBlackAnimation;
            var flinchTrack = m_animation.SetAnimation(0, flinch, false);
            m_animation.AddAnimation(0, m_info.idle1Animation, true, 0);
            yield return new WaitForSpineAnimationComplete(flinchTrack);
        }

        private IEnumerator ChangePhaseRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_willTrackConsecutiveHits = false;
            yield return FlinchRoutine();
            var rageAnimation = m_animation.SetAnimation(0, m_info.rageQuakeAnimation, false);
            m_animation.AddAnimation(0, m_info.idle2Animation, true, 0);
            yield return new WaitForSpineAnimationComplete(rageAnimation);
            m_willTrackConsecutiveHits = true;
            m_stateHandle.ApplyQueuedState();
        }
        #endregion

        #region Attacks
        private IEnumerator ScytheThrow()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);

            m_animation.SetAnimation(0, m_info.scytheThrowAnticipation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.scytheThrowAnticipation);

            m_animation.SetAnimation(0, m_info.scytheThrowAttack, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.scytheThrowAttack);

            m_attackCounter++;

            m_animation.SetAnimation(0, m_info.floatMove.animation, true);
            yield return new WaitForSeconds(1f);

            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        public void ThrowScythe()
        {
            m_scytheThrowProjectile.transform.position = m_scytheThrowPoint.position;

            if (m_phaseHandle.currentPhase == Phase.PhaseOne)
            {
                //Pattern 1
                m_scytheThrowProjectile.SetFlightInfo(m_info.scytheThrowPatterns[0]);
            }
            else
            {
                //Patern 1 or Pattern 2
                int rand = Random.Range(0, 2);
                if (rand == 0)
                {
                    m_scytheThrowProjectile.SetFlightInfo(m_info.scytheThrowPatterns[0]);
                }
                else
                {
                    m_scytheThrowProjectile.SetFlightInfo(m_info.scytheThrowPatterns[1]);
                }
            }
            m_scytheThrowProjectile.ExecuteFlight(m_scytheThrowPoint.position, m_targetInfo.position);
        }
        
        private IEnumerator ScytheSwipe()
        {
            //m_animation.SetAnimation(0, m_info.scytheSwipeAnticipation.animation, false);
            //yield return new WaitForAnimationComplete();
            m_stateHandle.Wait(State.ReevaluateSituation);

            m_statsData.SetAttackData(m_info.scytheSwipeAttackData);

            m_animation.SetAnimation(0, m_info.scytheSwipeAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.scytheSwipeAttack.animation);

            m_attackCounter++;

            m_animation.SetAnimation(0, m_info.floatMove.animation, true);
            yield return new WaitForSeconds(1f);

            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator ScytheSwipeTwo()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);

            m_animation.EnableRootMotion(true, true);

            m_statsData.SetAttackData(m_info.scytheSwipeTwoAttackData);

            m_animation.SetAnimation(0, m_info.scytheSwipeTwoAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.scytheSwipeTwoAttack.animation);

            m_attackCounter++;

            m_animation.SetAnimation(0, m_info.floatMove.animation, true);
            yield return new WaitForSeconds(1f);

            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator ScytheSmash()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);

            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator RoyalGuardianOne()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);

            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator RoyalGuardianTwo()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);

            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator Harvest()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);

            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator DeathStenchWave()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);

            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        #endregion

        #region Movement
        private IEnumerator ExecuteMove(Vector2 target, float attackRange, /*float heightOffset,*/ Attack attack)
        {
            m_animation.DisableRootMotion();
            bool inRange = false;
            /*Vector2.Distance(transform.position, target) > m_info.spearMeleeAttack.range*/ //old target in range condition
            while (!inRange)
            {

                bool xTargetInRange = Mathf.Abs(target.x - transform.position.x) < attackRange ? true : false;
                bool yTargetInRange = Mathf.Abs(target.y - transform.position.y) < 1 ? true : false;
                if (xTargetInRange && yTargetInRange)
                {
                    inRange = true;
                }
                //Debug.Log("Facing Target " + IsFacingTarget());
                DynamicMovement(new Vector2(m_targetInfo.position.x, target.y));
                yield return null;
            }
            m_agent.Stop();
            yield return null;
        }

        private void DynamicMovement(Vector2 target)
        {
            if (IsFacingTarget())
            {
                var velocityX = GetComponent<IsolatedPhysics2D>().velocity.x;
                var velocityY = GetComponent<IsolatedPhysics2D>().velocity.y;
                //Debug.Log("Read Dynamic Movements " + velocityX + " " + velocityY);
                m_agent.SetDestination(target);
                m_agent.Move(m_info.move.speed);

                m_animation.SetAnimation(0, m_info.move.animation, true);
            }
            else
            {
                if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation && GetComponent<IsolatedPhysics2D>().velocity.y <= 0 && m_stateHandle.currentState != State.Phasing)
                {
                    m_turnState = State.Attacking;
                    m_stateHandle.OverrideState(State.Turning);
                }
            }
        }
        #endregion

        private void UpdateAttackDeciderList()
        {
            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseOne:
                    m_longRangedAttackDecider.SetList(new AttackInfo<Attack>(Attack.ScytheThrow, 0),
                                            new AttackInfo<Attack>(Attack.ScytheSmash, 0));
                    break;
                case Phase.PhaseTwo:
                    m_shortRangedAttackDecider.SetList(new AttackInfo<Attack>(Attack.ScytheSwipe2, 0),
                                                        new AttackInfo<Attack>(Attack.Harvest, 0));
                    m_longRangedAttackDecider.SetList(new AttackInfo<Attack>(Attack.ScytheThrow, 0),
                                                       new AttackInfo<Attack>(Attack.ScytheSmash, 0));
                    break;
                case Phase.PhaseThree:
                    m_shortRangedAttackDecider.SetList(new AttackInfo<Attack>(Attack.ScytheSwipe2, 0),
                                                        new AttackInfo<Attack>(Attack.Harvest, 0));
                    m_longRangedAttackDecider.SetList(new AttackInfo<Attack>(Attack.ScytheThrow, 0),
                                                       new AttackInfo<Attack>(Attack.ScytheSmash, 0));
                    m_shortRangedAttackCountBasedAttackDecider.SetList(new AttackInfo<Attack>(Attack.ScytheSwipe2, 0),
                                                                        new AttackInfo<Attack>(Attack.Harvest, 0));
                    break;
            }
        }

        public override void ApplyData()
        {
            base.ApplyData();
        }


        private void OnDamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
        {
            if (m_willTrackConsecutiveHits)
            {
                TrackConsecutiveHit();
            }

            var flinchChance = m_info.GetFlinchChance(m_consecutiveHitToFlinchCounter);
            if (flinchChance > 0)
            {
                if (Random.Range(0, 1f) <= flinchChance)
                {
                    m_consecutiveHitToFlinchCounter = 0;
                    ForcedFlinch();
                }
            }
        }

        private void TrackConsecutiveHit()
        {
            if (m_consectiveHitTimer > 0)
            {
                m_consecutiveHitToFlinchCounter++;
            }
            else
            {
                m_consecutiveHitToFlinchCounter = 1;
            }
            m_consectiveHitTimer = m_info.consecutiveHitToFlinchInterval;
        }

        private void ForcedFlinch()
        {
            //StopCurrentBehaviour
            StartCoroutine(ConsecutiveHitFlinchRoutine());
        }

        private IEnumerator ConsecutiveHitFlinchRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            yield return FlinchRoutine();
            m_stateHandle.ApplyQueuedState();
        }


        protected override void Awake()
        {
            base.Awake();
            m_turnHandle.TurnDone += OnTurnDone;
            m_damageable.DamageTaken += OnDamageTaken;

            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            m_longRangedAttackDecider = new RandomAttackDecider<Attack>();

            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            m_willTrackConsecutiveHits = true;
            UpdateAttackDeciderList();

        }

        protected override void Start()
        {
            base.Start();
            //m_spineListener.Subscribe(m_info.deathFXEvent, m_deathFX.Play);
            m_animation.DisableRootMotion();
            m_startGroundPos = GroundPosition().y;

            m_phaseHandle = new PhaseHandle<Phase, PhaseInfo>();
            m_phaseHandle.Initialize(Phase.PhaseOne, m_info.phaseInfo, m_character, ChangeState, ApplyPhaseData);
            m_phaseHandle.ApplyChange();
        }

        private void Update()
        {
            m_consectiveHitTimer -= GameplaySystem.time.deltaTime;
            m_phaseHandle.MonitorPhase();

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
                case State.Attacking:

                    if(m_currentAttackDecider.hasDecidedOnAttack == false)
                    {
                        m_currentAttackDecider.DecideOnAttack();
                    }

                    switch (m_currentAttackDecider.chosenAttack.attack)
                    {
                        case Attack.ScytheThrow:
                            StartCoroutine(ScytheThrow());
                            break;
                        case Attack.ScytheSwipe1:
                            StartCoroutine(ScytheSwipe());
                            break;
                        case Attack.ScytheSwipe2:
                            StartCoroutine(ScytheSwipeTwo());
                            break;
                        case Attack.ScytheSmash:
                            StartCoroutine(ScytheSmash());
                            break;
                        case Attack.RoyalGuard1:
                            StartCoroutine(RoyalGuardianOne());
                            break;
                        case Attack.RoyalGuard2:
                            StartCoroutine(RoyalGuardianTwo());
                            break;
                        case Attack.Harvest:
                            StartCoroutine(Harvest());
                            break;
                        case Attack.DeathStenchWave:
                            StartCoroutine(DeathStenchWave());
                            break;
                    }                 
                    break;

                case State.ReevaluateSituation:
                    //Phase 1 Reevaluation consists of check boss HP -> Check player distance 

                    //Sample Set long range attack decider if requirements met
                    m_currentAttackDecider = m_longRangedAttackDecider;
                    //m_currentAttackDecider.hasDecidedOnAttack = false;

                    //Sample Force Attack
                    //m_currentAttackDecider.hasDecidedOnAttack = true;
                    //m_currentAttackDecider.DecideOnAttack(Attack.ScytheThrow);

                    //Phase 2 Reevaluation consists of check boss HP -> Check player distance -> check attack counter

                    //Phase 3 Reevaluation consists of check boss HP -> Check player distance -> check attack counter

                    //Temporary Reevaluation Behavior
                    if (IsFacingTarget())
                    {
                        m_stateHandle.SetState(State.Idle);
                    }
                    else
                    {
                        m_stateHandle.SetState(State.Turning);
                    }
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }
        }

        [Button]
        private void ForceAttack(Attack attack)
        {
            m_currentAttackDecider.DecideOnAttack(attack);
            m_currentAttackDecider.hasDecidedOnAttack = true;
            m_stateHandle.SetState(State.Attacking);
        }

        private void SetCurrentAttackData()
        {
            //Change Attack Data
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