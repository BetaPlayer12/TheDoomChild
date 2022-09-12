using DChild.Gameplay.Items;

namespace DChild.Gameplay.Inventories
{
    public interface IInventoryInfo
    {
        int storedItemCount { get; }
        IStoredItem GetItem(int index);
        IStoredItem GetItem(ItemData item);
    }
}