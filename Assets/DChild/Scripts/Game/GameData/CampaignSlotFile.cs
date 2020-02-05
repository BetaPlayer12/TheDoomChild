using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Serialization
{
    [CreateAssetMenu(fileName = "CampaignSlotFile",menuName = "DChild/Campaign Slot File")]
    public class CampaignSlotFile : ScriptableObject
    {
        [SerializeField,HideLabel]
        private CampaignSlot m_slot;

        public void LoadFileTo(CampaignSlot slot)
        {
            slot.Copy(m_slot);
        }
    }
}