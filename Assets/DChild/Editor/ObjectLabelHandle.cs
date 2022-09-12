using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DChildEditor
{
    public class ObjectLabelHandle<T> where T : Component
    {
        protected Dictionary<T, string> m_labelPair;
        protected GUIStyle m_labelStyle;

        public ObjectLabelHandle(Dictionary<T, string> labelPair,GUIStyle labelStyle)
        {
            m_labelPair = labelPair;
            m_labelStyle = labelStyle;
        }

        public virtual void SetObjectsToLabel(params T[] objects)
        {
            m_labelPair.Clear();
            for (int i = 0; i < objects.Length; i++)
            {
                var platform = objects[i];
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
        }

        public void Draw()
        {
            foreach (var toDraw in m_labelPair.Keys)
            {
                Handles.Label(toDraw.transform.position, m_labelPair[toDraw], m_labelStyle);
            }
        }
    }

}