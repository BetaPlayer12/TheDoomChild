using Holysoft.Event;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using System;

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
        public Army controlledArmy => m_controlledArmy;

        public event EventAction<ArmyAttackEvent> AttackChosen;
        public event EventAction<ArmyAbilityEvent> AbilityChosen;

        public void DisposeCurrentAttack()
        {
            m_currentAttack = new ArmyAttack(UnitType.None, 0, 0);
            /*m_currentAttackGroup.SetAvailability(false);
            m_currentAttackGroup = null;*/
        }

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

        public virtual void ChooseSpecial()
        {
            m_controlledArmy.GetAvailableAbilityGroups();
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

        protected void SendAbilityChosenEvent(ArmyAbilityGroup armyAttackEvent)
        {
            using (Cache<ArmyAbilityEvent> eventArgs = Cache<ArmyAbilityEvent>.Claim())
            {
                eventArgs.Value.Set(armyAttackEvent);
                AbilityChosen?.Invoke(this, eventArgs.Value);
                eventArgs.Release();
            }
        }

        protected ArmyAttack CreateAttack(ArmyAttackGroup armyAttackGroup)
        {
            ////return new ArmyAttack(armyAttackGroup.unitType, armyAttackGroup.GetTotalPower(), m_controlledArmy.powerModifier.GetModifier(armyAttackGroup.unitType));
            throw new NotImplementedException();
        }

        protected ArmyAttackEvent CreateAttackEvent(ArmyAttack armyAttack) => new ArmyAttackEvent(armyAttack);
    }
}