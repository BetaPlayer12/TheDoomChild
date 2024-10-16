using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.SpecialSkills.Modules
{
    public class AddDamageResistance : ISpecialSkillModule, ISpecialSkillImplementor
    {
        [SerializeField]
        private DamageType m_unitType;
        [SerializeField]
        private float m_modifier;
        public void ApplyEffect(ArmyController owner, ArmyController target)
        {
            target.controlledArmy.modifiers.resistanceModifier.AddModifier(m_unitType, m_modifier);
            
        }

        public void RemoveEffect(ArmyController owner, ArmyController target)
        {
            target.controlledArmy.modifiers.resistanceModifier.ResetModifiers();
           
        }
    }
}

