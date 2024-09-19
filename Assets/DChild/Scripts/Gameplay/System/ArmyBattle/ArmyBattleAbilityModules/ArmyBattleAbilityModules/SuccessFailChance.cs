using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class SuccessFailChance : ISpecialSkillModule, ISpecialSkillImplementor
    {
        [SerializeField, Range(0, 100)]
        private float m_successChance;
        [SerializeField]
        private bool m_effectSuccessful;
        public void ApplyEffect(ArmyController owner, ArmyController target)
        {
            float chance = Random.Range(0, 100);
            if ((chance <= m_successChance))
            {
                m_effectSuccessful = true;
            }
        }

        public void RemoveEffect(ArmyController owner, ArmyController target)
        {

        }
    }
}

