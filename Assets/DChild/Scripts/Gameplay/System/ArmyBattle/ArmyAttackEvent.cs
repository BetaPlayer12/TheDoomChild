using Holysoft.Event;

namespace DChild.Gameplay.ArmyBattle
{
    public struct ArmyAttackEvent : IEventActionArgs
    {
        private ArmyAttack m_attack;

        public ArmyAttackEvent(ArmyAttack attack)
        {
            m_attack = attack;
        }

        public ArmyAttack attack => m_attack;
    }
}