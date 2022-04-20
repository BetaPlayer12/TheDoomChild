using DChild.Gameplay.Inventories;
using DChild.Gameplay.Items;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Trade
{

    public class MerchantStore : MonoBehaviour, ITradeInventory
    {
        [SerializeField]
        private TradeRates m_tradeRates;
        [SerializeField, HideLabel, FoldoutGroup("Inventory")]
        private TradableInventory m_inventory = new TradableInventory(true, false);

        public TradeRates tradeRates => m_tradeRates;

        public void SetWares(IInventoryInfo reference)
        {
            m_inventory.ClearList();
            AddWares(reference);
        }

        public void AddWares(IInventoryInfo reference)
        {
            TradableInventory.Item inventoryItem;
            for (int i = 0; i < reference.storedItemCount; i++)
            {
                var storedItem = reference.GetItem(i);
                m_inventory.AddItem(storedItem.data, out inventoryItem, storedItem.count);
                inventoryItem.SetCountToInfinite(storedItem.hasInfiniteCount);
                ApplyCurrentTradeRates(inventoryItem);
            }
        }

        public void SetTradeRates(TradeRates tradeRates)
        {
            m_tradeRates = tradeRates;
        }

        public void ApplyCurrentTradeRates()
        {
            var itemCount = m_inventory.storedItemCount;
            for (int i = 0; i < itemCount; i++)
            {
                ApplyCurrentTradeRates(m_inventory.GetStoredItem(i));
            }
        }

        private void ApplyCurrentTradeRates(TradableInventory.Item item)
        {
            if (m_tradeRates != null)
            {
                item.OverrideCost(m_tradeRates.buyAskingPrice.GetAskingPrice(item.data));
            }
        }

        #region ITradeInventory Implementation
        int ITradeInventory.currency => 999999;

        void ITradeInventory.AddCurrency(int value)
        {

        }

        void ITradeInventory.AddItem(ItemData item, int count)
        {

        }

        ITradeItem[] ITradeInventory.FindTradeItemsOfType(ItemCategory category) => m_inventory.FindTradeItemsOfType(category);

        ITradeItem[] ITradeInventory.GetTradableItems() => m_inventory.GetTradableItems();

        bool ITradeInventory.HasSpaceFor(ItemData item, int count) => true;

        void ITradeInventory.RemoveItem(ItemData item, int count) => m_inventory.RemoveItem(item, count);

        public ITradeItem GetTradeItem(ItemData item) => m_inventory.GetTradeItem(item);
        #endregion

    }
}
