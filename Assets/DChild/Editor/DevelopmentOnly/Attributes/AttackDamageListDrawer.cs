using DChild.Gameplay.Combat;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay.Combat
{
    [OdinDrawer]
    public sealed class AttackDamageListDrawer : OdinAttributeDrawer<AttackDamageListAttribute, Damage[]>
    {
        private bool m_isVisible;

        protected override void DrawPropertyLayout(IPropertyValueEntry<Damage[]> entry, AttackDamageListAttribute attribute, GUIContent label)
        {
            var smartValue = entry.SmartValue;
            DrawList(entry, attribute, label);

            if (smartValue == null || smartValue.Length == 0)
            {
                smartValue = new Damage[attribute.size];
            }
            else if (smartValue.Length != attribute.size)
            {
                var newArray = new Damage[attribute.size];
                var copySize = newArray.Length < smartValue.Length ? newArray.Length : smartValue.Length;
                for (int i = 0; i < copySize; i++)
                {
                    newArray[i] = smartValue[i];
                }

                if (attribute.forceType)
                {
                }

                entry.SmartValue = newArray;
            }
            else
            {
                var newArray = new Damage[attribute.size];
                Array.Copy(smartValue, newArray, attribute.size);
                if (attribute.forceType)
                {
                }

                entry.SmartValue = newArray;
            }
        }

        private void DrawList(IPropertyValueEntry<Damage[]> entry, AttackDamageListAttribute attribute, GUIContent label)
        {
            if (attribute.forceType)
            {
                DrawReadOnlyList(entry, label);
            }
            else
            {
                CallNextDrawer(entry, label);
            }
        }

        private void DrawReadOnlyList(IPropertyValueEntry<Damage[]> list, GUIContent label)
        {
            var smartValue = list.SmartValue;
            SirenixEditorGUI.BeginBox(label);
            m_isVisible = SirenixEditorGUI.Foldout(m_isVisible, label);
            if (m_isVisible)
            {
                SirenixEditorGUI.BeginVerticalList();
                SirenixEditorGUI.BeginListItem();
                for (int i = 0; i < smartValue.Length; i++)
                {
                    Draw(smartValue[i]);
                }
                SirenixEditorGUI.EndListItem();
                SirenixEditorGUI.EndListItem();
            }
            SirenixEditorGUI.EndBox();
        }

        private void Draw(Damage attackDamage)
        {
            SirenixEditorGUI.BeginListItem();
            GUI.enabled = false;
            EditorGUILayout.EnumPopup("Type: ", attackDamage.type);
            GUI.enabled = true;
            var value = EditorGUILayout.IntField("Value: ", attackDamage.value);
            attackDamage.value = value < 0 ? 0 : value;
            SirenixEditorGUI.EndListItem();
        }
    }
}