using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Inventories.UI
{
    public class GridInventoryListUI : InventoryListUI<IInventory>
    {
        [SerializeField, MinValue(1), PropertyOrder(-1)]
        private int m_page;
        private int m_startIndex;
        private int m_availableSlot;
        private bool m_itemSubscribed;

        public void SetPage(int pageNumber)
        {
            m_page = pageNumber;
            m_startIndex = (pageNumber - 1) * itemUICount;
            m_availableSlot = itemUICount - 1;
        }


        [Button, HideInEditorMode, PropertyOrder(-1)]
        public override void UpdateUIList()
        {
            int i = 0;
            for (; i <= m_availableSlot; i++)
            {
                var itemIndex = m_startIndex + i;
                if (itemIndex >= m_inventory.storedItemCount)
                    break;
                var storedItem = m_inventory.GetItem(itemIndex);
                if (storedItem != null)
                {
                    var itemUI = m_itemUIs[i];
                    itemUI.Show();
                    itemUI.SetReference(storedItem);
                }
            }

            for (; i < itemUICount; i++)
            {
                m_itemUIs[i].Hide();
            }
            InvokeListOverallChange();
        }

        [ContextMenu("Subscibe To Inventory Updates")]
        public void SubscribeToInventoryUpdates()
        {
            if (m_itemSubscribed == false)
            {
                m_inventory.InventoryItemUpdate += OnInvnetoryItemUpdate;
                m_itemSubscribed = true;
            }

        }

        [ContextMenu("UnSubscibe To Inventory Updates")]
        public void UnSubscribeToInventoryUpdates()
        {
            if (m_itemSubscribed)
            {
                m_inventory.InventoryItemUpdate -= OnInvnetoryItemUpdate;
                m_itemSubscribed = false;

            }
        }


        private void OnInvnetoryItemUpdate(object sender, ItemEventArgs eventArgs)
        {
            if (eventArgs.currentCount == 0 || eventArgs.countModification > 0)
            {
                UpdateUIList();
            }
            else if (eventArgs.currentCount != 0 && eventArgs.countModification < 0)
            {
                for (int i = 0; i < m_availableSlot; i++)
                {
                    if (eventArgs.data == m_itemUIs[i].reference.data)
                    {
                        m_itemUIs[i].UpdateCurrentReferenceDetails();
                    }
                }
            }
        }

        public override void Reset()
        {
            SetPage(1);
        }
    }
}