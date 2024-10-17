using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField]
        private SelectedSkillButton m_selectedSkill;

        public void Display(IAttackingGroup group)
        {
            m_partyName.Display(group);
            m_characterGroupUI.Display(group?.GetCharacterGroup() ?? null);
            m_attackPowerUI.Display(group);
            m_selectedSkill.Display(group.GetDamageType());
        }
    }
}