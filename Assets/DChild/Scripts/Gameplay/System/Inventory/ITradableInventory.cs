using DChild.Gameplay.Items;
using System.Collections;
using System.Collections.Generic;

namespace DChild.Gameplay.Inventories
{
    public interface ITradableInventory
    {
        IStoredItem[] GetTradableItems();

        void Add(ItemData itemData, int count = 1);
        void Remove(ItemData itemData, int count = 1);
        bool CanAfford(int amount);
        void AddCurrency(int amount);
    }
}