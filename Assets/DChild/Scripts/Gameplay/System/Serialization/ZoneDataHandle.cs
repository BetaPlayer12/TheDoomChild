using UnityEngine;
using DChild.Gameplay;
using System;
using System.IO;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Sirenix.Serialization;
using System.Collections;
using UnityEngine.SceneManagement;
using PixelCrushers.DialogueSystem;
using DChild.Gameplay.Cinematics;
#if UNITY_EDITOR
using UnityEditor;
using DChildDebug;
#endif

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

            public ZoneData(Dictionary<SerializeID, ISaveData> savedDatas, bool createSeparateCopy)
            {
                if (createSeparateCopy)
                {
                    m_savedDatas = new Dictionary<SerializeID, ISaveData>(new SerializeID.EqualityComparer());
                    foreach (var ID in savedDatas.Keys)
                    {
                        m_savedDatas.Add(ID, savedDatas[ID].ProduceCopy());
                    }
                }
                else
                {
                    m_savedDatas = new Dictionary<SerializeID, ISaveData>(savedDatas, new SerializeID.EqualityComparer());
                }

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
            ISaveData ISaveData.ProduceCopy() => new ZoneData(m_savedDatas, true);

            public Dictionary<SerializeID, ISaveData> savedDatas => m_savedDatas;
        }
        [SerializeField, BoxGroup("Temporary (To Be Deleted)")]
        private bool m_useCameraPrioritization;

        public ZoneSlot Zone;

        [SerializeField]
        private SerializeID m_ID = new SerializeID(true);
        [SerializeField, ValueDropdown("GetComponents", IsUniqueList = true), TabGroup("Serializer", "Component"), OnValueChanged("UpdateEditorData", true)]
        private ComponentSerializer[] m_componentSerializers;
        [SerializeField, TabGroup("Serializer", "Dialogue Updates")]
        private DialogueSystemTrigger[] m_dialogueSystemUpdates;
        [SerializeField, TabGroup("Serializer", "Quest Listeners")]
        private QuestStateListener[] m_questsListener;
        [OdinSerialize, HideInEditorMode]
        private ZoneData m_zoneData = new ZoneData();
        private bool m_hasLoaded;


        private CampaignSlot m_cacheSlot;
        private ComponentSerializer m_cacheComponentSerializer;
        public bool hasLoaded => m_hasLoaded;

        public void ForceUpdateZoneData()
        {
            GameplaySystem.campaignSerializer.UpdateData(SerializationScope.Zone);
        }

        public void RemoveNullInComponentSerializers()
        {
            List<ComponentSerializer> serializers = new List<ComponentSerializer>(m_componentSerializers);
            for (int i = serializers.Count - 1; i >= 0; i--)
            {
                if (serializers[i] == null)
                {
                    serializers.RemoveAt(i);
                }
            }
            m_componentSerializers = serializers.ToArray();
        }

        private void OnPostDeserialization(object sender, CampaignSlotUpdateEventArgs eventArgs)
        {
            if (eventArgs.IsPartOfTheUpdate(SerializationScope.Zone))
            {
                m_cacheSlot = GameplaySystem.campaignSerializer.slot;
                m_zoneData = m_cacheSlot.GetZoneData<ZoneData>(m_ID);
                if (m_zoneData != null)
                {
                    UpdateSerializers();
                }

                PersistentDataManager.ApplySaveData(m_cacheSlot.dialogueSaveData, DatabaseResetOptions.KeepAllLoaded);
                for (int i = 0; i < m_questsListener.Length; i++)
                {
                    m_questsListener[i].UpdateIndicator(true);
                }
                Debug.Log("Zone Loaded:" + gameObject.scene.name);
                m_hasLoaded = true;
            }
        }

        private void OnPreSerialization(object sender, CampaignSlotUpdateEventArgs eventArgs)
        {
            if (eventArgs.IsPartOfTheUpdate(SerializationScope.Zone))
            {
                UpdateSaveData();
                Debug.Log("Zone Saved:" + gameObject.scene.name);
            }
        }

        private void UpdateSaveData()
        {
            for (int i = 0; i < m_componentSerializers.Length; i++)
            {
                m_cacheComponentSerializer = m_componentSerializers[i];
                try
                {
                    m_zoneData.SetData(m_cacheComponentSerializer.ID, m_cacheComponentSerializer.SaveData());
                }
                catch (Exception e)
                {
                    Debug.LogError($"Serialization Error: {m_cacheComponentSerializer.gameObject.name} \n {e.Message}", m_cacheComponentSerializer);
                }
            }

            var slot = GameplaySystem.campaignSerializer.slot;
            slot.UpdateZoneData(m_ID, m_zoneData);
            slot.UpdateDialogueSaveData();
            Debug.Log("Zone Save Updated:" + gameObject.scene.name);

        }

        private void UpdateDialogueSaveData(CampaignSlot slot)
        {
            if (m_dialogueSystemUpdates != null)
            {
                for (int i = 0; i < m_dialogueSystemUpdates.Length; i++)
                {
                    m_dialogueSystemUpdates[i].OnUse();
                }
            }
            slot.UpdateDialogueSaveData();
        }

        private void UpdateSerializers()
        {
            for (int i = 0; i < m_componentSerializers.Length; i++)
            {
                m_cacheComponentSerializer = m_componentSerializers[i];
                try
                {
                    m_cacheComponentSerializer.LoadData(m_zoneData.GetData(m_cacheComponentSerializer.ID));
                }
                catch (Exception e)
                {
                    Debug.LogError($"Deserialization Error: {m_cacheComponentSerializer.gameObject.name} \n {e.Message}", m_cacheComponentSerializer);
                }
            }
            Debug.Log("Zone Serializers Updated:" + gameObject.scene.name);
        }

        private IEnumerator LoadComponentsRoutine(bool hasData)
        {
            for (int i = 0; i < m_componentSerializers.Length; i++)
            {
                m_cacheComponentSerializer = m_componentSerializers[i];
                try
                {
                    m_cacheComponentSerializer.Initiatlize();

                    if (hasData)
                    {
                        m_cacheComponentSerializer.LoadData(m_zoneData.GetData(m_cacheComponentSerializer.ID));
                    }
                    else
                    {
                        m_cacheComponentSerializer.InitializeComponentData();
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Deserialization Error: {m_cacheComponentSerializer.gameObject.name} \n {e.ToString()}", m_cacheComponentSerializer);
                    //throw new Exception($"Error Occured In {m_cacheComponentSerializer.gameObject.name} \n {e.Message}");
                }
                yield return null;
            }
            Debug.Log("Zone Components Loaded:" + gameObject.scene.name);
            m_hasLoaded = true;
        }

        private void Start()
        {
#if UNITY_EDITOR
            SceneCameraIntergrityChecker.ValidateIntegrity();
#endif
            ((Cinema)GameplaySystem.cinema).m_useCameraPrioritization = m_useCameraPrioritization;


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

            StartCoroutine(LoadComponentsRoutine(hasData));
            //for (int i = 0; i < m_componentSerializers.Length; i++)
            //{
            //    m_cacheComponentSerializer = m_componentSerializers[i];
            //    m_cacheComponentSerializer.Initiatlize();

            //    if (hasData)
            //    {
            //        m_cacheComponentSerializer.LoadData(m_zoneData.GetData(m_cacheComponentSerializer.ID));
            //    }
            //}

            GameplaySystem.campaignSerializer.PreSerialization += OnPreSerialization;
            GameplaySystem.campaignSerializer.PostDeserialization += OnPostDeserialization;
            SceneManager.SetActiveScene(gameObject.scene);
        }

        private void OnDestroy()
        {
            var slot = GameplaySystem.campaignSerializer.slot;

            GameplaySystem.campaignSerializer.PreSerialization -= OnPreSerialization;
            GameplaySystem.campaignSerializer.PostDeserialization -= OnPostDeserialization;
        }

        #region Editor
#if UNITY_EDITOR
        [Button]
        public void CreateZoneSaveFile()
        {
            ZoneSaveFileCreate();
        }

        public ComponentSerializer[] GetComponentSerializers() => m_componentSerializers;

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

            [SerializeField, ListDrawerSettings(HideAddButton = true, HideRemoveButton = true), TabGroup("Component")]
            private List<ComponentData> m_componenetDatas;

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

            public EditorData(IReadOnlyList<ComponentSerializer> componentList)
            {
                m_componenetDatas = new List<ComponentData>();
                var componentSize = componentList?.Count ?? 0;
                for (int i = 0; i < componentSize; i++)
                {
                    m_componenetDatas.Add(new ComponentData(componentList[i]));
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
                    return new ZoneData(dictionary, false);
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
                m_editorData = new EditorData(m_componentSerializers);
            }
            else
            {
                var newData = new EditorData(m_componentSerializers);
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

        private IEnumerable GetComponents()
        {
            var serializers = FindObjectsOfType<ComponentSerializer>();
            var list = new ValueDropdownList<ComponentSerializer>();

            foreach (var serializer in serializers)
            {
                var text = $"{serializer.gameObject.name} ({serializer.ID.ToString()})";
                var item = new ValueDropdownItem<ComponentSerializer>(text, serializer);
                if (list.Contains(item))
                {
                    int index = 1;
                    do
                    {
                        item.Text = $"--{text} [{index}]";
                    } while (list.Contains(item));
                }
                list.Add(item);
            }
            return list;
        }

        private void ZoneSaveFileCreate()
        {
            String m_folderPath = "Assets/DChild/Objects/Misc/Zone Slots/";
            String m_extension = "SaveSlot.mat";

            for (int i = 0; i < m_componentSerializers.Length; i++)
            {
                m_cacheComponentSerializer = m_componentSerializers[i];
                m_cacheComponentSerializer.Initiatlize();
                m_zoneData.SetData(m_cacheComponentSerializer.ID, m_cacheComponentSerializer.SaveData());
            }
            if (!Directory.Exists(m_folderPath))
            {

                Directory.CreateDirectory(m_folderPath);

            }
            if (AssetDatabase.FindAssets(m_ID.ToString() + "SaveSlot", null) == null)
            {
                Debug.Log(AssetDatabase.FindAssets(m_ID.ToString() + "SaveSlot", null));
                var instance = AssetDatabase.LoadAssetAtPath<ZoneSlot>(m_folderPath + m_ID.ToString() + m_extension);
                instance.UpdateZoneSlot(m_zoneData);
                instance.UpdateZoneID(m_ID);
                EditorUtility.SetDirty(instance);
                AssetDatabase.SaveAssets();
            }
            else
            {
                var instance = ScriptableObject.CreateInstance<ZoneSlot>();
                instance.UpdateZoneSlot(m_zoneData);
                instance.UpdateZoneID(m_ID);
                AssetDatabase.CreateAsset(instance, m_folderPath + m_ID.ToString() + m_extension);
                EditorUtility.SetDirty(instance);
                AssetDatabase.SaveAssets();
            }
        }
#endif
        #endregion

    }
}