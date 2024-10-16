using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class ArmyBattleAttackGroupSelection : MonoBehaviour
    {
        [SerializeField]
        private ArmyDamageTypeOptionUI m_damageTypeIcon;
        [SerializeField]
        private AttackingGroupOptionUI m_ui;

        private List<IAttackingGroup> m_selection;

        private int m_selectionIndex;

        private int selectionIndex
        {
            get => m_selectionIndex;
            set
            {
                m_selectionIndex = (int)Mathf.Repeat(value, m_selection.Count);
                UpdateUI();
            }
        }

        public IAttackingGroup GetSelectedAttackGroup() => m_selection[m_selectionIndex];

        public void Prev()
        {
            selectionIndex -= 1;
        }

        public void Next()
        {
            selectionIndex += 1;
        }

        public void SetSelection(int index) => selectionIndex = index;

        public void SetSelectionIcon(DamageType type) => m_damageTypeIcon.SetType(type);

        public void SetSelectionList(List<IAttackingGroup> selection)
        {
            m_selection = selection.OrderBy(x => x.GetAttackPower()).ToList();
            selectionIndex = 0;

        }

        private void UpdateUI()
        {
            m_ui.Display(m_selection[m_selectionIndex]);
        }
    }
}