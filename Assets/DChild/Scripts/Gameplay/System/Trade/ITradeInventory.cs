using DChild.Gameplay.Inventories;
using DChild.Gameplay.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Trade
{
    public interface ITradeInventory
    {
        int currency { get; }

        void AddCurrency(int value);
        void AddItem(ItemData item, int count);
        void RemoveItem(ItemData item, int count);
        bool HasSpaceFor(ItemData item, int count);
        ITradeItem[] GetTradableItems();

        ITradeItem GetTradeItem(ItemData item);

        ITradeItem[] FindTradeItemsOfType(ItemCategory category);
    }
}
