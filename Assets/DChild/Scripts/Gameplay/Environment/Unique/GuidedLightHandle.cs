using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{
    public class GuidedLightHandle : MonoBehaviour
    {
        [SerializeField]
        private Renderer[] m_renderers;
        [SerializeField]
        private Transform m_tracker;

        private MaterialPropertyBlock m_materialPropertyBlock;
        private int m_trackerPositionParamID;

        private void UpdateRenderer(Renderer renderer)
        {
            renderer.GetPropertyBlock(m_materialPropertyBlock);
            m_materialPropertyBlock.SetVector(m_trackerPositionParamID, m_tracker.position);
            renderer.SetPropertyBlock(m_materialPropertyBlock);
        }

        private void Awake()
        {
            m_materialPropertyBlock = new MaterialPropertyBlock();
            m_trackerPositionParamID = Shader.PropertyToID("_LightGuide_Position");
        }

        private void LateUpdate()
        {
            for (int i = 0; i < m_renderers.Length; i++)
            {
                UpdateRenderer(m_renderers[i]);
            }

        }
    }
}