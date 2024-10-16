using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.SpecialSkills.Modules
{
    public class DealDamage : ISpecialSkillImplementor, ISpecialSkillModule
    {
        [SerializeField]
        private int m_damageDealt;
        public void ApplyEffect(ArmyController owner, ArmyController target)
        {
            target.controlledArmy.SubtractTroopCount(m_damageDealt);
      
        }

        public void RemoveEffect(ArmyController owner, ArmyController target)
        {
           

        }
    }
}

