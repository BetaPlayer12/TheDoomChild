using System.Collections.Generic;
using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    [System.Serializable]
    public class DamageList
    {
        [SerializeField]
        List<Damage> m_values;

        public DamageList()
        {
            m_values = new List<Damage>();
        }

        public List<Damage> values => m_values;

        public void AddValue(DamageType type, int value)
        {
            var index = FindIndex(type);
            if (index >= 0)
            {
                var damageElement = m_values[index];
                damageElement.value += value;
                if (damageElement.value == 0)
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
                m_values.Add(new Damage(type, value));
            }
        }

        public void SetValue(DamageType type, int value)
        {
            var index = FindIndex(type);
            if (index >= 0)
            {
                var damageElement = m_values[index];
                damageElement.value = value;
                if (damageElement.value == 0)
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
                m_values.Add(new Damage(type, value));
            }
        }

        public int GetValue(DamageType type)
        {
            var index = FindIndex(type);
            if (index >= 0)
            {
                return m_values[index].value;
            }
            return 0;
        }

        private int FindIndex(DamageType type)
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