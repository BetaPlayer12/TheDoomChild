using UnityEngine;

namespace DChild.Gameplay.Environment.Interractables
{
    public class GuidedLightHandle : MonoBehaviour
    {
        [SerializeField]
        private Renderer m_renderer;
        [SerializeField]
        private Transform m_tracker;

        private MaterialPropertyBlock m_materialPropertyBlock;
        private int m_trackerPositionParamID;

        private void Awake()
        {
            m_materialPropertyBlock = new MaterialPropertyBlock();
            m_trackerPositionParamID = Shader.PropertyToID("_LightGuide_Position");
        }

        private void LateUpdate()
        {
            m_renderer.GetPropertyBlock(m_materialPropertyBlock);
            m_materialPropertyBlock.SetVector(m_trackerPositionParamID, m_tracker.position);
            m_renderer.SetPropertyBlock(m_materialPropertyBlock);
        }
    }

}