using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class FloatyPeak : MonoBehaviour, IPlayerExternalModule
    {
        [SerializeField]
        private float m_minGravity;
        [SerializeField]
        private float m_fallSpeed;

        private float m_defaultGravity;
        private float m_currentGravity;

        private bool m_hasHighJumped;

        private IPlayerState m_state;
        private CharacterPhysics2D m_physics;

        public void Initialize(IPlayerModules player)
        {
            m_physics = player.physics;
            m_defaultGravity = m_physics.gravity.gravityScale;
            m_state = player.characterState;
        }

        private void Update()
        {
            m_currentGravity = m_physics.gravity.gravityScale; //gravity changes in slope 

            if (m_state.isAttacking)
            {
               
            }
            else if (!m_state.canDoubleJump && !m_state.isFalling)
            {
                if (m_physics.velocity.y > 0)
                {
                    m_physics.gravity.gravityScale = m_minGravity + 5;
                }
            }

            else if (m_state.canHighJump && !m_state.isDashing)//((!m_state.isFalling && !m_state.isGrounded))// || (m_state.hasDoubleJumped && !m_state.isFalling))
            {
                if (m_physics.velocity.y > 0)
                {
                    m_physics.gravity.gravityScale = m_minGravity;
                }
            }
            else
            {
                if (m_physics.gravity.gravityScale < m_defaultGravity)
                {
                    if (m_state.isGrounded)
                    {
                        m_physics.gravity.gravityScale = m_currentGravity;
                    }
                    else
                    {
                        var modifiedGravity = m_physics.gravity.gravityScale + (0.1f * m_fallSpeed);
                        m_physics.gravity.gravityScale = modifiedGravity;
                    }
                }
                else m_physics.gravity.gravityScale = m_currentGravity;
            }
        }

#if UNITY_EDITOR
        public void Initialize(float minGravity, float fallSpeed)
        {
            m_minGravity = minGravity;
            m_fallSpeed = fallSpeed;
        }
#endif
    }
}
