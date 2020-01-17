using UnityEngine;
using UnityEngine.UI;

namespace Holysoft.Gameplay.UI
{
    public class BarStatUI : CappedStatUI
    {
        [SerializeField]
        private Image m_targetGraphic;

        private float m_maxValue;

        public override float maxValue
        {
            set
            {
                m_maxValue = value;
            }
        }
        public override float currentValue
        {
            set
            {
                m_targetGraphic.fillAmount = value / m_maxValue;
            }
        }
    }

}