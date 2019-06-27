using DChild.Gameplay.Characters.Players.State;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class PlacementTracker : MonoBehaviour, IPlayerExternalModule
    {
        [MaxValue(0)]
        [SerializeField]
        private float m_velocityThreshold;

        private IPlacementState m_state;
        private CharacterPhysics2D m_physics;

        public void Initialize(IPlayerModules player)
        {
            m_state = player.characterState;
            m_physics = player.physics;
        }

        private void Start()
        {
            m_state.isGrounded = m_physics.onWalkableGround;
        }

        public void FixedUpdate()
        {
            if (m_state.isGrounded)
            {
                m_state.isFalling = false;
            }
            else
            {
                m_state.isFalling = m_physics.velocity.y < m_velocityThreshold;
                m_state.hasLanded = m_physics.onWalkableGround;
            }

            m_state.isGrounded = m_physics.onWalkableGround;
        }

        public void LateUpdate()
        {
            if (m_state.isGrounded)
            {
                m_state.hasLanded = false;
            }
        }

#if UNITY_EDITOR

        public void Initialize(float velocityThreshold)
        {
            m_velocityThreshold = velocityThreshold;
        }

#endif
    }

}