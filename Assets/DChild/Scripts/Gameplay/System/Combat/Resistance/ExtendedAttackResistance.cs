using System.Collections.Generic;

namespace DChild.Gameplay.Combat
{
    public class ExtendedAttackResistance : AttackResistance
    {
        private Dictionary<AttackType, float> m_additionalResistance;

        public void AddResistance(AttackType type, float resistance)
        {
            if (m_additionalResistance.ContainsKey(type))
            {
                m_additionalResistance[type] += resistance;
            }
            else
            {
                m_additionalResistance.Add(type, resistance);
            }
        }

        public void ReduceResistance(AttackType type, float resistance)
        {
            if (m_additionalResistance.ContainsKey(type))
            {
                m_additionalResistance[type] -= resistance;
            }
            else
            {
                m_additionalResistance.Add(type, resistance);
            }
        }

        public override float GetResistance(AttackType type)
        {
            var baseResistance = m_resistance.ContainsKey(type) ? m_resistance[type] : 0;
            var additionalResistance = m_additionalResistance.ContainsKey(type) ? m_additionalResistance[type] : 0;
            return baseResistance + additionalResistance;
        }

        private void Awake()
        {
            m_additionalResistance = new Dictionary<AttackType, float>();
        }
    }
}