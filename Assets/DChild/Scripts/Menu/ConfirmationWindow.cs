using Holysoft.Event;
using Holysoft.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DChild.Menu
{
    public class ConfirmationWindow : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_message;

        public event EventAction<EventActionArgs> RequestAffirmed;
        public event EventAction<EventActionArgs> RequestDeclined;

        public void SetMessage(string message)
        {
            m_message.text = message;
        }

        public void Affirm()
        {
            RequestAffirmed?.Invoke(this, EventActionArgs.Empty);
        }

        public void Decline()
        {
            RequestDeclined?.Invoke(this, EventActionArgs.Empty);
        }

    }
}