using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Toolkit
{

    [System.Serializable]
    public class TrialLootKit 
    {

       
#if UNITY_EDITOR

        // [MenuItem("Tools/Kit/TrialLootKit")]
        //private static void open()
        //{
        //    var window = GetWindow<TrialLootKit>();
        //    window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        //    window.PopulateList();
        //}


        [TableList(DrawScrollView = true, HideToolbar = true, AlwaysExpanded = true)]
        public List<LootToolKitInfo> m_breakableLootDroppers = new List<LootToolKitInfo>();

        [TableList(DrawScrollView = true, HideToolbar = true, AlwaysExpanded = true)]
        public List<LootToolKitInfo> m_minionLootDroppers = new List<LootToolKitInfo>();

        [TableList(DrawScrollView = true, HideToolbar = true, AlwaysExpanded = true)]
        public List<LootToolKitInfo> m_bossLootDroppers = new List<LootToolKitInfo>();

        [Serializable]
        public class LootToolKitInfo
        {
            [TableColumnWidth(200, false)]
            [SerializeField, VerticalGroup("GameObjects"), PropertyOrder(98), HideLabel]
            private GameObject m_minionPrefab;
            

            [SerializeField]
            [TableColumnWidth(220,false), InlineEditor, PropertyOrder(99), VerticalGroup("Loot Data")]
            private LootData m_loot;

           
            [SerializeField]
            [TableColumnWidth(20), PropertyOrder(100)]
            private int m_count;


            [TableColumnWidth(20), PropertyOrder(102)]
            [SerializeField, TextArea, ReadOnly]
            private string m_result;

       
            [TableColumnWidth(20)]
            [SerializeField, Button("Create Loot Data"), VerticalGroup("Loot Data"), PropertyOrder(99), ShowIf("@m_loot == null")]
            private void AssignNewCharacterDataToPrefab()
            {
                string fileName = $"{m_minionPrefab.name}_CSD";
                string assetPath = AssetDatabase.GetAssetPath(m_minionPrefab);
                m_loot = CreateLootDataForPrefab(fileName, assetPath);
                m_minionPrefab.GetComponent<LootDropper>().lootData = m_loot;
                EditorUtility.SetDirty(m_minionPrefab);
                AssetDatabase.SaveAssets();
            }

            private LootData CreateLootDataForPrefab(string fileName, string assetPath)
            {

                LootData asset = LootData.CreateInstance<LootData>();
                string name = UnityEditor.AssetDatabase.GenerateUniqueAssetPath($"{assetPath}{fileName}.asset");
                AssetDatabase.CreateAsset(asset, name);
                return AssetDatabase.LoadAssetAtPath<LootData>(name);


            }

              [SerializeField]
            [TableColumnWidth(20)]
            [Button("Calculate"), PropertyOrder(101)]
            private void LootChance() { }



            public GameObject theGameObject { get { return m_minionPrefab; } set { m_minionPrefab = value; } }
            public LootData theLootData { get { return m_loot; } set { m_loot = value; } }
            public int count { get { return m_count; } }
        }

       public void PopulateList()
        {

            EditorUtility.DisplayProgressBar("LootDropper Progress", $"Gather All GameObjects", 0);
            var prefabList = new List<GameObject>(AssetDatabase.FindAssets("t:GameObject").Select(guid => AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid))));
            for (int i = 0; i < prefabList.Count; i++)
            {

                if (prefabList[i].TryGetComponent(out LootDropper loot))
                {
                    if (prefabList[i].TryGetComponent(out ICombatAIBrain statData))
                    {
                       if(!prefabList[i].TryGetComponent(out Boss boss))
                        {
                            var instance = new LootToolKitInfo();
                            instance.theGameObject = loot.gameObject;
                            instance.theLootData = loot.lootData;
                            m_minionLootDroppers.Add(instance);
                        }
                    }       
                  
                }
                EditorUtility.DisplayProgressBar("LootDropper Progress", $"Filtering{i}/{m_minionLootDroppers.Count}", i / (float)m_minionLootDroppers.Count);
            }
            EditorUtility.ClearProgressBar();
        }

        public void PopulateBreakableList()
        {

            EditorUtility.DisplayProgressBar("LootDropper Progress", $"Gather All GameObjects", 0);
            var prefabList = new List<GameObject>(AssetDatabase.FindAssets("t:GameObject").Select(guid => AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid))));
            for (int i = 0; i < prefabList.Count; i++)
            {

                if (prefabList[i].TryGetComponent(out LootDropper loot))
                {
                    if (!prefabList[i].TryGetComponent(out ICombatAIBrain statData))
                    {
                        var instance = new LootToolKitInfo();
                        instance.theGameObject = loot.gameObject;
                        instance.theLootData = loot.lootData;
                        m_breakableLootDroppers.Add(instance);
                    }

                }
                EditorUtility.DisplayProgressBar("LootDropper Progress", $"Filtering{i}/{m_breakableLootDroppers.Count}", i / (float)m_breakableLootDroppers.Count);
            }
            EditorUtility.ClearProgressBar();
        }

        public void PopulateBossLootList()
        {

            EditorUtility.DisplayProgressBar("LootDropper Progress", $"Gather All GameObjects", 0);
            var prefabList = new List<GameObject>(AssetDatabase.FindAssets("t:GameObject").Select(guid => AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid))));
            for (int i = 0; i < prefabList.Count; i++)
            {

                if (prefabList[i].TryGetComponent(out LootDropper loot))
                {
                    if (prefabList[i].TryGetComponent(out Boss statData))
                    {
                        var instance = new LootToolKitInfo();
                        instance.theGameObject = loot.gameObject;
                        instance.theLootData = loot.lootData;
                        m_bossLootDroppers.Add(instance);
                    }

                }
                EditorUtility.DisplayProgressBar("LootDropper Progress", $"Filtering{i}/{m_bossLootDroppers.Count}", i / (float)m_bossLootDroppers.Count);
            }
            EditorUtility.ClearProgressBar();
        }


#endif
    }
}