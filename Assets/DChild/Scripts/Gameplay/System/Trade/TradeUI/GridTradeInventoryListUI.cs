using DChild.Gameplay.Inventories.UI;
using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Trade.UI
{
    public class GridTradeInventoryListUI : InventoryListUI<ITradeInventory>
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
            var tradableItems = m_inventory.GetTradableItems();
            for (; i <= m_availableSlot; i++)
            {
                var itemIndex = m_startIndex + i;
                if (itemIndex >= tradableItems.Length)
                    break;
                var storedItem = tradableItems[itemIndex];
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