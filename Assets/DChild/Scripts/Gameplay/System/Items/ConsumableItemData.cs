using DChild.Gameplay.Characters.Players;

namespace DChild.Gameplay.Items
{
    public abstract class ConsumableItemData : ItemData
    {
        public abstract bool CanBeUse(IPlayer player);
        public abstract void Use(IPlayer player);
    }
}
