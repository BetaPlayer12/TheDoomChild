using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace DChild.Gameplay.ArmyBattle
{
    public abstract class ArmyGroup
    {
        protected List<ArmyCharacterData> m_availableMembers;
        [ShowInInspector]
        private bool m_isAvailable;

        public ArmyGroup()
        {
            m_availableMembers = new List<ArmyCharacterData>();
            m_isAvailable = true;
        }
        public ArmyGroup(ArmyGroup reference)
        {
            m_availableMembers = new List<ArmyCharacterData>();
            for (int i = 0; i < reference.availableMemberCount; i++)
            {
                m_availableMembers.Add(reference.GetAvailableMember(i));
            }
            m_isAvailable = true;
        }


        [ShowInInspector, PropertyOrder(0)]
        public abstract string groupName { get; }
        public int availableMemberCount => m_availableMembers.Count;
        public bool isAvailable => m_isAvailable;

        public ArmyCharacterData GetAvailableMember(int index) => m_availableMembers[index];

        public void SetAvailability(bool isAvailable)
        {
            m_isAvailable = isAvailable;
        }
    }
}