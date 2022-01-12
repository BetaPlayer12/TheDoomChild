using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Combat.StatusAilment.UI
{
    public class StatusEffectScreenFilterUI : SerializedMonoBehaviour
    {
        [SerializeField]
        private Image m_filter;
        [SerializeField]
        private Dictionary<StatusEffectType, Material> m_filterPair;

        private StatusEffectType m_currentFilter = StatusEffectType._COUNT;

        public void ShowFilter(StatusEffectType type)
        {
            if (m_filterPair.TryGetValue(type, out Material material))
            {
                m_filter.enabled = true;
                m_filter.material = material;
                m_currentFilter = type;
            }
        }

        public void HideFilter(StatusEffectType type)
        {
            if (m_currentFilter == type)
            {
                m_filter.enabled = false;
                m_filter.material = null;
            }
        }
    }
}