using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Holysoft.UI
{
    public abstract class UICanvas : UIElement, IUICanvas
    {
        [SerializeField, ReadOnly]
        protected Canvas m_canvas;
        [SerializeField, ReadOnly, HorizontalGroup("Canvas Group")]
        private CanvasGroup m_canvasGroup;
#if UNITY_EDITOR
        [SerializeField, HideLabel, HorizontalGroup("Canvas Group")]
        private bool m_findGroup = true;
#endif

        public bool isShown => m_canvas.enabled;

        public event EventAction<EventActionArgs> CanvasShow;
        public event EventAction<EventActionArgs> CanvasHide;

        protected virtual bool canHide => m_canvas.enabled == true;
        protected virtual bool canShow
        {
            get
            {
                return m_canvas.enabled == false;
            }
        }

        public override void Hide()
        {
            if (canHide)
            {
                Disable();
                CanvasHide?.Invoke(this, EventActionArgs.Empty);
            }
            if (m_canvasGroup)
            {
                m_canvasGroup.interactable = false;
            }
        }

        public override void Show()
        {
            if (canShow)
            {
                Enable();
                CanvasShow?.Invoke(this, EventActionArgs.Empty);
            }
        }


        [PropertySpace]
        [Button("Enable")]
        public virtual void Enable()
        {
            m_canvas.enabled = true;
            enabled = true;
            enabled = true;
            if (m_canvasGroup != null)
            {
                m_canvasGroup.interactable = true;
                m_canvasGroup.blocksRaycasts = true;
            }
            //Show();
        }

        [Button("Disable")]
        public virtual void Disable()
        {
            m_canvas.enabled = false;
            enabled = false;
            if (m_canvasGroup != null)
            {
                m_canvasGroup.interactable = false;
                m_canvasGroup.blocksRaycasts = false;
            }
            //Hide();
        }

        protected void CallCanvasHide()
        {
            CanvasHide?.Invoke(this, EventActionArgs.Empty);
        }

        public void CallCanvasShow()
        {
            CanvasShow?.Invoke(this, EventActionArgs.Empty);
        }

        protected void AssignComponents()
        {
            m_canvas = GetComponentInParent<Canvas>();
#if UNITY_EDITOR
            m_canvasGroup = m_findGroup ? GetComponentInParent<CanvasGroup>() : null;
#endif
        }

        private void OnValidate()
        {
            AssignComponents();
        }
    }
}