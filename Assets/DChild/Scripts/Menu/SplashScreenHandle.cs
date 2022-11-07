using DChild.Menu.MainMenu;
using DChild.Temp;
using Doozy.Runtime.Signals;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace DChild.Menu
{
    public class SplashScreenHandle : MonoBehaviour
    {
        [SerializeField]
        private SplashScreenLabelAnimation m_promptAnimation;
        [SerializeField]
        private SplashScreenAnimation m_animation;
        [SerializeField]
        private SignalSender m_hideViewSignal;
        [SerializeField, Min(0)]
        private float m_hideViewSignalSendDelay;

        /// <summary>
        /// This Should be called by a Callback when the Input Prompt is Shown
        /// This is most likely called by a UIContainer in the scene
        /// </summary>
        public void ListenForInput()
        {
            StopAllCoroutines();
            InputSystem.onAnyButtonPress.CallOnce(OnAnyKeyDown);
        }

        private IEnumerator DelayedSignalSendRoutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            m_hideViewSignal.SendSignal();
        }

        private void OnAnyKeyDown(InputControl obj)
        {
            m_promptAnimation.Play();
            m_animation.TransistionAnimation();
            StartCoroutine(DelayedSignalSendRoutine(m_hideViewSignalSendDelay));
        }
    }
}