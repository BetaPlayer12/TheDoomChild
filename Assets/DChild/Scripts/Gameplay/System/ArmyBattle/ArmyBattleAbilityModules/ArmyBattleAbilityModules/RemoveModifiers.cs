using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class RemoveModifiers : ISpecialSkillModule, ISpecialSkillImplementor
    {
        public void ApplyEffect(ArmyController owner, ArmyController target)
        {
            target.controlledArmy.modifiers.Reset();
            throw new System.NotImplementedException();
        }

        public void RemoveEffect(ArmyController owner, ArmyController target)
        {
            throw new System.NotImplementedException();
        }
    }
}

