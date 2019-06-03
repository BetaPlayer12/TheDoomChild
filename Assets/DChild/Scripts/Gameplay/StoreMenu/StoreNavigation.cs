using Holysoft.Event;
using Holysoft.Menu;
using Holysoft.UI;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
#if UNITY_EDITOR
using Sirenix.Utilities.Editor;
#endif

namespace DChild.Gameplay.Systems
{
    public class StoreNavigation : PanelNavigationHandler<StoreNavigation.Page>
    {
        [System.Serializable]
        public class Page : EnumElement<StorePage>, INavigationPanel
        {
            [SerializeField, PropertyOrder(1)]
            private UICanvas m_canvas;
            [SerializeField, PropertyOrder(1)]
            private NavigationButton m_button;

            public UICanvas canvas => m_canvas;
            public NavigationButton button => m_button;


            public void Open()
            {
                m_canvas.Show();
            }

            public void Close()
            {
                m_canvas.Hide();
            }

            public void ForceClose()
            {
                m_canvas.Hide();
            }

            public void ForceOpen()
            {
                m_canvas.Show();
            }
#if UNITY_EDITOR
            string INavigationPanel.name => m_name.ToString();
#endif
        }

        [SerializeField, PropertyOrder(10), OnValueChanged("OnPanelChange"), ListDrawerSettings(HideAddButton = true, OnTitleBarGUI = "DrawUpdateButton", HideRemoveButton = true)]
        protected Page[] m_pages;

        protected override Page[] panels => m_pages;

        public void Open(StorePage page)
        {
            Open((int)page);
        }

        private void OnButtonClick(object sender, NavigationButtonEventArgs eventArgs)
        {
            Open(eventArgs.buttonID);
        }

        private void Awake()
        {
            for (int i = 0; i < m_pages.Length; i++)
            {
                m_pages[i].button.ButtonClick += OnButtonClick;
            }
        }

        private void OnValidate()
        {
            for (int i = 0; i < m_pages.Length; i++)
            {
                m_pages[i].button?.SetButtonID(i);
                if (Application.isPlaying)
                {
                    m_pages[i].ForceClose();
                }
            }
        }

#if UNITY_EDITOR
        protected override void OnPanelChange()
        {
            base.OnPanelChange();
            if (m_pages != null)
            {
                for (int i = 0; i < m_pages.Length; i++)
                {
                    m_pages[i].name = (StorePage)i;
                }
            }
        }

        private void DrawUpdateButton()
        {
            if (SirenixEditorGUI.ToolbarButton(EditorIcons.Refresh))
            {
                if (m_pages.Length != (int)StorePage._Count)
                {
                    Page[] newPages = new Page[(int)StorePage._Count];
                    if (m_pages == null)
                    {
                        for (int i = 0; i < newPages.Length; i++)
                        {
                            newPages[i] = new Page();
                        }
                    }
                    else
                    {
                        var length = m_pages.Length > newPages.Length ? newPages.Length : m_pages.Length;
                        Array.Copy(m_pages, newPages, length);
                    }

                    m_pages = newPages;
                }

                EnumList.AlignListElementTypeToEnums<Page, StorePage>(m_pages);
            }
        }
#endif
    }
}