using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusAilment
{
    public class StatusEffectResistance : SerializedMonoBehaviour, IStatusEffectResistance
    {
        [SerializeField]
        private StatusEffectChanceData m_data;
        [ShowInInspector, HideInEditorMode]
        private Dictionary<StatusEffectType, int> m_resistances;

        public int GetResistance(StatusEffectType type) => m_resistances.ContainsKey(type) ? m_resistances[type] : 0;

        public void SetResistance(StatusEffectType type, int resistanceValue)
        {
            resistanceValue = Mathf.Clamp(resistanceValue, 0, 100);
            if (resistanceValue == 0)
            {
                if (m_resistances.ContainsKey(type))
                {
                    m_resistances.Remove(type);
                }
            }
            else
            {
                if (m_resistances.ContainsKey(type))
                {
                    m_resistances[type] = resistanceValue;
                }
                else
                {
                    m_resistances.Add(type, resistanceValue);
                }
            }
        }

        public void SetData(StatusEffectChanceData data)
        {
            if (m_data != data)
            {
                m_data = data;
                Copy(m_data.chance, m_resistances);
            }
        }

        private void Copy(Dictionary<StatusEffectType, int> source, Dictionary<StatusEffectType, int> destination)
        {
            destination.Clear();
            foreach (var key in source.Keys)
            {
                destination.Add(key, source[key]);
            }
        }

        private void Awake()
        {
            m_resistances = new Dictionary<StatusEffectType, int>();
            if (m_data != null)
            {
                Copy(m_data.chance, m_resistances);
            }
        }
    }
}