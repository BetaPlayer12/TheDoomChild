using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using Refactor.DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class HighJump : MonoBehaviour, IComplexCharacterModule, IControllableModule
    {
        [SerializeField]
        [MinValue(0f)]
        private float m_velocityReduction = 1f;
        [SerializeField]
        private float m_movingAnimationVelocityTreshold;

        private float m_jumpVelocityX;
        private IHighJumpState m_state;
        protected CharacterPhysics2D m_characterPhysics2D;


        public void ConnectTo(IMainController controller)
        {
            //var jumpController = controller.GetSubController<IJumpController>();
            //jumpController.JumpCall += OnHighJumpCall;
        }

        public void Initialize(ComplexCharacterInfo info)
        {
            m_characterPhysics2D = info.physics;
            m_state = info.state;
        }

        public void HandleHighJump(bool isJumpHeld)
        {
           
            if (m_state.canHighJump)
            {
               
                if (isJumpHeld)
                {
                    if (m_characterPhysics2D.velocity.y <= 0f)
                    {
                        m_state.canHighJump = false;
                    }
                }
                else
                {
                    if (m_characterPhysics2D.velocity.y > 0f)
                    {
                        var yVelocity = m_characterPhysics2D.velocity.y;
                        yVelocity /= m_velocityReduction;
                        m_characterPhysics2D.SetVelocity(y: yVelocity);                 
                        m_state.canHighJump = false;
                    }
                }
            }
        }

        public void ConnectEvents()
        {
            GetComponentInParent<IHighJumpController>().HighJumpCall += OnHighJumpCall;
        }

        private void OnHighJumpCall(object sender, ControllerEventArgs eventArgs)
        {
            HandleHighJump(eventArgs.input.isJumpHeld);

            m_jumpVelocityX = m_characterPhysics2D.velocity.x;
            m_jumpVelocityX = (m_jumpVelocityX < 0) ? -m_jumpVelocityX : m_jumpVelocityX;
        }

#if UNITY_EDITOR
        public void Initialize(float velocityReduction, float movingAnimationVelocityTreshold)
        {
            m_velocityReduction = velocityReduction;
            m_movingAnimationVelocityTreshold = movingAnimationVelocityTreshold;
        }
#endif
    }
}