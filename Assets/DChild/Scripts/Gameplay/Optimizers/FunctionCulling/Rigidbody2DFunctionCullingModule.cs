using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Profiling;

namespace DChild.Gameplay.Optimization.Modules
{
    [System.Serializable, InfoBox("Rigidbody2D")]
    public sealed class Rigidbody2DFunctionCullingModule : FunctionCullingModule<Rigidbody2DFunctionCullingModule.Info, Rigidbody2D>, IFunctionCullingModule2D
    {
        public class Info : BaseInfo
        {
            protected override float m_defaulCullSize => 1;
            protected override void SetFunctionality(bool enabled)
            {
                m_instance.simulated = enabled;
            }
        }

        public void ExecuteOptimization2D(Vector3 reference, Bounds boundExtent)
        {
#if UNITY_EDITOR
            Profiler.BeginSample("Rigidbody2DFunctionCullingModule.ExecuteOptimization2D()");
#endif
            for (int i = 0; i < m_infos.Length; i++)
            {
                m_infos[i].ExecuteOptimization(boundExtent);
            }
#if UNITY_EDITOR
            Profiler.EndSample();
#endif
        }
    }
}