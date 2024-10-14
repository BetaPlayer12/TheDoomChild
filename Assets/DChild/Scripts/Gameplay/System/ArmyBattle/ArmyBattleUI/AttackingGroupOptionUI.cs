using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class AttackingGroupOptionUI : MonoBehaviour
    {
        [SerializeField]
        private ArmyPartyNameUI m_partyName;
        [SerializeField]
        private ArmyCharacterGroupUI m_characterGroupUI;
        [SerializeField]
        private AttackingGroupPowerUI m_attackPowerUI;
     

        public void Display(IAttackingGroup group)
        {
            m_partyName.Display(group);
            m_characterGroupUI.Display(group?.GetCharacterGroup() ?? null);
            m_attackPowerUI.Display(group);
        }
    }
}