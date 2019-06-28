using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public enum PlayerStat
    {
        Health,
        Magic,
        Attack,
        MagicAttack,
        Defense,
        MagicDefense,
        CritChance,
        StatusChance,
        MaxAttack,
        MaxDefense,
        [HideInInspector]
        _COUNT
    }
}