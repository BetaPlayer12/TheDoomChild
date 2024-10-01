using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.UI
{
    public class AttackingGroupOptionUI : MonoBehaviour
    {
        [SerializeField]
        private ArmyCharacterGroupUI m_characterGroupUI;
        [SerializeField]
        private AttackingGroupPowerUI m_attackPowerUI;

        public void Display(IAttackingGroup group)
        {
            m_characterGroupUI.Display(group?.GetCharacterGroup() ?? null);
            m_attackPowerUI.Display(group);
        }
    }
}