using DChild.Gameplay.Characters.Players;
using DChild.Serialization;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections.Generic;
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
        [SerializeField, HideReferenceObjectPicker]
        private Dictionary<PrimarySkill, bool> m_skills = new Dictionary<PrimarySkill, bool>();

        public event EventAction<PrimarySkillUpdateEventArgs> SkillUpdate;

        public void SetSkillStatus(PrimarySkill skill, bool enableSkill)
        {
            m_skills[skill] = enableSkill;
            m_moduleActivator.SetModuleLock(skill, enableSkill);
            SkillUpdate?.Invoke(this, new PrimarySkillUpdateEventArgs(skill, enableSkill));
        }

        public PrimarySkillsData SaveData()
        {
            var size = (int)PrimarySkill._COUNT;
            var data = new bool[size];
            for (int i = 0; i < size; i++)
            {
                data[i] = m_skills[(PrimarySkill)i];
            }
            return new PrimarySkillsData(data);
        }

        public void LoadData(PrimarySkillsData savedData)
        {
            var data = savedData.movementSkills;
            if (data != null)
            {
                m_moduleActivator.Validate();
                m_moduleActivator.Reset();
                for (int i = 0; i < data.Length; i++)
                {
                    var skill = (PrimarySkill)i;
                    var isUnlocked = data[i];
                    m_skills[skill] = isUnlocked;
                    m_moduleActivator.SetModuleLock(skill, isUnlocked);
                }
            }
        }
    }
}