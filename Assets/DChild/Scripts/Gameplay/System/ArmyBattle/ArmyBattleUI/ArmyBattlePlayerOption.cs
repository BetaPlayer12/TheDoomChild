using Doozy.Runtime.UIManager.Components;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class ArmyBattlePlayerOption : MonoBehaviour
    {
        [SerializeField]
        private PlayerArmyController m_player;
        public PlayerArmyController player => m_player;

        [SerializeField]
        private ArmyDamageOptionSelection m_damageSelection;
        [SerializeField]
        private UIButton m_specialSkillButton;
        [SerializeField]
        private ArmyBattleSpecialSkillSelection m_specialSelection;

        [SerializeField]
        private ArmyGroupIndexHandle m_groupIndex;
        [SerializeField]
        private SpecialGroupIndexHandle m_specialIndex;

        private IAttackingGroup m_selectedAttackingGroup;

        public void Initialize(PlayerArmyController player)
        {
            m_player = player;
            m_damageSelection.Initialize(player.controlledArmy);

            m_groupIndex.GroupSelected -= SetSelectedAttackingGroup;
            m_groupIndex.GroupSelected += SetSelectedAttackingGroup;
        }

        public void UpdateOptions()
        {
            var specialGroupCount = m_player.controlledArmy.GetAvailableSkills().Count;
            m_damageSelection.UpdateSelectionAvailability();
            m_specialSkillButton.interactable = ArmyBattleSystem.CanPlayerActivateSpecialSkill() && specialGroupCount > 0;
        }

        public void SetAttackGroupSelection(ArmyDamageTypeOptionUI option)
        {
            var damageType = option.damageType;
            var availableGroups =
      m_player.controlledArmy.GetAvailableGroups(damageType);

            var usedGroups =
                m_player.controlledArmy.GetUsedGroups(damageType);

            var selectionGroups = availableGroups
                .Concat(usedGroups)
                .ToList();

            m_selectedAttackingGroup = null;

            m_groupIndex.SetGroups(
                damageType,
                selectionGroups,
                availableGroups.Count);
        }

        public void SetSpecialSkillSelection()
        {
            var availableSpecialGroups = m_player.controlledArmy.GetAvailableSkills();
            var usedSpecialGroups = m_player.controlledArmy.GetUsedSkills();

            var selectionGroups = availableSpecialGroups
                .Concat(usedSpecialGroups)
                .ToList();

            m_specialSelection.SetSpecialSelectionList(availableSpecialGroups);
            m_specialIndex.SetGroups(selectionGroups, availableSpecialGroups.Count);
        }

        public void SelectCurrentAttackingGroup()
        {
            TrySelectCurrentAttackingGroup();
        }

        public bool TrySelectCurrentAttackingGroup()
        {
            if (m_selectedAttackingGroup == null)
            {
                Debug.LogWarning("Cannot select an attacking group because no group was chosen in More Groups.");
                return false;
            }

            m_player.UseThisTurn(m_selectedAttackingGroup);
            Debug.Log($"Selecting To Attack With {m_selectedAttackingGroup.GetCharacterGroup().name}");
            return true;
        }

        private void SetSelectedAttackingGroup(IAttackingGroup group)
        {
            m_selectedAttackingGroup = group;
            SelectCurrentAttackingGroup();
        }

        private void OnDestroy()
        {
            if (m_groupIndex != null)
            {
                m_groupIndex.GroupSelected -= SetSelectedAttackingGroup;
            }
        }
    }
}
