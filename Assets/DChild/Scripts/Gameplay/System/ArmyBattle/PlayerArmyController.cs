using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class PlayerArmyController : ArmyController
    {
        [SerializeField]
        private ArmyGroup m_chosenAttack;

        public void UseThisTurn(ArmyGroup chosenAttack)
        {
            m_chosenAttack = chosenAttack;
        }

        public override ArmyTurnAction GetTurnAction(int turnNumber)
        {
            return new ArmyTurnAction()
            {
                troopCount = m_controlledArmy.troopCount,
                //attack = new ArmyDamage(m_chosenAttack.GetDamageType(), m_chosenAttack.GetAttackPower())
            };
        }
    }
}