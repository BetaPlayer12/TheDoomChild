using TMPro;
using UnityEngine;

namespace Holysoft.UI
{
    public class LabelMaterialHighlight : UIHighlight
    {
        [SerializeField]
        private TextMeshProUGUI m_target;
        [SerializeField]
        private SwitchMaterialData m_data;

        public override void Normalize()
        {
            m_target.fontSharedMaterial = m_data.deselected;
        }

        public override void Highlight()
        {
            m_target.fontSharedMaterial = m_data.selected;
        }

        public override void UseHighlightState()
        {
            m_target.fontSharedMaterial = m_data.selected;
        }

        public override void UseNormalizeState()
        {
            m_target.fontSharedMaterial = m_data.deselected;
        }

        private void OnValidate()
        {
            if (m_target == null)
            {
                m_target = GetComponentInChildren<TextMeshProUGUI>();
            }
            if (m_target != null && m_data != null)
            {
                m_target.material = m_data.deselected;
            }
        }
    }
}