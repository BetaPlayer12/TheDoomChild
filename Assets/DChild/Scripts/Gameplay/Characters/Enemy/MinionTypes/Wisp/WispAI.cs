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

namespace DChild.Gameplay.Characters.Enemies
{
    [AddComponentMenu("DChild/Gameplay/Enemies/Minion/Wisp")]
    public class WispAI : CombatAIBrain<WispAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            //Basic Behaviours
            [SerializeField]
            private MovementInfo m_move = new MovementInfo();
            public MovementInfo move => m_move;

            //Animations
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_idleAnimation;
            public string idleAnimation => m_idleAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_turnAnimation;
            public string turnAnimation => m_turnAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathStartAnimation;
            public string deathStartAnimation => m_deathStartAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathFallLoopAnimation;
            public string deathFallLoopAnimation => m_deathFallLoopAnimation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathFallImpact1Animation;
            public string deathFallImpact1Animation => m_deathFallImpact1Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_deathFallImpact2Animation;
            public string deathFallImpact2Animation => m_deathFallImpact2Animation;
            [SerializeField, ValueDropdown("GetAnimations")]
            private string m_flinchAnimation;
            public string flinchAnimation => m_flinchAnimation;

            public override void Initialize()
            {
#if UNITY_EDITOR
                m_move.SetData(m_skeletonDataAsset);
#endif
            }
        }

        private enum State
        {
            Patrol,
            Turning,
            ReevaluateSituation,
            WaitBehaviourEnd,
        }

        [SerializeField, TabGroup("Reference")]
        private SpineEventListener m_spineEventListener;
        [SerializeField, TabGroup("Reference")]
        private GameObject m_bodyCollider;
        [SerializeField, TabGroup("Modules")]
        private AnimatedTurnHandle m_turnHandle;
        [SerializeField, TabGroup("Modules")]
        private WaveMovementHandler2D m_waveMovement;
        [SerializeField, TabGroup("Modules")]
        private DeathHandle m_deathHandle;
        [SerializeField, TabGroup("Modules")]
        private FlinchHandler m_flinchHandle;

        [SerializeField]
        private HorizontalDirection m_startingDirection = HorizontalDirection.Right;
        [ShowInInspector]
        private StateHandle<State> m_stateHandle;
        private State m_turnState;

        private void OnAttackDone(object sender, EventActionArgs eventArgs)
        {
            m_animation.DisableRootMotion();
            //transform.localScale = new Vector3(m_chosenAttack == Attack.Attack2 ? -transform.localScale.x : transform.localScale.x, 1, 1);
            m_stateHandle.ApplyQueuedState();
        }

        private void OnTurnRequest(object sender, EventActionArgs eventArgs) => m_stateHandle.OverrideState(State.Turning);

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
        }

        private void OnTurnDone(object sender, FacingEventArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            //m_animation.SetAnimation(0, m_info.flinchAnimation, false);
            //m_stateHandle.OverrideState(State.WaitBehaviourEnd);
            StopAllCoroutines();
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
        }

        private void CustomDirection(HorizontalDirection direction)
        {
            transform.localScale = new Vector3(direction == HorizontalDirection.Right ? transform.localScale.x : -transform.localScale.x, 1, 1);
            m_character.SetFacing(direction);
        }

        public override void ApplyData()
        {
            base.ApplyData();
        }

        private IEnumerator DeathRoutine()
        {
            m_animation.SetAnimation(0, m_info.deathStartAnimation, false);
            m_animation.EnableRootMotion(true, false);
            //yield return new WaitForAnimationComplete(m_animation.animationState, m_info.deathStartAnimation);
            yield return new WaitForSeconds(1.6f);
            //m_animation.DisableRootMotion();
            m_character.physics.simulateGravity = true;
            m_animation.SetAnimation(0, m_info.deathFallLoopAnimation, true);
            var animation = UnityEngine.Random.Range(0, 2) == 1 ? m_info.deathFallImpact1Animation : m_info.deathFallImpact2Animation;
            m_bodyCollider.SetActive(true);
            m_animation.SetAnimation(0, animation, false);
            yield return null;
        }

        private bool IsInRange(Vector2 position, float distance) => Vector2.Distance(position, m_character.centerMass.position) <= distance;

        protected override void OnDestroyed(object sender, EventActionArgs eventArgs)
        {
            //m_Audiosource.clip = m_DeadClip;
            //m_Audiosource.Play();
            base.OnDestroyed(sender, eventArgs);
            StopAllCoroutines();
            StartCoroutine(DeathRoutine());
        }

        protected override void Start()
        {
            base.Start();
            //m_selfCollider.SetActive(false);
            CustomDirection(m_startingDirection);
        }

        protected override void Awake()
        {
            Debug.Log(m_info);
            base.Awake();
            m_flinchHandle.FlinchStart += OnFlinchStart;
            m_flinchHandle.FlinchEnd += OnFlinchEnd;
            m_turnHandle.TurnDone += OnTurnDone;
            m_deathHandle.SetAnimation(m_info.deathFallImpact1Animation);
            m_stateHandle = new StateHandle<State>(State.Patrol, State.WaitBehaviourEnd);
        }

        private void Update()
        {

            switch (m_stateHandle.currentState)
            {
                case State.Patrol:
                    m_turnState = State.ReevaluateSituation;
                    m_animation.SetAnimation(0, m_info.move.animation, true);
                    m_waveMovement.MoveTowards(Vector2.right * transform.localScale.x, m_info.move.speed);
                    break;

                case State.Turning:
                    m_stateHandle.Wait(m_turnState);
                    m_turnHandle.Execute(m_info.turnAnimation, m_info.idleAnimation);
                    break;
                case State.ReevaluateSituation:
                    m_stateHandle.SetState(State.Patrol);
                    break;
                case State.WaitBehaviourEnd:
                    return;
            }
        }

        protected override void OnTargetDisappeared()
        {
            m_stateHandle.OverrideState(State.Patrol);
        }

        public void ResetAI()
        {
            m_targetInfo.Set(null, null);
            m_stateHandle.OverrideState(State.ReevaluateSituation);
            enabled = true;
        }

        protected override void OnBecomePassive()
        {
            ResetAI();
        }
    }
}

