using DChild.Gameplay.Characters.Players.Behaviour;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{

    public class Movement : MonoBehaviour, ICancellableBehaviour, IComplexCharacterModule
    {
        public enum Type
        {
            Crouch,
            Jog,
            MidAir,
            Pull,
            Push
        }

        [SerializeField, HideLabel]
        private MovementStatsInfo m_configuration;

        private float m_currentSpeed;
        private IPlayerModifer m_modifier;
        private Rigidbody2D m_rigidbody;
        private Character m_character;
        private Animator m_animator;
        private int m_speedAnimationParameter;

        [HideInEditorMode, ShowInInspector, ReadOnly]
        protected float speed => m_currentSpeed * m_modifier?.Get(PlayerModifier.MoveSpeed) ?? 1;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_modifier = info.modifier;
            m_character = info.character;
            m_rigidbody = info.rigidbody;
            SwitchConfigTo(Type.Jog);
            m_animator = info.animator;
            m_speedAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedX);
        }

        public void SetConfiguration(MovementStatsInfo info)
        {
            m_configuration.CopyInfo(info);
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
                    m_currentSpeed = m_configuration.crouchSpeed;
                    break;
                case Type.Jog:
                    m_currentSpeed = m_configuration.jogSpeed;
                    break;
                case Type.MidAir:
                    m_currentSpeed = m_configuration.midAirSpeed;
                    break;
                case Type.Pull:
                    m_currentSpeed = m_configuration.pullSpeed;
                    break;
                case Type.Push:
                    m_currentSpeed = m_configuration.pushSpeed;
                    break;
            }
        }

        public void UpdateFaceDirection(float direction)
        {
            if (Mathf.Sign(direction) != (int)m_character.facing)
            {
                var otherFacing = m_character.facing == HorizontalDirection.Right ? HorizontalDirection.Left : HorizontalDirection.Right;
                m_character.SetFacing(otherFacing);
            }
        }

        public void Move(float direction, bool faceDirection)
        {
            if (direction == 0)
            {
                m_animator.SetFloat(m_speedAnimationParameter, 0);
            }
            else
            {
                if (faceDirection == true)
                {
                    UpdateFaceDirection(direction);
                }

                m_animator.SetFloat(m_speedAnimationParameter, 1);
            }
            var xVelocity = speed * direction;
            m_rigidbody.velocity = new Vector2(xVelocity, m_rigidbody.velocity.y);
        }
    }
}
