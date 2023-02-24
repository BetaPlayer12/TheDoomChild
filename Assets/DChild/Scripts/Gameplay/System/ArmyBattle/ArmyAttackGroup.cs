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
        }

        public string groupName => m_reference.groupName;
        public UnitType unitType => m_reference.unitType;

        public int membersCount => m_members.Length;
        public bool isAvailable => m_isAvailable;

        public ArmyCharacter GetMember(int index) => m_members[index];

        public int GetTotalPower()
        {
            return 1;
        }

        public void SetAvailability(bool isAvailable)
        {
            m_isAvailable = isAvailable;
        }
    }
}