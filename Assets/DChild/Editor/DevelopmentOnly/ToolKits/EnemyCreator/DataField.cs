using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Toolkit.EnemyCreation
{
    [System.Serializable]
    public abstract class DataField<Data> where Data : ScriptableObject
    {
        [SerializeField, OnValueChanged("OnCreateNewData")]
        protected bool m_createNewData;
        [SerializeField, InlineEditor]
        protected Data m_data;


        private void OnCreateNewData()
        {
            if (m_createNewData)
            {
                m_data = ScriptableObject.CreateInstance<Data>();
            }
            else
            {
                m_data = null;
            }
        }

        protected void CreateData(string path, string suffix)
        {
            path += $"{suffix}.asset";
            AssetDatabase.CreateAsset(m_data, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.LoadAssetAtPath<Data>(path);
            m_createNewData = false;
            m_data = AssetDatabase.LoadAssetAtPath<Data>(path);
        }
    }
}