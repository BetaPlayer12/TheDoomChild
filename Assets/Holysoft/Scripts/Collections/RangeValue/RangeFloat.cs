using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Holysoft.Collections
{
    [System.Serializable]
    public struct RangeFloat
    {
        [SerializeField]
        private float m_min;
        [SerializeField]
        private float m_max;

        public RangeFloat(float min, float max)
        {
            this.m_min = min;
            this.m_max = max;
        }

        public float min => m_min;
        public float max => m_max;

        public float GenerateRandomValue() => Random.Range(m_min, m_max);

        public bool InRange(float value) => value >= m_min && value <= m_max;
    }
}