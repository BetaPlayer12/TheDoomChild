using UnityEngine;

namespace DChild.Gameplay.Combat.StatusInfliction
{
    [System.Serializable]
    public struct StatusInflictionInfo
    {
        [SerializeField]
        private StatusEffectType m_effect;
        [SerializeField, Range(0.1f, 100f)]
        private float m_chance;

        public StatusInflictionInfo(StatusEffectType m_effect, float m_chance)
        {
            this.m_effect = m_effect;
            this.m_chance = m_chance;
        }

        public StatusEffectType effect => m_effect;
        public float chance
        {
            get => m_chance; set
            {
                m_chance = Mathf.Clamp01(value);
            }
        }
    }
}