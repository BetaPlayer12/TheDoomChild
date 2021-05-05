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
        private Dictionary<IllusionPlatform, string> m_labelPair;
        private GUIStyle m_labelStyle;

        private void OnSceneGUI()
        {
            var listProp = Tree.GetPropertyAtUnityPath("m_list");
            var list = (IntervalIllusionPlatformHandle.Info[])listProp.ValueEntry.WeakSmartValue;
            m_labelPair.Clear();


            for (int i = 0; i < list.Length; i++)
            {
                var info = list[i];
                var platform = info.platform;
                if (platform != null)
                {
                    if (m_labelPair.ContainsKey(platform))
                    {
                        m_labelPair[platform] += $"\n{i + 1}";
                    }
                    else
                    {
                        m_labelPair.Add(platform, $"{i + 1}");
                    }
                }
            }

            foreach (var platform in m_labelPair.Keys)
            {
                Handles.Label(platform.transform.position, m_labelPair[platform], m_labelStyle);
            }
        }

        private void Awake()
        {
            m_labelPair = new Dictionary<IllusionPlatform, string>();
            m_labelStyle = new GUIStyle();
            m_labelStyle.normal.textColor = Color.green;
            m_labelStyle.fontSize = 30;
            m_labelStyle.contentOffset = new Vector2(-5, -20f);
        }
    }

}