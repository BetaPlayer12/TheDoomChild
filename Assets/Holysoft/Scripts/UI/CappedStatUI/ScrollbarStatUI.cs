using UnityEngine;
using UnityEngine.UI;

namespace Holysoft.Gameplay.UI
{
    public class ScrollbarStatUI : CappedStatUI
    {
        [SerializeField]
        private Scrollbar m_Scrollbar;

        private float m_maxValue;

        protected override float maxValue { set => m_maxValue = value; }
        protected override float currentValue
        {
            set
            {
                var uiValue = value / m_maxValue;
                m_Scrollbar.value = float.IsNaN(uiValue) ? 0 : uiValue;
            }
        }
    }
}