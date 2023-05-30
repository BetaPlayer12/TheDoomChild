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
            return true;
        }

        public void Use(IPlayer player)
        {
            player.statusEffectReciever.StopStatusEffect(m_toCure);
        }

        public override string ToString()
        {
            return $"Cure {m_toCure.ToString()}";
        }
    }
}
