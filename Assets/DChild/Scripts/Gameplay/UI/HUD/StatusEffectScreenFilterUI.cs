using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.Combat.StatusAilment.UI
{
    public class StatusEffectScreenFilterUI : SerializedMonoBehaviour
    {
        [SerializeField]
        private UIContainer m_container;
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

                m_container.Show();
            }
        }

        public void HideFilter(StatusEffectType type)
        {
            if (m_currentFilter == type)
            {
                //m_filter.enabled = false;
                //m_filter.material = null;

                m_container.Hide();
            }
        }

        private void SetFadeValue(Image image, float value)
        {
            image.material.SetFloat("_Overall_Opacity", value);
        }

        private void Awake()
        {
            if (m_filter.material)
            {
                SetFadeValue(m_filter, 0);
            }
        }
    }
}