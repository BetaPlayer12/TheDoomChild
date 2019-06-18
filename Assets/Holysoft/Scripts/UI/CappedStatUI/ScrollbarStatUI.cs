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
        protected override float currentValue { set => m_Scrollbar.value = value / m_maxValue; }
    }
}