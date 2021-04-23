using DChild.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Inventories.QuickItem
{
    public class QuickItemManager : SerializedMonoBehaviour
    {
        [SerializeField]
        private IItemContainer m_reference;
        [SerializeField]
        private QuickItemContainer m_container;
        [SerializeField]
        private QuickItemSelectionList m_selectionList;

        private QuickItemSlot[] m_slots;

        public void UpdateSelection()
        {
            m_selectionList.UpdateElements(m_reference);
        }

        public ISaveData Save() => m_container.SaveReferences();

        [Button]
        private void DebugExecution()
        {
            var count = m_container.maxSize <= m_reference.Count ? m_container.maxSize : m_reference.Count;
            for (int i = 0; i < m_container.maxSize; i++)
            {
                if (i < count)
                {
                    var slot = m_reference.GetSlot(i);
                    m_container.UpdateSlot(slot, i);
                    m_slots[i].UpdateSlot(slot);
                }
                else
                {
                    m_container.UpdateSlot(null, i);
                    m_slots[i].UpdateSlot(null);
                }
            }
        }

        private void Awake()
        {
            m_slots = GetComponentsInChildren<QuickItemSlot>();
        }
    }
}
