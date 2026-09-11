using Doozy.Runtime.UIManager.Animators;
using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class AttackingGroupSelectableOptionUI : AttackingGroupOptionUI
    {
        [SerializeField]
        private Image m_targetCommandIcon;
        public Image targetCommandIcon => m_targetCommandIcon;
        [SerializeField]
        private TextMeshProUGUI m_targetPartyName;
        [SerializeField]
        private TextMeshProUGUI m_targetPowerLabel;
        [SerializeField]
        private TextMeshProUGUI m_targetPowerValue;
       
        [SerializeField]
        private UIButton m_armyRow;
        public UIButton selectable => m_armyRow;
        [SerializeField]
        private Image m_highlightGlow;
        [SerializeField] private GameObject m_usedOverlay;

        public void SetUsed(bool isUsed)
        {
            m_usedOverlay.SetActive(isUsed);
            m_armyRow.interactable = !isUsed;
        }

        private IAttackingGroup m_group;
        private int m_selectionIndex;

        public IAttackingGroup group => m_group;
        public virtual int selectionIndex => m_selectionIndex;

        public void SetSelectionIndex(int index)
        {
            m_selectionIndex = index;
        }

        public override void Display(IAttackingGroup group)
        {
            gameObject.SetActive(group != null);
            if (group == null)
                return;

            m_group = group;
            m_armyRow.interactable = true;
            base.Display(group);
        }
    }
}
