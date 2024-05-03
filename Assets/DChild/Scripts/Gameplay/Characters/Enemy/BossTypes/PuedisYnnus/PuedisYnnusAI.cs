using DChild.Gameplay.Combat;
using Holysoft.Event;
using DChild.Gameplay.Characters.AI;
using UnityEngine;
using Spine.Unity;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;

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
            private BasicAnimationInfo m_rainProjectileAnticipationLoopAnimation;
            public BasicAnimationInfo rainProjectileAnticipationLoopAnimation => m_rainProjectileAnticipationLoopAnimation;
            [SerializeField, BoxGroup("Rain Projectile")]
            private BasicAnimationInfo m_rainProjectileAttackAnimation;
            public BasicAnimationInfo rainProjectileAttackAnimation => m_rainProjectileAttackAnimation;
            [SerializeField, BoxGroup("Rain Projectile")]
            private BasicEventInfo m_rainProjectileAttackEvent;
            public BasicEventInfo rainProjectileAttackEvent => m_rainProjectileAttackEvent;
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

            [SerializeField, BoxGroup("FleshBomb")]
            private BasicAnimationInfo m_fleshBombAttackAnimation;
            public BasicAnimationInfo fleshBombAttackAnimation => m_fleshBombAttackAnimation;

            [Title("Projectiles")]
            [SerializeField]
            private ProjectileInfo m_crimsonProjectile;
            public ProjectileInfo crimsonProjectile => m_crimsonProjectile;



            [SerializeField]
            private float m_damageCheckDuration;
            public float damageCheckDuration => m_damageCheckDuration;

            [SerializeField]
            private float m_floatTimeCheckDuration;
            public float floatTimeCheckDuration => m_floatTimeCheckDuration;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_moveForward.SetData(m_skeletonDataAsset);
                m_moveBackward.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);

                m_appearAnimation.SetData(m_skeletonDataAsset);
                m_disappearAnimation.SetData(m_skeletonDataAsset);

                m_rainProjectileAnticipationAnimation.SetData(m_skeletonDataAsset);
                m_rainProjectileAnticipationLoopAnimation.SetData(m_skeletonDataAsset);
                m_rainProjectileAttackAnimation.SetData(m_skeletonDataAsset);
                m_rainProjectileAttackEvent.SetData(m_skeletonDataAsset);

                m_encircledProjectileSummonAnimation.SetData(m_skeletonDataAsset);
                m_encircledProjectileScatterAnimation.SetData(m_skeletonDataAsset);

                m_massiveSpikeAttackAnimation.SetData(m_skeletonDataAsset);

                m_multipleSpikeSummonAnimation.SetData(m_skeletonDataAsset);

                m_fleshBombAttackAnimation.SetData(m_skeletonDataAsset);
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
            IllusionSpikeToOtherAttacksCombo,
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



        #region ProjectileLaunchers
        private ProjectileLauncher m_fleshBombProjectileLauncher;
        #endregion

        [SerializeField, TabGroup("Flight Areas")]
        private BoxCollider2D[] m_flightAreas;


        [TitleGroup("Attacks")]
        [TabGroup("Attacks/Split", "Part1")]
        [SerializeField, TabGroup("Attacks/Split/Part1", "Rain Projectile")]
        private BoxCollider2D m_rainProjectileSpawnArea;
        [SerializeField, TabGroup("Attacks/Split/Part1", "Rain Projectile")]
        private Transform m_rightmostFlightPosition;
        [SerializeField, TabGroup("Attacks/Split/Part1", "Rain Projectile")]
        private Transform m_leftmostFlightPosition;

        [SerializeField, TabGroup("Attacks/Split/Part1", "Encricled Projectile Attack")]
        private PuedisYnnusEncircledProjectileHandle m_encircledProjectileHandle;
        [SerializeField, TabGroup("Attacks/Split/Part1", "Encricled Projectile Attack")]
        private Transform m_encircledProjectileMovePoint;

        [SerializeField, TabGroup("Attacks/Split/Part1", "Massive Spike")]
        private PuedisYnnusMassiveSpikePattern[] m_massiveSpikePattern;

        [TabGroup("Attacks/Split", "Part2")]
        [SerializeField, TabGroup("Attacks/Split/Part2", "Multiple Spike")]
        private PuedisYnnusMultipleSpikeHandle[] m_multipleSpikes;
        [SerializeField, TabGroup("Attacks/Split/Part2", "Multiple Spike")]
        private Transform m_multipleSpikesCastPosition;


        [SerializeField, TabGroup("Attacks/Split/Part2", "Patterned Rain Projectile")]
        private BoxCollider2D[] m_rainProjectilePatterns;

        [SerializeField, TabGroup("Attacks/Split/Part2", "Flesh Bomb")]
        private Transform m_fleshBombSummonParent;
        [SerializeField, TabGroup("Attacks/Split/Part2", "Flesh Bomb")]
        private Transform m_fleshBombExplosionDestination;
        [SerializeField, TabGroup("Attacks/Split/Part2", "Flesh Bomb")]
        private PuedisYnnusFleshSpikeBomb m_fleshBomb;

        [TabGroup("Attacks/Split", "Part3")]
        [SerializeField, TabGroup("Attacks/Split/Part3", "Illusion Platform")]
        private Transform m_illusionPlatformCastPosition;
        [SerializeField, TabGroup("Attacks/Split/Part3", "Illusion Platform")]
        private PuedisYnnusIllusionPlatform[] m_illusionPlatforms;
        [SerializeField, TabGroup("Attacks/Split/Part3", "Illusion Platform")]
        private PuedisYnnusSequenceMassiveSpikePattern m_sequenceMassiveSpike;
        [SerializeField, TabGroup("Attacks/Split/Part3", "Illusion Platform")]
        private PuedisYnnusMultipleSpikeHandle m_bottomMultipleSpike;


        private PhaseInfo m_currentPhaseInfo;
        private PuedisYnnusRainProjectileHandle m_rainProjectileHandle;

        private int m_teleportCounter;
        private int m_attackCounter;
        private bool m_wasHitDuringDamageChecks;

        private bool m_wasWaitingDuringPhaseChange;

        private void ApplyPhaseData(PhaseInfo obj)
        {
            m_currentPhaseInfo = obj;
            UpdateAttackDeciderList();
        }

        private void ChangePhase()
        {
            m_phaseHandle.ApplyChange();
            m_attackCounter = 0;
            m_teleportCounter = 0;

            m_wasWaitingDuringPhaseChange = m_stateHandle.currentState == State.WaitBehaviourEnd;
            m_stateHandle.OverrideState(State.Phasing);
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
            //Has No Special Behaviour for Phasing
            if (m_wasWaitingDuringPhaseChange)
            {
                m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            }
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
            StopAllCoroutines();
            m_encircledProjectileHandle.ScatterProjectiles(m_info.crimsonProjectile.speed);
            for (int i = 0; i < m_massiveSpikePattern.Length; i++)
            {
                m_massiveSpikePattern[i].Disappear();
            }
            for (int i = 0; i < m_multipleSpikes.Length; i++)
            {
                m_multipleSpikes[i].Disappear();
            }
            m_bottomMultipleSpike.Disappear();
            m_fleshBomb.gameObject.SetActive(false);
            m_rainProjectileHandle.DropSpawnedProjectiles(m_info.crimsonProjectile.speed);
            m_movement.Stop();
            m_stateHandle.Wait(State.WaitBehaviourEnd);
            base.OnDestroyed(sender, eventArgs);
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

            while (Vector3.Distance(transform.position, destination) > 5f)
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

        private IEnumerator DisappearRoutine()
        {
            m_hitbox.Disable();
            m_animation.SetAnimation(0, m_info.disappearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.disappearAnimation);
        }

        private IEnumerator AppearRoutine()
        {
            m_animation.SetAnimation(0, m_info.appearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.appearAnimation);
            m_hitbox.Enable();
        }

        private IEnumerator TeleportToDestination(Vector3 destination)
        {
            m_stateHandle.Wait(State.ReevaluateSituation);

            yield return DisappearRoutine();
            yield return new WaitForSeconds(m_currentPhaseInfo.teleportReappearanceDelay);
            transform.position = destination;
            yield return null;

            if (IsFacingTarget() == false)
            {
                m_turnHandle.ForceTurnImmidiately();
            }

            yield return AppearRoutine();
            yield return null;
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator TeleportToRandomAreaThenAttack()
        {
            m_stateHandle.Wait(State.Attacking);
            yield return TeleportToDestination(GetRandomMajorFlightArea());
            m_stateHandle.ApplyQueuedState();
        }

        #region Attacks Modules

        private IEnumerator RainProjectileRoutine(Bounds bounds)
        {
            m_animation.SetAnimation(0, m_info.rainProjectileAnticipationAnimation.animation, false, 0);
            var anticipationLoopAnimation = m_animation.AddAnimation(0, m_info.rainProjectileAnticipationLoopAnimation.animation, true, 0);
            yield return new WaitForSpineAnimation(anticipationLoopAnimation, WaitForSpineAnimation.AnimationEventTypes.Start);
            m_rainProjectileHandle.SpawnProjectiles(m_info.crimsonProjectile.projectile, bounds, m_info.rainProjectileSpawnCount);
            yield return new WaitForSeconds(m_info.rainProjectileDropDelay);
            var attackAnimation = m_animation.SetAnimation(0, m_info.rainProjectileAttackAnimation, false);
            yield return new WaitForSpineEvent(m_animation.skeletonAnimation, m_info.rainProjectileAttackEvent.eventName);
            m_rainProjectileHandle.DropSpawnedProjectiles(m_info.crimsonProjectile.speed);
            yield return new WaitForSpineAnimationComplete(attackAnimation);
            yield return IdleFloatRoutine(0.1f);
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

        private IEnumerator SummonMultipleFleshSpikesRoutine(PuedisYnnusMultipleSpikeHandle spikeHandle)
        {
            var summonAnimation = m_animation.SetAnimation(0, m_info.multipleSpikeSummonAnimation, false);
            yield return new WaitForSpineAnimationComplete(summonAnimation, true);
            spikeHandle.Grow();
            yield return IdleFloatRoutine(0.1f);
        }

        private IEnumerator MultipleFleshSpikesRoutine()
        {
            for (int i = 0; i < m_multipleSpikes.Length; i++)
            {
                yield return SummonMultipleFleshSpikesRoutine(m_multipleSpikes[i]);
            }
            yield return null;
        }

        private IEnumerator PatternedRainProjectileRoutine()
        {
            var index = UnityEngine.Random.Range(0, m_rainProjectilePatterns.Length);
            yield return RainProjectileRoutine(m_rainProjectilePatterns[index].bounds);
        }

        private IEnumerator FleshBombRoutine()
        {
            m_animation.SetAnimation(0, m_info.fleshBombAttackAnimation.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.fleshBombAttackAnimation.animation);
            //Spawn The Flesh Bomb
            m_fleshBomb.transform.parent = m_fleshBombSummonParent;
            m_fleshBomb.transform.localPosition = Vector3.zero;
            yield return m_fleshBomb.BeSummoned();

            m_fleshBomb.transform.parent = null;
            yield return m_fleshBomb.FloatTo(m_fleshBombExplosionDestination.position);

            yield return m_fleshBomb.ExplodeRoutine();
            yield return IdleFloatRoutine(0.1f);
        }

        private IEnumerator IllusionSpikeRoutine()
        {
            yield return DisappearRoutine();

            for (int i = 0; i < m_illusionPlatforms.Length; i++)
            {
                m_illusionPlatforms[i].Show();
            }

            yield return new WaitForSeconds(2f);

            transform.position = m_illusionPlatformCastPosition.position;
            yield return AppearRoutine();
            yield return IdleFloatRoutine(1f);
        }

        private IEnumerator PatternedMassiveSpikeRoutine()
        {
            yield return m_sequenceMassiveSpike.ExecuteSequence();
        }

        private IEnumerator DamageCheckRoutine(float duration)
        {
            m_wasHitDuringDamageChecks = false;
            m_damageable.DamageTaken += OnDamageTakenDuringDamageCheck;
            var timer = 0f;
            do
            {
                yield return null;
                timer += GameplaySystem.time.deltaTime;
            } while (timer < duration && m_wasHitDuringDamageChecks == false);
            m_damageable.DamageTaken -= OnDamageTakenDuringDamageCheck;
        }

        private void OnDamageTakenDuringDamageCheck(object sender, Damageable.DamageEventArgs eventArgs)
        {
            m_wasHitDuringDamageChecks = true;
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

            m_animation.SetAnimation(0, m_info.massiveSpikeAttackAnimation.animation, false, 0);
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

            yield return FleshBombRoutine();

            yield return IdleFloatRoutine(1f);

            for (int i = 0; i < m_multipleSpikes.Length; i++)
            {
                m_multipleSpikes[i].Disappear();
            }

            m_attackDecider.hasDecidedOnAttack = false;
            m_stateHandle.ApplyQueuedState();
        }

        private IEnumerator IllusionSpikesToOtherAttacksCombo()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            yield return IllusionSpikeRoutine();

            yield return DamageCheckRoutine(m_info.damageCheckDuration);

            if (m_wasHitDuringDamageChecks)
            {
                var flinchAnimation = m_animation.SetAnimation(0, m_info.flinchAnimation, false);
                yield return new WaitForSpineAnimationComplete(flinchAnimation);
                yield return PatternedMassiveSpikeRoutine();
                yield return IdleFloatRoutine(1f);
            }

            yield return SummonMultipleFleshSpikesRoutine(m_bottomMultipleSpike);

            var startTime = Time.time;
            float timeDifferenceSinceStart = 0;
            do
            {
                yield return FloatToDestinationRoutine(GetRandomMajorFlightArea());
                timeDifferenceSinceStart = Time.time - startTime;
            } while (timeDifferenceSinceStart <= m_info.floatTimeCheckDuration);
            yield return IdleFloatRoutine(0.1f);

            m_bottomMultipleSpike.Disappear();

            for (int i = 0; i < m_illusionPlatforms.Length; i++)
            {
                m_illusionPlatforms[i].Hide();
            }
            m_attackDecider.hasDecidedOnAttack = false;
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
                    m_attackDecider.SetList(new AttackInfo<Attack>(Attack.RainProjectiles, 0),
                                            new AttackInfo<Attack>(Attack.MassiveFleshSpikes, 0),
                                             new AttackInfo<Attack>(Attack.SummonAndScatterProjectiles, 0));
                    break;
                case Phase.PhaseTwo:
                    m_attackDecider.SetList(new AttackInfo<Attack>(Attack.RainProjectiles, 0),
                                            new AttackInfo<Attack>(Attack.MassiveFleshSpikes, 0),
                                            new AttackInfo<Attack>(Attack.SummonAndScatterProjectiles, 0));
                    break;
                case Phase.PhaseThree:
                    m_attackDecider.SetList(new AttackInfo<Attack>(Attack.RainProjectiles, 0),
                                            new AttackInfo<Attack>(Attack.MassiveFleshSpikes, 0),
                                            new AttackInfo<Attack>(Attack.SummonAndScatterProjectiles, 0),
                                            new AttackInfo<Attack>(Attack.MultipleSpikeToOtherAttacksCombo, 0));
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
                    //StopAllCoroutines();
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
                        case Attack.IllusionSpikeToOtherAttacksCombo:
                            StartCoroutine(IllusionSpikesToOtherAttacksCombo());
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
                    else if (m_phaseHandle.currentPhase != Phase.PhaseOne && m_attackCounter > m_currentPhaseInfo.atttackCountThreshold)
                    {
                        m_attackCounter = 0;
                        m_stateHandle.SetState(State.Attacking);

                        switch (m_phaseHandle.currentPhase)
                        {
                            case Phase.PhaseTwo:
                                m_attackDecider.DecideOnAttack(Attack.MultipleSpikeToOtherAttacksCombo);
                                break;
                            case Phase.PhaseThree:
                                m_attackDecider.DecideOnAttack(Attack.IllusionSpikeToOtherAttacksCombo);
                                break;
                            default:
                                //For Testing ATM
                                m_attackDecider.DecideOnAttack(Attack.IllusionSpikeToOtherAttacksCombo);
                                break;
                        }
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
