using Doozy.Runtime.UIManager.Components;
using UnityEngine;

namespace DChild.Gameplay.Inventories.UI
{
    public class InventoryItemActionHandle : MonoBehaviour
    {
        [SerializeField] private UIButton m_swapButton;
        [SerializeField] private UIButton m_removeItemButton;

        public void ShowButtonActions(InventoryItemUI inventoryitemUI)
        {
            if (inventoryitemUI.reference == null)
            {
                Reset();
                return;
            }

            var itemCategory = inventoryitemUI.reference.data.category;

            var isSwappable = inventoryitemUI.isQuickItem &&
                (itemCategory == Items.ItemCategory.Consumable ||
                 itemCategory == Items.ItemCategory.Throwable);

            m_swapButton.gameObject.SetActive(isSwappable);
            m_removeItemButton.gameObject.SetActive(inventoryitemUI.isQuickItem);
        }

        public bool TryFocusFirstActionButton()
        {
            if (TryFocus(m_swapButton))
                return true;

            return TryFocus(m_removeItemButton);
        }

        public bool IsActionButton(GameObject target)
        {
            return target != null &&
                (target == m_swapButton.gameObject || target == m_removeItemButton.gameObject);
        }

        private bool TryFocus(UIButton button)
        {
            if (!button.gameObject.activeInHierarchy || !button.interactable)
                return false;

            button.Select();
            return true;
        }

        private void Reset()
        {
            m_swapButton.gameObject.SetActive(false);
            m_removeItemButton.gameObject.SetActive(false);
        }
    }
}
