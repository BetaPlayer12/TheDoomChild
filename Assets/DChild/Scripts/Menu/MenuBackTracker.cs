using Holysoft.Menu;
using Holysoft.UI;

namespace DChild.Menu
{
    public class MenuBackTracker : CanvasBackTracker
    {
        protected UICanvas m_toClose;
        protected UICanvas m_toOpen;

        public UICanvas toClose => m_toClose;
        public UICanvas toOpen => m_toOpen;

        public void RecordBacktrackCanvases()
        {
            m_toClose = m_stack[0];
            m_toOpen = m_stack[1];
        }        
    }
}