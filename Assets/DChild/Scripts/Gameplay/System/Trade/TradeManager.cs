using DChild.Gameplay.Characters.NPC;
using DChild.Gameplay.Inventories.UI;
using DChild.Gameplay.Inventories;
using DChild.Gameplay.Trade.UI;
using DChild.Menu;
using DChild.Menu.Trade;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Trade
{
    public class TradeManager : SerializedMonoBehaviour
    {
        [SerializeField]
        private TradeHandle m_tradeHandle;
        [SerializeField]
        private TradeOptionHandle m_tradeOption;
        [SerializeField]
        private TransactionDetailsUI m_transactionDetails;
        [SerializeField]
        private TradePlayerCurrencies m_playerCurrencies;
        [SerializeField]
        private InventoryListUI<ITradeInventory> m_listUI;
        [SerializeField]
        private ItemUI m_firstSelectedItemUI;
        [SerializeField]
        private ItemDetailsUI m_itemBeingTradedUI;
        [SerializeField]
        private NPCProfileUI m_sellerProfile;
        [SerializeField]
        private Image m_highlight;

        [SerializeField]
        private ConfirmationHandler m_tradeConfirmation;

        public void SetupTrade(ITradeInventory buyer, ITradeInventory seller)
        {
            m_tradeHandle.SetTraders(buyer, seller);
            m_tradeOption.ChangeToBuyOption(true);
            InitializeTradeUI();
            UpdateCurrencyUI();
        }

        public void SetSellerProfile(NPCProfile profile)
        {
            m_sellerProfile.Set(profile);
        }

        public void SetSellingTradeRates(TradeAskingPrice sellingPriceRate)
        {
            m_tradeHandle.SetSellingTradeRate(sellingPriceRate);
        }

        public void Select(ItemUI item)
        {
            m_itemBeingTradedUI.ShowDetails(item.reference);
            m_tradeHandle.SetItemToTrade((ITradeItem)item.reference);
            m_highlight.enabled = true;
            m_highlight.rectTransform.position = item.transform.position;
        }

        public void InitializeTradeUI()
        {
            m_listUI.Reset();
            m_listUI.SetInventoryReference(m_tradeHandle.currentSeller);
            Select(m_firstSelectedItemUI);
        }

        public void RequestConfirmTrade()
        {
            var transaction = m_tradeHandle.transactionInfo;
            var pluralization = transaction.count > 1 ? "s " : " ";
            var message = $"Would you like to Trade {transaction.count} {transaction.item.itemName}{pluralization}for S.E {transaction.totalCost}";
            m_tradeConfirmation.RequestConfirmation(OnTradeConfirmed, message);
        }

        private void OnTradeConfirmed(object sender, EventActionArgs eventArgs)
        {
            m_tradeHandle.CommenceTrade();
            if (m_tradeHandle.currentItemBeingTraded.count == 0)
            {
                Select(m_firstSelectedItemUI);
            }
            else
            {
                m_tradeHandle.SetItemToTrade(m_tradeHandle.currentItemBeingTraded);
            }
            UpdateCurrencyUI();
        }

        private void UpdateCurrencyUI()
        {
           m_playerCurrencies.UpdateUI(GameplaySystem.playerManager.player.inventory.currency, 0);
        }
        private void Awake()
        {
            m_transactionDetails.SetTransactionReference(m_tradeHandle.transactionInfo);
        }

    }

}