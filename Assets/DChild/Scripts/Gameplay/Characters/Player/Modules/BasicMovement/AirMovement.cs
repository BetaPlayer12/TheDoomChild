using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using Refactor.DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class AirMovement : MonoBehaviour, IComplexCharacterModule
    {
        [SerializeField]
        private AirMoveHandler m_moveHandler;
        private CharacterPhysics2D m_characterPhysics2D;
        private Character m_character;
        private IMoveState m_state;

        private Animator m_animator;
        private string m_speedXParameter;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_characterPhysics2D = info.physics;
            m_moveHandler.SetPhysics(m_characterPhysics2D);
            m_character = info.character;
            m_state = info.state;
            m_animator = info.animator;
            m_speedXParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedX);
        }

        public void Move(float direction)
        {
            if (direction == 0)
            {
                m_moveHandler.Deccelerate();
                m_state.isMoving = false;
                m_animator.SetInteger(m_speedXParameter, 0);
            }
            else
            {
                var moveDirection = direction > 0 ? Vector2.right : Vector2.left;
                m_moveHandler.SetDirection(moveDirection);
                m_moveHandler.Accelerate();
                m_state.isMoving = true;
                m_character.SetFacing(direction > 0 ? HorizontalDirection.Right : HorizontalDirection.Left);
                m_animator.SetInteger(m_speedXParameter, 1);
            }
        }

        public void UpdateVelocity()
        {
            var moveDirection = m_characterPhysics2D.velocity.x >= 0 ? Vector2.right : Vector2.left;
            m_moveHandler.SetDirection(moveDirection);
        }
    }
}