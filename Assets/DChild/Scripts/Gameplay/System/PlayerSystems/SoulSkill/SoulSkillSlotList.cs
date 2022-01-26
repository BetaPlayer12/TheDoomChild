using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.SoulSkills
{
    public class SoulSkillSlotList
    {
        [OdinSerialize, OnValueChanged("OnListManualChange")]
        private List<ISoulSkill> m_appliedSoulSkills;
        private int m_maxCapacityLimit;
        private int m_currentMaxCapacity;
        private int m_currentCapacity;

        public int maxCapacityLimit => m_maxCapacityLimit;
        public int currentMaxCapacity => m_currentMaxCapacity;
        public int appliedSoulSkillCount => m_appliedSoulSkills.Count;

        public bool isAppliedSoulSkillsWithinCapacity => m_currentCapacity <= m_currentMaxCapacity;

        public SoulSkillSlotList(int maxCapacityLimit, int currentMaxCapacity)
        {
            m_appliedSoulSkills = new List<ISoulSkill>();
            m_maxCapacityLimit = maxCapacityLimit;
            m_currentMaxCapacity = currentMaxCapacity;
            m_currentCapacity = 0;
        }

        public void SetMaxCapacityLimit(int maxCapacityLimit) => m_maxCapacityLimit = maxCapacityLimit;

        public void SetMaxCapacity(int maxCapacity)
        {
            m_currentMaxCapacity = (int)Mathf.Min(maxCapacity, m_maxCapacityLimit);
            if (m_currentMaxCapacity < 0)
            {
                m_currentMaxCapacity = 0;
            }
        }

        public bool Add(ISoulSkill skill)
        {
            var nextCapacity = m_currentCapacity + skill.soulCapacity;
            if (m_appliedSoulSkills.Contains(skill) || nextCapacity > m_currentMaxCapacity)
                return false;

            m_appliedSoulSkills.Add(skill);
            m_currentCapacity = nextCapacity;
            return true;
        }

        public bool Remove(ISoulSkill skill)
        {
            if (m_appliedSoulSkills.Contains(skill))
            {
                m_currentCapacity -= skill.soulCapacity;
                m_appliedSoulSkills.Remove(skill);
                return true;
            }
            return false;
        }

        public ISoulSkill[] RemoveAllSkills()
        {
            var skills = m_appliedSoulSkills.ToArray();
            m_appliedSoulSkills.Clear();
            m_currentCapacity = 0;
            return skills;
        }

        public ISoulSkill[] RemoveSoulSkillsAboveMaxCapacity()
        {
            List<ISoulSkill> removedSkills = new List<ISoulSkill>();
            for (int i = m_appliedSoulSkills.Count - 1; i >= 0; i--)
            {
                var skill = m_appliedSoulSkills[i];
                m_appliedSoulSkills.RemoveAt(i);
                removedSkills.Add(skill);
                m_currentCapacity -= skill.soulCapacity;

                if (isAppliedSoulSkillsWithinCapacity)
                    break;
            }

            return removedSkills.ToArray();
        }

        public ISoulSkill GetSoulSkill(int index) => m_appliedSoulSkills[index];
        public ISoulSkillInfo GetSoulSkillInfo(int index) => m_appliedSoulSkills[index];


        private void OnListManualChange()
        {
            m_currentCapacity = 0;
            for (int i = 0; i < m_appliedSoulSkills.Count; i++)
            {
                m_currentCapacity += m_appliedSoulSkills[i].soulCapacity;
            }
        }
    }
}
