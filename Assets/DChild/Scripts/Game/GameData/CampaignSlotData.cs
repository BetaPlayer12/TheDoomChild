using DChild.Serialization;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace DChildDebug
{
    [CreateAssetMenu(fileName = "CampaignSlotData", menuName = "DChild/Debug/Campaign Slot")]
    public class CampaignSlotData : SerializedScriptableObject
    {
        [OdinSerialize,HideReferenceObjectPicker,HideLabel]
        private CampaignSlot m_slot = new CampaignSlot();

        public CampaignSlot slot { get => m_slot;}
    }
}