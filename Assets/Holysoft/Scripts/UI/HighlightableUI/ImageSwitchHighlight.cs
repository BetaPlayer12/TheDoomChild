using UnityEngine;
using UnityEngine.UI;

namespace Holysoft.UI
{
    public class ImageSwitchHighlight : UIHighlight
    {
        [SerializeField]
        private Image m_targetGraphic;
        [SerializeField]
        private ImageSwitchHighlightData m_data;

        public override void Normalize()
        {
            m_targetGraphic.sprite = m_data.deselected;
        }

        public override void Highlight()
        {
            m_targetGraphic.sprite = m_data.selected;
        }

        public override void UseHighlightState()
        {
            m_targetGraphic.sprite = m_data.selected;
        }

        public override void UseNormalizeState()
        {
            m_targetGraphic.sprite = m_data.deselected;
        }

        private void OnValidate()
        {
            if (m_targetGraphic == null)
            {
                m_targetGraphic = GetComponentInChildren<Image>();
            }
            if (m_targetGraphic != null && m_data != null)
            {
                m_targetGraphic.sprite = m_data.deselected;
            }
        }
    }
}