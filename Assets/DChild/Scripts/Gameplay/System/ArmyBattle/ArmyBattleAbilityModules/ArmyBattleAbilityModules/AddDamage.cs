using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class AddDamage : ISpecialSkillModule, ISpecialSkillImplementor
    {
        [SerializeField]
        private int m_damageModiferValue;
        [SerializeField]
        private UnitType m_unit;

        public ArmyController ApplyEffect(ArmyController owner, ArmyController target)
        {
            owner.controlledArmy.powerModifier.SetModifier(m_unit, m_damageModiferValue);
            throw new System.NotImplementedException();
        }

        public ArmyController RemoveEffect(ArmyController owner, ArmyController target)
        {
            owner.controlledArmy.powerModifier.ResetModifiers();
            throw new System.NotImplementedException();
        }
    }
}

