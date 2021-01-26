using DChild.Gameplay.Characters.NPC;
using DChild.Gameplay.Inventories;
using Holysoft.Event;
using Holysoft.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Trading
{
    public class MerchantTradingManager : MonoBehaviour
    {
        [SerializeField]
        private NPCProfileUI m_traderProfile;
        [SerializeField]
        private FilteredTradePool m_filteredTradePool;
        [SerializeField]
        private TradePoolIntantiator m_initiator;
        [SerializeField]
        private TradeItemHandle m_tradeHandle;
        [SerializeField]
        private TradePlayerCurrencies m_playerCurrencies;
        [SerializeField]
        private TradeOptionHandle m_tradeOption;
        [SerializeField]
        private Image m_highlight;

        private ITradableInventory m_currentMerchant;
        private ITradableInventory m_currentPlayer;

        public void UpdateInvetoryItems()
        {
            UpdateTradingPool();
            m_initiator.GetTradableUI(0).Select();
        }



        public void SetProfile(NPCProfile profile)
        {
            m_traderProfile.Set(profile);
        }

        public void SetTradingPool(ITradableInventory merchant, ITraderAskingPrice merchantAskingPrice, ITradableInventory player)
        {
            m_tradeOption.ChangeToBuyOption(true);
            m_tradeHandle.SetTraders(merchant, player);
            m_tradeHandle.SetMerchantAskingPrice(merchantAskingPrice);
            m_tradeHandle.SetTradeType(m_tradeOption.tradeType);
            m_initiator.Instantiate(merchant, TradePoolFilter.All);
            m_filteredTradePool.ApplyFilter(TradePoolFilter.All);
            m_initiator.GetTradableUI(0).Select();
            m_currentMerchant = merchant;
            m_currentPlayer = player;
            m_tradeHandle.SetItemToTrade(null);
            m_highlight.enabled = false;
            UpdateCurrency();
        }

        public void SetTradingFilter(TradePoolFilter filter)
        {
            m_initiator.Instantiate(m_currentMerchant, filter);
            m_filteredTradePool.ApplyFilter(filter);
        }

        private void UpdateCurrency()
        {
            m_playerCurrencies.UpdateUI(m_currentPlayer.soulEssence, 0);
        }

        private void UpdateTradingPool()
        {
            if (m_tradeOption.tradeType == TradeType.Buy)
            {
                m_initiator.Instantiate(m_currentMerchant, TradePoolFilter.All);
            }
            else
            {
                m_initiator.Instantiate(m_currentPlayer, TradePoolFilter.All);
            }
            m_tradeHandle.SetTradeType(m_tradeOption.tradeType);
            m_filteredTradePool.ApplyFilter(TradePoolFilter.All);
        }

        private void OnSlotIntiateDone(object sender, EventActionArgs eventArgs)
        {
            var list = m_initiator.instantiatedSlots;
            for (int i = 0; i < list.Count; i++)
            {
                list[i].OnSelected -= OnItemSelected; //To Ensure there wont be double subscribtion
                list[i].OnSelected += OnItemSelected;
            }
        }

        private void OnItemSelected(object sender, EventActionArgs eventArgs)
        {
            var uiSlot = ((TradableItemUI)sender);
            m_tradeHandle.SetItemToTrade(uiSlot.itemSlot.item);
            m_highlight.enabled = true;
            m_highlight.rectTransform.position = uiSlot.transform.position;
        }

        private void OnItemSold(object sender, EventActionArgs eventArgs)
        {
            UpdateCurrency();
        }

        private void OnItemSoldOut(object sender, EventActionArgs eventArgs)
        {
            UpdateTradingPool();
            m_highlight.enabled = false;
        }


        private void Awake()
        {
            m_initiator.OnPoolUpdate += OnSlotIntiateDone;
            m_filteredTradePool.ApplyFilter(TradePoolFilter.All);
            m_tradeHandle.ItemSold += OnItemSold;
            m_tradeHandle.ItemSoldOut += OnItemSoldOut;
        }
    }
}