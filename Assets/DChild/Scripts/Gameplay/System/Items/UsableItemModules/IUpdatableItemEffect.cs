using DChild.Gameplay.Characters.Players;

namespace DChild.Gameplay.Items
{
    public interface IUpdatableItemEffect
    {
        IUpdatableItemEffect CreateCopy();
        void Execute(IPlayer player, float deltaTime);
    }
}
