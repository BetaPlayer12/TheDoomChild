using DChild.Gameplay.ArmyBattle;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace DChild.Gameplay.ArmyBattle
{

    [System.Serializable]
    public class ArmyAbility
    {
        private ArmyAbilityData m_reference;
        private List<ArmyCharacterData> m_availableMembers;
        [ShowInInspector]
        private int m_useCountLeft;

        public ArmyAbilityData reference => m_reference;

        [ShowInInspector, PropertyOrder(0)]
        public string abilityName => m_reference.abilityName;

        [ShowInInspector, PropertyOrder(0)]
        public string description => m_reference.description;
        public int availableMemberCount => m_availableMembers.Count;
        public bool isAvailable => m_useCountLeft > 0;

        public ArmyAbility(ArmyAbilityData data)
        {
            m_reference = data;
            m_availableMembers = new List<ArmyCharacterData>();
            for (int i = 0; i < data.membersCount; i++)
            {
                m_availableMembers.Add(data.GetMember(i));
            }
            ResetUseCount();
        }

        public void SetMemberAvailability(params bool[] memberAvailability)
        {
            m_availableMembers.Clear();
            for (int i = 0; i < m_reference.membersCount; i++)
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

        public ArmyCharacterData GetAvailableMember(int index) => m_availableMembers[index];

        public void UseAbility(Army owner, Army enemy)
        {
            m_reference.ApplyEffect(owner, enemy);
        }

        public void ReduceUseCount() => m_useCountLeft--;

        public void ResetUseCount()
        {
            if (m_reference.useCharactersForUseCount)
            {
                m_useCountLeft = m_availableMembers.Count;
            }
            else
            {
                m_useCountLeft = 1;
            }
        }
    }
}