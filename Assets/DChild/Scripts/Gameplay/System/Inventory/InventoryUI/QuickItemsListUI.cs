using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Inventories.UI
{
    public class QuickItemsListUI : InventoryListUI<QuickItemInventory>
    {
        [SerializeField] private List<InventoryItemUI> m_itemSlots;

        public override void UpdateUIList()
        {
            for (int i = 0; i < m_itemSlots.Count; i++)
                m_itemSlots[i].SetReference(m_inventory.GetItem(i));
        }

        public override void SwapItems(ItemUI itemOne, ItemUI itemTwo)
        {
            m_inventory.SwapItems(itemOne.reference.data, itemTwo.reference.data);
        }

        public void MoveInventoryItemToQuickItems(InventoryItemUI itemUI)
        {
            m_inventory.AddItem(itemUI.reference.data, itemUI.reference.count);
        }

        public InventoryItemUI FindSlot(Items.ItemData itemData)
        {
            for (int i = 0; i < m_itemSlots.Count; i++)
            {
                if (m_itemSlots[i].reference?.data == itemData)
                    return m_itemSlots[i];
            }

            return null;
        }

        public InventoryItemUI FindFirstEmptySlot()
        {
            for (int i = 0; i < m_itemSlots.Count; i++)
            {
                if (m_itemSlots[i].reference == null)
                    return m_itemSlots[i];
            }

            return null;
        }

        public InventoryItemUI FindNearestOccupiedSlot(InventoryItemUI origin)
        {
            var originIndex = m_itemSlots.IndexOf(origin);
            if (originIndex < 0)
                return null;

            for (int i = originIndex; i < m_itemSlots.Count; i++)
            {
                if (m_itemSlots[i].reference != null)
                    return m_itemSlots[i];
            }

            for (int i = originIndex - 1; i >= 0; i--)
            {
                if (m_itemSlots[i].reference != null)
                    return m_itemSlots[i];
            }

            return origin;
        }

        public void RemoveQuickItem(ItemUI itemUI)
        {
            m_inventory.RemoveItem(itemUI.reference.data, itemUI.reference.count);
        }

        public override void Reset()
        {
            throw new System.NotImplementedException();
        }

        public override void UpdateUIList(bool v)
        {
            throw new System.NotImplementedException();
        }

        public override void SetupScrollUI()
        {
            throw new System.NotImplementedException();
        }

        //private void Awake()
        //{
        //    m_inventory = GameplaySystem.playerManager.player.inventory.quickItemInventory;
        //}
    }
}
