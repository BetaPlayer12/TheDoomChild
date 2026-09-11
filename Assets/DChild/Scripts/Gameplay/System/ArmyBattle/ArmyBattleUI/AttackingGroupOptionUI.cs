using DChild.Gameplay.ArmyBattle.SpecialSkills;
using Doozy.Runtime.UIManager.Components;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using System.Runtime.Remoting;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Gameplay.ArmyBattle.UI
{

    public class AttackingGroupOptionUI : MonoBehaviour
    {
        [SerializeField]
        private ArmyPartyNameUI m_partyName;
        public ArmyPartyNameUI partyName => m_partyName;

        [SerializeField]
        private ArmyCharacterGroupUI m_characterGroupUI;
        public ArmyCharacterGroupUI characterGroupUI => m_characterGroupUI;

        [SerializeField]
        private AttackingGroupPowerUI m_attackPowerUI;
        public AttackingGroupPowerUI attackingPowerUI => m_attackPowerUI;

        [SerializeField]
        private SelectedSkillButton m_selectedSkill;
        public SelectedSkillButton selectedSkill => m_selectedSkill;


        public virtual void Display(IAttackingGroup group)
        {
            DamageType m_damageType = group.GetDamageType();
            m_characterGroupUI.Display(group?.GetCharacterGroup() ?? null);
            m_selectedSkill.Display(m_damageType);
            m_partyName.Display(group);
            m_attackPowerUI.Display(group);
        }
    }
}
