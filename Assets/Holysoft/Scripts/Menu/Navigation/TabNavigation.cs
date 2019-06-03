using Holysoft.Event;
using Holysoft.UI;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Holysoft.Menu
{
    public class TabNavigation : PanelNavigationHandler<TabNavigation.Panel>
    {
        [System.Serializable]
        public class Panel : INavigationPanel
        {
            [SerializeField]
            private UICanvas m_canvas;
            [SerializeField]
            private TabButton m_button;

            public UICanvas canvas => m_canvas;
            public TabButton button => m_button;

            public void Open()
            {
                m_canvas.Show();
                m_button.Highlight();
            }

            public void Close()
            {
                m_canvas.Hide();
                m_button.Normalize();
            }

            public void ForceClose()
            {
                m_canvas.Hide();
                m_button.UseNormalizeState();
            }

            public void ForceOpen()
            {
                m_canvas.Show();
                m_button.UseHighlightState();
            }

#if UNITY_EDITOR
            public string name => m_canvas.name;
#endif
        }

        [SerializeField, PropertyOrder(10), OnValueChanged("OnPanelChange")]
        protected Panel[] m_panels;

        protected override Panel[] panels => m_panels;

        private void OnButtonClick(object sender, NavigationButtonEventArgs eventArgs)
        {
            if (m_hasActivePanel)
            {
                m_active.Close();
            }
            m_active = m_panels[eventArgs.buttonID];
            m_hasActivePanel = true;
            m_active.Open();
        }

        private void Awake()
        {
            for (int i = 0; i < m_panels.Length; i++)
            {
                m_panels[i].button.ButtonClick += OnButtonClick;
            }
        }

        private void OnValidate()
        {
            for (int i = 0; i < m_panels.Length; i++)
            {
                m_panels[i].button.SetButtonID(i);
            }
        }

#if UNITY_EDITOR
        protected override void OnPanelChange()
        {
            base.OnPanelChange();
            if (panels != null)
            {
                m_activePanelName = m_panels[m_defaultPanel].canvas.name;
            }
        }
#endif
    }
}
