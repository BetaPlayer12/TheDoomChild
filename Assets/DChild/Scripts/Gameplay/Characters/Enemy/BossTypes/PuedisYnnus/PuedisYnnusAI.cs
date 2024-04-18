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
using Spine.Unity.Modules;
using Spine.Unity.Examples;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using DChild.Temp;

namespace DChild.Gameplay.Characters.Enemies
{

    [AddComponentMenu("DChild/Gameplay/Enemies/Boss/PuedisYnnus")]
    public class PuedisYnnusAI : CombatAIBrain<PuedisYnnusAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField]
            private PhaseInfo<Phase> m_phaseInfo;
            public PhaseInfo<Phase> phaseInfo => m_phaseInfo;

            [SerializeField, BoxGroup("Movement")]
            private MovementInfo m_moveForward = new MovementInfo();
            public MovementInfo moveForward => m_moveForward;
            [SerializeField, BoxGroup("Movement")]
            private MovementInfo m_moveBackward = new MovementInfo();
            public MovementInfo moveBackward => m_moveBackward;

            [Title("Animations")]
            [SerializeField, BoxGroup("Teleportation")]
            private BasicAnimationInfo m_appearAnimation;
            public BasicAnimationInfo appearAnimation => m_appearAnimation;
            [SerializeField, BoxGroup("Teleportation")]
            private BasicAnimationInfo m_disappearAnimation;
            public BasicAnimationInfo disappearAnimation => m_disappearAnimation;

            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation;
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;

            [SerializeField]
            private List<string> m_idleAnimations;
            public List<string> idleAnimations => m_idleAnimations;

            [SerializeField]
            private BasicAnimationInfo m_turnAnimation;
            public BasicAnimationInfo turnAnimation => m_turnAnimation;

            [Title("Attacks")]

            #region Rain Projectile
            [SerializeField, BoxGroup("Rain Projectile")]
            private int m_rainProjectileSpawnCount;
            public int rainProjectileSpawnCount => m_rainProjectileSpawnCount;
            [SerializeField, BoxGroup("Rain Projectile")]
            private float m_minimumDistancePerRainProjectileInstance;
            public float minimumDistancePerRainProjectileInstance => m_minimumDistancePerRainProjectileInstance;
            [SerializeField, BoxGroup("Rain Projectile")]
            private float m_rainProjectileDropDelay;
            public float rainProjectileDropDelay => m_rainProjectileDropDelay;
            [SerializeField, BoxGroup("Rain Projectile")]
            private BasicAnimationInfo m_rainProjectileAnticipationAnimation;
            public BasicAnimationInfo rainProjectileAnticipationAnimation => m_rainProjectileAnticipationAnimation;
            [SerializeField, BoxGroup("Rain Projectile")]
            private BasicAnimationInfo m_rainProjectileAttackAnimation;
            public BasicAnimationInfo rainProjectileAttackAnimation => m_rainProjectileAttackAnimation;
            #endregion

            #region Encircled Projectile
            [SerializeField, BoxGroup("Encircled Projectile")]
            private BasicAnimationInfo m_encircledProjectileSummonAnimation;
            public BasicAnimationInfo encircledProjectileSummonAnimation => m_encircledProjectileSummonAnimation;
            [SerializeField, MinValue(0), BoxGroup("Encircled Projectile")]
            private float m_encircledProjectileRotationDelay;
            public float encircledProjectileRotationDelay => m_encircledProjectileRotationDelay;
            [SerializeField, BoxGroup("Encircled Projectile")]
            private BasicAnimationInfo m_encircledProjectileScatterAnimation;
            public BasicAnimationInfo encircledProjectileScatterAnimation => m_encircledProjectileScatterAnimation;
            [SerializeField, MinValue(0), BoxGroup("Encircled Projectile")]
            private float m_encircledProjectileScatterDelay;
            public float encircledProjectileScatterDelay => m_encircledProjectileScatterDelay;
            #endregion

            #region Massive Spike
            [SerializeField, BoxGroup("Massive Spike")]
            private BasicAnimationInfo m_massiveSpikeAnticipationAnimation;
            public BasicAnimationInfo massiveSpikeAnticipationAnimation => m_massiveSpikeAnticipationAnimation;
            [SerializeField, BoxGroup("Massive Spike")]
            private BasicAnimationInfo m_massiveSpikeAttackAnimation;
            public BasicAnimationInfo massiveSpikeAttackAnimation => m_massiveSpikeAttackAnimation;
            [SerializeField, MinValue(0), BoxGroup("Massive Spike")]
            private float m_massiveSpikeStayDuration;
            public float massiveSpikeStayDuration => m_massiveSpikeStayDuration;
            #endregion

            [SerializeField, BoxGroup("Multiple Spike")]
            private BasicAnimationInfo m_multipleSpikeSummonAnimation;
            public BasicAnimationInfo multipleSpikeSummonAnimation => m_multipleSpikeSummonAnimation;

            [SerializeField]
            private int m_patternedRainProjectileRepeatCount;
            public int patternedRainProjectileRepeatCount => m_patternedRainProjectileRepeatCount;

            [Title("Projectiles")]
            [SerializeField]
            private ProjectileInfo m_crimsonProjectile;
            public ProjectileInfo crimsonProjectile => m_crimsonProjectile;




            [SerializeField, BoxGroup("FleshBomb")]
            private SimpleProjectileAttackInfo m_staffSpinFleshBombProjectile;
            public SimpleProjectileAttackInfo staffSpinFleshBombProjectile => m_staffSpinFleshBombProjectile;



            public override void Initialize()
            {
#if UNITY_EDITOR
                m_moveForward.SetData(m_skeletonDataAsset);
                m_moveBackward.SetData(m_skeletonDataAsset);

                m_staffSpinFleshBombProjectile.SetData(m_skeletonDataAsset);

                m_appearAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_disappearAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);

                m_rainProjectileAnticipationAnimation.SetData(m_skeletonDataAsset);
                m_rainProjectileAttackAnimation.SetData(m_skeletonDataAsset);

                m_encircledProjectileSummonAnimation.SetData(m_skeletonDataAsset);
                m_encircledProjectileScatterAnimation.SetData(m_skeletonDataAsset);

                m_massiveSpikeAnticipationAnimation.SetData(m_skeletonDataAsset);
                m_massiveSpikeAttackAnimation.SetData(m_skeletonDataAsset);

                m_multipleSpikeSummonAnimation.SetData(m_skeletonDataAsset);

                m_turnAnimation.SetData(m_skeletonDataAsset);
#endif
            }
        }

        [System.Serializable]
        public class PhaseInfo : IPhaseInfo
        {
            [SerializeField, MinValue(0)]
            private float m_idleAfterAttackDuration;
            [SerializeField, MinValue(0)]
            private int m_teleportCountThreshold;
            [SerializeField, MinValue(0)]
            private float m_teleportReappearanceDelay;
            [SerializeField, MinValue(0)]
            private int m_atttackCountThreshold;

            [SerializeField, BoxGroup("Rain Projectile")]
            private int m_rainProjectileRepeatCount;

            public float idleAfterAttackDuration => m_idleAfterAttackDuration;
            public int teleportCountThreshold => m_teleportCountThreshold;
            public float teleportReappearanceDelay => m_teleportReappearanceDelay;
            public int atttackCountThreshold => m_atttackCountThreshold;
            public int rainProjectileRepeatCount => m_rainProjectileRepeatCount;
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

        private enum Attack
        {
            RainProjectiles,
            MassiveFleshSpikes,
            SummonAndScatterProjectiles,
            MultipleSpikeToOtherAttacksCombo,
            TEST
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
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;

        [SerializeField]
        private SpineEventListener m_spineListener;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        [ShowInInspector]
        private PhaseHandle<Phase, PhaseInfo> m_phaseHandle;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_currentAttack;

        private int m_teleportCounter;
        private int m_attackCounter;

        #region ProjectileLaunchers
        private ProjectileLauncher m_fleshBombProjectileLauncher;
        #endregion

        [SerializeField, TabGroup("Flight Areas")]
        private BoxCollider2D[] m_flightAreas;


        [TitleGroup("Attacks")]
        [SerializeField, TabGroup("Attacks/Split", "Rain Projectile")]
        private BoxCollider2D m_rainProjectileSpawnArea;
        [SerializeField, TabGroup("Attacks/Split", "Rain Projectile")]
        private Transform m_rightmostFlightPosition;
        [SerializeField, TabGroup("Attacks/Split", "Rain Projectile")]
        private Transform m_leftmostFlightPosition;

        [SerializeField, TabGroup("Attacks/Split", "Encricled Projectile Attack")]
        private PuedisYnnusEncircledProjectileHandle m_encircledProjectileHandle;
        [SerializeField, TabGroup("Attacks/Split", "Encricled Projectile Attack")]
        private Transform m_encircledProjectileMovePoint;

        [SerializeField, TabGroup("Attacks/Split", "Massive Spike")]
        private PuedisYnnusMassiveSpikePattern[] m_massiveSpikePattern;

        [SerializeField, TabGroup("Attacks/Split", "Multiple Spike")]
        private PuedisYnnusMultipleSpikeHandle[] m_multipleSpikes;
        [SerializeField, TabGroup("Attacks/Split", "Multiple Spike")]
        private Transform m_multipleSpikesCastPosition;


        [SerializeField, TabGroup("Attacks/Split", "Patterned Rain Projectile")]
        private BoxCollider2D[] m_rainProjectilePatterns;

        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_projectilePoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Collider2D m_randomSpawnCollider;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_fleshBombPoint;
        [SerializeField, TabGroup("Spike Group")]
        private Transform m_horizontalSpikeGroup;
        [SerializeField, TabGroup("Spike Group")]
        private List<Transform> m_verticalSpikeGroups;


        private PhaseInfo m_currentPhaseInfo;
        private PuedisYnnusRainProjectileHandle m_rainProjectileHandle;

        private void ApplyPhaseData(PhaseInfo obj)
        {
            m_currentPhaseInfo = obj;
            UpdateAttackDeciderList();
        }

        private void ChangePhase()
        {
            StopAllCoroutines();
            m_stateHandle.OverrideState(State.Phasing);
            m_animation.DisableRootMotion();
            m_animation.SetEmptyAnimation(0, 0);
            m_phaseHandle.ApplyChange();

            m_attackCounter = 0;
        }

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null && m_stateHandle.currentState == State.Idle)
            {
                base.SetTarget(damageable, m_target);
                m_stateHandle.OverrideState(State.ReevaluateSituation);
            }
        }

        private IEnumerator ChangePhaseRoutine()
        {
            m_hitbox.Disable();
            m_animation.SetAnimation(0, m_info.disappearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.disappearAnimation);
            transform.position = new Vector2(m_randomSpawnCollider.bounds.center.x, m_randomSpawnCollider.bounds.center.y - 5);
            m_animation.SetAnimation(0, m_info.appearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.appearAnimation);

            m_hitbox.Enable();
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private void OnDamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
        {

        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {

        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            StopAllCoroutines();
            m_movement.Stop();
        }

        private Vector2 GetRandomPointInBounds(Bounds bounds)
        {
            var extents = bounds.extents;
            var xPosition = UnityEngine.Random.Range(-extents.x, extents.x);
            var yPosition = UnityEngine.Random.Range(-extents.y, extents.y);
            return bounds.center + new Vector3(xPosition, yPosition, 0);
        }

        private Vector3 GetRandomMajorFlightArea()
        {
            var randomIndex = UnityEngine.Random.Range(0, m_flightAreas.Length);
            return GetRandomPointInBounds(m_flightAreas[randomIndex].bounds);
        }

        private IEnumerator IdleFloatRoutine(float duration)
        {
            m_movement.Stop();
            var randomIdleAnimationIndex = UnityEngine.Random.Range(0, m_info.idleAnimations.Count);
            var chosenIdleAnimation = m_info.idleAnimations[randomIdleAnimationIndex];
            m_animation.SetAnimation(0, chosenIdleAnimation, true);
            yield return new WaitForSeconds(duration);
        }

        private IEnumerator FloatToDestinationRoutine(Vector3 destination)
        {
            var directionToDestination = (destination - transform.position).normalized;
            m_movement.MoveTowards(directionToDestination, m_info.moveForward.speed);

            if (IsFacing(destination))
            {
                m_animation.SetAnimation(0, m_info.moveForward, true);
            }
            else
            {
                m_animation.SetAnimation(0, m_info.moveBackward, true);
            }

            while (Vector3.Distance(transform.position, destination) > 0.5f)
            {
                yield return null;
            }
            m_movement.Stop();
        }

        private IEnumerator FloatToFarthestPoints()
        {
            var farthestFlightPath = new Vector3(m_rightmostFlightPosition.position.x, transform.position.y, 0);
            var distanceToRightMostFlightPath = Mathf.Abs(m_rightmostFlightPosition.position.x - transform.position.x);
            var distanceToLeftMostFlightPath = Mathf.Abs(m_leftmostFlightPosition.position.x - transform.position.x);

            if (distanceToLeftMostFlightPath > distanceToRightMostFlightPath)
            {
                farthestFlightPath = new Vector3(m_leftmostFlightPosition.position.x, transform.position.y, 0);
            }

            yield return FloatToDestinationRoutine(farthestFlightPath);

        }

        private IEnumerator FloatToRandomAreaThenAttackRoutine()
        {
            m_stateHandle.Wait(State.Attacking);
            yield return FloatToDestinationRoutine(GetRandomMajorFlightArea());
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator TeleportToDestination(Vector3 destination)
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_hitbox.Disable();
            m_animation.SetAnimation(0, m_info.disappearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.disappearAnimation);
            yield return new WaitForSeconds(m_currentPhaseInfo.teleportReappearanceDelay);
            transform.position = destination;
            yield return null;

            if (IsFacingTarget() == false)
            {
                m_turnHandle.ForceTurnImmidiately();
            }

            m_animation.SetAnimation(0, m_info.appearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.appearAnimation);
            m_hitbox.Enable();
            yield return null;
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator TeleportToRandomAreaThenAttack()
        {
            m_stateHandle.Wait(State.Attacking);
            yield return TeleportToDestination(GetRandomMajorFlightArea());
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator SpikeBombRoutine()
        {
            yield return IdleFloatRoutine(1f);
        }

        #region Attacks Modules

        private IEnumerator RainProjectileRoutine(Bounds bounds)
        {
            m_animation.SetAnimation(0, m_info.rainProjectileAnticipationAnimation, false);
            m_animation.AddAnimation(0, m_info.rainProjectileAttackAnimation, false, 0);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rainProjectileAttackAnimation);
            yield return IdleFloatRoutine(0.1f);
            m_rainProjectileHandle.SpawnProjectiles(m_info.crimsonProjectile.projectile, bounds, m_info.rainProjectileSpawnCount);
            yield return new WaitForSeconds(m_info.rainProjectileDropDelay);
            m_rainProjectileHandle.DropSpawnedProjectiles(m_info.crimsonProjectile.speed);
            yield return null;
        }

        private IEnumerator SummonEncircledProjectileRoutine()
        {
            var summonAnimation = m_animation.SetAnimation(0, m_info.encircledProjectileSummonAnimation, false);
            yield return new WaitForSpineAnimationComplete(summonAnimation, true);
            m_encircledProjectileHandle.SpawnProjectiles();
            yield return IdleFloatRoutine(0);
            yield return new WaitForSeconds(m_info.encircledProjectileRotationDelay);
            m_encircledProjectileHandle.StartRotation();
        }

        private IEnumerator ScatterEncircledProjectileRoutine()
        {
            var scatterAnimation = m_animation.SetAnimation(0, m_info.encircledProjectileScatterAnimation, false);
            yield return new WaitForSpineAnimationComplete(scatterAnimation, true);
            m_encircledProjectileHandle.StopRotation();
            yield return new WaitForSeconds(m_info.encircledProjectileScatterDelay);
            m_encircledProjectileHandle.ScatterProjectiles(m_info.crimsonProjectile.speed);
        }

        private IEnumerator MultipleFleshSpikesRoutine()
        {
            for (int i = 0; i < m_multipleSpikes.Length; i++)
            {
                var summonAnimation = m_animation.SetAnimation(0, m_info.multipleSpikeSummonAnimation, false);
                yield return new WaitForSpineAnimationComplete(summonAnimation, true);
                m_multipleSpikes[i].Grow();
                yield return IdleFloatRoutine(0.1f);
            }
            yield return null;
        }

        private IEnumerator PatternedRainProjectileRoutine()
        {
            var index = UnityEngine.Random.Range(0, m_rainProjectilePatterns.Length);
            yield return RainProjectileRoutine(m_rainProjectilePatterns[index].bounds);
        }

        private void LaunchFleshBomb()
        {
            if (!IsFacingTarget())
                CustomTurn();

            m_fleshBombProjectileLauncher.AimAt(m_targetInfo.position);
            m_fleshBombProjectileLauncher.LaunchProjectile();
        }

        private IEnumerator FleshBombRoutine()
        {
            m_animation.SetAnimation(0, m_info.staffSpinFleshBombProjectile.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.staffSpinFleshBombProjectile.animation);

            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        #endregion

        #region Attack Patterns

        private IEnumerator RainProjectileAttack()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            var repeat = 0;
            do
            {
                yield return RainProjectileRoutine(m_rainProjectileSpawnArea.bounds);
                yield return FloatToFarthestPoints();
                repeat++;
            } while (repeat < m_currentPhaseInfo.rainProjectileRepeatCount);
            yield return IdleFloatRoutine(m_currentPhaseInfo.idleAfterAttackDuration);

            m_teleportCounter++;
            m_attackCounter++;
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator SummonAndScatterProjectileAttack()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            yield return SummonEncircledProjectileRoutine();
            yield return FloatToDestinationRoutine(m_encircledProjectileMovePoint.position);
            yield return ScatterEncircledProjectileRoutine();
            yield return IdleFloatRoutine(m_currentPhaseInfo.idleAfterAttackDuration);

            m_teleportCounter++;
            m_attackCounter++;
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator MassiveSpikeAttack()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);

            var patternIndex = UnityEngine.Random.Range(0, m_massiveSpikePattern.Length);
            var chosenPattern = m_massiveSpikePattern[patternIndex];

            m_animation.SetAnimation(0, m_info.massiveSpikeAnticipationAnimation, false);
            m_animation.AddAnimation(0, m_info.massiveSpikeAttackAnimation, false, 0);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.massiveSpikeAttackAnimation);
            yield return IdleFloatRoutine(0.1f);
            chosenPattern.Grow();
            yield return new WaitForSeconds(m_info.massiveSpikeStayDuration);
            chosenPattern.Disappear();
            yield return IdleFloatRoutine(m_currentPhaseInfo.idleAfterAttackDuration);

            m_teleportCounter++;
            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator MultipleSpikeToOtherAttacksCombo()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            yield return FloatToDestinationRoutine(m_multipleSpikesCastPosition.position);
            yield return MultipleFleshSpikesRoutine();
            yield return IdleFloatRoutine(m_currentPhaseInfo.idleAfterAttackDuration);

            int rainProjectileRepeatCount = 0;
            do
            {
                yield return PatternedRainProjectileRoutine();
                rainProjectileRepeatCount++;
            } while (rainProjectileRepeatCount < m_info.patternedRainProjectileRepeatCount);

            yield return IdleFloatRoutine(m_currentPhaseInfo.idleAfterAttackDuration);

            yield return SpikeBombRoutine();

            for (int i = 0; i < m_multipleSpikes.Length; i++)
            {
                m_multipleSpikes[i].Disappear();
            }

            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator TeleportCountTEST()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_attackCounter++;
            m_teleportCounter++;
            m_attackDecider.hasDecidedOnAttack = false;
            yield return null;
            m_stateHandle.ApplyQueuedState();
        }

        #endregion

        private void UpdateAttackDeciderList()
        {
            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseOne:
                    m_attackDecider.SetList(new AttackInfo<Attack>(Attack.TEST, 0));
                    break;
                case Phase.PhaseTwo:
                    break;
                case Phase.Wait:
                    break;
            }

            m_attackDecider.hasDecidedOnAttack = false;
        }

        public override void ApplyData()
        {
            if (m_attackDecider != null)
            {
                UpdateAttackDeciderList();
            }
            base.ApplyData();
        }

        protected override void Awake()
        {
            base.Awake();
            //m_turnHandle.TurnDone += OnTurnDone;
            //m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            //m_fleshBombProjectileLauncher = new ProjectileLauncher(m_info.staffSpinFleshBombProjectile.projectileInfo, m_fleshBombPoint);

            m_damageable.DamageTaken += OnDamageTaken;
            m_attackDecider = new RandomAttackDecider<Attack>();
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            m_rainProjectileHandle = new PuedisYnnusRainProjectileHandle(m_info.minimumDistancePerRainProjectileInstance);
        }

        protected override void Start()
        {
            base.Start();
            //m_spineListener.Subscribe(m_info.handClenchSingleFleshSpikeProjectile.launchOnEvent, LaunchSingleSpike);
            //m_spineListener.Subscribe(m_info.staffSpinFleshBombProjectile.launchOnEvent, LaunchFleshBomb);

            m_animation.DisableRootMotion();

            m_phaseHandle = new PhaseHandle<Phase, PhaseInfo>();
            m_phaseHandle.Initialize(Phase.PhaseOne, m_info.phaseInfo, m_character, ChangePhase, ApplyPhaseData);
            m_phaseHandle.ApplyChange();
        }

        private void Update()
        {
            m_phaseHandle.MonitorPhase();
            switch (m_stateHandle.currentState)
            {
                case State.Idle:

                    //Leave Blank since Idle is being handle by A IdleFloatRoutine

                    break;

                case State.Phasing:
                    StopAllCoroutines();
                    StartCoroutine(ChangePhaseRoutine());
                    break;
                case State.Turning:
                    StopAllCoroutines();
                    // m_turnHandle.Execute(m_info.turnAnimation.animation, m_currentIdleAnimation);
                    m_movement.Stop();
                    break;
                case State.Attacking:
                    StopAllCoroutines();
                    if (m_attackDecider.hasDecidedOnAttack == false)
                    {
                        m_attackDecider.DecideOnAttack();
                    }
                    switch (m_attackDecider.chosenAttack.attack)
                    {
                        case Attack.RainProjectiles:
                            StartCoroutine(RainProjectileAttack());
                            break;
                        case Attack.SummonAndScatterProjectiles:
                            StartCoroutine(SummonAndScatterProjectileAttack());
                            break;
                        case Attack.MassiveFleshSpikes:
                            StartCoroutine(MassiveSpikeAttack());
                            break;
                        case Attack.MultipleSpikeToOtherAttacksCombo:
                            StartCoroutine(MultipleSpikeToOtherAttacksCombo());
                            break;
                        default:
                            StartCoroutine(TeleportCountTEST());
                            break;
                    }
                    m_stateHandle.Wait(State.ReevaluateSituation);

                    break;
                case State.ReevaluateSituation:

                    if (m_teleportCounter >= m_currentPhaseInfo.teleportCountThreshold)
                    {
                        m_teleportCounter = 0;
                        StartCoroutine(TeleportToDestination(GetRandomMajorFlightArea()));
                    }
                    else if (/*m_phaseHandle.currentPhase != Phase.PhaseOne &&*/ m_attackCounter > m_currentPhaseInfo.atttackCountThreshold)
                    {
                        m_attackCounter = 0;
                        m_stateHandle.SetState(State.Attacking);
                        m_attackDecider.DecideOnAttack(Attack.MultipleSpikeToOtherAttacksCombo);
                    }
                    else
                    {
                        StartCoroutine(FloatToRandomAreaThenAttackRoutine());
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