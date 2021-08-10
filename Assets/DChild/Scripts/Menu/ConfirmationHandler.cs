using System;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Menu
{
    public class ConfirmationHandler : MonoBehaviour
    {
        [SerializeField]
        private ConfirmationWindow m_confirmationWindow;
        private EventAction<EventActionArgs> m_listener;
        private bool m_isListenerSubscribed;

        public ConfirmationWindow window => m_confirmationWindow;

        public void RequestConfirmation(EventAction<EventActionArgs> listener, string message)
        {
            m_listener = listener;
           // m_confirmationWindow.RequestAffirmed += m_listener;
            m_isListenerSubscribed = true;
            m_confirmationWindow.SetMessage(message);
        }

        private void OnAffirm(object sender, EventActionArgs eventArgs)
        {
            m_listener?.Invoke(this, EventActionArgs.Empty);
        }

        public void UnsubcribeListener()
        {
            if (m_isListenerSubscribed && m_listener != null)
            {
                //m_confirmationWindow.RequestAffirmed -= m_listener;
                m_listener = null;
                m_isListenerSubscribed = false;
            }
        }

        private void OnConfirmationHide(object sender, EventActionArgs eventArgs)
        {
            if (m_isListenerSubscribed)
            {
                UnsubcribeListener();
            }
        }

        private void Awake()
        {
            m_confirmationWindow.RequestAffirmed += OnAffirm;
        }
    }

}