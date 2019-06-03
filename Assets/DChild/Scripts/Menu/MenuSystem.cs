using Holysoft.Event;
using Holysoft.Menu;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Menu
{
    public class MenuSystem : MonoBehaviour
    {
        public static MenuInput input { get; private set; }
        private static MenuBackTracker m_backtracker;
        private static WindowTransistionHandler m_windowTransistion;

        //public static ICanvasBackTracker canvasTracker => m_canvasTracker;
        public static CanvasBackTracker backTracker => m_backtracker;
        public static WindowTransistion windowTransistion => m_windowTransistion;
        private static ConfirmationHandler confirmationHander;
        private bool m_canBacktrack;

        public static void RequestConfirmation(EventAction<EventActionArgs> listener, string message)
        {
            confirmationHander.RequestConfirmation(listener, message);
            m_backtracker.Stack(confirmationHander.window);
        }

        private void OnTransistionEnd(object sender, WindowTransistionEventArgs eventArgs) => m_canBacktrack = true;

        private void OnTransistionStart(object sender, WindowTransistionEventArgs eventArgs) => m_canBacktrack = false;

        private void BacktrackCanvas()
        {
            if (m_canBacktrack && m_backtracker.canBacktrack && input.backTrack)
            {
                m_backtracker.RecordBacktrackCanvases();
                windowTransistion.SetCanvases(m_backtracker.toClose, m_backtracker.toOpen);
                windowTransistion.StartTransistion();
                m_backtracker.RemoveLastStack();
            }
        }

        private void Awake()
        {
            input = new MenuInput();
            m_backtracker = GetComponent<MenuBackTracker>();
            m_windowTransistion = GetComponent<WindowTransistionHandler>();
            confirmationHander = GetComponent<ConfirmationHandler>();
            m_windowTransistion.TransistionStart += OnTransistionStart;
            m_windowTransistion.TransistionEnd += OnTransistionEnd;
            m_canBacktrack = true;
        }


        private void Update()
        {
            input.Update();
            BacktrackCanvas();
        }

        private void LateUpdate()
        {
            input.LateUpdate();
        }
    }

}