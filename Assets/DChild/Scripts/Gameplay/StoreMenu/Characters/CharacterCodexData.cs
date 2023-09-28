using DChild;
using DChild.Gameplay.Environment;
using DChild.Menu.Codex;
using DChildEditor;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


 namespace DChild.Codex.Characters
{
    [CreateAssetMenu(fileName = "CharacterCodexData", menuName = "DChild/Database/Character Codex Data")]
    public class CharacterCodexData : DatabaseAsset, ICodexIndexInfo, ICodexInfo
    {

        #region EditorOnly
#if UNITY_EDITOR
        [SerializeField]
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
            //if (m_ID != -1)
            //{
            //    var connection = DChildDatabase.GetBestiaryConnection();
            //    connection.Initialize();
            //    var databaseName = connection.GetNameOf(m_ID);
            //    if (connection.GetNameOf(m_ID) != m_name)
            //    {
            //        m_name = databaseName;
            //        var fileName = m_name.Replace(" ", string.Empty);
            //        fileName += "_CCD";
            //        FileUtility.RenameAsset(this, assetPath, fileName);
            //    }
            //    connection.Close();
            //}
            //else
            //{
            //    m_name = "Not Assigned";
            //    FileUtility.RenameAsset(this, assetPath, "UnassignedData");
            //}
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
        public void SetStoreNotes(string storeNotes)
        {
            m_storeNotes = storeNotes;
        }
        public void SetInfoImage(Sprite infoImage)
        {
            m_infoImage = infoImage;
        }

#endif
        #endregion
        [SerializeField]
        private bool m_useCustomName;
        [SerializeField]
        private string m_customName;
        [SerializeField]
        private string m_title;
        [SerializeField]
        private Sprite m_indexImage;
        [SerializeField]
        private Sprite m_infoImage;
        [SerializeField]
        private Sprite m_sketchImage;
        [SerializeField, TextArea]
        private string m_description;
        [SerializeField, TextArea]
        private string m_storeNotes;


        public int id { get => m_ID; }
        public string creatureName { get => m_useCustomName ? m_customName : m_name; }
        public string title => m_title;
        public Sprite indexImage { get => m_indexImage; }
        public Sprite infoImage { get => m_infoImage; }
        public Sprite sketchImage { get => m_sketchImage; }
        public string description { get => m_description; }
        public string storeNotes { get => m_storeNotes; }
        public Location[] locatedIn { get => m_locatedIn; }


        //[SerializeField, ValueDropdown("GetLocations", IsUniqueList = true), ToggleGroup("m_enableEdit")]
        [SerializeField, DrawWithUnity]
        private Location[] m_locatedIn;

        [SerializeField, FoldoutGroup("File Utility")]
        private string m_projectName;

        public string projectName => m_projectName;

        public string indexName => throw new System.NotImplementedException();

#if UNITY_EDITOR
        [Button, FoldoutGroup("File Utility")]
        private void UpdateFileNames()
        {
            UpdateSpriteName(m_indexImage, " Index");
            UpdateSpriteName(m_infoImage, " Image");
            UpdateSpriteName(m_sketchImage, " Sketch");

            string assetPath = AssetDatabase.GetAssetPath(GetInstanceID());
            var fileName = m_projectName.Replace(" ", string.Empty);
            fileName += "_CCD";
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

