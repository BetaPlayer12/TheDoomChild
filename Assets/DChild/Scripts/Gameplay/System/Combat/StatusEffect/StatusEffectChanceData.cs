using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat.StatusAilment
{
    [CreateAssetMenu(fileName = "StatusEffectChanceData", menuName = "DChild/Gameplay/Combat/Inflictions/Status Effect Chance Data")]
    public class StatusEffectChanceData : SerializedScriptableObject
    {
        [SerializeField, OnValueChanged("Validate", true)]
        private Dictionary<StatusEffectType, int> m_chances = new Dictionary<StatusEffectType, int>();

        public Dictionary<StatusEffectType, int> chance => m_chances;

        private void OnDisable()
        {
            foreach (var key in m_chances.Keys)
            {
                if (m_chances[key] == 0)
                {
                    m_chances.Remove(key);
                }
            }
        }

        private void Validate()
        {
            foreach (var key in m_chances.Keys)
            {
                m_chances[key] = Mathf.Clamp(m_chances[key], 0, 100);
            }
        }
    }
}