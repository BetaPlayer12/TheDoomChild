using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public class ArmyInfo
    {
        [SerializeField]
        private string m_name;
        [SerializeField, Min(1)]
        private int m_troopCount;
        [SerializeField]
        private ArmyGroup[] m_groups;

        public string name => m_name;

        public ArmyInfo(string armyName, int troopCount, ArmyGroup[] groups)
        {
            m_name = armyName;
            m_troopCount = troopCount;
            m_groups = groups;
        }

        public int GetTroopCount()
        {
            return m_troopCount;
        }

        public ArmyGroup[] GetGroups()
        {
            return m_groups;
        }
    }
}

