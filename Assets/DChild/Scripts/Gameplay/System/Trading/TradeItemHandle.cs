using DChild.Gameplay.Inventories;
using DChild.Gameplay.Items;
using Holysoft.Event;
using Holysoft.UI;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Trading
{
    public class TradeItemHandle : MonoBehaviour
    {
        [SerializeField]
        private TradeItemShowcase m_showcase;
        [SerializeField]
        private Button m_tradeButton;

        private ItemData m_currentItem;
        private ITradableInventory m_merchant;
        private ITraderAskingPrice m_merchantAskingPrice;
        private ITradableInventory m_player;
        private IUIHighlight m_tradeButtonHighlight;
        private TradeType m_tradeType;

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
        }

        private void CommenceTrade(ITradableInventory buyer, ITradableInventory seller)
        {
            buyer.AddItem(m_currentItem, 1);
            buyer.AddSoulEssence(-m_currentItem.cost);
            seller.AddItem(m_currentItem, -1);
            seller.AddSoulEssence(m_currentItem.cost);
        }

        private void UpdateShowcase()
        {
            m_showcase.UpdateItemInfo(m_currentItem);
            var askingPrice = m_merchantAskingPrice.GetAskingPrice(m_currentItem, m_tradeType);
            m_showcase.UpdateItemCost(askingPrice);
            var merchantItemCount = m_merchant.GetCurrentAmount(m_currentItem);
            var playerItemCount = m_player.GetCurrentAmount(m_currentItem);
            m_showcase.UpdateItemCounts(merchantItemCount, playerItemCount);

            if (m_tradeType == TradeType.Buy)
            {
                m_tradeButton.interactable = merchantItemCount > 0 &&
                                            m_player.soulEssence >= askingPrice &&
                                            playerItemCount < m_currentItem.quantityLimit;
                if (m_tradeButton.interactable == false)
                {
                    m_tradeButtonHighlight.Normalize();
                }
                if (merchantItemCount == 0)
                {
                    ItemSoldOut?.Invoke(this, EventActionArgs.Empty);
                }
            }
            else
            {
                m_tradeButton.interactable = playerItemCount > 0;
                if (m_tradeButton.interactable == false)
                {
                    m_tradeButtonHighlight.Normalize();
                    ItemSoldOut?.Invoke(this, EventActionArgs.Empty);
                }
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