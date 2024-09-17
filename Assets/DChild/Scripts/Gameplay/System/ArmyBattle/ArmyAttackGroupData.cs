using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [CreateAssetMenu(fileName = "ArmyAttackGroupData", menuName = "DChild/Gameplay/Army/Attack Group Data")]
    public class ArmyAttackGroupData : ScriptableObject
    {
        [SerializeField]
        private string m_groupName;
        [SerializeField, DisableInInlineEditors]
        private UnitType m_unitType;
        [SerializeField, OnValueChanged("ClearMembers")]
        private bool m_useCharactersForPower;
        [SerializeField, MinValue(1), HideIf("m_useCharactersForPower")]
        private int m_power = 1;
#if UNITY_EDITOR
        [InfoBox("@\"Total Power: \" + GetTotalPower()", InfoMessageType = InfoMessageType.None), ShowIf("m_useCharactersForPower")]
#endif
        [SerializeField, InlineEditor(Expanded = true), ShowIf("m_useCharactersForPower")]
        private ArmyCharacterData[] m_members;

        public string groupName => m_groupName;
        public bool isUsingCharactersForPower => m_useCharactersForPower;
        public int memberCount => m_members.Length;
        public UnitType unitType => m_unitType;
        public ArmyCharacterData GetMember(int index)
        {
            if (m_useCharactersForPower)
            {
                return m_members[index];
            }
            else
            {
                return null;
            }
        }

        public int GetTotalPower()
        {
            if (m_useCharactersForPower)
            {
                var power = 0;
                for (int i = 0; i < m_members.Length; i++)
                {
                    power += m_members[i]?.attackPower ?? 0;
                }
                return power;
            }
            else
            {
                return m_power;
            }
        }

        private void ClearMembers()
        {
            m_members = new ArmyCharacterData[0];
        }
    }
}