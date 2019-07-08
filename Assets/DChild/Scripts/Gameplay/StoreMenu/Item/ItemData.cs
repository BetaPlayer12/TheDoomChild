using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
using DChildEditor;
#endif
namespace DChild.Menu.Item
{
    [CreateAssetMenu(fileName = "ItemData", menuName = "DChild/Database/Item Data")]
    public class ItemData : DatabaseAsset
    {
#if UNITY_EDITOR
        [ShowInInspector, ToggleGroup("m_enableEdit")]
        private bool m_enableEdit;

        protected override IEnumerable GetIDs()
        {
            var list = new ValueDropdownList<int>();
            list.Add("Not Assigned", -1);
            return list;
        }

        protected override void UpdateReference()
        {
            string assetPath = AssetDatabase.GetAssetPath(GetInstanceID());
            if (m_ID != -1)
            {
                var connection = DChildDatabase.GetBestiaryConnection();
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
#endif

        [SerializeField, PreviewField(100, ObjectFieldAlignment.Center), ToggleGroup("m_enableEdit")]
        private Sprite m_icon;
        [SerializeField, MinValue(1), ToggleGroup("m_enableEdit")]
        private int m_quantityLimit;
        [SerializeField, TextArea, ToggleGroup("m_enableEdit")]
        private string m_description;

        public int id { get => m_ID; }
        public string itemName { get => m_name; }
        public Sprite icon { get => m_icon; }
        public int quantityLimit { get => m_quantityLimit; }
        public string description { get => m_description; }
    }
}
