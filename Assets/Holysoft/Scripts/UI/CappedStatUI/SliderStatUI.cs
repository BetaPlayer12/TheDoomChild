using UnityEngine;
using UnityEngine.UI;

namespace Holysoft.Gameplay.UI
{
    public class SliderStatUI : CappedStatUI
    {
        [SerializeField]
        private Slider m_slider;

        public override float maxValue { set => m_slider.maxValue = value; }
        public override float currentValue { set => m_slider.value = value; }
    }
}