using DChild.Gameplay.Puzzles;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay
{
    [CustomEditor(typeof(HauntTeleport)), DisallowMultipleComponent]
    public class HauntTeleport_Inspector : OdinEditor
    {
        struct ChangeInfo
        {
            public int index;
            public Vector2 position;

            public ChangeInfo(int index, Vector2 position)
            {
                this.index = index;
                this.position = position;
            }
        }

        private List<ChangeInfo> m_changeList = new List<ChangeInfo>();
        private GUIStyle m_labelStyle = new GUIStyle();

        private void OnSceneGUI()
        {
            m_changeList.Clear();
            var property = Tree.GetPropertyAtPath("m_info");
            var info = (Dictionary<int, Vector2>)property.ValueEntry.WeakSmartValue;
            m_labelStyle.normal.textColor = Color.red;
            EditorGUI.BeginChangeCheck();
            foreach (var key in info.Keys)
            {
                var position = info[key];
                var newPosition = (Vector2)Handles.FreeMoveHandle(position, Quaternion.identity, 2, Vector3.one * 0.01f, Handles.DotHandleCap);
                Handles.Label(position, key.ToString(), m_labelStyle);
                if (position != newPosition)
                {
                    m_changeList.Add(new ChangeInfo(key, newPosition));
                }
            }
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "HauntTeleport Info Change");
                for (int i = 0; i < m_changeList.Count; i++)
                {
                    var changeInfo = m_changeList[i];
                    info[changeInfo.index] = changeInfo.position;
                }
            }
        }
    }
}