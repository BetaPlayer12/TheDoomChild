using DChild.Gameplay.Characters;
using DChild.Gameplay.Systems.WorldComponents;

namespace DChild.Gameplay
{
    public interface ICharacter
    {
        IsolatedObject isolatedObject { get; }
        CharacterPhysics2D physics { get; }
        CharacterColliders colliders { get; }
    }
}