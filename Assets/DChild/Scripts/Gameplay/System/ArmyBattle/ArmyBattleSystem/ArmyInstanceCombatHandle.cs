using DChild.Gameplay.Combat;

namespace DChild.Gameplay.ArmyBattle
{

    public class ArmyInstanceCombatHandle : IArmyCombatInfo, IArmyCombatHandle
    {
        private ArmyController m_armyController;
        private int m_attackLeftCount;
        private bool m_isTurnSkipped;

        public ArmyController armyController => m_armyController;
        //public ArmyAttack attackInfo => m_armyController.currentAttack;
        //public ArmyDamageTypeModifier damageReductionModifier => m_armyController.controlledArmy.damageReductionModifier;

        public bool canAttack => !m_isTurnSkipped && m_attackLeftCount > 0;

        public int troopCount => m_armyController.army.troopCount;

        Health IArmyCombatInfo.troopCount => throw new System.NotImplementedException();

        public ArmyDamageTypeModifier damageReductionModifier => throw new System.NotImplementedException();

        public ArmyAttack attackInfo => throw new System.NotImplementedException();

        public ArmyInstanceCombatHandle(ArmyController armyController)
        {
            m_armyController = armyController;
            m_attackLeftCount = 1;
        }

        public void InitializeArmy()
        {
            //armyController.controlledArmy.Initialize();
        }

        public void HandleAttackEnd()
        {
            m_attackLeftCount--;
            //m_armyController.DisposeCurrentAttack();
        }

        public void SkipTurn()
        {
            m_isTurnSkipped = true;
        }

        public void Reset()
        {
            m_attackLeftCount = 1;
            m_isTurnSkipped = false;
        }

        public void AddExtraAttack(int extraAttackCount)
        {
            m_attackLeftCount += extraAttackCount;
        }
    }
}