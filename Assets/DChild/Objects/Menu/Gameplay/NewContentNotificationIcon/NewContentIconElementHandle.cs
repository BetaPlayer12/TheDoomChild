using DChild.Gameplay.Systems;
using Doozy.Runtime.UIManager.Containers;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.UI
{

    [System.Serializable]
    public class NewContentIconElementHandle : NewContentIconHandle
    {
        [SerializeField]
        protected bool m_hasAlert;
        [SerializeField]
        private UIContainer m_container;

        [SerializeField]
        private NewContentIconTabHandle m_newContentIconTab;

        public override bool hasAlert { get => m_hasAlert; protected set => m_hasAlert = value; }

        public override void AlertSeen()
        {
            base.AlertSeen();

            if(m_newContentIconTab != null)
                m_newContentIconTab.CheckSeenDependencies();
        }
     

        public override void ShowAlert()
        {
            m_container.InstantShow();
        }

        public override void HideAlert()
        {
            m_container.InstantHide();
        }

        public void SetNewContentIconTab(NewContentIconTabHandle tabIcon)
        {
            m_newContentIconTab = tabIcon;
        }

        public override UIContentListAlertSaveData SaveData()
        {
            //An Element Should be returning any save Data since another script should handle it, this only contains the state;
            throw new NotImplementedException();
        }

        public override void LoadData(UIContentListAlertSaveData data)
        {
            //An Element Should be returning any save Data since another script should handle it, this only contains the state;
            throw new NotImplementedException();
        }
    }
}
