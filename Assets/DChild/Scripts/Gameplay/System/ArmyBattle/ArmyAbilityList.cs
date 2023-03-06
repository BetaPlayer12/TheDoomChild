using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public class ArmyAbilityList
    {
        [SerializeField]
        private ArmyAbility[] m_abilities;

        public ArmyAbilityList(params ArmyAbility[] abilities)
        {
            m_abilities = abilities;
        }

        public ArmyAbility[] abilities => m_abilities;

        public void ResetAbilities()
        {
            for (int i = 0; i < m_abilities.Length; i++)
            {
                m_abilities[i].ResetUseCount();
            }
        }
    }
}