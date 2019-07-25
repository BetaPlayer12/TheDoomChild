using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay.Characters.Player
{
    public class Skills_Drawer : OdinValueDrawer<Skills>
    {
        private bool m_movementSkillFoldout;

        protected override void DrawPropertyLayout(GUIContent label)
        {
            var skills = ValueEntry.SmartValue;
            SirenixEditorGUI.BeginInlineBox();
            DrawToolbar(label, skills);
            DrawMovementSkills(skills);
            SirenixEditorGUI.EndInlineBox();
        }

        private void DrawMovementSkills(Skills skills)
        {
            var movementSkills = skills.movementSkillEnabled;
            if (movementSkills != null || movementSkills.Length > 0)
            {
                m_movementSkillFoldout = SirenixEditorGUI.Foldout(m_movementSkillFoldout, "Movement Skill");
                if (m_movementSkillFoldout)
                {
                    EditorGUI.indentLevel++;
                    for (int i = 0; i < movementSkills.Length; i++)
                    {
                        movementSkills[i] = EditorGUILayout.Toggle(((MovementSkill)i).ToString(), movementSkills[i]);
                    }
                    EditorGUI.indentLevel--;
                }
            }
        }

        private static void DrawToolbar(GUIContent label, Skills skills)
        {
            var toolBar = SirenixEditorGUI.BeginHorizontalToolbar();
            EditorGUILayout.LabelField(label);
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.ArrowDown))
            {
                skills.Initialize();
            }
            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }
}