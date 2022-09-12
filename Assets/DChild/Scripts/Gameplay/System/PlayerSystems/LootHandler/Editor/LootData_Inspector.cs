using DChild.Gameplay.Systems;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay
{
    [CustomEditor(typeof(LootData))]
    public class LootData_Inspector : OdinEditor
    {
        private bool m_showSummary;
        private bool m_showEdit;

        protected override void DrawTree()
        {
            //TabGroup Header
            SirenixEditorGUI.BeginHorizontalToolbar();
            EditorGUI.BeginChangeCheck();
            m_showSummary = SirenixEditorGUI.ToolbarTab(m_showSummary, "Summary");
            if (EditorGUI.EndChangeCheck())
            {
                m_showEdit = false;
            }
            EditorGUI.BeginChangeCheck();
            m_showEdit = SirenixEditorGUI.ToolbarTab(m_showEdit, "Edit");
            if (EditorGUI.EndChangeCheck())
            {
                m_showSummary = false;
            }
            SirenixEditorGUI.EndHorizontalToolbar();


            SirenixEditorGUI.BeginBox();
            if (m_showSummary)
            {
                var lootData = target as LootData;
                if (lootData.data != null)
                {
                    SirenixEditorGUI.BeginBox("Summary");
                    lootData.data.DrawDetails(false);
                    SirenixEditorGUI.EndBox();
                }
            }
            else if (m_showEdit)
            {
                base.DrawTree();
            }
            SirenixEditorGUI.EndBox();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            var lootData = target as LootData;
            if (lootData.data == null)
            {
                m_showSummary = false;
                m_showEdit = true;
            }
            else
            {
                m_showSummary = true;
                m_showEdit = false;
            }
        }
    }

}