using UnityEngine;
using UnityEngine.UI;

namespace Holysoft.Gameplay.UI
{
    public class ImageEnablerStatUI : CappedStatUI
    {
        [SerializeField]
        private Image m_image;

        private float m_maxValue;

        public override float maxValue { set => m_maxValue = value; }
        public override float currentValue
        {
            set
            {
                m_image.enabled = value >= m_maxValue;
            }
        }
    }
}