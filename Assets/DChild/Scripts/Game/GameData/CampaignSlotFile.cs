using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Serialization
{
    [CreateAssetMenu(fileName = "CampaignSlotFile", menuName = "DChild/Campaign Slot File")]
    public class CampaignSlotFile : ScriptableObject
    {
        [SerializeField, MinValue(0), OnValueChanged("OnIDChange")]
        private int m_ID = 1;

        [SerializeField, HideLabel]
        private CampaignSlot m_slot;

        public void LoadFileTo(CampaignSlot slot)
        {
            slot.Copy(m_slot);
        }

#if UNITY_EDITOR
        private void OnIDChange()
        {
            m_slot.SetID(m_ID);
        }

        [Button]
        private void SaveToFile()
        {
            SerializationHandle.Save(m_slot.id, m_slot);
        }
#endif
    }
}