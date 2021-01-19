using DChild.Gameplay.Inventories;
using DChild.Gameplay.Items;
using Holysoft.Event;
using Holysoft.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Trading
{
    public class TradeItemHandle : MonoBehaviour
    {
        [SerializeField]
        private TradeItemShowcase m_showcase;
        [SerializeField]
        private ConfirmationHandler m_confirmation;
        [SerializeField]
        private Button m_tradeButton;

        private int m_itemToTransferCount;
        private int m_maxItemToTransferCount;

        private ItemData m_currentItem;
        private ITradableInventory m_merchant;
        private ITraderAskingPrice m_merchantAskingPrice;
        private ITradableInventory m_player;
        private IUIHighlight m_tradeButtonHighlight;
        private TradeType m_tradeType;

        public event EventAction<EventActionArgs> ItemSold;
        public event EventAction<EventActionArgs> ItemSoldOut;



        public void SetTraders(ITradableInventory merchant, ITradableInventory player)
        {
            m_merchant = merchant;
            m_player = player;

            if (m_currentItem != null && m_merchantAskingPrice != null)
            {
                UpdateShowcase();
            }
        }

        public void SetMerchantAskingPrice(ITraderAskingPrice askingPrice)
        {
            m_merchantAskingPrice = askingPrice;
            if (m_currentItem != null && m_merchant != null && m_player != null)
            {
                UpdateShowcase();
            }
        }

        public void SetTradeType(TradeType tradeType)
        {
            m_tradeType = tradeType;
        }

        public void SetItemToTrade(ItemData item)
        {
            m_currentItem = item;
            if (m_merchant != null && m_player != null)
            {
                UpdateShowcase();
            }

            m_itemToTransferCount = 0;
            UpdateMaxItemToTransferCount();
            EnableTradeButton(false);
        }

        public void AddItemToTransferCount()
        {
            if (m_itemToTransferCount < m_maxItemToTransferCount)
            {
                m_itemToTransferCount++;
                UpdatePredictedItemCountShowcase();
                EnableTradeButton(true);
            }
        }

        public void RemoveItemToTransferCount()
        {
            if (m_itemToTransferCount > 0)
            {
                m_itemToTransferCount--;
                UpdatePredictedItemCountShowcase();
                if (m_itemToTransferCount == 0)
                {
                    EnableTradeButton(false);
                }
            }
        }

        public void RequestTrade()
        {
            var tradeWord = m_tradeType == TradeType.Buy ? "buy" : "sell";
            m_confirmation.RequestConfirmation(OnTradeConfirm, $"Are you sure you want to {tradeWord}\n" +
                                                                $"<color=#804C1A>{m_itemToTransferCount}</color> {m_currentItem.itemName} for {m_currentItem.cost * m_itemToTransferCount} " +
                                                                $"<font=\"BACKCOUNTRY-Regular SDF\"><b>S.E.</b></font>?");
        }

        private void OnTradeConfirm(object sender, EventActionArgs eventArgs)
        {
            CommenceTrade();
            m_itemToTransferCount = 0;
            EnableTradeButton(false);
            UpdateMaxItemToTransferCount();
        }

        public void CommenceTrade()
        {
            if (m_tradeType == TradeType.Buy)
            {
                CommenceTrade(m_player, m_merchant);
            }
            else
            {
                CommenceTrade(m_merchant, m_player);
            }
            UpdateShowcase();
            ItemSold?.Invoke(this, EventActionArgs.Empty);
        }

        private void UpdatePredictedItemCountShowcase()
        {
            if (m_tradeType == TradeType.Buy)
            {
                m_showcase.UpdateItemCounts(m_merchant.GetCurrentAmount(m_currentItem) - m_itemToTransferCount, m_player.GetCurrentAmount(m_currentItem) + m_itemToTransferCount);
            }
            else
            {
                m_showcase.UpdateItemCounts(m_merchant.GetCurrentAmount(m_currentItem) + m_itemToTransferCount, m_player.GetCurrentAmount(m_currentItem) - m_itemToTransferCount);
            }
        }

        private void EnableTradeButton(bool value)
        {
            m_tradeButton.interactable = value;
            if (value == false)
            {
                m_tradeButtonHighlight.Normalize();
            }
        }

        private void UpdateMaxItemToTransferCount()
        {
            if (m_currentItem != null)
            {
                if (m_tradeType == TradeType.Buy)
                {
                    var spaceLeftInPlayerInventory = m_currentItem.quantityLimit - m_player.GetCurrentAmount(m_currentItem);
                    var itemCountFromMerchant = m_merchant.GetCurrentAmount(m_currentItem);
                    var itemCountPlayerCanAfford = m_player.soulEssence / m_currentItem.cost;
                    m_maxItemToTransferCount = Mathf.Min(itemCountFromMerchant, spaceLeftInPlayerInventory, itemCountPlayerCanAfford);
                }
                else
                {
                    m_maxItemToTransferCount = m_player.GetCurrentAmount(m_currentItem);
                }
            }
            else
            {
                m_maxItemToTransferCount = 0;
            }
        }

        private void CommenceTrade(ITradableInventory buyer, ITradableInventory seller)
        {
            var currentAmount = m_itemToTransferCount * m_currentItem.cost;
            buyer.AddItem(m_currentItem, m_itemToTransferCount);
            buyer.AddSoulEssence(-currentAmount);
            seller.AddItem(m_currentItem, -m_itemToTransferCount);
            seller.AddSoulEssence(currentAmount);
            UpdateMaxItemToTransferCount();
        }

        private void UpdateShowcase()
        {
            if (m_currentItem == null)
            {
                m_showcase.UpdateItemInfo(null);
                m_showcase.UpdateItemCost(0);
                m_showcase.UpdateItemCounts(0, 0);
                m_tradeButton.interactable = false;
                m_tradeButtonHighlight.Normalize();
            }
            else
            {
                m_showcase.UpdateItemInfo(m_currentItem);
                var askingPrice = m_merchantAskingPrice.GetAskingPrice(m_currentItem, m_tradeType);
                m_showcase.UpdateItemCost(askingPrice);
                var merchantItemCount = m_merchant.GetCurrentAmount(m_currentItem);
                var playerItemCount = m_player.GetCurrentAmount(m_currentItem);
                m_showcase.UpdateItemCounts(merchantItemCount, playerItemCount);

                //if (m_tradeType == TradeType.Buy)
                //{
                //    m_tradeButton.interactable = merchantItemCount > 0 &&
                //                                m_player.soulEssence >= askingPrice &&
                //                                playerItemCount < m_currentItem.quantityLimit;
                //    if (m_tradeButton.interactable == false)
                //    {
                //        m_tradeButtonHighlight.Normalize();
                //    }
                //    if (merchantItemCount == 0)
                //    {
                //        ItemSoldOut?.Invoke(this, EventActionArgs.Empty);
                //    }
                //}
                //else
                //{
                //    m_tradeButton.interactable = playerItemCount > 0;
                //    if (m_tradeButton.interactable == false)
                //    {
                //        m_tradeButtonHighlight.Normalize();
                //        ItemSoldOut?.Invoke(this, EventActionArgs.Empty);
                //    }
                //}
            }
        }

        private void UpdateItemCount()
        {
            m_showcase.UpdateItemCounts(m_merchant.GetCurrentAmount(m_currentItem), m_player.GetCurrentAmount(m_currentItem));
        }

        private void Awake()
        {
            m_tradeButtonHighlight = m_tradeButton.GetComponent<IUIHighlight>();
        }
    }
}