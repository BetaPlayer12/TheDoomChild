using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyInfo 
    {
        [SerializeField]
        private ArmyGroup[] m_groups;
        [SerializeField]
        private string m_name;
        
        public string name => m_name;

        public ArmyInfo(string armyName, ArmyGroup[] groups)
        {
            m_name = armyName;
            m_groups = groups;
        }

        public int GetTroopCount()
        {
            return m_groups.Length;
        }

        public ArmyGroup[] GetGroups()
        {
            return m_groups;
        }
    }
}

