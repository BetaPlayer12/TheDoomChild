using DChild.Gameplay.ArmyBattle.SpecialSkills;
using DChild.UI;
using Doozy.Runtime.UIManager.Components;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class SpecialGroupIndexHandle : MonoBehaviour, IPageHandle
    {
        [SerializeField]
        private ArmyBattleSpecialSkillSelection m_specialSkillSelection;
        [SerializeField]
        private UIButton m_previousButton;
        [SerializeField]
        private UIButton m_nextButton;
        [SerializeField]
        private UIButton m_firstSelectionOnNext;
        [SerializeField]
        private UIButton m_firstSelectionOnPrevious;
        [SerializeField, ChildGameObjectsOnly]
        private SmartSelectableNavigation[] m_selectableNavigations;
        [SerializeField]
        private List<SpecialSkillGroupOptionUI> m_selectableGroups;

        private List<ISpecialSkillGroup> m_groups = new List<ISpecialSkillGroup>();
        private List<ISpecialSkillGroup> m_filteredGroups = new List<ISpecialSkillGroup>();

        [SerializeField]
        private UIScrollbar m_scrollBar;
        //[SerializeField]
        //private List<UIButton> m_upperButtons;
        //[SerializeField]
        //private List<UIButton> m_lowerButtons;


        private int m_startingIndex = 0;
        private int m_page;
        private const int m_maxRows = 4;

        private int m_totalPages;
        private float m_scrollBarIncrements;
        private bool m_cyclePageGuard;
        private int m_availableGroupCount;

        public int currentPage => m_page;

        public event EventAction<EventActionArgs> PageChange;

        public void Select(SpecialSkillGroupOptionUI selectable)
        {
            if (selectable == null || selectable.group == null || selectable.isUsed)
                return;

            Debug.Log($"received special group: {selectable.group.GetCharacterGroup()}");
            m_specialSkillSelection.SelectSpecialGroup(selectable.group);
        }

        public void Display(List<ISpecialSkillGroup> specialGroups)
        {
            for (int i = 0; i < m_maxRows; i++)
            {
                var selectableGroup = m_selectableGroups[i];

                if (i < specialGroups.Count)
                {
                    int absoluteIndex = m_startingIndex + i;
                    bool isUsed = absoluteIndex >= m_availableGroupCount;

                    selectableGroup.Display(specialGroups[i]);
                    selectableGroup.SetUsed(isUsed);
                    continue;
                }
                selectableGroup.Display(null);
            }
        }

        public void NextPage()
        {
            m_cyclePageGuard = true;

            m_page++;
            SetPage(m_page);

            if (m_page != GetTotalPages() - 1)
            {
                m_scrollBar.value = m_scrollBarIncrements * m_page;

            }
            else
            {
                m_scrollBar.value = 1;
            }
            m_cyclePageGuard = false;

            StartCoroutine(ForceSelectFirstSelectionRoutine(m_firstSelectionOnNext));
        }

        public void PreviousPage()
        {
            m_cyclePageGuard = true;

            m_page--;
            SetPage(m_page);

            m_scrollBar.value = m_scrollBarIncrements * m_page;

            m_cyclePageGuard = false;

            StartCoroutine(ForceSelectFirstSelectionRoutine(m_firstSelectionOnPrevious));
        }

        public void Initialize()
        {
            Debug.Log($"total special groups: {m_groups.Count}");
            m_totalPages = Mathf.Max(1, GetTotalPages());
            m_scrollBar.numberOfSteps = m_totalPages;
            m_scrollBarIncrements = m_totalPages > 1 ? 1f / (m_totalPages - 1) : 0f;
            m_scrollBar.size = 1f / m_totalPages;
            m_scrollBar.value = 0;

            SetPage(0);

            //SetPage(0);
        }


        public void SetGroups(List<ISpecialSkillGroup> groups, int availableGroupCount)
        {
            m_groups = groups ?? new List<ISpecialSkillGroup>();
            m_availableGroupCount = Mathf.Clamp(availableGroupCount, 0, m_groups.Count);
            Initialize();
        }

        public int GetTotalPages()
        {
            return Mathf.CeilToInt(m_groups.Count / (float)m_maxRows);

        }

        public void SetPage(int pageIndex)
        {
            if (pageIndex < 0 || pageIndex >= m_totalPages)
                return;

            m_previousButton.interactable = pageIndex > 0;
            m_nextButton.interactable = pageIndex < (m_totalPages - 1);

            m_page = pageIndex;
            m_startingIndex = m_page * m_maxRows;

            int rangeCount = (m_startingIndex + m_maxRows) < m_groups.Count ? m_maxRows : (m_groups.Count - m_startingIndex);

            m_filteredGroups = m_groups.GetRange(m_startingIndex, rangeCount);

            Display(m_filteredGroups);

            for (int i = 0; i < m_selectableNavigations.Length; i++)
            {
                m_selectableNavigations[i].UpdateSelectionAvailability();
            }


        }

        private IEnumerator ForceSelectFirstSelectionRoutine(UIButton firstSelection)
        {
            yield return null;
            firstSelection.Select();
        }


        public void HandleScroll()
        {
            if (m_cyclePageGuard || m_totalPages <= 1)
                return;

            int updatedPage = Mathf.FloorToInt(m_scrollBar.value / m_scrollBarIncrements);
            updatedPage = Mathf.Clamp(updatedPage, 0, m_totalPages - 1);

            if (m_page != updatedPage)
            {
                SetPage(updatedPage);
            }
        }
    }
}
