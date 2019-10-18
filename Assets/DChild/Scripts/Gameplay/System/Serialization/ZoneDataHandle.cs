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
        private ZoneDataID m_ID;

        private void Awake()
        {
            GameplaySystem.campaignSerializer.PreSerialization += OnPreSerialization;
            GameplaySystem.campaignSerializer.PostDeserialization += OnPostDeserialization;
        }

        private void OnPostDeserialization(object sender, CampaignSlotUpdateEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }

        private void OnPreSerialization(object sender, CampaignSlotUpdateEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }
    }
}