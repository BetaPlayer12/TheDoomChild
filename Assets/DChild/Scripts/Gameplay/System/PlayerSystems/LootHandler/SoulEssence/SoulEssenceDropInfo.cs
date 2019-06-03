using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.SoulEssence
{
    [System.Serializable]
    public struct SoulEssenceDropInfo
    {
        [SerializeField, OnValueChanged("CalculateTotal")]
        private int m_valuePerEssence;
        [SerializeField, OnValueChanged("CalculateTotal"), Min(1)]
        private int m_essenceCount;

        public SoulEssenceDropInfo(int m_valuePerEssence, int m_essenceCount)
        {
            this.m_valuePerEssence = m_valuePerEssence;
            this.m_essenceCount = m_essenceCount;
#if UNITY_EDITOR
            m_totalEssence = m_essenceCount * m_valuePerEssence;
#endif
        }

        public int valuePerEssence => m_valuePerEssence;
        public int essenceCount => m_essenceCount;

#if UNITY_EDITOR
        [ShowInInspector, ReadOnly]
        private int m_totalEssence;

        private int ClampTo1(int value) => value < 1 ? 1 : value;

        private void CalculateTotal()
        {
            m_valuePerEssence = ClampTo1(m_valuePerEssence);
            m_essenceCount = ClampTo1(m_essenceCount);
            m_totalEssence = m_essenceCount * m_valuePerEssence;
        }
#endif
    }
}