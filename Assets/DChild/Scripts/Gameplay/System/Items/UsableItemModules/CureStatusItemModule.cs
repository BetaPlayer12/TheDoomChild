using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat.StatusAilment;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    [System.Serializable]
    public struct CureStatusItemModule : IUsableItemModule
    {
        [SerializeField]
        private StatusEffectType m_toCure;

        public bool CanBeUse(IPlayer player)
        {
            return player.statusEffectReciever.IsInflictedWith(m_toCure);
        }

        public void Use(IPlayer player)
        {
            player.statusEffectReciever.StopStatusEffect(m_toCure);
        }
    }
}
