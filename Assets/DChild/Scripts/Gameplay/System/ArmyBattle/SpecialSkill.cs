using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class SpecialSkill : ISpecialSkillImplementor
    {
        [SerializeField]
        private string m_description;
        [SerializeField, Min(1)]
        private int m_duration = 1;
        [SerializeField]
        private ISpecialSkillModule[] m_specialSkillModules;

        public int duration => m_duration;
        public string GetDescription() { return m_description; }

        public void ApplyEffect(ArmyController owner, ArmyController target)
        {
            for (int i = 0; i < m_specialSkillModules.Length; i++)
            {
                m_specialSkillModules[i].ApplyEffect(owner, target);
            }
        }


        public void RemoveEffect(ArmyController owner, ArmyController target)
        {
            for (int i = 0; i < m_specialSkillModules.Length; i++)
            {
                m_specialSkillModules[i].RemoveEffect(owner, target);
            }
        }
    }
}

