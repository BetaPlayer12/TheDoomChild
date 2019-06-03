using System;
using Holysoft.Event;
using Holysoft.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Holysoft.Menu
{
    public class WindowTransistionHandler : WindowTransistion, IWindowTransistionEvent
    {
        private CanvasTransistion m_toCloseTransistion;
        private CanvasTransistion m_toOpenTransistion;

        private WindowTransistionEventArgs m_eventArgs;
        private bool m_endTransistionOnHide;

        public event EventAction<WindowTransistionEventArgs> TransistionEnd;

        public override void StartTransistion()
        {
            CallTransistionStart();
            m_toClose.CanvasHide += OnCanvasHide;
            SubscibeToOpenCanvasEvents();
            m_toClose.Hide();
        }

        private void SubscibeToOpenCanvasEvents()
        {
            if (m_toOpen.isShown)
            {
                m_endTransistionOnHide = false;
                m_toCloseTransistion = m_toClose.GetComponent<CanvasTransistion>();
                if (m_toCloseTransistion == null)
                {
                    m_endTransistionOnHide = true;
                }
                else
                {
                    m_toCloseTransistion.ExitTransistionEnd += OnExitTransistionEnd;
                }
            }
            else
            {
                m_endTransistionOnHide = false;
                m_toOpenTransistion = m_toOpen.GetComponent<CanvasTransistion>();
                if (m_toOpenTransistion == null)
                {
                    m_toOpen.CanvasShow += OnCanvasOpened;
                }
                else
                {
                    m_toOpenTransistion.EnterTransistionEnd += OnEnterTransistionEnd;
                }
            }
        }

        private void OnExitTransistionEnd(object sender, EventActionArgs eventArgs)
        {
            TransistionEnd?.Invoke(this, m_eventArgs);
            m_endTransistionOnHide = false;
            m_toCloseTransistion.ExitTransistionEnd -= OnExitTransistionEnd;
            m_toCloseTransistion = null;
        }

        private void OnEnterTransistionEnd(object sender, EventActionArgs eventArgs)
        {
            TransistionEnd?.Invoke(this, m_eventArgs);
            m_toOpenTransistion.EnterTransistionEnd -= OnEnterTransistionEnd;
            m_toOpenTransistion = null;
        }

        private void OnCanvasOpened(object sender, EventActionArgs eventArgs)
        {
            TransistionEnd?.Invoke(this, m_eventArgs);
            m_toOpen.CanvasShow -= OnCanvasOpened;
            m_toOpen = null;
        }

        protected virtual void OnCanvasHide(object sender, EventActionArgs eventArgs)
        {
            m_toOpen.Show();
            m_toClose.CanvasHide -= OnCanvasHide;
            m_toClose = null;
            if (m_endTransistionOnHide)
            {
                TransistionEnd?.Invoke(this, m_eventArgs);
            }
        }
    }
}