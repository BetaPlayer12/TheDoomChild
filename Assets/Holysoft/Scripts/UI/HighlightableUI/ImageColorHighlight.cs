using UnityEngine;
using UnityEngine.UI;

namespace Holysoft.UI
{
    public class ImageColorHighlight : UIHighlight
    {
        [SerializeField]
        private Image m_targetGraphic;
        [SerializeField]
        private Color m_deselected = Color.white;
        [SerializeField]
        private Color m_selected = Color.red;

        public override void Normalize()
        {
            m_targetGraphic.color = m_deselected;
        }

        public override void Highlight()
        {
            m_targetGraphic.color = m_selected;
        }

        public override void UseHighlightState()
        {
            m_targetGraphic.color = m_selected;

        }

        public override void UseNormalizeState()
        {
            m_targetGraphic.color = m_deselected;
        }

        private void OnValidate()
        {
            if (m_targetGraphic == null)
            {
                m_targetGraphic = GetComponentInChildren<Image>();
            }
            if (m_targetGraphic != null)
            {
                m_targetGraphic.color = m_deselected;
            }
        }
    }
}