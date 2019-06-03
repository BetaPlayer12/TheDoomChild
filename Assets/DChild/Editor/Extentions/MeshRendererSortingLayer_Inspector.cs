
using UnityEngine;
using UnityEditor;
using System.Collections;
using UnityEditorInternal;
using System;
using System.Reflection;

namespace HolysoftEditor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MeshRenderer))]
    public class MeshRendererSortingLayersEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            MeshRenderer renderer = target as MeshRenderer;

            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            var sortingLayers = GetSortingLayerNames();
            var index = GetCurrentIndex(SortingLayer.IDToName(renderer.sortingLayerID), sortingLayers);
            index = EditorGUILayout.Popup("Sorting Layer Name", index, sortingLayers);
            if (EditorGUI.EndChangeCheck())
            {
                var name = sortingLayers[index];
                renderer.sortingLayerID = SortingLayer.NameToID(name);
                renderer.sortingLayerName = name;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            int order = EditorGUILayout.IntField("Sorting Order", renderer.sortingOrder);
            if (EditorGUI.EndChangeCheck())
            {
                renderer.sortingOrder = order;
            }
            EditorGUILayout.EndHorizontal();

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