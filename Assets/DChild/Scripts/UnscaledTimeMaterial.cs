using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild
{
    public class UnscaledTimeMaterial : MonoBehaviour
    {
        [SerializeField]
        private Renderer[] m_renderer;
        [SerializeField, MinValue(1)]
        private float m_timeWrapper;

        private MaterialPropertyBlock[] m_propertyBlocks;
        private MaterialPropertyBlock m_cachePropertyBlock;

        private const string TIME_VARNAME = "_Vector1_Time";
        private float m_time;

        private void Start()
        {
            m_time = 0;
            m_propertyBlocks = new MaterialPropertyBlock[m_renderer.Length];
            for (int i = 0; i < m_propertyBlocks.Length; i++)
            {
                m_propertyBlocks[i] = new MaterialPropertyBlock();
            }
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < m_renderer.Length; i++)
            {
                m_cachePropertyBlock = m_propertyBlocks[i];
                m_renderer[i].GetPropertyBlock(m_cachePropertyBlock);
                m_time += Time.unscaledDeltaTime;
                m_time = Mathf.Repeat(m_time, m_timeWrapper);
                m_cachePropertyBlock.SetFloat(TIME_VARNAME, m_time);
                m_renderer[i].SetPropertyBlock(m_cachePropertyBlock);
            }
        }
    }
}
