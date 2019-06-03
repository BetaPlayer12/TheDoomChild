using DChild.Gameplay.Characters.Enemies;
using UnityEngine;

namespace DChild.Gameplay.Characters.AI
{
    public struct CombatCharacterVisualInfo
    {
        public Vector2 position { get; }
        public HorizontalDirection currentFacingDirection { get; }

        public CombatCharacterVisualInfo(CombatCharacter character)
        {
            position = character.position;
            currentFacingDirection = character.currentFacingDirection;
        }

        public CombatCharacterVisualInfo(IEnemyTarget target)
        {
            position = target.position;
            currentFacingDirection = target.currentFacingDirection;
        }
    }
}