using DChild;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Environment;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using Spine.Unity;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using DChildEditor;
#endif

namespace DChild.Menu.Bestiary
{

    [CreateAssetMenu(fileName = "BestiaryData", menuName = "DChild/Database/Bestiary Data")]
    public class BestiaryData : DatabaseAsset
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
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
#endif 
        #endregion
        [SerializeField, ShowIf("@m_title != string.Empty || m_enableEdit"), ToggleGroup("m_enableEdit")]
        private string m_title;
        [SerializeField, PreviewField(100), ToggleGroup("m_enableEdit")]
        private Sprite m_indexImage;
        [SerializeField, PreviewField(100), ToggleGroup("m_enableEdit")]
        private Sprite m_infoImage;
        [SerializeField, PreviewField, ToggleGroup("m_enableEdit")]
        private Sprite m_sketchImage;
        [SerializeField, PreviewField, ToggleGroup("m_enableEdit")]
        private SkeletonDataAsset m_spineAsset;
        [SerializeField, SpineAnimation(dataField = "m_spineAsset"), Indent, ToggleGroup("m_enableEdit")]
        private string m_idleAnimation;
        [SerializeField, TextArea, ToggleGroup("m_enableEdit")]
        private string m_description;
        //[SerializeField, ValueDropdown("GetLocations", IsUniqueList = true), ToggleGroup("m_enableEdit")]
        [SerializeField,DrawWithUnity]
        private Location[] m_locatedIn;

        public int id { get => m_ID; }
        public string creatureName { get => m_name; }
        public string title => m_title;
        public Sprite indexImage { get => m_indexImage; }
        public Sprite infoImage { get => m_infoImage; }
        public Sprite sketchImage { get => m_sketchImage; }
        public string description { get => m_description; }
        public Location[] locatedIn { get => m_locatedIn; }
        public void SetupSpine(SkeletonAnimation animation)
        {
            animation.skeletonDataAsset = m_spineAsset;
            animation.Initialize(true);
            var track = animation.state.SetAnimation(0, m_idleAnimation, true);
            track.MixDuration = 0;
        }

#if UNITY_EDITOR
        private void OnEnable()
        {
            var connection = DChildDatabase.GetBestiaryConnection();
            connection.Initialize();
            var databaseName = connection.GetNameOf(m_ID);
            connection.Close();
            if (m_connectToDatabase && m_name != databaseName)
            {
                UpdateReference();
            }
        }

        private IEnumerable GetLocations() => Enum.GetValues(typeof(Location)).Cast<Location>();

        [Button, ToggleGroup("m_enableEdit"), HideIf("m_connectToDatabase")]
        private void AddToDatabase()
        {
            var connection = DChildDatabase.GetBestiaryConnection();
            connection.Initialize();
            if (m_ID != -1)
            {
                connection.Insert(m_ID, m_name, m_title, m_description);
                connection.UpdateLocation(m_ID, m_locatedIn);
            }
            connection.Close();
            m_connectToDatabase = true;
            AssetDatabase.SaveAssets();
        }

        [Button, ToggleGroup("m_enableEdit"), ShowIf("m_connectToDatabase")]
        private void SaveToDatabase()
        {
            var connection = DChildDatabase.GetBestiaryConnection();
            connection.Initialize();
            if (m_ID != -1)
            {
                connection.Update(m_ID, m_name, m_title, m_description);
                connection.UpdateLocation(m_ID, m_locatedIn);
            }
            connection.Close();
            AssetDatabase.SaveAssets();
        }

        [Button, ToggleGroup("m_enableEdit"), ShowIf("m_connectToDatabase")]
        private void LoadFromDatabase()
        {
            var connection = DChildDatabase.GetBestiaryConnection();
            connection.Initialize();
            if (m_ID != -1)
            {
                var info = connection.GetInfo(m_ID);
                m_description = info.description;
                m_locatedIn = info.locations;
            }
            connection.Close();

            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }
#endif
    }
}