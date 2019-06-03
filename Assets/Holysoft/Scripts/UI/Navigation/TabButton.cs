namespace Holysoft.UI
{
    public class TabButton : NavigationButton
    {
        private IUIHighlight m_highlight;

        public void Highlight() => m_highlight.Highlight();

        public void Normalize()
        {
            m_highlight.Normalize();
        }

        public void UseHighlightState()
        {
            m_highlight?.UseHighlightState();
        }

        public void UseNormalizeState()
        {
            m_highlight?.UseNormalizeState();
        }
        protected override void Awake()
        {
            base.Awake();
            m_highlight = GetComponent<UIHighlightHandler>();
            if (m_highlight == null)
            {
                m_highlight = GetComponentInChildren<IUIHighlight>();
            }

        }
    }
}
