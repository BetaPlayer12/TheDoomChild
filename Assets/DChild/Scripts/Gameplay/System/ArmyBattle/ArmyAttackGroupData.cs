using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [CreateAssetMenu(fileName = "ArmyAttackGroupData", menuName = "DChild/Gameplay/Army/Attack Group Data")]
    public class ArmyAttackGroupData : ScriptableObject
    {
        [SerializeField]
        private string m_groupName;
        [SerializeField]
        private UnitType m_unitType;
#if UNITY_EDITOR
        [InfoBox("@\"Total Power: \" + GetTotalPower()", InfoMessageType = InfoMessageType.None)]
#endif
        [SerializeField, InlineEditor(Expanded = true)]
        private ArmyCharacter[] m_members;

        public string groupName => m_groupName;
        public int memberCount => m_members.Length;
        public UnitType unitType => m_unitType;
        public ArmyCharacter GetMember(int index) => m_members[index];

        private int GetTotalPower()
        {
            var power = 0;
            for (int i = 0; i < m_members.Length; i++)
            {
                power += m_members[i].power;
            }
            return power;
        }
    }
}