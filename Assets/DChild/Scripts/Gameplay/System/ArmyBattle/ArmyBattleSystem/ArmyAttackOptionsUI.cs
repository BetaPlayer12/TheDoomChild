using Doozy.Runtime.UIManager.Components;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class ArmyAttackOptionsUI : MonoBehaviour
    {
        [SerializeField]
        private UIButton m_rockButton;
        [SerializeField]
        private UIButton m_paperButton;
        [SerializeField]
        private UIButton m_scissorButton;

        private Army army;

        public void UpdateOptions()
        {
            //m_rockButton.interactable = army.HasAvailableAttackGroup(UnitType.Rock);
            //m_paperButton.interactable = army.HasAvailableAttackGroup(UnitType.Paper);
            //m_scissorButton.interactable = army.HasAvailableAttackGroup(UnitType.Scissors);
        }

        public void Initialize(PlayerArmyController controller)
        {
            army = controller.controlledArmy;
            //m_rockButton.onClickEvent.AddListener(controller.ChooseRockAttack);
            //m_paperButton.onClickEvent.AddListener(controller.ChoosePaperAttacker);
            //m_scissorButton.onClickEvent.AddListener(controller.ChooseScissorAttack);
        }
    }
}