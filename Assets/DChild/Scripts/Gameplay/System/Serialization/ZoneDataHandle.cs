using UnityEngine;
using DChild.Gameplay;
using System;
#if UNITY_EDITOR
#endif


namespace DChild.Serialization
{
    public class ZoneDataHandle : MonoBehaviour
    {
        [SerializeField]
        private SerializeDataID m_ID;
        private ISaveData m_saveDataHandler;

        private void Awake()
        {
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