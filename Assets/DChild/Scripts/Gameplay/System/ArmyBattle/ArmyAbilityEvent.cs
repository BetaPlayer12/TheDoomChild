using Holysoft.Event;

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyAbilityEvent : IEventActionArgs
    {
        private ArmyAbilityGroup m_ability;

        public ArmyAbilityGroup ability => m_ability;

        public void Set(ArmyAbilityGroup abilityGroup)
        {
            m_ability = abilityGroup;
        }
    }
}