using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Items
{
    [System.Serializable]
    public struct RegenerationItemModule : IUsableItemModule
    {
        private enum Stat
        {
            Health,
            Magic
        }

        [SerializeField]
        private Stat m_toRegenerate;
        [SerializeField, MinValue(1)]
        private int m_value;

        public void Use(IPlayer player)
        {
            if (m_toRegenerate == Stat.Health)
            {
                player.health.AddCurrentValue(m_value);
            }
            else
            {
                player.magic.AddCurrentValue(m_value);
            }
        }
    }
}
