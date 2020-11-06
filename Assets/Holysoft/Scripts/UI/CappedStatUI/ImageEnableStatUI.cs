using UnityEngine;
using UnityEngine.UI;

namespace Holysoft.Gameplay.UI
{
    public class ImageEnableStatUI : CappedStatUI
    {
        [SerializeField]
        private Image m_targetGraphic;

        private float m_cacheMaxValue;
        private float m_cacheCurrentValue;

        public override float maxValue
        {
            set
            {
                m_cacheMaxValue = value;
                m_targetGraphic.enabled = m_cacheCurrentValue >= m_cacheMaxValue;
            }
        }
        public override float currentValue
        {
            set
            {
                m_cacheCurrentValue = value;
                m_targetGraphic.enabled = m_cacheCurrentValue >= m_cacheMaxValue;
            }
        }
    }

}