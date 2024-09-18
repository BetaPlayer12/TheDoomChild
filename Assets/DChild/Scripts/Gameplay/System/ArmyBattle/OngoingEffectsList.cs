using System;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{
    [System.Serializable]
    public class OngoingEffectsList : IArmyAbilityEffect
    {
        [SerializeField, Min(2)]
        private int m_roundDuration = 2;
        [SerializeField]
        private IArmyAbilityEffect[] m_effects = new IArmyAbilityEffect[0];

        public void ApplyEffect(Army owner, Army opponent)
        {
            Action effect = () =>
            {
                for (int i = 0; i < m_effects.Length; i++)
                {
                    m_effects[i].ApplyEffect(owner, opponent);
                }
            };

            //ArmyBattleSystem.HandleOngoingEffect(effect, m_roundDuration);
        }
    }
}