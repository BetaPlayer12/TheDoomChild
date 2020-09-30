using DChild.Gameplay.Items;
using Sirenix.OdinInspector;
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
            [SerializeField, OnValueChanged("OnLimitCountChange")]
            private bool m_hasLimitedCount;
            [SerializeField, EnableIf("m_hasLimitedCount")]
            private int m_count;

            public ItemData item => m_item;
            public bool hasLimitedCount => m_hasLimitedCount;
            public int count => m_count;

#if UNITY_EDITOR
            private void OnLimitCountChange()
            {
                if (m_hasLimitedCount == false)
                {
                    m_count = 99;
                }
            }
#endif
        }

        [SerializeField,TableList(AlwaysExpanded = true)]
        private Slot[] m_slots;

        public int slotCount => m_slots.Length;

        public (ItemData item, int count) GetInfo(int index) => (m_slots[index].item, m_slots[index].count);

        public bool HasLimitedCount(ItemData itemData)
        {
            for (int i = 0; i < m_slots.Length; i++)
            {
                if (m_slots[i].item == itemData)
                {
                    return m_slots[i].hasLimitedCount;
                }
            }
            return false;
        }
    }
}
