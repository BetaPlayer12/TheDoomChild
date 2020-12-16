using DChild.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Inventories.QuickItem
{
    public class QuickItemContainer : MonoBehaviour
    {
        [System.Serializable]
        public class SaveData : ISaveData
        {
            [SerializeField]
            private int[] m_ids;

            public SaveData(ItemSlot[] slots)
            {
                m_ids = new int[slots.Length];
                for (int i = 0; i < m_ids.Length; i++)
                {
                    m_ids[i] = slots[i].item.id;
                }
            }

            public SaveData(int[] ids)
            {
                m_ids = new int[ids.Length];
                for (int i = 0; i < ids.Length; i++)
                {
                    m_ids[i] = ids[i];
                }
            }

            public int[] ids => m_ids;

            public ISaveData ProduceCopy()
            {
                return new SaveData(m_ids);
            }
        }

        [SerializeField, MinValue(1)]
        private int m_maxSize;
        private ItemSlot[] m_slots;

        public int maxSize => m_maxSize;

        public ItemSlot GetSlot(int index) => m_slots[index];

        public void UpdateSlot(ItemSlot slot, int index)
        {
            m_slots[index] = slot;
        }

        public void ReduceItemCount(ItemSlot slot, int count)
        {
            slot.AddCount(-1);
            if(slot.count <= 0)
            {
                for (int i = 0; i < m_slots.Length; i++)
                {
                    if(m_slots[i] == slot)
                    {
                        m_slots[i] = null;
                        break;
                    }
                }
            }
        }

        public ISaveData SaveReferences() => new SaveData(m_slots);

        private void Awake()
        {
            m_slots = new ItemSlot[m_maxSize];
        }
    }
}
