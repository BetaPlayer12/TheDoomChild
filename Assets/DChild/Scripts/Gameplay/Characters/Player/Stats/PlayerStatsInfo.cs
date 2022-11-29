using Holysoft.Event;
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
                case PlayerStat.CritChance:
                    critChance += value;
                    break;
                case PlayerStat.StatusChance:
                    statusChance += value;
                    break;
                default:
                    Debug.LogError($"{stat} cannot be added");
                    break;
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
                case PlayerStat.CritChance:
                    critChance = value;
                    break;
                case PlayerStat.StatusChance:
                    statusChance = value;
                    break;
                default:
                    Debug.LogError($"{stat} cannot be Set");
                    break;
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
                case PlayerStat.CritChance:
                    return critChance;
                case PlayerStat.StatusChance:
                    return statusChance;
                case PlayerStat.MaxAttack:
                    return attack + magicAttack;
                default:
                    throw new System.Exception($"{stat} does not exist");
            }
        }

        public void CopyInfo(PlayerStatsInfo playerStatsInfo)
        {
            health = playerStatsInfo.GetStat(PlayerStat.Health);
            magic = playerStatsInfo.GetStat(PlayerStat.Magic);
            attack = playerStatsInfo.GetStat(PlayerStat.Attack);
            magicAttack = playerStatsInfo.GetStat(PlayerStat.MagicAttack);
            critChance = playerStatsInfo.GetStat(PlayerStat.CritChance);
            statusChance = playerStatsInfo.GetStat(PlayerStat.StatusChance);
        }
    }
}