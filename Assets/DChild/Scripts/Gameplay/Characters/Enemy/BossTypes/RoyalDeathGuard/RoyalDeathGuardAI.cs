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


            [Title("Attack Behaviours")]
            [SerializeField, TabGroup("Attack 1")]
            private SimpleAttackInfo m_attack1 = new SimpleAttackInfo();
            public SimpleAttackInfo attack1 => m_attack1;
            [SerializeField, TabGroup("Attack 2")]
            private SimpleAttackInfo m_attack2 = new SimpleAttackInfo();
            public SimpleAttackInfo attack2 => m_attack2;
            [SerializeField, TabGroup("Attack 3")]
            private SimpleAttackInfo m_attack3 = new SimpleAttackInfo();
            public SimpleAttackInfo attack3 => m_attack3;
            [SerializeField, TabGroup("Attack 4")]
            private SimpleAttackInfo m_attack4 = new SimpleAttackInfo();
            public SimpleAttackInfo attack4 => m_attack4;
            [SerializeField, TabGroup("Attack 4")]
            private BasicAnimationInfo m_attack4bAnimation;
            public BasicAnimationInfo attack4bAnimation => m_attack4bAnimation;
            [SerializeField, TabGroup("Attack 4")]
            private BasicAnimationInfo m_attack4FinalAnimation;
            public BasicAnimationInfo attack4FinalAnimation => m_attack4FinalAnimation;
            [SerializeField, TabGroup("Scythe Spin")]
            private SimpleAttackInfo m_scytheSpinAttack = new SimpleAttackInfo();
            public SimpleAttackInfo scytheSpinAttack => m_scytheSpinAttack;


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
                m_attack1.SetData(m_skeletonDataAsset);
                m_attack2.SetData(m_skeletonDataAsset);
                m_attack3.SetData(m_skeletonDataAsset);
                m_attack4.SetData(m_skeletonDataAsset);
                m_scytheSpinAttack.SetData(m_skeletonDataAsset);
                //m_projectile.SetData(m_skeletonDataAsset);

                m_attack4bAnimation.SetData(m_skeletonDataAsset);
                m_attack4FinalAnimation.SetData(m_skeletonDataAsset);
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
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_currentAttack;
        //private ProjectileLauncher m_projectileLauncher;

        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_projectilePoint;

        private int m_consecutiveHitToFlinchCounter;
        private float m_consectiveHitTimer;
        private bool m_willTrackConsecutiveHits;

        private int m_randomAttack;
        private float m_startGroundPos;
        private bool m_hasHealed;
        private PhaseInfo m_phaseInfo;

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
        private IEnumerator Attack1Routine()
        {
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.attack1.animation, false);
            m_animation.AddAnimation(0, m_info.idle1Animation, true, 0)/*.MixDuration = 1*/;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack1.animation);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator Attack2Routine()
        {
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.attack2.animation, false);
            m_animation.AddAnimation(0, m_info.idle1Animation, true, 0)/*.MixDuration = 1*/;
            yield return new WaitForSeconds(.5f);
            m_groundStabBB.transform.position = new Vector2(m_targetInfo.position.x, GroundPosition().y);
            yield return new WaitForSeconds(1.75f);
            m_slashGroundFX.Play();
            m_groundStabBB.enabled = true;
            m_scytheStabBB.enabled = true;
            yield return new WaitForSeconds(1f);
            m_groundStabBB.enabled = false;
            m_scytheStabBB.enabled = false;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack2.animation);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator Attack3Routine()
        {
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.attack3.animation, false);
            m_animation.AddAnimation(0, m_info.idle1Animation, true, 0)/*.MixDuration = 1*/;
            yield return new WaitForSeconds(3f);
            //m_scytheSpinFX.gameObject.SetActive(true);
            m_scytheSpinFX.Play(); //m_scytheSpinFX.GetComponent<ParticleSystem>().Play();
            m_scytheSpinBB.enabled = true;
            yield return new WaitForSeconds(1.5f);
            m_scytheSpinFX.Stop();
            m_scytheSpinBB.enabled = false;
            //m_scytheSpinFX.gameObject.SetActive(false); //m_scytheSpinFX.GetComponent<ParticleSystem>().Stop();
            //yield return new WaitForSeconds(1.3f);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack3.animation);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator Attack4Routine()
        {
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.attack4.animation, false);
            m_animation.AddAnimation(0, m_info.attack4bAnimation, true, 0)/*.MixDuration = 1*/;
            m_animation.AddAnimation(0, m_info.idle1Animation, true, 0)/*.MixDuration = 1*/;
            //yield return new WaitForSeconds(1.3f);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.attack4bAnimation);
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
                    m_attackDecider.SetList(new AttackInfo<Attack>(Attack.ScytheThrow, 0),
                                            new AttackInfo<Attack>(Attack.ScytheSmash, 0));
                    break;
                case Phase.PhaseTwo:
                    break;
                case Phase.PhaseThree:
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
            m_attackDecider = new RandomAttackDecider<Attack>();

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


                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.ScytheThrow:
                            break;
                        case Attack.ScytheSwipe1:
                            break;
                        case Attack.ScytheSwipe2:
                            break;
                        case Attack.ScytheSmash:
                            break;
                        case Attack.RoyalGuard1:
                            break;
                        case Attack.RoyalGuard2:
                            break;
                        case Attack.Harvest:
                            break;
                        case Attack.DeathStenchWave:
                            break;
                    }
                    //m_stateHandle.Wait(State.Attacking);
                    //var randomFacing = UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1;
                    //var target = new Vector2(m_targetInfo.position.x, GroundPosition().y /*m_startGroundPos*/);
                    //m_attackCount++;
                    //switch (m_currentPattern)
                    //{
                    //    case Pattern.AttackPattern1:
                    //        if (m_randomAttack == 1) //do dis tomorow
                    //        {
                    //            if (m_attackCount <= m_phaseInfo.attackCount)
                    //            {
                    //                StartCoroutine(ExecuteMove(target, m_info.attack1.range, /*0,*/ Attack.Attack1));
                    //            }
                    //            else
                    //            {
                    //                m_stateHandle.OverrideState(State.ReevaluateSituation);
                    //            }
                    //        }
                    //        else
                    //        {
                    //            if (m_phaseHandle.currentPhase == Phase.PhaseTwo)
                    //            {
                    //                switch (m_attackCount)
                    //                {
                    //                    case 1:
                    //                        StartCoroutine(ExecuteMove(target, m_info.attack1.range, /*0,*/ Attack.Attack1));
                    //                        break;
                    //                    case 2:
                    //                        //StartCoroutine(ExecuteMove(m_targetInfo.position, m_info.attack2.range, /*0,*/ Attack.Attack2));
                    //                        ExecuteAttack(Attack.Attack2);
                    //                        break;
                    //                    case 3:
                    //                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                    //                        break;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                m_stateHandle.OverrideState(State.ReevaluateSituation);
                    //            }
                    //        }
                    //        ///////
                    //        //m_stateHandle.OverrideState(State.ReevaluateSituation);
                    //        break;
                    //    case Pattern.AttackPattern2:
                    //        switch (m_attackCount)
                    //        {
                    //            case 1:
                    //                StartCoroutine(ExecuteMove(target, m_info.attack2.range, /*0,*/ Attack.Attack2));
                    //                break;
                    //            case 2:
                    //                m_stateHandle.OverrideState(State.ReevaluateSituation);
                    //                break;
                    //        }
                    //        ///////
                    //        //m_stateHandle.OverrideState(State.ReevaluateSituation);
                    //        break;
                    //    case Pattern.AttackPattern3:
                    //        switch (m_attackCount)
                    //        {
                    //            case 1:
                    //                StartCoroutine(ExecuteMove(target, m_info.attack2.range, /*0,*/ Attack.Attack3));
                    //                break;
                    //            case 2:
                    //                Debug.Log("pattern 3 condition for health: " + (m_health.maxValue * .6f));
                    //                if (m_phaseHandle.currentPhase == Phase.PhaseOne ? m_health.currentValue <= (m_health.maxValue * .6f) : m_health.currentValue <= (m_health.maxValue * .1f))
                    //                {
                    //                    StartCoroutine(ExecuteMove(target, m_info.attack2.range, /*0,*/ Attack.Attack3));
                    //                }
                    //                else
                    //                {
                    //                    m_stateHandle.OverrideState(State.ReevaluateSituation);
                    //                }
                    //                break;
                    //            case 3:
                    //                if (m_phaseHandle.currentPhase == Phase.PhaseOne ? m_health.currentValue <= (m_health.maxValue * .6f) : m_health.currentValue <= (m_health.maxValue * .1f))
                    //                {
                    //                    m_randomAttack = m_phaseHandle.currentPhase == Phase.PhaseOne ? 1 : m_randomAttack;
                    //                    if (m_randomAttack == 1)
                    //                    {
                    //                        StartCoroutine(ExecuteMove(target, m_info.attack2.range, /*0,*/ Attack.Attack3));
                    //                    }
                    //                    else
                    //                    {
                    //                        StartCoroutine(ExecuteMove(target, m_info.attack2.range, /*0,*/ Attack.Attack4));
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    m_stateHandle.OverrideState(State.ReevaluateSituation);
                    //                }
                    //                break;
                    //            case 4:
                    //                m_stateHandle.OverrideState(State.ReevaluateSituation);
                    //                break;
                    //        }
                    //        ///////
                    //        //m_stateHandle.OverrideState(State.ReevaluateSituation);
                    //        break;
                    //}
                    break;

                //case State.Chasing:
                //    //DecidedOnAttack(false);
                //    ////if (IsTargetInRange(m_info.attack1.range))
                //    ////{
                //    ////    m_currentPattern = Pattern.AttackPattern1;
                //    ////}
                //    //if (IsFacingTarget())
                //    //{
                //    //    if (m_patternDecider.hasDecidedOnAttack)
                //    //    {
                //    //        m_attackCount = 0;
                //    //        m_randomAttack = UnityEngine.Random.Range(0, 2);
                //    //        m_stateHandle.SetState(State.Attacking);
                //    //    }
                //    //}
                //    //else
                //    //{
                //    //    m_turnState = State.Chasing;
                //    //    if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation /*&& m_animation.GetCurrentAnimation(0).ToString() != m_info.attackDaggersIdle.animation*/)
                //    //        m_stateHandle.SetState(State.Turning);
                //    //}
                //    break;
                case State.ReevaluateSituation:
                    //Debug.Log("20% of health is: " + m_health.maxValue * .2f);
                    //if (m_health.currentValue <= m_health.maxValue * .2f && !m_hasHealed)
                    //{
                    //    Debug.Log("Current health is: " + m_health.currentValue);
                    //    m_stateHandle.Wait(State.ReevaluateSituation);
                    //    StartCoroutine(HealingRoutine());
                    //    return;
                    //}
                    //m_stateHandle.SetState(State.Chasing);
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