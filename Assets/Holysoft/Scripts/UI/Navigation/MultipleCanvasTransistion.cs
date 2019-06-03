using System;
using Holysoft.Event;
using Holysoft.Menu;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Holysoft.UI
{
    [RequireComponent(typeof(UIStylishCanvas))]
    public class MultipleCanvasTransistion : CanvasTransistion
    {
        [System.Serializable]
        public class TransistionInfo
        {
            [SerializeField]
            private UICanvas m_canvas;
            [SerializeField]
            private UITransistionCanvas m_transistionCanvas;

            public UICanvas canvas => m_canvas;
            public UITransistionCanvas transistionCanvas => m_transistionCanvas;
        }

        [SerializeField]
        protected WindowTransistion m_windowTransistion;
        [FoldoutGroup("Specific Transistions")]
        [SerializeField]
        private TransistionInfo[] m_enterTransistion;
        [FoldoutGroup("Specific Transistions")]
        [SerializeField]
        private TransistionInfo[] m_exitTransistion;

        private UITransistionCanvas m_currentTransition;
        private bool m_usingEnterTransistion;

        private void OnTransitionStart(object sender, WindowTransistionEventArgs eventArgs)
        {
            if (m_canvas == eventArgs.toOpen)
            {
                m_usingEnterTransistion = true;
                var transistionInfo = GetTransistionForCanvas(m_enterTransistion, eventArgs.toClose);
                m_currentTransition = transistionInfo?.transistionCanvas ?? null;

            }
            else if (m_canvas == eventArgs.toClose)
            {
                m_usingEnterTransistion = false;
                var transistionInfo = GetTransistionForCanvas(m_exitTransistion, eventArgs.toOpen);
                m_currentTransition = transistionInfo?.transistionCanvas ?? null;
                if (m_currentTransition != null)
                {
                    m_currentTransition.transistion.TransistionEnd += OnExitTransistionEnd;
                }
            }
        }

        private void OnExitTransistionEnd(object sender, EventActionArgs eventArgs)
        {
            m_hasPlayedExitTransistion = true;
            m_canvas.Hide();
        }

        protected override void OnCanvasShow(object sender, EventActionArgs eventArgs)
        {
            m_hasPlayedExitTransistion = false;
            m_usingEnterTransistion = true;
            if (m_usingEnterTransistion)
            {
                m_currentTransition?.Show();
            }
        }

        protected override bool OnRequestCanvasHide(object sender, EventActionArgs eventArgs)
        {
            if (m_usingEnterTransistion == false)
            {
                if (m_currentTransition == null || m_hasPlayedExitTransistion)
                {
                    return true;
                }
                else
                {
                    m_currentTransition.Show();
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private TransistionInfo GetTransistionForCanvas(TransistionInfo[] infos, UICanvas canvas)
        {
            for (int i = 0; i < infos.Length; i++)
            {
                if (infos[i].canvas == canvas)
                {
                    return infos[i];
                }
            }

            return null;
        }

        protected virtual void Start()
        {
            m_windowTransistion.TransistionStart += OnTransitionStart;
        }

        private void OnValidate()
        {
            ComponentUtility.AssignNullComponent(this, ref m_windowTransistion);
        }
    }
}