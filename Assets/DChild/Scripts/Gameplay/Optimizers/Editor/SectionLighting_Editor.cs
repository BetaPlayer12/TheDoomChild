using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DChild.Gameplay.Optimization.Lights
{
    [CustomEditor(typeof(SectionLighting)), DisallowMultipleComponent]
    public class SectionLighting_Editor : OdinEditor
    {
        private Vector2 m_rightExtent;
        private Vector2 m_leftExtent;
        private Vector2 m_topExtent;
        private Vector2 m_bottomExtent;
        private Vector2 m_axisRestriction;

        private const float HANDLESIZE = 5f;
        private static Vector3 HANDLESNAP => Vector2.one * 0.5f;

        private void UpdateHandleFields(SectionLighting target)
        {
            var bounds = new Bounds(target.boundCenter, target.boundSize);
            m_rightExtent = bounds.center;
            m_rightExtent.x += bounds.extents.x;

            m_leftExtent = bounds.center;
            m_leftExtent.x -= bounds.extents.x;

            m_topExtent = bounds.center;
            m_topExtent.y += bounds.extents.y;

            m_bottomExtent = bounds.center;
            m_bottomExtent.y -= bounds.extents.y;

            m_axisRestriction = bounds.center;
        }

        private void DrawHandles()
        {
            m_rightExtent = Handles.FreeMoveHandle(m_rightExtent, Quaternion.identity, HANDLESIZE, HANDLESNAP, Handles.CubeHandleCap);
            m_rightExtent.y = m_axisRestriction.y;
            m_leftExtent = Handles.FreeMoveHandle(m_leftExtent, Quaternion.identity, HANDLESIZE, HANDLESNAP, Handles.CubeHandleCap);
            m_leftExtent.y = m_axisRestriction.y;
            m_topExtent = Handles.FreeMoveHandle(m_topExtent, Quaternion.identity, HANDLESIZE, HANDLESNAP, Handles.CubeHandleCap);
            m_topExtent.x = m_axisRestriction.x;
            m_bottomExtent = Handles.FreeMoveHandle(m_bottomExtent, Quaternion.identity, HANDLESIZE, HANDLESNAP, Handles.CubeHandleCap);
            m_bottomExtent.x = m_axisRestriction.x;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

        }

        private void OnSceneGUI()
        {
            var sectionLighting = target as SectionLighting;

            UpdateHandleFields(sectionLighting);

            EditorGUI.BeginChangeCheck();
            DrawHandles();

            if (EditorGUI.EndChangeCheck())
            {
                var center = (m_rightExtent + m_leftExtent + m_topExtent + m_bottomExtent) / 4f;
                var bounds = new Bounds(center, Vector3.one);
                bounds.Encapsulate(m_rightExtent);
                bounds.Encapsulate(m_leftExtent);
                bounds.Encapsulate(m_topExtent);
                bounds.Encapsulate(m_bottomExtent);

                sectionLighting.SetBounds(bounds);
            }
        }


    }
}