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
    [CreateAssetMenu(fileName = "CampaignSlotDataTesting", menuName = "DChild/Debug/Campaign Slot Testing")]
    public class CampaignSlotDataTesting : SerializedScriptableObject
    {
        [SerializeField, MinValue(1), OnValueChanged("ChangeCampaignSlotID")]
        private int m_ID;
        [OdinSerialize, HideReferenceObjectPicker, HideLabel]
        private CampaignSlotTesting m_slot = new CampaignSlotTesting(1);

        public CampaignSlotTesting slot { get => m_slot; }

        public void LoadFileTo(CampaignSlotTesting slot)
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
            //SerializationHandle.Save(m_slot.id, m_slot);
        }
    }
}