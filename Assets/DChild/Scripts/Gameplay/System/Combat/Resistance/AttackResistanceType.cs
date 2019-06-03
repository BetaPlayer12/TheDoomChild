using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public enum AttackResistanceType
    {
        [HideInInspector]
        None,
        Weak,
        Strong,
        Immune,
        Absorb
    }
}