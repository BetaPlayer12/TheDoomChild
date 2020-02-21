/***************************************************
 * 
 * Attackers should look for this in order to damage an Object
 * 
 ***************************************************/
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [AddComponentMenu("DChild/Gameplay/Combat/Ray Collider Damage")]
    public class RayColliderDamage : ColliderDamage
    {
        protected override bool IsValidToDamage(Collider2D collision)
        {
            return Raycaster.SearchCast(transform.position, collision.bounds.center, LayerMask.GetMask("Environment"));
        }
    }
}
