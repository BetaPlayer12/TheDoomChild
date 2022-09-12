using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Gameplay.Optimization.Modules
{
    [System.Serializable]
    public struct CircleCuller : ICullingVisibilityChecker
    {
        [SerializeField, MinValue(0f)]
        private float m_distanceThreshold;

        private float m_initialScaleMagnitude;
        private float m_runtimeThreshold;

        public void InitializeRuntimeData(Vector3 centerPosition, Transform reference)
        {
            var scale = reference.lossyScale;
            scale.z = 0;
            m_initialScaleMagnitude = scale.magnitude;
            m_runtimeThreshold = m_distanceThreshold;
        }

        public void UpdateRuntimeData(Vector3 centerPosition, Transform reference)
        {
            var scale = reference.lossyScale;
            scale.z = 0;
            m_runtimeThreshold = m_distanceThreshold * Mathf.Abs(scale.magnitude / m_initialScaleMagnitude);
        }

        public bool IsVisible(Vector3 centerPosition, Transform reference, Bounds cameraBounds)
        {
            var instancePosition = centerPosition;
            instancePosition.z = 0;
            var clostestPoint = cameraBounds.ClosestPoint(instancePosition);
            return Vector3.Distance(instancePosition, clostestPoint) < m_distanceThreshold;
        }

#if UNITY_EDITOR
        [SerializeField, HideInInspector]
        private bool m_configInitialized;

        public CircleCuller(float distanceThreshold) : this()
        {
            m_distanceThreshold = distanceThreshold;
        }

        public void DrawGizmos(Vector3 centerPosition, Transform transform, Color gizmoColor)
        {
            var color = Handles.color;
            Handles.color = gizmoColor;
            var radius = Application.isPlaying ? m_runtimeThreshold : m_distanceThreshold;
            Handles.DrawSolidArc(centerPosition, Vector3.forward, Vector3.right, 360, radius);
            Handles.color = color;
        }
        public void DrawHandles(Vector3 centerPosition, Transform transform, Color gizmoColor, Object undoReference)
        {
            var color = Handles.color;
            if (Selection.activeTransform == transform)
            {
                Handles.color = Handles.selectedColor;
            }
            else
            {
                Handles.color = gizmoColor;
            }
            EditorGUI.BeginChangeCheck();
            var newDistance = Handles.RadiusHandle(Quaternion.identity, centerPosition, m_distanceThreshold);
            newDistance = Handles.ScaleValueHandle(newDistance, centerPosition, Quaternion.identity, 5f, Handles.ArrowHandleCap, 0.01f);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RegisterCompleteObjectUndo(undoReference, "CircleCullingChange");
                m_distanceThreshold = newDistance;
            }
            Handles.color = color;
        }

        public void InitializeConfiguration(float cullSize)
        {
            if (m_configInitialized == false)
            {
                m_distanceThreshold = cullSize;
                m_configInitialized = true;
            }
        }

        public ICullingVisibilityChecker CreateCopy() => new CircleCuller(m_distanceThreshold);
#endif
    }
}