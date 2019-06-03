using DChild.Gameplay;
using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class AirMoveDoubleJumpHandler : MonoBehaviour, IPlayerExternalModule
    {
        [SerializeField]
        private float m_additionalSpeed;
        private AirMoveHandler m_handler;
        private IFacing m_facing;
        private IPlacementState m_state;

        private CharacterPhysics2D m_characterPhysics;

        private void OnEnable()
        {
            m_characterPhysics = GetComponentInParent<CharacterPhysics2D>();
        }

        private void Move(float direction)
        {
            var facing = m_facing.currentFacingDirection == HorizontalDirection.Left ? Vector2.left : Vector2.right;

            if (m_state.isFalling)
            {
                if(direction == 0)
                {
                    m_characterPhysics.SetVelocity(0);
                }
            }
            else
            {
                if(direction != 0)
                {
                    m_characterPhysics.AddForce(m_additionalSpeed * facing);
                }
            }
        }

        private void Update()
        {
            Move(Input.GetAxis("Horizontal"));
        }

        public void Initialize(IPlayerModules player)
        {
            m_facing = player;
            m_state = player.characterState;
        }
    }
}