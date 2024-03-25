using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Inventories
{
    [System.Serializable]
    public class TradableInventorySerialization
    {
        [System.Serializable]
        public struct ItemSerialization
        {
            public ItemSerialization(IStoredItem item)
            {
                m_id = item.data.id;
                m_count = item.count;
                m_isInfinite = item.hasInfiniteCount;
            }

            public ItemSerialization(ItemSerialization reference)
            {
                m_id = reference.id;
                m_count = reference.count;
                m_isInfinite = reference.isInfinite;
            }

            [SerializeField, ItemDataID]
            private int m_id;
            [SerializeField, DisableIf("m_isInfinite")]
            private int m_count;
            [SerializeField]
            private bool m_isInfinite;

            public int id => m_id;
            public int count => m_count;
            public bool isInfinite => m_isInfinite;
        }

        [SerializeField]
        private int m_soulEssence;
        [SerializeField, TableList]
        private ItemSerialization[] m_serializedItems;


        public TradableInventorySerialization()
        {
            m_serializedItems = new ItemSerialization[0];
        }

        public TradableInventorySerialization(TradableInventory inventory)
        {
            m_serializedItems = new ItemSerialization[inventory.storedItemCount];
            for (int i = 0; i < m_serializedItems.Length; i++)
            {
                m_serializedItems[i] = new ItemSerialization(inventory.GetItem(i));
            }
            m_soulEssence = inventory.currency;
        }


        public TradableInventorySerialization(TradableInventorySerialization inventory)
        {
            if (inventory == null)
            {
                m_serializedItems = new ItemSerialization[0];
                m_soulEssence = 0;
            }
            else
            {
                m_serializedItems = new ItemSerialization[inventory.count];
                for (int i = 0; i < m_serializedItems.Length; i++)
                {
                    m_serializedItems[i] = new ItemSerialization(inventory.GetSerializedItem(i));
                }
                m_soulEssence = inventory.soulEssence;
            }
        }

        public int soulEssence => m_soulEssence;
        public int count => m_serializedItems.Length;


        public ItemSerialization GetSerializedItem(int index) => m_serializedItems[index];
    }
}