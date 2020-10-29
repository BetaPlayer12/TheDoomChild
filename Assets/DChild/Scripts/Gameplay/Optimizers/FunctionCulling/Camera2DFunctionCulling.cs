using DChild.Gameplay.Optimization.Modules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Optimization
{
    [AddComponentMenu("DChild/Gameplay/Optimizer/Camera2D Function Culling")]
    public sealed class Camera2DFunctionCulling : SerializedMonoBehaviour
    {
        [SerializeField]
        private Camera m_reference;
#if UNITY_EDITOR
        [SerializeField]
        private bool m_drawGizmoSelectedOnly = true;
#endif
        [SerializeField, HideReferenceObjectPicker, ListDrawerSettings(NumberOfItemsPerPage = 1)]
        private IFunctionCullingModule2D[] m_modules = new IFunctionCullingModule2D[0];
        private Bounds m_cameraBounds;

        private bool m_prevOrthographic;
        private float m_prevFieldOFView;
        private float m_prevZ;

        public void SetReference(Camera reference)
        {
            m_reference = reference;
            UpdateCameraBounds();
            UpdateCache();
        }

        private void UpdateCache()
        {
            m_prevOrthographic = m_reference.orthographic;
            m_prevFieldOFView = m_prevOrthographic ? m_reference.orthographicSize : m_reference.fieldOfView;
            m_prevZ = m_reference.transform.position.z;
        }

        private void UpdateCameraBounds()
        {
            if (m_reference.orthographic)
            {
                var vertExtent = m_reference.orthographicSize;
                var horzExtent = vertExtent * Screen.width / Screen.height;
                m_cameraBounds.extents = new Vector3(horzExtent, vertExtent, 0f);
            }
            else
            {
                var frustumHeight = -m_reference.transform.position.z * Mathf.Tan(m_reference.fieldOfView * 0.5f * Mathf.Deg2Rad);
                var frustumWidth = frustumHeight * m_reference.aspect;
                m_cameraBounds.extents = new Vector3(frustumWidth, frustumHeight, 0f);
            }
        }

        private bool HasCameraConfigChanged()
        {
            var configurationChanges = m_reference.orthographic != m_prevOrthographic || (m_prevFieldOFView != (m_prevOrthographic ? m_reference.orthographicSize : m_reference.fieldOfView));
            if (configurationChanges == false && m_reference.orthographic == false)
            {
                return m_prevZ != m_reference.transform.position.z;
            }
            else
            {
                return true;
            }
        }

        private void Awake()
        {
            for (int i = 0; i < m_modules.Length; i++)
            {
                m_modules[i].Initalize();
            }
        }

        private void OnEnable()
        {
            UpdateCameraBounds();
            UpdateCache();
        }

        public void LateUpdate()
        {
            var position = m_reference.transform.position;
            position.z = 0;
            m_cameraBounds.center = position;

            if (HasCameraConfigChanged())
            {
                UpdateCameraBounds();
                UpdateCache();
            }

            for (int i = 0; i < m_modules.Length; i++)
            {
                m_modules[i].ExecuteOptimization2D(position, m_cameraBounds);
            }
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (m_drawGizmoSelectedOnly == false)
            {
                DrawGizmos();
            }
#endif
        }

        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            if (m_drawGizmoSelectedOnly)
            {
                DrawGizmos();
            }
#endif
        }

        private void OnValidate()
        {
            for (int i = 0; i < m_modules.Length; i++)
            {
                m_modules[i].Validate();
            }
        }

#if UNITY_EDITOR
        private void DrawGizmos()
        {
            bool isEditor = Application.isPlaying == false;
            for (int i = 0; i < m_modules.Length; i++)
            {
                if (isEditor)
                {
                    m_modules[i].Validate();
                }
                m_modules[i].DrawGizmos();
            }
            var color = Gizmos.color;
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(m_cameraBounds.center, m_cameraBounds.size);
            Gizmos.color = color;
        }
#endif
    }
}