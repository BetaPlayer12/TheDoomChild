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
        [SerializeField]
        private bool m_hasPlayerOnlyCheck;

        protected override bool IsValidToHit(Collider2D collision)
        {
            var mask = m_hasPlayerOnlyCheck ? LayerMask.GetMask("Environment", "PlayerOnly") : LayerMask.GetMask("Environment");

            var searchCastResult = Raycaster.SearchCast(transform.position, collision.bounds.center, mask, out RaycastHit2D[] hitbuffer);
            if (searchCastResult == false)
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
                {
                    return collision == hitbuffer[0].collider;
                }
                else if (m_hasPlayerOnlyCheck && collision.gameObject.layer == LayerMask.NameToLayer("PlayerOnly"))
                {
                    return collision == hitbuffer[0].collider;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return searchCastResult;
            }
        }
    }
}
