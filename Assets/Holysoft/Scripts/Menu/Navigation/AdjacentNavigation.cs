using Holysoft.Event;
using Holysoft.Menu;
using Holysoft.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Holysoft.Menu
{
    public abstract class AdjacentNavigation : UIBehaviour, IAdjacentNavigation, IAdjacentNavigationEvents
    {
        public event EventAction<EventActionArgs> FirstItemReached;
        public event EventAction<EventActionArgs> LastItemReached;
        public event EventAction<EventActionArgs> NavigatingItem;

        protected int m_currentNavigationIndex;
        protected abstract int lastNavigationIndex { get; }

        public virtual void Next()
        {
            if (m_currentNavigationIndex < lastNavigationIndex)
            {
                GoToNextItem();
                if (m_currentNavigationIndex == lastNavigationIndex)
                {
                    LastItemReached?.Invoke(this, EventActionArgs.Empty);
                }
                else
                {
                    NavigatingItem?.Invoke(this, EventActionArgs.Empty);
                }
            }
        }

        public virtual void Previous()
        {
            if (m_currentNavigationIndex > 0)
            {
                GoToPreviousItem();
                if (m_currentNavigationIndex == 0)
                {
                    FirstItemReached?.Invoke(this, EventActionArgs.Empty);
                }
                else
                {
                    NavigatingItem?.Invoke(this, EventActionArgs.Empty);
                }
            }
        }

        protected abstract void GoToPreviousItem();
        protected abstract void GoToNextItem();

        protected void CallFirstItemReached()
        {
            FirstItemReached?.Invoke(this, EventActionArgs.Empty);
        }

        protected void CallLastItemReached()
        {
            LastItemReached?.Invoke(this, EventActionArgs.Empty);
        }

        protected virtual void Awake()
        {
            m_currentNavigationIndex = 0;
        }

        protected virtual void Start()
        {
            FirstItemReached?.Invoke(this, EventActionArgs.Empty);
        }
    }

}