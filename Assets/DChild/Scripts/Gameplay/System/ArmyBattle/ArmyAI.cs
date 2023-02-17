using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyAI : ArmyController
    {
        public void ChooseAttack()
        {
            ChooseAttack((UnitType)Random.Range(0, 3));
        }
    }
}