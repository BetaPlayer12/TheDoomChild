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
        private TextMeshProUGUI m_count;

        private ItemSlot m_currentSlot;

        public void Set(ItemSlot slot)
        {
            if (m_currentSlot != null)
            {
                m_currentSlot.CountChange -= OnCountChange;
            }

            m_icon.sprite = slot.item.icon;
            m_count.text = slot.count.ToString();
            m_currentSlot = slot;
            m_currentSlot.CountChange += OnCountChange;
        }

        private void OnCountChange(object sender, ItemSlot.InfoChangeEventArgs eventArgs)
        {
            m_count.text = eventArgs.count.ToString();
        }
    }
}
