using UnityEngine;

namespace Holysoft.Collections
{
    [System.Serializable]
    public struct RangeInt
    {
        [SerializeField]
        private int m_min;
        [SerializeField]
        private int m_max;

        public RangeInt(int min, int max)
        {
            this.m_min = min;
            this.m_max = max;
        }

        public int min => m_min;
        public int max => m_max;

        public int GenerateRandomValue()
        {
            return Random.Range(m_min, m_max + 1);
        }

        public bool InRange(int value) => value >= m_min && value <= m_max;
    }
}