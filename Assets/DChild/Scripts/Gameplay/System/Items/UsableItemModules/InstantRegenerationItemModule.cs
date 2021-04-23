using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Items
{

    [System.Serializable]
    public struct InstantRegenerationItemModule : IUsableItemModule
    {
        private enum Stat
        {
            Health,
            Magic,
            Armor
        }

        [SerializeField]
        private Stat m_toRegenerate;
        [SerializeField, MinValue(1)]
        private int m_value;

        public void Use(IPlayer player)
        {
            switch (m_toRegenerate)
            {
                case Stat.Health:
                    if (player.health.isFull == false)
                    {
                        GameplaySystem.combatManager.Heal(player.healableModule, m_value);
                    }
                    break;
                case Stat.Magic:
                    if (player.magic.isFull == false)
                    {
                        player.magic.AddCurrentValue(m_value);
                    }
                    break;
                case Stat.Armor:
                    if (player.armor.isFull == false)
                    {
                        player.armor.AddCurrentValue(m_value);
                    }
                    break;
            }
        }

        public bool CanBeUse(IPlayer player)
        {
            switch (m_toRegenerate)
            {
                case Stat.Health:
                    return player.health.isFull == false;
                case Stat.Magic:
                    return player.magic.isFull == false;
                case Stat.Armor:
                    return player.armor.isFull == false;
                default:
                    return true;
            }
        }

        public override string ToString()
        {
            if (m_value >= 0)
            {
                return $"Regenerate {m_value} {m_toRegenerate.ToString()}";
            }
            else
            {
                return $"Consumes {-m_value} {m_toRegenerate.ToString()}";
            }
        }
    }
}
