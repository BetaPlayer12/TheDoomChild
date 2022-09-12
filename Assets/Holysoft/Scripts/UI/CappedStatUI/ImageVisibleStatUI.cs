using UnityEngine;
using UnityEngine.UI;

namespace Holysoft.Gameplay.UI
{
    public class ImageVisibleStatUI : CappedStatUI
    {
        [SerializeField]
        private Image m_target;

        private float m_maxValue;

        public override float maxValue { set => m_maxValue = value; }
        public override float currentValue { set => m_target.enabled = value == m_maxValue; }
    }

}