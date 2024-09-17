using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public class ArmyCharacterGroup : IAttackingGroup, ISpecialSkillGroup
    {
        [SerializeField]
        private string m_name;
        [SerializeField]
        protected List<ArmyCharacterData> m_armyGroupList;
        [ShowInInspector]
        private bool m_isAvailable;

        public string name => m_name;

        public ArmyCharacterGroup()
        {
            m_armyGroupList = new List<ArmyCharacterData>();
            m_isAvailable = true;
        }
        public ArmyCharacterGroup(ArmyCharacterGroup reference)
        {
            m_armyGroupList = new List<ArmyCharacterData>();
            //for (int i = 0; i < reference.GetTroopCount; i++)
            //{
            //    m_armyGroupList.Add(reference.GetAvailableMember(i));
            //}
            m_isAvailable = true;
        }

        public ArmyCharacterData GetCharacter(int index)
        {
            return m_armyGroupList[index];
        }

        public int GetTroopCount()
        {
            throw new System.NotImplementedException();
        }

        public int GetAttackPower()
        {
            throw new System.NotImplementedException();
        }

        public ArmyCharacterGroup GetCharacterGroup()
        {
            throw new System.NotImplementedException();
        }

        public DamageType GetDamageType()
        {
            throw new System.NotImplementedException();
        }

        public bool HasSpecialSkill()
        {
            throw new System.NotImplementedException();
        }

        public ArmyCharacterGroup GetCharacterGroup(ArmyCharacterGroup characterGroup)
        {
            throw new System.NotImplementedException();
        }

        public SpecialSkill GetSpecialSkill()
        {
            throw new System.NotImplementedException();
        }

        public bool HasSpecialSkill(bool hasSkill)
        {
            throw new System.NotImplementedException();
        }

        public SpecialSkill GetSpecialSkill(SpecialSkill specialSkill)
        {
            throw new System.NotImplementedException();
        }
    }
}