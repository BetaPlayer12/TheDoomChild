using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class DisableUnitType : ISpecialSkillModule, ISpecialSkillImplementor
    {
        [SerializeField]
        private DamageType m_damageType;
        public ArmyController ApplyEffect(ArmyController owner, ArmyController target)
        {
            target.controlledArmy.SetAttackingGroups(m_damageType);
            throw new System.NotImplementedException();
        }

        public ArmyController RemoveEffect(ArmyController owner, ArmyController target)
        {
            target.controlledArmy.ResetGroupAvailability();
            throw new System.NotImplementedException();
        }
    }
}

