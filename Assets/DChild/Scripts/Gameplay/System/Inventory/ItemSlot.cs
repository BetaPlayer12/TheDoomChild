using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
#endif
namespace DChild.Gameplay.Inventories
{
    [System.Serializable]
    public class ItemSlot
    {
        [SerializeField, OnValueChanged("RestrictCount")]
        private ItemData m_item;
        [SerializeField, MinValue(0), OnValueChanged("RestrictCount")]
        private int m_count;

        public ItemSlot(ItemData m_item, int m_count)
        {
            this.m_item = m_item;
            this.m_count = m_count;
        }

        public ItemData item => m_item;
        public int count => m_count;

        public void AddCount(int count) => m_count = Mathf.Max(m_count + count, 0);

        public void SetCount(int count) => m_count = count;

        public void RestrictCount()
        {
            var maxCount = m_item?.quantityLimit ?? 0;
            m_count = Mathf.Min(m_count, maxCount);
        }
    }
}
