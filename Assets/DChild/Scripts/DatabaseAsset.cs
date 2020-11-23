using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using DChildEditor;
#endif

namespace DChild
{
    public abstract class DatabaseAsset : SerializedScriptableObject
    {
        [SerializeField, ReadOnly, HideLabel, HorizontalGroup("Signature")]
        protected int m_ID = -1;
        [SerializeField, ReadOnly, HideLabel, HorizontalGroup("Signature")]
        protected string m_name;
        [PropertySpace]

#if UNITY_EDITOR
        [SerializeField, OnValueChanged("ChangeConnection")]
        protected bool m_connectToDatabase;

        [SerializeField, ShowIf("m_connectToDatabase"), LabelText("Name"), ValueDropdown("GetIDs"), OnValueChanged("UpdateID")]
        protected int m_databaseID = -1;

        [SerializeField, HideIf("m_connectToDatabase"), LabelText("Name"), OnValueChanged("UpdateName")]
        protected string m_customName = "None";

        protected abstract IEnumerable GetIDs();
        protected abstract void UpdateReference();

        private void ChangeConnection()
        {
            if (m_connectToDatabase)
            {
                m_ID = m_databaseID;
                UpdateReference();
            }
            else
            {
                //Find away to not rely on GetInstanceID all the time since it actually changes Value
                m_ID =Mathf.Abs(GetInstanceID());
                m_name = m_customName;
                string assetPath = AssetDatabase.GetAssetPath(GetInstanceID());
                FileUtility.RenameAsset(this, assetPath, "Custom" + GetType().Name);
            }
        }

        private void UpdateID()
        {
            m_ID = m_databaseID;
            UpdateReference();
        }

        private void UpdateName() => m_name = m_customName;

        private void Awake()
        {
            if (m_initialized == false)
            {
                m_ID = Mathf.Abs(GetInstanceID());
                m_name = m_customName;
                m_initialized = true;
            }
        }

        [SerializeField, HideInInspector]
        protected bool m_initialized;
#endif
        public int id => m_ID;
    }
}