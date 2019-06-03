using Holysoft.Event;
using Holysoft.Menu;
using UnityEngine;

namespace Holysoft.UI
{
    [RequireComponent(typeof(IAdjacentNavigationEvents))]
    public abstract class AdjacentNavigationButtonVisual : MonoBehaviour
    {
        protected IAdjacentNavigationEvents m_sender;

        protected abstract void OnNavigatingItem(object sender, EventActionArgs eventArgs);
        protected abstract void OnLastItemReached(object sender, EventActionArgs eventArgs);
        protected abstract void OnFirstItemReached(object sender, EventActionArgs eventArgs);

        protected virtual void Awake()
        {
            m_sender = GetComponentInParent<IAdjacentNavigationEvents>();
            m_sender.FirstItemReached += OnFirstItemReached;
            m_sender.LastItemReached += OnLastItemReached;
            m_sender.NavigatingItem += OnNavigatingItem;
        }
    }
}