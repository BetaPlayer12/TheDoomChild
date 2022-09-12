using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Profiling;

namespace DChild.Gameplay.Optimization.Modules
{
    [System.Serializable, InfoBox("Rigidbody")]
    public sealed class RigidbodyFunctionCullingModule : FunctionCullingModule<RigidbodyFunctionCullingModule.Info, Rigidbody>, IFunctionCullingModule2D
    {
        public class Info : BaseInfo
        {
            protected override float m_defaulCullSize => throw new System.NotImplementedException();

            protected override void SetFunctionality(bool enabled)
            {
                m_instance.detectCollisions = enabled;
            }
        }

        public void ExecuteOptimization2D(Vector3 reference, Bounds boundExtent)
        {
#if UNITY_EDITOR
            Profiler.BeginSample("RigidbodyFunctionCullingModule.ExecuteOptimization2D()");
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