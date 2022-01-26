using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.SoulSkills
{

    public class SoulSkillAcquisitionHandle : MonoBehaviour
    {
        [ShowInInspector]
        private HashSet<int> m_acquiredSoulSkills;

        //public event EventAction<SoulSkillAcquiredEventArgs> SkillAcquisistionChanged;

        public bool HasAcquired(int ID) => m_acquiredSoulSkills.Contains(ID);

        public void SetAcquisition(ISoulSkillInfo skill, bool isAcquired)
        {
            if (isAcquired)
            {
                m_acquiredSoulSkills.Add(skill.id);
            }
            else
            {
                m_acquiredSoulSkills.Remove(skill.id);
            }
        }

        public ISoulSkill[] GetAcquiredSkills()
        {
            if (m_acquiredSoulSkills.Count == 0)
                return null;

            ISoulSkill[] acquiredSkills = new ISoulSkill[m_acquiredSoulSkills.Count];

            return acquiredSkills;
        }

        public void Initialize()
        {
            m_acquiredSoulSkills = new HashSet<int>();
        }
    }
}
