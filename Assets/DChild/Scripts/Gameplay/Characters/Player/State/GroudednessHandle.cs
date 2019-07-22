using System;
using DChild.Gameplay.Characters.Players.State;
using Holysoft.Collections;
using Holysoft.Event;
using Refactor.DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    /// <summary>
    /// Replacement of PlacementTracker
    /// </summary>

    [System.Serializable]
    public class LandHandle
    {
        [SerializeField]
        private Animator m_animator;

        public void Execute()
        {

        }
    }

    [System.Serializable]
    public class FallHandle
    {
        [SerializeField, MaxValue(0)]
        private float m_velocityThreshold;
        [SerializeField, MinValue(0)]
        private float m_durationBeforeLongFall;

        private CountdownTimer m_timer;
        private Animator m_animator;
        private string m_animationParameter;
        private int m_speedY;

        public void Initialize(Animator animator, AnimationParametersData animationParameters)
        {
            m_animator = animator;
            m_animationParameter = animationParameters.GetParameterLabel(AnimationParametersData.Parameter.SpeedY);
            m_timer = new CountdownTimer(m_durationBeforeLongFall);
            m_timer.CountdownEnd += OnCountdownEnd;
        }

        private void OnCountdownEnd(object sender, EventActionArgs eventArgs)
        {
            m_animator.SetInteger(m_animationParameter, -2);
        }

        public void Reset()
        {
            m_timer.Reset();
            m_animator.SetInteger(m_animationParameter, -1);
        }

        public void Execute(float deltaTime)
        {
            m_timer.Tick(deltaTime);
        }

        public bool isFalling(CharacterPhysics2D physics) => physics.velocity.y < m_velocityThreshold;
    }

    public class GroudednessHandle : MonoBehaviour
    {
        private IPlacementState m_state;
        private CharacterPhysics2D m_physics;
        private AnimationParametersData m_animationParameters;

        [SerializeField]
        private FallHandle m_fallHandle;
        private LandHandle m_landHandle;

        private void Start()
        {
            m_state.isGrounded = m_physics.onWalkableGround;
        }

        public void FixedUpdate()
        {
            if (m_state.isGrounded)
            {
                m_state.isFalling = false;
                m_state.isGrounded = m_physics.onWalkableGround;
            }
            else
            {
                var isFalling = m_fallHandle.isFalling(m_physics);
                if (isFalling)
                {
                    if (m_state.isFalling)
                    {
                        m_fallHandle.Execute(Time.deltaTime);
                    }
                    else
                    {
                        m_fallHandle.Reset();
                    }
                }

                var hasLanded = m_physics.onWalkableGround;
                if (hasLanded)
                {
                    m_state.isGrounded = true;
                    m_landHandle.Execute();
                }
            }
        }

        public void LateUpdate()
        {
            if (m_state.isGrounded)
            {
                m_state.hasLanded = false;
            }
        }
    }

}