using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DChild.Gameplay.ArmyBattle.SpecialSkills;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public class Army
    {
        //#endif
        [SerializeField]
        private int m_troopCount;

        [SerializeField, FoldoutGroup("Info"), HideLabel]
        private ArmyInfo m_info;

        private List<IAttackingGroup> m_availableAttackingGroups;
        private List<IAttackingGroup> m_usedAttackingGroups;

        private List<ISpecialSkillGroup> m_availableSpecialSkills;
        private List<ISpecialSkillGroup> m_usedSpecialSkills;

        public ArmyModifier modifiers;
        public int troopCount => m_troopCount;
        public float troopCountPercent => (m_troopCount / m_info.GetTroopCount()) * 100f;

        public ArmyOverviewData overview => m_info.overview;

        public Army(ArmyInfo info)
        {
            m_info = info;
            m_troopCount = info.GetTroopCount();

            m_availableAttackingGroups = new List<IAttackingGroup>();
            m_usedAttackingGroups = new List<IAttackingGroup>();

            m_availableSpecialSkills = new List<ISpecialSkillGroup>();
            m_usedSpecialSkills = new List<ISpecialSkillGroup>();

            ResetGroupAvailability();
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
            m_troopCount = m_info.GetTroopCount();
        }

        public bool HasAvailableGroup(DamageType damageType)
        {
            for (int i = 0; i < m_availableAttackingGroups.Count; i++)
            {
                if (m_availableAttackingGroups[i].GetDamageType() == damageType)
                {
                    return true;
                }
            }
            return false;
        }

        public List<IAttackingGroup> GetAvailableGroups(DamageType damageType)
        {
            return m_availableAttackingGroups.Where(x => x.GetDamageType() == damageType).OrderBy(x => x.GetAttackPower()).ToList();
        }

        public void SetAttackingGroupAvailability(IAttackingGroup attackingGroup, bool isAvailable)
        {
            if (isAvailable)
            {
                var relevantSkill = GetAvailableGroup(attackingGroup.id, m_usedAttackingGroups);
                if (m_usedAttackingGroups.Contains(relevantSkill))
                {
                    m_usedAttackingGroups.Remove(relevantSkill);
                    m_availableAttackingGroups.Add(relevantSkill);
                }
            }
            else
            {
                var relevantSkill = GetAvailableGroup(attackingGroup.id, m_availableAttackingGroups);
                if (m_availableAttackingGroups.Contains(relevantSkill))
                {
                    m_availableAttackingGroups.Remove(relevantSkill);
                    m_usedAttackingGroups.Add(relevantSkill);
                }
            }
        }

        public List<ISpecialSkillGroup> GetAvailableSkills()
        {
            return m_availableSpecialSkills;
        }

        public void SetSpecialSkillAvailability(ISpecialSkillGroup group, bool isAvailable)
        {
            if (isAvailable)
            {
                if (m_usedSpecialSkills.Contains(group))
                {
                    m_usedSpecialSkills.Remove(group);
                    m_availableSpecialSkills.Add(group);
                }
            }
            else
            {
                if (m_availableSpecialSkills.Contains(group))
                {
                    m_availableSpecialSkills.Remove(group);
                    m_usedSpecialSkills.Add(group);
                }
            }
        }

        public void ResetGroupAvailability()
        {
            var armyGroups = m_info.GetGroups();
            for (int i = 0; i < armyGroups.Length; i++)
            {
                var group = armyGroups[i];
                m_availableAttackingGroups.Add(group);
                if (group.HasSpecialSkill())
                {
                    m_availableSpecialSkills.Add(group);
                }
            }

            m_usedAttackingGroups.Clear();
            m_usedSpecialSkills.Clear();
        }

        private IAttackingGroup GetAvailableGroup(int id, List<IAttackingGroup> reference)
        {
            for (int i = 0; i < reference.Count; i++)
            {
                if (reference[i].id == id)
                    return reference[i];
            }
            return null;
        }

        private ISpecialSkillGroup GetAvailableGroup(int id, List<ISpecialSkillGroup> reference)
        {
            for (int i = 0; i < reference.Count; i++)
            {
                if (reference[i].id == id)
                    return reference[i];
            }
            return null;
        }
    }
}