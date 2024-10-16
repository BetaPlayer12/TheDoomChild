using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.SpecialSkills.Modules
{
    public class ResetSkillAvailability : ISpecialSkillModule, ISpecialSkillImplementor
    {
        public void ApplyEffect(ArmyController owner, ArmyController target)
        {
            target.controlledArmy.GetAvailableSkills().Clear();
        }

        public void RemoveEffect(ArmyController owner, ArmyController target)
        {

        }
    }
}

