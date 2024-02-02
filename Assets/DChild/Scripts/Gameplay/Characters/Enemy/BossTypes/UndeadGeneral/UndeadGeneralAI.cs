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
using UnityEngine.Events;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Boss/UndeadGeneral")]
    public class UndeadGeneralAI : CombatAIBrain<UndeadGeneralAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField]
            private PhaseInfo<Phase> m_phaseInfo;
            public PhaseInfo<Phase> phaseInfo => m_phaseInfo;

            [SerializeField]
            private MovementInfo m_moveSlow = new MovementInfo();
            public MovementInfo moveSlow => m_moveSlow;
            [SerializeField]
            private MovementInfo m_moveMedium = new MovementInfo();
            public MovementInfo moveMedium => m_moveMedium;
            [SerializeField]
            private MovementInfo m_moveFast = new MovementInfo();
            public MovementInfo moveFast => m_moveFast;
            [SerializeField]
            private MovementInfo m_moveForwardGuardStance = new MovementInfo();
            public MovementInfo moveForwardGuardStance => m_moveForwardGuardStance;
            [SerializeField]
            private MovementInfo m_moveBackGuardStance = new MovementInfo();
            public MovementInfo moveBackGuardStance => m_moveBackGuardStance;
            [SerializeField]
            private MovementInfo m_dodgeHop = new MovementInfo();
            public MovementInfo dodgeHop => m_dodgeHop;

            [Title("Attack Behaviours")]
            [SerializeField]
            private SimpleAttackInfo m_oneHitComboAttack = new SimpleAttackInfo();
            public SimpleAttackInfo oneHitComboAttack => m_oneHitComboAttack;
            [SerializeField]
            private SimpleAttackInfo m_twoHitComboAttack = new SimpleAttackInfo();
            public SimpleAttackInfo twoHitComboAttack => m_twoHitComboAttack;
            [SerializeField, BoxGroup("Third Hit Combo")]
            private SimpleAttackInfo m_threeHitComboAttack = new SimpleAttackInfo();
            public SimpleAttackInfo threeHitComboAttack => m_threeHitComboAttack;
            [SerializeField]
            private SimpleAttackInfo m_runningSlashAttack = new SimpleAttackInfo();
            public SimpleAttackInfo runningSlashAttack => m_runningSlashAttack;
            [SerializeField]
            private SimpleAttackInfo m_swordThrustAttack = new SimpleAttackInfo();
            public SimpleAttackInfo swordThrustAttack => m_swordThrustAttack;
            [SerializeField, BoxGroup("Earth Shaker")]
            private SimpleAttackInfo m_earthShakerAttack = new SimpleAttackInfo();
            public SimpleAttackInfo earthShakerAttack => m_earthShakerAttack;
            [SerializeField, BoxGroup("Earth Shaker"), Range(0, 1)]
            private float m_earthShakerTrackingDelay;
            public float earthShakerTrackingDelay => m_earthShakerTrackingDelay;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Earth Shaker")]
            private string m_earthShakerDisappearAnimation;
            public string earthShakerDisappearAnimation => m_earthShakerDisappearAnimation;
            [SerializeField, BoxGroup("Special Thrust")]
            private SimpleAttackInfo m_specialThrustAttack = new SimpleAttackInfo();
            public SimpleAttackInfo specialThrustAttack => m_specialThrustAttack;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Special Thrust")]
            private string m_specialThrustStartAnimation;
            public string specialThrustStartAnimation => m_specialThrustStartAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Special Thrust")]
            private string m_specialThrustAirStartAnimation;
            public string specialThrustAirStartAnimation => m_specialThrustAirStartAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Special Thrust")]
            private string m_specialThrustFallAnimation;
            public string specialThrustFallAnimation => m_specialThrustFallAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Special Thrust")]
            private string m_specialThrustLandAnimation;
            public string specialThrustLandAnimation => m_specialThrustLandAnimation;
            [SerializeField, BoxGroup("Special Thrust")]
            private float m_specialThrustAirStartHeight;
            public float specialThrustAirStartHeight => m_specialThrustAirStartHeight;
            //[SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Special Thrust")]
            //private string m_specialThrustHitAnimation;
            //public string specialThrustHitAnimation => m_specialThrustHitAnimation;
            [SerializeField, BoxGroup("Dodge Attack")]
            private SimpleAttackInfo m_dodgeAttack = new SimpleAttackInfo();
            public SimpleAttackInfo dodgeAttack => m_dodgeAttack;
            [SerializeField, BoxGroup("Guard Attack")]
            private SimpleAttackInfo m_guardAttack = new SimpleAttackInfo();
            public SimpleAttackInfo guardAttack => m_guardAttack;
            [SerializeField, BoxGroup("Guard Attack")]
            private float m_guardAttackDashSpeed;
            public float guardAttackDashSpeed => m_guardAttackDashSpeed;
            [SerializeField, BoxGroup("Guard Attack"), ValueDropdown("GetAnimations")]
            private string m_guardTriggerAnimation;
            public string guardTriggerAnimation => m_guardTriggerAnimation;
            //[SerializeField, MinValue(0)]
            //private float m_attackCD;
            //public float attackCD => m_attackCD;

            [Title("Misc")]
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

            [Title("Animations")]
            [SerializeField]
            private BasicAnimationInfo m_intro1Animation;
            public BasicAnimationInfo intro1Animation => m_intro1Animation;
            [SerializeField]
            private BasicAnimationInfo m_intro2Animation;
            public BasicAnimationInfo intro2Animation => m_intro2Animation;
            [SerializeField]
            private BasicAnimationInfo m_intro3Animation;
            public BasicAnimationInfo intro3Animation => m_intro3Animation;
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleCombatAnimation;
            public BasicAnimationInfo idleCombatAnimation => m_idleCombatAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleRagedAnimation;
            public BasicAnimationInfo idleRagedAnimation => m_idleRagedAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleGuardAnimation;
            public BasicAnimationInfo idleGuardAnimation => m_idleGuardAnimation;
            [SerializeField]
            private BasicAnimationInfo m_idleToCombatTransitionAnimation;
            public BasicAnimationInfo idleToCombatTransitionAnimation => m_idleToCombatTransitionAnimation;
            [SerializeField]
            private BasicAnimationInfo m_turnAnimation;
            public BasicAnimationInfo turnAnimation => m_turnAnimation;
            [SerializeField]
            private BasicAnimationInfo m_moveFastAnticipationAnimation;
            public BasicAnimationInfo moveFastAnticipationAnimation => m_moveFastAnticipationAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchInplaceAnimation;
            public BasicAnimationInfo flinchInplaceAnimation => m_flinchInplaceAnimation;
            [SerializeField]
            private BasicAnimationInfo m_flinchKnockbackAnimation;
            public BasicAnimationInfo flinchKnockbackAnimation => m_flinchKnockbackAnimation;
            [SerializeField]
            private BasicAnimationInfo m_phasingEnragedAnimation;
            public BasicAnimationInfo phasingEnragedAnimation => m_phasingEnragedAnimation;
            [SerializeField]
            private BasicAnimationInfo m_defeatStartAnimation;
            public BasicAnimationInfo defeatStartAnimation => m_defeatStartAnimation;
            [SerializeField]
            private BasicAnimationInfo m_defeatLoopAnimation;
            public BasicAnimationInfo defeatLoopAnimation => m_defeatLoopAnimation;
            [SerializeField]
            private BasicAnimationInfo m_victory1Animation;
            public BasicAnimationInfo victory1Animation => m_victory1Animation;
            [SerializeField]
            private BasicAnimationInfo m_victory2Animation;
            public BasicAnimationInfo victory2Animation => m_victory2Animation;

            [Title("FX")]
            [SerializeField]
            private GameObject m_fx;
            public GameObject fx => m_fx;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_dustLandEvent;
            public string dustLandEvent => m_dustLandEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_dustTakeOffEvent;
            public string dustTakeOffEvent => m_dustTakeOffEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_leftFootstepSoundEvent;
            public string leftFootstepSoundEvent => m_leftFootstepSoundEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_rightFootstepSoundEvent;
            public string rightFootstepSoundEvent => m_rightFootstepSoundEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_rootMoveEndEvent;
            public string rootMoveEndEvent => m_rootMoveEndEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_rootMoveStartEvent;
            public string rootMoveStartEvent => m_rootMoveStartEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_swordSlash1Event;
            public string swordSlash1Event => m_swordSlash1Event;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_swordSlash2Event;
            public string swordSlash2Event => m_swordSlash2Event;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_swordSlash3Event;
            public string swordSlash3Event => m_swordSlash3Event;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_swordStabEvent;
            public string swordStabEvent => m_swordStabEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_heavySlashEvent;
            public string heavySlashEvent => m_heavySlashEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_earthShakerEvent;
            public string earthShakerEvent => m_earthShakerEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_specialThrustEvent;
            public string specialThrustEvent => m_specialThrustEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_jumpEvent;
            public string jumpEvent => m_jumpEvent;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_moveSlow.SetData(m_skeletonDataAsset);
                m_moveMedium.SetData(m_skeletonDataAsset);
                m_moveFast.SetData(m_skeletonDataAsset);
                m_moveForwardGuardStance.SetData(m_skeletonDataAsset);
                moveBackGuardStance.SetData(m_skeletonDataAsset);
                m_dodgeHop.SetData(m_skeletonDataAsset);
                m_guardAttack.SetData(m_skeletonDataAsset);
                m_oneHitComboAttack.SetData(m_skeletonDataAsset);
                m_twoHitComboAttack.SetData(m_skeletonDataAsset);
                m_threeHitComboAttack.SetData(m_skeletonDataAsset);
                m_runningSlashAttack.SetData(m_skeletonDataAsset);
                m_earthShakerAttack.SetData(m_skeletonDataAsset);
                m_earthShakerAttack.SetData(m_skeletonDataAsset);
                m_swordThrustAttack.SetData(m_skeletonDataAsset);
                m_specialThrustAttack.SetData(m_skeletonDataAsset);
                m_dodgeAttack.SetData(m_skeletonDataAsset);

                m_intro1Animation.SetData(m_skeletonDataAsset);
                m_intro2Animation.SetData(m_skeletonDataAsset);
                m_intro3Animation.SetData(m_skeletonDataAsset);
                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_idleCombatAnimation.SetData(m_skeletonDataAsset);
                m_idleRagedAnimation.SetData(m_skeletonDataAsset);
                m_idleGuardAnimation.SetData(m_skeletonDataAsset);
                m_idleToCombatTransitionAnimation.SetData(m_skeletonDataAsset);
                m_turnAnimation.SetData(m_skeletonDataAsset);
                m_moveFastAnticipationAnimation.SetData(m_skeletonDataAsset);
                m_flinchInplaceAnimation.SetData(m_skeletonDataAsset);
                m_flinchKnockbackAnimation.SetData(m_skeletonDataAsset);
                m_phasingEnragedAnimation.SetData(m_skeletonDataAsset);
                m_defeatStartAnimation.SetData(m_skeletonDataAsset);
                m_defeatLoopAnimation.SetData(m_skeletonDataAsset);
                m_victory1Animation.SetData(m_skeletonDataAsset);
                m_victory2Animation.SetData(m_skeletonDataAsset);
#endif
            }
        }

        [System.Serializable]
        public class PhaseInfo : IPhaseInfo
        {
            [SerializeField]
            private int m_phaseIndex;
            public int phaseIndex => m_phaseIndex;
            [SerializeField]
            private List<float> m_fullCD;
            public List<float> fullCD => m_fullCD;
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

        //private enum Pattern
        //{
        //    AttackPattern1,
        //    AttackPattern2,
        //    AttackPattern3,
        //    WaitAttackEnd,
        //}

        private enum Attack
        {
            OneHitCombo,
            TwoHitCombo,
            ThreeHitCombo,
            RunningSlash,
            SwordThrust,
            SpecialThrust,
            EarthShaker,
            EarthShakerPlus,
            WaitAttackEnd,
        }

        public enum Phase
        {
            PhaseOne,
            PhaseTwo,
            PhaseThree,
            PhaseFour,
            Wait,
        }

        private bool[] m_attackUsed;
        private List<Attack> m_attackCache;
        private List<float> m_attackRangeCache;

        [SerializeField, TabGroup("Reference")]
        private Boss m_boss;
        [SerializeField, TabGroup("Reference")]
        private Hitbox m_hitbox;
        //[SerializeField, TabGroup("Modules")]
        //private TransformTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Cinematic")]
        private BlackDeathCinematicPlayah m_cinematic;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Hurtbox")]
        private Collider2D m_swordSlash1BB;
        [SerializeField, TabGroup("Hurtbox")]
        private Collider2D m_swordSlash2BB;
        [SerializeField, TabGroup("Hurtbox")]
        private Collider2D m_swordSlash3BB;
        [SerializeField, TabGroup("Hurtbox")]
        private Collider2D m_swordStabBB;
        [SerializeField, TabGroup("Hurtbox")]
        private Collider2D m_heavySlashBB;
        [SerializeField, TabGroup("Hurtbox")]
        private Collider2D m_earthShakerBB;
        [SerializeField, TabGroup("Hurtbox")]
        private Collider2D m_specialThrustBB;

        [SerializeField, TabGroup("FX")]
        private ParticleFX m_earthShakerExplosionFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_earthShakerGlitterFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_earthShakerSwordTrailFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_enragedFX;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_trailFX;
        [SerializeField, TabGroup("FX")]
        private Transform m_fxParent;
        [SerializeField, TabGroup("FX")]
        private ParticleFX m_swordThrustChargeFX;

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
        //private Pattern m_chosenPattern;
        //private Pattern m_previousPattern;
        private Attack m_currentAttack;
        private float m_currentAttackRange;
        [SerializeField]
        private Health m_percentHealth;
        [SerializeField]
        private UnityEvent m_onTrigger;

        private int m_currentPhaseIndex;
        private Coroutine m_currentAttackCoroutine;
        private Coroutine m_counterAttackCoroutine;
        private Coroutine m_hurtboxCoroutine;
        private Coroutine m_changePhaseCoroutine;

        private Vector2 m_lastTargetPos;
        private string m_currentIdleAnimation;
        private string m_currentIdleTransitionAnimation;
        private string m_currentMovementAnimation;
        private float m_currentMovementSpeed;
        private float m_currentThirdSlashDashSpeed;
        private float m_currentCD;
        private float m_pickedCD;
        private int m_hitCounter;
        private bool m_canBlockCounter;
        private Collider2D m_currentHurtbox;
        private List<float> m_currentFullCD;
        private bool m_isDetecting;

        private void ApplyPhaseData(PhaseInfo obj)
        {
            m_attackCache.Clear();
            m_attackRangeCache.Clear();
            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseOne:
                    m_currentIdleAnimation = m_info.idleCombatAnimation.animation;
                    m_currentIdleTransitionAnimation = m_info.idleToCombatTransitionAnimation.animation;
                    m_currentMovementAnimation = m_info.moveMedium.animation;
                    m_currentMovementSpeed = m_info.moveMedium.speed;
                    m_currentThirdSlashDashSpeed = 50;
                    AddToAttackCache(Attack.TwoHitCombo, Attack.RunningSlash);
                    AddToRangeCache(m_info.twoHitComboAttack.range, m_info.runningSlashAttack.range);
                    break;
                case Phase.PhaseTwo:
                    m_currentIdleAnimation = m_info.idleCombatAnimation.animation;
                    m_currentIdleTransitionAnimation = m_info.idleToCombatTransitionAnimation.animation;
                    m_currentMovementAnimation = m_info.moveMedium.animation;
                    m_currentMovementSpeed = m_info.moveMedium.speed;
                    m_currentThirdSlashDashSpeed = 50;
                    AddToAttackCache(Attack.ThreeHitCombo, Attack.RunningSlash, Attack.EarthShaker);
                    AddToRangeCache(m_info.threeHitComboAttack.range, m_info.runningSlashAttack.range, m_info.earthShakerAttack.range);
                    break;
                case Phase.PhaseThree:
                    m_currentIdleAnimation = m_info.idleRagedAnimation.animation;
                    m_currentIdleTransitionAnimation = m_info.idleRagedAnimation.animation;
                    m_currentMovementAnimation = m_info.moveFast.animation;
                    m_currentMovementSpeed = m_info.moveFast.speed;
                    m_currentThirdSlashDashSpeed = 100;
                    AddToAttackCache(Attack.ThreeHitCombo, Attack.RunningSlash, Attack.EarthShaker, Attack.SpecialThrust);
                    AddToRangeCache(m_info.threeHitComboAttack.range, m_info.runningSlashAttack.range, m_info.earthShakerAttack.range, m_info.specialThrustAttack.range);
                    break;
                case Phase.PhaseFour:
                    m_currentIdleAnimation = m_info.idleRagedAnimation.animation;
                    m_currentIdleTransitionAnimation = m_info.idleRagedAnimation.animation;
                    m_currentMovementAnimation = m_info.moveFast.animation;
                    m_currentMovementSpeed = m_info.moveFast.speed;
                    m_currentThirdSlashDashSpeed = 200;
                    AddToAttackCache(Attack.ThreeHitCombo, Attack.RunningSlash, Attack.EarthShakerPlus, Attack.SpecialThrust);
                    AddToRangeCache(m_info.threeHitComboAttack.range, m_info.runningSlashAttack.range, m_info.earthShakerAttack.range, m_info.specialThrustAttack.range);
                    break;
            }
            m_attackUsed = new bool[m_attackCache.Count];
            m_currentPhaseIndex = obj.phaseIndex;
            if (m_currentFullCD.Count != 0)
            {
                m_currentFullCD.Clear();
            }
            for (int i = 0; i < obj.fullCD.Count; i++)
            {
                m_currentFullCD.Add(obj.fullCD[i]);
            }
        }

        private void ChangeState()
        {
            //StopCurrentAttackRoutine();
            //SetAIToPhasing();
            StartCoroutine(SmartChangePhaseRoutine());
        }

        private void HealthChecker()
        {
            if(m_percentHealth.currentValue <= 300)
            {
                Debug.Log("30% health");
                m_onTrigger?.Invoke();
            }
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs)
        {
            if (m_stateHandle.currentState != State.Phasing /*&& !m_hasPhaseChanged*/)
            {
                m_stateHandle.OverrideState(State.Turning);
            }
        }

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            if (damageable != null)
            {
                Debug.Log("UG Ecnountered Player");
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
            if (m_currentAttackCoroutine == null)
            {
                if (m_stateHandle.currentState != State.Phasing)
                {
                    m_animation.animationState.TimeScale = 1f;
                    m_stateHandle.ApplyQueuedState();
                }
                m_phaseHandle.allowPhaseChange = true;
            }
        }

        private void OnDamageTaken(object sender, Damageable.DamageEventArgs eventArgs)
        {
            if (m_counterAttackCoroutine == null && m_changePhaseCoroutine == null)
            {
                if (m_hitCounter < 5)
                    m_hitCounter++;
                else
                    m_hitbox.SetCanBlockDamageState(true);

                if (m_hitbox.canBlockDamage)
                {
                    if (m_currentAttackCoroutine != null)
                    {
                        StopCoroutine(m_currentAttackCoroutine);
                        m_currentAttackCoroutine = null;
                        m_attackDecider.hasDecidedOnAttack = false;
                    }

                    m_currentHurtbox.enabled = false;
                    m_trailFX.Stop();
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    m_counterAttackCoroutine = UnityEngine.Random.Range(0, 2) == 0 ? StartCoroutine(DodgeAttackRoutine()) : StartCoroutine(GuardAttackRoutine(false, false));
                    //m_counterAttackCoroutine = StartCoroutine(GuardAttackRoutine(false, false));
                }
            }
        }

        private void OnDamageBlocked(object sender, Damageable.DamageEventArgs eventArgs)
        {
            if (m_canBlockCounter)
            {
                if (m_currentAttackCoroutine != null)
                {
                    StopCoroutine(m_currentAttackCoroutine);
                    m_currentAttackCoroutine = null;
                }

                StopCoroutine(m_counterAttackCoroutine);
                m_counterAttackCoroutine = StartCoroutine(GuardAttackRoutine(true, Vector2.Distance(m_targetInfo.position, transform.position) > m_info.oneHitComboAttack.range ? false : true));
            }
        }

        private IEnumerator IntroRoutine()
        {
            m_stateHandle.Wait(State.Chasing);
            m_movement.Stop();
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            //m_cinematic.PlayCinematic(1, false);
            m_animation.animationState.TimeScale = 1;
            m_animation.EnableRootMotion(true, false);
            m_hitbox.Enable();
            m_hitbox.SetInvulnerability(Invulnerability.None);
            //m_stateHandle.ApplyQueuedState();
            ChooseAttack();
            StartCoroutine(RunAnticipationRoutine());
            yield return null;
        }

        private IEnumerator SmartChangePhaseRoutine()
        {
            yield return new WaitWhile(() => !m_phaseHandle.allowPhaseChange);
            StopCurrentAttackRoutine();
            SetAIToPhasing();
            yield return null;
        }

        private void SetAIToPhasing()
        {
            //m_hasPhaseChanged = true;
            m_phaseHandle.ApplyChange();
            m_animation.DisableRootMotion();
            m_hitCounter = 0;
            m_animation.SetEmptyAnimation(0, 0);
            m_stateHandle.OverrideState(State.Phasing);
        }

        private void StopCurrentAttackRoutine()
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
        }

        private IEnumerator ChangePhaseRoutine()
        {
            //m_stateHandle.Wait(State.ReevaluateSituation);
            enabled = false;
            m_hitbox.Disable();
            m_trailFX.Stop();
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.phasingEnragedAnimation, false);
            m_animation.animationState.GetCurrent(0).MixDuration = 0;
            yield return new WaitWhile(() => m_animation.animationState.GetCurrent(0).AnimationTime < 0.5f);
            m_enragedFX.Play();
            yield return new WaitWhile (() => m_animation.animationState.GetCurrent(0).AnimationTime < m_animation.animationState.GetCurrent(0).AnimationEnd * 0.8f) ;
            m_enragedFX.Stop();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.phasingEnragedAnimation);
            //m_animation.SetAnimation(0, m_info.moveFastAnticipationAnimation, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.moveFastAnticipationAnimation);
            //m_trailFX.Play();
            m_hitbox.Enable();
            m_changePhaseCoroutine = null;
            //m_stateHandle.ApplyQueuedState();
            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseTwo:
                    m_currentAttack = Attack.EarthShaker;
                    break;
                case Phase.PhaseThree:
                    m_currentAttack = Attack.SpecialThrust;
                    break;
            }
            m_stateHandle.OverrideState(State.Attacking);
            yield return null;
            enabled = true;
        }
        #region Attacks

        private IEnumerator OneHitComboAttackRoutine()
        {
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.oneHitComboAttack.animation, false);
            //yield return new WaitForSeconds(0.5f);
            //m_character.physics.SetVelocity(m_info.shoulderBashVelocity.x * transform.localScale.x, 0);
            //yield return new WaitForSeconds(0.15f);
            //m_movement.Stop();
            //while (m_animation.animationState.GetCurrent(0).AnimationTime < m_animation.animationState.GetCurrent(0).AnimationEnd)
            //{
            //    if (!IsFacingTarget()) CustomTurn();
            //    yield return new WaitForSeconds(m_animation.animationState.GetCurrent(0).AnimationTime > 0.85f ? 0 : 1f);
            //    yield return null;
            //}

            //m_animation.AddAnimation(0, m_info.idleGuardAnimation, false, 0).TimeScale = 5f;
            //m_animation.animationState.GetCurrent(0).MixDuration = 0;
            m_animation.AddAnimation(0, m_currentIdleTransitionAnimation, m_currentIdleTransitionAnimation == m_info.idleToCombatTransitionAnimation.animation ? false : true, 0).TimeScale = m_phaseHandle.currentPhase == Phase.PhaseThree ? 5 : 1;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_currentIdleTransitionAnimation);
            //yield return new WaitForSeconds(3f);
            m_attackDecider.hasDecidedOnAttack = false;
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator TwoHitComboAttackRoutine()
        {
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.oneHitComboAttack.animation, false);
            m_animation.AddAnimation(0, m_info.twoHitComboAttack.animation, false, 0).MixDuration = 0;
            //yield return new WaitForSeconds(0.5f);
            //m_character.physics.SetVelocity(m_info.shoulderBashVelocity.x * transform.localScale.x, 0);
            //yield return new WaitForSeconds(0.15f);
            //m_movement.Stop();
            //while (m_animation.animationState.GetCurrent(0).AnimationTime < m_animation.animationState.GetCurrent(0).AnimationEnd)
            //{
            //    if (!IsFacingTarget()) CustomTurn();
            //    yield return new WaitForSeconds(m_animation.animationState.GetCurrent(0).AnimationTime > 0.85f ? 0 : 1f);
            //    yield return null;
            //}
            //m_animation.AddAnimation(0, m_info.idleGuardAnimation, false, 0).TimeScale = 5f;
            //m_animation.animationState.GetCurrent(0).MixDuration = 0;
            m_animation.AddAnimation(0, m_currentIdleTransitionAnimation, m_currentIdleTransitionAnimation == m_info.idleToCombatTransitionAnimation.animation ? false : true, 0).MixDuration = 0;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_currentIdleTransitionAnimation);
            //yield return new WaitForSeconds(3f);
            m_attackDecider.hasDecidedOnAttack = false;
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator ThreeHitComboAttackRoutine()
        {
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.oneHitComboAttack.animation, false);
            m_animation.AddAnimation(0, m_info.twoHitComboAttack.animation, false, 0);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.twoHitComboAttack.animation);
            m_animation.DisableRootMotion();
            m_animation.AddAnimation(0, m_info.threeHitComboAttack.animation, false, 0).MixDuration = 0f;
            var waitTime = m_animation.animationState.GetCurrent(0).AnimationEnd * 0.3f;
            yield return new WaitForSeconds(waitTime);
            m_lastTargetPos = m_targetInfo.position;
            //var attackTimeScale = m_info.earthShakerAttack.range / Mathf.Abs(m_lastTargetPos.x - transform.position.x);
            //m_animation.animationState.TimeScale = attackTimeScale;
            //var adaptiveMoveSpeed = Mathf.Abs(m_lastTargetPos.x - transform.position.x) / (m_currentThirdSlashDashSpeed * 1.25f);
            //adaptiveMoveSpeed = adaptiveMoveSpeed * m_currentThirdSlashDashSpeed;
            var adaptiveMoveSpeed = Mathf.Abs(m_lastTargetPos.x - transform.position.x) / (m_currentThirdSlashDashSpeed * 1.25f);
            adaptiveMoveSpeed = Mathf.Abs(m_lastTargetPos.x - transform.position.x) < m_info.threeHitComboAttack.range ? adaptiveMoveSpeed * m_currentThirdSlashDashSpeed : m_currentThirdSlashDashSpeed;
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            m_animation.DisableRootMotion();
            while (m_animation.animationState.GetCurrent(0).AnimationTime < (m_animation.animationState.GetCurrent(0).AnimationEnd * 0.5f))
            {
                m_movement.MoveTowards(Vector2.one * transform.localScale.x, adaptiveMoveSpeed);
                yield return null;
            }
            m_animation.animationState.TimeScale = 1f;
            m_movement.Stop();
            yield return new WaitUntil(() => m_animation.animationState.GetCurrent(0).AnimationTime >= (m_animation.animationState.GetCurrent(0).AnimationEnd * 0.75f));
            m_animation.SetAnimation(0, m_currentIdleTransitionAnimation, m_currentIdleTransitionAnimation == m_info.idleToCombatTransitionAnimation.animation ? false : true).TimeScale = m_phaseHandle.currentPhase == Phase.PhaseThree ? 5 : 1;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_currentIdleTransitionAnimation);
            //yield return new WaitForSeconds(3f);
            m_attackDecider.hasDecidedOnAttack = false;
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator RunningSlasAttackhRoutine()
        {
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.dodgeHop.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.dodgeHop.animation);
            //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.moveFastAnticipationAnimation)
            //{
            //}
            m_animation.SetAnimation(0, m_info.moveFastAnticipationAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.moveFastAnticipationAnimation);
            m_trailFX.Play();
            while (Mathf.Abs(m_lastTargetPos.x - transform.position.x) >= 30f)
            {
                m_animation.SetAnimation(0, m_info.moveMedium.animation, true);
                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.moveMedium.speed);
                yield return null;
            }
            m_trailFX.Stop();
            m_animation.DisableRootMotion();
            //m_character.physics.AddForce(transform.right * 1f, ForceMode2D.Impulse);
            //m_character.physics.SetVelocity(transform.localScale.x * 75f);
            m_animation.SetAnimation(0, m_info.runningSlashAttack.animation, false).MixDuration = 0;
            var time = 0f;
            while (time < 0.35f)
            {
                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_currentMovementSpeed);
                time += Time.deltaTime;
                yield return null;
            }
            m_animation.animationState.TimeScale = 1f;
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.runningSlashAttack.animation);
            m_movement.Stop();
            yield return new WaitUntil(() => m_animation.animationState.GetCurrent(0).AnimationTime >= (m_animation.animationState.GetCurrent(0).AnimationEnd * 0.5f));
            m_animation.SetAnimation(0, m_currentIdleTransitionAnimation, m_currentIdleTransitionAnimation == m_info.idleToCombatTransitionAnimation.animation ? false : true).TimeScale = m_phaseHandle.currentPhase == Phase.PhaseThree ? 5 : 1;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_currentIdleTransitionAnimation);
            //yield return new WaitForSeconds(3f);
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator DodgeAttackRoutine()
        {
            enabled = false;
            if (!IsFacingTarget()) CustomTurn();
            m_animation.EnableRootMotion(true, false);
            m_hitCounter = 0;
            m_hitbox.SetCanBlockDamageState(false);
            m_animation.SetAnimation(0, m_info.dodgeHop.animation, false);
            m_animation.AddAnimation(0, m_info.oneHitComboAttack.animation, false, 0);
            m_animation.AddAnimation(0, m_info.twoHitComboAttack.animation, false, 0);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.twoHitComboAttack.animation);
            m_animation.DisableRootMotion();
            m_animation.AddAnimation(0, m_info.threeHitComboAttack.animation, false, 0).MixDuration = 0f;
            var waitTime = m_animation.animationState.GetCurrent(0).AnimationEnd * 0.3f;
            yield return new WaitForSeconds(waitTime);
            m_lastTargetPos = m_targetInfo.position;
            //var attackTimeScale = m_info.earthShakerAttack.range / Mathf.Abs(m_lastTargetPos.x - transform.position.x);
            //m_animation.animationState.TimeScale = attackTimeScale;
            //var adaptiveMoveSpeed = Mathf.Abs(m_lastTargetPos.x - transform.position.x) / (m_currentThirdSlashDashSpeed * 1.25f);
            //adaptiveMoveSpeed = adaptiveMoveSpeed * m_currentThirdSlashDashSpeed;
            var adaptiveMoveSpeed = Mathf.Abs(m_lastTargetPos.x - transform.position.x) / (m_currentThirdSlashDashSpeed * 1.25f);
            adaptiveMoveSpeed = adaptiveMoveSpeed * m_currentThirdSlashDashSpeed;
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            m_animation.DisableRootMotion();
            while (m_animation.animationState.GetCurrent(0).AnimationTime < (m_animation.animationState.GetCurrent(0).AnimationEnd * 0.5f))
            {
                m_movement.MoveTowards(Vector2.one * transform.localScale.x, adaptiveMoveSpeed);
                yield return null;
            }
            m_animation.animationState.TimeScale = 1f;
            m_movement.Stop();
            yield return new WaitUntil(() => m_animation.animationState.GetCurrent(0).AnimationTime >= (m_animation.animationState.GetCurrent(0).AnimationEnd * 0.75f));
            m_animation.SetAnimation(0, m_currentIdleTransitionAnimation, m_currentIdleTransitionAnimation == m_info.idleToCombatTransitionAnimation.animation ? false : true).TimeScale = m_phaseHandle.currentPhase == Phase.PhaseThree ? 5 : 1;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_currentIdleTransitionAnimation);
            //yield return new WaitForSeconds(3f);
            m_attackDecider.hasDecidedOnAttack = false;
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_counterAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
            enabled = true;
        }

        private IEnumerator GuardAttackRoutine(bool hasBlocked, bool willDodge)
        {
            enabled = false;
            m_hitCounter = 0;
            if (!hasBlocked)
            {
                m_hitbox.SetCanBlockDamageState(true);
                m_animation.SetAnimation(0, willDodge ? m_currentIdleAnimation : m_info.idleGuardAnimation.animation, true);
                var time = 0f;
                yield return new WaitForSeconds(0.1f);
                m_canBlockCounter = true;
                while (time < 2f)
                {
                    if (!IsFacingTarget())
                    {
                        CustomTurn();
                    }
                    time += Time.deltaTime;
                    yield return null;
                }
                //yield return new WaitForSeconds(3f);
            }
            m_hitbox.SetCanBlockDamageState(false);
            m_canBlockCounter = false;
            if (!willDodge)
            {
                if (!IsFacingTarget()) CustomTurn();
                m_animation.EnableRootMotion(true, false);
                m_animation.SetAnimation(0, m_info.guardTriggerAnimation, false).MixDuration = 0;
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.guardTriggerAnimation);
                m_animation.DisableRootMotion();
                m_animation.SetAnimation(0, m_info.guardAttack.animation, false);
                var waitTime = m_animation.animationState.GetCurrent(0).AnimationEnd * 0.1f;
                yield return new WaitForSeconds(waitTime);
                if (!IsFacingTarget())
                {
                    CustomTurn();
                }
                m_swordStabBB.enabled = true;
                var time = 0f;
                while (time < 0.5f)
                {
                    m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.guardAttackDashSpeed);
                    time += Time.deltaTime;
                    yield return null;
                }
                //yield return new WaitForSeconds(0.5f);
                m_swordStabBB.enabled = false;
                m_animation.animationState.TimeScale = 1f;
                m_movement.Stop();
                m_animation.AddAnimation(0, m_currentIdleTransitionAnimation, m_currentIdleTransitionAnimation == m_info.idleToCombatTransitionAnimation.animation ? false : true, 0).TimeScale = m_phaseHandle.currentPhase == Phase.PhaseThree ? 5 : 1;
                yield return new WaitForAnimationComplete(m_animation.animationState, m_currentIdleTransitionAnimation);
                m_attackDecider.hasDecidedOnAttack = false;
                m_currentAttackCoroutine = null;
                m_counterAttackCoroutine = null;
                m_stateHandle.ApplyQueuedState();
                enabled = true;
            }
            else
            {
                m_currentAttackCoroutine = StartCoroutine(DodgeAttackRoutine());
            }
            //m_currentAttackCoroutine = StartCoroutine(SpecialThrustAttackRoutine());
            yield return null;
        }

        private IEnumerator EarthShakerAttackRoutine()
        {
            m_animation.EnableRootMotion(true, false);
            m_earthShakerExplosionFX.Play();
            m_earthShakerGlitterFX.Play();
            m_earthShakerSwordTrailFX.Play();
            m_hitbox.Disable();
            m_animation.SetAnimation(0, m_info.earthShakerDisappearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.earthShakerDisappearAnimation);
            transform.position = new Vector2(m_targetInfo.position.x, GroundPosition().y);
            m_animation.animationState.TimeScale = 1f;
            m_animation.SetAnimation(0, m_info.earthShakerAttack.animation, false).MixDuration = 0;
            m_animation.AddAnimation(0, m_currentIdleTransitionAnimation, m_currentIdleTransitionAnimation == m_info.idleToCombatTransitionAnimation.animation ? false : true, 0).TimeScale = m_phaseHandle.currentPhase == Phase.PhaseThree ? 5 : 1;
            yield return new WaitForSeconds(1.0f);
            m_earthShakerGlitterFX.Stop();
            EarthShaker();
            yield return new WaitForAnimationComplete(m_animation.animationState, m_currentIdleTransitionAnimation);
            m_hitbox.Enable();
            m_earthShakerExplosionFX.transform.SetParent(null);
            m_attackDecider.hasDecidedOnAttack = false;
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }
        
        private IEnumerator SpecialThrustAttackRoutine()
        {
            //m_animation.SetAnimation(0, m_info.specialThrustStartAnimation, false);
            m_swordThrustChargeFX.Play();
            var startAnimation = UnityEngine.Random.Range(0, 2) == 1 ? m_info.specialThrustStartAnimation : m_info.specialThrustAirStartAnimation;
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, startAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, startAnimation);
            m_animation.EnableRootMotion(true, true);
            m_animation.SetAnimation(0, m_info.specialThrustAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.specialThrustAttack.animation);
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.specialThrustFallAnimation, false).MixDuration = 0.25f;
            yield return new WaitUntil(() => m_groundSensor.allRaysDetecting);
            m_animation.SetAnimation(0, m_info.specialThrustLandAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.specialThrustLandAnimation);
            m_animation.SetAnimation(0, m_currentIdleTransitionAnimation, m_currentIdleTransitionAnimation == m_info.idleToCombatTransitionAnimation.animation ? false : true).TimeScale = m_phaseHandle.currentPhase == Phase.PhaseThree ? 5 : 1;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_currentIdleTransitionAnimation);
            m_animation.DisableRootMotion();
            m_attackDecider.hasDecidedOnAttack = false;
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        #endregion

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            StopAllCoroutines();
            m_currentHurtbox.enabled = false;
            m_hitbox.Disable();
            m_animation.DisableRootMotion();
            m_movement.Stop();
            m_trailFX.Stop();
            StartCoroutine(DefeatRoutine());
        }

        private IEnumerator DefeatRoutine()
        {
            this.transform.SetParent(null);
            m_animation.SetAnimation(0, m_info.defeatStartAnimation, false).MixDuration = 0;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.moveFastAnticipationAnimation);
            m_animation.SetAnimation(0, m_info.defeatLoopAnimation, true);
            m_isDetecting = false;
            enabled = false;
            yield return null;
        }

        #region Movement
        private IEnumerator RunAnticipationRoutine()
        {
            if (m_currentMovementAnimation != m_info.moveSlow.animation)
            {
                m_animation.EnableRootMotion(true, false);
                //m_animation.SetAnimation(0, m_info.specialThrustStartAnimation, false);
                m_animation.SetAnimation(0, m_info.moveFastAnticipationAnimation, false).MixDuration = 0;
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.moveFastAnticipationAnimation);
                m_trailFX.Play();
            }
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private void MoveToTarget(float targetRange)
        {
            if (!IsTargetInRange(targetRange) && m_groundSensor.isDetecting /*&& !m_wallSensor.isDetecting && m_edgeSensor.isDetecting*/)
            {
                m_animation.EnableRootMotion(true, false);
                m_animation.SetAnimation(0, m_currentMovementAnimation, true);
                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_currentMovementSpeed);
            }
            else
            {
                m_movement.Stop();
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
        }
        #endregion

        private void UpdateAttackDeciderList()
        {
            //m_patternDecider.SetList(new AttackInfo<Pattern>(Pattern.AttackPattern1, m_info.targetDistanceTolerance),
            //                        new AttackInfo<Pattern>(Pattern.AttackPattern2, m_info.targetDistanceTolerance),
            //                        new AttackInfo<Pattern>(Pattern.AttackPattern3, m_info.targetDistanceTolerance));
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.OneHitCombo, m_info.oneHitComboAttack.range)
                                    , new AttackInfo<Attack>(Attack.TwoHitCombo, m_info.twoHitComboAttack.range)
                                    , new AttackInfo<Attack>(Attack.ThreeHitCombo, m_info.threeHitComboAttack.range)
                                    , new AttackInfo<Attack>(Attack.RunningSlash, m_info.runningSlashAttack.range)
                                    , new AttackInfo<Attack>(Attack.EarthShaker, m_info.earthShakerAttack.range)
                                    , new AttackInfo<Attack>(Attack.SpecialThrust, m_info.specialThrustAttack.range));
            m_attackDecider.hasDecidedOnAttack = false;
        }

        private Vector2 GroundPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1000, DChildUtility.GetEnvironmentMask());
            return hit.point;
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

        //void UpdateRangeCache(params float[] list)
        //{
        //    for (int i = 0; i < list.Length; i++)
        //    {
        //        m_attackRangeCache[i] = list[i];
        //    }
        //}

        protected override void Awake()
        {
            base.Awake();
            m_turnHandle.TurnDone += OnTurnDone;
            m_damageable.DamageTaken += OnDamageTaken;
            m_damageable.DamageTaken += OnDamageBlocked;
            //m_patternDecider = new RandomAttackDecider<Pattern>();
            m_attackDecider = new RandomAttackDecider<Attack>();
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            UpdateAttackDeciderList();
            m_attackCache = new List<Attack>();
            AddToAttackCache(Attack.OneHitCombo, Attack.TwoHitCombo, Attack.ThreeHitCombo, Attack.RunningSlash, Attack.SwordThrust, Attack.SpecialThrust, Attack.EarthShaker, Attack.EarthShakerPlus);
            m_attackRangeCache = new List<float>();
            AddToRangeCache(m_info.oneHitComboAttack.range, m_info.twoHitComboAttack.range, m_info.threeHitComboAttack.range, m_info.runningSlashAttack.range, m_info.swordThrustAttack.range, m_info.specialThrustAttack.range, m_info.earthShakerAttack.range, m_info.earthShakerAttack.range);
            m_attackUsed = new bool[m_attackCache.Count];
            m_currentFullCD = new List<float>();
        }

        private void SwordThrustRootMoveEnd()
        {
            m_movement.Stop();
        }

        private void SwordThrustRootMoveStart()
        {
            m_character.physics.SetVelocity(transform.right * 5f);
        }

        private void SwordSlash1()
        {
            m_currentHurtbox = m_swordSlash1BB;
            m_hurtboxCoroutine = StartCoroutine(BoundingBoxRoutine(0.25f));
        }

        private void SwordSlash2()
        {
            m_currentHurtbox = m_swordSlash2BB;
            m_hurtboxCoroutine = StartCoroutine(BoundingBoxRoutine(0.25f));
        }

        private void SwordSlash3()
        {
            m_currentHurtbox = m_swordSlash3BB;
            m_hurtboxCoroutine = StartCoroutine(BoundingBoxRoutine(0.25f));
        }

        private void SwordStab()
        {
            m_currentHurtbox = m_swordStabBB;
            m_hurtboxCoroutine = StartCoroutine(BoundingBoxRoutine(0.25f));
        }

        private void HeavySlash()
        {
            m_currentHurtbox = m_heavySlashBB;
            m_hurtboxCoroutine = StartCoroutine(BoundingBoxRoutine(0.25f));
        }

        private void EarthShaker()
        {
            //m_earthShakerFX.Play();
            StartCoroutine(EarthShakerBBRoutine(5f));
            m_currentHurtbox = m_earthShakerBB;
            m_hurtboxCoroutine = StartCoroutine(BoundingBoxRoutine(0.50f));
        }

        private void SpecialThrust()
        {
            m_currentHurtbox = m_specialThrustBB;
            m_hurtboxCoroutine = StartCoroutine(BoundingBoxRoutine(0.5f));
        }

        private void JumpEvent()
        {
            m_character.physics.AddForce(new Vector2(0, m_info.specialThrustAirStartHeight), ForceMode2D.Impulse);
        }

        private IEnumerator EarthShakerBBRoutine(float expandSpeed)
        {
            m_earthShakerBB.transform.localScale = Vector3.one * 0.1f;
            var offset = 0f;
            while (m_earthShakerBB.transform.localScale.x < 0.95f)
            {
                offset = Time.deltaTime * expandSpeed;
                m_earthShakerBB.transform.localScale += new Vector3(m_earthShakerBB.transform.localScale.x * offset, m_earthShakerBB.transform.localScale.y * offset);
                yield return null;
            }
            m_earthShakerBB.transform.localScale = Vector3.one;
            //yield return new WaitForSeconds(1f);
            yield return null;
        }

        private IEnumerator BoundingBoxRoutine(/*Collider2D hurtbox,*/ float duration)
        {
            m_currentHurtbox.enabled = true;
            yield return new WaitForSeconds(duration);
            m_currentHurtbox.enabled = false;
            yield return null;
        }

        protected override void Start()
        {
            base.Start();
            //m_spineListener.Subscribe(m_info.phaseEvent, PhaseFX);
            m_animation.DisableRootMotion();
            m_phaseHandle = new PhaseHandle<Phase, PhaseInfo>();
            m_phaseHandle.Initialize(Phase.PhaseOne, m_info.phaseInfo, m_character, ChangeState, ApplyPhaseData);
            m_phaseHandle.ApplyChange();

            m_spineListener.Subscribe(m_info.swordSlash1Event, SwordSlash1);
            m_spineListener.Subscribe(m_info.swordSlash2Event, SwordSlash2);
            m_spineListener.Subscribe(m_info.swordSlash3Event, SwordSlash3);
            m_spineListener.Subscribe(m_info.swordStabEvent, SwordStab);
            m_spineListener.Subscribe(m_info.heavySlashEvent, HeavySlash);
            m_spineListener.Subscribe(m_info.earthShakerEvent, EarthShaker);
            m_spineListener.Subscribe(m_info.specialThrustEvent, SpecialThrust);
            m_spineListener.Subscribe(m_info.rootMoveEndEvent, SwordThrustRootMoveEnd);
            m_spineListener.Subscribe(m_info.rootMoveStartEvent, SwordThrustRootMoveStart);
            m_spineListener.Subscribe(m_info.specialThrustEvent, SpecialThrust);
            m_spineListener.Subscribe(m_info.jumpEvent, JumpEvent);
        }

        private void Update()
        {
            //if (!m_hasPhaseChanged && m_stateHandle.currentState != State.Phasing)
            //{
            //}
            HealthChecker();
            m_phaseHandle.MonitorPhase();
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    m_animation.SetAnimation(0, m_info.idleAnimation, true);
                    break;
                case State.Intro:
                    if (IsFacingTarget())
                    {
                        StartCoroutine(IntroRoutine());
                        //m_stateHandle.OverrideState(State.Chasing);
                    }
                    else
                    {
                        m_turnState = State.Intro;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    break;
                case State.Phasing:
                    if (IsFacingTarget())
                    {
                        if (m_changePhaseCoroutine == null)
                        {
                            m_stateHandle.Wait(State.ReevaluateSituation);
                            m_changePhaseCoroutine = StartCoroutine(ChangePhaseRoutine());
                        }
                    }
                    else
                    {
                        //m_turnState = State.Phasing;
                        //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                        //    m_stateHandle.SetState(State.Turning);
                        CustomTurn();
                    }
                    break;
                case State.Turning:
                    if (m_currentAttackCoroutine != null)
                    {
                        StopCoroutine(m_currentAttackCoroutine);
                        m_currentAttackCoroutine = null;
                    }
                    m_phaseHandle.allowPhaseChange = false;
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute(m_info.turnAnimation.animation, m_currentIdleAnimation);
                    m_movement.Stop();
                    m_trailFX.Stop();
                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    switch (m_currentAttack)
                    {
                        case Attack.OneHitCombo:
                            m_currentAttackCoroutine = StartCoroutine(OneHitComboAttackRoutine());
                            //m_currentAttackCoroutine = StartCoroutine(EarthShakerAttackRoutine());
                            m_pickedCD = m_currentFullCD[0];
                            break;
                        case Attack.TwoHitCombo:
                            m_lastTargetPos = m_targetInfo.position;
                            m_currentAttackCoroutine = StartCoroutine(TwoHitComboAttackRoutine());
                            m_pickedCD = m_currentFullCD[1];
                            break;
                        case Attack.ThreeHitCombo:
                            m_lastTargetPos = m_targetInfo.position;
                            m_currentAttackCoroutine = StartCoroutine(ThreeHitComboAttackRoutine());
                            m_pickedCD = m_currentFullCD[1];
                            break;
                        case Attack.RunningSlash:
                            m_lastTargetPos = m_targetInfo.position;
                            m_currentAttackCoroutine = StartCoroutine(RunningSlasAttackhRoutine());
                            //m_currentAttackCoroutine = StartCoroutine(EarthShakerAttackRoutine());
                            m_pickedCD = m_currentFullCD[1];
                            break;
                        case Attack.EarthShaker:
                            if (m_earthShakerExplosionFX.transform.parent == null)
                            {
                                m_earthShakerExplosionFX.transform.SetParent(m_fxParent);
                                m_earthShakerExplosionFX.transform.localPosition = Vector3.zero;
                                m_earthShakerExplosionFX.transform.localScale = new Vector3(-1.333333f, 1.333333f, 1);
                            }
                            m_currentAttackCoroutine = StartCoroutine(EarthShakerAttackRoutine());
                            m_pickedCD = m_currentFullCD[2];
                            break;
                        case Attack.SpecialThrust:
                            m_currentAttackCoroutine = StartCoroutine(SpecialThrustAttackRoutine());
                            m_pickedCD = m_currentFullCD[3];
                            break;
                    }

                    break;

                case State.Cooldown:
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
                            m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        m_animation.SetAnimation(0, m_currentIdleAnimation, true).TimeScale = 1;
                    }

                    if (m_currentCD <= m_pickedCD)
                    {
                        m_currentCD += Time.deltaTime;
                    }
                    else
                    {
                        m_currentCD = 0;
                        //m_stateHandle.OverrideState(State.ReevaluateSituation);
                        ChooseAttack();
                        if (Vector2.Distance(m_targetInfo.position, transform.position) > m_currentAttackRange)
                        {
                            m_stateHandle.Wait(State.ReevaluateSituation);
                            StartCoroutine(RunAnticipationRoutine());
                        }
                        else
                        {
                            m_stateHandle.OverrideState(State.ReevaluateSituation);
                        }
                    }

                    break;

                case State.Chasing:
                    if (IsFacingTarget())
                    {
                        if (IsTargetInRange(m_currentAttackRange) && m_currentAttackCoroutine == null && !m_hitbox.canBlockDamage)
                        {
                            m_attackDecider.hasDecidedOnAttack = false;
                            m_trailFX.Stop();
                            m_stateHandle.SetState(State.Attacking);
                        }
                        else
                        {
                            MoveToTarget(m_currentAttackRange);
                        }
                    }
                    else
                    {
                        m_turnState = State.Attacking;
                        if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation.animation)
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