using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class DealDamage : ISpecialSkillImplementor, ISpecialSkillModule
    {
        [SerializeField]
        private int m_damageDealt;
        public ArmyController ApplyEffect(ArmyController owner, ArmyController target)
        {
            target.army.SubtractTroopCount(m_damageDealt);
            throw new System.NotImplementedException();
        }

        public ArmyController RemoveEffect(ArmyController owner, ArmyController target)
        {
            target.army.SubtractTroopCount(m_damageDealt);
            throw new System.NotImplementedException();
        }
    }
}

