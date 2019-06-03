using DChild.Serialization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild
{
    [System.Serializable]
    public class CampaignSlotList
    {
        [SerializeField, ValidateInput("ValidateSlots")]
        private CampaignSlot[] m_slots;

        public int slotCount => m_slots.Length;
        public CampaignSlot GetSlot(int id) => m_slots[id];

#if UNITY_EDITOR
        private bool ValidateSlots(CampaignSlot[] slots)
        {
            for (int i = 0; i < (slots?.Length ?? 0); i++)
            {
                slots[i].SetID(i + 1);
            }
            return true;
        }
#endif
    }
}