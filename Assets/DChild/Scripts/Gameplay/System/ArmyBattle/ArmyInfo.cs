using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    public class ArmyInfo 
    {
        [SerializeField]
        private ArmyGroup[] m_groups;
        
        public string name;

        public ArmyInfo(string armyName, ArmyGroup[] groups)
        {
            armyName = name;
            m_groups = groups;
        }

        public int GetTroopCount()
        {
            return m_groups.Length;
        }

        public ArmyGroup GetGroups(ArmyGroup[] groups)
        {
            return groups[0];
        }
    }
}

