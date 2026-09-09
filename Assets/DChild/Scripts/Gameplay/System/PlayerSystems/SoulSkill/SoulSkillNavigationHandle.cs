using DChild.Gameplay.Characters.Players.SoulSkills;
using Doozy.Runtime.UIManager.Components;
using DChild.Menu;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace DChild.Gameplay.SoulSkills.UI
{
    public class SoulSkillNavigationHandle : MonoBehaviour
    {
        [SerializeField] private UIScrollbar m_soulScroll;
        [SerializeField] private TextMeshProUGUI m_pageLabel;

        [SerializeField] private SoulSkillListUI m_listUI;
        [SerializeField] private ScrollbarMouseWheel m_mouseWheel;

        private int m_currentPageIndex;

        private int m_totalSections;


        [Button]
        public void SetupScroll(SoulSkillList soulList, int toggleCount)
        {
            m_currentPageIndex = 0;
            m_totalSections = toggleCount > 0 ? Mathf.CeilToInt(soulList.Count / (float)toggleCount) : 0;

            m_soulScroll.numberOfSteps = m_totalSections;
            // Doozy clamps its step count. Keep all pages reachable if the list exceeds that limit.
            if (m_soulScroll.numberOfSteps != m_totalSections)
                m_soulScroll.numberOfSteps = 0;
            m_soulScroll.size = m_totalSections > 0 ? 1f / m_totalSections : 1f;
            // InitializeList populates page zero after setup; do not invoke its callback here.
            m_soulScroll.SetValueWithoutNotify(0f);
            if (m_mouseWheel != null)
                m_mouseWheel.SetStepCount(Mathf.Max(1, m_totalSections));

            UpdatePageLabel(m_totalSections > 0 ? 1 : 0);
        }

        public void HandleScroll()
        {
            if (m_totalSections <= 1)
                return;

            int updatedPage = Mathf.RoundToInt(Mathf.Clamp01(m_soulScroll.value) * (m_totalSections - 1));
            m_soulScroll.SetValueWithoutNotify(updatedPage / (float)(m_totalSections - 1));

            if (m_currentPageIndex != updatedPage)
            {
                m_currentPageIndex = updatedPage;
                UpdatePageLabel(m_currentPageIndex + 1);
                SetPage(m_currentPageIndex);
            }
        }
        private void UpdatePageLabel(int currentPage)
        {
            m_pageLabel.text = $"{currentPage} of {m_totalSections}";
        }

        private void SetPage(int pageIndex)
        {
            m_listUI.UpdateToggleData(pageIndex);
        }
    }
}
