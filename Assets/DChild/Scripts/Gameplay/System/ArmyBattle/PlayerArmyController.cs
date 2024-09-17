using Holysoft.Event;
using System;
using System.Collections.Generic;

namespace DChild.Gameplay.ArmyBattle
{

    public class PlayerArmyController : ArmyController
    {
        public event Action<List<ArmyAttackGroup>> AttackTypeChosen;
        public event Action<List<ArmyAbilityGroup>> AbilityTypeChosen;

        public void ChooseAttack(ArmyAttackGroup attackGroup)
        {
            m_currentAttackGroup = attackGroup;
            m_currentAttack = CreateAttack(attackGroup);
            SendAttackChosenEvent(CreateAttackEvent(m_currentAttack));
        }

        public void ChooseAbility(ArmyAbilityGroup abilityGroup)
        {
            SendAbilityChosenEvent(abilityGroup);
        }

        public override void ChooseSpecial()
        {
            base.ChooseSpecial();
            //AbilityTypeChosen?.Invoke(m_controlledArmy.GetAvailableAbilityGroups());
        }

        protected override void ChooseAttack(UnitType unitType)
        {
            //AttackTypeChosen?.Invoke(m_controlledArmy.GetAvailableAttackGroups(unitType));
        }
    }
}