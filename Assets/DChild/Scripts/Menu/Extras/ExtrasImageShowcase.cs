using DChild.Menu.Extras;
using Holysoft.Menu;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Extras
{
    public class ExtrasImageShowcase : AdjacentNavigation
    {
#if UNITY_EDITOR
        [SerializeField]
#endif
        private SpriteList m_list;

        [SerializeField]
        private Image m_image;

        protected override int lastNavigationIndex => m_list.count -1;

        public void Showcase(SpriteList list, int index)
        {
            m_list = list;
            m_currentNavigationIndex = index;
            m_image.sprite = m_list.GetSprite(m_currentNavigationIndex);
            if (m_currentNavigationIndex == 0)
            {
                CallFirstItemReached();
            }
            else if (m_currentNavigationIndex == lastNavigationIndex)
            {
                CallLastItemReached();
            }
        }

        protected override void GoToNextItem()
        {
            m_currentNavigationIndex++;
            m_image.sprite = m_list.GetSprite(m_currentNavigationIndex);
        }

        protected override void GoToPreviousItem()
        {
            m_currentNavigationIndex--;
            m_image.sprite = m_list.GetSprite(m_currentNavigationIndex);
        }
    }
}