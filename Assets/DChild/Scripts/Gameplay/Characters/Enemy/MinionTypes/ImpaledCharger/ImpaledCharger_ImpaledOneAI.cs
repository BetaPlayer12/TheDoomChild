using DChild.Gameplay.Combat;
using DChild.Gameplay.Characters.AI;
using UnityEngine;
using Spine.Unity;
using Sirenix.OdinInspector;
using System.Collections;
using Holysoft.Event;
using System;
using Spine;

namespace DChild.Gameplay.Characters.Enemies
{

    public class ImpaledCharger_ImpaledOneAI : CombatAIBrain<ImpaledCharger_ImpaledOneAI.Info>
    {
        [System.Serializable]
        public class Info : BaseInfo
        {
            [SerializeField]
            private BasicAnimationInfo m_idleAnimation;
            [SerializeField]
            private BasicAnimationInfo m_detectAnimation;
            [SerializeField]
            private BasicAnimationInfo m_struggleAnimation;

            public BasicAnimationInfo idleAnimation => m_idleAnimation;
            public BasicAnimationInfo detectAnimation => m_detectAnimation;
            public BasicAnimationInfo struggleAnimation => m_struggleAnimation;

            public override void Initialize()
            {
                m_idleAnimation.SetData(m_skeletonDataAsset);
                m_detectAnimation.SetData(m_skeletonDataAsset);
                m_struggleAnimation.SetData(m_skeletonDataAsset);
            }
        }

        private enum State
        {
            Idle,
            Struggle,
            WaitForBehaviour
        }

        public enum Mode
        {
            Alone,
            Manipulated
        }

        [SerializeField]
        private FlinchHandler m_flinch;
        [ShowInInspector]
        private StateHandle<State> m_stateHandle;

        public override void ReturnToSpawnPoint()
        {

        }

        protected override void OnTargetDisappeared()
        {

        }

        #region ImpaleCharger Only Functions
        public Vector2 position => m_character.centerMass.position;

        public TrackEntry SetAnimation(int index, IAIAnimationInfo animationInfo, bool loop)
        {
            return AIBrainUtility.SetAnimation(m_animation, index, animationInfo, loop);
        }

        public void SetHitboxInvulnerability(Invulnerability level)
        {
            var hitbox = m_damageable.GetHitboxes();
            for (int i = 0; i < hitbox.Length; i++)
            {
                hitbox[i].SetInvulnerability(level);
            }
        }

        public void EnableHitbox(bool enabled)
        {
            var hitbox = m_damageable.GetHitboxes();
            for (int i = 0; i < hitbox.Length; i++)
            {
                if (enabled)
                {
                    hitbox[i].Enable();
                }
                else
                {
                    hitbox[i].Disable();
                }
            }
        }
        #endregion

        public void SetMode(Mode mode)
        {
            StopAllCoroutines();
            switch (mode)
            {
                case Mode.Alone:
                    enabled = true;
                    m_aggroBoundary.gameObject.SetActive(true);
                    m_character.physics.Enable();
                    m_character.colliders.Enable();
                    if (m_targetInfo.doesTargetExist)
                    {
                        m_stateHandle.OverrideState(State.Struggle);
                    }
                    else
                    {
                        m_stateHandle.OverrideState(State.Idle);
                    }
                    break;
                case Mode.Manipulated:
                    enabled = false;
                    m_aggroBoundary.gameObject.SetActive(false);
                    m_character.physics.Disable();
                    m_character.colliders.Disable();
                    break;
            }
        }

        public override void SetTarget(IDamageable damageable, Character m_target = null)
        {
            base.SetTarget(damageable, m_target);
            if (m_stateHandle.currentState == State.Idle)
            {
                StopAllCoroutines();
                StartCoroutine(DetectTargetRoutine());
            }
        }

        private IEnumerator DetectTargetRoutine()
        {
            m_stateHandle.Wait(State.Struggle);
            var track = AIBrainUtility.SetAnimation(m_animation, 0, m_info.detectAnimation, false);
            yield return new WaitForSpineAnimationComplete(track);
            m_stateHandle.ApplyQueuedState();
        }

        private void OnFlinchEnd(object sender, EventActionArgs eventArgs)
        {
            m_stateHandle.ApplyQueuedState();
            if (m_targetInfo.doesTargetExist)
            {
                m_stateHandle.OverrideState(State.Struggle);
            }
        }

        private void OnFlinchStart(object sender, EventActionArgs eventArgs)
        {
            if (m_stateHandle.currentState == State.Idle)
            {
                m_stateHandle.Wait(State.Idle);
            }
            else
            {
                StopAllCoroutines();
                m_stateHandle.Wait(State.Struggle);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            m_stateHandle = new StateHandle<State>(State.Idle, State.WaitForBehaviour);
            m_flinch.FlinchStart += OnFlinchStart;
            m_flinch.FlinchEnd += OnFlinchEnd;
        }

        private void Update()
        {
            switch (m_stateHandle.currentState)
            {
                case State.Idle:
                    AIBrainUtility.SetAnimation(m_animation, 0, m_info.idleAnimation, true);
                    break;
                case State.Struggle:
                    AIBrainUtility.SetAnimation(m_animation, 0, m_info.struggleAnimation, true);
                    break;
                case State.WaitForBehaviour:
                    break;
            }
        }
    }
}
