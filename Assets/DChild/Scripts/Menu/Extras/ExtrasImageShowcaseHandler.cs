using Holysoft.Event;
using Holysoft.Menu;
using Holysoft.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Menu.Extras
{
    public class ExtrasImageShowcaseHandler : AdjacentNavigation
    {
        [SerializeField]
        private ExtrasImageShowcase m_canvas;
        [SerializeField]
        [ReadOnly]
        private ExtrasImage[] m_images;

        protected override int lastNavigationIndex => m_images.Length - 1;

        private void OnImageSelected(object sender, ExtrasImageEventArgs eventArgs)
        {
            if (m_canvas.isShown == false)
            {
                m_canvas.Show();
                m_canvas.SetNavgationHandler(this);
                MenuSystem.backTracker.Stack(m_canvas);
            }
            m_currentNavigationIndex = eventArgs.id;      
            m_canvas.SetShowcase(eventArgs.sprite);

            //To Update Image Showcase of the current ID
            if (m_currentNavigationIndex == 0)
            {
                CallFirstItemReached();
            }
            else if (m_currentNavigationIndex == m_images.Length - 1)
            {
                CallLastItemReached();
            }
        }

        protected override void GoToPreviousItem()
        {
            m_images[m_currentNavigationIndex - 1].Select();
        }

        protected override void GoToNextItem()
        {
            m_images[m_currentNavigationIndex + 1].Select();
        }

        protected override void Awake()
        {
            base.Awake();
            for (int i = 0; i < m_images.Length; i++)
            {
                m_images[i].ImageSelected += OnImageSelected;
            }
        }

        private void OnValidate()
        {
            m_images = GetComponentsInChildren<ExtrasImage>();
            for (int i = 0; i < m_images.Length; i++)
            {
                m_images[i].SetID(i);
            }
        }      
    }
}