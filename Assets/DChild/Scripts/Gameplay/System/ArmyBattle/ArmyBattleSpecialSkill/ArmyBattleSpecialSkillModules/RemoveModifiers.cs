using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.SpecialSkills.Modules
{
    public class RemoveModifiers : ISpecialSkillModule, ISpecialSkillImplementor
    {
        public void ApplyEffect(ArmyController owner, ArmyController target)
        {
            target.controlledArmy.modifiers.Reset();
        }

        public void RemoveEffect(ArmyController owner, ArmyController target)
        {
        }
    }
}

