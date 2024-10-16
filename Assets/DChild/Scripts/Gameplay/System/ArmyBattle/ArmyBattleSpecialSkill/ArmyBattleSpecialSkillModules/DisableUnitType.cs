using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.SpecialSkills.Modules
{
    public class DisableUnitType : ISpecialSkillModule, ISpecialSkillImplementor
    {
        [SerializeField]
        private DamageType m_damageType;
        public void ApplyEffect(ArmyController owner, ArmyController target)
        {

            // Should not Be Empty
            Debug.Log("It works now Apply the Effects");
        }

        public void RemoveEffect(ArmyController owner, ArmyController target)
        {
            target.controlledArmy.ResetGroupAvailability();
        }
    }
}

