using DChild.Gameplay.ArmyBattle;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace DChild.Gameplay.ArmyBattle
{

    [System.Serializable]
    public class ArmyAbilityGroup : ArmyCharacterGroup
    {
        private ISpecialSkillGroup m_reference;

        public ArmyAbilityGroup(ISpecialSkillGroup data) : base()
        {
            m_reference = data;
        }
        public ArmyAbilityGroup(ArmyAbilityGroup data) : base(data)
        {
            m_reference = data.reference;
        }

        public ISpecialSkillGroup reference => m_reference;

        [ShowInInspector, PropertyOrder(0)]
        public string description => m_reference.abilityDescription;

        public override string name => m_reference.groupName;

        public void UseAbility(Army owner, Army enemy)
        {
            m_reference.UseAbility(owner, enemy);
        }
    }
}