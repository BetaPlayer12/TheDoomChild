using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    [System.Serializable]
    public struct RatioCalulation
    {
        [SerializeField, MinValue(1)]
        private int m_perValue;
        [SerializeField, MinValue(1)]
        private int m_output;

        public int CalculateOutput(int value) => Mathf.FloorToInt(value / m_perValue) * m_output;
    }
}