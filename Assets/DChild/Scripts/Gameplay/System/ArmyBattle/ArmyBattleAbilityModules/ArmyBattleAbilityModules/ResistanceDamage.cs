using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class ResistanceDamage : ISpecialSkillModule, ISpecialSkillImplementor
    {
        [SerializeField]
        private UnitType m_unitType;
        [SerializeField]
        private float m_modifier;
        public ArmyController ApplyEffect(ArmyController owner, ArmyController target)
        {
            //target.controlledArmy.damageReductionModifier.SetModifier(m_unitType, m_modifier);
            throw new System.NotImplementedException();
        }

        public ArmyController RemoveEffect(ArmyController owner, ArmyController target)
        {
            //target.controlledArmy.damageReductionModifier.SetModifier(m_unitType, m_modifier);
            throw new System.NotImplementedException();
        }
    }
}

