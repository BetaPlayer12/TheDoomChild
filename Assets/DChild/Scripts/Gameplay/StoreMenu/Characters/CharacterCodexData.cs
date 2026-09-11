using DChild;
using DChild.Gameplay.ArmyBattle;
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
        //[SerializeField]
        //private bool m_enableEdit;


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
        //public void UseDisplayName(bool useName)
        //{
        //    m_useCustomName = useName;
        //}

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


        [SerializeField, Tooltip("used to refer character instead of 'm_name'")]
        private string m_displayName;
        [SerializeField]
        private string m_title;
        [SerializeField]
        private Sprite m_indexImage;
        [SerializeField]
        private Sprite m_infoImage;
        [SerializeField, TextArea]
        private string m_description;
        [SerializeField]
        private bool m_specialCharacter;
        [SerializeField, ShowIf("m_specialCharacter")]
        private bool m_firstInteract;
        [SerializeField, ShowIf("m_specialCharacter")]
        private bool m_doomedKnightComment; 
        [SerializeField, ShowIf("m_specialCharacter")]
        private bool m_secondInteract;

        [SerializeField, TextArea, ShowIf("m_specialCharacter")]
        private string m_doomedKnightRemarks;
        [SerializeField, TextArea, ShowIf("m_specialCharacter")]
        private string m_necroRemarks;

        [SerializeField]
        private CharacterType m_characterType;
        [SerializeField, ShowIf("@m_characterType == CharacterType.Army")]
        private ArmyCharacterData m_armyData;


        public int id { get => m_ID; }
        public string characterName => m_displayName;
        public string title => m_title;
        public Sprite indexImage { get => m_indexImage; }
        public Sprite infoImage { get => m_infoImage; }
        public string description { get => m_description; }

        public bool specialCharacter => m_specialCharacter;
        public bool firstInteract { get => m_firstInteract; set { m_firstInteract = value; } }
        public bool doomedKnightComment { get => m_doomedKnightComment; set { m_doomedKnightComment = value; } }
        public bool secondInteract { get => m_secondInteract; set { m_secondInteract = value; } }

        public string doomedknightRemarks => m_doomedKnightRemarks;
        public string necroRemarks => m_necroRemarks;

        public CharacterType characterType => m_characterType;
        public ArmyCharacterData armyData => m_armyData;

        //[SerializeField, ValueDropdown("GetLocations", IsUniqueList = true), ToggleGroup("m_enableEdit")]

        [SerializeField, FoldoutGroup("File Utility")]
        private string m_projectName;

        public string projectName => m_projectName;

        public string indexName => m_displayName;

#if UNITY_EDITOR
        [Button, FoldoutGroup("File Utility")]
        private void UpdateFileNames()
        {
            UpdateSpriteName(m_indexImage, " Index");
            UpdateSpriteName(m_infoImage, " Image");

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

        /// <summary>
        /// should only be used in toolkits
        /// </summary>
        /// <param name="value"></param>
        public void SetName(string value)
        {
            m_customName = value;
            m_displayName = value;
        }
#endif
    }
}

