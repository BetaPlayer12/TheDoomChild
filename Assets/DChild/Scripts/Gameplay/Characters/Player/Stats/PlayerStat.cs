using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public enum PlayerStat
    {
        Health,
        Magic,
        Attack,
        MagicAttack,
        CritChance,
        StatusChance,
        [HideInInspector]
        _COUNT,
        MaxAttack,
    }
}