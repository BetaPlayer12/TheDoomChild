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
using DChild.Temp;
using Spine.Unity.Modules;
using Spine.Unity.Examples;
using DChild.Gameplay.Pooling;
using UnityEngine.Playables;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Boss/CinderBolt")]
    public class CinderBoltAI : CombatAIBrain<CinderBoltAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField]
            private PhaseInfo<Phase> m_phaseInfo;
            public PhaseInfo<Phase> phaseInfo => m_phaseInfo;

            [SerializeField, BoxGroup("Movement")]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;

            [SerializeField, BoxGroup("Movement"), ValueDropdown("GetAnimations")]
            private string m_moveTurnAnimation;
            public string moveTurnAnimation => m_moveTurnAnimation;

            [SerializeField, Title("Hover"), BoxGroup("Movement")]
            private MovementInfo m_hoverUpward = new MovementInfo();
            public MovementInfo hoverUpward => m_hoverUpward;
            [SerializeField, Title("Hover"), BoxGroup("Movement")]
            private MovementInfo m_overchargedHoverUpward = new MovementInfo();
            public MovementInfo overchargedHoverUpward => m_overchargedHoverUpward;
            [SerializeField, Title("Hover"), BoxGroup("Movement")]
            private MovementInfo m_hoverBackward = new MovementInfo();
            public MovementInfo hoverBackward => m_hoverBackward;
            [SerializeField, Title("Hover"), BoxGroup("Movement")]
            private MovementInfo m_overchargedHoverBackward = new MovementInfo();
            public MovementInfo overchargedHoverBackward => m_overchargedHoverBackward;
            [SerializeField, Title("Hover"), BoxGroup("Movement")]
            private MovementInfo m_hoverDownward = new MovementInfo();
            public MovementInfo hoverDownward => m_hoverDownward;
            [SerializeField, Title("Hover"), BoxGroup("Movement")]
            private MovementInfo m_overchargedHoverDownward = new MovementInfo();
            public MovementInfo overchargedHoverDownward => m_overchargedHoverDownward;
            [SerializeField, Title("Hover"), BoxGroup("Movement")]
            private MovementInfo m_hoverForward = new MovementInfo();
            public MovementInfo hoverForward => m_hoverForward;
            [SerializeField, Title("Hover"), BoxGroup("Movement")]
            private MovementInfo m_overchargedHoverForward = new MovementInfo();
            public MovementInfo overchargedHoverForward => m_overchargedHoverForward;
            [SerializeField, Title("Long Dash"), BoxGroup("Movement")]
            private MovementInfo m_longDash = new MovementInfo();
            public MovementInfo longDash => m_longDash;
            [SerializeField, Title("Long Dash"), BoxGroup("Movement")]
            private MovementInfo m_overchargedLongDash = new MovementInfo();
            public MovementInfo overchargedLongDash => m_overchargedLongDash;
            [SerializeField, Title("Long Dash"), BoxGroup("Movement"), ValueDropdown("GetAnimations")]
            private string m_longDashBoosterChargeAnimation;
            public string longDashBoosterChargeAnimation => m_longDashBoosterChargeAnimation;
            [SerializeField, Title("Long Dash"), BoxGroup("Movement"), ValueDropdown("GetAnimations")]
            private string m_overchargedLongDashBoosterChargeAnimation;
            public string overchargedLongDashBoosterChargeAnimation => m_overchargedLongDashBoosterChargeAnimation;
            [SerializeField, Title("Long Dash"), BoxGroup("Movement"), ValueDropdown("GetAnimations")]
            private string m_longDashStopAnimation;
            public string longDashStopAnimation => m_longDashStopAnimation;
            [SerializeField, Title("Short Dash"), BoxGroup("Movement")]
            private MovementInfo m_shortDash = new MovementInfo();
            public MovementInfo shortDash => m_shortDash;

            [SerializeField, Title("Straight Left and Uppercut"), BoxGroup("Attack")]
            private SimpleAttackInfo m_straightLeftAndUppercutAttack = new SimpleAttackInfo();
            public SimpleAttackInfo straightLeftAndUppercutAttack => m_straightLeftAndUppercutAttack;
            [SerializeField, Title("Straight Left and Uppercut"), BoxGroup("Attack")]
            private SimpleAttackInfo m_overchargedStraightLeftAndUppercutAttack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedStraightLeftAndUppercutAttack => m_overchargedStraightLeftAndUppercutAttack;
            [SerializeField, Title("Flame Thrower"), BoxGroup("Attack")]
            private SimpleAttackInfo m_flameThrowerAttack = new SimpleAttackInfo();
            public SimpleAttackInfo flameThrowerAttack => m_flameThrowerAttack;
            [SerializeField, Title("Flame Thrower"), BoxGroup("Attack")]
            private SimpleAttackInfo m_overchargedFlameThrowerAttack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedFlameThrowerAttack => m_overchargedFlameThrowerAttack;
            [SerializeField, Title("Flame Beam"), BoxGroup("Attack")]
            private SimpleAttackInfo m_flameBeamAttack = new SimpleAttackInfo();
            public SimpleAttackInfo flameBeamAttack => m_flameBeamAttack;
            [SerializeField, Title("Flame Beam"), BoxGroup("Attack")]
            private SimpleAttackInfo m_overchargedFlameBeamAttack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedFlameBeamAttack => m_overchargedFlameBeamAttack;
            [SerializeField, Title("Puch"), BoxGroup("Attack")]
            private SimpleAttackInfo m_punchAttack = new SimpleAttackInfo();
            public SimpleAttackInfo punchAttack => m_punchAttack;
            [SerializeField, Title("Puch"), BoxGroup("Attack")]
            private SimpleAttackInfo m_overchargedPunchAttack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedPunchAttack => m_overchargedPunchAttack;
            [SerializeField, Title("Shotgun Blast"), BoxGroup("Attack")]
            private SimpleAttackInfo m_shotgunBlastFireAttack = new SimpleAttackInfo();
            public SimpleAttackInfo shotgunBlastFireAttack => m_shotgunBlastFireAttack;
            [SerializeField, Title("Shotgun Blast"), BoxGroup("Attack")]
            private SimpleAttackInfo m_overchargedShotgunBlastFireAttack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedShotgunBlastFireAttack => m_overchargedShotgunBlastFireAttack;
            [SerializeField, Title("Shotgun Blast"), BoxGroup("Attack"), ValueDropdown("GetAnimations")]
            private string m_shotgunBlastBackToIdleAnimation;
            public string shotgunBlastBackToIdleAnimation => m_shotgunBlastBackToIdleAnimation;
            [SerializeField, Title("Shotgun Blast"), BoxGroup("Attack"), ValueDropdown("GetAnimations")]
            private string m_shotgunBlastPreAnimation;
            public string shotgunBlastPreAnimation => m_shotgunBlastPreAnimation;
            [SerializeField, Title("Shotgun Blast"), BoxGroup("Attack")]
            private SimpleAttackInfo m_shotgunBlastRapidFireAttack = new SimpleAttackInfo();
            public SimpleAttackInfo shotgunBlastRapidFireAttack => m_shotgunBlastRapidFireAttack;
            [SerializeField, Title("Shotgun Blast"), BoxGroup("Attack")]
            private SimpleAttackInfo m_overchargedShotgunBlastRapidFireAttack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedShotgunBlastRapidFireAttack => m_overchargedShotgunBlastRapidFireAttack;
            [SerializeField, Title("Uppercut"), BoxGroup("Attack")]
            private SimpleAttackInfo m_uppercutAttack = new SimpleAttackInfo();
            public SimpleAttackInfo uppercutAttack => m_uppercutAttack;
            [SerializeField, Title("Uppercut"), BoxGroup("Attack")]
            private SimpleAttackInfo m_overchargedUppercutAttack = new SimpleAttackInfo();
            public SimpleAttackInfo overchargedUppercutAttack => m_overchargedUppercutAttack;
            [SerializeField, Title("Firebeam"), BoxGroup("Attack")]
            private SimpleAttackInfo m_firebeamAttack = new SimpleAttackInfo();
            public SimpleAttackInfo firebeamAttack => m_firebeamAttack;
            [SerializeField, Title("Meteor Smash"), BoxGroup("Attack")]
            private SimpleAttackInfo m_meteorAttack = new SimpleAttackInfo();
            public SimpleAttackInfo meteorAttack => m_meteorAttack;
            [SerializeField, Title("Spin Attack"), BoxGroup("Attack")]
            private SimpleAttackInfo m_spinAttack = new SimpleAttackInfo();
            public SimpleAttackInfo spinAttack => m_spinAttack;

            [SerializeField, TabGroup("Phase 1"), BoxGroup("Pattern Ranges")]
            private float m_phase1Pattern1Range;
            public float phase1Pattern1Range => m_phase1Pattern1Range;
            [SerializeField, TabGroup("Phase 1"), BoxGroup("Pattern Ranges")]
            private float m_phase1Pattern2Range;
            public float phase1Pattern2Range => m_phase1Pattern2Range;
            [SerializeField, TabGroup("Phase 2"), BoxGroup("Pattern Ranges")]
            private float m_phase2Pattern1Range;
            public float phase2Pattern1Range => m_phase2Pattern1Range;
            [SerializeField, TabGroup("Phase 2"), BoxGroup("Pattern Ranges")]
            private float m_phase2Pattern2Range;
            public float phase2Pattern2Range => m_phase2Pattern2Range;
            [SerializeField, TabGroup("Phase 2"), BoxGroup("Pattern Ranges")]
            private float m_phase2Pattern3Range;
            public float phase2Pattern3Range => m_phase2Pattern3Range;
            [SerializeField, TabGroup("Phase 2"), BoxGroup("Pattern Ranges")]
            private float m_phase2Pattern4Range;
            public float phase2Pattern4Range => m_phase2Pattern4Range;


            [Title("Misc")]
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

            [Title("Animations")]
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_punchUppercut;
            public string punchUppercut => m_punchUppercut;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flamethower1;
            public string flamethrower1 => m_flamethower1;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_overchargedFlinchAnimation;
            public string overchargedFlinchAnimation => m_overchargedFlinchAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_hydraulicsAnimation;
            public string hydraulicsAnimation => m_hydraulicsAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_malfunctionStateAnimation;
            public string malfunctionStateAnimation => m_malfunctionStateAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_malfunctionStateIdleAnimation;
            public string malfunctionStateIdleAnimation => m_malfunctionStateIdleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_malfunctionRecoveryStateAnimation;
            public string malfunctionRecoveryStateAnimation => m_malfunctionRecoveryStateAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_spinAnimation;
            public string spinAnimation => m_spinAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_spinDropAnimation;
            public string spinDropAnimation => m_spinDropAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_spinPreAnimation;
            public string spinPreAnimation => m_spinPreAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_spinEndAnimation;
            public string spinEndAnimation => m_spinEndAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_overchargedSpinAnimation;
            public string overchargedSpinAnimation => m_overchargedSpinAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_overchargedSpinDropAnimation;
            public string overchargedSpinDropAnimation => m_overchargedSpinDropAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_overchargedSpinPreAnimation;
            public string overchargedSpinPreAnimation => m_overchargedSpinPreAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_overchargedSpinEndAnimation;
            public string overchargedSpinEndAnimation => m_overchargedSpinEndAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_punchUppercutEvent;
            public string punchUppercutEvent => m_punchUppercutEvent;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_move.SetData(m_skeletonDataAsset);
                m_hoverUpward.SetData(m_skeletonDataAsset);
                m_overchargedHoverUpward.SetData(m_skeletonDataAsset);
                m_hoverBackward.SetData(m_skeletonDataAsset);
                m_overchargedHoverBackward.SetData(m_skeletonDataAsset);
                m_hoverDownward.SetData(m_skeletonDataAsset);
                m_overchargedHoverDownward.SetData(m_skeletonDataAsset);
                m_hoverForward.SetData(m_skeletonDataAsset);
                m_overchargedHoverForward.SetData(m_skeletonDataAsset);
                m_longDash.SetData(m_skeletonDataAsset);
                m_overchargedLongDash.SetData(m_skeletonDataAsset);
                m_shortDash.SetData(m_skeletonDataAsset);
                m_straightLeftAndUppercutAttack.SetData(m_skeletonDataAsset);
                m_overchargedStraightLeftAndUppercutAttack.SetData(m_skeletonDataAsset);
                m_flameThrowerAttack.SetData(m_skeletonDataAsset);
                m_overchargedFlameThrowerAttack.SetData(m_skeletonDataAsset);
                m_flameBeamAttack.SetData(m_skeletonDataAsset);
                m_overchargedFlameBeamAttack.SetData(m_skeletonDataAsset);
                m_punchAttack.SetData(m_skeletonDataAsset);
                m_overchargedPunchAttack.SetData(m_skeletonDataAsset);
                m_shotgunBlastFireAttack.SetData(m_skeletonDataAsset);
                m_overchargedShotgunBlastFireAttack.SetData(m_skeletonDataAsset);
                m_shotgunBlastRapidFireAttack.SetData(m_skeletonDataAsset);
                m_overchargedShotgunBlastRapidFireAttack.SetData(m_skeletonDataAsset);
                m_uppercutAttack.SetData(m_skeletonDataAsset);
                m_overchargedUppercutAttack.SetData(m_skeletonDataAsset);
                m_firebeamAttack.SetData(m_skeletonDataAsset);
                m_meteorAttack.SetData(m_skeletonDataAsset);
                m_spinAttack.SetData(m_skeletonDataAsset);
#endif
            }
        }

        [System.Serializable]
        public class PhaseInfo : IPhaseInfo
        {
            [SerializeField]
            private int m_attackCount;
            public int attackCount => m_attackCount;
            [SerializeField]
            private List<int> m_patternCount;
            public List<int> patternCount => m_patternCount;
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
            Phase1Pattern1,
            Phase1Pattern2,
            Phase2Pattern1,
            Phase2Pattern2,
            Phase2Pattern3,
            Phase2Pattern4,
            WaitAttackEnd,
        }

        private enum Attack
        {
            PunchUppercut,
            ShotgunBlast,
            SpinAttack,
            MeteorSmash,
            Flamethrower1,
            Flamethrower2,
            Firebeam,
            WaitAttackEnd,
        }

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
        private AttackHandle m_attackHandle;
        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private Health m_health;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;

        [SerializeField, TabGroup("FX")]
        private ParticleFX m_flamethrower1FX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_firebeamFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_firebeamAnticipationFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_spinAttackFX;

        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Spawn Points")]
        private List<Transform> m_projectilePoints;
        [SerializeField, TabGroup("Spawn Points")]
        private GameObject m_flamethrower1SpawnPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private GameObject m_flamethrower2SpawnPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private GameObject m_rightHandSpawnPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private GameObject m_leftHandSpawnPoint;

        [SerializeField, TabGroup("Spawn Points")]
        private List<Transform> m_firebeamTransformPoints;

        [SerializeField]
        private SpineEventListener m_spineListener;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        State m_turnState;
        [ShowInInspector]
        private PhaseHandle<Phase, PhaseInfo> m_phaseHandle;
        private bool m_phase1Done;
        private bool m_phase2Done;
        [ShowInInspector]
        private RandomAttackDecider<Pattern> m_patternDecider;
        private Pattern m_chosenPattern;
        private Pattern m_previousPattern;
        [ShowInInspector]
        private RandomAttackDecider<Attack>[] m_attackDecider;
        private Attack m_currentAttack;
        [ShowInInspector]
        private float m_currentAttackRange;
        private ProjectileLauncher m_projectileLauncher;

        private int m_currentPhaseIndex;
        private float m_attackCount;
        private float[] m_patternCount;
        private int m_hitCount;
        private bool m_hasPhaseChanged;
        private Coroutine m_currentAttackCoroutine;
        [SerializeField]
        private Collider2D m_punchAttackCollider;

        private Coroutine m_changeLocationRoutine;
        private bool m_isDetecting;
        /*private float leftPos;
        private float rightPos;
        private float counter;*/
        [SerializeField]
        private List<Collider2D> m_spinAttackCollider;

        [SerializeField, TabGroup("Laser")]
        private LineRenderer m_lineRenderer;
        [SerializeField, TabGroup("Laser")]
        private LineRenderer m_telegraphLineRenderer;
        [SerializeField, TabGroup("Laser")]
        private EdgeCollider2D m_edgeCollider;
        [SerializeField, TabGroup("Laser")]
        private GameObject m_muzzleFXGO;
        [SerializeField, TabGroup("Laser")]
        private Transform m_lazerOrigin;
        [SerializeField, TabGroup("Laser")]
        private float m_lazerDuration;

        private bool m_beamOn;
        private bool m_aimOn;

        private void ApplyPhaseData(PhaseInfo obj)
        {
            /*m_attackCache.Clear();
            m_attackRangeCache.Clear();
            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseOne:
                    AddToAttackCache(Attack.PunchUppercut, Attack.Flamethrower1, Attack.SpinAttack, Attack.Firebeam);
                    AddToRangeCache(m_info.punchAttack.range, m_info.flameThrowerAttack.range, m_info.spinAttack.range, m_info.firebeamAttack.range);
                    break;
                case Phase.PhaseTwo:
                    AddToAttackCache(Attack.PunchUppercut, Attack.Flamethrower1, Attack.SpinAttack, Attack.Firebeam, Attack.Flamethrower2, Attack.MeteorSmash, Attack.ShotgunBlast);
                    AddToRangeCache(m_info.punchAttack.range, m_info.flameThrowerAttack.range, m_info.spinAttack.range, m_info.firebeamAttack.range, m_info.meteorAttack.range, m_info.shotgunBlastFireAttack.range);
                    break;
            }*/
            m_currentPhaseIndex = obj.phaseIndex;
            for (int i = 0; i < m_patternCount.Length; i++)
            {
                m_patternCount[i] = obj.patternCount[i];
            }
        }

        private void ChangeState()
        {
            if (!m_hasPhaseChanged && m_changeLocationRoutine == null)
            {
                m_hitbox.SetInvulnerability(Invulnerability.Level_1);
                StopAllCoroutines();
                m_stateHandle.OverrideState(State.Phasing);
                m_hasPhaseChanged = true;
                m_phaseHandle.ApplyChange();
                m_animation.DisableRootMotion();
                m_animation.SetEmptyAnimation(0, 0);
            }
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs)
        {
            if (m_stateHandle.currentState != State.Phasing)
            {
                m_stateHandle.OverrideState(State.Turning);
            }
        }

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                base.SetTarget(damageable, m_target);
                if (!m_isDetecting)
                {
                    m_isDetecting = true;
                }
                m_stateHandle.OverrideState(State.Intro);
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            if (m_stateHandle.currentState != State.Phasing)
            {
                m_animation.animationState.TimeScale = 1f;
                m_stateHandle.ApplyQueuedState();
            }
            m_phaseHandle.allowPhaseChange = true;
        }

        private IEnumerator IntroRoutine()
        {
            m_stateHandle.Wait(State.Chasing);
            m_movement.Stop();
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            CustomTurn();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        
        private IEnumerator ChangePhaseRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchAnimation);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_hasPhaseChanged = false;
            m_stateHandle.ApplyQueuedState();
            if (m_phaseHandle.currentPhase == Phase.PhaseTwo)
            {
                m_phase1Done = true;
            }
            yield return null;
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            StopAllCoroutines();
            m_movement.Stop();
            m_isDetecting = false;
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            m_hitCount++;
            m_flamethrower1FX.Stop();
            m_firebeamAnticipationFX.Stop();
            m_firebeamFX.Stop();
            if (m_hitCount == 5 && m_phaseHandle.currentPhase == Phase.PhaseOne)
            {
                StopAllCoroutines();
                m_hitCount = 0;
            }
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            m_stateHandle.OverrideState(State.ReevaluateSituation);
        }

        #region Attacks
        private IEnumerator PunchAttackRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            m_stateHandle.Wait(State.Chasing);
            m_movement.Stop();
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.punchUppercut, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.punchUppercut);
            m_punchAttackCollider.enabled = false;
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            m_punchAttackCollider.enabled = false;
            yield return null;
        }
        private IEnumerator Flamethrower1Routine()
        {
            yield return new WaitForSeconds(0.5f);
            m_stateHandle.Wait(State.Chasing);
            m_movement.Stop();
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.flameThrowerAttack.animation, false);
            yield return new WaitForSeconds(0.4f);
            m_flamethrower1FX.Play();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flameThrowerAttack.animation);
            m_flamethrower1FX.Stop();
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        private bool SpinColliders(bool isDone)
        {
            for (int i = 0; i < m_spinAttackCollider.Count; i++)
            {
                m_spinAttackCollider[i].enabled = isDone;
            }
            return isDone;
        }
        private IEnumerator SpinAttackRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            m_stateHandle.Wait(State.Chasing);
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.spinPreAnimation, false);
            yield return new WaitForSeconds(0.5f);
            m_animation.SetAnimation(0, m_info.spinAttack, true);
            SpinColliders(true);
            var m_followElapsedTime = 0f;
            var m_followDuration = 10f;
            while (m_followElapsedTime < m_followDuration)
            {
                m_movement.MoveTowards(new Vector2(m_targetInfo.position.x - transform.position.x, 0).normalized, m_info.move.speed);
                m_followElapsedTime += Time.deltaTime;
                yield return null;
            }
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.spinEndAnimation, false);
            SpinColliders(false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.spinEndAnimation);
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        private IEnumerator FirebeamLaserRoutine()
        {
            yield return null;
        }
        private IEnumerator FirebeamRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            m_stateHandle.Wait(State.Chasing);
            Vector2 firebeamPosition;
            if (Vector2.Distance(m_firebeamTransformPoints[2].position, m_targetInfo.position) > Vector2.Distance(m_firebeamTransformPoints[3].position, m_targetInfo.position))   
            {
                firebeamPosition = m_firebeamTransformPoints[3].position;
                yield return new WaitForSeconds(1.5f);
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                firebeamPosition = m_firebeamTransformPoints[2].position;
                yield return new WaitForSeconds(1.5f);
                transform.localScale = new Vector3(1, 1, 1);
            }
            while (Vector2.Distance(transform.position, firebeamPosition) > 10f)
            {
                Debug.Log(Vector2.Distance(transform.position, firebeamPosition));
                m_movement.MoveTowards(new Vector2(firebeamPosition.x - transform.position.x, 0), m_info.move.speed);
                yield return null;
            }
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.firebeamAttack, false);
            m_firebeamAnticipationFX.Play();
            yield return new WaitForSeconds(1f);
            m_firebeamFX.Play();
            StartCoroutine(FirebeamLaserRoutine());
            yield return new WaitForSeconds(3f);
            m_firebeamAnticipationFX.Stop();
            m_firebeamFX.Stop();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.firebeamAttack);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        #endregion

        #region Movement
        private void MoveToTarget(float targetRange)
        {
            if (!IsTargetInRange(targetRange))
            {
                m_animation.SetAnimation(0, m_info.move, true);
                m_movement.MoveTowards(Vector2.right * transform.localScale.x, m_info.move.speed);
                if(transform.position.y + 5 < m_targetInfo.position.y)
                {
                    m_movement.Stop();
                    m_movement.MoveTowards(Vector2.up * transform.localScale.y, m_info.move.speed);
                }
                if(m_targetInfo.position.y < -100)
                {
                    m_movement.Stop();
                    m_movement.MoveTowards(Vector2.down * transform.localScale.y, m_info.move.speed);
                    if(transform.position.y < -127)
                    {
                        //m_movement.Stop();
                        m_movement.MoveTowards(Vector2.right * transform.localScale.x, m_info.move.speed);
                    }
                }
            }
            else
            {
                //m_movement.Stop();
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
            
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
            m_patternDecider.hasDecidedOnAttack = condition;
            for (int i = 0; i < m_attackDecider.Length; i++)
            {
                m_attackDecider[i].hasDecidedOnAttack = condition;
            }
        }

        private void UpdateAttackDeciderList()
        {
            m_patternDecider.SetList(new AttackInfo<Pattern>(Pattern.Phase1Pattern1, m_info.targetDistanceTolerance),
                                     new AttackInfo<Pattern>(Pattern.Phase1Pattern2, m_info.targetDistanceTolerance),
                                     new AttackInfo<Pattern>(Pattern.Phase2Pattern1, m_info.targetDistanceTolerance),
                                     new AttackInfo<Pattern>(Pattern.Phase2Pattern2, m_info.targetDistanceTolerance),
                                     new AttackInfo<Pattern>(Pattern.Phase2Pattern3, m_info.targetDistanceTolerance),
                                     new AttackInfo<Pattern>(Pattern.Phase2Pattern4, m_info.targetDistanceTolerance));
            m_attackDecider[0].SetList(new AttackInfo<Attack>(Attack.PunchUppercut, m_info.targetDistanceTolerance),
                                     new AttackInfo<Attack>(Attack.Flamethrower1, m_info.targetDistanceTolerance),
                                     new AttackInfo<Attack>(Attack.SpinAttack, m_info.targetDistanceTolerance),
                                     new AttackInfo<Attack>(Attack.Firebeam, m_info.targetDistanceTolerance));
            m_attackDecider[1].SetList(new AttackInfo<Attack>(Attack.PunchUppercut, m_info.targetDistanceTolerance),
                                     new AttackInfo<Attack>(Attack.ShotgunBlast, m_info.targetDistanceTolerance),
                                     new AttackInfo<Attack>(Attack.Flamethrower1, m_info.targetDistanceTolerance),
                                     new AttackInfo<Attack>(Attack.Flamethrower2, m_info.targetDistanceTolerance),
                                     new AttackInfo<Attack>(Attack.SpinAttack, m_info.targetDistanceTolerance),
                                     new AttackInfo<Attack>(Attack.MeteorSmash, m_info.targetDistanceTolerance),
                                     new AttackInfo<Attack>(Attack.Firebeam, m_info.targetDistanceTolerance));
            DecidedOnAttack(false);
        }

        public override void ApplyData()
        {
            if (m_patternDecider != null)
            {
                UpdateAttackDeciderList();
            }
            base.ApplyData();
        }

        private void ExecuteAttack(int patternIndex)
        {
            if (m_attackCount < m_patternCount[patternIndex])
            {
                ChooseAttack(patternIndex);
                if (IsTargetInRange(m_currentAttackRange))
                {
                    m_stateHandle.Wait(State.Attacking);
                    switch (m_currentAttack)
                    {
                        case Attack.PunchUppercut:
                            Debug.Log("punch");
                            if (patternIndex == 0 || patternIndex == 1 || patternIndex == 2 || patternIndex == 3)
                            {
                                m_attackCount++;
                                m_currentAttackCoroutine = StartCoroutine(PunchAttackRoutine());
                            }
                            else
                            {
                                DecidedOnAttack(false);
                                m_stateHandle.ApplyQueuedState();
                            }
                            break;
                        case Attack.Flamethrower1:
                            Debug.Log("flame");
                            if (patternIndex == 0 || patternIndex == 1 || patternIndex == 2)
                            {
                                m_attackCount++;
                                m_currentAttackCoroutine = StartCoroutine(Flamethrower1Routine());
                            }
                            else
                            {
                                DecidedOnAttack(false);
                                m_stateHandle.ApplyQueuedState();
                            }
                            break;
                        case Attack.SpinAttack:
                            Debug.Log("spin");
                            if (patternIndex == 1 || patternIndex == 2 || patternIndex == 3 || patternIndex == 4)
                            {
                                m_attackCount++;
                                m_currentAttackCoroutine = StartCoroutine(SpinAttackRoutine());
                            }
                            else
                            {
                                DecidedOnAttack(false);
                                m_stateHandle.ApplyQueuedState();
                            }
                            break;
                        case Attack.Firebeam:
                            Debug.Log("firebeam");
                            if (patternIndex == 1 || patternIndex == 4 || patternIndex == 5)
                            {
                                //if (m_currentPhaseIndex == 1)
                                //{
                                    m_attackCount++;
                                    m_currentAttackCoroutine = StartCoroutine(FirebeamRoutine());
                                //}
                               // else
                                //{
                                 //   DecidedOnAttack(false);
                                //    m_stateHandle.ApplyQueuedState();
                                //}
                            }
                            else
                            {
                                DecidedOnAttack(false);
                                m_stateHandle.ApplyQueuedState();
                            }
                            break;
                        case Attack.ShotgunBlast:
                            if (patternIndex == 2 || patternIndex == 3)
                            {
                                //if (m_currentPhaseIndex == 1)
                                //{
                                    m_attackCount++;
                               //}
                                //else
                                //{
                                    //DecidedOnAttack(false);
                                    //m_stateHandle.ApplyQueuedState();
                                //}
                            }
                            else
                            {
                                DecidedOnAttack(false);
                                m_stateHandle.ApplyQueuedState();
                            }
                            break;
                        case Attack.Flamethrower2:
                            if (patternIndex == 2 || patternIndex == 5)
                            {
                               // if (m_currentPhaseIndex == 1)
                               // {
                                    m_attackCount++;
                               // }
                               // else
                              //  {
                             //       DecidedOnAttack(false);
                             //       m_stateHandle.ApplyQueuedState();
                              //  }
                            }
                            else
                            {
                                DecidedOnAttack(false);
                                m_stateHandle.ApplyQueuedState();
                            }
                            break;
                        case Attack.MeteorSmash:
                            if (patternIndex == 4)
                            {
                               // if (m_currentPhaseIndex == 1)
                              //  {
                                    m_attackCount++;
                              // }
                               // else
                              //  {
                               //     DecidedOnAttack(false);
                               //     m_stateHandle.ApplyQueuedState();
                               // }
                            }
                            else
                            {
                                DecidedOnAttack(false);
                                m_stateHandle.ApplyQueuedState();
                            }
                            break;
                    }
                }
                else
                {
                    MoveToTarget(m_currentAttackRange);
                }
            }
        }

        private void ChooseAttack(int patternIndex)
        {
            if (!m_attackDecider[patternIndex].hasDecidedOnAttack)
            {
                IsAllAttackComplete();
                for (int i = 0; i < m_attackCache.Count; i++)
                {
                    m_attackDecider[patternIndex].DecideOnAttack();
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
        void UpdateRangeCache(params float[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                m_attackRangeCache[i] = list[i];
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_patternDecider = new RandomAttackDecider<Pattern>();
            m_attackDecider = new RandomAttackDecider<Attack>[3];
            for (int i = 0; i < 3; i++)
            {
                m_attackDecider[i] = new RandomAttackDecider<Attack>();
            }
            m_patternCount = new float[6];
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            UpdateAttackDeciderList();
            m_attackCache = new List<Attack>();
            AddToAttackCache(Attack.Firebeam, Attack.Flamethrower1, Attack.Flamethrower2, Attack.MeteorSmash, Attack.PunchUppercut, Attack.ShotgunBlast, Attack.SpinAttack);
            m_attackRangeCache = new List<float>();
            AddToRangeCache(m_info.firebeamAttack.range, m_info.flameBeamAttack.range, m_info.flameBeamAttack.range, m_info.meteorAttack.range, m_info.uppercutAttack.range, m_info.shotgunBlastFireAttack.range, m_info.spinAttack.range);
            m_attackUsed = new bool[m_attackCache.Count];
        }

        protected override void Start()
        {
            base.Start();
            m_spineListener.Subscribe(m_info.punchUppercutEvent, PunchAttack);
            m_animation.DisableRootMotion();
            m_phaseHandle = new PhaseHandle<Phase, PhaseInfo>();
            m_phaseHandle.Initialize(Phase.PhaseOne, m_info.phaseInfo, m_character, ChangeState, ApplyPhaseData);
            m_phaseHandle.ApplyChange();
        }

        private void PunchAttack()
        {
            m_punchAttackCollider.enabled = true;
            //throw new System.NotImplementedException();
        }

        private void Update()
        {
            /*Debug.Log("l "+Vector2.Distance(m_targetInfo.position, m_firebeamTransformPoints[0].position));
            Debug.Log("r " + Vector2.Distance(m_targetInfo.position, m_firebeamTransformPoints[1].position));*/
            /*leftPos = Vector2.Distance(m_targetInfo.position, m_firebeamTransformPoints[0].position);
            rightPos = Vector2.Distance(m_targetInfo.position, m_firebeamTransformPoints[1].position);*/
            m_phaseHandle.MonitorPhase();
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    //m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    break;
                case State.Intro:
                    if (IsFacingTarget())
                    {
                        StartCoroutine(IntroRoutine());
                    }
                    else
                    {
                        CustomTurn();
                    }
                    break;
                case State.Phasing:
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    StartCoroutine(ChangePhaseRoutine());
                    break;
                case State.Turning:
                    m_phaseHandle.allowPhaseChange = false;
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                    m_movement.Stop();
                    break;
                case State.Attacking:
                    if (IsFacingTarget())
                    {
                        switch (m_chosenPattern)
                        {
                            case Pattern.Phase1Pattern1:
                                UpdateRangeCache(m_info.uppercutAttack.range, m_info.flameThrowerAttack.range);
                                ExecuteAttack(0);
                                break;
                            case Pattern.Phase1Pattern2:
                                UpdateRangeCache(m_info.uppercutAttack.range, m_info.flameThrowerAttack.range, m_info.spinAttack.range, m_info.firebeamAttack.range);
                                ExecuteAttack(1);
                                break;
                            case Pattern.Phase2Pattern1:
                                if(m_phaseHandle.currentPhase == Phase.PhaseTwo)
                                {
                                    if (m_currentPhaseIndex == 1)
                                    {
                                        UpdateRangeCache(m_info.uppercutAttack.range, m_info.flameThrowerAttack.range, m_info.spinAttack.range, m_info.shotgunBlastFireAttack.range, m_info.flameThrowerAttack.range);//change the flamethrower to flamethrower2range
                                        ExecuteAttack(2);
                                        //StartCoroutine(PunchAttackRoutine());
                                    }
                                    
                                }
                                m_stateHandle.OverrideState(State.ReevaluateSituation);
                                break;
                            case Pattern.Phase2Pattern2:
                                if (m_phaseHandle.currentPhase == Phase.PhaseTwo)
                                {
                                    if (m_currentPhaseIndex == 1)
                                    {
                                        UpdateRangeCache(m_info.uppercutAttack.range, m_info.spinAttack.range, m_info.shotgunBlastFireAttack.range);
                                        ExecuteAttack(3);
                                        //StartCoroutine(PunchAttackRoutine());
                                    }
                                }
                                m_stateHandle.OverrideState(State.ReevaluateSituation);
                                break;
                            case Pattern.Phase2Pattern3:
                                if (m_phaseHandle.currentPhase == Phase.PhaseTwo)
                                {
                                    if (m_currentPhaseIndex == 1)
                                    {
                                        UpdateRangeCache(m_info.spinAttack.range, m_info.firebeamAttack.range, m_info.meteorAttack.range);
                                        ExecuteAttack(4);
                                        //StartCoroutine(PunchAttackRoutine());
                                    }
                                }
                                m_stateHandle.OverrideState(State.ReevaluateSituation);
                                break;
                            case Pattern.Phase2Pattern4:
                                if (m_phaseHandle.currentPhase == Phase.PhaseTwo)
                                {
                                    if (m_currentPhaseIndex == 1)
                                    {
                                        UpdateRangeCache(m_info.firebeamAttack.range, m_info.flameThrowerAttack.range);//change the flamethrower to flamethrower2range
                                        ExecuteAttack(5);
                                        //StartCoroutine(PunchAttackRoutine());
                                    }
                                }
                                m_stateHandle.OverrideState(State.ReevaluateSituation);
                                break;
                        }
                    }
                    else
                    {
                        m_turnState = State.Attacking;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    break;

                case State.Chasing:
                    m_attackCount = 0;
                    DecidedOnAttack(false);
                    m_patternDecider.DecideOnAttack();
                    m_chosenPattern = m_patternDecider.chosenAttack.attack;
                    if (m_chosenPattern == m_previousPattern)
                    {
                        m_patternDecider.hasDecidedOnAttack = false;
                    }
                    if (m_patternDecider.hasDecidedOnAttack)
                    {
                        m_previousPattern = m_chosenPattern;
                        m_stateHandle.SetState(State.Attacking);
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
        }

        public override void ReturnToSpawnPoint()
        {
        }

        protected override void OnForbidFromAttackTarget()
        {
        }
    }
}