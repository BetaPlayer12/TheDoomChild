using Doozy.Runtime.UIManager.Containers;
using UnityEngine;

namespace DChild.Gameplay
{
    public abstract class NotificationUI : MonoBehaviour
    {
        private UIContainer m_container;

        public UIContainer container
        {
            get
            {
                if (m_container == null)
                {
                    m_container = GetComponent<UIContainer>();
                }
                return m_container;
            }
        }
    }
}