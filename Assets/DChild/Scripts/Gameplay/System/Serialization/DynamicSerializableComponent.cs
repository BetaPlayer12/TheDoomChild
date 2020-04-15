using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using System.Collections;
using UnityEditor;
#endif

namespace DChild.Serialization
{
    [ExecuteAlways]
    public class DynamicSerializableComponent : MonoBehaviour
    {
        private enum SerializationType
        {
            LoadOnly,
            SaveOnly,
            Both
        }

        [SerializeField, DisableIf("@m_multiSceneData != null"), ValueDropdown("GetValues"), OnValueChanged("OnValueChanged")]
        private DynamicSerializableData m_multiSceneData;
        [SerializeField]
        private SerializationType m_serializationType;
        private ISerializableComponent m_component;

        public DynamicSerializableData data => m_multiSceneData;

        public void Load()
        {
            if (m_serializationType == SerializationType.LoadOnly || m_serializationType == SerializationType.Both)
            {
                try
                {
                    m_multiSceneData.LoadData();
                }
                catch (System.NullReferenceException)
                {
                    return;
                }
                var data = m_multiSceneData.GetData<ISaveData>();
                if (data != null)
                {
                    m_component.Load(data);
                }
            }
        }

        public void LoadUsingCurrent()
        {
            if (m_serializationType == SerializationType.LoadOnly || m_serializationType == SerializationType.Both)
            {
                m_component.Load(m_multiSceneData.GetData<ISaveData>());
            }
        }


        public void Save()
        {
            if (m_serializationType == SerializationType.SaveOnly || m_serializationType == SerializationType.Both)
            {
                m_multiSceneData.SetData(m_component.Save());
                m_multiSceneData.SaveData();
            }
        }

        public void Initialize()
        {
            m_component = GetComponent<ISerializableComponent>();
        }

#if UNITY_EDITOR
        private IEnumerable GetValues()
        {
            ValueDropdownList<DynamicSerializableData> list = new ValueDropdownList<DynamicSerializableData>();
            var filePaths = AssetDatabase.FindAssets("t:DynamicSerializableData");
            for (int i = 0; i < filePaths.Length; i++)
            {
                var asset = AssetDatabase.LoadAssetAtPath<DynamicSerializableData>(AssetDatabase.GUIDToAssetPath(filePaths[i]));
                if (asset != null)
                {
                    list.Add(asset);
                }
            }
            return list;
        }

        private void OnValueChanged()
        {
            m_multiSceneData.SetLock(true);
        }

        private void OnDestroy()
        {
            if (Application.isPlaying == false)
            {
                m_multiSceneData?.SetLock(false);
            }
        }
#endif
    }
}