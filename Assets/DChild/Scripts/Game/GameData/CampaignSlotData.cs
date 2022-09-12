using DChild.Serialization;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

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

        public void LoadFileTo(CampaignSlot slot)
        {
            slot.Copy(m_slot);
        }

#if UNITY_EDITOR
        private void ChangeCampaignSlotID()
        {
            m_slot.SetID(m_ID);
        }
        [Button]
        public void SaveChanges()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
#endif
        [Button]
        public void SaveToFile()
        {
            SerializationHandle.SaveCampaignSlot(m_slot.id, m_slot);
        }
    }
}