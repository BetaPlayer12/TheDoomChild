using UnityEngine;

namespace DChild.Gameplay.Optimization.Modules
{
    public interface IFunctionCullingModule
    {
        void Validate();
        void ExecuteOptimization(Vector3 reference, Bounds boundExtent);
#if UNITY_EDITOR
        void DrawGizmo();
#endif
    }
}