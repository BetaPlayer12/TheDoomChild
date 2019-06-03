using System;
using Holysoft.Event;
using Holysoft.Menu;
using Holysoft.UI;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Extras
{
    public class ExtrasImageShowcase : UICanvas, IAdjacentNavigation, IAdjacentNavigationEvents
    {
        [SerializeField]
        private Image m_showcase;
        private ExtrasImageShowcaseHandler m_handler;
        private bool m_inMiddleOfItems;

        public event EventAction<EventActionArgs> FirstItemReached;
        public event EventAction<EventActionArgs> LastItemReached;
        public event EventAction<EventActionArgs> NavigatingItem;

        public void Next() => m_handler.Next();
        public void Previous() => m_handler.Previous();

        public void SetNavgationHandler(ExtrasImageShowcaseHandler navigation)
        {
            if (m_handler != null)
            {
                m_handler.FirstItemReached -= OnFirstItemReached;
                m_handler.LastItemReached -= OnLastItemReached;
                if (m_inMiddleOfItems == false)
                {
                    m_handler.NavigatingItem -= OnNavigatingItem;
                }
            }
            navigation.FirstItemReached += OnFirstItemReached;
            navigation.LastItemReached += OnLastItemReached;
            m_handler = navigation;

            //To Reset Visuals
            m_inMiddleOfItems = true;
            NavigatingItem?.Invoke(this, EventActionArgs.Empty);
        }

        private void OnNavigatingItem(object sender, EventActionArgs eventArgs)
        {
            NavigatingItem?.Invoke(this, eventArgs);
            m_inMiddleOfItems = true;
            m_handler.NavigatingItem -= OnNavigatingItem;
        }

        private void OnLastItemReached(object sender, EventActionArgs eventArgs)
        {
            LastItemReached?.Invoke(this, eventArgs);
            m_inMiddleOfItems = false;
            m_handler.NavigatingItem += OnNavigatingItem;
        }

        private void OnFirstItemReached(object sender, EventActionArgs eventArgs)
        {
            FirstItemReached?.Invoke(this, eventArgs);
            m_inMiddleOfItems = false;
            m_handler.NavigatingItem += OnNavigatingItem;
        }

        public void SetShowcase(Sprite sprite)
        {
            m_showcase.sprite = sprite;
            m_showcase.color = Color.white;
            m_showcase.preserveAspect = true;
        }
    }
}