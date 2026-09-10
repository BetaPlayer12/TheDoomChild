using DChild.Gameplay.Inventories;
using DChild.Gameplay.Inventories.UI;
using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay.Trade.UI
{
    public class GridTradeInventoryListUI : FilteredInventoryListUI<ITradeInventory>
    {
        [SerializeField, MinValue(1), PropertyOrder(-1)]
        private int m_page;
        private int m_startIndex = 0;
        private int m_availableSlot;

        [SerializeField] private UIScrollbar m_gridScroll;
        [SerializeField, MinValue(1)] private int m_columnCount = 6;
        private int m_currentRowIndex;
        private int m_totalRowPositions;

        private ITradeItem[] m_filteredItemList;

        #region Scrollbar Methods
        [Button]
        public override void SetupScrollUI()
        {
            SetupScroll(m_filteredItemList);
        }
        public void SetupScroll(ITradeItem[] tradeItems, int toggleCount = 24)
        {
            bool wasVisible = m_gridScroll.gameObject.activeSelf;
            m_currentRowIndex = -1;
            int visibleRows = Mathf.CeilToInt(toggleCount / (float)m_columnCount);
            int totalRows = Mathf.CeilToInt(tradeItems.Length / (float)m_columnCount);
            m_totalRowPositions = Mathf.Max(1, totalRows - visibleRows + 1);
            bool shouldShow = m_totalRowPositions > 1;

            m_gridScroll.numberOfSteps = m_totalRowPositions;
            m_gridScroll.size = totalRows > 0 ? Mathf.Clamp01(visibleRows / (float)totalRows) : 1f;
            if (shouldShow && !wasVisible)
                ResetScrollPosition();
            m_gridScroll.gameObject.SetActive(shouldShow);

        }
        public void HandleScroll()
        {
            int updatedRow = Mathf.RoundToInt(m_gridScroll.value * (m_totalRowPositions - 1));

            if (m_currentRowIndex != updatedRow)
            {
                m_currentRowIndex = updatedRow;
                SetPage(m_currentRowIndex);
                UpdateUIList();
            }
        }

        public void SetPage(int rowIndex)
        {
            m_page = rowIndex;
            m_startIndex = rowIndex * m_columnCount;

            m_availableSlot = itemUICount;
        }
        #endregion

        #region UpdateUIList Overloading
        [Button, HideInEditorMode, PropertyOrder(-1)]
        public override void UpdateUIList()
        {
            int i = 0;

            if (m_currentFilter == Items.ItemCategory.All)
                m_filteredItemList = m_inventory.GetTradableItems();

            else
                m_filteredItemList = m_inventory.FindTradeItemsOfType(m_currentFilter);

            SetupScrollUI();
            UpdateUIList(ref i, m_filteredItemList);

            for (; i < itemUICount; i++)
            {
                //m_itemUIs[i].Hide();
                m_itemUIs[i].gameObject.SetActive(false);
            }

            InvokeListOverallChange();
        }

        private void UpdateUIList(ref int i, ITradeItem[] tradableItems)
        {
            for (int slotIndex = 0; slotIndex < itemUICount; slotIndex++)
            {
                int itemDataIndex = m_startIndex + slotIndex;

                if (itemDataIndex >= tradableItems.Length)
                {
                    break;
                }

                var storedItem = tradableItems[itemDataIndex];
                if (storedItem != null)
                {
                    var itemUI = m_itemUIs[slotIndex];
                    itemUI.gameObject.SetActive(true);
                    //itemUI.Show();
                    itemUI.SetReference(storedItem);

                    i = slotIndex + 1;
                }
            }
        }
        #endregion
        public override void SwapItems(ItemUI itemOne, ItemUI itemTwo)
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateUIList(bool v)
        {
            throw new System.NotImplementedException();
        }

        public override void Reset()
        {
            ResetScrollPosition();
            m_filteredItemList = null;
        }

        private void ResetScrollPosition()
        {
            m_currentRowIndex = 0;
            SetPage(0);
            m_gridScroll.SetValueWithoutNotify(0f);
        }

        private void OnEnable() => ResetScrollPosition();

     
    }

}
