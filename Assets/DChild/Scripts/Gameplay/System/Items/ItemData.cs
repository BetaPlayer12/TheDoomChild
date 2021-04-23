using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
using DChildEditor;
#endif
namespace DChild.Gameplay.Items
{

    [CreateAssetMenu(fileName = "ItemData", menuName = "DChild/Database/Item Data")]
    public class ItemData : DatabaseAsset
    {
        #region EDITOR

#if UNITY_EDITOR
        [ShowInInspector, ToggleGroup("m_enableEdit")]
        private bool m_enableEdit;

        protected virtual string fileSuffix => "Data";

        protected override IEnumerable GetIDs()
        {
            var connection = DChildDatabase.GetItemConnection();
            connection.Initialize();
            var infoList = connection.GetAllInfo();
            connection.Close();

            var list = new ValueDropdownList<int>();
            list.Add("Not Assigned", -1);
            for (int i = 0; i < infoList.Length; i++)
            {
                var info = infoList[i];
                list.Add(info.name, info.ID);
            }
            return list;
        }

        protected override void UpdateReference()
        {
            string assetPath = AssetDatabase.GetAssetPath(GetInstanceID());
            if (m_ID != -1)
            {
                var connection = DChildDatabase.GetItemConnection();
                connection.Initialize();
                var databaseName = connection.GetNameOf(m_ID);
                if (connection.GetNameOf(m_ID) != m_name)
                {
                    m_name = databaseName;
                    var fileName = m_name.Replace(" ", string.Empty);
                    fileName += fileSuffix;
                    FileUtility.RenameAsset(this, assetPath, fileName);
                }
                connection.Close();
            }
            else
            {
                m_name = "Not Assigned";
                FileUtility.RenameAsset(this, assetPath, "UnassignedData");
            }
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        [Button, ToggleGroup("m_enableEdit"), HideIf("m_connectToDatabase")]
        private void InsertToDatabase()
        {
            var connection = DChildDatabase.GetItemConnection();
            connection.Initialize();
            m_ID = connection.Insert(Mathf.Abs(m_ID), m_name, m_description, m_quantityLimit, m_cost);
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
                EditorUtility.SetDirty(this);
                AssetDatabase.SaveAssets();
            }
        }


        [Button, ToggleGroup("m_enableEdit"), ShowIf("m_connectToDatabase")]
        private void SaveToDatabase()
        {
            var connection = DChildDatabase.GetItemConnection();
            connection.Initialize();
            connection.Update(m_ID, m_description, m_quantityLimit, m_cost);
            connection.Close();
        }

        [Button, ToggleGroup("m_enableEdit"), ShowIf("m_connectToDatabase")]
        private void LoadFromDatabase()
        {
            var connection = DChildDatabase.GetItemConnection();
            connection.Initialize();
            var info = connection.GetInfoOf(m_ID);
            m_description = info.description;
            m_quantityLimit = info.quantityLimit;
            m_cost = info.cost;
            connection.Close();

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        [Button, ToggleGroup("m_enableEdit")]
        private void UpdateSelf()
        {
            if (m_connectToDatabase)
            {
                var connection = DChildDatabase.GetItemConnection();
                connection.Initialize();
                var databaseName = connection.GetNameOf(m_ID);
                if (connection.GetNameOf(m_ID) != m_name)
                {
                    m_name = databaseName;
                }
                connection.Close();
            }
            else
            {
                m_name = m_customName;
            }
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
#endif 
        #endregion

        [SerializeField, ToggleGroup("m_enableEdit")]
        private ItemCategory m_category;
        [SerializeField, PreviewField(100, ObjectFieldAlignment.Center), ToggleGroup("m_enableEdit")]
        private Sprite m_icon;
        [SerializeField, MinValue(1), ToggleGroup("m_enableEdit")]
        private int m_quantityLimit;
        [SerializeField, MinValue(0), ToggleGroup("m_enableEdit")]
        private int m_cost;
        [SerializeField, TextArea, ToggleGroup("m_enableEdit")]
        private string m_description;
        [SerializeField, ToggleGroup("m_enableEdit")]
        private bool m_canBeSold = true;

        public int id { get => m_ID; }
        public string itemName { get => m_name; }
        public ItemCategory category => m_category;

        public Sprite icon { get => m_icon; }
        public int quantityLimit { get => m_quantityLimit; }
        public int cost { get => m_cost; }
        public string description { get => m_description; }
        public bool canBeSold => m_canBeSold;
        public virtual bool hasInfiniteUses => false;
    }
}
