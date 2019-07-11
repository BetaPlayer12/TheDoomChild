using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Refactor.DChild.Gameplay.Combat
{
    public class AttackCalculator : MonoBehaviour
    {
        [ShowInInspector]
        private List<AttackDamage> m_damage;
        private float m_modifier = 1;
        private List<AttackDamage> m_modifiedDamage;

        public void SetDamage(IEnumerable<AttackDamage> damage)
        {
            m_damage.Clear();
            m_damage.AddRange(damage);
            CalculateDamage();
        }

        public List<AttackDamage> GetDamage(float modifier)
        {
            if (m_modifier != modifier)
            {
                m_modifier = modifier;
                CalculateDamage();
            }
            return m_modifiedDamage;
        }

        private void CalculateDamage()
        {
            m_modifiedDamage.Clear();
            if (m_modifier == 1)
            {
                m_modifiedDamage.AddRange(m_damage);
            }
            else
            {
                for (int i = 0; i < m_damage.Count; i++)
                {
                    var damage = m_damage[i];
                    damage.damage = Mathf.CeilToInt(damage.damage * m_modifier);
                    m_modifiedDamage.Add(damage);
                }
            }
        }

        private void Awake()
        {
            m_damage = new List<AttackDamage>();
            m_modifier = 1;
            m_modifiedDamage = new List<AttackDamage>();
            CalculateDamage();
        }
    }
}