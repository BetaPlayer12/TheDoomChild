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
using DChild.Gameplay.Projectiles;

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
            [SerializeField, BoxGroup("Phase 2")]
            private float m_phase2Pattern5Range;
            public float phase2Pattern5Range => m_phase2Pattern5Range;

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
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_appearAnimation;
            public string appearAnimation => m_appearAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_disappearAnimation;
            public string disappearAnimation => m_disappearAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchStunnedAnimation;
            public string flinchStunnedAnimation => m_flinchStunnedAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private List<string> m_idleAnimations;
            public List<string> idleAnimations => m_idleAnimations;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_stunnedAnimation;
            public string stunnedAnimation => m_stunnedAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;

            [Title("Projectiles")]
            [SerializeField, BoxGroup("RainProjectiles")]
            private float m_rainProjectilesDuration;
            public float rainProjectilesDuration => m_rainProjectilesDuration;
            [SerializeField, BoxGroup("RainProjectiles")]
            private SimpleProjectileAttackInfo m_OrbSummonRainProjectile;
            public SimpleProjectileAttackInfo OrbSummonRainProjectile => m_OrbSummonRainProjectile;
            [SerializeField, BoxGroup("RainProjectiles"), ValueDropdown("GetAnimations")]
            private string m_OrbSummonRainProjectileLoopAnimation;
            public string OrbSummonRainProjectileLoopAnimation => m_OrbSummonRainProjectileLoopAnimation;
            [SerializeField, BoxGroup("RainProjectiles")]
            private SimpleProjectileAttackInfo m_staffPointRainProjectile;
            public SimpleProjectileAttackInfo staffPointRainProjectile => m_staffPointRainProjectile;
            [SerializeField, BoxGroup("RainProjectiles"), ValueDropdown("GetAnimations")]
            private string m_staffPointRainProjectileLoopAnimation;
            public string staffPointRainProjectileLoopAnimation => m_staffPointRainProjectileLoopAnimation;
            [SerializeField, BoxGroup("RainProjectiles"), ValueDropdown("GetAnimations")]
            private string m_staffPointToIdleRainProjectileAnimation;
            public string staffPointToIdleRainProjectileAnimation => m_staffPointToIdleRainProjectileAnimation;
            [SerializeField, BoxGroup("SingleFleshSpike")]
            private SimpleProjectileAttackInfo m_handClenchSingleFleshSpikeProjectile;
            public SimpleProjectileAttackInfo handClenchSingleFleshSpikeProjectile => m_handClenchSingleFleshSpikeProjectile;
            [SerializeField, BoxGroup("EnragedMultipleFleshSpike")]
            private SimpleProjectileAttackInfo m_multipleFleshSpikeProjectile;
            public SimpleProjectileAttackInfo multipleFleshSpikeProjectile => m_multipleFleshSpikeProjectile;
            [SerializeField, BoxGroup("EnragedMultipleFleshSpike"), ValueDropdown("GetAnimations")]
            private string m_channelingMultipleFleshSpikeProjectileLoopAnimation;
            public string channelingMultipleFleshSpikeProjectileLoopAnimation => m_channelingMultipleFleshSpikeProjectileLoopAnimation;
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
#endif
            }
        }

        [System.Serializable]
        public class PhaseInfo : IPhaseInfo
        {
            [SerializeField]
            private List<float> m_fullCooldown;
            public List<float> fullCooldown => m_fullCooldown;
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
            Phase2Pattern5,
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

        private int m_currentPhaseIndex;
        private int m_attackCount;
        private int m_hitCount;
        private float m_currentCooldown;
        private float m_pickedCooldown;
        private List<float> m_currentFullCooldown;
        private int[] m_patternAttackCount;
        private List<float> m_patternCooldown;

        private Coroutine m_attackRoutine;

        #region Animations
        private string m_currentIdleAnimation;
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
                    //m_idleAnimation = m_info.idleCombatAnimation;
                    AddToAttackCache(Attack.Phase1Pattern1, Attack.Phase1Pattern2/*, Attack.Phase1Pattern3*/);
                    AddToRangeCache(m_info.phase1Pattern1Range, m_info.phase1Pattern2Range/*, m_info.phase1Pattern3Range*/);
                    for (int i = 0; i < m_info.phase1PatternCooldown.Count; i++)
                        m_patternCooldown.Add(m_info.phase1PatternCooldown[i]);
                    break;
                case Phase.PhaseTwo:
                    //m_idleAnimation = m_info.idleCombatAnimation;
                    AddToAttackCache(Attack.Phase2Pattern1, Attack.Phase2Pattern2, Attack.Phase2Pattern3, Attack.Phase2Pattern4, Attack.Phase2Pattern5);
                    AddToRangeCache(m_info.phase2Pattern1Range, m_info.phase2Pattern2Range, m_info.phase2Pattern3Range, m_info.phase2Pattern5Range);
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
                m_stateHandle.OverrideState(State.Intro);
                GameEventMessage.SendEvent("Boss Encounter");
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

        private IEnumerator ChangePhaseRoutine()
        {
            enabled = false;
            m_stateHandle.Wait(State.Chasing);
            m_hitbox.Disable();
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
        }

        #region Attacks

        private void LaunchSingleSpike()
        {
            var randomOffsetX = UnityEngine.Random.Range(10, 20);
            randomOffsetX = UnityEngine.Random.Range(0, 2) == 1 ? randomOffsetX : randomOffsetX * -1;
            var targetPos = new Vector2(m_targetInfo.position.x + randomOffsetX, GroundPosition(m_targetInfo.position).y);
            m_singleFleshSpikeProjectileLauncher.AimAt(targetPos);
            m_singleFleshSpikeProjectileLauncher.LaunchProjectile();
        }
        
        private IEnumerator RainProjectilesRoutine()
        {
            m_animation.SetAnimation(0, m_info.staffPointRainProjectile.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.staffPointRainProjectile.animation);
            m_animation.SetAnimation(0, m_info.staffPointRainProjectileLoopAnimation, true);
            yield return new WaitForSeconds(m_info.rainProjectilesDuration);
            m_animation.SetAnimation(0, m_info.staffPointToIdleRainProjectileAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.staffPointToIdleRainProjectileAnimation);
            m_currentIdleAnimation = m_info.idleAnimations[UnityEngine.Random.Range(0, m_info.idleAnimations.Count)];
            m_animation.SetAnimation(0, m_currentIdleAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            m_attackRoutine = null;
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
            m_attackRoutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        #endregion

        #region Movement
        private IEnumerator TeleportToTargetRoutine(Vector2 target, Attack attack, Vector2 positionOffset/*, bool isGrounded*/)
        {
            m_stateHandle.Wait(State.Attacking);
            m_animation.SetAnimation(0, m_info.disappearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.disappearAnimation);
            transform.position = new Vector2(target.x + (m_targetInfo.transform.GetComponent<Character>().facing == HorizontalDirection.Right ? -positionOffset.x : positionOffset.x), /*isGrounded ? GroundPosition().y :*/ target.y + positionOffset.y);
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            m_animation.SetAnimation(0, m_info.appearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.appearAnimation);
            if (attack == Attack.WaitAttackEnd)
            {
                m_currentIdleAnimation = m_info.idleAnimations[UnityEngine.Random.Range(0, m_info.idleAnimations.Count)];
                m_animation.SetAnimation(0, m_currentIdleAnimation, true);
                m_stateHandle.ApplyQueuedState();
            }
            else
            {
                //ExecuteAttack(attack);
                m_stateHandle.ApplyQueuedState();
            }
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

        private bool AllowAttack(int phaseIndex, State state)
        {
            if (m_currentPhaseIndex >= phaseIndex)
            {
                return true;
            }
            else
            {
                DecidedOnAttack(false);
                m_stateHandle.OverrideState(state);
                return false;
            }
        }

        private void DecidedOnAttack(bool condition)
        {
            m_attackDecider.hasDecidedOnAttack = condition;
            //m_attackDecider.hasDecidedOnAttack = condition;
        }

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Phase1Pattern1, m_info.phase1Pattern1Range),
                                     new AttackInfo<Attack>(Attack.Phase1Pattern2, m_info.phase1Pattern2Range),
                                     //new AttackInfo<Attack>(Attack.Phase1Pattern3, m_info.phase1Pattern3Range),
                                     new AttackInfo<Attack>(Attack.Phase2Pattern1, m_info.phase2Pattern1Range),
                                     new AttackInfo<Attack>(Attack.Phase2Pattern2, m_info.phase2Pattern2Range),
                                     new AttackInfo<Attack>(Attack.Phase2Pattern3, m_info.phase2Pattern3Range),
                                     new AttackInfo<Attack>(Attack.Phase2Pattern4, m_info.phase2Pattern4Range),
                                     new AttackInfo<Attack>(Attack.Phase2Pattern5, m_info.phase2Pattern5Range));
            //m_attackDecider.SetList(new AttackInfo<Attack>(Attack.OrbSummonRainProjectiles, m_info.OrbSummonRainProjectile.range)
            //                      , new AttackInfo<Attack>(Attack.StaffPointRainProjectiles, m_info.staffPointRainProjectile.range)
            //                      , new AttackInfo<Attack>(Attack.SingleFleshSpike, m_info.handClenchSingleFleshSpikeProjectile.range)
            //                      , new AttackInfo<Attack>(Attack.EnragedMultipleFleshSpikes, m_info.multipleFleshSpikeProjectile.range)
            //                      , new AttackInfo<Attack>(Attack.FleshBomb, m_info.staffSpinFleshBombProjectile.range));
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

        private void AddHitCount(object sender, EventActionArgs eventArgs)
        {
            m_hitCount++;
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
            m_turnHandle.TurnDone += OnTurnDone;
            //m_hitDetector.PlayerHit += AddHitCount;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_orbSummonRainProjectileLauncher = new ProjectileLauncher(m_info.OrbSummonRainProjectile.projectileInfo, m_projectilePoint);
            m_staffPointRainProjectileLauncher = new ProjectileLauncher(m_info.staffPointRainProjectile.projectileInfo, m_projectilePoint);
            m_singleFleshSpikeProjectileLauncher = new ProjectileLauncher(m_info.handClenchSingleFleshSpikeProjectile.projectileInfo, m_projectilePoint);
            m_multipleFleshSpikeProjectileLauncher = new ProjectileLauncher(m_info.multipleFleshSpikeProjectile.projectileInfo, m_projectilePoint);
            m_fleshBombProjectileLauncher = new ProjectileLauncher(m_info.staffSpinFleshBombProjectile.projectileInfo, m_projectilePoint);
            m_patternAttackCount = new int[2];
            //m_patternDecider = new RandomAttackDecider<Pattern>();
            m_attackDecider = new RandomAttackDecider<Attack>();
            //for (int i = 0; i < 3; i++)
            //{
            //    m_attackDecider[i] = new RandomAttackDecider<Attack>();
            //}
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            UpdateAttackDeciderList();
            //m_patternCount = new float[4];
            m_attackCache = new List<Attack>();
            AddToAttackCache(Attack.Phase1Pattern1, Attack.Phase1Pattern2/*, Attack.Phase1Pattern3*/, Attack.Phase2Pattern1, Attack.Phase2Pattern2, Attack.Phase2Pattern3, Attack.Phase2Pattern4, Attack.Phase2Pattern5);
            m_attackRangeCache = new List<float>();
            AddToRangeCache(m_info.phase1Pattern1Range, m_info.phase1Pattern2Range/*, m_info.phase1Pattern3Range*/, m_info.phase2Pattern1Range, m_info.phase2Pattern2Range, m_info.phase2Pattern3Range, m_info.phase2Pattern5Range);
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
            //m_spineListener.Subscribe(m_info.multipleFleshSpikeProjectile.launchOnEvent, m_deathFX.Play);
            //m_spineListener.Subscribe(m_info.staffSpinFleshBombProjectile.launchOnEvent, m_deathFX.Play);

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
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
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
                    m_turnHandle.Execute(m_info.turnAnimation, m_currentIdleAnimation);
                    m_movement.Stop();
                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    var randomFacing = UnityEngine.Random.Range(0, 2) == 1 ? 1 : -1;
                    var randomAttack = UnityEngine.Random.Range(0, 2);
                    var randomGroundPos = new Vector2(RandomTeleportPoint().x, GroundPosition(m_projectilePoint.position).y);
                    switch (m_currentAttack)
                    {
                        case Attack.Phase1Pattern1:
                            m_attackRoutine = StartCoroutine(RainProjectilesRoutine());
                            m_pickedCooldown = m_currentFullCooldown[0];
                            break;
                        case Attack.Phase1Pattern2:
                            m_attackRoutine = StartCoroutine(SingleFleshSpikeRoutine());
                            m_pickedCooldown = m_currentFullCooldown[1];
                            break;
                        //case Attack.Phase1Pattern3:
                        //    m_attackRoutine = StartCoroutine(RainProjectilesRoutine());
                        //    m_pickedCooldown = m_currentFullCooldown[3];
                        //    break;
                        case Attack.Phase2Pattern1:
                            m_attackRoutine = StartCoroutine(RainProjectilesRoutine());
                            m_pickedCooldown = m_currentFullCooldown[0];
                            break;
                        case Attack.Phase2Pattern2:
                            m_attackRoutine = StartCoroutine(RainProjectilesRoutine());
                            m_pickedCooldown = m_currentFullCooldown[1];
                            break;
                        case Attack.Phase2Pattern3:
                            m_attackRoutine = StartCoroutine(RainProjectilesRoutine());
                            m_pickedCooldown = m_currentFullCooldown[2];
                            break;
                        case Attack.Phase2Pattern4:
                            m_attackRoutine = StartCoroutine(RainProjectilesRoutine());
                            m_pickedCooldown = m_currentFullCooldown[3];
                            break;
                        case Attack.Phase2Pattern5:
                            m_attackRoutine = StartCoroutine(RainProjectilesRoutine());
                            m_pickedCooldown = m_currentFullCooldown[4];
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
                    DecidedOnAttack(false);
                    ChooseAttack();
                    if (IsFacingTarget())
                    {
                        if (m_attackDecider.hasDecidedOnAttack)
                        {
                            m_hitCount = 0;
                            m_attackCount = 0;
                            //m_stateHandle.SetState(State.Attacking);
                            StartCoroutine(TeleportToTargetRoutine(m_targetInfo.position, m_currentAttack, Vector2.zero));
                        }
                    }
                    else
                    {
                        m_turnState = State.Chasing;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation /*&& m_animation.GetCurrentAnimation(0).ToString() != m_info.attackDaggersIdle.animation*/)
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