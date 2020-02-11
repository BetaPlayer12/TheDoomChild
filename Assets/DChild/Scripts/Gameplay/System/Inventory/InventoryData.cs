using DChild.Gameplay.Items;
using UnityEngine;

namespace DChild.Gameplay.Inventories
{
    [CreateAssetMenu(fileName = "InventoryData", menuName = "DChild/Database/Inventory Data")]
    public class InventoryData : ScriptableObject
    {
        [System.Serializable]
        public class Slot
        {
            [SerializeField]
            private ItemData m_item;
            [SerializeField]
            private int m_count;

            public ItemData item => m_item;
            public int count => m_count;
        }

        [SerializeField]
        private Slot[] m_slots;

        public int slotCount => m_slots.Length;

        public (ItemData item, int count) GetInfo(int index) => (m_slots[index].item, m_slots[index].count);
    }
}
