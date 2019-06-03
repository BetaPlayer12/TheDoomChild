using Holysoft.Event;
using UnityEngine;
using UnityEngine.UI;

namespace Holysoft.UI
{
    public class DisablingNavigationButton : AdjacentNavigationButtonVisual
    {
        [SerializeField]
        private Button m_next;
        [SerializeField]
        private Button m_previous;

        protected override void OnFirstItemReached(object sender, EventActionArgs eventArgs)
        {
            m_next.interactable = true;
            m_previous.interactable = false;
            m_sender.NavigatingItem += OnNavigatingItem;
        }

        protected override void OnLastItemReached(object sender, EventActionArgs eventArgs)
        {
            m_next.interactable = false;
            m_previous.interactable = true;
            m_sender.NavigatingItem += OnNavigatingItem;
        }

        protected override void OnNavigatingItem(object sender, EventActionArgs eventArgs)
        {
            m_next.interactable = true;
            m_previous.interactable = true;
            m_sender.NavigatingItem -= OnNavigatingItem;
        }
    }
}