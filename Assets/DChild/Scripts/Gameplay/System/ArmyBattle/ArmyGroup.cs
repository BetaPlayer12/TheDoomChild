using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public class ArmyGroup : IAttackingGroup, ISpecialSkillGroup
    {
        [SerializeField, InfoBox("@\"Has Total Power of: \" + GetTotalCharacterGroupAttackPower()", InfoMessageType = InfoMessageType.None), HideLabel]
        private ArmyCharacterGroup m_members;
        [SerializeField]
        private DamageType m_type;
        [SerializeField]
        private bool m_useCustomAttackPower;
        [SerializeField, ShowIf("m_useCustomAttackPower"), Indent]
        private int m_attackPower;

        public ArmyGroup(ArmyCharacterGroup members, DamageType type)
        {
            m_members = members;
            m_type = type;

            m_useCustomAttackPower = false;
            m_attackPower = GetTotalCharacterGroupAttackPower();
        }

        public ArmyCharacterGroup GetCharacterGroup()
        {
            return m_members;
        }

        public DamageType GetDamageType()
        {
            return m_type;
        }

        public int GetTroopCount()
        {
            var totalTroopCount = 0;
            for (int i = 0; i < m_members.memberCount; i++)
            {
                totalTroopCount += m_members.GetCharacter(i).troopCount;
            }
            return totalTroopCount;
        }

        public int GetAttackPower()
        {
            return m_attackPower;
        }
        public SpecialSkill GetSpecialSkill()
        {
            throw new System.NotImplementedException();
        }
        public bool HasSpecialSkill()
        {
            return false;
        }

        private int GetTotalCharacterGroupAttackPower()
        {
            var totalPower = 0;
            for (int i = 0; i < m_members.memberCount; i++)
            {
                totalPower += m_members.GetCharacter(i).attackPower;
            }
            return totalPower;
        }
    }
}

