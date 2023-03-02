using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyAI : ArmyController
    {
        public void ChooseAttack()
        {
            UnitType chosenAttackType;
            do
            {
                chosenAttackType = (UnitType)Random.Range(0, 3);
            } while (controlledArmy.HasAvailableAttackGroup(chosenAttackType) == false);
            ChooseAttack(chosenAttackType);
        }
    }
}