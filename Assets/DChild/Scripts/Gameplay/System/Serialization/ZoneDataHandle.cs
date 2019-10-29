using UnityEngine;
using DChild.Gameplay;
using System;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Sirenix.Serialization;

namespace DChild.Serialization
{

    public class ZoneDataHandle : SerializedMonoBehaviour
    {
        [System.Serializable]
        public class ZoneData : ISaveData
        {
            [SerializeField]
            private Dictionary<int, ISaveData> m_savedDatas;

            public ZoneData(ComponentSerializer[] serializers)
            {
                m_savedDatas = new Dictionary<int, ISaveData>();
                for (int i = 0; i < serializers.Length; i++)
                {
                    m_savedDatas.Add(serializers[i].ID, null);
                }
            }

            public void SetData(int ID, ISaveData data)
            {
                if (m_savedDatas.ContainsKey(ID))
                {
                    m_savedDatas[ID] = data;
                }
                else
                {
                    m_savedDatas.Add(ID, data);
                }
            }

            public ISaveData GetData(int ID) => m_savedDatas.ContainsKey(ID) ? m_savedDatas[ID] : null;
        }

        [SerializeField]
        private SerializeDataID m_ID;
        [SerializeField]
        private ComponentSerializer[] m_componentSerializers;
        [OdinSerialize]
        private ZoneData m_zoneData;

        private ComponentSerializer m_cacheComponentSerializer;

        private void Awake()
        {
            var proposedData = GameplaySystem.campaignSerializer.slot.GetZoneData<ZoneData>(m_ID);
            if (proposedData != null)
            {
                m_zoneData = proposedData;
                UpdateSerializers();
            }
            GameplaySystem.campaignSerializer.PreSerialization += OnPreSerialization;
            GameplaySystem.campaignSerializer.PostDeserialization += OnPostDeserialization;
        }

        private void OnPostDeserialization(object sender, CampaignSlotUpdateEventArgs eventArgs)
        {
            m_zoneData = GameplaySystem.campaignSerializer.slot.GetZoneData<ZoneData>(m_ID);
            UpdateSerializers();
        }

        private void OnPreSerialization(object sender, CampaignSlotUpdateEventArgs eventArgs)
        {
            for (int i = 0; i < m_componentSerializers.Length; i++)
            {
                m_cacheComponentSerializer = m_componentSerializers[i];
                m_zoneData.SetData(m_cacheComponentSerializer.ID, m_cacheComponentSerializer.SaveData());
            }
            GameplaySystem.campaignSerializer.slot.UpdateZoneData(m_ID, m_zoneData);
        }

        private void UpdateSerializers()
        {
            for (int i = 0; i < m_componentSerializers.Length; i++)
            {
                m_cacheComponentSerializer = m_componentSerializers[i];
                m_cacheComponentSerializer.LoadData(m_zoneData.GetData(m_cacheComponentSerializer.ID));
            }
        }
    }
}