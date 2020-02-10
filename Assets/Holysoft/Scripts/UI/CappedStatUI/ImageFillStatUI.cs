using UnityEngine;
using UnityEngine.UI;

namespace Holysoft.Gameplay.UI
{
    public class ImageFillStatUI : CappedStatUI
    {
        [SerializeField]
        private Image m_image;
        private float m_maxValue;


        public override float maxValue { set => m_maxValue = value; }
        public override float currentValue
        {
            set
            {
                var uiValue = value / m_maxValue;
                m_image.fillAmount = float.IsNaN(uiValue) ? 0 : uiValue;
            }
        }
    }
}