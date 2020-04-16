using System;
using UnityEngine;

namespace DChild
{
    public class MaterialParameterCall : MonoBehaviour
    {
        [SerializeField]
        private string m_parameter;
        [SerializeField]
        private Renderer[] m_renderers;

        private MaterialPropertyBlock m_propertyBlock;
        private int m_shaderID;
        private bool m_isInitialized;

        public void SetTargetParameter(string parameter)
        {
            m_parameter = parameter;
            m_shaderID = Shader.PropertyToID(m_parameter);
        }

        public void SetRenderers(params Renderer[] renderers) => m_renderers = renderers;

        public void SetValue(bool value)
        {
            Initialize();
            SetPropertyBlock((MaterialPropertyBlock materialPropertyBlock) => { materialPropertyBlock.SetInt(m_shaderID, value ? 1 : 0); });
        }

        public void SetValue(float value)
        {
            Initialize();
            SetPropertyBlock((MaterialPropertyBlock materialPropertyBlock) => { materialPropertyBlock.SetFloat(m_shaderID, value); });
        }

        private void SetPropertyBlock(Action<MaterialPropertyBlock> action)
        {
            for (int i = 0; i < m_renderers.Length; i++)
            {
                m_renderers[i].GetPropertyBlock(m_propertyBlock);
                action?.Invoke(m_propertyBlock);
                m_renderers[i].SetPropertyBlock(m_propertyBlock);
            }
        }

        private void Initialize()
        {
            if (m_isInitialized == false)
            {
                if (m_parameter != string.Empty)
                {
                    m_shaderID = Shader.PropertyToID(m_parameter);
                }
                m_propertyBlock = new MaterialPropertyBlock();
                m_isInitialized = true;
            }
        }

        private void Awake()
        {
            Initialize();
        }
    }
}