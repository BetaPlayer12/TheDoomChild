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
        protected Army m_army;

        public Army army => m_army;

        public void SetArmyToControl(Army army)
        {
            m_army = army;
        }

        public ArmyGroup[] GetGroups()
        {
            throw new NotImplementedException();
        }    
    }
}