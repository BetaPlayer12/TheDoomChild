using Holysoft.UI;
using System;
using UnityEngine;

namespace DChild.Menu.Trade
{
    public class TradePoolFilterButton : MonoBehaviour
    {
        //[SerializeField]
        //private MerchantTradingManager m_manager;
        [SerializeField]
        private TradePoolFilter m_filter;

        public void Select()
        {
            //m_manager.SetTradingFilter(m_filter);
        }

        private void OnValidate()
        {
            gameObject.name = m_filter.ToString() + "TradeFilterTab";
        }
    }
}