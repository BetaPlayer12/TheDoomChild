using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public class ArmyGroup : IAttackingGroup, ISpecialSkillGroup
    {
        [SerializeField]
        private int m_id;
        [SerializeField, InfoBox("@\"Has Total Power of: \" + GetTotalCharacterGroupAttackPower()", InfoMessageType = InfoMessageType.None), HideLabel]
        private ArmyCharacterGroup m_members;
        [SerializeField]
        private DamageType m_type;
        [SerializeField]
        private bool m_useCustomAttackPower;
        [SerializeField, ShowIf("m_useCustomAttackPower"), Indent]
        private int m_attackPower;

        [SerializeField]
        private bool m_hasSpecialSkill;
        [SerializeField, ShowIf("m_hasSpecialSkill"), Indent, HideLabel, BoxGroup("Special Skill Info")]
        private SpecialSkill m_specialSkill;

        public int id => m_id;

        public ArmyGroup(int id, ArmyCharacterGroup members, DamageType type, SpecialSkill specialSkill)
        {
            m_id = id;
            m_members = members;
            m_type = type;

            m_useCustomAttackPower = false;
            m_attackPower = GetTotalCharacterGroupAttackPower();

            m_specialSkill = specialSkill;
            m_hasSpecialSkill = m_specialSkill != null;
        }

        public ArmyCharacterGroup GetCharacterGroup() => m_members;

        public DamageType GetDamageType() => m_type;

        public int GetTroopCount()
        {
            var totalTroopCount = 0;
            for (int i = 0; i < m_members.memberCount; i++)
            {
                totalTroopCount += m_members.GetCharacter(i).troopCount;
            }
            return totalTroopCount;
        }

        public int GetAttackPower() => m_attackPower;
        public SpecialSkill GetSpecialSkill() => m_hasSpecialSkill ? m_specialSkill : null;
        public bool HasSpecialSkill() => m_hasSpecialSkill;

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

