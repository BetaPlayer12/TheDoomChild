using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using Sirenix.Serialization;
using System;
#if UNITY_EDITOR
using UnityEditor;
using DChildEditor;
#endif

namespace DChild.Gameplay.Characters.Players.SoulSkills
{
    [ShowOdinSerializedPropertiesInInspector]
    [CreateAssetMenu(fileName = "SoulSkill", menuName = "DChild/Database/Soul Skill")]
    public class SoulSkill : DatabaseAsset
    {
        #region EditorOnly
#if UNITY_EDITOR
        [ShowInInspector, ToggleGroup("m_enableEdit")]
        private bool m_enableEdit;

        protected override IEnumerable GetIDs()
        {
            var list = DChildUtility.GetSoulSkills();
            list.Insert(0, new ValueDropdownItem<int>("Not Assigned", -1));
            return list;
        }

        protected override void UpdateReference()
        {
            string assetPath = AssetDatabase.GetAssetPath(GetInstanceID());
            if (m_ID != -1)
            {
                var connection = DChildDatabase.GetSoulSkillConnection();
                connection.Initialize();
                var databaseName = connection.GetNameOf(m_ID);
                if (connection.GetNameOf(m_ID) != m_name)
                {
                    m_name = databaseName;
                    var fileName = m_name.Replace(" ", string.Empty);
                    fileName += "Skill";
                    FileUtility.RenameAsset(this, assetPath, fileName);
                }
                connection.Close();
            }
            else
            {
                m_name = "Not Assigned";
                FileUtility.RenameAsset(this, assetPath, "UnassignedSkill");
            }
            AssetDatabase.SaveAssets();
        }


        [Button, ToggleGroup("m_enableEdit"), ShowIf("m_connectToDatabase")]
        private void SaveToDatabase()
        {
            var connection = DChildDatabase.GetSoulSkillConnection();
            connection.Initialize();
            connection.Update(m_ID, m_capacity, m_description);
            connection.Close();
        }

        [Button, ToggleGroup("m_enableEdit"), ShowIf("m_connectToDatabase")]
        private void LoadFromDatabase()
        {
            var connection = DChildDatabase.GetSoulSkillConnection();
            connection.Initialize();
            var info = connection.GetInfoOf(m_ID);
            m_capacity = info.capacity;
            m_description = info.description;
            connection.Close();
        }

        [Button, ToggleGroup("m_enableEdit"), HideIf("m_connectToDatabase")]
        private void InsertToDatabase()
        {
            var connection = DChildDatabase.GetSoulSkillConnection();
            connection.Initialize();
            m_ID = connection.Insert(Mathf.Abs(m_ID), m_name, m_description, m_capacity);
            m_databaseID = m_ID;
            m_customName = m_name;
            m_connectToDatabase = true;
            connection.Close();

            string assetPath = AssetDatabase.GetAssetPath(GetInstanceID());
            if (m_ID != -1)
            {
                var fileName = m_name.Replace(" ", string.Empty);
                fileName += "Data";
                FileUtility.RenameAsset(this, assetPath, fileName);
                AssetDatabase.SaveAssets();
            }
        }
#endif
        #endregion
        [SerializeField, ToggleGroup("m_enableEdit")]
        private Sprite m_icon;
        [SerializeField, MinValue(1), ToggleGroup("m_enableEdit")]
        private int m_capacity = 1;
        [SerializeField, TextArea, ToggleGroup("m_enableEdit")]
        private string m_description;
        [NonSerialized, OdinSerialize, ToggleGroup("m_enableEdit")]
        private ISoulSkillModule[] m_modules = new ISoulSkillModule[1];

        public Sprite icon => m_icon;
        public int capacity => m_capacity;
        public string description => m_description;

        public void AttachTo(IPlayer player)
        {
            for (int i = 0; i < m_modules.Length; i++)
            {
                m_modules[i].AttachTo(GetInstanceID(), player);
            }
        }

        public void DetachFrom(IPlayer player)
        {
            for (int i = 0; i < m_modules.Length; i++)
            {
                m_modules[i].DetachFrom(GetInstanceID(), player);
            }
        }

#if UNITY_EDITOR
        private void OnEnable()
        {
            var connection = DChildDatabase.GetSoulSkillConnection();
            connection.Initialize();
            var databaseName = connection.GetNameOf(m_ID);
            connection.Close();
            //if (m_connectToDatabase && m_name != databaseName)
            //{
            //    UpdateReference();
            //}
        }
#endif

        public string name => m_name;
    }
}