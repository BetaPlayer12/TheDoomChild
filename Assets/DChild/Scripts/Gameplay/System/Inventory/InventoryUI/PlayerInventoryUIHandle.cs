using DChild.Gameplay.Items;
using Doozy.Runtime.UIManager.Components;
using Holysoft.Event;
using Holysoft.UI;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DChild.Gameplay.Inventories.UI
{
    public class PlayerInventoryUIHandle : SerializedMonoBehaviour
    {
        [SerializeField] private ItemDetailsUI m_detailedUI;
        [SerializeField] private InventoryListUI<IInventory> m_listUI;
        [SerializeField] private QuickItemsListUI m_quickItemListUI;
        [SerializeField] private ItemUI m_firstSelectedItemUI;
        [SerializeField] private UsableInventoryItemHandle m_usableInventoryItemHandle;
        [SerializeField] private InventoryItemActionHandle m_itemActionsHandle;
        [SerializeField] private InventoryUISwapHandle m_swapHandle;
        [SerializeField] private InventoryCategoryToggleUI[] m_filterToggles;

        private Coroutine m_selectInitialSlotRoutine;

        public InventoryItemUI firstSelectedItem => m_firstSelectedItemUI as InventoryItemUI;

        public void Select(ItemUI itemUI)
        {
            var inventoryItem = itemUI as InventoryItemUI;
            if (inventoryItem == null)
                return;

            m_swapHandle.SelectForBrowse(inventoryItem, true);
        }

        public void PresentSelection(InventoryItemUI inventoryItem)
        {
            if (inventoryItem == null)
                return;

            m_detailedUI.ShowDetails(inventoryItem.reference);
            m_itemActionsHandle.ShowButtonActions(inventoryItem);

            if (inventoryItem.reference?.data?.category != ItemCategory.Consumable)
            {
                m_usableInventoryItemHandle.Hide();
                return;
            }

            m_usableInventoryItemHandle.Show();
            m_usableInventoryItemHandle.HandleUsageOfItem(inventoryItem.reference.data, inventoryItem.isQuickItem);
        }

        public void FocusAndPresent(InventoryItemUI inventoryItem)
        {
            if (inventoryItem == null)
                return;

            PresentSelection(inventoryItem);
            var toggle = inventoryItem.GetComponent<UIToggle>();
            toggle.SetIsOn(true, true, false);
            toggle.Select();
        }

        public bool TryFocusFirstAction()
        {
            if (m_usableInventoryItemHandle.TryFocusUseButton())
                return true;

            return m_itemActionsHandle.TryFocusFirstActionButton();
        }

        public bool IsActionButton(GameObject target)
        {
            return m_usableInventoryItemHandle.IsUseButton(target) ||
                m_itemActionsHandle.IsActionButton(target);
        }

        [Button]
        public void SwapItems(ItemUI itemOne, ItemUI itemTwo)
        {
            if (itemOne == null || itemTwo == null)
                return;

            if (IsEitherSlotQuickItem(itemOne, itemTwo))
            {
                m_quickItemListUI.SwapItems(itemOne, itemTwo);
                return;
            }

            m_listUI.SwapItems(itemOne, itemTwo);
        }

        public void UpdateShardIcon(ItemSprite type)
        {
        }

        public void SelectFirstSlot()
        {
            var firstItem = firstSelectedItem;
            if (firstItem == null)
                return;

            EventSystem.current?.SetSelectedGameObject(null);
            m_swapHandle.SelectForBrowse(firstItem, false);
        }

        public void SetQuickSelectionMode(bool enabled)
        {
            if (m_listUI is GridInventoryListUI gridInventory)
                gridInventory.SetQuickSelectionMode(enabled);
        }

        public bool MoveInventoryItemToQuickItems(InventoryItemUI itemUI)
        {
            if (itemUI?.reference?.data == null || itemUI.isQuickItem || m_quickItemListUI.inventory.isInventoryFull)
                return false;

            m_quickItemListUI.MoveInventoryItemToQuickItems(itemUI);
            m_listUI.inventory.RemoveItem(itemUI.reference.data, itemUI.reference.count);
            return true;
        }

        public InventoryItemUI FindFirstEmptyQuickSlot()
        {
            return m_quickItemListUI.FindFirstEmptySlot();
        }

        public InventoryItemUI FindSlot(ItemData itemData, bool isQuickItem)
        {
            if (itemData == null)
                return null;

            if (isQuickItem)
                return m_quickItemListUI.FindSlot(itemData);

            return (m_listUI as GridInventoryListUI)?.FindSlot(itemData);
        }

        public InventoryItemUI FindNearestOccupiedSlot(InventoryItemUI origin, bool isQuickItem)
        {
            if (origin == null)
                return null;

            if (isQuickItem)
                return m_quickItemListUI.FindNearestOccupiedSlot(origin);

            return (m_listUI as GridInventoryListUI)?.FindNearestOccupiedSlot(origin);
        }

        private bool IsEitherSlotQuickItem(ItemUI itemOne, ItemUI itemTwo)
        {
            return (itemOne as InventoryItemUI).isQuickItem || (itemTwo as InventoryItemUI).isQuickItem;
        }

        public void UpdateInventorySlots()
        {
            m_quickItemListUI.UpdateUIList();
            m_listUI.UpdateUIList();
        }

        private void SetupFilterToggles()
        {
            foreach (var toggle in m_filterToggles)
                toggle.UpdateToggleVisuals();
        }

        public void Initialize()
        {
            m_swapHandle.BindCancelInput();
            m_listUI.Reset();
            SetQuickSelectionMode(false);
            UpdateInventorySlots();
            SetupFilterToggles();
            SelectFirstSlot();

            if (m_selectInitialSlotRoutine != null)
                StopCoroutine(m_selectInitialSlotRoutine);
            m_selectInitialSlotRoutine = StartCoroutine(SelectInitialSlotNextFrame());
        }

        private IEnumerator SelectInitialSlotNextFrame()
        {
            yield return null;
            m_selectInitialSlotRoutine = null;
            SelectFirstSlot();
        }

        private void OnListOverallChange(object sender, EventActionArgs eventArgs)
        {
            m_detailedUI.ShowDetails(m_firstSelectedItemUI.reference);
        }

        private void OnItemCountReduced(ItemData itemData, bool isQuickItem, int remainingCount)
        {
            var previousSlot = m_swapHandle.itemOne;
            var restoreActionFocus = m_swapHandle.isActionFocused;
            UpdateInventorySlots();

            var focusItem = FindSlot(itemData, isQuickItem);
            if (focusItem == null)
                focusItem = FindNearestOccupiedSlot(previousSlot, isQuickItem);
            if (focusItem == null)
                focusItem = firstSelectedItem;

            m_swapHandle.RestoreAfterItemUse(focusItem, remainingCount > 0 && restoreActionFocus);
        }

        private void Awake()
        {
            m_listUI.ListOverallChange += OnListOverallChange;
            m_usableInventoryItemHandle.OnItemCountReduced += OnItemCountReduced;
        }

        private void OnDisable()
        {
            if (m_selectInitialSlotRoutine == null)
                return;

            StopCoroutine(m_selectInitialSlotRoutine);
            m_selectInitialSlotRoutine = null;
        }
    }
}
