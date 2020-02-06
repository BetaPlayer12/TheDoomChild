using UnityEngine;
using DChild.Gameplay;
using System;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Sirenix.Serialization;
using System.Collections;

namespace DChild.Serialization
{
    public class ZoneDataHandle : SerializedMonoBehaviour
    {
        [System.Serializable]
        public class ZoneData : ISaveData
        {
            [SerializeField]
            private Dictionary<int, ISaveData> m_savedDatas;

            public ZoneData()
            {
                m_savedDatas = new Dictionary<int, ISaveData>();
            }

            public ZoneData(ComponentSerializer[] serializers)
            {
                m_savedDatas = new Dictionary<int, ISaveData>();
                for (int i = 0; i < serializers.Length; i++)
                {
                    m_savedDatas.Add(serializers[i].ID, null);
                }
            }

            public ZoneData(Dictionary<int, ISaveData> savedDatas)
            {
                m_savedDatas = savedDatas;
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
        [SerializeField, ValueDropdown("GetComponents", IsUniqueList = true), TabGroup("Serializer", "Component"), OnValueChanged("UpdateEditorData", true)]
        private ComponentSerializer[] m_componentSerializers;
        [SerializeField, ValueDropdown("GetDynamicSerializers", IsUniqueList = true), TabGroup("Serializer", "Dynamic")]
        private DynamicSerializableComponent[] m_dynamicSerializers;
        [OdinSerialize, HideInEditorMode]
        private ZoneData m_zoneData = new ZoneData();

        private CampaignSlot m_cacheSlot;
        private ComponentSerializer m_cacheComponentSerializer;


        private void OnPostDeserialization(object sender, CampaignSlotUpdateEventArgs eventArgs)
        {
            m_cacheSlot = GameplaySystem.campaignSerializer.slot;
            m_zoneData = m_cacheSlot.GetZoneData<ZoneData>(m_ID);
            if (m_zoneData != null)
            {
                UpdateSerializers();
            }
            for (int i = 0; i < m_dynamicSerializers.Length; i++)
            {
                m_dynamicSerializers[i].Load();
            }
        }

        private void OnPreSerialization(object sender, CampaignSlotUpdateEventArgs eventArgs)
        {
            UpdateSaveData();
        }

        private void UpdateSaveData()
        {
            for (int i = 0; i < m_componentSerializers.Length; i++)
            {
                m_cacheComponentSerializer = m_componentSerializers[i];
                m_zoneData.SetData(m_cacheComponentSerializer.ID, m_cacheComponentSerializer.SaveData());
            }
            for (int i = 0; i < m_dynamicSerializers.Length; i++)
            {
                m_dynamicSerializers[i].Save();
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
        private void Awake()
        {
            var proposedData = GameplaySystem.campaignSerializer.slot.GetZoneData<ZoneData>(m_ID);
#if UNITY_EDITOR
            if (m_useEditorData)
            {
                proposedData = m_editorData;
            }
#endif
            bool hasData = false;
            if (proposedData != null)
            {
                hasData = true;
                m_zoneData = proposedData;
            }

            for (int i = 0; i < m_componentSerializers.Length; i++)
            {
                m_cacheComponentSerializer = m_componentSerializers[i];
                m_cacheComponentSerializer.Initiatlize();

                if (hasData)
                {
                    m_cacheComponentSerializer.LoadData(m_zoneData.GetData(m_cacheComponentSerializer.ID));
                }
            }
            for (int i = 0; i < m_dynamicSerializers.Length; i++)
            {
                m_dynamicSerializers[i].Initialize();
                m_dynamicSerializers[i].Load();
            }
            GameplaySystem.campaignSerializer.PreSerialization += OnPreSerialization;
            GameplaySystem.campaignSerializer.PostDeserialization += OnPostDeserialization;
        }

        private void OnDestroy()
        {
            UpdateSaveData();
        }

        #region Editor
#if UNITY_EDITOR
        [System.Serializable]
        private class EditorData
        {
            [System.Serializable, HideReferenceObjectPicker]
            public class Data
            {
                [SerializeField, ReadOnly]
                public ComponentSerializer m_serializer;
                [SerializeField, HideReferenceObjectPicker]
                public ISaveData m_saveData;

                public Data(ComponentSerializer serializer)
                {
                    m_serializer = serializer;
                    m_serializer.Initiatlize();
                    m_saveData = m_serializer.SaveData();
                }
            }

            [SerializeField, ListDrawerSettings(HideAddButton = true, HideRemoveButton = true)]
            private List<Data> m_datas;

            public Data GetData(ComponentSerializer serializer)
            {
                for (int i = 0; i < m_datas.Count; i++)
                {
                    if (serializer == m_datas[i].m_serializer)
                    {
                        return m_datas[i];
                    }
                }
                return null;
            }

            public EditorData(IReadOnlyList<ComponentSerializer> list)
            {
                m_datas = new List<Data>();
                for (int i = 0; i < list.Count; i++)
                {
                    m_datas.Add(new Data(list[i]));
                }
            }

            public void CopyPastSavedData(EditorData source)
            {
                for (int i = 0; i < m_datas.Count; i++)
                {
                    var data = source.GetData(m_datas[i].m_serializer);
                    if (data != null)
                    {
                        m_datas[i] = data;
                    }
                }
            }

            public static implicit operator ZoneData(EditorData editorData)
            {
                if (editorData == null)
                {
                    return null;
                }
                else
                {
                    var dictionary = new Dictionary<int, ISaveData>();
                    for (int i = 0; i < editorData.m_datas.Count; i++)
                    {
                        var save = editorData.m_datas[i];
                        dictionary.Add(save.m_serializer.ID, save.m_saveData);
                    }
                    return new ZoneData(dictionary);
                }
            }
        }

        [SerializeField, HideInPlayMode]
        private bool m_useEditorData;
        [OdinSerialize, HideInPlayMode, HideReferenceObjectPicker, ShowIf("m_useEditorData")]
        private EditorData m_editorData;

        private void UpdateEditorData()
        {
            if (m_editorData == null)
            {
                m_editorData = new EditorData(m_componentSerializers);
            }
            else
            {
                var newData = new EditorData(m_componentSerializers);
                newData.CopyPastSavedData(m_editorData);
                m_editorData = newData;
            }
        }

        private IEnumerable GetComponents() => FindObjectsOfType<ComponentSerializer>();

        private IEnumerable GetDynamicSerializers() => FindObjectsOfType<DynamicSerializableComponent>();
#endif 
        #endregion
    }
}