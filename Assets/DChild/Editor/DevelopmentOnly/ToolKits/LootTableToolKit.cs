using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector.Editor;
using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using UnityEditor;

namespace DChildEditor.Toolkit
{
    public class LootTableToolKit : OdinEditorWindow
    {
        private static LootTableToolKit m_instance;

        [MenuItem("Tools/Kit/Loot Table ToolKit")]
        private static void OpenWindow()
        {
            m_instance = EditorWindow.GetWindow<LootTableToolKit>();
            m_instance.Show();
        }

        [System.Serializable]
        public class Info
        {
            [ReadOnly]
            public string name;
            [DisplayAsString, OnInspectorGUI("DrawDetails")]
            public string lootDetails;
            [ReadOnly]
            public LootData loot;

            private void DrawDetails() => loot.data.DrawDetails(false);
        }

        [SerializeField, AssetSelector(Paths = "Assets/DChild/Objects", IsUniqueList = true),
            ListDrawerSettings(DraggableItems = false, HideRemoveButton = true), OnValueChanged("OnEnemyLootChanged")]
        private LootData[] m_enemyLoot;

        [SerializeField, TableList(AlwaysExpanded = true, HideToolbar = true,IsReadOnly = true),
            ListDrawerSettings(DraggableItems = false, HideAddButton = true, HideRemoveButton = true)]
        private Info[] m_infos;

        protected override void DrawEditors()
        {
            base.DrawEditors();
        }

        private void OnEnemyLootChanged()
        {
            m_infos = new Info[m_enemyLoot.Length];
            for (int i = 0; i < m_infos.Length; i++)
            {
                var enemyLoot = m_enemyLoot[i];
                m_infos[i] = new Info
                {
                    loot = enemyLoot,
                    name = enemyLoot.name.Replace("LootData", string.Empty),
                    lootDetails = ""
                };
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

        }
    }
}