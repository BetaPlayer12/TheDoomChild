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
        public ArmyController ApplyEffect(ArmyController owner, ArmyController target)
        {
            float chance = Random.Range(0, 100);
            if ((chance <= m_successChance)
            {
                m_effectSuccessful = true;
            }
            throw new System.NotImplementedException();
        }

        public ArmyController RemoveEffect(ArmyController owner, ArmyController target)
        {
            throw new System.NotImplementedException();
        }
    }
}

