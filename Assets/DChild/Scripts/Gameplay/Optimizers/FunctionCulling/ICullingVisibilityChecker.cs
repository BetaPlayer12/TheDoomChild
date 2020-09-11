using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Optimization.Modules
{
    public interface ICullingVisibilityChecker
    {
        void InitializeRuntimeData(Vector3 centerPosition, Transform reference);
        void UpdateRuntimeData(Vector3 centerPosition, Transform reference);
        bool IsVisible(Vector3 centerPosition, Transform reference, Bounds cameraBounds);
#if UNITY_EDITOR
        void DrawGizmos(Vector3 centerPosition, Transform transform, Color gizmoColor);
        void DrawHandles(Vector3 centerPosition, Transform transform, Color gizmoColor, Object undoReference);
        void InitializeConfiguration(float cullSize);
        ICullingVisibilityChecker CreateCopy();
#endif
    }
}