using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Profiling;

namespace DChild.Gameplay.Optimization.Modules
{

    [System.Serializable, InfoBox("Spine")]
    public sealed class SpineFunctionCullingModule : FunctionCullingModule<SpineFunctionCullingModule.Info, SkeletonAnimation>, IFunctionCullingModule2D
    {
        public class Info : BaseInfo
        {
            [SerializeField, HideInInspector]
            private Character m_character;

            public Info() : base()
            {
                m_culler = new CircleCuller();
#if UNITY_EDITOR
                InitializeCuller();
#endif
            }

            protected override Vector3 position => m_character?.centerMass?.position ?? base.position;

            protected override float m_defaulCullSize => 10;

            public override void SetReference(SkeletonAnimation instance)
            {
                base.SetReference(instance);
                m_character = instance.GetComponentInParent<Character>();
            }

            protected override void SetFunctionality(bool enabled)
            {
                m_instance.enabled = enabled;
            }

        }
        public void ExecuteOptimization2D(Vector3 reference, Bounds boundExtent)
        {
#if UNITY_EDITOR
            Profiler.BeginSample("SpineFunctionCullingModule.ExecuteOptimization2D()");
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