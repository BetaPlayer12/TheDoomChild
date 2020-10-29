using DChild.Gameplay.Inventories;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.UI
{
    public class TradableItemUI : MonoBehaviour
    {
        [SerializeField]
        private Image m_icon;
        [SerializeField]
        private TextMeshProUGUI m_labelText;
        [SerializeField]
        private TextMeshProUGUI m_countText;
        [SerializeField]
        private TextMeshProUGUI m_costText;

        private ItemSlot m_itemSlot;

        public void Set(ItemSlot item)
        {
            if (m_itemSlot != item)
            {
                if (m_itemSlot != null)
                {
                    m_itemSlot.CountChange -= OnCountChange;
                }
                m_itemSlot = item;

                var data = m_itemSlot.item;
                m_icon.sprite = data.icon;
                m_labelText.text = data.itemName;
                m_countText.text = m_itemSlot.count.ToString();
                m_costText.text = data.cost.ToString();

                m_itemSlot.CountChange += OnCountChange;
            }
        }

        private void OnCountChange(object sender, ItemSlot.InfoChangeEventArgs eventArgs)
        {
            m_countText.text = m_itemSlot.count.ToString();
        }
    }
}