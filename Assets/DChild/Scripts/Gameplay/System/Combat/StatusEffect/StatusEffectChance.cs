using UnityEngine;

namespace DChild.Gameplay.Combat.StatusAilment
{
    [System.Serializable]
    public struct StatusEffectChance
    {
        [SerializeField]
        private StatusEffectType m_type;
        [SerializeField, Range(0, 100)]
        private int m_chance;

        public StatusEffectChance(StatusEffectType m_type, int m_chance)
        {
            this.m_type = m_type;
            this.m_chance = m_chance;
        }

        public StatusEffectType type => m_type;
        public int chance { get => m_chance; set => m_chance = value; }

    }
}