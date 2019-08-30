using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.AI
{
    public struct SimpleTurnHandler : ITurnHandler
    {
        private CombatCharacter m_character;
        public SimpleTurnHandler(CombatCharacter m_character)
        {
            this.m_character = m_character;
        }

        public void LookAt(Vector2 target)
        {
            var position = m_character.position;
            var facing = m_character.currentFacingDirection;
            if (position.x > target.x && facing != HorizontalDirection.Left)
            {
                m_character.SetFacing(HorizontalDirection.Left);
            }
            else if(position.x < target.x && facing != HorizontalDirection.Right)
            {
                m_character.SetFacing(HorizontalDirection.Right);
            }
        }
    }
}
