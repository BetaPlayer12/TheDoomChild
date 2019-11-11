using DChild.Gameplay.Items;

namespace DChild.Gameplay.Inventories
{
    public interface ITradableInventory
    {
        int soulEssence { get; }
        IItemContainer items { get; }
        void AddSoulEssence(int value);
        //void AddItem(ItemData item, int count);
    }
}
