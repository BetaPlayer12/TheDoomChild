using DChild.Gameplay.Items;

namespace DChild.Gameplay.Inventories
{
    public interface ITradableInventory
    {
        int soulEssence { get; }
        void AddSoulEssence(int value);
        void AddItem(ItemData item, int count);
        int GetCurrentAmount(ItemData itemData);
        bool CanAfford(int cost);
    }
}
