using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [CreateAssetMenu(fileName = "ArmyAttackGroupData", menuName = "DChild/Gameplay/Army/Attack Group Data")]
    public class ArmyAttackGroupData : ScriptableObject
    {
        [SerializeField]
        private string m_groupName;
        [SerializeField]
        private ArmyCharacter[] m_members;

        public int memberCount => m_members.Length;
        public ArmyCharacter GetMember(int index) => m_members[index];
    }

    [System.Serializable]
    public class ArmyAttackGroup
    {
        private ArmyCharacter[] m_members;
        private bool m_isAvailable;

        public ArmyAttackGroup(ArmyAttackGroupData data)
        {

        }

        public string groupName;
        public int totalPower;

        public int membersCount => m_members.Length;
        public bool isAvailable => m_isAvailable;

        public ArmyCharacter GetMember(int index) => m_members[index];

        public void SetAvailability(bool isAvailable)
        {
            m_isAvailable = isAvailable;
        }
    }
}