﻿using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    [System.Serializable]
    public struct PlayerStatsInfo
    {
        [SerializeField,MinValue(0)]
        private int health;
        [SerializeField, MinValue(0)]
        private int magic;
        [SerializeField, MinValue(0)]
        private int attack;
        [SerializeField, MinValue(0)]
        private int magicAttack;
        [SerializeField, MinValue(0)]
        private int defense;
        [SerializeField, MinValue(0)]
        private int magicDefense;
        [SerializeField, MinValue(0)]
        private int critChance;
        [SerializeField, MinValue(0)]
        private int statusChance;

        public void AddStat(PlayerStat stat, int value)
        {
            switch (stat)
            {
                case PlayerStat.Health:
                    health += value;
                    break;
                case PlayerStat.Magic:
                    magic += value;
                    break;
                case PlayerStat.Attack:
                    attack += value;
                    break;
                case PlayerStat.MagicAttack:
                    magicAttack += value;
                    break;
                case PlayerStat.Defense:
                    defense += value;
                    break;
                case PlayerStat.MagicDefense:
                    magicDefense += value;
                    break;
                case PlayerStat.CritChance:
                    critChance += value;
                    break;
                case PlayerStat.StatusChance:
                    statusChance += value;
                    break;
                default:
                    throw new System.Exception($"{stat} cannot be added");
            }
        }

        public void SetStat(PlayerStat stat, int value)
        {
            switch (stat)
            {
                case PlayerStat.Health:
                    health = value;
                    break;
                case PlayerStat.Magic:
                    magic = value;
                    break;
                case PlayerStat.Attack:
                    attack = value;
                    break;
                case PlayerStat.MagicAttack:
                    magicAttack = value;
                    break;
                case PlayerStat.Defense:
                    defense = value;
                    break;
                case PlayerStat.MagicDefense:
                    magicDefense = value;
                    break;
                case PlayerStat.CritChance:
                    critChance = value;
                    break;
                case PlayerStat.StatusChance:
                    statusChance = value;
                    break;
                default:
                    throw new System.Exception($"{stat} cannot be added");
            }
        }

        public int GetStat(PlayerStat stat)
        {
            switch (stat)
            {
                case PlayerStat.Health:
                    return health;
                case PlayerStat.Magic:
                    return magic;
                case PlayerStat.Attack:
                    return attack;
                case PlayerStat.MagicAttack:
                    return magicAttack;
                case PlayerStat.Defense:
                    return defense;
                case PlayerStat.MagicDefense:
                    return magicDefense;
                case PlayerStat.CritChance:
                    return critChance;
                case PlayerStat.StatusChance:
                    return statusChance;
                default:
                    throw new System.Exception($"{stat} cannot be added");
            }
        }
    }
}