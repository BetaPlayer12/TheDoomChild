using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Gameplay.Optimization.Modules
{
    public struct SphereCuller : ICullingVisibilityChecker
    {
        [SerializeField, MinValue(0f)]
        private float m_distanceThreshold;

        private float m_scaleMagnitude;
        private float m_runtimeThreshold;

        public void InitializeRuntimeData(Vector3 centerPosition, Transform reference)
        {
            m_scaleMagnitude = reference.lossyScale.magnitude;
            m_runtimeThreshold = m_distanceThreshold;
        }

        public void UpdateRuntimeData(Vector3 centerPosition, Transform reference)
        {
            m_runtimeThreshold = reference.lossyScale.magnitude / m_scaleMagnitude;
        }

        public bool IsVisible(Vector3 centerPosition, Transform reference, Bounds cameraBounds)
        {
            var instancePosition = centerPosition;
            var clostestPoint = cameraBounds.ClosestPoint(instancePosition);
            return Vector3.Distance(instancePosition, clostestPoint) < m_distanceThreshold;
        }

#if UNITY_EDITOR
        [SerializeField, HideInInspector]
        private bool m_configInitialized;

        public SphereCuller(float distanceThreshold) : this()
        {
            m_distanceThreshold = distanceThreshold;
        }

        public void DrawGizmos(Vector3 centerPosition, Transform transform, Color gizmoColor)
        {
            var color = Gizmos.color;
            Gizmos.color = gizmoColor;
            Gizmos.DrawSphere(centerPosition, m_distanceThreshold);
            Gizmos.color = color;
        }

        public void DrawHandles(Vector3 centerPosition, Transform transform, Color gizmoColor, Object undoReference)
        {
            var color = Handles.color;
            Handles.color = gizmoColor;
            m_distanceThreshold = Handles.ScaleValueHandle(m_distanceThreshold, centerPosition, Quaternion.identity, 5f, Handles.ArrowHandleCap, 0.01f);
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

        public ICullingVisibilityChecker CreateCopy() => new SphereCuller(m_distanceThreshold);
#endif
    }
}