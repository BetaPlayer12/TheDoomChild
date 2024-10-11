using DChild.Gameplay.Environment;
using DChild.Menu.Codex;
using DChildEditor;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DChild.Codex.LocationCodex
{
    [CreateAssetMenu(fileName = "LocationCodexData", menuName = "DChild/Database/Location Codex Data")]
    public class LocationCodexData : DatabaseAsset, ICodexIndexInfo, ICodexInfo
    {
        #region EditorOnly
#if UNITY_EDITOR
        [ShowInInspector, ToggleGroup("m_enableEdit")]
        private bool m_enableEdit;


        protected override IEnumerable GetIDs()
        {
            var connection = DChildDatabase.GetBestiaryConnection();
            connection.Initialize();
            var infoList = connection.GetAllInfo();
            connection.Close();

            var list = new ValueDropdownList<int>();
            list.Add("Not Assigned", -1);
            for (int i = 0; i < infoList.Length; i++)
            {
                list.Add(infoList[i].name, infoList[i].ID);
            }
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
                    fileName += "_LCD";
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

        public void SetDisplayName(string name)
        {
            m_customName = name;
        }
        public void UseDisplayName(bool useName)
        {
            m_useCustomName = useName;
        }
        public void SetTitle(string title)
        {
            m_title = title;
        }
        public void SetDesciption(string desciption)
        {
            m_description = desciption;
        }
        public void SetInfoImage(Sprite infoImage)
        {
            m_infoImage = infoImage;
        }

#endif
        #endregion
        [SerializeField, ToggleGroup("m_enableEdit"), LabelText("Use Display Name")]
        private bool m_useCustomName;
        [SerializeField, ShowIf("m_useCustomName"), ToggleGroup("m_enableEdit"), LabelText("Display Name")]
        private string m_customName;
        [SerializeField, ToggleGroup("m_enableEdit")]
        private string m_title;
        [SerializeField, PreviewField(100), ToggleGroup("m_enableEdit")]
        private Sprite m_indexImage;
        [SerializeField, PreviewField(100), ToggleGroup("m_enableEdit")]
        private Sprite m_infoImage;
        [SerializeField, PreviewField, ToggleGroup("m_enableEdit")]
        private Sprite m_sketchImage;
        [SerializeField, TextArea, ToggleGroup("m_enableEdit")]
        private string m_description;

        //[SerializeField, ValueDropdown("GetLocations", IsUniqueList = true), ToggleGroup("m_enableEdit")]
        [SerializeField, DrawWithUnity]
        private Location[] m_locatedIn;

        [SerializeField, FoldoutGroup("File Utility")]
        private string m_projectName;

        public string projectName => m_projectName;

        public string indexName => m_customName;

        public Sprite indexImage => m_indexImage;

        public Location[] locatedIn => m_locatedIn;

        public string description => m_description;

#if UNITY_EDITOR
        [Button, FoldoutGroup("File Utility")]
        private void UpdateFileNames()
        {
            UpdateSpriteName(m_indexImage, " Index");
            UpdateSpriteName(m_infoImage, " Image");
            UpdateSpriteName(m_sketchImage, " Sketch");

            string assetPath = AssetDatabase.GetAssetPath(GetInstanceID());
            var fileName = m_projectName.Replace(" ", string.Empty);
            fileName += "_LCD";
            FileUtility.RenameAsset(this, assetPath, fileName, false);

            void UpdateSpriteName(Sprite sprite, string extention)
            {
                if (sprite)
                {
                    var indexSpriteFilePath = AssetDatabase.GetAssetPath(sprite);
                    FileUtility.RenameAsset<Sprite>(sprite, indexSpriteFilePath, m_projectName + extention, false);
                }
            }
        }
#endif
    }
}

