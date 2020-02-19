using DChild.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild
{
    [System.Serializable]
    public class CampaignSlotList
    {
        [SerializeField, MinValue(1), OnValueChanged("ValidateSlots")]
        private int m_slotCount;
        [SerializeField, ListDrawerSettings(HideRemoveButton = true, HideAddButton = true, DraggableItems = false, NumberOfItemsPerPage = 1)]
        private CampaignSlot[] m_slots = new CampaignSlot[1];

        public int slotCount => m_slotCount;
        public CampaignSlot GetSlot(int id) => m_slots[id];

        public void SetSlots(CampaignSlot[] slots) => m_slots = slots;

#if UNITY_EDITOR
        private void ValidateSlots()
        {
            var newSlot = new CampaignSlot[m_slotCount];
            var copySize = m_slots.Length < m_slotCount ? m_slots.Length : m_slotCount;
            for (int i = 0; i < copySize; i++)
            {
                newSlot[i] = m_slots[i];
            }
            for (int i = 0; i < m_slotCount; i++)
            {
                if (newSlot[i] == null)
                {
                    newSlot[i] = new CampaignSlot(i + 1);
                }
                else
                {
                    newSlot[i].SetID(i + 1);
                }
            }
            m_slots = newSlot;
        }
#endif
    }
}