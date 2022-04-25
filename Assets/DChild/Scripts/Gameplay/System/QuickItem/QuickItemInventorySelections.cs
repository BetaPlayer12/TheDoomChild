using System.Collections.Generic;
using System.Linq;
using DChild.Gameplay.Items;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Inventories
{
    public class QuickItemInventorySelections : QuickItemSelections
    {
        [SerializeField]
        private IInventory m_inventory;

        private IReadOnlyCollection<IStoredItem> m_currentList;

        public override int itemCount => m_currentList.Count;

        public override int FindIndexOf(IStoredItem item)
        {
            for (int i = 0; i < m_currentList.Count; i++)
            {
                if (m_currentList.ElementAt(i) == item)
                {
                    return i;
                }
            }

            return -1;
        }

        public override IStoredItem GetItem(int index)
        {
            return m_currentList.ElementAt(index);
        }
        public override bool IsInSelections(ItemData data)
        {
            for (int i = 0; i < m_currentList.Count; i++)
            {
                if (m_currentList.ElementAt(i).data == data)
                {
                    return true;
                }
            }
            return false;
        }

        public override void UpdateSelection()
        {
            m_currentList = m_inventory.FindStoredItemsOfType(ItemCategory.QuickItem);
        }

        private void OnMassInventoryUpdate(object sender, EventActionArgs eventArgs)
        {
            UpdateSelection();
            InvokeSelectionUpdate();
        }
        private void OnInventoryItemUpdate(object sender, ItemEventArgs eventArgs)
        {
            if (IsAValidQuickItem(eventArgs.data))
            {
                if (eventArgs.currentCount == 0)
                {
                    UpdateSelection();
                    InvokeSelectionUpdate();
                }
                else if (eventArgs.currentCount > 0 && IsInSelections(eventArgs.data))
                {
                    UpdateSelection();
                    InvokeSelectionUpdate();
                }
                else
                {
                    InvokeSelectionDetailsUpdate();
                }
            }
        }

        private bool IsAValidQuickItem(ItemData data) => data.category == ItemCategory.Consumable || data.category == ItemCategory.Throwable;


        private void Awake()
        {
            m_inventory.MassInventoryItemUpdate += OnMassInventoryUpdate;
            m_inventory.InventoryItemUpdate += OnInventoryItemUpdate;
            UpdateSelection();
            InvokeSelectionUpdate();
        }

    }
}
