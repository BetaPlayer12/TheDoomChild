using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class DisableUnitType : ISpecialSkillModule, ISpecialSkillImplementor
    {
        [SerializeField]
        private DamageType m_damageType;
        public void ApplyEffect(ArmyController owner, ArmyController target)
        {
            //target.controlledArmy.SetAttackingGroups(m_damageType);
            throw new System.NotImplementedException();
        }

        public void RemoveEffect(ArmyController owner, ArmyController target)
        {
            target.controlledArmy.ResetGroupAvailability();
            throw new System.NotImplementedException();
        }
    }
}

