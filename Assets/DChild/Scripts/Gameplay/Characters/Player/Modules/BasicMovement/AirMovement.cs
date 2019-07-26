using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using Refactor.DChild.Gameplay.Characters.Players;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class AirMovement : MonoBehaviour, IComplexCharacterModule, IControllableModule
    {
        [SerializeField]
        private AirMoveHandler m_moveHandler;
        private CharacterPhysics2D m_characterPhysics2D;
        private Character m_character;
        private IMoveState m_state;

        public void ConnectTo(IMainController controller)
        {
            controller.GetSubController<IAirMoveController>().MoveCall += OnMoveCall;
        }

        public void Initialize(ComplexCharacterInfo info)
        {
            m_characterPhysics2D = info.physics;
            m_moveHandler.SetPhysics(m_characterPhysics2D);
            m_character = info.character;
            m_state = info.state;
        }

        public void Move(float direction)
        {
            if (direction == 0)
            {
                m_moveHandler.Deccelerate();
                m_state.isMoving = false;
            }
            else
            {
                var moveDirection = direction > 0 ? Vector2.right : Vector2.left;
                m_moveHandler.SetDirection(moveDirection);
                m_moveHandler.Accelerate();
                m_state.isMoving = true;
                m_character.SetFacing(direction > 0 ? HorizontalDirection.Right : HorizontalDirection.Left);
            }
        }

        public void UpdateVelocity()
        {
            var moveDirection = m_characterPhysics2D.velocity.x >= 0 ? Vector2.right : Vector2.left;
            m_moveHandler.SetDirection(moveDirection);
        }

        private void OnMoveCall(object sender, ControllerEventArgs eventArgs)
        {
            Move(eventArgs.input.direction.horizontalInput);
        }
    }
}