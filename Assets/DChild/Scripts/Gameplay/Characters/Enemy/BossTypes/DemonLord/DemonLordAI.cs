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
    [AddComponentMenu("DChild/Gameplay/Enemies/Boss/DemonLord")]
    public class DemonLordAI : CombatAIBrain<DemonLordAI.Info>
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
            [SerializeField]
            private MovementInfo m_lightningStepAway = new MovementInfo();
            public MovementInfo lightningStepAway => m_lightningStepAway;
            [SerializeField]
            private MovementInfo m_lightningStepMidair = new MovementInfo();
            public MovementInfo lightningStepMidair => m_lightningStepMidair;


            [Title("Attack Behaviours")]
            [SerializeField, TabGroup("Ephemeral Arms")]
            private SimpleAttackInfo m_ephemeralArmsSmashAttack = new SimpleAttackInfo();
            public SimpleAttackInfo ephemeralArmsSmashAttack => m_ephemeralArmsSmashAttack;
            [SerializeField, TabGroup("Ephemeral Arms")]
            private SimpleAttackInfo m_ephemeralArmsComboAttack = new SimpleAttackInfo();
            public SimpleAttackInfo ephemeralArmsComboAttack => m_ephemeralArmsComboAttack;
            [SerializeField, TabGroup("Fireball Attacks")]
            private float m_threeFireBallsCycle;
            public float threeFireBallsCycle => m_threeFireBallsCycle;
            [SerializeField, TabGroup("Fireball Attacks")]
            private SimpleAttackInfo m_threeFireBallsAttack = new SimpleAttackInfo();
            public SimpleAttackInfo threeFireBallsAttack => m_threeFireBallsAttack;
            [SerializeField, ValueDropdown("GetAnimations"), TabGroup("Fireball Attacks")]
            private string m_threeFireBallsPreAnimation;
            public string threeFireBallsPreAnimation => m_threeFireBallsPreAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), TabGroup("Fireball Attacks")]
            private string m_threeFireBallsFireAnimation;
            public string threeFireBallsFireAnimation => m_threeFireBallsFireAnimation;
            [SerializeField, TabGroup("Fireball Attacks")]
            private SimpleAttackInfo m_flameWaveAttack = new SimpleAttackInfo();
            public SimpleAttackInfo flameWaveAttack => m_flameWaveAttack;
            [SerializeField, TabGroup("Ice Attacks")]
            private float m_rayOfFrostChargeDuration;
            public float rayOfFrostChargeDuration => m_rayOfFrostChargeDuration;
            [SerializeField, TabGroup("Ice Attacks")]
            private SimpleAttackInfo m_rayOfFrostAttack = new SimpleAttackInfo();
            public SimpleAttackInfo rayOfFrostAttack => m_rayOfFrostAttack;
            [SerializeField, ValueDropdown("GetAnimations"), TabGroup("Ice Attacks")]
            private string m_rayOfFrostChargeAnimation;
            public string rayOfFrostChargeAnimation => m_rayOfFrostChargeAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), TabGroup("Ice Attacks")]
            private string m_rayOfFrostFireAnimation;
            public string rayOfFrostFireAnimation => m_rayOfFrostFireAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), TabGroup("Ice Attacks")]
            private string m_rayOfFrostFireToIdleAnimation;
            public string rayOfFrostFireToIdleAnimation => m_rayOfFrostFireToIdleAnimation;
            [SerializeField, TabGroup("Ice Attacks")]
            private SimpleAttackInfo m_iceBombAttack = new SimpleAttackInfo();
            public SimpleAttackInfo iceBombAttack => m_iceBombAttack;
            [SerializeField, ValueDropdown("GetAnimations"), TabGroup("Ice Attacks")]
            private string m_IceBombThrowAnimation;
            public string IceBombThrowAnimation => m_IceBombThrowAnimation;
            [SerializeField, TabGroup("Electric Attacks")]
            private SimpleAttackInfo m_electricOrbAttack = new SimpleAttackInfo();
            public SimpleAttackInfo electricOrbAttack => m_electricOrbAttack;
            [SerializeField, TabGroup("Electric Attacks")]
            private SimpleAttackInfo m_lightningGroundAttack = new SimpleAttackInfo();
            public SimpleAttackInfo lightningGroundAttack => m_lightningGroundAttack;

            [TitleGroup("Pattern Ranges")]
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;
            [SerializeField]
            private float m_target1CHDistance;
            public float target1CHDistance => m_target1CHDistance;
            [SerializeField]
            private float m_target3CHDistance;
            public float target3CHDistance => m_target3CHDistance;
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
            //[SerializeField, BoxGroup("Phase 2")] //Disabled
            //private float m_phase2Pattern1Range;
            //public float phase2Pattern1Range => m_phase2Pattern1Range;
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
            [SerializeField, BoxGroup("Phase 3")]
            private float m_phase3Pattern1Range;
            public float phase3Pattern1Range => m_phase3Pattern1Range;
            [SerializeField, BoxGroup("Phase 3")]
            private float m_phase3Pattern2Range;
            public float phase3Pattern2Range => m_phase3Pattern2Range;
            //[SerializeField, BoxGroup("Phase 3")] //Disabled
            //private float m_phase3Pattern3Range;
            //public float phase3Pattern3Range => m_phase3Pattern3Range;
            //[SerializeField, BoxGroup("Phase 3")] //Disabled
            //private float m_phase3Pattern4Range;
            //public float phase3Pattern4Range => m_phase3Pattern4Range;
            [SerializeField, BoxGroup("Phase 3")]
            private float m_phase3Pattern5Range;
            public float phase3Pattern5Range => m_phase3Pattern5Range;
            [SerializeField, BoxGroup("Phase 3")]
            private float m_phase3Pattern6Range;
            public float phase3Pattern6Range => m_phase3Pattern6Range;

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

            //[Title("Misc")]
            //[SerializeField]
            //private float m_targetDistanceTolerance;
            //public float targetDistanceTolerance => m_targetDistanceTolerance;

            [Title("Animations")]
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;

            //[Title("Projectiles")]
            //[SerializeField, BoxGroup("RainProjectiles")]
            //private float m_rainProjectilesDuration;
            //public float rainProjectilesDuration => m_rainProjectilesDuration;
            [SerializeField, BoxGroup("FireBallProjectile")]
            private SimpleProjectileAttackInfo m_fireBallProjectile;
            public SimpleProjectileAttackInfo fireBallProjectile => m_fireBallProjectile;
            [SerializeField, BoxGroup("FireBallProjectile")]
            private float m_fireBallDelay;
            public float fireBallDelay => m_fireBallDelay;
            [SerializeField, BoxGroup("IceBombProjectile")]
            private SimpleProjectileAttackInfo m_iceBombProjectile;
            public SimpleProjectileAttackInfo iceBombProjectile => m_iceBombProjectile;
            [SerializeField, BoxGroup("IceBombProjectile")]
            private float m_iceBombDelay;
            public float iceBombDelay => m_iceBombDelay;

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
                m_move.SetData(m_skeletonDataAsset);
                m_lightningStepAway.SetData(m_skeletonDataAsset);
                m_lightningStepMidair.SetData(m_skeletonDataAsset);
                m_ephemeralArmsSmashAttack.SetData(m_skeletonDataAsset);
                m_ephemeralArmsComboAttack.SetData(m_skeletonDataAsset);
                m_threeFireBallsAttack.SetData(m_skeletonDataAsset);
                m_flameWaveAttack.SetData(m_skeletonDataAsset);
                m_rayOfFrostAttack.SetData(m_skeletonDataAsset);
                m_iceBombAttack.SetData(m_skeletonDataAsset);
                m_electricOrbAttack.SetData(m_skeletonDataAsset);
                m_lightningGroundAttack.SetData(m_skeletonDataAsset);
                m_fireBallProjectile.SetData(m_skeletonDataAsset);
                m_iceBombProjectile.SetData(m_skeletonDataAsset);
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
            //private int m_hitCount;
            //public int hitCount => m_hitCount;
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
            Phase1Pattern3,
            Phase1Pattern4,
            //Phase2Pattern1,
            Phase2Pattern2,
            Phase2Pattern3,
            Phase2Pattern4,
            Phase2Pattern5,
            Phase3Pattern1,
            Phase3Pattern2,
            //Phase3Pattern3,
            //Phase3Pattern4,
            Phase3Pattern5,
            Phase3Pattern6,
            WaitAttackEnd,
        }

        private enum FollowUpAttack
        {
            EphemeralArmsSmash,
            EphemeralArmsCombo,
            ThreeFireBalls,
            FlameWave,
            RayOfFrost,
            IceBomb,
            ElectricOrb,
            LightningGround,
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
        //[SerializeField, TabGroup("Reference")]
        //private DemonLordEphemeralArms m_ephemeralArms;
        [SerializeField, TabGroup("Reference")]
        private DemonLordEphemeralArms m_ephemeralArmsFront;
        [SerializeField, TabGroup("Reference")]
        private DemonLordEphemeralArms m_ephemeralArmsBack;
        [SerializeField, TabGroup("Reference")]
        private DemonLordBook m_book;

        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private PathFinderAgent m_agent;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandler;

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
        private ProjectileLauncher m_fireBallProjectileLauncher;
        private ProjectileLauncher m_iceBombProjectileLauncher;
        #endregion

        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_fireBallSpawnPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_iceBombSpawnPoint;
        [SerializeField, TabGroup("Target Points")]
        private Transform m_fireBallTargetPoint;
        [SerializeField, TabGroup("Target Points")]
        private List<Transform> m_iceBombTargetPoints;

        private Vector2 m_lastTargetPos;
        private float m_currentCooldown;
        private float m_pickedCooldown;
        private List<float> m_currentFullCooldown;
        private int[] m_patternAttackCount;
        private List<float> m_patternCooldown;
        //private int m_maxHitCount;
        //private int m_currentHitCount;
        
        private Coroutine m_currentAttackCoroutine;

        private bool m_isDetecting;

        private void ApplyPhaseData(PhaseInfo obj)
        {
            m_attackCache.Clear();
            m_attackRangeCache.Clear();
            if (m_patternCooldown.Count != 0)
                m_patternCooldown.Clear();
            //m_maxHitCount = obj.hitCount;
            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseOne:
                    AddToAttackCache(Attack.Phase1Pattern1, Attack.Phase1Pattern2, Attack.Phase1Pattern3, Attack.Phase1Pattern4);
                    AddToRangeCache(m_info.phase1Pattern1Range, m_info.phase1Pattern2Range, m_info.phase1Pattern3Range, m_info.phase1Pattern4Range);
                    for (int i = 0; i < m_info.phase1PatternCooldown.Count; i++)
                        m_patternCooldown.Add(m_info.phase1PatternCooldown[i]);
                    break;
                case Phase.PhaseTwo:
                    AddToAttackCache(/*Attack.Phase2Pattern1,*/ Attack.Phase2Pattern2, Attack.Phase2Pattern3, Attack.Phase2Pattern4, Attack.Phase2Pattern5);
                    AddToRangeCache(/*m_info.phase2Pattern1Range,*/ m_info.phase2Pattern2Range, m_info.phase2Pattern3Range, m_info.phase2Pattern4Range, m_info.phase2Pattern5Range);
                    for (int i = 0; i < m_info.phase2PatternCooldown.Count; i++)
                        m_patternCooldown.Add(m_info.phase2PatternCooldown[i]);
                    break;
                case Phase.PhaseThree:
                    AddToAttackCache(Attack.Phase3Pattern1, Attack.Phase3Pattern2, /*Attack.Phase3Pattern3,*/ /*Attack.Phase3Pattern4,*/ Attack.Phase3Pattern5);
                    AddToRangeCache(m_info.phase3Pattern1Range, m_info.phase3Pattern2Range, /*m_info.phase3Pattern3Range,*/ /*m_info.phase3Pattern4Range,*/ m_info.phase3Pattern5Range);
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

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            if (m_stateHandle.currentState != State.Phasing)
            {
                m_book.Idle(true);
                m_animation.animationState.TimeScale = 1f;
                m_stateHandle.ApplyQueuedState();
            }
        }

        private IEnumerator IntroRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_agent.Stop();
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            yield return new WaitForSeconds(2);
            m_animation.SetAnimation(0, m_info.move.animation, true);
            yield return new WaitForSeconds(5);
            //GetComponentInChildren<MeshRenderer>().sortingOrder = 99;
            m_book.EphemeralArmsSmash(false);
            //m_ephemeralArms.EphemeralArmsSmash(false);
            m_ephemeralArmsFront.EphemeralArmsSmash(false);
            m_ephemeralArmsBack.EphemeralArmsSmash(false);
            m_animation.SetAnimation(0, m_info.ephemeralArmsSmashAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.ephemeralArmsSmashAttack.animation);
            m_book.Idle(true);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        //private void ResetCounts()
        //{
        //    m_currentHitCount = 0;
        //}

        private IEnumerator ChangePhaseRoutine()
        {
            enabled = false;
            //m_hitbox.SetCanBlockDamageState(false);
            m_hitbox.Disable();
            m_stateHandle.Wait(State.Chasing);
            //ResetCounts();

            m_hitbox.Enable();
            m_stateHandle.ApplyQueuedState();
            yield return null;
            enabled = true;
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            m_book.Flinch(false);
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            m_book.Idle(true);
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            m_book.Death(false);
            StopAllCoroutines();
            m_agent.Stop();
            m_isDetecting = false;
        }

        #region Attacks

        private IEnumerator EphemeralArmsSmashAttackRoutine(FollowUpAttack attack)
        {
            m_book.EphemeralArmsSmash(false);
            //m_ephemeralArms.EphemeralArmsSmash(false);
            m_ephemeralArmsFront.EphemeralArmsSmash(false);
            m_ephemeralArmsBack.EphemeralArmsSmash(false);
            m_animation.SetAnimation(0, m_info.ephemeralArmsSmashAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.ephemeralArmsSmashAttack.animation);
            //m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            //m_stateHandle.ApplyQueuedState();
            if (m_phaseHandle.currentPhase == Phase.PhaseThree)
            {
                if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) <= m_info.target3CHDistance)
                {
                    attack = FollowUpAttack.EphemeralArmsCombo;
                }
            }
            else
            {
                if (attack == FollowUpAttack.ThreeFireBalls)
                {
                    if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) <= m_info.target1CHDistance)
                    {
                        yield return null;
                    }
                }
            }
            switch (attack)
            {
                case FollowUpAttack.EphemeralArmsCombo:
                    m_currentAttackCoroutine = StartCoroutine(EphemeralArmsComboAttackRoutine());
                    break;
                case FollowUpAttack.ThreeFireBalls:
                    m_currentAttackCoroutine = StartCoroutine(ThreeFireBallsAttackRoutine());
                    break;
                case FollowUpAttack.FlameWave:
                    m_currentAttackCoroutine = StartCoroutine(FlameWaveAttackRoutine());
                    break;
                case FollowUpAttack.RayOfFrost:
                    m_currentAttackCoroutine = StartCoroutine(RayOfFrostAttackRoutine());
                    break;
                case FollowUpAttack.IceBomb:
                    m_currentAttackCoroutine = StartCoroutine(IceBombAttackRoutine());
                    break;
                case FollowUpAttack.ElectricOrb:
                    m_currentAttackCoroutine = StartCoroutine(ElectricOrbAttackRoutine());
                    break;
                case FollowUpAttack.LightningGround:
                    m_currentAttackCoroutine = StartCoroutine(LightningGroundAttackRoutine());
                    break;
            }
            yield return null;
        }

        private IEnumerator EphemeralArmsComboAttackRoutine()
        {
            m_book.EphemeralArmsCombo(false);
            //m_ephemeralArms.EphemeralArmsCombo(false);
            m_ephemeralArmsFront.EphemeralArmsCombo(false);
            m_ephemeralArmsBack.EphemeralArmsCombo(false);
            m_animation.SetAnimation(0, m_info.ephemeralArmsComboAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.ephemeralArmsComboAttack.animation);
            m_book.Idle(true);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator ThreeFireBallsAttackRoutine()
        {
            m_book.ThreeFireBallsPre(false);
            //m_ephemeralArms.ThreeFireBallsPre(false);
            m_ephemeralArmsFront.ThreeFireBallsPre(false);
            m_ephemeralArmsBack.ThreeFireBallsPre(false);
            m_animation.SetAnimation(0, m_info.threeFireBallsPreAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.threeFireBallsPreAnimation);
            m_book.ThreeFireBallsFire(false);
            //m_ephemeralArms.ThreeFireBallsFire(false);
            m_ephemeralArmsFront.ThreeFireBallsFire(false);
            m_ephemeralArmsBack.ThreeFireBallsFire(false);
            m_animation.SetAnimation(0, m_info.threeFireBallsFireAnimation, false);
            for (int i = 0; i < m_info.threeFireBallsCycle; i++)
            {
                m_fireBallProjectileLauncher.AimAt(m_targetInfo.position);
                yield return new WaitForSeconds(m_info.fireBallDelay);
                m_fireBallProjectileLauncher.LaunchProjectile();
                yield return null;
            }
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.threeFireBallsFireAnimation);
            m_book.Idle(true);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator FlameWaveAttackRoutine()
        {
            m_book.FlameWave(false);
            m_animation.SetAnimation(0, m_info.flameWaveAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flameWaveAttack.animation);
            m_book.Idle(true);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator RayOfFrostAttackRoutine()
        {
            m_book.LightningMidair(false);
            m_animation.SetAnimation(0, m_info.lightningStepMidair.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.lightningStepMidair.animation);
            m_book.RayOfFrost(false);
            //m_ephemeralArms.RayOfFrost(false);
            m_ephemeralArmsFront.RayOfFrost(false);
            m_ephemeralArmsBack.RayOfFrost(false);
            m_animation.SetAnimation(0, m_info.rayOfFrostAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rayOfFrostAttack.animation);
            m_book.RayOfFrostCharge(true);
            //m_ephemeralArms.RayOfFrostCharge(true);
            m_ephemeralArmsFront.RayOfFrostCharge(true);
            m_ephemeralArmsBack.RayOfFrostCharge(true);
            m_animation.SetAnimation(0, m_info.rayOfFrostChargeAnimation, true);
            yield return new WaitForSeconds(m_info.rayOfFrostChargeDuration);
            m_book.RayOfFrostFire(false);
            //m_ephemeralArms.RayOfFrostFire(false);
            m_ephemeralArmsFront.RayOfFrostFire(false);
            m_ephemeralArmsBack.RayOfFrostFire(false);
            m_animation.SetAnimation(0, m_info.rayOfFrostFireAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rayOfFrostFireAnimation);
            m_book.RayOfFrostFireToIdle(false);
            //m_ephemeralArms.RayOfFrostFireToIdle(false);
            m_ephemeralArmsFront.RayOfFrostFireToIdle(false);
            m_ephemeralArmsBack.RayOfFrostFireToIdle(false);
            m_animation.SetAnimation(0, m_info.rayOfFrostFireToIdleAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.rayOfFrostFireToIdleAnimation);
            m_book.Idle(true);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator IceBombAttackRoutine()
        {
            m_book.IceBomb(false);
            //m_ephemeralArms.IceBomb(false);
            m_ephemeralArmsFront.IceBomb(false);
            m_ephemeralArmsBack.IceBomb(false);
            m_animation.SetAnimation(0, m_info.iceBombAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.iceBombAttack.animation);
            m_book.IceBombThrow(false);
            //m_ephemeralArms.IceBombThrow(false);
            m_ephemeralArmsFront.IceBombThrow(false);
            m_ephemeralArmsBack.IceBombThrow(false);
            m_animation.SetAnimation(0, m_info.IceBombThrowAnimation, false);
            yield return new WaitForSeconds(m_info.iceBombDelay);
            for (int i = 0; i < m_iceBombTargetPoints.Count; i++)
            {
                var instance = GameSystem.poolManager.GetPool<ProjectilePool>().GetOrCreateItem(m_info.iceBombProjectile.projectileInfo.projectile);
                instance.transform.position = m_iceBombSpawnPoint.position;
                instance.GetComponent<DemonLordIceBomb>().SetTarget(m_iceBombTargetPoints[i]);

                m_iceBombProjectileLauncher.AimAt(m_iceBombTargetPoints[i].position);
                m_iceBombProjectileLauncher.LaunchProjectile(m_iceBombSpawnPoint.right, instance.gameObject);
            }
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.IceBombThrowAnimation);
            m_book.Idle(true);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator ElectricOrbAttackRoutine()
        {
            //m_ephemeralArms.ThreeFireBallsPre(false);
            m_ephemeralArmsFront.ThreeFireBallsPre(false);
            m_ephemeralArmsBack.ThreeFireBallsPre(false);
            m_animation.SetAnimation(0, m_info.electricOrbAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.electricOrbAttack.animation);
            //m_ephemeralArms.EmptyAnimation();
            m_ephemeralArmsFront.EmptyAnimation();
            m_ephemeralArmsBack.EmptyAnimation();
            m_book.Idle(true);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator LightningGroundAttackRoutine()
        {
            m_book.LightningGround(false);
            //m_ephemeralArms.LightningGround(false);
            m_ephemeralArmsFront.LightningGround(false);
            m_ephemeralArmsBack.LightningGround(false);
            m_animation.SetAnimation(0, m_info.lightningGroundAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.lightningGroundAttack.animation);
            m_book.Idle(true);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        #endregion

        #region Movement
        #endregion

        //private void DecidedOnAttack(bool condition)
        //{
        //    m_attackDecider.hasDecidedOnAttack = condition;
        //}

        private void UpdateAttackDeciderList()
        {
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Phase1Pattern1, m_info.phase1Pattern1Range),
                                     new AttackInfo<Attack>(Attack.Phase1Pattern2, m_info.phase1Pattern2Range),
                                     new AttackInfo<Attack>(Attack.Phase1Pattern3, m_info.phase1Pattern3Range),
                                     new AttackInfo<Attack>(Attack.Phase1Pattern3, m_info.phase1Pattern4Range),
                                     //new AttackInfo<Attack>(Attack.Phase2Pattern1, m_info.phase2Pattern1Range),
                                     new AttackInfo<Attack>(Attack.Phase2Pattern2, m_info.phase2Pattern2Range),
                                     new AttackInfo<Attack>(Attack.Phase2Pattern3, m_info.phase2Pattern3Range),
                                     new AttackInfo<Attack>(Attack.Phase2Pattern4, m_info.phase2Pattern4Range),
                                     new AttackInfo<Attack>(Attack.Phase2Pattern5, m_info.phase2Pattern5Range),
                                     new AttackInfo<Attack>(Attack.Phase3Pattern1, m_info.phase3Pattern1Range),
                                     new AttackInfo<Attack>(Attack.Phase3Pattern2, m_info.phase3Pattern2Range),
                                     //new AttackInfo<Attack>(Attack.Phase3Pattern3, m_info.phase3Pattern3Range),
                                     //new AttackInfo<Attack>(Attack.Phase3Pattern4, m_info.phase3Pattern4Range),
                                     new AttackInfo<Attack>(Attack.Phase3Pattern5, m_info.phase3Pattern5Range),
                                     new AttackInfo<Attack>(Attack.Phase3Pattern5, m_info.phase3Pattern6Range));
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
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_flinchHandler.FlinchStart += OnFlinchStart;
            m_flinchHandler.FlinchEnd += OnFlinchEnd;
            m_fireBallProjectileLauncher = new ProjectileLauncher(m_info.fireBallProjectile.projectileInfo, m_fireBallSpawnPoint);
            m_iceBombProjectileLauncher = new ProjectileLauncher(m_info.iceBombProjectile.projectileInfo, m_iceBombSpawnPoint);
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
            AddToAttackCache(Attack.Phase1Pattern1, Attack.Phase1Pattern2, Attack.Phase1Pattern3, Attack.Phase1Pattern4, 
                /*Attack.Phase2Pattern1,*/ Attack.Phase2Pattern2, Attack.Phase2Pattern3, Attack.Phase2Pattern4, Attack.Phase2Pattern5, 
                Attack.Phase3Pattern1, Attack.Phase3Pattern2, /*Attack.Phase3Pattern3,*/ /*Attack.Phase3Pattern4,*/ Attack.Phase3Pattern5);
            m_attackRangeCache = new List<float>();
            AddToRangeCache(m_info.phase1Pattern1Range, m_info.phase1Pattern2Range, m_info.phase1Pattern3Range, m_info.phase1Pattern4Range, 
                /*m_info.phase2Pattern1Range,*/ m_info.phase2Pattern2Range, m_info.phase2Pattern3Range, m_info.phase2Pattern4Range, m_info.phase2Pattern5Range, 
                m_info.phase3Pattern1Range, m_info.phase3Pattern2Range, /*m_info.phase3Pattern3Range,*/ /*m_info.phase3Pattern4Range,*/ m_info.phase3Pattern5Range);
            m_attackUsed = new bool[m_attackCache.Count];
            m_currentFullCooldown = new List<float>();
            m_patternCooldown = new List<float>();
        }

        protected override void Start()
        {
            base.Start();
            //m_spineListener.Subscribe(m_info.OrbSummonRainProjectile.launchOnEvent, m_deathFX.Play);

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
                    m_book.Idle(true);
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
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
                    m_book.Turn(false);
                    m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                    m_agent.Stop();
                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    m_lastTargetPos = m_targetInfo.position;
                    m_agent.Stop();

                    switch (m_currentAttack)
                    {
                        #region PHASE 1 ATTACKS
                        case Attack.Phase1Pattern1:
                            if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) > m_info.target1CHDistance)
                            {
                                m_currentAttackCoroutine = StartCoroutine(ThreeFireBallsAttackRoutine());
                            }
                            else
                            {
                                m_currentAttackCoroutine = StartCoroutine(EphemeralArmsSmashAttackRoutine(FollowUpAttack.ThreeFireBalls));
                            }
                            m_pickedCooldown = m_currentFullCooldown[0];
                            break;
                        case Attack.Phase1Pattern2:
                            if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) > m_info.target1CHDistance)
                            {
                                m_currentAttackCoroutine = StartCoroutine(IceBombAttackRoutine());
                            }
                            else
                            {
                                m_currentAttackCoroutine = StartCoroutine(EphemeralArmsSmashAttackRoutine(FollowUpAttack.IceBomb));
                            }
                            m_pickedCooldown = m_currentFullCooldown[1];
                            break;
                        case Attack.Phase1Pattern3:
                            if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) > m_info.target1CHDistance)
                            {
                                m_currentAttackCoroutine = StartCoroutine(ElectricOrbAttackRoutine());
                            }
                            else
                            {
                                m_currentAttackCoroutine = StartCoroutine(EphemeralArmsSmashAttackRoutine(FollowUpAttack.ElectricOrb));
                            }
                            m_pickedCooldown = m_currentFullCooldown[3];
                            break;
                        case Attack.Phase1Pattern4:
                            if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) > m_info.target1CHDistance)
                            {
                                m_currentAttackCoroutine = StartCoroutine(FlameWaveAttackRoutine());
                            }
                            else
                            {
                                m_currentAttackCoroutine = StartCoroutine(EphemeralArmsSmashAttackRoutine(FollowUpAttack.FlameWave));
                            }
                            m_pickedCooldown = m_currentFullCooldown[3];
                            break;
                        #endregion
                        #region PHASE 2 ATTACKS
                        //case Attack.Phase2Pattern1:
                        //    m_currentAttackCoroutine = StartCoroutine(TestingAttackRoutine());
                        //    m_pickedCooldown = m_currentFullCooldown[0];
                        //    break;
                        case Attack.Phase2Pattern2:
                            if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) > m_info.target1CHDistance)
                            {
                                m_currentAttackCoroutine = StartCoroutine(IceBombAttackRoutine());
                            }
                            else
                            {
                                m_currentAttackCoroutine = StartCoroutine(EphemeralArmsSmashAttackRoutine(FollowUpAttack.IceBomb));
                            }
                            m_pickedCooldown = m_currentFullCooldown[1];
                            break;
                        case Attack.Phase2Pattern3:
                            if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) > m_info.target1CHDistance)
                            {
                                m_currentAttackCoroutine = StartCoroutine(ElectricOrbAttackRoutine());
                            }
                            else
                            {
                                m_currentAttackCoroutine = StartCoroutine(EphemeralArmsSmashAttackRoutine(FollowUpAttack.ElectricOrb));
                            }
                            m_pickedCooldown = m_currentFullCooldown[2];
                            break;
                        case Attack.Phase2Pattern4:
                            if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) > m_info.target1CHDistance)
                            {
                                m_currentAttackCoroutine = StartCoroutine(FlameWaveAttackRoutine());
                            }
                            else
                            {
                                m_currentAttackCoroutine = StartCoroutine(EphemeralArmsSmashAttackRoutine(FollowUpAttack.FlameWave));
                            }
                            m_pickedCooldown = m_currentFullCooldown[3];
                            break;
                        case Attack.Phase2Pattern5:
                            m_currentAttackCoroutine = StartCoroutine(LightningGroundAttackRoutine());
                            m_pickedCooldown = m_currentFullCooldown[4];
                            break;
                        #endregion
                        #region PHASE 3 ATTACKS
                        case Attack.Phase3Pattern1:
                            if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) > m_info.target1CHDistance)
                            {
                                m_currentAttackCoroutine = StartCoroutine(ThreeFireBallsAttackRoutine());
                            }
                            else
                            {
                                m_currentAttackCoroutine = StartCoroutine(EphemeralArmsSmashAttackRoutine(FollowUpAttack.ThreeFireBalls));
                            }
                            m_pickedCooldown = m_currentFullCooldown[0];
                            break;
                        case Attack.Phase3Pattern2:
                            if (Vector2.Distance(m_targetInfo.position, m_character.centerMass.position) > m_info.target1CHDistance)
                            {
                                m_currentAttackCoroutine = StartCoroutine(IceBombAttackRoutine());
                            }
                            else
                            {
                                m_currentAttackCoroutine = StartCoroutine(EphemeralArmsSmashAttackRoutine(FollowUpAttack.IceBomb));
                            }
                            m_pickedCooldown = m_currentFullCooldown[1];
                            break;
                        //case Attack.Phase3Pattern3:
                        //    //m_currentAttackCoroutine = StartCoroutine(TestingAttackRoutine());
                        //    m_pickedCooldown = m_currentFullCooldown[2];
                        //    break;
                        //case Attack.Phase3Pattern4:
                        //    //m_currentAttackCoroutine = StartCoroutine(TestingAttackRoutine());
                        //    m_pickedCooldown = m_currentFullCooldown[3];
                        //    break;
                        case Attack.Phase3Pattern5:
                            m_currentAttackCoroutine = StartCoroutine(LightningGroundAttackRoutine());
                            m_pickedCooldown = m_currentFullCooldown[4];
                            break;
                        case Attack.Phase3Pattern6:
                            m_currentAttackCoroutine = StartCoroutine(RayOfFrostAttackRoutine());
                            m_pickedCooldown = m_currentFullCooldown[5];
                            break;
                            #endregion
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
                        m_book.Idle(true);
                        m_animation.SetAnimation(0, m_info.idleAnimation, true);
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
                        if (m_attackDecider.hasDecidedOnAttack && IsTargetInRange(m_currentAttackRange))
                        {
                            m_stateHandle.SetState(State.Attacking);
                        }
                        else
                        {
                            m_animation.SetAnimation(0, m_info.move.animation, true);
                            m_agent.SetDestination(m_targetInfo.position);
                            m_agent.Move(m_info.move.speed);
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