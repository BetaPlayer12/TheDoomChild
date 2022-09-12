using DChild.Gameplay.Items;

namespace DChild.Gameplay.Inventories
{
    public interface IStoredItem
    {
        ItemData data { get; }
        bool hasInfiniteCount { get; }
        int count { get; }
    }

}