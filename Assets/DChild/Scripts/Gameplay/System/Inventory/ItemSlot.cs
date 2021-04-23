using DChild.Gameplay.Items;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
#endif
namespace DChild.Gameplay.Inventories
{
    [System.Serializable]
    public class ItemSlot
    {
        public struct InfoChangeEventArgs : IEventActionArgs
        {
            public InfoChangeEventArgs(int count) : this()
            {
                this.count = count;
            }

            public int count { get; }
        }

        [System.Serializable]
        public struct Restriction
        {
            public bool canBeSold;
            public bool hasInfiniteCount;
        }

        [SerializeField, OnValueChanged("RestrictCount")]
        private ItemData m_item;
        [SerializeField, MinValue(0), OnValueChanged("RestrictCount")]
        private int m_count;

        public event EventAction<InfoChangeEventArgs> CountChange;

        public ItemSlot(ItemData m_item, int m_count)
        {
            this.m_item = m_item;
            this.m_count = m_count;
            restrictions = new Restriction()
            {
                canBeSold = m_item.canBeSold,
                hasInfiniteCount = m_item.hasInfiniteUses
            };
        }

        public ItemData item => m_item;
        public int count => m_count;
        
        public Restriction restrictions { get; private set; }

        public void AddCount(int count)
        {
            if (restrictions.hasInfiniteCount == false)
            {
                m_count = Mathf.Max(m_count + count, 0);
                CountChange?.Invoke(this, new InfoChangeEventArgs(m_count));
            }
        }

        public void SetCount(int count)
        {
            if (restrictions.hasInfiniteCount == false)
            {
                m_count = count;
                CountChange?.Invoke(this, new InfoChangeEventArgs(m_count));
            }
        }

        public void SetRestriction(Restriction restriction)
        {
            restrictions = restriction;
        }

        public void RestrictCount()
        {
            var maxCount = m_item?.quantityLimit ?? 0;
            m_count = Mathf.Min(m_count, maxCount);
            CountChange?.Invoke(this, new InfoChangeEventArgs(m_count));
        }
    }
}
