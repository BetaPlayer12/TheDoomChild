using Holysoft.Event;
using Holysoft.Menu;
using UnityEngine;

namespace Holysoft.UI
{
    [RequireComponent(typeof(IAdjacentNavigation))]
    public abstract class AdjacentNavigationButtonVisual : MonoBehaviour
    {
        protected IAdjacentNavigation m_sender;

        protected abstract void OnNavigatingItem(object sender, EventActionArgs eventArgs);
        protected abstract void OnLastItemReached(object sender, EventActionArgs eventArgs);
        protected abstract void OnFirstItemReached(object sender, EventActionArgs eventArgs);

        protected virtual void Awake()
        {
            m_sender = GetComponentInParent<IAdjacentNavigation>();
            m_sender.FirstItemReached += OnFirstItemReached;
            m_sender.LastItemReached += OnLastItemReached;
            m_sender.NavigatingItem += OnNavigatingItem;
        }
    }
}