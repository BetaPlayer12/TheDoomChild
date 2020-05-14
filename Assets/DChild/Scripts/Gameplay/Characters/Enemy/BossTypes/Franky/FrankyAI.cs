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
using Doozy.Engine;
using Spine.Unity.Modules;
using Spine.Unity.Examples;
using DChild.Gameplay.Pooling;

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Boss/FrankyAI")]
    public class FrankyAI : CombatAIBrain<FrankyAI.Info>
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

            [Title("Attack Behaviours")]
            #region ShoulderBash
            [SerializeField, Range(0, 100)]
            private float m_shoulderBashReelSpeed;
            public float shoulderBashReelSpeed => m_shoulderBashReelSpeed;
            [Title("Attack Behaviours")]
            [SerializeField]
            private Vector2 m_shoulderBashVelocity;
            public Vector2 shoulderBashVelocity => m_shoulderBashVelocity;
            [SerializeField]
            private SimpleAttackInfo m_shoulderBashAttack = new SimpleAttackInfo();
            public SimpleAttackInfo shoulderBashAttack => m_shoulderBashAttack;
            [SerializeField]
            private SimpleAttackInfo m_shoulderBashHookAttack = new SimpleAttackInfo();
            public SimpleAttackInfo shoulderBashHookAttack => m_shoulderBashHookAttack;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_shoulderBashLoopAnimation;
            public string shoulderBashLoopAnimation => m_shoulderBashLoopAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_shoulderBashEndAnimation;
            public string shoulderBashEndAnimation => m_shoulderBashEndAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_shoulderBashAnimation;
            public string shoulderBashAnimation => m_shoulderBashAnimation;
            #endregion
            #region ChainFistPunch
            [SerializeField]
            private float m_punchVelocity;
            public float punchVelocity => m_punchVelocity;
            [SerializeField]
            private SimpleAttackInfo m_chainFistPunchAttack = new SimpleAttackInfo();
            public SimpleAttackInfo chainFistPunchAttack => m_chainFistPunchAttack;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_chainFistPunchUpperAnimation;
            public string chainFistPunchUpperAnimation => m_chainFistPunchUpperAnimation;
            #endregion
            #region LeapAttack
            [SerializeField]
            private SimpleAttackInfo m_leapAttack = new SimpleAttackInfo();
            public SimpleAttackInfo leapAttack => m_leapAttack;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_leapfirstAttackAnimation;
            public string leapfirstAttackAnimation => m_leapfirstAttackAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_leapTransitionAnimation;
            public string leapTransitionAnimation => m_leapTransitionAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_leapAttackEndAnimation;
            public string leapAttackEndAnimation => m_leapAttackEndAnimation;
            [SerializeField, TabGroup("Leap Attack Values")]
            private float m_leapVelocity;
            public float leapVelocity => m_leapVelocity;
            [SerializeField, TabGroup("Leap Attack Values")]
            private float m_transitionStart;
            public float transitionStart => m_transitionStart;
            #endregion
            #region ChainShock
            [SerializeField]
            private SimpleAttackInfo m_chainShockAttack = new SimpleAttackInfo();
            public SimpleAttackInfo chainShockAttack => m_chainShockAttack;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_chainShockLoopAnimation;
            public string chainShockLoopAnimation => m_chainShockLoopAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_chainShockEndAnimation;
            public string chainShockEndAnimation => m_chainShockEndAnimation;
            [SerializeField]
            private float m_shockTime;
            public float shockTime => m_shockTime;
            #endregion
            [SerializeField]
            private SimpleAttackInfo m_lightningStompAttack = new SimpleAttackInfo();
            public SimpleAttackInfo lightningStompAttack => m_lightningStompAttack;

            [Title("Misc")]
            [SerializeField]
            private float m_targetDistanceTolerance;
            public float targetDistanceTolerance => m_targetDistanceTolerance;

            [Title("Animations")]
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_introAnimation;
            public string introAnimation => m_introAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idle2Animation;
            public string idle2Animation => m_idle2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathAnimation;
            public string deathAnimation => m_deathAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_roarAnimation;
            public string roarAnimation => m_roarAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_hookTravelLoopAnimation;
            public string hookTravelLoopAnimation => m_hookTravelLoopAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_hookBackLoopAnimation;
            public string hookBackLoopAnimation => m_hookBackLoopAnimation;

            [Title("Projectiles")]
            [SerializeField]
            private SimpleProjectileAttackInfo m_stompProjectile;
            public SimpleProjectileAttackInfo stompProjectile => m_stompProjectile;

            [Title("Events")]
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_phaseEvent;
            public string phaseEvent => m_phaseEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_stopRoarEvent;
            public string stopRoarEvent => m_stopRoarEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_stompEvent;
            public string stompEvent => m_stompEvent;
            [SerializeField, ValueDropdown("GetEvents")]
            private string m_leapEvent;
            public string leapEvent => m_leapEvent;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_move.SetData(m_skeletonDataAsset);
                m_shoulderBashAttack.SetData(m_skeletonDataAsset);
                m_shoulderBashHookAttack.SetData(m_skeletonDataAsset);
                m_chainFistPunchAttack.SetData(m_skeletonDataAsset);
                m_leapAttack.SetData(m_skeletonDataAsset);
                m_chainShockAttack.SetData(m_skeletonDataAsset);
                m_lightningStompAttack.SetData(m_skeletonDataAsset);
                m_stompProjectile.SetData(m_skeletonDataAsset);
#endif
            }
        }

        [System.Serializable]
        public class PhaseInfo : IPhaseInfo
        {
            [SerializeField]
            private List<float> m_patternCount;
            public List<float> patternCount => m_patternCount;
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
            AttackPattern1,
            AttackPattern2,
            AttackPattern3,
            AttackPattern4,
            WaitAttackEnd,
        }

        private enum Attack
        {
            ShoulderBash,
            ShoulderBashHook,
            ChanFistPunch,
            LightningStomp,
            ChainShock,
            WaitAttackEnd,
        }

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
        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private MovementHandle2D m_movement;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Sensors")]
        private RaySensor m_groundSensor;
        [SerializeField, TabGroup("Effects")]
        private GameObject m_wallStickStartFX;
        [SerializeField, TabGroup("Effects")]
        private GameObject m_wallStickLoopFX;
        [SerializeField, TabGroup("Effects")]
        private GameObject m_wallStickEndFX;
        [SerializeField, TabGroup("Effects")]
        private GameObject m_leapFX;
        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_orbLightningFX;
        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_stompFX;
        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_bodyLightningFX;
        [SerializeField, TabGroup("Effects")]
        private ParticleFX m_phase3FX;
        [SerializeField]
        private SpineEventListener m_spineListener;

        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        State m_turnState;
        [ShowInInspector]
        private PhaseHandle<Phase, PhaseInfo> m_phaseHandle;
        [ShowInInspector]
        private RandomAttackDecider<Pattern> m_patternDecider;
        [ShowInInspector]
        private RandomAttackDecider<Attack>[] m_attackDecider;
        private Pattern m_chosenPattern;
        private Pattern m_previousPattern;
        private Attack m_currentAttack;
        private float m_currentAttackRange;
        private ProjectileLauncher m_projectileLauncher;

        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_headPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_wristPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_wallPosPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_fistPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_fistRefPoint;
        [SerializeField, TabGroup("Spawn Points")]
        private Transform m_projectilePoint;
        [SerializeField, TabGroup("Chain")]
        private BoxCollider2D m_chainHurtBox;

        private int m_currentPhaseIndex;
        private float m_attackCount;
        private float[] m_patternCount;
        private float m_currentLeapDuration;
        private bool m_stickToGround;
        private bool m_stickToWall;

        private void ApplyPhaseData(PhaseInfo obj)
        {
            m_currentPhaseIndex = obj.phaseIndex;
            for (int i = 0; i < m_patternCount.Length; i++)
            {
                m_patternCount[i] = obj.patternCount[i];
            }
        }

        private void ChangeState()
        {
            m_stateHandle.OverrideState(State.Phasing);
            StopAllCoroutines();
            m_animation.DisableRootMotion();
            m_wallPosPoint.SetParent(m_hitbox.transform);
            m_fistPoint.GetComponent<SkeletonUtilityBone>().enabled = false;
            m_wallPosPoint.gameObject.SetActive(false);
            m_chainHurtBox.gameObject.SetActive(false);
            m_stickToWall = false;
            m_animation.SetEmptyAnimation(0, 0);
            m_phaseHandle.ApplyChange();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

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
            if (m_stateHandle.currentState != State.Phasing)
            {
                m_animation.animationState.TimeScale = 1f;
                m_stateHandle.ApplyQueuedState();
            }
        }

        private void CustomTurn()
        {
            transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            m_character.SetFacing(transform.localScale.x == 1 ? HorizontalDirection.Right : HorizontalDirection.Left);
        }

        private IEnumerator IntroRoutine()
        {
            m_stateHandle.Wait(State.ReevaluateSituation);
            m_movement.Stop();
            m_hitbox.SetInvulnerability(true);
            yield return new WaitForSeconds(2);
            m_animation.SetAnimation(0, m_info.move.animation, true);
            yield return new WaitForSeconds(5);
            GetComponentInChildren<MeshRenderer>().sortingOrder = 99;
            m_animation.SetAnimation(0, m_info.introAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.introAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            m_hitbox.SetInvulnerability(false);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator ChangePhaseRoutine()
        {
            m_hitbox.SetInvulnerability(false);
            m_animation.SetAnimation(0, m_info.roarAnimation, false);
            yield return new WaitForSeconds(3.9f);
            m_hitbox.SetInvulnerability(false);
            StartCoroutine(StickToGroundRoutine(GroundPosition().y));
            StartCoroutine(LeapAttackRoutine(6));
            yield return null;
        }
        #region Attacks
        private IEnumerator ShoulderBashRoutine()
        {
            m_animation.EnableRootMotion(true, false);
            m_animation.SetAnimation(0, m_info.shoulderBashAnimation, false).MixDuration = 0;
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.shoulderBashAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true).MixDuration = 0;
            DecidedOnAttack(false);
            m_animation.DisableRootMotion();
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator ChainShockRoutine()
        {
            m_fistPoint.GetComponent<SkeletonUtilityBone>().enabled = true;
            m_animation.SetAnimation(0, m_info.chainShockAttack.animation, false);
            yield return new WaitForSeconds(.65f);
            m_fistPoint.position = m_wristPoint.position;
            m_animation.SetAnimation(0, m_info.hookTravelLoopAnimation, true);
            var wallPos = WallPosition();
            while (Vector2.Distance(m_fistPoint.position, wallPos) > 1.5f)
            {
                m_fistPoint.position = Vector2.MoveTowards(m_fistPoint.position, wallPos, 5);
                yield return null;
            }
            var fxPos = new Vector2(m_fistPoint.position.x + (2.5f * transform.localScale.x), m_fistPoint.position.y);
            var wallStickStartFX = GameSystem.poolManager.GetPool<FXPool>().GetOrCreateItem(m_wallStickStartFX);
            wallStickStartFX.transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.localScale.x > 0 ? 90 : 270));
            wallStickStartFX.transform.position = fxPos;
            var wallStickLoopFX = GameSystem.poolManager.GetPool<FXPool>().GetOrCreateItem(m_wallStickLoopFX);
            wallStickLoopFX.transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.localScale.x > 0 ? 90 : 270));
            wallStickLoopFX.transform.position = fxPos;
            m_chainHurtBox.gameObject.SetActive(true);
            m_chainHurtBox.size = new Vector2((Vector2.Distance(m_wristPoint.position, wallPos)) * .65f, m_chainHurtBox.size.y);
            m_chainHurtBox.offset = new Vector2(m_chainHurtBox.size.x * .5f, 0);
            m_animation.SetAnimation(0, m_info.chainShockLoopAnimation, true);
            yield return new WaitForSeconds(m_info.shockTime);
            wallStickLoopFX.GetComponent<FX>().Stop();
            var wallStickEndFX = GameSystem.poolManager.GetPool<FXPool>().GetOrCreateItem(m_wallStickEndFX);
            wallStickEndFX.transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.localScale.x > 0 ? 90 : 270));
            wallStickEndFX.transform.position = fxPos;
            m_chainHurtBox.gameObject.SetActive(false);
            m_animation.SetAnimation(0, m_info.hookBackLoopAnimation, true);
            while (Vector2.Distance(m_fistPoint.position, m_wristPoint.position) > 1.5f)
            {
                m_fistPoint.position = Vector2.MoveTowards(m_fistPoint.position, m_wristPoint.position, 5);
                yield return null;
            }
            m_animation.SetAnimation(0, m_info.chainShockEndAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.chainShockEndAnimation);
            m_fistPoint.GetComponent<SkeletonUtilityBone>().enabled = false;
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            DecidedOnAttack(false);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator ShoulderBashHookRoutine()
        {
            m_fistPoint.GetComponent<SkeletonUtilityBone>().enabled = true;
            m_wallPosPoint.position = WallPosition();
            m_wallPosPoint.SetParent(null);
            m_animation.SetAnimation(0, m_info.shoulderBashAttack.animation, false);
            yield return new WaitForSeconds(.65f);
            m_fistPoint.position = m_wristPoint.position;
            m_animation.SetAnimation(0, m_info.hookTravelLoopAnimation, true);
            while (Vector2.Distance(m_fistPoint.position, m_wallPosPoint.position) > 3f)
            {
                m_fistPoint.position = Vector2.MoveTowards(m_fistPoint.position, m_wallPosPoint.position, 5);
                yield return null;
            }
            var fxPos = new Vector2(m_fistPoint.position.x + (2.5f * transform.localScale.x), m_fistPoint.position.y);
            var wallStickStartFX = GameSystem.poolManager.GetPool<FXPool>().GetOrCreateItem(m_wallStickStartFX);
            wallStickStartFX.transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.localScale.x > 0 ? 90 : 270));
            wallStickStartFX.transform.position = fxPos;
            var wallStickLoopFX = GameSystem.poolManager.GetPool<FXPool>().GetOrCreateItem(m_wallStickLoopFX);
            wallStickLoopFX.transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.localScale.x > 0 ? 90 : 270));
            wallStickLoopFX.transform.position = fxPos;
            m_wallPosPoint.gameObject.SetActive(true);
            StartCoroutine(StickToWallRoutine(m_wallPosPoint.position));
            GetComponentInChildren<SkeletonRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            m_animation.SetAnimation(0, m_info.shoulderBashLoopAnimation, true);
            while (Vector2.Distance(transform.position, m_fistPoint.position) > 15f)
            {
                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.shoulderBashReelSpeed);
                yield return null;
            }
            wallStickLoopFX.GetComponent<FX>().Stop();
            var wallStickEndFX = GameSystem.poolManager.GetPool<FXPool>().GetOrCreateItem(m_wallStickEndFX);
            wallStickEndFX.transform.rotation = Quaternion.Euler(new Vector3(0, 0, transform.localScale.x > 0 ? 90 : 270));
            wallStickEndFX.transform.position = fxPos;
            m_wallPosPoint.gameObject.SetActive(false);
            GetComponentInChildren<SkeletonRenderer>().maskInteraction = SpriteMaskInteraction.None;
            m_stickToWall = false;
            m_animation.SetAnimation(0, m_info.shoulderBashEndAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.shoulderBashEndAnimation);
            m_fistPoint.GetComponent<SkeletonUtilityBone>().enabled = false;
            m_wallPosPoint.SetParent(m_hitbox.transform);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            DecidedOnAttack(false);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator ChainFistPunchRoutine()
        {
            m_animation.SetAnimation(0, m_info.chainFistPunchAttack.animation, false);
            yield return new WaitForSeconds(0.65f);
            m_character.physics.SetVelocity(m_info.punchVelocity * transform.localScale.x, 0);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.chainFistPunchAttack.animation);
            if (!IsFacingTarget())
            {
                CustomTurn();
            }
            m_animation.SetAnimation(0, m_info.chainFistPunchUpperAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.chainFistPunchUpperAnimation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            DecidedOnAttack(false);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private IEnumerator LightningStompRoutine()
        {
            m_animation.SetAnimation(0, m_info.lightningStompAttack.animation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.lightningStompAttack.animation);
            m_animation.SetAnimation(0, m_info.idleAnimation, true);
            DecidedOnAttack(false);
            m_stateHandle.ApplyQueuedState();
            yield return null;
        }

        private void LaunchProjectile()
        {
            m_stompFX.Play();
            var target = new Vector2(m_projectilePoint.position.x + (5 * transform.localScale.x), m_projectilePoint.position.y);
            m_projectileLauncher.AimAt(target);
            m_projectileLauncher.LaunchProjectile();
        }

        private IEnumerator LeapAttackRoutine(int repeats)
        {
            for (int i = 0; i < repeats; i++)
            {
                m_animation.SetAnimation(0, i == 0 ? m_info.leapfirstAttackAnimation : m_info.leapAttack.animation, false).AnimationStart = i == 0 ? 0 : m_info.transitionStart;
                m_animation.animationState.GetCurrent(0).MixDuration = 0;
                while (m_currentLeapDuration < .65f)
                {
                    m_movement.MoveTowards(Vector2.one * transform.localScale.x, UnityEngine.Random.Range(m_info.leapVelocity * .1f, m_info.leapVelocity));
                    m_currentLeapDuration += Time.deltaTime;
                    yield return null;
                }
                m_currentLeapDuration = 0;
                m_movement.Stop();
                yield return new WaitForAnimationComplete(m_animation.animationState, i == 0 ? m_info.leapfirstAttackAnimation : m_info.leapAttack.animation);
                if (i < repeats-1)
                {
                    m_animation.SetAnimation(0, m_info.leapTransitionAnimation, false).MixDuration = 0;
                    yield return new WaitForSeconds(m_info.transitionStart);
                    CustomTurn();
                }
            }
            m_animation.SetAnimation(0, m_info.leapAttackEndAnimation, false);
            yield return new WaitForAnimationComplete(m_animation.animationState, m_info.leapAttackEndAnimation);
            m_stickToGround = false;
            DecidedOnAttack(false);
            m_stateHandle.OverrideState(State.Chasing);
            yield return null;
        }
        #endregion

        private IEnumerator StickToGroundRoutine(float groundPoint)
        {
            m_stickToGround = true;
            while (m_stickToGround)
            {
                transform.position = new Vector2(transform.position.x, groundPoint);
                yield return null;
            }
        }

        private IEnumerator StickToWallRoutine(Vector2 wallPoint)
        {
            m_stickToWall = true;
            while (m_stickToWall)
            {
                m_fistPoint.position = new Vector2(wallPoint.x, wallPoint.y);
                m_wallPosPoint.position = new Vector2(m_wallPosPoint.position.x, wallPoint.y);
                yield return null;
            }
        }

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            base.OnDestroyed(sender, eventArgs);
            StopAllCoroutines();
            m_bodyLightningFX.Stop();
            m_wallStickLoopFX.GetComponent<FX>().Stop();
            m_phase3FX.Stop();
            StartCoroutine(StickToGroundRoutine(GroundPosition().y));
            //m_deathFX.Play();
            m_movement.Stop();
        }
        
        #region Movement
        private void MoveToTarget(float targetRange)
        {
            if (!IsTargetInRange(targetRange) && m_groundSensor.isDetecting /*&& !m_wallSensor.isDetecting && m_edgeSensor.isDetecting*/)
            {
                m_animation.SetAnimation(0, m_info.move.animation, true);
                m_movement.MoveTowards(Vector2.one * transform.localScale.x, m_info.move.speed);
            }
            else
            {
                m_movement.Stop();
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
            m_patternDecider.SetList(new AttackInfo<Pattern>(Pattern.AttackPattern1, m_info.targetDistanceTolerance),
                                    new AttackInfo<Pattern>(Pattern.AttackPattern2, m_info.targetDistanceTolerance),
                                    new AttackInfo<Pattern>(Pattern.AttackPattern3, m_info.targetDistanceTolerance),
                                    new AttackInfo<Pattern>(Pattern.AttackPattern4, m_info.targetDistanceTolerance));
            m_attackDecider[0].SetList(new AttackInfo<Attack>(Attack.ShoulderBash, m_info.shoulderBashAttack.range)
                                    , new AttackInfo<Attack>(Attack.ShoulderBashHook, m_info.shoulderBashHookAttack.range)
                                    , new AttackInfo<Attack>(Attack.ChanFistPunch, m_info.chainFistPunchAttack.range));
            m_attackDecider[1].SetList(new AttackInfo<Attack>(Attack.ShoulderBash, m_info.shoulderBashAttack.range)
                                    , new AttackInfo<Attack>(Attack.ChanFistPunch, m_info.chainFistPunchAttack.range)
                                    , new AttackInfo<Attack>(Attack.LightningStomp, m_info.lightningStompAttack.range));
            m_attackDecider[2].SetList(new AttackInfo<Attack>(Attack.ShoulderBashHook, m_info.shoulderBashHookAttack.range)
                                    , new AttackInfo<Attack>(Attack.ChanFistPunch, m_info.chainFistPunchAttack.range)
                                    , new AttackInfo<Attack>(Attack.LightningStomp, m_info.lightningStompAttack.range)
                                    , new AttackInfo<Attack>(Attack.ChainShock, m_info.chainShockAttack.range));
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

        private Vector2 WallPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(m_wristPoint.position, Vector2.right * transform.localScale.x, 1000, LayerMask.GetMask("Environment"));
            return hit.point;
        }

        private Vector2 GroundPosition()
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1000, LayerMask.GetMask("Environment"));
            return hit.point;
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
                        case Attack.ShoulderBash:
                            if (patternIndex == 0 || patternIndex == 1)
                            {
                                if (m_currentPhaseIndex != 3)
                                {
                                    m_attackCount++;
                                    StartCoroutine(ShoulderBashRoutine());
                                }
                                else
                                {
                                    DecidedOnAttack(false);
                                    m_stateHandle.ApplyQueuedState();
                                }
                            }
                            else
                            {
                                DecidedOnAttack(false);
                                m_stateHandle.ApplyQueuedState();
                            }
                            break;
                        case Attack.ChanFistPunch:
                            if (patternIndex == 0 || patternIndex == 1 || patternIndex == 2 && m_currentPhaseIndex != 3)
                            {
                                m_attackCount++;
                                StartCoroutine(ChainFistPunchRoutine());
                            }
                            else
                            {
                                DecidedOnAttack(false);
                                m_stateHandle.ApplyQueuedState();
                            }
                            break;
                        case Attack.ShoulderBashHook:
                            if (patternIndex == 0 || patternIndex == 2)
                            {
                                if (AllowAttack(2, State.Attacking))
                                {
                                    m_attackCount++;
                                    StartCoroutine(ShoulderBashHookRoutine());
                                }
                            }
                            else
                            {
                                DecidedOnAttack(false);
                                m_stateHandle.ApplyQueuedState();
                            }
                            break;
                        case Attack.LightningStomp:
                            if (AllowAttack(3, State.Attacking))
                            {
                                m_attackCount++;
                                StartCoroutine(LightningStompRoutine());
                            }
                            break;
                        case Attack.ChainShock:
                            if (patternIndex == 1)
                            {
                                if (AllowAttack(3, State.Attacking))
                                {
                                    m_attackCount++;
                                    StartCoroutine(ChainShockRoutine());
                                }
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
            else
            {
                if (patternIndex == 0 || patternIndex == 1)
                {
                    if (patternIndex == 1 && m_currentPhaseIndex == 1)
                    {
                        m_stateHandle.OverrideState(State.Chasing);
                        return;
                    }

                    if (IsTargetInRange(m_info.leapAttack.range))
                    {
                        var leapCount = 3;
                        if (patternIndex == 1 && m_currentPhaseIndex == 3)
                        {
                            leapCount = 6;
                        }
                        m_stateHandle.Wait(State.Chasing);
                        StartCoroutine(StickToGroundRoutine(GroundPosition().y));
                        StartCoroutine(LeapAttackRoutine(leapCount));
                    }
                    else
                    {
                        MoveToTarget(m_info.leapAttack.range);
                    }
                }
                else if (patternIndex == 2)
                {
                    m_stateHandle.OverrideState(State.Chasing);
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
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathAnimation);
            m_projectileLauncher = new ProjectileLauncher(m_info.stompProjectile.projectileInfo, m_projectilePoint);
            m_patternDecider = new RandomAttackDecider<Pattern>();
            m_attackDecider = new RandomAttackDecider<Attack>[3];
            for (int i = 0; i < 3; i++)
            {
                m_attackDecider[i] = new RandomAttackDecider<Attack>();
            }
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitBehaviourEnd);
            UpdateAttackDeciderList();
            m_patternCount = new float[4];
            m_attackCache = new List<Attack>();
            AddToAttackCache(Attack.ChainShock, Attack.ChanFistPunch, Attack.LightningStomp, Attack.ShoulderBash, Attack.ShoulderBashHook);
            m_attackRangeCache = new List<float>();
            AddToRangeCache(m_info.chainShockAttack.range, m_info.chainFistPunchAttack.range, m_info.lightningStompAttack.range, m_info.shoulderBashAttack.range, m_info.shoulderBashHookAttack.range);
            m_attackUsed = new bool[m_attackCache.Count];
        }

        private void PhaseFX()
        {
            m_orbLightningFX.Play();
            m_bodyLightningFX.Play();
            if (m_currentPhaseIndex == 3)
            {
                m_phase3FX.Play();
            }
        }

        private void LeapFX()
        {
            if (m_animation.GetCurrentAnimation(0).ToString() != m_info.leapAttackEndAnimation)
            {
                var fxPool = GameSystem.poolManager.GetPool<FXPool>().GetOrCreateItem(m_leapFX);
                fxPool.transform.position = new Vector2(transform.position.x + (17 * transform.localScale.x), transform.position.y - 1.5f);
            }
        }

        protected override void Start()
        {
            base.Start();
            m_spineListener.Subscribe(m_info.phaseEvent, PhaseFX);
            m_spineListener.Subscribe(m_info.leapEvent, LeapFX);
            m_spineListener.Subscribe(m_info.stopRoarEvent, m_orbLightningFX.Stop);
            m_spineListener.Subscribe(m_info.stompEvent, LaunchProjectile);
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
                    m_stateHandle.Wait(State.ReevaluateSituation);
                    StartCoroutine(ChangePhaseRoutine());
                    break;
                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                    m_movement.Stop();
                    break;
                case State.Attacking:
                    if (IsFacingTarget())
                    {
                        switch (m_chosenPattern)
                        {
                            case Pattern.AttackPattern1:
                                UpdateRangeCache(m_info.chainShockAttack.range, m_info.chainFistPunchAttack.range, m_info.lightningStompAttack.range, m_currentPhaseIndex != 3 ? m_info.shoulderBashAttack.range : m_info.lightningStompAttack.range, m_info.shoulderBashHookAttack.range);
                                ExecuteAttack(0);
                                break;
                            case Pattern.AttackPattern2:
                                UpdateRangeCache(m_info.chainShockAttack.range, m_info.chainFistPunchAttack.range, m_info.lightningStompAttack.range, m_currentPhaseIndex != 3 ? m_info.shoulderBashAttack.range : m_info.chainShockAttack.range, m_info.shoulderBashHookAttack.range);
                                ExecuteAttack(1);
                                break;
                            case Pattern.AttackPattern3:
                                UpdateRangeCache(m_info.chainShockAttack.range, m_currentPhaseIndex != 3 ? m_info.chainFistPunchAttack.range : m_info.chainShockAttack.range, m_info.lightningStompAttack.range, m_info.shoulderBashHookAttack.range, m_info.shoulderBashHookAttack.range);
                                ExecuteAttack(2);
                                break;
                            case Pattern.AttackPattern4:
                                if (AllowAttack(3, State.Chasing))
                                {
                                    if (IsTargetInRange(m_info.leapAttack.range))
                                    {
                                        m_stateHandle.Wait(State.Chasing);
                                        StartCoroutine(StickToGroundRoutine(GroundPosition().y));
                                        StartCoroutine(LeapAttackRoutine(6));
                                    }
                                    else
                                    {
                                        MoveToTarget(m_info.leapAttack.range);
                                    }
                                }
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
            m_stickToGround = false;
            //m_currentCD = 0;
        }
    }
}