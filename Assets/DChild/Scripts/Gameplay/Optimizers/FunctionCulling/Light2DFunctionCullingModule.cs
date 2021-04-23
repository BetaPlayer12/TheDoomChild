using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Profiling;

namespace DChild.Gameplay.Optimization.Modules
{

    [System.Serializable, InfoBox("Light 2D")]
    public sealed class Light2DFunctionCullingModule : FunctionCullingModule<Light2DFunctionCullingModule.Info, Light2D>, IFunctionCullingModule2D
    {
        public class Info : BaseInfo
        {
            [SerializeField, HideInInspector]
            private Vector3 m_shapePathCentriod;

            private Vector3 m_runtimeShapePathCentroid;
            private Vector3 m_lossyScale;

            private Vector3 m_rotationOffset;
            private Vector3 m_rotation;

            public Info() : base()
            {
                m_culler = new CircleCuller();
#if UNITY_EDITOR
                InitializeCuller();
#endif
            }

            protected override float m_defaulCullSize => 1;
            protected override Vector3 position
            {
                get
                {
                    if (m_instance.lightType == Light2D.LightType.Freeform)
                    {
                        return m_instance.transform.position + m_rotationOffset;
                    }
                    else
                    {
                        return m_instance.transform.position;
                    }
                }
            }

            public override void Initialize()
            {
                UpdateRuntimeShapePathCentroid();
                m_lossyScale = scale;
                UpdateRotationOffset();
                m_rotation = m_instance.transform.rotation.eulerAngles;
                base.Initialize();
            }

            public override void ExecuteOptimization(Bounds cameraBounds)
            {
                bool transformChanges = false;
                var lossyScale = scale;
                var rotation = m_instance.transform.rotation.eulerAngles;
                if (m_lossyScale != lossyScale)
                {
                    UpdateRuntimeShapePathCentroid();
                    UpdateRotationOffset();
                    m_lossyScale = lossyScale;
                    transformChanges = true;
                }
                else if (m_rotation != rotation)
                {
                    m_rotation = rotation;
                    UpdateRotationOffset();
                    transformChanges = true;
                }

                if (transformChanges)
                {
                    m_culler.UpdateRuntimeData(position, m_instance.transform);
                }
                base.ExecuteOptimization(cameraBounds);
            }



            public void Validate()
            {
                var centroid = Vector3.zero;
                if (m_instance != null)
                {
                    var lightPath = m_instance.shapePath;
                    for (int i = 0; i < lightPath.Length; i++)
                    {
                        centroid += lightPath[i];
                    }

                    centroid /= lightPath.Length;
                }
                m_shapePathCentriod = centroid;
                UpdateRuntimeShapePathCentroid();
                UpdateRotationOffset();
            }

            protected override void SetFunctionality(bool enabled)
            {
                m_instance.enabled = enabled;
            }

            private void UpdateRuntimeShapePathCentroid()
            {
                m_runtimeShapePathCentroid = m_shapePathCentriod;
                m_runtimeShapePathCentroid.x *= scale.x;
                m_runtimeShapePathCentroid.y *= scale.y;
            }

            private void UpdateRotationOffset()
            {
                var transform = m_instance.transform;
                m_rotationOffset = (m_runtimeShapePathCentroid.x * transform.right) + (m_runtimeShapePathCentroid.y * transform.up);
            }
        }

        public override void Validate()
        {
            base.Validate();
            for (int i = 0; i < m_infos.Length; i++)
            {
                m_infos[i].Validate();
            }
        }

        public void ExecuteOptimization2D(Vector3 reference, Bounds boundExtent)
        {
#if UNITY_EDITOR
            Profiler.BeginSample("Light2DFunctionCullingModule.ExecuteOptimization2D()");
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