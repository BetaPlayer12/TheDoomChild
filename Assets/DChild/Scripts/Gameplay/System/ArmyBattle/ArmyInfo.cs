using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public class ArmyInfo
    {
        [SerializeField]
        private ArmyOverviewData m_overview;
        [SerializeField]
        private Sprite m_icon;

        [SerializeField, Min(1)]
        private int m_troopCount;
        [SerializeField, ListDrawerSettings(NumberOfItemsPerPage = 5)]
        private ArmyGroup[] m_groups;

        public ArmyOverviewData overview => m_overview;

        public ArmyInfo(ArmyOverviewData overview, int troopCount, ArmyGroup[] groups)
        {
            m_overview = overview;
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

