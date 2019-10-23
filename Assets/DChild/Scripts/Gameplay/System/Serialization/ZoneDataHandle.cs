using UnityEngine;
using DChild.Gameplay;
using System;
#if UNITY_EDITOR
#endif


namespace DChild.Serialization
{
    public class ZoneDataHandle : MonoBehaviour
    {

        public DataSave m_saveDataHandler;

        public int numVal;


        
        [SerializeField]
        private SerializeDataID m_ID;

        

        private void Awake()
        {
            m_saveDataHandler.numval = numVal;
            GameplaySystem.campaignSerializer.PreSerialization += OnPreSerialization;
            GameplaySystem.campaignSerializer.PostDeserialization += OnPostDeserialization;
        }

        private void OnPostDeserialization(object sender, CampaignSlotUpdateEventArgs eventArgs)
        {
            GameplaySystem.campaignSerializer.slot.GetZoneData<ISaveData>(m_ID);
        }

        private void OnPreSerialization(object sender, CampaignSlotUpdateEventArgs eventArgs)
        {
            GameplaySystem.campaignSerializer.slot.UpdateZoneData(m_ID, m_saveDataHandler);
        }
    }
}