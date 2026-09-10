using DChild.Gameplay.ArmyBattle.SpecialSkills;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class ArmyBattleSpecialSkillSelection : MonoBehaviour
    {
        private List<ISpecialSkillGroup> m_specialSelection;

        private ISpecialSkillGroup m_selectedGroup;

        //private int m_selectionIndex;

        //protected int selectionIndex
        //{
        //    get => m_selectionIndex;
        //    set => m_selectionIndex = (int)Mathf.Repeat(value, m_specialSelection.Count);
        //}

        public void SetSpecialSelectionList(List<ISpecialSkillGroup> selection)
        {
            m_specialSelection = selection ?? new List<ISpecialSkillGroup>();
            m_selectedGroup = m_specialSelection.FirstOrDefault();
            //selectionIndex = 0;
        }

        public ISpecialSkillGroup GetSelectedSpecialSkillGroup()
        {
            //return m_specialSelection[m_selectionIndex];
            return m_selectedGroup;
        }

        public void SelectSpecialGroup(ISpecialSkillGroup receivedGroup)
        {
            if (receivedGroup == null || m_specialSelection == null)
                return;

            var selectedGroup = m_specialSelection.Find(group => group.id == receivedGroup.id);
            if (selectedGroup != null)
            {
                m_selectedGroup = selectedGroup;
            }
        }

        //public void SetSelection(int index) => selectionIndex = index;

    }
}
