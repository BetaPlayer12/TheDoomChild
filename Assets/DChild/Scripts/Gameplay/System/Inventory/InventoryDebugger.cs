using DChild.Gameplay.Items;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Inventories
{
    public class InventoryDebugger : SerializedMonoBehaviour
    {
        [SerializeField]
        private IInventory inventory;

        [SerializeField]
        private ItemData m_item;
        [SerializeField]
        private int m_count;

        [Button, HorizontalGroup]
        public void Remove()
        {
            inventory.RemoveItem(m_item, m_count);
        }

        [Button, HorizontalGroup]
        public void Set()
        {
            inventory.SetItem(m_item, m_count);
        }

        [Button,HorizontalGroup]
        public void Add()
        {
            inventory.AddItem(m_item, m_count);
        }
    }
}