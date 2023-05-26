
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat.StatusAilment;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    [System.Serializable]
    public struct PlayerStatusImmunityItemEffect : IDurationItemEffect
    {
        [SerializeField]
        private StatusEffectType m_type;

        public void StartEffect(IPlayer player)
        {
            player.statusResistance.SetResistance(m_type, 100);
        }

        public void StopEffect(IPlayer player)
        {
            player.statusResistance.SetResistance(m_type, 0);
        }
    }
}
