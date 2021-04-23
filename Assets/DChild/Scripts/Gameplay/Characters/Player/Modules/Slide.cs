using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class Slide : MonoBehaviour, IResettableBehaviour, ICancellableBehaviour, IComplexCharacterModule, ISlide
    {
        [SerializeField, MinValue(0)]
        private float m_velocity;
        [SerializeField, MinValue(0)]
        private float m_cooldown;
        [SerializeField, MinValue(0)]
        private float m_duration;

        private float m_cooldownTimer;
        private float m_durationTimer;

        private Character m_character;
        private Rigidbody2D m_rigidbody;
        private ISlideState m_state;
        private Animator m_animator;
        private int m_animationParameter;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_character = info.character;
            m_rigidbody = info.rigidbody;
            m_state = info.state;
            m_state.canSlide = true;
            m_animator = info.animator;
            m_animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsSliding);
        }

        public void Execute()
        {
            if (m_state.isSliding == false)
            {
                m_state.canSlide = false;
                m_animator.SetBool(m_animationParameter, true);
                m_state.isSliding = true;
            }

            var direction = (float)m_character.facing;
            m_rigidbody.velocity = new Vector2(0, m_rigidbody.velocity.y);
            m_rigidbody.AddForce(new Vector2(direction * m_velocity, 0), ForceMode2D.Impulse);
        }

        public void Reset()
        {
            m_state.canSlide = true;
        }

        public void Cancel()
        {
            m_rigidbody.velocity = Vector2.zero;
            m_state.isSliding = false;
            m_animator.SetBool(m_animationParameter, false);
        }

        public void HandleCooldown()
        {
            if (m_cooldownTimer <= 0)
            {
                Reset();
            }
            else
            {
                m_cooldownTimer -= GameplaySystem.time.deltaTime;
            }
        }

        public void ResetCooldownTimer()
        {
            m_cooldownTimer = m_cooldown;
        }

        public void HandleDurationTimer() => m_durationTimer -= GameplaySystem.time.deltaTime;

        public bool IsSlideDurationOver() => m_durationTimer <= 0;

        public void ResetDurationTimer() => m_durationTimer = m_duration;
    }
}
