using Holysoft;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.UI
{
    [RequireComponent(typeof(Slider))]
    public class BarUI : StatUI
    {
        [SerializeField]
        private Slider m_slider;

        public override float currentValue { set => m_slider.value = value; }
        public override float maxValue { set => m_slider.maxValue = value; }

        private void OnValidate()
        {
            ComponentUtility.AssignNullComponent(this, ref m_slider);
        }
    }
}