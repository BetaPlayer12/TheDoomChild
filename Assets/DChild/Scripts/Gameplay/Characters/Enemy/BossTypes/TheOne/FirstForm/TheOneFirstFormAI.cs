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
    [AddComponentMenu("DChild/Gameplay/Enemies/Boss/TheOneFirstForm")]
    public class TheOneFirstFormAI : CombatAIBrain<TheOneFirstFormAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField]
            private PhaseInfo<Phase> m_phaseInfo;
            public PhaseInfo<Phase> phaseInfo => m_phaseInfo;

            [SerializeField]
            private MovementInfo m_walk = new MovementInfo();
            public MovementInfo walk => m_walk;

            [Title("Attack Behaviours")]
            [SerializeField, BoxGroup("Downward Slash 1")]
            private SimpleAttackInfo m_downwardSlash1Attack = new SimpleAttackInfo();
            public SimpleAttackInfo downwardSlash1Attack => m_downwardSlash1Attack;
            [SerializeField, BoxGroup("Downward Slash 2")]
            private SimpleAttackInfo m_downwardSlash2Attack = new SimpleAttackInfo();
            public SimpleAttackInfo downwardSlash2Attack => m_downwardSlash2Attack;
            [SerializeField, BoxGroup("Sword Stab")]
            private SimpleAttackInfo m_swordStabAttack = new SimpleAttackInfo();
            public SimpleAttackInfo swordStabAttack => m_swordStabAttack;
            [SerializeField, BoxGroup("Heavy Sword Stab")]
            private SimpleAttackInfo m_heavySwordStabAttack = new SimpleAttackInfo();
            public SimpleAttackInfo heavySwordStabAttack => m_heavySwordStabAttack;
            [SerializeField, BoxGroup("Twin Slash 1")]
            private SimpleAttackInfo m_twinSlash1Attack = new SimpleAttackInfo();
            public SimpleAttackInfo twinSlash1Attack => m_twinSlash1Attack;
            [SerializeField, BoxGroup("Twin Slash 2")]
            private SimpleAttackInfo m_twinSlash2Attack = new SimpleAttackInfo();
            public SimpleAttackInfo twinSlash2Attack => m_twinSlash2Attack;
            [SerializeField, BoxGroup("Drill Dash 1")]
            private SimpleAttackInfo m_drillDash1Attack = new SimpleAttackInfo();
            public SimpleAttackInfo drillDash1Attack => m_drillDash1Attack;
            [SerializeField, BoxGroup("Drill Dash 2")]
            private SimpleAttackInfo m_drillDash2Attack = new SimpleAttackInfo();
            public SimpleAttackInfo drillDash2Attack => m_drillDash2Attack;
            [SerializeField, BoxGroup("ProjectileWaveSlash")]
            private SimpleAttackInfo m_projectilWaveSlashGround1Attack = new SimpleAttackInfo();
            public SimpleAttackInfo projectilWaveSlashGround1Attack => m_projectilWaveSlashGround1Attack;
            [SerializeField, BoxGroup("ProjectileWaveSlash")]
            private SimpleAttackInfo m_projectilWaveSlashGround2Attack = new SimpleAttackInfo();
            public SimpleAttackInfo projectilWaveSlashGround2Attack => m_projectilWaveSlashGround2Attack;
            [SerializeField, BoxGroup("ProjectileWaveSlash")]
            private SimpleAttackInfo m_projectilWaveSlashMidAir1Attack = new SimpleAttackInfo();
            public SimpleAttackInfo projectilWaveSlashMidAir1Attack => m_projectilWaveSlashMidAir1Attack;
            [SerializeField, BoxGroup("ProjectileWaveSlash")]
            private SimpleAttackInfo m_projectilWaveSlashMidAir2Attack = new SimpleAttackInfo();
            public SimpleAttackInfo projectilWaveSlashMidAir2Attack => m_projectilWaveSlashMidAir2Attack;
            [SerializeField, BoxGroup("ProjectileWaveSlash")]
            private SimpleAttackInfo m_scytheWaveAttack = new SimpleAttackInfo();
            public SimpleAttackInfo scytheWaveAttack => m_scytheWaveAttack;
            [SerializeField, BoxGroup("ProjectileWaveSlash")]
            private SimpleAttackInfo m_scytheDoubleWaveAttack = new SimpleAttackInfo();
            public SimpleAttackInfo scytheDoubleWaveAttack => m_scytheDoubleWaveAttack;
            [SerializeField, BoxGroup("GeyserBurst")]
            private SimpleAttackInfo m_geyserBurstGreenAttack = new SimpleAttackInfo();
            public SimpleAttackInfo geyserBurstGreenAttack => m_geyserBurstGreenAttack;
            [SerializeField, BoxGroup("GeyserBurst")]
            private SimpleAttackInfo m_geyserBurstPurpleAttack = new SimpleAttackInfo();
            public SimpleAttackInfo geyserBurstPurpleAttack => m_geyserBurstPurpleAttack;
            [SerializeField, BoxGroup("GeyserBurst")]
            private SimpleAttackInfo m_geyserBurstRedAttack = new SimpleAttackInfo();
            public SimpleAttackInfo geyserBurstRedAttack => m_geyserBurstRedAttack;

            [Title("Attack Cooldown States")]
            [SerializeField, MinValue(0), BoxGroup("Phase 1")]
            private float m_phase1Pattern1CD;
            public float phase1Pattern1CD => m_phase1Pattern1CD;
            [SerializeField, MinValue(0), BoxGroup("Phase 1")]
            private float m_phase1Pattern2CD;
            public float phase1Pattern2CD => m_phase1Pattern2CD;
            [SerializeField, MinValue(0), BoxGroup("Phase 1")]
            private float m_phase1Pattern3CD;
            public float phase1Pattern3CD => m_phase1Pattern3CD;
            [SerializeField, MinValue(0), BoxGroup("Phase 1")]
            private float m_phase1Pattern4CD;
            public float phase1Pattern4CD => m_phase1Pattern4CD;

            [Title("Attack Colors")]
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_drillNormalMixAnimation;
            public string drillNormalMixAnimation => m_drillNormalMixAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_drillGreenMixAnimation;
            public string drillGreenMixAnimation => m_drillGreenMixAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_drillPurpleMixAnimation;
            public string drillPurpleMixAnimation => m_drillPurpleMixAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_drillRedMixAnimation;
            public string drillRedMixAnimation => m_drillRedMixAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_swordNormalMixAnimation;
            public string swordNormalMixAnimation => m_swordNormalMixAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_swordGreenMixAnimation;
            public string swordGreenMixAnimation => m_swordGreenMixAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_swordPurpleMixAnimation;
            public string swordPurpleMixAnimation => m_swordPurpleMixAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_swordRedMixAnimation;
            public string swordRedMixAnimation => m_swordRedMixAnimation;

            [Title("Ability Behaviours")]
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Blink")]
            private string m_blinkAppearBackwardAnimation;
            public string blinkAppearBackwardAnimation => m_blinkAppearBackwardAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Blink")]
            private string m_blinkAppearForwardAnimation;
            public string blinkAppearForwardAnimation => m_blinkAppearForwardAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Blink")]
            private string m_blinkAppearUpwardAnimation;
            public string blinkAppearUpwardAnimation => m_blinkAppearUpwardAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Blink")]
            private string m_blinkDisappearBackwardAnimation;
            public string blinkDisappearBackwardAnimation => m_blinkDisappearBackwardAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Blink")]
            private string m_blinkDisappearForwardAnimation;
            public string blinkDisappearForwardAnimation => m_blinkDisappearForwardAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Blink")]
            private string m_blinkDisappearUpwardAnimation;
            public string blinkDisappearUpwardAnimation => m_blinkDisappearUpwardAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Blink")]
            private string m_blinkFakeAnimation;
            public string blinkFakeAnimation => m_blinkFakeAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Sword Change")]
            private string m_swordChangeAnimation;
            public string swordChangeAnimation => m_swordChangeAnimation;
            [SerializeField, ValueDropdown("GetAnimations"), BoxGroup("Summon Swords")]
            private string m_summonSwordsAnimation;
            public string summonSwordsAnimation => m_summonSwordsAnimation;


            [Title("Pattern Ranges")]
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

            [Title("Misc")]
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

            [Title("Animations")]
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleCombatAnimation;
            public string idleCombatAnimation => m_idleCombatAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_drillToGroundAnimation;
            public string drillToGroundAnimation => m_drillToGroundAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_groundtoDrillAnimation;
            public string groundtoDrillAnimation => m_groundtoDrillAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_fallAniamtion;
            public string fallAniamtion => m_fallAniamtion;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_landAnimation;
            public string landAnimation => m_landAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_staggerAnimation;
            public string staggerAnimation => m_staggerAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_staggerWithKnockbackAnimation;
            public string staggerWithKnockbackAnimation => m_staggerWithKnockbackAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_defStaggerWithKnockbackAnimation;
            public string defStaggerWithKnockbackAnimation => m_defStaggerWithKnockbackAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_defeated1Animation;
            public string defeated1Animation => m_defeated1Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_defeated2Animation;
            public string defeated2Animation => m_defeated2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_defeated3Animation;
            public string defeated3Animation => m_defeated3Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_defeated4Animation;
            public string defeated4Animation => m_defeated4Animation;

            [Title("FX")]
            [SerializeField]
            private GameObject m_fx;
            public GameObject fx => m_fx;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_dustLandEvent;
            public string dustLandEvent => m_dustLandEvent;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_walk.SetData(m_skeletonDataAsset);
                m_downwardSlash1Attack.SetData(m_skeletonDataAsset);
                m_downwardSlash2Attack.SetData(m_skeletonDataAsset);
                m_swordStabAttack.SetData(m_skeletonDataAsset);
                m_heavySwordStabAttack.SetData(m_skeletonDataAsset);
                m_twinSlash1Attack.SetData(m_skeletonDataAsset);
                m_twinSlash2Attack.SetData(m_skeletonDataAsset);
                m_drillDash1Attack.SetData(m_skeletonDataAsset);
                m_drillDash2Attack.SetData(m_skeletonDataAsset);
                m_projectilWaveSlashGround1Attack.SetData(m_skeletonDataAsset);
                m_projectilWaveSlashGround2Attack.SetData(m_skeletonDataAsset);
                m_projectilWaveSlashMidAir1Attack.SetData(m_skeletonDataAsset);
                m_projectilWaveSlashMidAir2Attack.SetData(m_skeletonDataAsset);
                m_scytheWaveAttack.SetData(m_skeletonDataAsset);
                m_scytheDoubleWaveAttack.SetData(m_skeletonDataAsset);
                m_geyserBurstGreenAttack.SetData(m_skeletonDataAsset);
                m_geyserBurstPurpleAttack.SetData(m_skeletonDataAsset);
                m_geyserBurstRedAttack.SetData(m_skeletonDataAsset);
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

        private enum BlinkState
        {
            AppearForward,
            AppearBackward,
            AppearUpward,
            DisappearForward,
            DisappearBackward,
            DisappearUpward,
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
            Phase1Pattern1,
            Phase1Pattern2,
            Phase1Pattern3,
            Phase1Pattern4,
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
        //[SerializeField, TabGroup("Modules")]
        //private TransformTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private TransformTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Hurtbox")]
        private Collider2D m_swordSlash1BB;

        [SerializeField, TabGroup("FX")]
        private ParticleFX m_earthShakerExplosionFX;

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
        private Coroutine m_counterAttackCoroutine;
        private Coroutine m_changePhaseCoroutine;

        private Vector2 m_lastTargetPos;
        private float m_currentCD;
        private float m_pickedCD;
        private int m_hitCounter;
        private bool m_canBlockCounter;
        private Collider2D m_currentHurtbox;
        private List<float> m_currentFullCD;

        #region Animation
        private string m_idleAnimation;
        private string m_blinkAppearAnimation;
        private string m_blinkDisappearAnimation;
        #endregion  

        private void ApplyPhaseData(PhaseInfo obj)
        {
            m_attackCache.Clear();
            m_attackRangeCache.Clear();
            switch (m_phaseHandle.currentPhase)
            {
                case Phase.PhaseOne:
                    m_idleAnimation = m_info.idleCombatAnimation;
                    AddToAttackCache(Attack.Phase1Pattern1, Attack.Phase1Pattern2, Attack.Phase1Pattern3, Attack.Phase1Pattern4);
                    AddToRangeCache(m_info.phase1Pattern1Range, m_info.phase1Pattern2Range, m_info.phase1Pattern3Range, m_info.phase1Pattern4Range);
                    break;
                case Phase.PhaseTwo:
                    //m_currentIdleAnimation = m_info.idleCombatAnimation;
                    //m_currentIdleTransitionAnimation = m_info.idleToCombatTransitionAnimation;
                    //m_currentMovementAnimation = m_info.moveMedium.animation;
                    //m_currentMovementSpeed = m_info.moveMedium.speed;
                    //m_currentThirdSlashDashSpeed = 50;
                    //AddToAttackCache(Attack.ThreeHitCombo, Attack.RunningSlash, Attack.EarthShaker);
                    //AddToRangeCache(m_info.threeHitComboAttack.range, m_info.runningSlashAttack.range, m_info.earthShakerAttack.range);
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
                Debug.Log("UG Ecnountered Player");
                base.SetTarget(damageable, m_target);
                m_stateHandle.OverrideState(State.Intro);
                GameEventMessage.SendEvent("Boss Encounter");
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
            if (m_counterAttackCoroutine == null)
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
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    m_counterAttackCoroutine = UnityEngine.Random.Range(0, 2) == 0 ? StartCoroutine(ProjectileWaveSlashAttackCounterRoutine()) : StartCoroutine(DrillDashAttackCounterRoutine());
                    //m_counterAttackCoroutine = StartCoroutine(GuardAttackRoutine(false, false));
                }
            }
        }

        //private void OnDamageBlocked(object sender, Damageable.DamageEventArgs eventArgs)
        //{
        //    if (m_canBlockCounter)
        //    {
        //        if (m_currentAttackCoroutine != null)
        //        {
        //            StopCoroutine(m_currentAttackCoroutine);
        //            m_currentAttackCoroutine = null;
        //        }

        //        StopCoroutine(m_counterAttackCoroutine);
        //        m_counterAttackCoroutine = StartCoroutine(GuardAttackRoutine(true, Vector2.Distance(m_targetInfo.position, transform.position) > m_info.oneHitComboAttack.range ? false : true));
        //    }
        //}

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
            m_animation.EnableRootMotion(true, false);
            m_hitbox.Enable();
            m_hitbox.SetInvulnerability(Invulnerability.None);
            //m_stateHandle.ApplyQueuedState();
            ChooseAttack();
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
            //m_hitbox.Disable();
            //m_trailFX.Stop();
            //m_animation.EnableRootMotion(true, false);
            //m_animation.SetAnimation(0, m_info.phasingEnragedAnimation, false);
            //m_animation.animationState.GetCurrent(0).MixDuration = 0;
            //yield return new WaitWhile(() => m_animation.animationState.GetCurrent(0).AnimationTime < 0.5f);
            //m_enragedFX.Play();
            //yield return new WaitWhile(() => m_animation.animationState.GetCurrent(0).AnimationTime < m_animation.animationState.GetCurrent(0).AnimationEnd * 0.8f);
            //m_enragedFX.Stop();
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.phasingEnragedAnimation);
            //m_hitbox.Enable();
            //m_changePhaseCoroutine = null;
            //switch (m_phaseHandle.currentPhase)
            //{
            //    case Phase.PhaseTwo:
            //        m_currentAttack = Attack.EarthShaker;
            //        break;
            //    case Phase.PhaseThree:
            //        m_currentAttack = Attack.SpecialThrust;
            //        break;
            //}
            m_stateHandle.OverrideState(State.Attacking);
            yield return null;
        }
        #region Attacks

        private IEnumerator DownwardSlash1AttackRoutine()
        {
            yield return null;
        }

        private IEnumerator DownwardSlash2AttackRoutine()
        {
            yield return null;
        }

        private IEnumerator SwordStabAttackRoutine()
        {
            yield return null;
        }

        private IEnumerator HeavySwordStabAttackRoutine()
        {
            yield return null;
        }

        private IEnumerator TwinSlash1AttackRoutine()
        {
            yield return null;
        }

        private IEnumerator TwinSlash2AttackRoutine()
        {
            yield return null;
        }

        private IEnumerator DrillDash1AttackRoutine()
        {
            yield return null;
        }

        private IEnumerator DrillDash2AttackRoutine()
        {
            yield return null;
        }

        private IEnumerator ProjectileWaveSlashAttackRoutine()
        {
            yield return null;
        }

        private IEnumerator ScytheWaveAttackRoutine()
        {
            yield return null;
        }

        private IEnumerator GeyserBurstAttackRoutine()
        {
            yield return null;
        }

        private IEnumerator Phase1Pattern1AttackRoutine()
        {
            if (IsTargetInRange(m_info.phase1Pattern1Range))
            {
                m_animation.SetAnimation(0, m_info.downwardSlash1Attack.animation, false);
                yield return new WaitForAnimationComplete(m_animation.animationState, m_info.downwardSlash1Attack.animation);

                if (!IsFacingTarget())
                    CustomTurn();

                for (int i = 0; i < 3; i++)
                {
                    if (IsTargetInRange(m_info.phase1Pattern1Range) || i == 3)
                    {
                        m_stateHandle.ApplyQueuedState();
                        yield return null;
                    }

                    switch (i)
                    {
                        case 1:
                            m_animation.SetAnimation(0, m_info.swordStabAttack.animation, false);
                            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.swordStabAttack.animation);
                            m_animation.SetAnimation(0, m_info.heavySwordStabAttack.animation, false);
                            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.heavySwordStabAttack.animation);
                            break;
                        case 2:
                            m_animation.SetAnimation(0, m_info.downwardSlash2Attack.animation, false);
                            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.downwardSlash2Attack.animation);
                            m_animation.SetAnimation(0, m_info.twinSlash1Attack.animation, false);
                            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.twinSlash1Attack.animation);
                            break;
                    }
                }
            }
            else
            {
                StartCoroutine(BlinkRoutine(BlinkState.DisappearForward, BlinkState.AppearForward));
                yield return null;
            }
            yield return null;
        }

        private IEnumerator Phase1Pattern2AttackRoutine()
        {
            yield return null;
        }

        private IEnumerator Phase1Pattern3AttackRoutine()
        {
            yield return null;
        }

        private IEnumerator Phase1Pattern4AttackRoutine()
        {
            yield return null;
        }
        #endregion

        #region Attack Counters
        private IEnumerator ProjectileWaveSlashAttackCounterRoutine()
        {
            yield return null;
        }

        private IEnumerator DrillDashAttackCounterRoutine()
        {
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
            StartCoroutine(DefeatRoutine());
        }

        private IEnumerator DefeatRoutine()
        {
            yield return null;
        }

        #region Movement
        private void MoveToTarget(float targetRange)
        {
            if (!IsTargetInRange(targetRange) && m_groundSensor.isDetecting /*&& !m_wallSensor.isDetecting && m_edgeSensor.isDetecting*/)
            {
                m_animation.EnableRootMotion(true, false);
                m_animation.SetAnimation(0, m_info.walk.animation, true);
                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.walk.speed);
            }
            else
            {
                m_movement.Stop();
                m_animation.SetAnimation(0, m_info.idleAnimation, true);
            }
        }

        private IEnumerator BlinkRoutine(BlinkState m_disappearState, BlinkState m_appearState)
        {
            switch (m_disappearState)
            {
                case BlinkState.DisappearForward:
                    m_blinkDisappearAnimation = m_info.blinkDisappearForwardAnimation;
                    break;
                case BlinkState.DisappearBackward:
                    m_blinkDisappearAnimation = m_info.blinkDisappearBackwardAnimation;
                    break;
                case BlinkState.DisappearUpward:
                    m_blinkDisappearAnimation = m_info.blinkDisappearBackwardAnimation;
                    break;
            }

            m_animation.SetAnimation(0, m_blinkDisappearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_blinkDisappearAnimation);

            switch (m_appearState)
            {
                case BlinkState.AppearForward:
                    m_blinkAppearAnimation = m_info.blinkAppearForwardAnimation;
                    break;
                case BlinkState.AppearBackward:
                    m_blinkAppearAnimation = m_info.blinkAppearBackwardAnimation;
                    break;
                case BlinkState.AppearUpward:
                    m_blinkAppearAnimation = m_info.blinkAppearUpwardAnimation;
                    break;
            }

            m_animation.SetAnimation(0, m_blinkAppearAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_blinkAppearAnimation);

            yield return null;
        }
        #endregion

        private void UpdateAttackDeciderList()
        {
            //m_patternDecider.SetList(new AttackInfo<Pattern>(Pattern.AttackPattern1, m_info.targetDistanceTolerance),
            //                        new AttackInfo<Pattern>(Pattern.AttackPattern2, m_info.targetDistanceTolerance),
            //                        new AttackInfo<Pattern>(Pattern.AttackPattern3, m_info.targetDistanceTolerance));
            m_attackDecider.SetList(new AttackInfo<Attack>(Attack.Phase1Pattern1, m_info.phase1Pattern1Range)
                                    , new AttackInfo<Attack>(Attack.Phase1Pattern2, m_info.phase1Pattern2Range)
                                    , new AttackInfo<Attack>(Attack.Phase1Pattern3, m_info.phase1Pattern3Range)
                                    , new AttackInfo<Attack>(Attack.Phase1Pattern4, m_info.phase1Pattern4Range));
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
            //m_damageable.DamageTaken += OnDamageBlocked;
            //m_patternDecider = new RandomAttackDecider<Pattern>();
            m_attackDecider = new RandomAttackDecider<Attack>();
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            UpdateAttackDeciderList();
            m_attackCache = new List<Attack>();
            AddToAttackCache(Attack.Phase1Pattern1, Attack.Phase1Pattern2, Attack.Phase1Pattern3, Attack.Phase1Pattern4);
            m_attackRangeCache = new List<float>();
            AddToRangeCache(m_info.phase1Pattern1Range, m_info.phase1Pattern2Range, m_info.phase1Pattern3Range, m_info.phase1Pattern4Range);
            m_attackUsed = new bool[m_attackCache.Count];
            m_currentFullCD = new List<float>();
        }

        protected override void Start()
        {
            base.Start();
            //m_spineListener.Subscribe(m_info.phaseEvent, PhaseFX);
            m_animation.DisableRootMotion();
            m_phaseHandle = new PhaseHandle<Phase, PhaseInfo>();
            m_phaseHandle.Initialize(Phase.PhaseOne, m_info.phaseInfo, m_character, ChangeState, ApplyPhaseData);
            m_phaseHandle.ApplyChange();

            //m_spineListener.Subscribe(m_info.swordSlash1Event, SwordSlash1);
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
                        m_stateHandle.SetState(State.Turning);
                    }
                    break;
                case State.Phasing:
                    if (IsFacingTarget())
                    {
                        if (m_changePhaseCoroutine == null)
                        {
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
                    m_phaseHandle.allowPhaseChange = false;
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute();
                    m_movement.Stop();
                    break;
                case State.Attacking:
                    m_stateHandle.Wait(State.Cooldown);
                    switch (m_currentAttack)
                    {
                        case Attack.Phase1Pattern1:
                            m_currentAttackCoroutine = StartCoroutine(Phase1Pattern1AttackRoutine());
                            //m_currentAttackCoroutine = StartCoroutine(EarthShakerAttackRoutine());
                            m_pickedCD = m_currentFullCD[0];
                            break;
                        case Attack.Phase1Pattern2:
                            m_lastTargetPos = m_targetInfo.position;
                            m_currentAttackCoroutine = StartCoroutine(Phase1Pattern2AttackRoutine());
                            m_pickedCD = m_currentFullCD[1];
                            break;
                        case Attack.Phase1Pattern3:
                            m_lastTargetPos = m_targetInfo.position;
                            m_currentAttackCoroutine = StartCoroutine(Phase1Pattern3AttackRoutine());
                            m_pickedCD = m_currentFullCD[1];
                            break;
                        case Attack.Phase1Pattern4:
                            m_lastTargetPos = m_targetInfo.position;
                            m_currentAttackCoroutine = StartCoroutine(Phase1Pattern4AttackRoutine());
                            //m_currentAttackCoroutine = StartCoroutine(EarthShakerAttackRoutine());
                            m_pickedCD = m_currentFullCD[1];
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
                        m_animation.SetAnimation(0, m_idleAnimation, true).TimeScale = 1;
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
                        m_stateHandle.OverrideState(State.ReevaluateSituation);
                    }

                    break;

                case State.Chasing:
                    if (IsFacingTarget())
                    {
                        if (IsTargetInRange(m_currentAttackRange) && m_currentAttackCoroutine == null && !m_hitbox.canBlockDamage)
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