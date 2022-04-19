using DChild.Gameplay.Items;
using Holysoft.Event;
using System.Collections.Generic;

namespace DChild.Gameplay.Inventories
{
    public interface IInventory : IInventoryInfo
    {
        event EventAction<ItemEventArgs> InventoryItemUpdate;
        event EventAction<EventActionArgs> MassInventoryItemUpdate;

        void AddItem(ItemData itemData, int count = 1);
        void RemoveItem(ItemData itemData, int count = 1);
        void SetItem(ItemData itemData, int count = 1);
        IStoredItem[] FindStoredItemsOfType(ItemCategory category);
        IStoredItem[] FindStoredItemsOfTypes(params ItemCategory[] categories);
    }
}