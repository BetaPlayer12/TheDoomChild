using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public class BattleAbilities : SerializedMonoBehaviour
    {
        private int[] m_abilityLevels;

        public bool IsAbilityActivated(BattleAbility battleAbility)
        {
            // return GetAbilityLevel(battleAbility) > 0;
            return true; //Temporary Setup
        }

        public int GetAbilityLevel(BattleAbility battleAbility)
        {
            return m_abilityLevels[(int)battleAbility];
        }

        public void SetAbilityLevel(BattleAbility battleAbility, int level)
        {
            m_abilityLevels[(int)battleAbility] = Mathf.Min(0, level);
        }

        private void Awake()
        {
            if (m_abilityLevels == null)
            {
                m_abilityLevels = new int[(int)BattleAbility._Count];
            }
        }
    }
}