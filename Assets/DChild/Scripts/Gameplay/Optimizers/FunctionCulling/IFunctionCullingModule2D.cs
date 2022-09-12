using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Optimization.Modules
{
    public interface IFunctionCullingModule2D
    {
        void Initalize();
        void Validate();
        void ExecuteOptimization2D(Vector3 reference, Bounds boundExtent);
#if UNITY_EDITOR
        void DrawGizmos();
        void DrawHandles(UnityEngine.Object undoReference);
#endif
    }
}