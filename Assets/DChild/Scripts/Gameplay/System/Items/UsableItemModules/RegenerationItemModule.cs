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
                if (player.health.isFull == false)
                {
                    GameplaySystem.combatManager.Heal(player.healableModule, m_value);
                }
            }
            else
            {
                if (player.magic.isFull == false)
                {
                    player.magic.AddCurrentValue(m_value);
                }
            }
        }

        public bool CanBeUse(IPlayer player)
        {
            if (m_toRegenerate == Stat.Health)
            {
                return player.health.isFull == false;
            }
            else
            {
                return player.magic.isFull == false;
            }
        }
    }
}
