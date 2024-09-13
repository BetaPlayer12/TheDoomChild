using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class RemoveModifiers : ISpecialSkillModule, ISpecialSkillImplementor
    {
        public ArmyController ApplyEffect(ArmyController owner, ArmyController target)
        {
            target.controlledArmy.damageReductionModifier.ResetModifiers();
            throw new System.NotImplementedException();
        }

        public ArmyController RemoveEffect(ArmyController owner, ArmyController target)
        {
            target.controlledArmy.damageReductionModifier.ResetModifiers();
            throw new System.NotImplementedException();
        }
    }
}

