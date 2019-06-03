using System;
using Holysoft.Event;
using UnityEngine;

namespace Holysoft.UI
{
    [RequireComponent(typeof(UIStylishCanvas))]
    public class SingleCanvasTransistion : CanvasTransistion
    {
        [SerializeField]
        private UITransistionCanvas m_enterTransistion;
        [SerializeField]
        private UITransistionCanvas m_exitTransistion;

        protected override bool OnRequestCanvasHide(object sender, EventActionArgs eventArgs)
        {
            if (m_exitTransistion == null || m_hasPlayedExitTransistion)
            {
                return true;
            }
            else
            {
                m_exitTransistion.Show();
                return false;
            }
        }

        protected override void OnCanvasShow(object sender, EventActionArgs eventArgs)
        {
            m_hasPlayedExitTransistion = false;
            m_enterTransistion?.Show();
        }

        private void OnExitTransistionEnd(object sender, EventActionArgs eventArgs)
        {
            m_hasPlayedExitTransistion = true;
            CallExitTransistionEnd();
            m_canvas.Hide();
        }

        private void OnEnterTransistionEnd(object sender, EventActionArgs eventArgs)
        {
            CallEnterTransistionEnd();
        }

        protected override void Awake()
        {
            base.Awake();
            if (m_enterTransistion != null)
            {
                m_enterTransistion.transistion.TransistionEnd += OnEnterTransistionEnd;
            }
            if (m_exitTransistion != null)
            {
                m_exitTransistion.transistion.TransistionEnd += OnExitTransistionEnd;
            }
        }
    }
}