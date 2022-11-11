using Holysoft.Event;
using UnityEngine;

namespace DChild.Menu
{
    [System.Serializable]
    public class ConfirmationRequestHandle
    {
        [SerializeField]
        private ConfirmationHandler m_confirmation;
        [SerializeField]
        private string m_confirmationMessage;

        public void Execute(EventAction<EventActionArgs> OnAffirmation) 
        {
            m_confirmation.RequestConfirmation(OnAffirmation, m_confirmationMessage);
        }
    }

}