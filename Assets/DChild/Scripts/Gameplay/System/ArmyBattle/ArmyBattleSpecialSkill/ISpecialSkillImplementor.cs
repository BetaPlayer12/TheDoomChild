using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.SpecialSkills
{
    public interface ISpecialSkillImplementor 
    {
        void ApplyEffect(ArmyController owner, ArmyController target);
        void RemoveEffect(ArmyController owner, ArmyController target);
    }
}

