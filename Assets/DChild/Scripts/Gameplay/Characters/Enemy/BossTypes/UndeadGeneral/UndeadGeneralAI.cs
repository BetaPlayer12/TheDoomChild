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
using UnityEngine.Playables;

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
            private SimpleAttackInfo m_normalAttack = new SimpleAttackInfo();
            public SimpleAttackInfo normalAttack => m_normalAttack;
            [SerializeField]
            private SimpleAttackInfo m_heavySlashAttack = new SimpleAttackInfo();
            public SimpleAttackInfo heavySlashAttack => m_heavySlashAttack;
            [SerializeField, BoxGroup("Earth Shaker")]
            private SimpleAttackInfo m_earthShakerAttack = new SimpleAttackInfo();
            public SimpleAttackInfo earthShakerAttack => m_earthShakerAttack;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Earth Shaker")]
            private string m_earthShakerCancelAnimation;
            public string earthShakerCancelAnimation => m_earthShakerCancelAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Earth Shaker")]
            private string m_earthShakerStunnedLoopAnimation;
            public string earthShakerStunnedLoopAnimation => m_earthShakerStunnedLoopAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Earth Shaker")]
            private string m_earthShakerRecoverAnimation;
            public string earthShakerRecoverAnimation => m_earthShakerRecoverAnimation;
            [SerializeField, BoxGroup("Special Thrust")]
            private SimpleAttackInfo m_specialThrustAttack = new SimpleAttackInfo();
            public SimpleAttackInfo specialThrustAttack => m_specialThrustAttack;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Special Thrust")]
            private string m_specialThrustStartAnimation;
            public string specialThrustStartAnimation => m_specialThrustStartAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Special Thrust")]
            private string m_specialThrustHitAnimation;
            public string specialThrustHitAnimation => m_specialThrustHitAnimation;
            [SerializeField, BoxGroup("Dodge Attack")]
            private SimpleAttackInfo m_dodgeAttack = new SimpleAttackInfo();
            public SimpleAttackInfo dodgeAttack => m_dodgeAttack;
            [SerializeField, BoxGroup("Guard Attack")]
            private SimpleAttackInfo m_guardAttack = new SimpleAttackInfo();
            public SimpleAttackInfo guardAttack => m_guardAttack;
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
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_intro1Animation;
            public string intro1Animation => m_intro1Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_intro2Animation;
            public string intro2Animation => m_intro2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_intro3Animation;
            public string intro3Animation => m_intro3Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleCombatAnimation;
            public string idleCombatAnimation => m_idleCombatAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleGuardAnimation;
            public string idleGuardAnimation => m_idleGuardAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleToCombatTransitionAnimation;
            public string idleToCombatTransitionAnimation => m_idleToCombatTransitionAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchInplaceAnimation;
            public string flinchInplaceAnimation => m_flinchInplaceAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchKnockbackAnimation;
            public string flinchKnockbackAnimation => m_flinchKnockbackAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_defeatStartAnimation;
            public string defeatStartAnimation => m_defeatStartAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_defeatLoopAnimation;
            public string defeatLoopAnimation => m_defeatLoopAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_victory1Animation;
            public string victory1Animation => m_victory1Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_victory2Animation;
            public string victory2Animation => m_victory2Animation;

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
                m_normalAttack.SetData(m_skeletonDataAsset);
                m_heavySlashAttack.SetData(m_skeletonDataAsset);
                m_earthShakerAttack.SetData(m_skeletonDataAsset);
                m_earthShakerAttack.SetData(m_skeletonDataAsset);
                m_specialThrustAttack.SetData(m_skeletonDataAsset);
                m_dodgeAttack.SetData(m_skeletonDataAsset);
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
            Normal,
            HeavySlash,
            EarthShaker,
            SpecialThrust,
            Dodge,
            Guard,
            DodgeGuard,
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
        [SerializeField, TabGroup("Modules")]
        private TransformTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Cinematic")]
        private BlackDeathCinematicPlayah m_cinematic;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Effects")]
        private GameObject m_fx;
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

        private int m_currentPhaseIndex;
        private Coroutine m_currentAttackCoroutine;
        private Coroutine m_hurtboxCoroutine;

        private string m_currentMovementAnimation;
        private float m_currentMovementSpeed;
        private float m_currentCD;
        private float m_pickedCD;
        private List<float> m_currentFullCD;
        private bool m_willDodgeGuard;

        private void ApplyPhaseData(PhaseInfo obj)
        {
            m_attackCache.Clear();
            m_attackRangeCache.Clear();
            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseOne:
                    m_currentMovementAnimation = m_info.moveSlow.animation;
                    m_currentMovementSpeed = m_info.moveSlow.speed;
                    AddToAttackCache(Attack.Normal, Attack.HeavySlash, Attack.Dodge, Attack.Guard);
                    AddToRangeCache(m_info.normalAttack.range, m_info.heavySlashAttack.range, m_info.dodgeAttack.range, m_info.guardAttack.range);
                    break;
                case Phase.PhaseTwo:
                    m_currentMovementAnimation = m_info.moveSlow.animation;
                    m_currentMovementSpeed = m_info.moveSlow.speed;
                    AddToAttackCache(Attack.Normal, Attack.HeavySlash, Attack.DodgeGuard, Attack.EarthShaker);
                    AddToRangeCache(m_info.normalAttack.range, m_info.heavySlashAttack.range, m_info.dodgeAttack.range, m_info.earthShakerAttack.range);
                    break;
                case Phase.PhaseThree:
                    m_currentMovementAnimation = m_info.moveMedium.animation;
                    m_currentMovementSpeed = m_info.moveMedium.speed;
                    AddToAttackCache(Attack.Normal, Attack.HeavySlash, Attack.DodgeGuard, Attack.EarthShaker, Attack.Guard, Attack.SpecialThrust);
                    AddToRangeCache(m_info.normalAttack.range, m_info.heavySlashAttack.range, m_info.dodgeAttack.range, m_info.earthShakerAttack.range, m_info.guardAttack.range, m_info.specialThrustAttack.range);
                    break;
                case Phase.PhaseFour:
                    m_currentMovementAnimation = m_info.moveFast.animation;
                    m_currentMovementSpeed = m_info.moveFast.speed;
                    AddToAttackCache(Attack.Normal, Attack.HeavySlash, Attack.EarthShaker, Attack.SpecialThrust, Attack.Guard);
                    AddToRangeCache(m_info.normalAttack.range, m_info.heavySlashAttack.range, m_info.earthShakerAttack.range, m_info.specialThrustAttack.range, m_info.guardAttack.range);
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
                base.SetTarget(damageable, m_target);
                m_stateHandle.OverrideState(State.Intro);
                GameEventMessage.SendEvent("Boss Encounter");
            }
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            if (m_stateHandle.currentState != State.Phasing /*&& !m_hasPhaseChanged*/)
            {
                m_animation.animationState.TimeScale = 1f;
                m_stateHandle.ApplyQueuedState();
            }
            m_phaseHandle.allowPhaseChange = true;
        }

        private void CustomTurn()
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
        }

        private IEnumerator IntroRoutine()
        {
            m_stateHandle.Wait(State.Chasing);
            m_movement.Stop();
            m_hitbox.SetInvulnerability(Invulnerability.MAX);
            //m_cinematic.PlayCinematic(1, false);
            m_animation.animationState.TimeScale = 1;
            //int introCount = UnityEngine.Random.Range(1, 4);
            //var introAnim = m_info.intro1Animation;
            //switch (introCount)
            //{
            //    case 1:
            //        introAnim = m_info.intro1Animation;
            //        break;
            //    case 2:
            //        introAnim = m_info.intro2Animation;
            //        break;
            //    case 3:
            //        introAnim = m_info.intro3Animation;
            //        break;
            //}
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.intro1Animation, false);
            m_animation.AddAnimation(0, m_info.idleAnimation, true, 0);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.idleAnimation);
            m_animation.DisableRootMotion();
            m_hitbox.Enable();
            m_hitbox.SetInvulnerability(Invulnerability.None);
            m_stateHandle.ApplyQueuedState();
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
            m_animation.SetEmptyAnimation(0, 0);
            StartCoroutine(ChangePhaseRoutine());
        }

        private void StopCurrentAttackRoutine()
        {
            if (m_currentAttackCoroutine != null)
            {
                StopCoroutine(m_currentAttackCoroutine);
                m_currentAttackCoroutine = null;
                m_attackDecider.hasDecidedOnAttack = false;
            }
        }

        private IEnumerator ChangePhaseRoutine()
        {
            m_phaseHandle.allowPhaseChange = false;

            m_stateHandle.Wait(State.ReevaluateSituation);
            m_hitbox.Disable();
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.flinchKnockbackAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.flinchKnockbackAnimation);
            m_animation.SetAnimation(0, m_info.idleCombatAnimation, true);
            m_animation.DisableRootMotion();
            m_hitbox.Enable();
            m_stateHandle.ApplyQueuedState();
            yield return null;

            m_phaseHandle.allowPhaseChange = true;
        }
        #region Attacks

        private IEnumerator NormalAttackRoutine()
        {
            m_phaseHandle.allowPhaseChange = false;

            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.normalAttack.animation, false);
            //yield return new WaitForSeconds(0.5f);
            //m_character.physics.SetVelocity(m_info.shoulderBashVelocity.x * transform.localScale.x, 0);
            //yield return new WaitForSeconds(0.15f);
            //m_movement.Stop();
            m_animation.AddAnimation(0, m_info.idleGuardAnimation, false, 0).TimeScale = 5f;
            m_animation.animationState.GetCurrent(0).MixDuration = 0;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.idleGuardAnimation);
            //m_animation.SetAnimation(0, m_info.idleCombatAnimation, true);
            //yield return new WaitForSeconds(3f);
            m_attackDecider.hasDecidedOnAttack = false;
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;

            m_phaseHandle.allowPhaseChange = true;
        }

        private IEnumerator HeavySlashRoutine(Vector2 target)
        {
            m_phaseHandle.allowPhaseChange = false;

            while (Vector2.Distance(target, transform.position) >= 15f)
            {
                m_animation.EnableRootMotion(true, false);
                m_animation.SetAnimation(0, m_info.moveMedium.animation, true);
                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.moveMedium.speed);
                yield return null;
            }
            m_movement.Stop();
            m_animation.SetAnimation(0, m_info.heavySlashAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.heavySlashAttack.animation);
            m_animation.SetAnimation(0, m_info.idleCombatAnimation, true);
            //yield return new WaitForSeconds(3f);
            m_attackDecider.hasDecidedOnAttack = false;
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;

            m_phaseHandle.allowPhaseChange = true;
        }

        private IEnumerator DodgeAttackRoutine()
        {
            m_phaseHandle.allowPhaseChange = false;

            if (!IsFacingTarget()) CustomTurn();
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.dodgeHop.animation, false);
            m_animation.AddAnimation(0, m_info.normalAttack.animation, false, 0);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.normalAttack.animation);
            m_animation.AddAnimation(0, m_info.idleGuardAnimation, false, 0).TimeScale = 5f;
            m_animation.animationState.GetCurrent(0).MixDuration = 0;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.idleGuardAnimation);
            //yield return new WaitForSeconds(3f);
            m_attackDecider.hasDecidedOnAttack = false;
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;

            m_phaseHandle.allowPhaseChange = true;
        }

        private IEnumerator GuardAttackRoutine(bool hasBlocked, bool willDodge)
        {
            m_phaseHandle.allowPhaseChange = false;

            if (!hasBlocked)
            {
                m_hitbox.SetCanBlockDamageState(true);
                m_animation.SetAnimation(0, willDodge ? m_info.idleCombatAnimation : m_info.idleGuardAnimation, true);
                yield return new WaitForSeconds(3f);
            }
            m_hitbox.SetCanBlockDamageState(false);
            if (!willDodge)
            {
                if (!IsFacingTarget()) CustomTurn();
                m_animation.SetAnimation(0, m_info.guardTriggerAnimation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.guardTriggerAnimation);
                m_animation.SetAnimation(0, m_info.guardAttack.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.guardAttack.animation);
                m_attackDecider.hasDecidedOnAttack = false;
                m_currentAttackCoroutine = null;
                m_stateHandle.ApplyQueuedState();
            }
            else
            {
                m_currentAttackCoroutine = StartCoroutine(DodgeAttackRoutine());
            }
            //m_currentAttackCoroutine = StartCoroutine(SpecialThrustAttackRoutine());
            yield return null;

            m_phaseHandle.allowPhaseChange = true;
        }

        private void DamageBlocked(object sender, Damageable.DamageEventArgs eventArgs)
        {
            if (m_animation.GetCurrentAnimation(0).ToString() == m_info.idleGuardAnimation)
            {
                StopCoroutine(m_currentAttackCoroutine);
                m_currentAttackCoroutine = StartCoroutine(GuardAttackRoutine(true, m_willDodgeGuard));
            }
        }

        private IEnumerator EarthShakerAttackRoutine()
        {
            m_phaseHandle.allowPhaseChange = false;

            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.earthShakerAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.earthShakerAttack.animation);
            m_animation.SetAnimation(0, m_info.idleCombatAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;

            m_phaseHandle.allowPhaseChange = true;
        }
        
        private IEnumerator SpecialThrustAttackRoutine()
        {
            m_phaseHandle.allowPhaseChange = false;

            m_animation.EnableRootMotion(true, false);
            //m_animation.SetAnimation(0, m_info.specialThrustStartAnimation, false);
            m_animation.SetAnimation(0, m_info.specialThrustAttack.animation, false);
            m_animation.AddAnimation(0, m_info.specialThrustHitAnimation, false, 0);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.specialThrustHitAnimation);
            m_animation.SetAnimation(0, m_info.idleCombatAnimation, true);
            m_attackDecider.hasDecidedOnAttack = false;
            m_animation.DisableRootMotion();
            m_currentAttackCoroutine = null;
            m_stateHandle.ApplyQueuedState();
            yield return null;

            m_phaseHandle.allowPhaseChange = true;
        }

        #endregion

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            StopAllCoroutines();
            //m_deathFX.Play();
            m_movement.Stop();
        }

        #region Movement
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
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Normal, m_info.normalAttack.range)
                                    , new AttackInfo<Attack>(Attack.HeavySlash, m_info.heavySlashAttack.range)
                                    , new AttackInfo<Attack>(Attack.EarthShaker, m_info.earthShakerAttack.range)
                                    , new AttackInfo<Attack>(Attack.SpecialThrust, m_info.specialThrustAttack.range)
                                    , new AttackInfo<Attack>(Attack.Dodge, m_info.dodgeAttack.range)
                                    , new AttackInfo<Attack>(Attack.Guard, m_info.guardAttack.range)
                                    , new AttackInfo<Attack>(Attack.DodgeGuard, m_info.dodgeAttack.range));
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
            m_deathHandle.SetAnimation(m_info.defeatStartAnimation);
            //m_patternDecider = new RandomAttackDecider<Pattern>();
            m_attackDecider = new RandomAttackDecider<Attack>();
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            UpdateAttackDeciderList();
            m_attackCache = new List<Attack>();
            AddToAttackCache(Attack.Normal, Attack.HeavySlash, Attack.EarthShaker, Attack.SpecialThrust, Attack.Dodge, Attack.Guard);
            m_attackRangeCache = new List<float>();
            AddToRangeCache(m_info.normalAttack.range, m_info.heavySlashAttack.range, m_info.earthShakerAttack.range, m_info.specialThrustAttack.range, m_info.dodgeAttack.range, m_info.guardAttack.range);
            m_attackUsed = new bool[m_attackCache.Count];
            m_damageable.DamageTaken += DamageBlocked;
            m_currentFullCD = new List<float>();
        }

        private void SwordSlash1()
        {
            m_hurtboxCoroutine = StartCoroutine(BoundingBoxRoutine(m_swordSlash1BB, 0.25f));
        }

        private void SwordSlash2()
        {
            m_hurtboxCoroutine = StartCoroutine(BoundingBoxRoutine(m_swordSlash2BB, 0.25f));
        }

        private void SwordSlash3()
        {
            m_hurtboxCoroutine = StartCoroutine(BoundingBoxRoutine(m_swordSlash3BB, 0.25f));
        }

        private void SwordStab()
        {
            m_hurtboxCoroutine = StartCoroutine(BoundingBoxRoutine(m_swordStabBB, 0.25f));
        }

        private void HeavySlash()
        {
            m_hurtboxCoroutine = StartCoroutine(BoundingBoxRoutine(m_heavySlashBB, 0.25f));
        }

        private void EarthShaker()
        {
            m_hurtboxCoroutine = StartCoroutine(BoundingBoxRoutine(m_earthShakerBB, 0.25f));
        }

        private void SpecialThrust()
        {
            m_hurtboxCoroutine = StartCoroutine(BoundingBoxRoutine(m_specialThrustBB, 0.5f));
        }

        private IEnumerator BoundingBoxRoutine(Collider2D hurtbox, float duration)
        {
            hurtbox.enabled = true;
            yield return new WaitForSeconds(duration);
            hurtbox.enabled = false;
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
        }

        private void Update()
        {
            //if (!m_hasPhaseChanged && m_stateHandle.currentState != State.Phasing)
            //{
            //}
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
                        //m_stateHandle.OverrideState(State.Chasing);
                    }
                    else
                    {
                        CustomTurn();
                        //m_turnState = State.Intro;
                        //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                        //    m_stateHandle.SetState(State.Turning);
                    }
                    break;
                case State.Phasing:
                    StartCoroutine(ChangePhaseRoutine());
                    break;
                case State.Turning:
                    m_phaseHandle.allowPhaseChange = false;
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute();
                    m_movement.Stop();
                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    switch (m_currentAttack)
                    {
                        case Attack.Normal:
                            m_currentAttackCoroutine = StartCoroutine(NormalAttackRoutine());
                            m_pickedCD = m_currentFullCD[0];
                            break;
                        case Attack.HeavySlash:
                            m_currentAttackCoroutine = StartCoroutine(HeavySlashRoutine(m_targetInfo.position));
                            m_pickedCD = m_currentFullCD[1];
                            break;
                        case Attack.EarthShaker:
                            m_currentAttackCoroutine = StartCoroutine(EarthShakerAttackRoutine());
                            m_pickedCD = m_currentFullCD[2];
                            break;
                        case Attack.SpecialThrust:
                            m_currentAttackCoroutine = StartCoroutine(SpecialThrustAttackRoutine());
                            m_pickedCD = m_currentFullCD[3];
                            break;
                        case Attack.Dodge:
                            m_currentAttackCoroutine = StartCoroutine(DodgeAttackRoutine());
                            m_pickedCD = m_currentFullCD[4];
                            break;
                        case Attack.Guard:
                            m_willDodgeGuard = false;
                            m_currentAttackCoroutine = StartCoroutine(GuardAttackRoutine(false, m_willDodgeGuard));
                            m_pickedCD = m_currentFullCD[5];
                            break;
                        case Attack.DodgeGuard:
                            m_willDodgeGuard = true;
                            m_currentAttackCoroutine = StartCoroutine(GuardAttackRoutine(false, m_willDodgeGuard));
                            m_pickedCD = m_currentFullCD[6];
                            break;
                    }

                    break;

                case State.Cooldown:
                    //m_stateHandle.Wait(State.ReevaluateSituation);
                    //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
                    if (!IsFacingTarget())
                    {
                        m_turnState = State.Cooldown;
                        m_stateHandle.SetState(State.Turning);
                    }
                    else
                    {
                        m_animation.SetAnimation(0, m_info.idleCombatAnimation, true);
                    }

                    if (m_currentCD <= m_pickedCD)
                    {
                        m_currentCD += Time.deltaTime;
                    }
                    else
                    {
                        m_currentCD = 0;
                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                    }

                    break;

                case State.Chasing:
                    if (IsFacingTarget())
                    {
                        ChooseAttack();
                        if (IsTargetInRange(m_currentAttackRange) && m_currentAttackCoroutine == null)
                        {
                            m_attackDecider.hasDecidedOnAttack = false;
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
                        //if (m_animation.GetCurrentAnimation(0).ToString() != m_info.turnAnimation)
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