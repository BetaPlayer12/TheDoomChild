using System.Collections.Generic;
using DChild.Gameplay.Combat;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Characters.Players
{
    [System.Serializable]
    public class DamageList
    {
        [SerializeField]
        List<AttackDamage> m_values;

        public AttackDamage[] values { get => m_values.ToArray(); }

        public void AddValue(AttackType type, int value)
        {
            var index = FindIndex(type);
            if (index >= 0)
            {
                var damageElement = m_values[index];
                damageElement.damage += value;
                if (damageElement.damage == 0)
                {
                    m_values.RemoveAt(index);
                }
                else
                {
                    m_values[index] = damageElement;
                }
            }
            else
            {
                m_values.Add(new AttackDamage(type, value));
            }
        }

        public void SetValue(AttackType type, int value)
        {
            var index = FindIndex(type);
            if (index >= 0)
            {
                var damageElement = m_values[index];
                damageElement.damage = value;
                if (damageElement.damage == 0)
                {
                    m_values.RemoveAt(index);
                }
                else
                {
                    m_values[index] = damageElement;
                }
            }
            else
            {
                m_values.Add(new AttackDamage(type, value));
            }
        }

        public int GetValue(AttackType type)
        {
            var index = FindIndex(type);
            if (index >= 0)
            {
                return m_values[index].damage;
            }
            return 0;
        }

        private int FindIndex(AttackType type)
        {
            for (int i = 0; i < m_values.Count; i++)
            {
                if (m_values[i].type == type)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}