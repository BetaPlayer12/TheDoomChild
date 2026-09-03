using DChild.Gameplay.Characters.Players.SoulSkills;
using DChild.Gameplay.SoulSkills.UI;
using Doozy.Runtime.UIManager.Components;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DChild.Menu.Codex
{
    public class CodexScrollNavigationHandle : MonoBehaviour
    {
        [SerializeField] private UIScrollbar m_scrollBar;
        [SerializeField] private TextMeshProUGUI m_pageLabel;

        private ScrollbarMouseWheel m_mouseWheel;
        private int m_currentPageIndex;

        private int m_totalSections;

        public Action<int> OnCurrentPageChange;

        [Button]
        public void SetupScroll(int entryListCount, int toggleCount)
        {
            m_currentPageIndex = 0;
            m_totalSections = toggleCount > 0 ? Mathf.CeilToInt(Mathf.Max(0, entryListCount) / (float)toggleCount) : 0;

            m_scrollBar.numberOfSteps = m_totalSections;
            // Doozy limits its step count; snap pages ourselves when the list exceeds that limit.
            if (m_scrollBar.numberOfSteps != m_totalSections)
                m_scrollBar.numberOfSteps = 0;
            m_scrollBar.size = m_totalSections > 0 ? 1f / m_totalSections : 1f;
            // Gallery initialization already populates page zero. Avoid a second selection callback.
            m_scrollBar.SetValueWithoutNotify(0f);

            if (m_mouseWheel == null)
                m_mouseWheel = m_scrollBar.GetComponentInParent<ScrollbarMouseWheel>(true);
            if (m_mouseWheel != null)
            {
                // Some prefab variants replace the template's scrollbar with their own.
                m_mouseWheel.SetScrollbar(m_scrollBar);
                m_mouseWheel.SetStepCount(Mathf.Max(1, m_totalSections));
            }
        }

        public void HandleScroll()
        {
            if (m_totalSections <= 1)
                return;

            int updatedPage = Mathf.RoundToInt(Mathf.Clamp01(m_scrollBar.value) * (m_totalSections - 1));
            m_scrollBar.SetValueWithoutNotify(updatedPage / (float)(m_totalSections - 1));

            if (m_currentPageIndex != updatedPage)
            {
                m_currentPageIndex = updatedPage;
                SetPage(m_currentPageIndex);
            }
        }

        private void SetPage(int pageIndex)
        {
            OnCurrentPageChange?.Invoke(pageIndex);
        }
    }


}
