using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class AddDamage : ISpecialSkillModule, ISpecialSkillImplementor
    {
        [SerializeField]
        private float m_damageModiferValue;
        [SerializeField]
        private DamageType m_unit;

        public void ApplyEffect(ArmyController owner, ArmyController target)
        {
            owner.controlledArmy.modifiers.damageModifier.AddModifier(m_unit, m_damageModiferValue);
        }

        public void RemoveEffect(ArmyController owner, ArmyController target)
        {
            owner.controlledArmy.modifiers.damageModifier.ResetModifiers();        
        }
    }
}

