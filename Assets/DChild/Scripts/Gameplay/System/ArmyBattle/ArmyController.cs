using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public abstract class ArmyController : MonoBehaviour
    {
        [SerializeField]
        protected Army m_controlledArmy;

        [ShowInInspector, HideInPlayMode, DisableInPlayMode]
        protected ArmyAttack m_currentAttack;

        public ArmyAttack currentAttack => m_currentAttack;

        public Army controlledArmy => m_controlledArmy;

        public event EventAction<ArmyAttackEvent> AttackChosen;

        public void ChooseRockAttack()
        {
            ChooseAttack(UnitType.Rock);
        }

        public void ChooseScissorAttack()
        {
            ChooseAttack(UnitType.Scissors);
        }

        public void ChoosePaperAttacker()
        {
            ChooseAttack(UnitType.Paper);
        }

        protected void ChooseAttack(UnitType unitType)
        {
            m_currentAttack = CreateAttack(unitType);
            SendAttackChosenEvent(CreateAttackEvent(m_currentAttack));
        }

        protected void SendAttackChosenEvent(ArmyAttackEvent armyAttackEvent)
        {
            AttackChosen?.Invoke(this, armyAttackEvent);
        }

        private ArmyAttack CreateAttack(UnitType unitType) => new ArmyAttack(unitType, m_controlledArmy.GetPower(unitType));

        private ArmyAttackEvent CreateAttackEvent(ArmyAttack armyAttack) => new ArmyAttackEvent(armyAttack);
    }
}