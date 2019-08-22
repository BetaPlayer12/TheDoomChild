using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public enum AttackResistanceType
    {
        [HideInInspector]
        None = -1,
        Weak,
        Strong,
        Immune,
        Absorb
    }
}