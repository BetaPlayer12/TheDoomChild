using DChild.Gameplay.Characters;
using DChild.Gameplay.Systems.WorldComponents;

namespace DChild.Gameplay
{
    public interface ICharacter
    {
        IsolatedObject isolatedObject { get; }
        IsolatedCharacterPhysics2D physics { get; }
        CharacterColliders colliders { get; }
    }
}