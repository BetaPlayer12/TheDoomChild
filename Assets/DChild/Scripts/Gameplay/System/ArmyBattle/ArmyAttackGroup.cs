using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public class ArmyAttackGroup
    {
        private ArmyAttackGroupData m_reference;

        private List<ArmyCharacter> m_availableMembers;
        [ShowInInspector]
        private bool m_isAvailable;

        public ArmyAttackGroup(ArmyAttackGroupData data)
        {
            m_reference = data;
            m_availableMembers = new List<ArmyCharacter>();
            for (int i = 0; i < data.memberCount; i++)
            {
                m_availableMembers.Add(data.GetMember(i));
            }
            m_isAvailable = true;
        }

        public ArmyAttackGroup(ArmyAttackGroup reference)
        {
            m_reference = reference.reference;
            m_availableMembers = new List<ArmyCharacter>();
            for (int i = 0; i < reference.availableMemberCount; i++)
            {
                m_availableMembers.Add(reference.GetAvailableMember(i));
            }
            m_isAvailable = true;
        }

        public ArmyAttackGroupData reference => m_reference;

        [ShowInInspector, PropertyOrder(0)]
        public string groupName => m_reference.groupName;
        public UnitType unitType => m_reference.unitType;

        public int availableMemberCount => m_availableMembers.Count;
        public bool isAvailable => m_isAvailable;

        [ShowInInspector, PropertyOrder(0)]
        private int totalPower => GetTotalPower();

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

        public ArmyCharacter GetAvailableMember(int index) => m_availableMembers[index];

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
                power = m_reference.GetTotalPower();
            }
            return power;
        }

        public void SetAvailability(bool isAvailable)
        {
            m_isAvailable = isAvailable;
        }
    }
}