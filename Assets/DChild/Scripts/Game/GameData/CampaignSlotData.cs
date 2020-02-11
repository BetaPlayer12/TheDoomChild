using DChild.Serialization;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace DChildDebug
{
    [CreateAssetMenu(fileName = "CampaignSlotData", menuName = "DChild/Debug/Campaign Slot")]
    public class CampaignSlotData : SerializedScriptableObject
    {
        [SerializeField, MinValue(1), OnValueChanged("ChangeCampaignSlotID")]
        private int m_ID;
        [OdinSerialize, HideReferenceObjectPicker, HideLabel]
        private CampaignSlot m_slot = new CampaignSlot(1);

        public CampaignSlot slot { get => m_slot; }
# if UNITY_EDITOR
        private void ChangeCampaignSlotID()
        {
            m_slot.SetID(m_ID);
        }
#endif
    }
}