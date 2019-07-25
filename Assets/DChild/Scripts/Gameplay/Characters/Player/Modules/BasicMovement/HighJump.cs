using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class HighJump : MonoBehaviour, IPlayerExternalModule, IEventModule
    {
        [SerializeField]
        [MinValue(0f)]
        private float m_velocityReduction = 1f;
        [SerializeField]
        private float m_movingAnimationVelocityTreshold;

        private float m_jumpVelocityX;
        private IHighJumpState m_state;
        private IPlayerAnimationState m_animationState;
        protected CharacterPhysics2D m_characterPhysics2D;

        public void Initialize(IPlayerModules player)
        {
            m_animationState = player.animationState;
            m_characterPhysics2D = player.physics;
            m_state = player.characterState;
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
            m_animationState.isShortJumping = (m_jumpVelocityX < m_movingAnimationVelocityTreshold) ? true : false;
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