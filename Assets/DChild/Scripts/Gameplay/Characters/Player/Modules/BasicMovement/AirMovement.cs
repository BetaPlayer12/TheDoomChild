using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class AirMovement : MonoBehaviour, IPlayerExternalModule, IEventModule
    {
        [SerializeField]
        private AirMoveHandler m_moveHandler;
        private CharacterPhysics2D m_characterPhysics2D;
        private IMoveState m_state;
        private IFacingConfigurator m_facingConfig;

        public void Initialize(IPlayerModules player)
        {
            m_characterPhysics2D = player.physics;
            m_moveHandler.SetPhysics(m_characterPhysics2D);
            m_state = player.characterState;
            m_facingConfig = player;
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
                m_facingConfig.SetFacing(direction > 0 ? HorizontalDirection.Right : HorizontalDirection.Left);
            }
        }

        public void UpdateVelocity()
        {
            var moveDirection = m_characterPhysics2D.velocity.x >= 0 ? Vector2.right : Vector2.left;
            m_moveHandler.SetDirection(moveDirection);
        }

        public void ConnectEvents()
        {
            GetComponentInParent<IAirMoveController>().MoveCall += OnMoveCall;
        }

        private void OnMoveCall(object sender, ControllerEventArgs eventArgs)
        {
            Move(eventArgs.input.direction.horizontalInput);
        }

#if UNITY_EDITOR
        public void Initialize(float maxSpeed, float acceleration, float decceleration)
        {
            m_moveHandler = new AirMoveHandler(maxSpeed, acceleration, decceleration);
        }
#endif
    }
}