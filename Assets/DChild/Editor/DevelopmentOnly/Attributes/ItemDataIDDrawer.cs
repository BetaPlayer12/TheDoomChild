using DChild;
using DChild.Gameplay.Items;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace DChildEditor
{
    public sealed class ItemDataIDDrawer : OdinAttributeDrawer<ItemDataIDAttribute, int>
    {
        private ItemData m_itemData;

        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (m_itemData == null)
            {
                var filePaths = AssetDatabase.FindAssets("t:ItemData");
                for (int i = 0; i < filePaths.Length; i++)
                {
                    var itemData = AssetDatabase.LoadAssetAtPath<ItemData>(AssetDatabase.GUIDToAssetPath(filePaths[i]));
                    if (ValueEntry.SmartValue == itemData.id)
                    {
                        m_itemData = itemData;
                        break;
                    }
                }
            }

            if (m_itemData == null)
            {
                SirenixEditorGUI.WarningMessageBox($"Item Data with ID:{ValueEntry.SmartValue} not found");
            }
            EditorGUI.BeginChangeCheck();
            m_itemData = (ItemData)SirenixEditorFields.UnityObjectField(label, m_itemData, typeof(ItemData), false);

            if (EditorGUI.EndChangeCheck())
            {
                if (m_itemData == null)
                {
                    ValueEntry.SmartValue = -1;
                }
                else
                {
                    ValueEntry.SmartValue = m_itemData.id;
                }
                ValueEntry.Property.Tree.ApplyChanges();
            }
        }
    }
}
