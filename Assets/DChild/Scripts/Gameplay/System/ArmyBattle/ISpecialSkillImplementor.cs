using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public interface ISpecialSkillImplementor 
    {
        ArmyController ApplyEffect(ArmyController owner, ArmyController target);
        ArmyController RemoveEffect(ArmyController owner, ArmyController target);
    }
}

