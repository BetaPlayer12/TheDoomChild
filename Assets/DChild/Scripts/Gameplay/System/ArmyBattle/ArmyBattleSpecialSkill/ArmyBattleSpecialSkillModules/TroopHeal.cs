using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.SpecialSkills.Modules
{
    public class TroopHeal : ISpecialSkillModule, ISpecialSkillImplementor
    {
        [SerializeField]
        private int m_troopCount;

        public void ApplyEffect(ArmyController owner, ArmyController target)
        {
            owner.controlledArmy.AddTroopCount(m_troopCount);
        
        }

        public void RemoveEffect(ArmyController owner, ArmyController target)
        {
            owner.controlledArmy.SubtractTroopCount(m_troopCount);

        }
    }
}

