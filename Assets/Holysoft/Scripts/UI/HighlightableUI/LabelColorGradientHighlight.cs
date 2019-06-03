using TMPro;
using UnityEngine;

namespace Holysoft.UI
{
    public class LabelColorGradientHighlight : UIHighlight
    {
        [SerializeField]
        private TextMeshProUGUI m_target;
        [SerializeField]
        private LabelColorGradientData m_data;

        public override void Normalize()
        {
            m_target.colorGradientPreset = m_data.deselected;
        }

        public override void Highlight()
        {
            m_target.colorGradientPreset = m_data.selected;
        }

        public override void UseHighlightState()
        {
            m_target.colorGradientPreset = m_data.selected;
        }

        public override void UseNormalizeState()
        {
            m_target.colorGradientPreset = m_data.deselected;
        }

        private void OnValidate()
        {
            if (m_target == null)
            {
                m_target = GetComponentInChildren<TextMeshProUGUI>();
            }
            if (m_target != null && m_data != null)
            {
                m_target.colorGradientPreset = m_data.deselected;
            }
        }
    }
}