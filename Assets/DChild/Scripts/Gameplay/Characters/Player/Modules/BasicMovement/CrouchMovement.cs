using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using Spine.Unity.Modules;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class CrouchMovement : MonoBehaviour, IPlayerExternalModule, IEventModule
    {
        [SerializeField]
        private GroundMoveHandler m_moveHandler;
        private CharacterPhysics2D m_characterPhysics2D;
        private Animator m_animator;

        private ICrouchState m_state;
        private IFacingConfigurator m_facingConfig;

        public void Move(float direction)
        {
            if (m_state.isCrouched)
            {

                
                if (direction == 0)
                {
                    Debug.Log("in stasis");
                    m_moveHandler.Deccelerate();
                    m_state.isMoving = false;
                   
                }
                else
                {
                    Debug.Log("in moving");
                    var moveDirection = direction > 0 ? Vector2.right : Vector2.left;
                    m_moveHandler.SetDirection(moveDirection);
                    m_moveHandler.Accelerate();
                    m_state.isMoving = true;
                    m_facingConfig.SetFacing(direction > 0 ? HorizontalDirection.Right : HorizontalDirection.Left);
                }
            }
        }

        public void Initialize(IPlayerModules player)
        {
            m_characterPhysics2D = player.physics;
            m_moveHandler.SetPhysics(m_characterPhysics2D);
            m_state = player.characterState;
            m_facingConfig = player;
        }

        public void ConnectEvents()
        {
            GetComponentInParent<ICrouchController>().CrouchMoveCall += OnCrouchMoveCall;
        }

        private void OnCrouchMoveCall(object sender, ControllerEventArgs eventArgs)
        {
           
            //m_moveHandler.Deccelerate();
            Move(eventArgs.input.direction.horizontalInput);
        }

#if UNITY_EDITOR
        public void Initialize(float maxSpeed, float acceleration, float decceleration)
        {
            m_moveHandler = new GroundMoveHandler(maxSpeed, acceleration, decceleration);
        }
#endif
    }

}