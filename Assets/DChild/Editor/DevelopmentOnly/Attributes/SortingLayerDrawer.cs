using System;
using System.Reflection;
using DChild;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace DChildEditor
{
    public sealed class SortingLayerDrawer : OdinAttributeDrawer<SortingLayerAttribute, int>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            int index = 0;
            var sortingLayers = GetSortingLayerNames();
            index = GetCurrentIndex(SortingLayer.IDToName(ValueEntry.SmartValue), sortingLayers);
            index = EditorGUILayout.Popup(ValueEntry.Property.Name, index, sortingLayers);
            ValueEntry.SmartValue = SortingLayer.NameToID(sortingLayers[index]);
        }

        private static string[] GetSortingLayerNames()
        {
            Type internalEditorUtilityType = typeof(InternalEditorUtility);
            PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
            return (string[])sortingLayersProperty.GetValue(null, new object[0]);
        }

        private int GetCurrentIndex(string value, string[] options)
        {
            for (int i = 0; i < options.Length; i++)
            {
                if (value == options[i])
                {
                    return i;
                }
            }
            return 0;
        }
    }
}
