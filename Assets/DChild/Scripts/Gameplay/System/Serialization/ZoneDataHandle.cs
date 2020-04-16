﻿using UnityEngine;
using DChild.Gameplay;
using System;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Sirenix.Serialization;
using System.Collections;
using DChildDebug.Serialization;
using UnityEngine.SceneManagement;

namespace DChild.Serialization
{
    public class ZoneDataHandle : SerializedMonoBehaviour
    {
        [System.Serializable]
        public class ZoneData : ISaveData
        {
            [SerializeField]
            private Dictionary<SerializeID, ISaveData> m_savedDatas = new Dictionary<SerializeID, ISaveData>(new SerializeID.EqualityComparer());

            public ZoneData()
            {
                m_savedDatas = new Dictionary<SerializeID, ISaveData>(new SerializeID.EqualityComparer());
            }

            public ZoneData(ComponentSerializer[] serializers)
            {
                m_savedDatas = new Dictionary<SerializeID, ISaveData>();
                for (int i = 0; i < serializers.Length; i++)
                {
                    m_savedDatas.Add(serializers[i].ID, null);
                }
            }

            public ZoneData(Dictionary<SerializeID, ISaveData> savedDatas)
            {
                m_savedDatas = new Dictionary<SerializeID, ISaveData>(savedDatas, new SerializeID.EqualityComparer());
            }

            public void SetData(SerializeID ID, ISaveData data)
            {
                if (m_savedDatas.ContainsKey(ID))
                {
                    m_savedDatas[ID] = data;
                }
                else
                {
                    m_savedDatas.Add(new SerializeID(ID, false), data);
                }
            }

            public ISaveData GetData(SerializeID ID) => m_savedDatas.ContainsKey(ID) ? m_savedDatas[ID] : null;

#if UNITY_EDITOR
            public Dictionary<SerializeID, ISaveData> savedDatas => m_savedDatas;
#endif
        }

        [SerializeField]
        private SerializeID m_ID = new SerializeID(true);
        [SerializeField, ValueDropdown("GetComponents", IsUniqueList = true), TabGroup("Serializer", "Component"), OnValueChanged("UpdateEditorData", true)]
        private ComponentSerializer[] m_componentSerializers;
        [SerializeField, ValueDropdown("GetDynamicSerializers", IsUniqueList = true), TabGroup("Serializer", "Dynamic"), OnValueChanged("UpdateEditorData", true)]
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
        private void Start()
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

#if UNITY_EDITOR
            if (m_useEditorData)
            {
                m_editorData.InitializeDynamicDatas();
                for (int i = 0; i < m_dynamicSerializers.Length; i++)
                {
                    m_dynamicSerializers[i].Initialize();
                    m_dynamicSerializers[i].LoadUsingCurrent();
                }
            }
            else
            {
                for (int i = 0; i < m_dynamicSerializers.Length; i++)
                {
                    m_dynamicSerializers[i].Initialize();
                    m_dynamicSerializers[i].Load();
                }
            }
#else

            for (int i = 0; i < m_dynamicSerializers.Length; i++)
            {
                m_dynamicSerializers[i].Initialize();
                m_dynamicSerializers[i].Load();
            }
#endif

            GameplaySystem.campaignSerializer.PreSerialization += OnPreSerialization;
            GameplaySystem.campaignSerializer.PostDeserialization += OnPostDeserialization;
            SceneManager.SetActiveScene(gameObject.scene);
        }

        private void OnDestroy()
        {
            GameplaySystem.campaignSerializer.PreSerialization -= OnPreSerialization;
            GameplaySystem.campaignSerializer.PostDeserialization -= OnPostDeserialization;
        }

        #region Editor
#if UNITY_EDITOR
        [System.Serializable]
        private class EditorData
        {
            [System.Serializable, HideReferenceObjectPicker]
            public class ComponentData
            {
                [SerializeField, ReadOnly, DrawWithUnity]
                public ComponentSerializer m_serializer;
                [SerializeField, HideReferenceObjectPicker]
                public ISaveData m_saveData;

                public ComponentData(ComponentSerializer serializer)
                {
                    m_serializer = serializer;
                    m_serializer.Initiatlize();
                    m_saveData = m_serializer.SaveData();
                }
            }

            [System.Serializable]
            public class DynamicData
            {
                [SerializeField, ReadOnly, DrawWithUnity]
                public DynamicSerializableData m_data;
                [SerializeField, HideReferenceObjectPicker]
                public ISaveData m_saveData;

                public DynamicData(DynamicSerializableComponent serializer)
                {
                    m_data = serializer.data;
                    m_saveData = m_data.GetData<ISaveData>();
                }
            }

            [SerializeField, ListDrawerSettings(HideAddButton = true, HideRemoveButton = true), TabGroup("Component")]
            private List<ComponentData> m_componenetDatas;
            [SerializeField, ListDrawerSettings(HideAddButton = true, HideRemoveButton = true), TabGroup("Dy")]
            private List<DynamicData> m_dynamicDatas;

            public void InitializeDynamicDatas()
            {
                for (int i = 0; i < m_dynamicDatas.Count; i++)
                {
                    m_dynamicDatas[i].m_data.SetData(m_dynamicDatas[i].m_saveData);
                }
            }

            public ComponentData GetData(ComponentSerializer serializer)
            {
                for (int i = 0; i < m_componenetDatas.Count; i++)
                {
                    if (serializer == m_componenetDatas[i].m_serializer)
                    {
                        return m_componenetDatas[i];
                    }
                }
                return null;
            }

            public EditorData(IReadOnlyList<ComponentSerializer> componentList, IReadOnlyList<DynamicSerializableComponent> dynamicList)
            {
                m_componenetDatas = new List<ComponentData>();
                var componentSize = componentList?.Count ?? 0;
                for (int i = 0; i < componentSize; i++)
                {
                    m_componenetDatas.Add(new ComponentData(componentList[i]));
                }

                m_dynamicDatas = new List<DynamicData>();
                var dynamicSize = dynamicList?.Count ?? 0;
                for (int i = 0; i < dynamicSize; i++)
                {
                    bool isNew = true;
                    var data = dynamicList[i].data;
                    for (int j = 0; j < m_dynamicDatas.Count; j++)
                    {
                        if (m_dynamicDatas[j].m_data == data)
                        {
                            isNew = false;
                            break;
                        }
                    }
                    if (isNew)
                    {
                        m_dynamicDatas.Add(new DynamicData(dynamicList[i]));
                    }
                }
            }

            public void CopyPastSavedData(EditorData source)
            {
                for (int i = 0; i < m_componenetDatas.Count; i++)
                {
                    var data = source.GetData(m_componenetDatas[i].m_serializer);
                    if (data != null)
                    {
                        m_componenetDatas[i] = data;
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
                    var dictionary = new Dictionary<SerializeID, ISaveData>();
                    for (int i = 0; i < editorData.m_componenetDatas.Count; i++)
                    {
                        var save = editorData.m_componenetDatas[i];
                        dictionary.Add(save.m_serializer.ID, save.m_saveData);
                    }
                    return new ZoneData(dictionary);
                }
            }
        }

        [SerializeField, HideInPlayMode, ToggleGroup("m_useEditorData")]
        private bool m_useEditorData;
        [OdinSerialize, HideInPlayMode, HideReferenceObjectPicker, ShowIf("m_useEditorData"), ToggleGroup("m_useEditorData"), HideLabel]
        private EditorData m_editorData;

        private void UpdateEditorData()
        {

            if (m_editorData == null)
            {
                m_editorData = new EditorData(m_componentSerializers, m_dynamicSerializers);
            }
            else
            {
                var newData = new EditorData(m_componentSerializers, m_dynamicSerializers);
                newData.CopyPastSavedData(m_editorData);
                m_editorData = newData;
            }
        }

        public (SerializeID ID, ZoneData data) GetDefaultData()
        {
            var zoneData = new ZoneData();
            for (int i = 0; i < m_componentSerializers.Length; i++)
            {
                m_cacheComponentSerializer = m_componentSerializers[i];
                m_cacheComponentSerializer.Initiatlize();
                zoneData.SetData(m_cacheComponentSerializer.ID, m_cacheComponentSerializer.SaveData());
            }
            return (m_ID, zoneData);
        }

        private IEnumerable GetComponents() => FindObjectsOfType<ComponentSerializer>();

        private IEnumerable GetDynamicSerializers() => FindObjectsOfType<DynamicSerializableComponent>();
#endif 
        #endregion
    }
}