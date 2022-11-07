using DChild.Gameplay.Items;
using System;
using UnityEngine;

namespace DChild.Menu.Trade
{
    public class TradePoolFilterInfo : MonoBehaviour
    {
        [SerializeField]
        private ItemCategory m_filter;
        [SerializeField]
        private string m_filterName;

        public ItemCategory filter => m_filter;
        public string filterName => m_filterName;

        private void OnValidate()
        {
            gameObject.name = m_filterName + "TradeFilterTab";
        }
    }
}