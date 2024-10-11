using Doozy.Runtime.UIManager.Containers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.UI
{
    public class NewContentIconMainTabHandle : MonoBehaviour
    {
        [SerializeField]
        private NewContentIconTabHandle[] m_listOfTabNotificationIcons;
        [SerializeField]
        private UIContainer m_container;

        public void SetUpTabsIcons()
        {
            for(int i = 0; i < m_listOfTabNotificationIcons.Length; i++)
            {
                if (!m_listOfTabNotificationIcons[i].hasAlert)
                    m_container.InstantShow();

            }
        }

        public void ResetTabIcons()
        {

        }
    }

}
