using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class TroopHeal : ISpecialSkillModule, ISpecialSkillImplementor
    {
        [SerializeField]
        private int m_troopCount;

        public void ApplyEffect(ArmyController owner, ArmyController target)
        {
            owner.controlledArmy.AddTroopCount(m_troopCount);
            throw new System.NotImplementedException();
        }

        public void RemoveEffect(ArmyController owner, ArmyController target)
        {
            owner.controlledArmy.SubtractTroopCount(m_troopCount);
            throw new System.NotImplementedException();
        }
    }
}

