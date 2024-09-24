using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public struct PrimarySkillUpdateEventArgs : IEventActionArgs
    {
        public PrimarySkillUpdateEventArgs(PrimarySkill skill, bool isEnabled) : this()
        {
            this.skill = skill;
            this.isEnabled = isEnabled;
        }

        public PrimarySkill skill { get; }
        public bool isEnabled { get; }
    }

    public class PlayerSkills : SerializedMonoBehaviour, IPrimarySkills
    {
        [SerializeField]
        private PlayerModuleActivator m_moduleActivator;
        [SerializeField, EnumToggleButtons,OnValueChanged("UpdateAllSkillState")]
        private PrimarySkill m_skills;

        public event EventAction<PrimarySkillUpdateEventArgs> SkillUpdate;

        public void SetSkillStatus(PrimarySkill skill, bool enableSkill)
        {
            if (enableSkill)
            {
                m_skills |= skill;
            }
            else
            {
                m_skills &= ~skill;
            }
            m_moduleActivator.SetModuleLock(skill, enableSkill);
            SkillUpdate?.Invoke(this, new PrimarySkillUpdateEventArgs(skill, enableSkill));
        }

        public bool IsSkillUnlocked(PrimarySkill skill) => m_moduleActivator.IsModuleUnlock(skill);

        public PrimarySkillsData SaveData()
        {
            return new PrimarySkillsData(m_skills);
        }

        public void LoadData(PrimarySkillsData savedData)
        {
            m_moduleActivator.Validate();
            m_moduleActivator.Reset();
            m_skills = savedData.activatedSkills;
            UpdateAllSkillState();
        }

        private void UpdateAllSkillState()
        {
            var enumValue = Enum.GetValues(typeof(PrimarySkill));
            foreach (PrimarySkill value in enumValue)
            {
                if (value != PrimarySkill.None && value != PrimarySkill.All)
                {
                    var isUnlocked = m_skills.HasFlag(value);
                    m_moduleActivator.SetModuleLock(value, isUnlocked);
                }
            }
        }
    }
}