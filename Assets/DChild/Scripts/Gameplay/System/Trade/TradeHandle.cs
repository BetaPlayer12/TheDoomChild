using DChild.Gameplay.Inventories;
using DChild.Menu.Trade;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Trade
{
    public class TradeHandle : SerializedMonoBehaviour
    {
        [SerializeField, BoxGroup("Trade Transaction"), HideLabel]
        private TradeTransaction m_transaction;
        [ShowInInspector, ReadOnly]
        private ITradeInventory m_buyer;
        [ShowInInspector, ReadOnly]
        private ITradeInventory m_seller;

        private TradeAskingPrice m_currentSellingAskingPrice;

        private ITradeItem m_currentItemBeingTraded;

        public ITradeItem currentItemBeingTraded => m_currentItemBeingTraded;
        public ITradeTransactionInfo transactionInfo => m_transaction;
        public ITradeInventory currentSeller => m_seller;

        public void SetItemToTrade(ITradeItem item)
        {
            m_currentItemBeingTraded = item;
            m_transaction.SetTransaction(m_currentItemBeingTraded.data, m_currentItemBeingTraded.cost, 1);
        }

        public void IncreaseItemCount()
        {
            var sellerHasEnoughItem = m_currentItemBeingTraded.count >= m_transaction.count + 1;
            var buyerHasEnoughMoney = m_buyer.currency >= m_transaction.totalCost + m_currentItemBeingTraded.cost;
            var buyerCanStoreItem = m_buyer.HasSpaceFor(m_transaction.item, m_transaction.count + 1);
            if (sellerHasEnoughItem && buyerHasEnoughMoney && buyerCanStoreItem)
            {
                m_transaction.IncreaseItemCount(1);
            }
        }

        public void DecreaseItemCount()
        {
            if (m_transaction.count > 1)
            {
                m_transaction.IncreaseItemCount(-1);
            }
        }

        public ITradeItem[] GetSellerInventory() => m_seller.GetTradableItems();

        public void SwapRoles()
        {
            var temp = m_seller;
            m_seller = m_buyer;
            m_buyer = temp;
        }

        public void CommenceTrade()
        {
            CommenceTransaction(m_seller, m_buyer);
        }

        public void SetTraders(ITradeInventory buyerInventory, ITradeInventory sellerInventory)
        {
            m_buyer = buyerInventory;
            m_seller = sellerInventory;
            UpdateBuyerSellingPrice();
            m_transaction.SetTransaction(null, 0, 0);
        }

        public void SetSellingTradeRate(TradeAskingPrice sellingAskingPrice)
        {
            m_currentSellingAskingPrice = sellingAskingPrice;
        }

        private void UpdateBuyerSellingPrice()
        {
            var tradableItems = m_buyer.GetTradableItems();
            if (m_currentSellingAskingPrice == null)
            {
                for (int i = 0; i < tradableItems.Length; i++)
                {
                    tradableItems[i].RemoveCostOverride();
                }
            }
            else
            {
                for (int i = 0; i < tradableItems.Length; i++)
                {
                    var item = tradableItems[i];
                    item.OverrideCost(m_currentSellingAskingPrice.GetAskingPrice(item.data));
                }
            }
        }

        private void CommenceTransaction(ITradeInventory from, ITradeInventory to)
        {
            from.AddCurrency(m_transaction.totalCost);
            from.RemoveItem(m_transaction.item, m_transaction.count);
            to.AddCurrency(-m_transaction.totalCost);
            to.AddItem(m_transaction.item, m_transaction.count);
        }

        private void Awake()
        {
            m_transaction = new TradeTransaction();
        }
    }

}