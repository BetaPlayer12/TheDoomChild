using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public abstract class ArmyController : MonoBehaviour
    {
        [SerializeField]
        private Army m_controlledArmy;

        protected ArmyAttack m_currentAttack;

        public ArmyAttack currentAttack => m_currentAttack;

        public Army controlledArmy => m_controlledArmy;

        public event EventAction<ArmyAttackEvent> AttackChosen;

        protected void SendAttackChosenEvent(ArmyAttackEvent armyAttackEvent)
        {
            AttackChosen?.Invoke(this, armyAttackEvent);
        }
    }
}