using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Bestiary
{
    public class AdaptivePageSlider : SerializedMonoBehaviour
    {
        [SerializeField]
        private IPageHandle m_pageHandle;
        private float m_sliderValueToPageIndexModifier;

        private Slider m_slider;

        public void SyncWithPageHandle()
        {
            m_slider.value = (m_pageHandle.currentPage - 1) * m_sliderValueToPageIndexModifier;
        }

        public void OnSliderValueChange(float sliderValue)
        {
            m_pageHandle.PageChange -= OnPageChange;
            var page = Mathf.FloorToInt(m_slider.value / m_sliderValueToPageIndexModifier) + 1;
            m_pageHandle.SetPage(page);
            m_pageHandle.PageChange += OnPageChange;
        }

        private void OnPageChange(object sender, EventActionArgs eventArgs)
        {
            SyncWithPageHandle();
        }

        private void Awake()
        {
            m_slider = GetComponent<Slider>();
            m_pageHandle.PageChange += OnPageChange;
            m_sliderValueToPageIndexModifier = 1f / m_pageHandle.GetTotalPages();
        }
    }
}