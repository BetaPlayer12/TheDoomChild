using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public class AttackOnEdge : MonoBehaviour, IPlayerExternalModule
    {
        [SerializeField]
        private float m_directionForce;

        private IPlayerState m_state;
        private IFacing m_facing;

        private CharacterPhysics2D m_character;
        private bool m_hasAttacked;

        public void Initialize(IPlayerModules player)
        {
            m_state = player.characterState;
            m_facing = player;
            m_character = player.physics;
        }

        private void Update()
        {
            if (m_state.isAttacking)
            {
                m_hasAttacked = true;
            }
            if (m_state.isNearEdge && m_state.isGrounded)
            {
                if (m_hasAttacked)
                {
                    var facingDirection = (m_facing.currentFacingDirection == HorizontalDirection.Left) ? Vector2.left : Vector2.right;
                    m_character.AddForce(facingDirection * m_directionForce, ForceMode2D.Impulse);
                }
            }
            else
            {
                m_hasAttacked = false;
            }
        }

#if UNITY_EDITOR
        public void Initialize(float directionForce)
        {
            m_directionForce = directionForce;
        }
#endif
    } 
}
