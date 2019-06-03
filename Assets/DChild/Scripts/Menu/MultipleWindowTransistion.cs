using Holysoft.UI;

namespace DChild.Menu
{
    public class MultipleWindowTransistion : MultipleCanvasTransistion
    {
        protected override void Start()
        {
            m_windowTransistion = MenuSystem.windowTransistion;
            base.Start();
        }
    }

}