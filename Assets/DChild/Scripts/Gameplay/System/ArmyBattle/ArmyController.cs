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

        protected ArmyAttackGroup m_currentAttackGroup;

        public Army controlledArmy => m_controlledArmy;

        public abstract ArmyTurnAction GetTurnAction(int turnNumber);

        public void SetArmyToControl(Army army)
        {
            m_controlledArmy = army;
        }

        public ArmyGroup[] GetGroups()
        {
            throw new NotImplementedException();
        }
    }
}