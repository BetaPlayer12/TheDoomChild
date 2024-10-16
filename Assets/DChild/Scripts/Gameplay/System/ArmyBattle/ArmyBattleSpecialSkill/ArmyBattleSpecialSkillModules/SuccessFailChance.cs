using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.SpecialSkills.Modules
{
    [InfoBox("This will not Remove the Effects of either fail or success results")]
    public class SuccessFailChance : ISpecialSkillModule, ISpecialSkillImplementor
    {
        [SerializeField, Range(0, 100)]
        private float m_successChance;
        [SerializeField, TabGroup("OnSuccess")]
        private ISpecialSkillModule[] m_onSuccess;
        [SerializeField, TabGroup("OnFail")]
        private ISpecialSkillModule[] m_onFail;

        public void ApplyEffect(ArmyController owner, ArmyController target)
        {
            float chance = Random.Range(0, 100);
            if (chance <= m_successChance)
            {
                ApplyModules(m_onSuccess, owner, target);
            }
            else
            {
                ApplyModules(m_onFail, owner, target);
            }
        }

        public void RemoveEffect(ArmyController owner, ArmyController target)
        {
            //Will not have any effect here
        }

        private void ApplyModules(ISpecialSkillModule[] specialSkillModules, ArmyController owner, ArmyController target)
        {
            for (int i = 0; i < specialSkillModules.Length; i++)
            {
                specialSkillModules[i].ApplyEffect(owner, target);
            }
        }
    }
}

