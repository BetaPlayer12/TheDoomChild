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
        protected override bool IsValidToHit(Collider2D collision)
        {
            var searchCastResult = Raycaster.SearchCast(transform.position, collision.bounds.center, LayerMask.GetMask("Environment"), out RaycastHit2D[] hitbuffer);
            if (searchCastResult == false && collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
            {
                return collision == hitbuffer[0].collider;
            }
            else
            {
                return searchCastResult;
            }
        }
    }
}
