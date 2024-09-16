using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public class ArmyAbilityList
    {
        [SerializeField]
        private List<ArmyAbilityGroup> m_abilityGroups;

        public ArmyAbilityList()
        {
            m_abilityGroups = new List<ArmyAbilityGroup>();
        }

        public ArmyAbilityList(ArmyAbilityGroup[] abilities)
        {
            m_abilityGroups = new List<ArmyAbilityGroup>(abilities);
        }
        public ArmyAbilityList(ArmyAbilityList reference)
        {

        }

        public int count => m_abilityGroups.Count;

        public ArmyAbilityGroup GetAbilityGroup(int index) => m_abilityGroups[index];

        public void ResetAvailability()
        {
            for (int i = 0; i < m_abilityGroups.Count; i++)
            {
                //m_abilityGroups[i].SetAvailability(true);
            }
        }
    }
}