using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{

    [System.Serializable]
    public class ArmyAttackGroup : ArmyGroup
    {
        private IArmyAttackInfo m_reference;

        public ArmyAttackGroup(IArmyAttackInfo data) : base()
        {
            m_reference = data;
        }

        public ArmyAttackGroup(ArmyAttackGroup reference) : base(reference)
        {
            m_reference = reference.reference;
        }

        public IArmyAttackInfo reference => m_reference;
        public UnitType unitType => m_reference.attackType;

        [ShowInInspector, PropertyOrder(0)]
        private int totalPower => GetTotalPower();

        public override string groupName => m_reference.groupName;

        public void SetMemberAvailability(params bool[] memberAvailability)
        {
            m_availableMembers.Clear();
            for (int i = 0; i < m_reference.memberCount; i++)
            {
                if (i >= memberAvailability.Length)
                {
                    break;
                }
                else if (memberAvailability[i])
                {
                    m_availableMembers.Add(reference.GetMember(i));
                }
            }
        }

        public int GetTotalPower()
        {
            var power = 0;
            if (m_reference.isUsingCharactersForPower)
            {
                for (int i = 0; i < m_availableMembers.Count; i++)
                {
                    power += m_availableMembers[i].power;
                }
            }
            else
            {
                power = m_reference.GetTotalAttackPower();
            }
            return power;
        }
    }
}