using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyAI : ArmyController
    {
        [SerializeField]
        private ArmyAIData m_AiAttackData;
        public void ChooseAttack(int round)
        {
            var chosenArmyGroup = m_AiAttackData.ChooseAttack(round);
            //m_currentAttackGroup = new ArmyAttackGroup(chosenArmyGroup);
            m_currentAttack = CreateAttack(m_currentAttackGroup);
            SendAttackChosenEvent(CreateAttackEvent(m_currentAttack));
        }
        /* UnitType chosenAttackType;
         do
         {
             chosenAttackType = (UnitType)Random.Range(0, 3);
         } while (controlledArmy.HasAvailableAttackGroup(chosenAttackType) == false);
         ChooseAttack(chosenAttackType);*/

    }
}