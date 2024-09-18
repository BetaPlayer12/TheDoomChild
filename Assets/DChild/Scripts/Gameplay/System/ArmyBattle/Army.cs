using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DChild.Gameplay.ArmyBattle
{
    public class Army : MonoBehaviour
    {
        //#endif
        [SerializeField]
        private int m_troopCount;

        [SerializeField]
        private ArmyInfo m_info;
        [SerializeField]
        private List<IAttackingGroup> m_availableAttackingGroups;
        [SerializeField]
        private List<ISpecialSkillGroup> m_availableSpecialSkills;

        public ArmyModifier modifiers;
        public int troopCount => m_troopCount;

        public Army(ArmyInfo info)
        {
            m_info = info;
        }

        public int AddTroopCount(int additionalTroops)
        {
            return m_troopCount + additionalTroops;
        }

        public int SubtractTroopCount(int subtractedTroops)
        {
            return m_troopCount - subtractedTroops;
        }

        public void ResetTroopCount()
        {
            throw new NotImplementedException();
        }

        public List<IAttackingGroup> GetAvailableGroups(DamageType damageType)
        {
            throw new NotImplementedException();
        }

        public void SetAttackingGroupAvailability(IAttackingGroup attackingGroup, bool isAvailable)
        {
            throw new NotImplementedException();
        }

        public void ResetGroupAvailability()
        {
            throw new NotImplementedException();
        }

        public List<SpecialSkill> GetAvailableSkills()
        {
            throw new NotImplementedException();
        }

        public void SetSpecialSkillAvailability(SpecialSkill group, bool isAvailable)
        {
            throw new NotImplementedException();
        }
    }
}