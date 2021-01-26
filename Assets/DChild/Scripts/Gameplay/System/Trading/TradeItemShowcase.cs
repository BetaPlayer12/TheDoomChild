using DChild.Gameplay.Items;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Trading
{
    public class TradeItemShowcase : MonoBehaviour
    {
        [SerializeField]
        private Image m_icon;
        [SerializeField]
        private TextMeshProUGUI m_itemName;
        [SerializeField]
        private TextMeshProUGUI m_cost;
        [SerializeField]
        private TextMeshProUGUI m_description;

        [SerializeField]
        private TextMeshProUGUI m_merchantOwned;
        [SerializeField]
        private TextMeshProUGUI m_playerOwned;


        public void UpdateItemInfo(ItemData item)
        {
            if (item == null)
            {
                m_icon.enabled = false;
                m_itemName.text = "";
                m_cost.text = "0";
                m_description.text = "";
            }
            else
            {
                m_icon.enabled = true;
                m_icon.sprite = item.icon;
                m_itemName.text = item.itemName;
                m_cost.text = item.cost.ToString();
                m_description.text = item.description;
            }
        }

        public void UpdateItemCost(int cost)
        {
            m_cost.text = cost.ToString();
        }

        public void UpdateItemCounts(int merchantOwned, int playerOwned)
        {
            m_merchantOwned.text = merchantOwned.ToString();
            m_playerOwned.text = playerOwned.ToString();
        }
    }
}