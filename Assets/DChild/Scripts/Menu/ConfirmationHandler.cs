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
            m_confirmationWindow.RequestAffirmed += m_listener;
            m_isListenerSubscribed = true;
            m_confirmationWindow.SetMessage(message);
            m_confirmationWindow.Show();
        }

        private void UnsubcribeListner()
        {
            m_confirmationWindow.RequestAffirmed -= m_listener;
            m_listener = null;
            m_isListenerSubscribed = false;
        }

        private void OnRequesDeclined(object sender, EventActionArgs eventArgs)
        {
            if (m_listener != null)
            {
                UnsubcribeListner();
            }
            m_confirmationWindow.Hide();
            MenuSystem.backTracker.RemoveLastStack();
            Debug.Log("Hide");
        }


        private void OnRequestAffirmed(object sender, EventActionArgs eventArgs)
        {
            m_confirmationWindow.Hide();
            MenuSystem.backTracker.RemoveLastStack();
        }

        private void OnConfirmationHide(object sender, EventActionArgs eventArgs)
        {
            if (m_isListenerSubscribed)
            {
                UnsubcribeListner();
            }
        }

        private void Awake()
        {
            m_confirmationWindow.CanvasHide += OnConfirmationHide;
            m_confirmationWindow.RequestAffirmed += OnRequestAffirmed;
            m_confirmationWindow.RequestDeclined += OnRequesDeclined;
        }
    }

}