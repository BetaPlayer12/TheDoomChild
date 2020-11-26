using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Inventories
{
    public class QuickItemElement : MonoBehaviour
    {
        [SerializeField]
        private Image m_icon;
        [SerializeField]
        private TextMeshProUGUI m_name;
        [SerializeField]
        private TextMeshProUGUI m_count;

        private ItemSlot m_currentSlot;

        public void Set(ItemSlot slot)
        {
            if (m_currentSlot != null)
            {
                if (m_count)
                {
                    m_currentSlot.CountChange -= OnCountChange;
                }
            }

            if (slot == null)
            {
                m_icon.sprite = null;
                m_icon.color = new Color(1, 1, 1, 0);
                if (m_name)
                {
                    m_name.text = "";
                }
                if (m_count)
                {
                    m_count.text = "";
                }
            }
            else
            {
                m_icon.sprite = slot.item.icon;
                m_icon.color = Color.white;
                if (m_name)
                {
                    m_name.text = slot.item.itemName;
                }
                if (m_count)
                {
                    if (slot.restrictions.hasInfiniteCount)
                    {
                        m_count.text = "\u221E"; //Infinity Symbol
                    }
                    else
                    {
                        m_count.text = slot.count.ToString();
                        m_currentSlot = slot;
                        m_currentSlot.CountChange += OnCountChange;
                    }
                }
            }
        }

        private void OnCountChange(object sender, ItemSlot.InfoChangeEventArgs eventArgs)
        {
            m_count.text = eventArgs.count.ToString();

        }
    }
}
