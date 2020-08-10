using DChild.Gameplay.Characters.Players.Behaviour;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class Movement : MonoBehaviour, ICancellableBehaviour, IComplexCharacterModule
    {
        public enum Type
        {
            Crouch,
            Jog,
            MidAir
        }

        [SerializeField]
        private float m_jogSpeed;
        [SerializeField]
        private float m_crouchSpeed;
        [SerializeField]
        private float m_midAirSpeed;

        private float m_currentSpeed;
        private Rigidbody2D m_rigidbody;
        private Character m_character;
        private Animator m_animator;
        private int m_speedAnimationParameter;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_character = info.character;
            m_rigidbody = info.rigidbody;
            SwitchConfigTo(Type.Jog);
            m_animator = info.animator;
            m_speedAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedX);
        }

        public void Cancel()
        {
            m_animator.SetFloat(m_speedAnimationParameter, 0);
            m_rigidbody.velocity = new Vector2(0, m_rigidbody.velocity.y);
        }

        public void SwitchConfigTo(Type type)
        {
            switch (type)
            {
                case Type.Crouch:
                    m_currentSpeed = m_crouchSpeed;
                    break;
                case Type.Jog:
                    m_currentSpeed = m_jogSpeed;
                    break;
                case Type.MidAir:
                    m_currentSpeed = m_midAirSpeed;
                    break;
            }
        }

        public void Move(float direction)
        {
            if (direction == 0)
            {
                m_animator.SetFloat(m_speedAnimationParameter, 0);
            }
            else
            {
                if (Mathf.Sign(direction) != (int)m_character.facing)
                {
                    var otherFacing = m_character.facing == HorizontalDirection.Right ? HorizontalDirection.Left : HorizontalDirection.Right;
                    m_character.SetFacing(otherFacing);
                }
                m_animator.SetFloat(m_speedAnimationParameter, 1);
            }
            var xVelocity = m_currentSpeed * direction;
            m_rigidbody.velocity = new Vector2(xVelocity, m_rigidbody.velocity.y);
        }
    }
}
