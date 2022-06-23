using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class BlackBlood : DPSColliderDamage
    {
        protected override bool IsValidColliderToHit(Collider2D collision)
        {
            return !(collision.GetComponentInParent<BlackBloodImmunity>()?.isActive ??false);
        }
    }
}