using DChild.Gameplay.Inventories;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Item
{
    public class ItemContainerUI : SerializedMonoBehaviour
    {
        [SerializeField, InlineEditor]
        private IItemContainer m_itemContainer;
        [SerializeField, MinValue(1), PropertyOrder(-1)]
        private int m_page;
        private ItemSlotUI[] m_slotUI;
        private int m_slotCount;
        private int m_startIndex;
        private int m_availableSlot;

        public void SetPage(int pageNumber)
        {
            m_page = pageNumber;
            m_startIndex = (pageNumber - 1) * m_slotCount;
            if (m_itemContainer.restrictSize)
            {
                var endIndex = m_startIndex + m_slotCount;
                if (endIndex >= m_itemContainer.MaxSize)
                {
                    m_availableSlot = (m_itemContainer.MaxSize - 1) - m_startIndex;
                }
                else
                {
                    m_availableSlot = m_slotCount -1;
                }
            }
            else
            {
                m_availableSlot = m_slotCount -1 ;
            }
        }

        [Button, HideInEditorMode, PropertyOrder(-1)]
        public void UpdateUI()
        {
            int i = 0;
            for (; i <= m_availableSlot; i++)
            {
                var itemIndex = m_startIndex + i;
                var slot = m_itemContainer.GetSlot(itemIndex);
                m_slotUI[i].Show();
                if (slot == null)
                {
                    m_slotUI[i].SetInteractable(false);
                }
                else
                {
                    m_slotUI[i].SetSlot(slot);
                    m_slotUI[i].SetInteractable(true);
                }
            }

            for (; i < m_slotCount; i++)
            {
                m_slotUI[i].Hide();
            }
        }

        private void Awake()
        {
            m_slotUI = GetComponentsInChildren<ItemSlotUI>();
            m_slotCount = m_slotUI.Length;
        }
    }
}
