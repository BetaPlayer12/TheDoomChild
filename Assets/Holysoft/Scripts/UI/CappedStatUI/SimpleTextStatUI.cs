using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Holysoft.Gameplay.UI
{
    public class SimpleTextStatUI : CappedStatUI
    {
        [SerializeField]
        private TextMeshProUGUI m_currentValueLabel;
        [SerializeField]
        private TextMeshProUGUI m_maxValueLabel;

        public override float maxValue { set => m_maxValueLabel.text = value.ToString(); }
        public override float currentValue { set => m_currentValueLabel.text = value.ToString(); }
    }
}