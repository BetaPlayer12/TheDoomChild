using System.Collections.Generic;
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
        [SerializeField]
        private List<Image> m_partyClass;

        [SerializeField]
        private Sprite m_meleeGlow;
        [SerializeField]
        private Sprite m_rangeGlow;
        [SerializeField]
        private Sprite m_magicGlow;

        public void Display(IAttackingGroup group)
        {
            DamageType m_damageType = group.GetDamageType();

            m_partyName.Display(group);
            m_characterGroupUI.Display(group?.GetCharacterGroup() ?? null);
            m_attackPowerUI.Display(group);
            m_selectedSkill.Display(m_damageType);

            switch(m_damageType)
            {
                case DamageType.Melee:
                    SelectGlow(m_meleeGlow);
                    break;
                case DamageType.Range:
                    SelectGlow(m_rangeGlow);
                    break;
                case DamageType.Magic:
                    SelectGlow(m_magicGlow);
                    break;
            }
        }

        private void SelectGlow(Sprite glow)
        {
            foreach (Image armyUnit in m_partyClass)
            {
                armyUnit.sprite = glow;
            }
        }
    }
}