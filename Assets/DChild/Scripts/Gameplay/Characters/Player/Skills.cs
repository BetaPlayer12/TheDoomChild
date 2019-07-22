using DChild.Serialization;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    [System.Serializable, Title("Skills")]
    public class Skills : IPlayerSkills, ISkillConfigurator
    {
        [SerializeField, OnValueChanged("ValidateMovementSkill")]
        private bool[] m_movementSkillEnabled;

        public Skills()
        {
            m_movementSkillEnabled = new bool[(int)PrimarySkill._COUNT];
        }

        public bool IsEnabled(PrimarySkill skill)
        {
            return m_movementSkillEnabled[(int)skill];
        }

        public void Enable(PrimarySkill skill, bool enableSkill)
        {
            m_movementSkillEnabled[(int)skill] = enableSkill;
        }

        public void LoadData(PlayerSkillsData data)
        {
            m_movementSkillEnabled = data.movementSkills;
        }

#if UNITY_EDITOR
        public bool[] movementSkillEnabled => m_movementSkillEnabled;

        public void Initialize()
        {
            m_movementSkillEnabled = new bool[(int)PrimarySkill._COUNT];
        }
#endif
    }
}
