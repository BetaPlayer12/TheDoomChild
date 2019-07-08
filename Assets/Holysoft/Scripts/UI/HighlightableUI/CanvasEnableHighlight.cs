using UnityEngine;

namespace Holysoft.UI
{
    public class CanvasEnableHighlight : UIHighlight
    {
        [SerializeField]
        private Canvas m_canvas;
        [SerializeField]
        private bool m_enableOnHightlight = true;

        public override void Highlight()
        {
            m_canvas.enabled = m_enableOnHightlight;
        }

        public override void Normalize()
        {
            m_canvas.enabled = !m_enableOnHightlight;
        }

        public override void UseHighlightState()
        {
            m_canvas.enabled = m_enableOnHightlight;
        }

        public override void UseNormalizeState()
        {
            m_canvas.enabled = !m_enableOnHightlight;
        }
    }
}