using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
using DChildEditor;
#endif
namespace DChild.Gameplay.Inventories
{

    [CreateAssetMenu(fileName = "ItemData", menuName = "DChild/Database/Item Data")]
    public class ItemData : DatabaseAsset
    {
#if UNITY_EDITOR
        [ShowInInspector, ToggleGroup("m_enableEdit")]
        private bool m_enableEdit;

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
                    fileName += "Data";
                    FileUtility.RenameAsset(this, assetPath, fileName);
                }
                connection.Close();
            }
            else
            {
                m_name = "Not Assigned";
                FileUtility.RenameAsset(this, assetPath, "UnassignedData");
            }
            AssetDatabase.SaveAssets();
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
        }
#endif

        [SerializeField, PreviewField(100, ObjectFieldAlignment.Center), ToggleGroup("m_enableEdit")]
        private Sprite m_icon;
        [SerializeField, MinValue(1), ToggleGroup("m_enableEdit")]
        private int m_quantityLimit;
        [SerializeField, MinValue(0), ToggleGroup("m_enableEdit")]
        private int m_cost;
        [SerializeField, TextArea, ToggleGroup("m_enableEdit")]
        private string m_description;

        public int id { get => m_ID; }
        public string itemName { get => m_name; }
        public Sprite icon { get => m_icon; }
        public int quantityLimit { get => m_quantityLimit; }
        public int cost { get => m_cost; }
        public string description { get => m_description; }
    }
}
