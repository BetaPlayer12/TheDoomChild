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

        private Scrollbar m_scrollBar;

        public void SyncWithPageHandle()
        {
            m_scrollBar.value = (m_pageHandle.currentPage - 1) * m_sliderValueToPageIndexModifier;
        }

        public void OnSliderValueChange(float sliderValue)
        {
            m_pageHandle.PageChange -= OnPageChange;
            var page = Mathf.FloorToInt(m_scrollBar.value / m_sliderValueToPageIndexModifier) + 1;
            m_pageHandle.SetPage(page);
            m_pageHandle.PageChange += OnPageChange;
        }

        private void OnPageChange(object sender, EventActionArgs eventArgs)
        {
            SyncWithPageHandle();
        }

        private void Start()
        {
            m_scrollBar = GetComponent<Scrollbar>();
            m_scrollBar.onValueChanged.AddListener(OnSliderValueChange);
            m_pageHandle.PageChange += OnPageChange;
            m_sliderValueToPageIndexModifier = 1f / m_pageHandle.GetTotalPages();
        }
    }
}