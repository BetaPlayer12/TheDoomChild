using DChild.Gameplay.Items;
using Doozy.Runtime.UIManager;
using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Inventories.UI
{
    public class GridInventoryListUI : FilteredInventoryListUI<IInventory>
    {
        [SerializeField, MinValue(1), PropertyOrder(-1)]
        private int m_page;
        private int m_startIndex;
        private int m_availableSlot;

        private bool m_isQuickSlotSelected;

        [SerializeField] private Sprite m_notSelectableBG;
        [SerializeField] private Sprite m_currentBG;

        public void SetPage(int pageNumber)
        {
            m_page = pageNumber;
            m_startIndex = (pageNumber - 1) * itemUICount;
            m_availableSlot = itemUICount - 1;
        }

        public override void SwapItems(ItemUI itemOne, ItemUI itemTwo)
        {
            if (itemOne == null || itemTwo == null) return;

            m_inventory.SwapItems(itemOne.reference.data, itemTwo.reference.data);
        }


        [Button, HideInEditorMode, PropertyOrder(-1)]
        public override void UpdateUIList()
        {
            int i = 0;

            UpdateUIList(ref i, m_inventory.FindStoredItemsOfType(m_currentFilter), m_isQuickSlotSelected);

            for (; i < itemUICount; i++)
            {
                m_itemUIs[i].Hide();
            }
            InvokeListOverallChange();
        }

        public override void UpdateUIList(bool quickItemSelected = false)
        {
            m_isQuickSlotSelected = quickItemSelected;
            UpdateUIList();
        }

        public void SetQuickSelectionMode(bool enabled)
        {
            m_isQuickSlotSelected = enabled;
        }

        private void UpdateUIList(ref int i, IStoredItem[] items, bool quickItemSelected)
        {
            for (; i <= m_availableSlot; i++)
            {
                int itemIndex = m_startIndex + i;

                // Guard: Index out of bounds
                if (itemIndex >= items.Length) break;

                var storedItem = items[itemIndex];

                // Guard: Null item
                if (storedItem == null) continue;

                UpdateItemSlot(m_itemUIs[i], storedItem, quickItemSelected);
            }
        }
        private void UpdateItemSlot(ItemUI itemUI, IStoredItem storedItem, bool quickItemSelected)
        {
            itemUI.Show();
            itemUI.SetReference(storedItem);
            itemUI.SetIconColor(false);
            itemUI.SetItemFrame(m_currentBG);

            var toggle = itemUI.GetComponent<UIToggle>();

            // Reset state only if it's not in a persistent state (Selected/Disabled)
            if (toggle.currentUISelectionState != UISelectionState.Selected &&
                toggle.currentUISelectionState != UISelectionState.Disabled)
            {
                toggle.SetState(UISelectionState.Normal);
            }

            if (quickItemSelected)
            {
                ApplyQuickSelectionRestrictions(itemUI, toggle);
            }
        }

        public void ApplyQuickSelectionRestrictions(ItemUI itemUI, UIToggle toggle)
        {
            bool isRestricted = !InventoryUISwapHandle.CanAssignToQuickItems(itemUI as InventoryItemUI);

            if (isRestricted)
            {
                toggle.interactable = false;
                toggle.SetState(UISelectionState.Normal);
                itemUI.SetIconColor(true);
                itemUI.SetItemFrame(m_notSelectableBG);
            }
        }

        public override void Reset()
        {
            m_isQuickSlotSelected = false;
            SetPage(1);
        }

        public override void SetupScrollUI()
        {
            throw new System.NotImplementedException();
        }
    }
}
