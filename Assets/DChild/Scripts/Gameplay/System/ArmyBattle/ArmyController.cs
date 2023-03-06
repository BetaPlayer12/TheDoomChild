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
        protected ArmyAttackGroup m_currentAttackGroup;

        public ArmyAttack currentAttack => m_currentAttack;
        public ArmyAttackGroup currentAttackGroup => m_currentAttackGroup;
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

        protected virtual void ChooseAttack(UnitType unitType)
        {
            var chosenGroups = m_controlledArmy.GetAvailableAttackGroups(unitType);
            m_currentAttackGroup = chosenGroups[Random.Range(0, chosenGroups.Count)];
            m_currentAttack = CreateAttack(m_currentAttackGroup);
            SendAttackChosenEvent(CreateAttackEvent(m_currentAttack));
        }

        protected void SendAttackChosenEvent(ArmyAttackEvent armyAttackEvent)
        {
            AttackChosen?.Invoke(this, armyAttackEvent);
        }

        protected ArmyAttack CreateAttack(ArmyAttackGroup armyAttackGroup) => new ArmyAttack(armyAttackGroup.unitType, armyAttackGroup.GetTotalPower());

        protected ArmyAttackEvent CreateAttackEvent(ArmyAttack armyAttack) => new ArmyAttackEvent(armyAttack);
    }
}