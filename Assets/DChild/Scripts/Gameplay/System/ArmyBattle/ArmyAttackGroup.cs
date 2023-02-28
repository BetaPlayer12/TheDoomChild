namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public class ArmyAttackGroup
    {
        private ArmyAttackGroupData m_reference;

        private ArmyCharacter[] m_members;
        private bool m_isAvailable;

        public ArmyAttackGroup(ArmyAttackGroupData data)
        {
            m_reference = data;
            m_members = new ArmyCharacter[data.memberCount];
            for (int i = 0; i < data.memberCount; i++)
            {
                m_members[i] = data.GetMember(i);
            }
            m_isAvailable = true;
        }

        public ArmyAttackGroup(ArmyAttackGroup reference)
        {
            m_reference = reference.reference;
            m_members = new ArmyCharacter[reference.membersCount];
            for (int i = 0; i < reference.membersCount; i++)
            {
                m_members[i] = reference.GetMember(i);
            }
            m_isAvailable = true;
        }

        public ArmyAttackGroupData reference => m_reference;
        public string groupName => m_reference.groupName;
        public UnitType unitType => m_reference.unitType;

        public int membersCount => m_members.Length;
        public bool isAvailable => m_isAvailable;

        public ArmyCharacter GetMember(int index) => m_members[index];

        public int GetTotalPower()
        {
            var power = 0;
            for (int i = 0; i < m_members.Length; i++)
            {
                power += m_members[i].power;
            }
            return power;
        }

        public void SetAvailability(bool isAvailable)
        {
            m_isAvailable = isAvailable;
        }
    }
}