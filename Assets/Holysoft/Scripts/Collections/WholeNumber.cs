using UnityEngine;

namespace Holysoft.Collections
{
    [System.Serializable]
    public class WholeNumber
    {
        [SerializeField]
        private int m_value;

        public int value => m_value;

        public void Add(int value) => m_value += value;

        public void Subtract(int value)
        {
            m_value -= value;
            if (value < 0)
            {
                value = 0;
            }
        }

        public void Set(int value) => m_value = value;
    }
}
