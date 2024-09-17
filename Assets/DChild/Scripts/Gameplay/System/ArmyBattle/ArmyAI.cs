using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyAI : ArmyController
    {
        [SerializeField]
        private ArmyAIData m_AiAttackData;

        public override ArmyTurnAction GetTurnAction(int turnNumber)
        {
            var chosenAttack = m_AiAttackData.ChooseAttack(turnNumber);
            return new ArmyTurnAction()
            {
                troopCount = m_controlledArmy.troopCount,
                attack = new ArmyDamage(DamageType.Melee, 30)
            };
        }
    }
}