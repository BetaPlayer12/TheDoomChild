using DChild.Gameplay.Systems;
using DChild.UI;
using Doozy.Runtime.UIManager.Containers;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.UI
{
    public class NewContentIconTabHandle : NewContentIconHandle
    {
        private bool m_hasSeenAllDependencies;

        public override bool hasAlert
        {
            get => m_hasSeenAllDependencies || DoesDependenciesHaveAlerts();


            protected set
            {
                if (value == true)
                {
                    m_notificationIconHandle.AlertSeen();
                    m_notificationIconHandle.ShowAlert();
                }
                else
                {
                    m_notificationIconHandle.AlertNotSeen();
                    m_notificationIconHandle.HideAlert();
                }
            }
        }

        [SerializeField]
        private UIContainer m_container;

        [SerializeField, BoxGroup("Easy Populator")]
        private GameObject m_contentListHolder;

        [SerializeField]
        private List<NewContentIconElementHandle> m_listOfDependencies;

        [SerializeField]
        private bool m_isMainTab;

        [SerializeField]
        private NewContentIconElementHandle m_notificationIconHandle;

        [Button, BoxGroup("Easy Populator")]
        private void PopulateDependenciesUsingContentListHolder()
        {
            if (m_contentListHolder != null)
            {
                for (int i = 0; i < m_contentListHolder.transform.childCount; i++)
                {
                    NewContentIconElementHandle button = m_contentListHolder.transform.GetChild(i).GetComponentInChildren<NewContentIconElementHandle>();

                    if (!button.hasAlert)
                    {
                        continue;
                    }

                    if (button.hasAlert)
                    {
                        m_listOfDependencies.Add(button);
                    }
                }
            }
        }

        private void SetParentOfDependencies()
        {
            if(m_contentListHolder != null || m_isMainTab)
            {
                for(int i = 0; i < m_listOfDependencies.Count; i++)
                {
                    NewContentIconElementHandle button = m_listOfDependencies[i];
                    button.SetNewContentIconTab(this);
                }
            }
        }

        private bool DoesDependenciesHaveAlerts()
        {
            for (int i = 0; i < m_listOfDependencies.Count; i++)
            {
                if (m_listOfDependencies[i].hasAlert)
                {
                    return true;
                }
            }
            return false;
        }

        private void UpdateIconVisibility()
        {
            if (m_hasSeenAllDependencies)
            {
                m_notificationIconHandle.AlertSeen();
                m_notificationIconHandle.HideAlert();
            }
            else
            {
                m_notificationIconHandle.AlertNotSeen();
                m_notificationIconHandle.ShowAlert();
            }
        }

        private void EmptyDependencies()
        {
            m_listOfDependencies.Clear();
        }

        [Button]
        private void ShowDependenciesStates()
        {
            foreach (NewContentIconElementHandle icons in m_listOfDependencies)
            {
                Debug.Log(icons.hasAlert);
            }
        }
        public void CheckSeenDependencies()
        {
            m_hasSeenAllDependencies = !DoesDependenciesHaveAlerts();

            UpdateIconVisibility();
        }

        public void SetUpNotificationIcon()
        {
            if (m_listOfDependencies.Count <= 0)
                throw new System.Exception("There is no dependencies in content list in " + this.gameObject.name);
            SetParentOfDependencies();
            UpdateIconVisibility();
        }

        public void ResetNotificationIcon()
        {
            m_listOfDependencies.Clear();
            m_hasSeenAllDependencies = true;
        }

        public override void ShowAlert()
        {
            m_container.InstantShow();
        }

        public override void HideAlert()
        {
            m_container.InstantHide();
        }

        public override void AlertSeen()
        {
            base.AlertSeen();
            for (int i = 0; i < m_listOfDependencies.Count; i++)
            {
                m_listOfDependencies[i].AlertSeen();
            }
        }

        public override void AlertNotSeen()
        {
            base.AlertNotSeen();

            for (int i = 0; i < m_listOfDependencies.Count; i++)
            {
                m_listOfDependencies[i].AlertNotSeen();
            }
        }

        public override UIContentListAlertSaveData SaveData()
        {
            if (IsUsedAsComposite())
            {
                return new CompositeUIContentListAlertSaveData(null);
            }
            else  
            {
                return new BaseUIContentListAlertSavaData(null);
            }
        }

        private bool IsUsedAsComposite()
        {
            bool isUsedAsComposite = false;
            var selfType = typeof(NewContentIconTabHandle);
            for (int i = 0; i < m_listOfDependencies.Count; i++)
            {
                if (m_listOfDependencies[i].GetType() == selfType)
                {
                    isUsedAsComposite = true;
                    break;
                }
            }

            return isUsedAsComposite;
        }

        public override void LoadData(UIContentListAlertSaveData data)
        {
            if (IsUsedAsComposite())
            {
                var castedData = (CompositeUIContentListAlertSaveData)data;

            }
            else
            {
                var castedData = (BaseUIContentListAlertSavaData)data;

            }
        }
    }

}
