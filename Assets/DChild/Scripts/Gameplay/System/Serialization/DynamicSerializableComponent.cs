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
        [SerializeField, DisableIf("@m_multiSceneData != null"), ValueDropdown("GetValues"), OnValueChanged("OnValueChanged")]
        private DynamicSerializableData m_multiSceneData;
        private ISerializableComponent m_component;

        public void Load()
        {
            try
            {
                m_multiSceneData.LoadData();
            }
            catch (System.NullReferenceException)
            {
                return;
            }
            m_component.Load(m_multiSceneData.GetData<ISaveData>());
        }

        public void Save()
        {
            m_multiSceneData.SetData(m_component.Save());
            m_multiSceneData.SaveData();
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
                if (asset != null && asset.isLocked == false)
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