using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild
{
    public class UnscaledTimeMaterial : MonoBehaviour
    {
        [SerializeField]
        private Material[] m_materials;
        [SerializeField, MinValue(1)]
        private float m_timeWrapper;

        private const string TIME_VARNAME = "_Vector1_Time";
        private float m_time;

        private void Start()
        {
            m_time = 0;
        }

        // Update is called once per frame
        void Update()
        {
            for (int i = 0; i < m_materials.Length; i++)
            {
                m_time += Time.unscaledDeltaTime;
                m_time = Mathf.Repeat(m_time, m_timeWrapper);
                m_materials[i].SetFloat(TIME_VARNAME, m_time);
            }
        }
    }
}
