using DChild.Gameplay.Environment;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay.Environment
{
    [CustomEditor(typeof(IntervalIllusionPlatformHandle))]
    public class IntervalIllusionPlatformHandle_Inspector : OdinEditor
    {
        private ObjectLabelHandle<IllusionPlatform> m_labelHandle;

        private void OnSceneGUI()
        {
            var listProp = Tree.GetPropertyAtUnityPath("m_list");
            var list = (IllusionPlatform[])listProp.ValueEntry.WeakSmartValue;
            m_labelHandle.SetObjectsToLabel(list);
            m_labelHandle.Draw();
        }

        private void Awake()
        {
            var labelStyle = new GUIStyle();
            labelStyle.normal.textColor = Color.green;
            labelStyle.fontSize = 30;
            labelStyle.contentOffset = new Vector2(-5, -20f);
            m_labelHandle = new IllusionPlatformLabelHandle(new Dictionary<IllusionPlatform, string>(), labelStyle);
        }
    }

}