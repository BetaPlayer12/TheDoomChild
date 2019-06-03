using System;
using Holysoft.Event;
using Holysoft.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Holysoft.Menu
{
    public class WindowNavigation : MonoBehaviour, IMenuNavigation
    {
        [System.Serializable]
        public class NavigationInfo
        {
            [SerializeField]
            private UICanvas m_canvas;
            [SerializeField]
            private NavigationButton m_button;
            [SerializeField]
            private bool m_isScreen;

            public UICanvas canvas => m_canvas;
            public NavigationButton button => m_button;
            public bool isScreen => m_isScreen;

            public void Open()
            {
                m_canvas.Show();
            }

            public void Close()
            {
                m_canvas.Hide();
            }
        }

        [SerializeField]
        protected UICanvas m_mainCanvas;
        [SerializeField]
        [InfoBox("IF Canvases has Transistion, this will make the transistion effort easier")]
        private WindowTransistion m_windowTransistion;
        [SerializeField]
        [PropertyOrder(10)]
        private NavigationInfo[] m_navigatables;

        public UICanvas mainCanvas => m_mainCanvas;

        public event UnityEventAction<UICanvas> CanvasOpen;

        protected virtual void NavigateTo(NavigationInfo current)
        {
            if (current.isScreen)
            {
                if (m_windowTransistion == null)
                {
                    m_mainCanvas.Hide();
                    current.Open();
                }
                else
                {
                    m_windowTransistion.SetCanvases(m_mainCanvas, current.canvas);
                    m_windowTransistion.StartTransistion();
                }
            }
            else
            {
                current.Open();
            }
        }

        private void OnButtonClick(object sender, NavigationButtonEventArgs eventArgs)
        {
            var current = m_navigatables[eventArgs.buttonID];
            CanvasOpen?.Invoke(this, current.canvas);
            NavigateTo(current);
        }

        private void Awake()
        {
            for (int i = 0; i < m_navigatables.Length; i++)
            {
                m_navigatables[i].button.ButtonClick += OnButtonClick;
            }
        }

#if UNITY_EDITOR
        public void CloseAll()
        {
            for (int i = 0; i < m_navigatables.Length; i++)
            {

                m_navigatables[i].Close();
                UIUtility.CloseCanvas(m_navigatables[i].canvas);
            }
        }
#endif

        private void OnValidate()
        {
            if (m_mainCanvas == null)
            {
                m_mainCanvas = GetComponent<UICanvas>();
            }
            if (m_navigatables != null)
            {
                for (int i = 0; i < m_navigatables.Length; i++)
                {
                    m_navigatables[i].button?.SetButtonID(i);
                }
            }
        }
    }
}
