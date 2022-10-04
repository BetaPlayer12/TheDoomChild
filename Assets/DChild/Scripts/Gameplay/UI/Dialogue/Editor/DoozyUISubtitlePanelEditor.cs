using DChild.UI;
using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay.Dialogue
{
    [CustomEditor(typeof(DoozyUISubtitlePanel))]
    public class DoozyUISubtitlePanelEditor : StandardUISubtitlePanelEditor
    {
        /// <summary>
        /// Have to do this so that it is inline with PixelCrusher's Architecture
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("m_continueUIButton"), true);
            serializedObject.ApplyModifiedProperties();
            base.OnInspectorGUI();

        }

    }
}