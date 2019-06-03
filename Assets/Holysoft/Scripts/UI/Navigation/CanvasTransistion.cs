using System;
using Holysoft.Event;
using UnityEngine;

namespace Holysoft.UI
{
    [RequireComponent(typeof(UIStylishCanvas))]
    public abstract class CanvasTransistion : MonoBehaviour
    {
        protected UIStylishCanvas m_canvas;
        protected bool m_hasPlayedExitTransistion;

        public event EventAction<EventActionArgs> ExitTransistionEnd;
        public event EventAction<EventActionArgs> EnterTransistionEnd;

        protected abstract bool OnRequestCanvasHide(object sender, EventActionArgs eventArgs);
        protected abstract void OnCanvasShow(object sender, EventActionArgs eventArgs);

        protected void CallExitTransistionEnd() => ExitTransistionEnd?.Invoke(this, EventActionArgs.Empty);
        protected void CallEnterTransistionEnd() => EnterTransistionEnd?.Invoke(this, EventActionArgs.Empty);

        protected virtual void Awake()
        {
            m_canvas = GetComponent<UIStylishCanvas>();
            m_canvas.RequestCanvasHide = OnRequestCanvasHide;
            m_canvas.CanvasShow += OnCanvasShow;
            m_hasPlayedExitTransistion = false;
        }
    }
}