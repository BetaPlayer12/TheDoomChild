using DChild.Gameplay.ArmyBattle.SpecialSkills;
using DChild.UI;
using Doozy.Runtime.UIManager.Components;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{

    public class ArmyGroupIndexHandle : MonoBehaviour, IPageHandle
    {
        [SerializeField]
        private MoreGroupsClassLabel m_panelLabel;
        [SerializeField]
        private UIButton m_previousButton;
        [SerializeField]
        private UIButton m_nextButton;
        [SerializeField]
        private UIButton m_firstSelectionOnPrevious;
        [SerializeField]
        private UIButton m_firstSelectionOnNext;
        [SerializeField, ChildGameObjectsOnly]
        private SmartSelectableNavigation[] m_selectableNavigations;
        [SerializeField]
        private List<AttackingGroupSelectableOptionUI> m_selectableGroups;

        private List<IAttackingGroup> m_groups = new List<IAttackingGroup>();
        private List<IAttackingGroup> m_filteredGroups = new List<IAttackingGroup>();

        [SerializeField]
        private UIScrollbar m_scrollBar;

        private int m_startingIndex = 0;
        private int m_page;
        private const int m_maxRows = 8;

        private int m_totalPages;
        private float m_scrollBarIncrements;
        private bool m_cyclePageGuard;

        public int currentPage => m_page;
        public event EventAction<EventActionArgs> PageChange;
        public event Action<IAttackingGroup> GroupSelected;

        private int m_availableGroupCount;

        public void SetGroups(
            DamageType damageType,
            List<IAttackingGroup> groups,
            int availableGroupCount)
        {
            m_panelLabel?.SetPanelLabel(damageType);
            m_groups = groups ?? new List<IAttackingGroup>();
            m_availableGroupCount = availableGroupCount;

            Initialize();
        }

        public void Select(AttackingGroupSelectableOptionUI selectable)
        {
            if (selectable == null || selectable.group == null)
                return;

            GroupSelected?.Invoke(selectable.group);
        }

        public void SetAvailableGroups(DamageType damageType, List<IAttackingGroup> groups)
        {
            m_panelLabel?.SetPanelLabel(damageType);
            m_groups = groups ?? new List<IAttackingGroup>();
            Initialize();
        }

        public void Initialize()
        {
            m_totalPages = Mathf.Max(1, GetTotalPages());
            m_scrollBar.numberOfSteps = m_totalPages;
            m_scrollBarIncrements = m_totalPages > 1 ? 1f / (m_totalPages - 1) : 0f;
            m_scrollBar.size = 1f / m_totalPages;
            m_scrollBar.value = 0;

            SetPage(0);
        }   

        private void EnsureReferenceAndSelect(AttackingGroupSelectableOptionUI entryButton)
        {
            var button = entryButton.GetComponent<UIButton>();
            button.Select();
        }

        public void Display(List<IAttackingGroup> attackingGroups)
        {
            for (int i = 0; i < m_maxRows; i++)
            {
                var selectableGroup = m_selectableGroups[i];

                if (i < attackingGroups.Count)
                {
                    int absoluteIndex = m_startingIndex + i;
                    bool isUsed = absoluteIndex >= m_availableGroupCount;

                    selectableGroup.SetSelectionIndex(absoluteIndex);
                    selectableGroup.Display(attackingGroups[i]);
                    selectableGroup.SetUsed(isUsed);
                    continue;
                }

                selectableGroup.Display(null);
            }
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
            EnsureReferenceAndSelect(m_selectableGroups[0]);
        }

        private void Awake()
        {
            if (m_panelLabel == null)
            {
                m_panelLabel = GetComponentInChildren<MoreGroupsClassLabel>(true);
            }
        }
    }
}
