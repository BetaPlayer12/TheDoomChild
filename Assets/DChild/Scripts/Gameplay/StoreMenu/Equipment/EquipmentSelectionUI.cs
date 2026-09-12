using DChild.Gameplay.EquipmentSystem;
using Doozy.Runtime.UIManager.Components;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace DChild.Menu.Equipment.UI
{
    public class EquipmentSelectionUI : MonoBehaviour
    {
        [BoxGroup("MAIN UI"), SerializeField] private EquipmentUI m_equipmentUI;

        [BoxGroup("ITEM GRID"), SerializeField] private List<EquipmentGridItemUI> m_itemGrid;
        [BoxGroup("ITEM GRID"), SerializeField] private TextMeshProUGUI m_noItemsLabel;
        [BoxGroup("ITEM GRID"), SerializeField] private EquipmentEquipButtonUI m_equipButtonUI;
        public EquipmentEquipButtonUI equipButtonUI => m_equipButtonUI;

        private List<SoulEquipmentItem> m_acquiredItems;
        private SoulSlot m_slotFilter;
        private EquipmentCurrentItemUI m_currentItem;

        public void SetFilter(SoulSlot value) => m_slotFilter = value;

        public void SetupUI(List<SoulEquipmentItem> acquiredItems)
        {
            SetFilter(SoulSlot.Head);
            m_acquiredItems = acquiredItems;
        }

        public void UpdateItems(EquipmentCurrentItemUI currentItem, bool selectFirstItem = true)
        {
            DisconnectGridItems();
            m_currentItem = currentItem;

            var filteredItems = m_acquiredItems.Where(item => item.soulEquipment.Slot == m_slotFilter).ToList();
            int itemCount = Mathf.Min(filteredItems.Count, m_itemGrid.Count);
            bool hasItems = itemCount > 0;

            m_noItemsLabel.gameObject.SetActive(!hasItems);

            int i = 0;
            for (; i < itemCount; i++)
            {
                var item = filteredItems[i];

                m_itemGrid[i].ResetSelection();
                m_itemGrid[i].OnGridItemSelected += currentItem.OnGridItemSelected;
                m_equipmentUI.detailsUI.ConnectGridItem(m_itemGrid[i]);
                m_itemGrid[i].Display(item);
                m_itemGrid[i].GetEquippedStatus(currentItem);
            }

            for (; i < m_itemGrid.Count; i++)
            {
                m_itemGrid[i].ResetSelection();
                m_itemGrid[i].Display();
            }

            RefreshGridNavigation();

            if (!hasItems)
            {
                m_equipmentUI.FocusCategory(m_slotFilter);
                m_equipButtonUI.ClearSelection();
                m_equipmentUI.detailsUI.Clear();
                return;
            }

            if (selectFirstItem)
            {
                m_equipmentUI.EnterItemSelection(m_slotFilter);
                m_itemGrid[0].Select();
            }
            else
                m_itemGrid[0].PrepareAttachedItem();
        }

        public void SetItemDetails(SoulEquipmentItem equipmentItem)
        {
            m_equipmentUI.detailsUI.SetHighlightedEquipment(equipmentItem);
        }

        private void DisconnectGridItems()
        {
            foreach (EquipmentGridItemUI item in m_itemGrid)
            {
                if (m_currentItem != null)
                    item.OnGridItemSelected -= m_currentItem.OnGridItemSelected;

                m_equipmentUI.detailsUI.DisconnectGridItem(item);
            }
        }

        private void RefreshGridNavigation()
        {
            var toggleGroup = GetComponentInChildren<UIToggleGroup>();
            var gridLayout = toggleGroup?.GetComponent<GridLayoutGroup>();
            if (toggleGroup == null || gridLayout == null)
                return;

            var gridToggles = toggleGroup.GetComponentsInChildren<UIToggle>()
                .Where(toggle => toggle != toggleGroup)
                .ToList();

            foreach (UIToggle toggle in gridToggles)
            {
                var navigation = toggle.navigation;
                navigation.mode = Navigation.Mode.None;
                toggle.navigation = navigation;
            }

            var activeToggles = gridToggles
                .Where(toggle => toggle.gameObject.activeInHierarchy && toggle.interactable)
                .ToList();

            int columnCount = gridLayout.constraintCount;
            for (int index = 0; index < activeToggles.Count; index++)
            {
                int row = index / columnCount;
                int column = index % columnCount;

                var navigation = new Navigation
                {
                    mode = Navigation.Mode.Explicit,
                    selectOnLeft = GetNavigationTarget(activeToggles, index, row, column - 1, columnCount),
                    selectOnRight = GetNavigationTarget(activeToggles, index, row, column + 1, columnCount),
                    selectOnUp = GetNavigationTarget(activeToggles, index, row - 1, column, columnCount),
                    selectOnDown = GetNavigationTarget(activeToggles, index, row + 1, column, columnCount)
                };

                activeToggles[index].navigation = navigation;
            }
        }

        private UIToggle GetNavigationTarget(List<UIToggle> toggles, int currentIndex, int row, int column, int columnCount)
        {
            if (row < 0 || column < 0 || column >= columnCount)
                return toggles[currentIndex];

            int targetIndex = row * columnCount + column;
            return targetIndex < toggles.Count ? toggles[targetIndex] : toggles[currentIndex];
        }
    }
}
