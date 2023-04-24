using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public class BattleAbilities : SerializedMonoBehaviour
    {
        private int[] m_abilityLevels;

        public bool IsAbilityActivated(CombatArt battleAbility)
        {
            return GetAbilityLevel(battleAbility) > 0;
        }

        public int GetAbilityLevel(CombatArt battleAbility)
        {
            return m_abilityLevels[(int)battleAbility];
        }

        public void SetAbilityLevel(CombatArt battleAbility, int level)
        {
            m_abilityLevels[(int)battleAbility] = Mathf.Max(0, level);
        }

        private void Awake()
        {
            if (m_abilityLevels == null)
            {
                m_abilityLevels = new int[(int)CombatArt._Count];
            }
        }
    }
}