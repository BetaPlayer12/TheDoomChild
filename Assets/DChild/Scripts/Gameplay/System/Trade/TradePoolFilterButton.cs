using DChild.Gameplay.Items;
using System;
using UnityEngine;

namespace DChild.Menu.Trade
{
    public class TradePoolFilterButton : MonoBehaviour
    {
        [SerializeField]
        private TradePoolFilterHandle m_handle;
        [SerializeField]
        private ItemCategory m_filter;
        [SerializeField]
        private string m_filterName;

        public void Select()
        {
            m_handle.SetFilter(m_filter, m_filterName);
        }

        private void OnValidate()
        {
            gameObject.name = m_filterName + "TradeFilterTab";
        }
    }
}