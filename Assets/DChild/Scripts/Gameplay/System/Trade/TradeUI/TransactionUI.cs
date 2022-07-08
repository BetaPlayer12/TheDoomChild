using DChild.Gameplay.Inventories.UI;
using DChild.Gameplay.Items;
using DChild.Gameplay.Trade;
using Holysoft.Event;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Trade.UI
{
    public class TransactionUI : MonoBehaviour
    {
        [SerializeField]
        private ItemDetailsUI m_itemDetailsUI;
        private ITradeTransactionInfo m_transaction;
        private ItemData m_currentItemDisplayed;

        public void SetTransaction(ITradeTransactionInfo tradeTransaction)
        {
            if (m_transaction != null)
            {
                m_transaction.TransactionModified -= OnTransactionModified;
            }
            m_transaction = tradeTransaction;
            UpdateDisplay();
            tradeTransaction.TransactionModified += OnTransactionModified;
        }

        private void OnTransactionModified(object sender, EventActionArgs eventArgs)
        {
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            //if (m_currentItemDisplayed != m_transaction.item)
            //{
            //    m_currentItemDisplayed = m_transaction.item;
            //    m_itemDetailsUI.DisplayItem(m_currentItemDisplayed);
            //}
        }
    } 
}
