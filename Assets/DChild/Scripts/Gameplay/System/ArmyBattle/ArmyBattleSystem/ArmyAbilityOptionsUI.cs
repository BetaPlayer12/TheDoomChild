using Doozy.Runtime.UIManager.Components;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class ArmyAbilityOptionsUI : MonoBehaviour
    {
        [SerializeField]
        private UIButton m_specialButton;

        private Army army;

        public void UpdateOptions()
        {
            //m_specialButton.interactable = army.HasAvailableAbilityGroup();
        }

        public void Initialize(PlayerArmyController controller)
        {
            army = controller.controlledArmy;
            //m_specialButton.onClickEvent.AddListener(controller.ChooseSpecial);
        }
    }
}