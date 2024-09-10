using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class SpecialSkill
    {
        [SerializeField]
        private string m_description;
        [SerializeField]
        private ISpecialSkillModule[] m_specialSkillModules;

        public string GetDescription() { return m_description; }
    }
}

