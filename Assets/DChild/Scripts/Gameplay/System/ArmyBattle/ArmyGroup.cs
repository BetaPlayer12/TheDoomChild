using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyGroup : IAttackingGroup, ISpecialSkillGroup
    {
        [SerializeField]
        private ArmyCharacterGroup m_members;
        [SerializeField]
        private DamageType m_type;

        public ArmyGroup (ArmyCharacterGroup members, DamageType type)
        {
            m_members = members;
            m_type = type;
        }

        public int GetAttackPower()
        {
            throw new System.NotImplementedException();
        }

        public ArmyCharacterGroup GetCharacterGroup()
        {
            throw new System.NotImplementedException();
        }

        public ArmyCharacterGroup GetCharacterGroup(ArmyCharacterGroup characterGroup)
        {
            throw new System.NotImplementedException();
        }

        public DamageType GetDamageType()
        {
            throw new System.NotImplementedException();
        }

        public SpecialSkill GetSpecialSkill(SpecialSkill specialSkill)
        {
            throw new System.NotImplementedException();
        }

        public int GetTroopCount()
        {
            throw new System.NotImplementedException();
        }

        public bool HasSpecialSkill(bool hasSkill)
        {
            throw new System.NotImplementedException();
        }
    }
}

