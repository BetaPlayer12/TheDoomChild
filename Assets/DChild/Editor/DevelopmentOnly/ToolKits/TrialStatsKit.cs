using DChild.Gameplay;
using DChild.Gameplay.Characters.AI;
using DChild.Gameplay.Characters.Enemies;
using DChild.Gameplay.Combat;
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
    public class TrialStatsKit 
    {

#if UNITY_EDITOR
        //[MenuItem("Tools/Kit/TrialStatKit")]
        //private static void open()
        //{
        //    var window = GetWindow<TrialStatsKit>();
        //    window.position = GUIHelper.GetEditorWindowRect().AlignCenter(1920, 1080);
        //    window.PopulateList();
        //}


        [TableList(DrawScrollView = true, HideToolbar = true,AlwaysExpanded = true)]
        public List<MinionStatsKit> m_characterStatsList = new List<MinionStatsKit>();

        [Serializable]
        public class MinionStatsKit
        {
            [TableColumnWidth(200,false)]
            [SerializeField, VerticalGroup("GameObjects"), PropertyOrder(98), HideLabel]
            private GameObject m_minionPrefab;

            [SerializeField]
            [TableColumnWidth(180), PropertyOrder(99), HorizontalGroup("CharacterStats Data"), HideLabel]
            private CharacterStatsData m_statData;

            [SerializeField]
            [TableColumnWidth(180,false), PropertyOrder(100), HideLabel,HideInInspector, HideIf("@m_attackerData == null")]
            private AttackData m_attackerData;

            private bool hasNoCharacterStatData => m_statData == null;

            [SerializeField, HorizontalGroup("Health"), TableColumnWidth(100, false), PropertyOrder(100), PropertySpace, HideLabel, HideIf("@hasNoCharacterStatData")]
            private int m_healthValue;

            [SerializeField, HorizontalGroup("Health"), TableColumnWidth(180,false), PropertyOrder(100), ProgressBar(0, 100, ColorMember = "HealthColor", DrawValueLabel = false), HideLabel, HideIf("@m_statData == null")]
            private int m_health = 100;


            [SerializeField, HorizontalGroup("Damage"), TableColumnWidth(100, false), PropertyOrder(100), PropertySpace, HideLabel, HideIf("@hasNoCharacterStatData")]
            private int m_damageValue;

            [SerializeField,HorizontalGroup("Damage"), TableColumnWidth(180,false), PropertyOrder(100), ProgressBar(0, 100, ColorMember = "DamageColor", DrawValueLabel = false), HideLabel, HideIf("@m_statData == null")]
            private int m_damage = 50;


            [SerializeField, HorizontalGroup("Physical Resistance"), TableColumnWidth(100, false), PropertyOrder(100), PropertySpace, HideLabel, HideIf("@hasNoCharacterStatData")]
            private int m_physicalResistanceValue;

            [SerializeField,HorizontalGroup("Physical Resistance"), TableColumnWidth(180,false), PropertyOrder(100), ProgressBar(0, 100, ColorMember = "AttackResistanceColor", DrawValueLabel = false), HideLabel, HideIf("@m_statData == null")]
            private int m_physicalResistance = 80;


            [SerializeField, HorizontalGroup("Status Effect Resistance"), TableColumnWidth(100, false), PropertyOrder(100), PropertySpace, HideLabel, HideIf("@hasNoCharacterStatData")]
            private int m_statusEffectResistanceValue;

            [SerializeField, HorizontalGroup("Status Effect Resistance"), TableColumnWidth(180,false), PropertyOrder(100), ProgressBar(0, 100, ColorMember = "StatusResistanceColor", DrawValueLabel = false), HideLabel, HideIf("@m_statData == null")]
            private int m_statusResistance = 30;
            

            [TableColumnWidth(300)]
            [SerializeField, Button("Create CharacterStats Data"),HorizontalGroup("CharacterStats Data"), PropertyOrder(99), ShowIf("@hasNoCharacterStatData")]
            private void AssignNewCharacterDataToPrefab()
            {
                string fileName = $"{m_minionPrefab.name}_CSD";
                string assetPath = AssetDatabase.GetAssetPath(m_minionPrefab);
                m_statData = CreateCharacterStatsData(fileName, assetPath);
                m_minionPrefab.GetComponent<ICombatAIBrain>().statsData = m_statData;
                EditorUtility.SetDirty(m_minionPrefab);
                AssetDatabase.SaveAssets();
            }

            private CharacterStatsData CreateCharacterStatsData(string fileName,string assetPath)
            {
                CharacterStatsData asset = CharacterStatsData.CreateInstance<CharacterStatsData>();
                string name = UnityEditor.AssetDatabase.GenerateUniqueAssetPath($"{assetPath}{fileName}.asset");
                AssetDatabase.CreateAsset(asset, name);
                return AssetDatabase.LoadAssetAtPath<CharacterStatsData>(name);             
            }

            public GameObject theGameObject { get { return m_minionPrefab; } set { m_minionPrefab = value; } }
            public CharacterStatsData theStatData { get { return m_statData; } set { m_statData = value; } }
            public AttackData theAttackerData { get { return m_attackerData; } set { m_attackerData = value; } }


            private Color HealthColor = new Color32(24, 231, 119, 255);



            private Color DamageColor = Color.red;

            private Color AttackResistanceColor = Color.white;

            private Color StatusResistanceColor = Color.green;

        }

        public void PopulateList()
        {
            EditorUtility.DisplayProgressBar("CharacterStats Progress", $"Gather All GameObjects", 0);
            var prefabList = new List<GameObject>(AssetDatabase.FindAssets("t:GameObject").Select(guid => AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(guid))));
            for (int i = 0; i < prefabList.Count; i++)
            {
                if (prefabList[i].TryGetComponent(out ICombatAIBrain statData))
                {
                    if (prefabList[i] != prefabList[i].TryGetComponent(out Boss boss))
                    {
                        var instance = new MinionStatsKit();
                        instance.theGameObject = prefabList[i];
                        instance.theStatData = statData.statsData;
                        m_characterStatsList.Add(instance);
                    }
                }
               
               
                EditorUtility.DisplayProgressBar("CharacterStats Progress", $"Filtering{i}/{m_characterStatsList.Count}", i / (float)m_characterStatsList.Count);
            }
            EditorUtility.ClearProgressBar();
        }

#endif

    }
}
