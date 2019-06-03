using Holysoft.Event;
using UnityEngine;

namespace Holysoft.UI
{
    public class ConnectedUIElements : UICanvasComponent
    {
        [SerializeField]
        private UIElement[] m_elements;

        protected override void OnHide(object sender, EventActionArgs eventArgs)
        {
            for (int i = 0; i < (m_elements?.Length ?? 0); i++)
            {
                m_elements[i].Hide();
            }
        }

        protected override void OnShow(object sender, EventActionArgs eventArgs)
        {
            for (int i = 0; i <( m_elements?.Length ?? 0); i++)
            {
                m_elements[i].Show();
            }
        }
    }
}