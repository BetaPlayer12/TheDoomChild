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

        public override void Reset()
        {
            SetPage(1);
        }
    }
}