using Holysoft.Event;
using Holysoft.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DChild.Menu
{
    public class ConfirmationWindow : UICanvas
    {
        [SerializeField]
        private TextMeshProUGUI m_message;
        private UIHighlightHandler[] m_buttonHighlights;

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

        public override void Enable()
        {
            base.Enable();
            for (int i = 0; i < m_buttonHighlights.Length; i++)
            {
                m_buttonHighlights[i].UseNormalizeState();
            }
        }

        private void Awake()
        {
            m_buttonHighlights = GetComponentsInChildren<UIHighlightHandler>();
        }
    }
}