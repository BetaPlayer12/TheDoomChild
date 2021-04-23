using DChild.Gameplay.Cinematics;
using DChild.Gameplay.Optimization.Modules;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Optimization
{
    [AddComponentMenu("DChild/Gameplay/Optimizer/Camera2D Function Culling")]
    public sealed class Camera2DFunctionCulling : SerializedMonoBehaviour
    {
        [SerializeField]
        private CameraBounds m_reference;
#if UNITY_EDITOR
        [SerializeField]
        private bool m_drawGizmoSelectedOnly = true;
#endif
        [SerializeField, HideReferenceObjectPicker, ListDrawerSettings(NumberOfItemsPerPage = 1)]
        private IFunctionCullingModule2D[] m_modules = new IFunctionCullingModule2D[0];

        private void Awake()
        {
            for (int i = 0; i < m_modules.Length; i++)
            {
                m_modules[i].Initalize();
            }
        }

        public void LateUpdate()
        {
            var camBounds = m_reference.value;
            for (int i = 0; i < m_modules.Length; i++)
            {
                m_modules[i].ExecuteOptimization2D(camBounds.center, camBounds);
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
        }
#endif
    }
}