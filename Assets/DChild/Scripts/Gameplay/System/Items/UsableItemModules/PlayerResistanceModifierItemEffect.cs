using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.StatusAilment;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    [System.Serializable]
    public struct PlayerResistanceModifierItemEffect : IDurationItemEffect
    {
        [SerializeField]
        private DamageType m_type;
        [SerializeField, SuffixLabel("%", overlay: true)]
        private int m_addedResistance;

        public void StartEffect(IPlayer player)
        {
            player.attackResistance.AddResistance(m_type, m_addedResistance / 100f);
        }

        public void StopEffect(IPlayer player)
        {
            player.attackResistance.ReduceResistance(m_type, m_addedResistance / 100f);
        }
    }
}
