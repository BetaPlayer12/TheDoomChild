using Sirenix.OdinInspector;
using UnityEngine;
using DChild.Gameplay.Characters.Players;
using Sirenix.Serialization;
using System;

namespace DChild.Serialization
{
    [System.Serializable, HideLabel, Title("Skills")]
    public struct PrimarySkillsData
    {
        [SerializeField, ReadOnly]
        private int m_activatedSkills;

        public PrimarySkillsData(PrimarySkill skills)
        {
            m_activatedSkills = (int)skills;
#if UNITY_EDITOR
            m_activatedSkillEnum = skills;
#endif
        }

#if UNITY_EDITOR
        [SerializeField, EnumToggleButtons, OnValueChanged("OnSkillChange")]
        private PrimarySkill m_activatedSkillEnum;

        private void OnSkillChange()
        {
            m_activatedSkills = (int)m_activatedSkillEnum;
        }
#endif

        public PrimarySkill activatedSkills => (PrimarySkill)m_activatedSkills;
    }
}