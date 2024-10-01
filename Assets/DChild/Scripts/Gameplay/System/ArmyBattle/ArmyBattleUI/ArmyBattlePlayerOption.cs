using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class ArmyBattlePlayerOption : MonoBehaviour
    {
        [SerializeField]
        private PlayerArmyController m_player;
        [SerializeField]
        private ArmyBattleAttackGroupSelection m_group;

        public void SetAttackGroupSelection(ArmyDamageTypeOptionUI option)
        {
            var damageType = option.damageType;
            m_group.SetSelectionList(m_player.controlledArmy.GetAvailableGroups(damageType));
            m_group.SetSelectionIcon(damageType);
        }

        public void SelectCurrentAttackingGroup()
        {
            m_player.UseThisTurn(m_group.GetSelectedAttackGroup());
        }
    }
}