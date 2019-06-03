using Holysoft.Menu;
using Holysoft.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Menu
{
    public class MenuTransistion : WindowTransistion
    {
        public override void StartTransistion()
        {
            MenuSystem.windowTransistion.SetCanvases(m_toClose, m_toOpen);
            CallTransistionStart();
            MenuSystem.windowTransistion.StartTransistion();
        }
    }

}