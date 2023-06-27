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

            [SerializeField]
            private MovementInfo m_moveForward = new MovementInfo();
            public MovementInfo moveForward => m_moveForward;
            [SerializeField]
            private MovementInfo m_moveBackward = new MovementInfo();
            public MovementInfo moveBackward => m_moveBackward;


            //[Title("Attack Behaviours")]
            //[SerializeField, TabGroup("BladeThrow")]
            //private SimpleAttackInfo m_attackDaggers = new SimpleAttackInfo();
            //public SimpleAttackInfo attackDaggers => m_attackDaggers;

            [TitleGroup("Pattern Ranges")]
            [SerializeField, BoxGroup("Phase 1")]
            private float m_phase1Pattern1Range;
            public float phase1Pattern1Range => m_phase1Pattern1Range;
            [SerializeField, BoxGroup("Phase 1")]
            private float m_phase1Pattern2Range;
            public float phase1Pattern2Range => m_phase1Pattern2Range;
            //[SerializeField, BoxGroup("Phase 1")]
            //private float m_phase1Pattern3Range;
            //public float phase1Pattern3Range => m_phase1Pattern3Range;
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
            //[SerializeField, BoxGroup("Phase 2")]
            //private float m_phase2Pattern5Range;
            //public float phase2Pattern5Range => m_phase2Pattern5Range;

            [TitleGroup("Attack Cooldown States")]
            [SerializeField, MinValue(0)]
            private List<float> m_phase1PatternCooldown;
            public List<float> phase1PatternCooldown => m_phase1PatternCooldown;
            [SerializeField, MinValue(0)]
            private List<float> m_phase2PatternCooldown;
            public List<float> phase2PatternCooldown => m_phase2PatternCooldown;

            [Title("Misc")]
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

            [Title("Animations")]
            [SerializeField]
            private BasicAnimationInfo m_appearAnimation;
            public BasicAnimationInfo appearAnimation => m_appearAnimation;
            [SerializeField]
            private BasicAnimationInfo m_deathAnimation;
            public BasicAnimationInfo deathAnimation => m_deathAnimation;
            [SerializeField]
            private BasicAnimationInfo m_disappearAnimation;
            public BasicAnimationInfo disappearAnimation => m_disappearAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchAnimation;
            public BasicAnimationInfo flinchAnimation => m_flinchAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchStunnedAnimation;
            public BasicAnimationInfo flinchStunnedAnimation => m_flinchStunnedAnimation;
            [SerializeField]
            private List<string> m_idleAnimations;
            public List<string> idleAnimations => m_idleAnimations;
            [SerializeField]
            private BasicAnimationInfo m_stunnedAnimation;
            public BasicAnimationInfo stunnedAnimation => m_stunnedAnimation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation;
            public BasicAnimationInfo turnAnimation => m_turnAnimation;

            [Title("Projectiles")]
            [SerializeField, BoxGroup("RainProjectiles")]
            private float m_rainProjectilesDuration;
            public float rainProjectilesDuration => m_rainProjectilesDuration;
            [SerializeField, BoxGroup("RainProjectiles")]
            private SimpleProjectileAttackInfo m_OrbSummonRainProjectile;
            public SimpleProjectileAttackInfo OrbSummonRainProjectile => m_OrbSummonRainProjectile;
            [SerializeField, BoxGroup("RainProjectiles")]
            private BasicAnimationInfo m_OrbSummonRainProjectileLoopAnimation;
            public BasicAnimationInfo OrbSummonRainProjectileLoopAnimation => m_OrbSummonRainProjectileLoopAnimation;
            [SerializeField, BoxGroup("RainProjectiles")]
            private SimpleProjectileAttackInfo m_staffPointRainProjectile;
            public SimpleProjectileAttackInfo staffPointRainProjectile => m_staffPointRainProjectile;
            [SerializeField, BoxGroup("RainProjectiles")]
            private int m_rainProjectileCount;
            public int rainProjectileCount => m_rainProjectileCount;
            [SerializeField, BoxGroup("RainProjectiles"), Min(0)]
            private Vector2 m_rainOffset;
            public Vector2 rainOffset => m_rainOffset;
            [SerializeField, BoxGroup("RainProjectiles")]
            private BasicAnimationInfo m_staffPointRainProjectileLoopAnimation;
            public BasicAnimationInfo staffPointRainProjectileLoopAnimation => m_staffPointRainProjectileLoopAnimation;
            [SerializeField, BoxGroup("RainProjectiles")]
            private BasicAnimationInfo m_staffPointToIdleRainProjectileAnimation;
            public BasicAnimationInfo staffPointToIdleRainProjectileAnimation => m_staffPointToIdleRainProjectileAnimation;
            [SerializeField, BoxGroup("SingleFleshSpike")]
            private SimpleProjectileAttackInfo m_handClenchSingleFleshSpikeProjectile;
            public SimpleProjectileAttackInfo handClenchSingleFleshSpikeProjectile => m_handClenchSingleFleshSpikeProjectile;
            [SerializeField, BoxGroup("SingleFleshSpike")]
            private float m_handClenchSingleFleshSpikeDuration;
            public float handClenchSingleFleshSpikeDuration => m_handClenchSingleFleshSpikeDuration;
            //[SerializeField, BoxGroup("SingleFleshSpike")]
            //private PuedisYnnusSpike m_handClenchSingleFleshSpike;
            //public PuedisYnnusSpike handClenchSingleFleshSpike => m_handClenchSingleFleshSpike;
            [SerializeField, BoxGroup("EnragedMultipleFleshSpike")]
            private SimpleProjectileAttackInfo m_multipleFleshSpikeProjectile;
            public SimpleProjectileAttackInfo multipleFleshSpikeProjectile => m_multipleFleshSpikeProjectile;
            [SerializeField, BoxGroup("EnragedMultipleFleshSpike")]
            private BasicAnimationInfo m_channelingMultipleFleshSpikeProjectileLoopAnimation;
            public BasicAnimationInfo channelingMultipleFleshSpikeProjectileLoopAnimation => m_channelingMultipleFleshSpikeProjectileLoopAnimation;
            [SerializeField, BoxGroup("EnragedMultipleFleshSpike")]
            private int m_multipleFleshSpikeCount;
            public int multipleFleshSpikeCount => m_multipleFleshSpikeCount;
            [SerializeField, BoxGroup("EnragedMultipleFleshSpike")]
            private int m_enragedMultipleFleshSpikeCount;
            public int enragedMultipleFleshSpikeCount => m_enragedMultipleFleshSpikeCount;
            [SerializeField, BoxGroup("EnragedMultipleFleshSpike")]
            private float m_multipleFleshSpikeDuration;
            public float multipleFleshSpikeDuration => m_multipleFleshSpikeDuration;
            [SerializeField, BoxGroup("EnragedMultipleFleshSpike"), Min(0)]
            private Vector2 m_enragedSpikeOffset;
            public Vector2 enragedSpikeOffset => m_enragedSpikeOffset;
            [SerializeField, BoxGroup("EnragedMultipleFleshSpike"), Min(0)]
            private Vector2 m_enragedSpikeCageCount;
            public Vector2 enragedSpikeCageCount => m_enragedSpikeCageCount;
            [SerializeField, BoxGroup("FleshBomb")]
            private SimpleProjectileAttackInfo m_staffSpinFleshBombProjectile;
            public SimpleProjectileAttackInfo staffSpinFleshBombProjectile => m_staffSpinFleshBombProjectile;

            //[Title("Events")]
            //[SerializeField, ValueDropdown("GetEvents")]
            //private string m_deathFXEvent;
            //public string deathFXEvent => m_deathFXEvent;
            //[SerializeField, ValueDropdown("GetEvents")]
            //private string m_teleportFXEvent;
            //public string teleportFXEvent => m_teleportFXEvent;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_moveForward.SetData(m_skeletonDataAsset);
                m_moveBackward.SetData(m_skeletonDataAsset);
                m_OrbSummonRainProjectile.SetData(m_skeletonDataAsset);
                m_staffPointRainProjectile.SetData(m_skeletonDataAsset);
                m_handClenchSingleFleshSpikeProjectile.SetData(m_skeletonDataAsset);
                m_multipleFleshSpikeProjectile.SetData(m_skeletonDataAsset);
                m_staffSpinFleshBombProjectile.SetData(m_skeletonDataAsset);

                m_appearAnimation.SetData(m_skeletonDataAsset);
                m_deathAnimation.SetData(m_skeletonDataAsset);
                m_disappearAnimation.SetData(m_skeletonDataAsset);
                m_flinchAnimation.SetData(m_skeletonDataAsset);
                m_flinchStunnedAnimation.SetData(m_skeletonDataAsset);
                m_stunnedAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_OrbSummonRainProjectileLoopAnimation.SetData(m_skeletonDataAsset);
                m_staffPointRainProjectileLoopAnimation.SetData(m_skeletonDataAsset);
                m_staffPointToIdleRainProjectileAnimation.SetData(m_skeletonDataAsset);
                m_channelingMultipleFleshSpikeProjectileLoopAnimation.SetData(m_skeletonDataAsset);

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

        private enum Attack
        {
            Phase1Pattern1,
            Phase1Pattern2,
            //Phase1Pattern3,
            Phase2Pattern1,
            Phase2Pattern2,
            Phase2Pattern3,
            Phase2Pattern4,
            EnragedAttack,
            WaitAttackEnd,
        }

        //private enum Attack
        //{
        //    OrbSummonRainProjectiles,
        //    StaffPointRainProjectiles,
        //    SingleFleshSpike,
        //    EnragedMultipleFleshSpikes,
        //    FleshBomb,
        //    WaitAttackEnd,
        //}

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
        State m_turnState;
        [ShowInInspector]
        private PhaseHandle<Phase, PhaseInfo> m_phaseHandle;
        //[ShowInInspector]
        //private RandomAttackDecider<Pattern> m_patternDecider;
        [ShowInInspector]
        private RandomAttackDecider<Attack> m_attackDecider;
        private Attack m_currentAttack;
        private float m_currentAttackRange;

        #region ProjectileLaunchers
        private ProjectileLauncher m_orbSummonRainProjectileLauncher;
        private ProjectileLauncher m_staffPointRainProjectileLauncher;
        private ProjectileLauncher m_singleFleshSpikeProjectileLauncher;
        private ProjectileLauncher m_multipleFleshSpikeProjectileLauncher;
        private ProjectileLauncher m_fleshBombProjectileLauncher;
        #endregion

        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_projectilePoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Collider2D m_randomSpawnCollider;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_rainProjectilePoint;
        private Transform m_cacheRainProjectilePoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_enragedSpikePoint;
        private Transform m_cacheEnragedSpikePoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_fleshBombPoint;
        [SerializeField, TabGroup("Spike Group")]
        private Transform m_horizontalSpikeGroup;
        [SerializeField, TabGroup("Spike Group")]
        private List<Transform> m_verticalSpikeGroups;

        private Vector2 m_lastTargetPos;
        private float m_currentCooldown;
        private float m_pickedCooldown;
        private List<float> m_currentFullCooldown;
        private int[] m_patternAttackCount;
        private List<float> m_patternCooldown;
        private int m_maxHitCount;
        private int m_currentHitCount;

        private Coroutine m_counterAttackCoroutine;
        private Coroutine m_currentAttackCoroutine;
        private Coroutine m_teleportCoroutine;

        #region Animations
        private string m_currentIdleAnimation;
        #endregion

        private bool m_isDetecting;

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
                    //m_idleAnimation = m_info.idleCombatAnimation;
                    AddToAttackCache(Attack.Phase1Pattern1, Attack.Phase1Pattern2/*, Attack.Phase1Pattern3*/);
                    AddToRangeCache(m_info.phase1Pattern1Range, m_info.phase1Pattern2Range/*, m_info.phase1Pattern3Range*/);
                    for (int i = 0; i < m_info.phase1PatternCooldown.Count; i++)
                        m_patternCooldown.Add(m_info.phase1PatternCooldown[i]);
                    break;
                case Phase.PhaseTwo:
                    //m_idleAnimation = m_info.idleCombatAnimation;
                    AddToAttackCache(Attack.Phase2Pattern1, Attack.Phase2Pattern2, Attack.Phase2Pattern3, Attack.Phase2Pattern4);
                    AddToRangeCache(m_info.phase2Pattern1Range, m_info.phase2Pattern2Range, m_info.phase2Pattern3Range, m_info.phase2Pattern4Range);
                    for (int i = 0; i < m_info.phase2PatternCooldown.Count; i++)
                        m_patternCooldown.Add(m_info.phase2PatternCooldown[i]);
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
            if (m_currentAttackCoroutine != null)
            {
                StopCoroutine(m_currentAttackCoroutine);
                m_currentAttackCoroutine = null;
                m_attackDecider.hasDecidedOnAttack = false;
            }
            if (m_counterAttackCoroutine != null)
            {
                StopCoroutine(m_counterAttackCoroutine);
                m_counterAttackCoroutine = null;
            }
            if (m_teleportCoroutine != null)
            {
                StopCoroutine(m_teleportCoroutine);
                m_teleportCoroutine = null;
            }
            enabled = true;
            StopAllCoroutines();
            m_stateHandle.OverrideState(State.Phasing);
            m_animation.DisableRootMotion();
            m_animation.SetEmptyAnimation(0, 0);
            m_phaseHandle.ApplyChange();
        }

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            //m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null && m_stateHandle.currentState == State.Idle)
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

        private void OnDamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
        {
            if (m_teleportCoroutine == null && m_counterAttackCoroutine == null)
            {
                switch (m_phaseHandle.currentPhase)
                {
                    case Phase.PhaseOne:
                        if (m_currentHitCount < m_maxHitCount)
                            m_currentHitCount++;
                        else
                        {
                            if (m_currentAttackCoroutine != null)
                            {
                                StopCoroutine(m_currentAttackCoroutine);
                                m_currentAttackCoroutine = null;
                                m_attackDecider.hasDecidedOnAttack = false;
                            }

                            m_stateHandle.Wait(State.ReevaluateSituation);

                            m_currentAttack = Attack.EnragedAttack;
                            m_teleportCoroutine = StartCoroutine(TeleportToTargetRoutine(m_randomSpawnCollider.bounds.center/*, Attack.EnragedAttack*/, Vector2.zero));
                            //m_counterAttackCoroutine = StartCoroutine(EnragedMultipleFleshSpikeRoutine());
                            m_currentHitCount = 0;
                        }
                        break;
                    case Phase.PhaseTwo:
                        if (m_currentHitCount < m_maxHitCount)
                            m_currentHitCount++;
                        else
                        {
                            if (m_currentAttackCoroutine != null)
                            {
                                StopCoroutine(m_currentAttackCoroutine);
                                m_currentAttackCoroutine = null;
                                m_attackDecider.hasDecidedOnAttack = false;
                            }

                            m_stateHandle.Wait(State.ReevaluateSituation);

                            m_currentAttack = Attack.EnragedAttack;
                            m_teleportCoroutine = StartCoroutine(TeleportToTargetRoutine(m_randomSpawnCollider.bounds.center/*, Attack.EnragedAttack*/, Vector2.zero));
                            //m_counterAttackCoroutine = StartCoroutine(EnragedMultipleFleshSpikeRoutine());
                            m_currentHitCount = 0;
                        }
                        break;
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

        private IEnumerator IntroRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_movement.Stop();
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            yield return new WaitForSeconds(2);
            m_animation.SetAnimation(0, m_info.moveForward.animation, true);
            yield return new WaitForSeconds(5);
            //GetComponentInChildren<MeshRenderer>().sortingOrder = 99;
            m_animation.SetAnimation(0, m_info.handClenchSingleFleshSpikeProjectile.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.handClenchSingleFleshSpikeProjectile.animation);
            m_currentIdleAnimation = m_info.idleAnimations[UnityEngine.Random.Range(0, m_info.idleAnimations.Count)];
            m_animation.SetAnimation(0, m_currentIdleAnimation, true);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private void ResetCounts()
        {
            m_currentHitCount = 0;
        }

        private IEnumerator ChangePhaseRoutine()
        {
            enabled = false;
            //m_hitbox.SetCanBlockDamageState(false);
            m_hitbox.Disable();
            m_stateHandle.Wait(State.Chasing);
            m_rainProjectilePoint = m_cacheRainProjectilePoint;
            ResetCounts();
            m_animation.SetAnimation(0, m_info.disappearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.disappearAnimation);
            transform.position = new Vector2(m_randomSpawnCollider.bounds.center.x, m_randomSpawnCollider.bounds.center.y - 5);
            m_animation.SetAnimation(0, m_info.appearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.appearAnimation);

            m_hitbox.Enable();
            m_stateHandle.ApplyQueuedState();
            yield return null;
            enabled = true;
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            StopAllCoroutines();
            m_movement.Stop();
            m_isDetecting = false;
        }

        #region Attacks

        //public void AimAt(Vector2 target)
        //{
        //    Vector2 spitPos = m_projectilePoint.position;
        //    Vector3 v_diff = (target - spitPos);
        //    float atan2 = Mathf.Atan2(v_diff.y, v_diff.x);
        //    m_projectilePoint.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg);
        //}

        private void LaunchSingleSpike()
        {
            m_lastTargetPos = m_targetInfo.position;
            LaunchSpike(PuedisYnnusSpike.SkinType.Big, false, Quaternion.identity, true);
        }

        private void LaunchSpike(PuedisYnnusSpike.SkinType spikeType, bool isRandom, Quaternion rotation, bool useOffset)
        {
            var randomOffsetX = UnityEngine.Random.Range(10, 20);
            randomOffsetX = UnityEngine.Random.Range(0, 2) == 1 ? randomOffsetX : randomOffsetX * -1;
            var targetPos = useOffset ? new Vector2(m_lastTargetPos.x + randomOffsetX, GroundPosition(m_lastTargetPos).y) : m_lastTargetPos;

            var component = Instantiate(m_info.handClenchSingleFleshSpikeProjectile.projectileInfo.projectile, targetPos, Quaternion.identity);
            component.GetComponent<PuedisYnnusSpike>().WillPool(false);
            component.GetComponent<PuedisYnnusSpike>().RandomScale();
            component.GetComponent<PuedisYnnusSpike>().SetSkin(spikeType, isRandom);

            component.transform.rotation = rotation;
            var middleSpawn = UnityEngine.Random.Range(0, 2) == 1 ? true : false;
            if (middleSpawn)
            {
                component.transform.position = new Vector2(m_lastTargetPos.x, GroundPosition(m_lastTargetPos).y);
                component.GetComponent<PuedisYnnusSpike>().MassiveSpikeSpawn(m_info.handClenchSingleFleshSpikeDuration);
            }
            else
            {
                if (component.transform.position.x < m_lastTargetPos.x)
                    component.GetComponent<PuedisYnnusSpike>().RightSpikeSpawn(m_info.handClenchSingleFleshSpikeDuration);
                else
                    component.GetComponent<PuedisYnnusSpike>().LeftSpikeSpawn(m_info.handClenchSingleFleshSpikeDuration);
            }
        }

        private void LaunchFleshBomb()
        {
            if (!IsFacingTarget())
                CustomTurn();

            m_fleshBombProjectileLauncher.AimAt(m_targetInfo.position);
            m_fleshBombProjectileLauncher.LaunchProjectile();
        }

        private IEnumerator RainProjectilesRoutine()
        {
            m_animation.SetAnimation(0, m_info.staffPointRainProjectile.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.staffPointRainProjectile.animation);
            m_animation.SetAnimation(0, m_info.staffPointRainProjectileLoopAnimation, true);
            var aimPoint = new Vector2(m_targetInfo.position.x, GroundPosition(m_targetInfo.position).y);
            var offsetLeftAimPoint = aimPoint;
            var offsetRightAimPoint = aimPoint;
            for (int i = 0; i < m_info.rainProjectileCount; i++)
            {
                aimPoint = new Vector2(aimPoint.x + UnityEngine.Random.Range(-10, 10), aimPoint.y);
                m_rainProjectilePoint.position = new Vector2(aimPoint.x, m_rainProjectilePoint.position.y);
                m_staffPointRainProjectileLauncher.AimAt(aimPoint);
                m_staffPointRainProjectileLauncher.LaunchProjectile();
                for (int x = 0; x < m_info.rainProjectileCount; x++)
                {
                    m_rainProjectilePoint.position = new Vector2(offsetLeftAimPoint.x, m_rainProjectilePoint.position.y);
                    m_staffPointRainProjectileLauncher.AimAt(offsetLeftAimPoint);
                    m_staffPointRainProjectileLauncher.LaunchProjectile();
                    offsetLeftAimPoint = new Vector2(offsetLeftAimPoint.x - UnityEngine.Random.Range(m_info.rainOffset.x, m_info.rainOffset.y), offsetLeftAimPoint.y);
                }
                m_rainProjectilePoint.position = m_cacheRainProjectilePoint.position;
                for (int y = 0; y < m_info.rainProjectileCount; y++)
                {
                    m_rainProjectilePoint.position = new Vector2(offsetRightAimPoint.x, m_rainProjectilePoint.position.y);
                    m_staffPointRainProjectileLauncher.AimAt(offsetRightAimPoint);
                    m_staffPointRainProjectileLauncher.LaunchProjectile();
                    offsetRightAimPoint = new Vector2(offsetRightAimPoint.x + UnityEngine.Random.Range(m_info.rainOffset.x, m_info.rainOffset.y), offsetRightAimPoint.y);
                }
                yield return new WaitForSeconds(m_info.rainProjectilesDuration);
                m_rainProjectilePoint.position = m_cacheRainProjectilePoint.position;
                aimPoint = new Vector2(m_targetInfo.position.x, GroundPosition(m_targetInfo.position).y);
                offsetLeftAimPoint = aimPoint;
                offsetRightAimPoint = aimPoint;
            }
            m_rainProjectilePoint.position = m_cacheRainProjectilePoint.position;
            m_animation.SetAnimation(0, m_info.staffPointToIdleRainProjectileAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.staffPointToIdleRainProjectileAnimation);
            m_currentIdleAnimation = m_info.idleAnimations[UnityEngine.Random.Range(0, m_info.idleAnimations.Count)];
            m_animation.SetAnimation(0, m_currentIdleAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator SingleFleshSpikeRoutine()
        {
            m_animation.SetAnimation(0, m_info.handClenchSingleFleshSpikeProjectile.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.handClenchSingleFleshSpikeProjectile.animation);
            m_currentIdleAnimation = m_info.idleAnimations[UnityEngine.Random.Range(0, m_info.idleAnimations.Count)];
            m_animation.SetAnimation(0, m_currentIdleAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator FleshBombRoutine()
        {
            m_animation.SetAnimation(0, m_info.staffSpinFleshBombProjectile.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.staffSpinFleshBombProjectile.animation);
            m_currentIdleAnimation = m_info.idleAnimations[UnityEngine.Random.Range(0, m_info.idleAnimations.Count)];
            m_animation.SetAnimation(0, m_currentIdleAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator MultipleFleshSpikeRoutine()
        {
            m_animation.SetAnimation(0, m_info.multipleFleshSpikeProjectile.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.multipleFleshSpikeProjectile.animation);
            m_animation.SetAnimation(0, m_info.channelingMultipleFleshSpikeProjectileLoopAnimation, true);
            var aimPoint = new Vector2(m_targetInfo.position.x, GroundPosition(m_targetInfo.position).y);
            var offsetLeftAimPoint = aimPoint;
            var offsetRightAimPoint = aimPoint;
            for (int i = 0; i < m_info.multipleFleshSpikeCount; i++)
            {
                aimPoint = new Vector2(aimPoint.x + UnityEngine.Random.Range(-10, 10), aimPoint.y);
                m_lastTargetPos = aimPoint;
                LaunchSpike(PuedisYnnusSpike.SkinType.Small, true, Quaternion.identity, true);
                for (int x = 0; x < m_info.multipleFleshSpikeCount; x++)
                {
                    m_lastTargetPos = offsetLeftAimPoint;
                    LaunchSpike(PuedisYnnusSpike.SkinType.Small, true, Quaternion.identity, true);
                    offsetLeftAimPoint = new Vector2(offsetLeftAimPoint.x - UnityEngine.Random.Range(m_info.enragedSpikeOffset.x, m_info.enragedSpikeOffset.y), offsetLeftAimPoint.y);
                    m_lastTargetPos = offsetRightAimPoint;
                    LaunchSpike(PuedisYnnusSpike.SkinType.Small, true, Quaternion.identity, true);
                    offsetRightAimPoint = new Vector2(offsetRightAimPoint.x + UnityEngine.Random.Range(m_info.enragedSpikeOffset.x, m_info.enragedSpikeOffset.y), offsetRightAimPoint.y);
                    yield return new WaitForSeconds(0.2f);
                }
                m_enragedSpikePoint.position = m_cacheEnragedSpikePoint.position;
                yield return new WaitForSeconds(m_info.multipleFleshSpikeDuration);
                m_enragedSpikePoint.position = m_cacheEnragedSpikePoint.position;
                aimPoint = new Vector2(m_targetInfo.position.x, GroundPosition(m_randomSpawnCollider.bounds.center).y);
                offsetLeftAimPoint = aimPoint;
                offsetRightAimPoint = aimPoint;
            }
            m_enragedSpikePoint.position = m_cacheEnragedSpikePoint.position;
            m_currentIdleAnimation = m_info.idleAnimations[UnityEngine.Random.Range(0, m_info.idleAnimations.Count)];
            m_animation.SetAnimation(0, m_currentIdleAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
            enabled = true;
        }

        private IEnumerator EnragedMultipleFleshSpikeRoutine()
        {
            enabled = false;
            m_animation.SetAnimation(0, m_info.multipleFleshSpikeProjectile.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.multipleFleshSpikeProjectile.animation);
            m_animation.SetAnimation(0, m_info.channelingMultipleFleshSpikeProjectileLoopAnimation, true);
            var aimPoint = new Vector2(m_targetInfo.position.x, GroundPosition(m_targetInfo.position).y);
            var offsetLeftAimPoint = aimPoint;
            var offsetRightAimPoint = aimPoint;
            for (int i = 0; i < m_info.enragedMultipleFleshSpikeCount; i++)
            {
                aimPoint = new Vector2(aimPoint.x + UnityEngine.Random.Range(-10, 10), aimPoint.y);
                m_lastTargetPos = aimPoint;
                LaunchSpike(PuedisYnnusSpike.SkinType.Small, true, Quaternion.identity, true);
                for (int x = 0; x < m_info.enragedMultipleFleshSpikeCount; x++)
                {
                    m_lastTargetPos = offsetLeftAimPoint;
                    LaunchSpike(PuedisYnnusSpike.SkinType.Small, true, Quaternion.identity, true);
                    offsetLeftAimPoint = new Vector2(offsetLeftAimPoint.x - UnityEngine.Random.Range(m_info.enragedSpikeOffset.x, m_info.enragedSpikeOffset.y), offsetLeftAimPoint.y);
                    m_lastTargetPos = offsetRightAimPoint;
                    LaunchSpike(PuedisYnnusSpike.SkinType.Small, true, Quaternion.identity, true);
                    offsetRightAimPoint = new Vector2(offsetRightAimPoint.x + UnityEngine.Random.Range(m_info.enragedSpikeOffset.x, m_info.enragedSpikeOffset.y), offsetRightAimPoint.y);
                    yield return new WaitForSeconds(0.2f);
                }
                m_enragedSpikePoint.position = m_cacheEnragedSpikePoint.position;
                yield return new WaitForSeconds(m_info.multipleFleshSpikeDuration);
                m_enragedSpikePoint.position = m_cacheEnragedSpikePoint.position;
                aimPoint = new Vector2(m_targetInfo.position.x, GroundPosition(m_randomSpawnCollider.bounds.center).y);
                offsetLeftAimPoint = aimPoint;
                offsetRightAimPoint = aimPoint;
            }
            m_enragedSpikePoint.position = m_cacheEnragedSpikePoint.position;
            aimPoint = m_horizontalSpikeGroup.position;
            offsetLeftAimPoint = aimPoint;
            offsetRightAimPoint = aimPoint;
            m_lastTargetPos = aimPoint;
            LaunchSpike(PuedisYnnusSpike.SkinType.Big, false, Quaternion.identity, true);
            for (int i = 0; i < m_info.enragedSpikeCageCount.x; i++)
            {
                m_lastTargetPos = offsetLeftAimPoint;
                LaunchSpike(PuedisYnnusSpike.SkinType.Big, false, Quaternion.identity, true);
                offsetLeftAimPoint = new Vector2(offsetLeftAimPoint.x - 20, offsetLeftAimPoint.y);
                m_lastTargetPos = offsetRightAimPoint;
                LaunchSpike(PuedisYnnusSpike.SkinType.Big, false, Quaternion.identity, true);
                offsetRightAimPoint = new Vector2(offsetRightAimPoint.x + 20, offsetRightAimPoint.y);
            }
            yield return new WaitForSeconds(m_info.multipleFleshSpikeDuration);
            aimPoint = m_verticalSpikeGroups[0].position;
            offsetLeftAimPoint = aimPoint;
            offsetRightAimPoint = aimPoint;
            m_lastTargetPos = aimPoint;
            LaunchSpike(PuedisYnnusSpike.SkinType.Big, false, Quaternion.Euler(0, 0, 270f), false);
            for (int i = 0; i < m_info.enragedSpikeCageCount.y; i++)
            {
                m_lastTargetPos = offsetLeftAimPoint;
                LaunchSpike(PuedisYnnusSpike.SkinType.Big, false, Quaternion.Euler(0, 0, 270f), false);
                offsetLeftAimPoint = new Vector2(offsetLeftAimPoint.x, offsetLeftAimPoint.y - 20);
                m_lastTargetPos = offsetRightAimPoint;
                LaunchSpike(PuedisYnnusSpike.SkinType.Big, false, Quaternion.Euler(0, 0, 270f), false);
                offsetRightAimPoint = new Vector2(offsetRightAimPoint.x, offsetRightAimPoint.y + 20);
            }
            aimPoint = m_verticalSpikeGroups[1].position;
            offsetLeftAimPoint = aimPoint;
            offsetRightAimPoint = aimPoint;
            m_lastTargetPos = aimPoint;
            LaunchSpike(PuedisYnnusSpike.SkinType.Big, false, Quaternion.Euler(0, 0, 90f), false);
            for (int i = 0; i < m_info.enragedSpikeCageCount.y; i++)
            {
                m_lastTargetPos = offsetLeftAimPoint;
                LaunchSpike(PuedisYnnusSpike.SkinType.Big, false, Quaternion.Euler(0, 0, 90f), false);
                offsetLeftAimPoint = new Vector2(offsetLeftAimPoint.x, offsetLeftAimPoint.y - 20);
                m_lastTargetPos = offsetRightAimPoint;
                LaunchSpike(PuedisYnnusSpike.SkinType.Big, false, Quaternion.Euler(0, 0, 90f), false);
                offsetRightAimPoint = new Vector2(offsetRightAimPoint.x, offsetRightAimPoint.y + 20);
            }
            //m_horizontalSpikeGroup.PlayRandomSpikeGroup(m_info.handClenchSingleFleshSpikeDuration);
            //yield return new WaitForSeconds(m_info.multipleFleshSpikeDuration);
            //for (int i = 0; i < m_verticalSpikeGroups.Count; i++)
            //{
            //    m_verticalSpikeGroups[i].PlayRandomSpikeGroup(m_info.handClenchSingleFleshSpikeDuration);
            //}
            yield return new WaitForSeconds(m_info.multipleFleshSpikeDuration);
            m_currentIdleAnimation = m_info.idleAnimations[UnityEngine.Random.Range(0, m_info.idleAnimations.Count)];
            m_animation.SetAnimation(0, m_currentIdleAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            //m_hitbox.SetCanBlockDamageState(false);
            m_counterAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
            enabled = true;
        }
        #endregion

        #region Movement
        private IEnumerator TeleportToTargetRoutine(Vector2 target/*, Attack attack*/, Vector2 positionOffset/*, bool isGrounded*/)
        {
            m_stateHandle.Wait(State.Attacking);
            m_hitbox.Disable();
            m_animation.SetAnimation(0, m_info.disappearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.disappearAnimation);
            transform.position = (Vector2)RandomTeleportPoint();
            //transform.position = new Vector2(target.x + (m_targetInfo.transform.GetComponent<Character>().facing == HorizontalDirection.Right ? -positionOffset.x : positionOffset.x), /*isGrounded ? GroundPosition().y :*/ target.y + positionOffset.y);
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            m_hitbox.Enable();
            m_animation.SetAnimation(0, m_info.appearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.appearAnimation);
            m_teleportCoroutine = null;
            //if (attack == Attack.WaitAttackEnd)
            //{
            //    m_currentIdleAnimation = m_info.idleAnimations[UnityEngine.Random.Range(0, m_info.idleAnimations.Count)];
            //    m_animation.SetAnimation(0, m_currentIdleAnimation, true);
            //    m_stateHandle.ApplyQueuedState();
            //}
            //else
            //{
            //    //ExecuteAttack(attack);
            //    m_stateHandle.ApplyQueuedState();
            //}
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private Vector3 RandomTeleportPoint()
        {
            Vector3 randomPos = transform.position;
            while (Vector2.Distance(transform.position, randomPos) <= 50f)
            {
                randomPos = m_randomSpawnCollider.bounds.center + new Vector3(
               (UnityEngine.Random.value - 0.5f) * m_randomSpawnCollider.bounds.size.x,
               (UnityEngine.Random.value - 0.5f) * m_randomSpawnCollider.bounds.size.y,
               (UnityEngine.Random.value - 0.5f) * m_randomSpawnCollider.bounds.size.z);
            }
            return randomPos;
        }
        #endregion

        //private void DecidedOnAttack(bool condition)
        //{
        //    m_attackDecider.hasDecidedOnAttack = condition;
        //}

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Phase1Pattern1, m_info.phase1Pattern1Range),
                                     new AttackInfo<Attack>(Attack.Phase1Pattern2, m_info.phase1Pattern2Range),
                                     //new AttackInfo<Attack>(Attack.Phase1Pattern3, m_info.phase1Pattern3Range),
                                     new AttackInfo<Attack>(Attack.Phase2Pattern1, m_info.phase2Pattern1Range),
                                     new AttackInfo<Attack>(Attack.Phase2Pattern2, m_info.phase2Pattern2Range),
                                     new AttackInfo<Attack>(Attack.Phase2Pattern3, m_info.phase2Pattern3Range),
                                     new AttackInfo<Attack>(Attack.Phase2Pattern4, m_info.phase2Pattern4Range));
            //m_attackDecider.SetList(new AttackInfo<Attack>(Attack.OrbSummonRainProjectiles, m_info.OrbSummonRainProjectile.range)
            //                      , new AttackInfo<Attack>(Attack.StaffPointRainProjectiles, m_info.staffPointRainProjectile.range)
            //                      , new AttackInfo<Attack>(Attack.SingleFleshSpike, m_info.handClenchSingleFleshSpikeProjectile.range)
            //                      , new AttackInfo<Attack>(Attack.EnragedMultipleFleshSpikes, m_info.multipleFleshSpikeProjectile.range)
            //                      , new AttackInfo<Attack>(Attack.FleshBomb, m_info.staffSpinFleshBombProjectile.range));
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

        //private void ExecuteAttack(Attack m_attack)
        //{
        //    var randomFacing = UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1;
        //    switch (m_attack)
        //    {
        //        case Attack.OrbSummonRainProjectiles:

        //            break;
        //        case Attack.StaffPointRainProjectiles:

        //            break;
        //        case Attack.SingleFleshSpike:

        //            break;
        //        case Attack.EnragedMultipleFleshSpikes:

        //            break;
        //        case Attack.FleshBomb:

        //            break;
        //    }
        //}

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
            m_turnHandle.TurnDone += OnTurnDone;
            //m_hitDetector.PlayerHit += AddHitCount;
            m_deathHandle.SetAnimation(m_info.deathAnimation.animation);
            m_orbSummonRainProjectileLauncher = new ProjectileLauncher(m_info.OrbSummonRainProjectile.projectileInfo, m_projectilePoint);
            m_staffPointRainProjectileLauncher = new ProjectileLauncher(m_info.staffPointRainProjectile.projectileInfo, m_rainProjectilePoint);
            m_singleFleshSpikeProjectileLauncher = new ProjectileLauncher(m_info.handClenchSingleFleshSpikeProjectile.projectileInfo, m_projectilePoint);
            m_multipleFleshSpikeProjectileLauncher = new ProjectileLauncher(m_info.multipleFleshSpikeProjectile.projectileInfo, m_enragedSpikePoint);
            m_fleshBombProjectileLauncher = new ProjectileLauncher(m_info.staffSpinFleshBombProjectile.projectileInfo, m_fleshBombPoint);
            m_patternAttackCount = new int[2];
            //m_patternDecider = new RandomAttackDecider<Pattern>();
            m_damageable.DamageTaken += OnDamageTaken;
            m_attackDecider = new RandomAttackDecider<Attack>();
            //for (int i = 0; i < 3; i++)
            //{
            //    m_attackDecider[i] = new RandomAttackDecider<Attack>();
            //}
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            UpdateAttackDeciderList();
            //m_patternCount = new float[4];
            m_attackCache = new List<Attack>();
            AddToAttackCache(Attack.Phase1Pattern1, Attack.Phase1Pattern2/*, Attack.Phase1Pattern3*/, Attack.Phase2Pattern1, Attack.Phase2Pattern2, Attack.Phase2Pattern3, Attack.Phase2Pattern4);
            m_attackRangeCache = new List<float>();
            AddToRangeCache(m_info.phase1Pattern1Range, m_info.phase1Pattern2Range/*, m_info.phase1Pattern3Range*/, m_info.phase2Pattern1Range, m_info.phase2Pattern2Range, m_info.phase2Pattern3Range);
            m_attackUsed = new bool[m_attackCache.Count];
            m_currentFullCooldown = new List<float>();
            m_patternCooldown = new List<float>();
        }

        protected override void Start()
        {
            base.Start();
            //m_spineListener.Subscribe(m_info.OrbSummonRainProjectile.launchOnEvent, m_deathFX.Play);
            //m_spineListener.Subscribe(m_info.staffPointRainProjectile.launchOnEvent, m_deathFX.Play);
            m_spineListener.Subscribe(m_info.handClenchSingleFleshSpikeProjectile.launchOnEvent, LaunchSingleSpike);
            //m_spineListener.Subscribe(m_info.multipleFleshSpikeProjectile.launchOnEvent, LaunchFleshBomb);
            m_spineListener.Subscribe(m_info.staffSpinFleshBombProjectile.launchOnEvent, LaunchFleshBomb);

            m_cacheRainProjectilePoint = m_rainProjectilePoint;
            m_cacheEnragedSpikePoint = m_enragedSpikePoint;

            m_animation.DisableRootMotion();
            m_currentIdleAnimation = m_info.idleAnimations[UnityEngine.Random.Range(0, m_info.idleAnimations.Count)];

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
                    m_animation.SetAnimation(0, m_currentIdleAnimation, true);
                    break;
                case State.Intro:
                    if (IsFacingTarget())
                    {
                        //StartCoroutine(IntroRoutine());
                        m_stateHandle.OverrideState(State.Chasing);
                    }
                    else
                    {
                        m_turnState = State.Intro;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    break;
                case State.Phasing:
                    Debug.Log("Phase Time");
                    StartCoroutine(ChangePhaseRoutine());
                    break;
                case State.Turning:
                    Debug.Log("Turning Steet");
                    m_stateHandle.Wait(m_turnState);
                    StopAllCoroutines();
                    m_currentIdleAnimation = m_info.idleAnimations[UnityEngine.Random.Range(0, m_info.idleAnimations.Count)];
                    m_turnHandle.Execute(m_info.turnAnimation.animation, m_currentIdleAnimation);
                    m_movement.Stop();
                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    m_lastTargetPos = m_targetInfo.position;

                    switch (m_currentAttack)
                    {
                        case Attack.Phase1Pattern1:
                            m_currentAttackCoroutine = StartCoroutine(RainProjectilesRoutine());
                            m_pickedCooldown = m_currentFullCooldown[0];
                            break;
                        case Attack.Phase1Pattern2:
                            m_currentAttackCoroutine = StartCoroutine(SingleFleshSpikeRoutine());
                            m_pickedCooldown = m_currentFullCooldown[1];
                            break;
                        //case Attack.Phase1Pattern3:
                        //    m_attackRoutine = StartCoroutine(RainProjectilesRoutine());
                        //    m_pickedCooldown = m_currentFullCooldown[3];
                        //    break;
                        case Attack.Phase2Pattern1:
                            m_currentAttackCoroutine = StartCoroutine(RainProjectilesRoutine());
                            m_pickedCooldown = m_currentFullCooldown[0];
                            break;
                        case Attack.Phase2Pattern2:
                            m_currentAttackCoroutine = StartCoroutine(SingleFleshSpikeRoutine());
                            m_pickedCooldown = m_currentFullCooldown[1];
                            break;
                        case Attack.Phase2Pattern3:
                            m_currentAttackCoroutine = StartCoroutine(FleshBombRoutine());
                            m_pickedCooldown = m_currentFullCooldown[2];
                            break;
                        case Attack.Phase2Pattern4:
                            m_currentAttackCoroutine = StartCoroutine(MultipleFleshSpikeRoutine());
                            m_pickedCooldown = m_currentFullCooldown[3];
                            break;
                        case Attack.EnragedAttack:
                            m_counterAttackCoroutine = StartCoroutine(EnragedMultipleFleshSpikeRoutine());
                            break;
                    }
                    break;

                case State.Cooldown:
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        m_animation.SetAnimation(0, m_currentIdleAnimation, true);
                    }

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
                    ChooseAttack();
                    if (IsFacingTarget())
                    {
                        if (m_attackDecider.hasDecidedOnAttack)
                        {
                            var randomOffsetX = UnityEngine.Random.Range(10, 20);
                            randomOffsetX = UnityEngine.Random.Range(0, 2) == 1 ? randomOffsetX : randomOffsetX * -1;
                            m_teleportCoroutine = StartCoroutine(TeleportToTargetRoutine(m_targetInfo.position/*, m_currentAttack*/, new Vector2(randomOffsetX, 10)));
                        }
                    }
                    else
                    {
                        m_turnState = State.Chasing;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation /*&& m_animation.GetCurrentAnimation(0).ToString() != m_info.attackDaggersIdle.animation*/)
                            m_stateHandle.SetState(State.Turning);
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