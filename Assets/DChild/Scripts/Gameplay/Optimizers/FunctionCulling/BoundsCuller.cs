using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.IMGUI.Controls;
#endif

namespace DChild.Gameplay.Optimization.Modules
{
    [System.Serializable]
    public struct BoundsCuller : ICullingVisibilityChecker
    {
        [SerializeField]
        private Vector2 m_offset;
        [SerializeField, MinValue(0)]
        private Vector2 m_size;

        private Vector2 m_initialScale;
        private Bounds m_runtimeBounds;

        public void InitializeRuntimeData(Vector3 centerPosition, Transform reference)
        {
            m_initialScale = reference.lossyScale;
            UpdateRuntimeBounds(centerPosition, reference);
        }

        public bool IsVisible(Vector3 centerPosition, Transform reference, Bounds cameraBounds)
        {
            var closestPointToBounds = m_runtimeBounds.ClosestPoint(cameraBounds.center);
            return cameraBounds.Contains(closestPointToBounds);
        }

        public void UpdateRuntimeData(Vector3 centerPosition, Transform reference)
        {
            UpdateRuntimeBounds(centerPosition, reference);
        }

        private void UpdateRuntimeBounds(Vector3 centerPosition, Transform reference)
        {
            centerPosition += (reference.right * m_offset.x) + (reference.up * m_offset.y);
            m_runtimeBounds.center = centerPosition;
            var extents = m_size / 2;
            if (Application.isPlaying)
            {
                var currentScale = reference.lossyScale;
                extents.x *= currentScale.x / m_initialScale.x;
                extents.y *= currentScale.y / m_initialScale.y;
            }
            var pointA = (-reference.right * extents.x) + (reference.up * extents.y);
            pointA.z = 0;
            var pointB = (reference.right * extents.x) + (reference.up * extents.y);
            pointB.z = 0;
            //Might Be Expensive
            var horizontalExtent = Mathf.Max(Mathf.Abs(pointA.x), Mathf.Abs(pointB.x));
            var verticalExtent = Mathf.Max(Mathf.Abs(pointA.y), Mathf.Abs(pointB.y));
            m_runtimeBounds.extents = new Vector3(horizontalExtent, verticalExtent, 0);

#if UNITY_EDITOR
            UpdateGizmoRect();
#endif
        }

#if UNITY_EDITOR
        [SerializeField, HideInInspector]
        private bool m_configInitialized;
        private Rect m_runtimeGizmoRect;

        public BoundsCuller(Vector2 offset, Vector2 size) : this()
        {
            m_offset = offset;
            m_size = size;
        }

        public void DrawGizmos(Vector3 centerPosition, Transform transform, Color gizmoColor)
        {
            if (Application.isPlaying == false)
            {
                UpdateRuntimeBounds(centerPosition, transform);
                UpdateGizmoRect();
            }
            var color = Handles.color;
            Handles.color = gizmoColor;
            Handles.DrawSolidRectangleWithOutline(m_runtimeGizmoRect, gizmoColor, Color.white);
            Handles.color = color;
        }

        public void DrawHandles(Vector3 centerPosition, Transform transform, Color gizmoColor, Object undoReference)
        {
            EditorGUI.BeginChangeCheck();

            Handles.color = Color.red;
            var xScale = Handles.ScaleSlider(m_size.x, m_runtimeBounds.center, Vector3.right, Quaternion.identity, 1f, 0.01f);
            if (xScale < 0)
            {
                xScale = 0;
            }
            Handles.color = Color.green;

            var yScale = Handles.ScaleSlider(m_size.y, m_runtimeBounds.center, Vector3.up, Quaternion.identity, 1f, 0.01f);
            if (yScale < 0)
            {
                yScale = 0;
            }

            Handles.color = Color.white;
            var newCenter = Handles.PositionHandle(m_runtimeBounds.center, transform.rotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RegisterCompleteObjectUndo(undoReference, "BoundCullingChange");
                m_size.x = xScale;
                m_size.y = yScale;
                m_offset = newCenter - centerPosition;
                m_runtimeBounds.center = newCenter;
            }
        }

        public void InitializeConfiguration(float cullSize)
        {
            if (m_configInitialized == false)
            {
                m_size = Vector2.one * cullSize;
                m_initialScale = Vector2.one;
                m_configInitialized = true;
            }
        }

        private void UpdateGizmoRect()
        {
            var rectPos = (Vector2)m_runtimeBounds.center;
            rectPos -= (Vector2)m_runtimeBounds.extents;
            m_runtimeGizmoRect = new Rect(rectPos, m_runtimeBounds.size);
        }

        public ICullingVisibilityChecker CreateCopy() => new BoundsCuller(m_offset, m_size);
#endif
    }
}