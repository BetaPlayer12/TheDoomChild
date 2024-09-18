using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class TroopHeal : ISpecialSkillModule, ISpecialSkillImplementor
    {
        [SerializeField]
        private int m_troopCount;

        public ArmyController ApplyEffect(ArmyController owner, ArmyController target)
        {
            owner.army.AddTroopCount(m_troopCount);
            throw new System.NotImplementedException();
        }

        public ArmyController RemoveEffect(ArmyController owner, ArmyController target)
        {
            owner.army.SubtractTroopCount(m_troopCount);
            throw new System.NotImplementedException();
        }
    }
}

