using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class ArmyBattlePlayerOption : MonoBehaviour
    {
        [SerializeField]
        private PlayerArmyController m_player;
        [SerializeField]
        private ArmyDamageOptionSelection m_damageSelection;
        [SerializeField]
        private ArmyBattleAttackGroupSelection m_groupSelection;

        public void Initialize(PlayerArmyController player)
        {
            m_player = player;
            m_damageSelection.Initialize(player.controlledArmy);
        }

        public void UpdateOptions()
        {
            m_damageSelection.UpdateSelectionAvailability();
        }

        public void SetAttackGroupSelection(ArmyDamageTypeOptionUI option)
        {
            var damageType = option.damageType;
            m_groupSelection.SetSelectionList(m_player.controlledArmy.GetAvailableGroups(damageType));
            m_groupSelection.SetSelectionIcon(damageType);
            SelectCurrentAttackingGroup();
        }

        public void SelectCurrentAttackingGroup()
        {
            m_player.UseThisTurn(m_groupSelection.GetSelectedAttackGroup());
            Debug.Log($"Selecting To Attack With {m_groupSelection.GetSelectedAttackGroup().GetCharacterGroup().name}");
        }
    }
}