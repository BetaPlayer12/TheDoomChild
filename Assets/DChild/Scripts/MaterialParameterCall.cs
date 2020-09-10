using DChild.Gameplay;
using System;
using System.Collections;
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
        private float m_lerpSpeed;

        public void SetTargetParameter(string parameter)
        {
            m_parameter = parameter;
            m_shaderID = Shader.PropertyToID(m_parameter);
        }

        public void SetLerpSpeed(float speed) => m_lerpSpeed = speed;

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

        public void LerpValue(float toValue)
        {
            Initialize();
            StartCoroutine(LerpRoutine(m_shaderID, toValue, m_lerpSpeed));
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

        private IEnumerator LerpRoutine(int shaderID, float destinationValue, float speed)
        {
            var lerpValue = 0f;
            var originValue = new float[m_renderers.Length];
            for (int i = 0; i < m_renderers.Length; i++)
            {
                m_renderers[i].GetPropertyBlock(m_propertyBlock);
                originValue[i] = m_propertyBlock.GetFloat(shaderID);
            }


            var value = 0f;
            Action<MaterialPropertyBlock> action = (MaterialPropertyBlock materialPropertyBlock) => { materialPropertyBlock.SetFloat(shaderID, value); };
            do
            {
                lerpValue += speed * GameplaySystem.time.deltaTime;
                for (int i = 0; i < m_renderers.Length; i++)
                {
                    m_renderers[i].GetPropertyBlock(m_propertyBlock);
                    value = Mathf.Lerp(originValue[i], destinationValue, lerpValue);
                    action?.Invoke(m_propertyBlock);
                    m_renderers[i].SetPropertyBlock(m_propertyBlock);
                }
                yield return null;
            } while (lerpValue < 1);
        }

        private void Awake()
        {
            Initialize();
        }
    }
}