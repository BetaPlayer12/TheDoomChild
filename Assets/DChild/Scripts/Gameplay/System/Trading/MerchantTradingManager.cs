﻿using DChild.Gameplay.Inventories;
using Holysoft.Event;
using Holysoft.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Menu.Trading
{
    public class MerchantTradingManager : MonoBehaviour
    {
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

        private ITradableInventory m_currentMerchant;
        private ITradableInventory m_currentPlayer;

        public void UpdateCurrency()
        {
            m_playerCurrencies.UpdateUI(m_currentPlayer.soulEssence, 0);
        }

        public void UpdateInvetoryItems()
        {
            if (m_tradeOption.tradeType == TradeType.Buy)
            {
                m_initiator.Instantiate(m_currentMerchant);
            }
            else
            {
                m_initiator.Instantiate(m_currentPlayer);
            }
            m_tradeHandle.SetTradeType(m_tradeOption.tradeType);
            m_filteredTradePool.ApplyFilter(TradePoolFilter.All);
            m_initiator.GetTradableUI(0).Select();
        }

        public void SetTradingPool(ITradableInventory merchant, ITraderAskingPrice merchantAskingPrice,ITradableInventory player)
        {
            m_tradeOption.ChangeToBuyOption(true);
            m_tradeHandle.SetTraders(merchant, player);
            m_tradeHandle.SetMerchantAskingPrice(merchantAskingPrice);
            m_tradeHandle.SetTradeType(m_tradeOption.tradeType);
            m_initiator.Instantiate(merchant);
            m_filteredTradePool.ApplyFilter(TradePoolFilter.All);
            m_initiator.GetTradableUI(0).Select();
            m_currentMerchant = merchant;
            m_currentPlayer = player;
            UpdateCurrency();
        }

        public void SetTradingFilter(TradePoolFilter filter)
        {
            m_filteredTradePool.ApplyFilter(filter);
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
            m_tradeHandle.SetItemToTrade(((TradableItemUI)sender).itemSlot.item);
        }

        private void Awake()
        {
            m_initiator.OnPoolUpdate += OnSlotIntiateDone;
            m_filteredTradePool.ApplyFilter(TradePoolFilter.All);
        }
    }
}