using DChild.Gameplay.Inventories;
using Holysoft.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Trading
{
    public class TradableItemUI : MonoBehaviour
    {
        [SerializeField]
        private Image m_icon;
        [SerializeField]
        private TextMeshProUGUI m_countText;

        private ItemSlot m_itemSlot;

        public event EventAction<EventActionArgs> OnSelected;
        public ItemSlot itemSlot => m_itemSlot;

        public void Set(ItemSlot item)
        {
            gameObject.SetActive(item != null);
            if (m_itemSlot != item)
            {
                if (m_itemSlot != null)
                {
                    m_itemSlot.CountChange -= OnCountChange;
                }
                m_itemSlot = item;

                var data = m_itemSlot.item;
                m_icon.sprite = data.icon;
                var itemCount = m_itemSlot.count;
                m_countText.text = itemCount > 1 ? itemCount.ToString() : "";
                m_itemSlot.CountChange += OnCountChange;
            }
        }

        public void Select()
        {
            OnSelected?.Invoke(this, EventActionArgs.Empty);
        }

        private void OnCountChange(object sender, ItemSlot.InfoChangeEventArgs eventArgs)
        {
            var itemCount = m_itemSlot.count;
            if (itemCount > 0)
            {
                gameObject.SetActive(true);
                m_countText.text = itemCount > 1 ? m_itemSlot.count.ToString() : "";
            }
            else
            {
                gameObject.SetActive(false);
                m_itemSlot.CountChange -= OnCountChange;
            }
        }

        private void OnDestroy()
        {
            if (m_itemSlot != null)
            {
                m_itemSlot.CountChange -= OnCountChange;
            }
        }
    }
}