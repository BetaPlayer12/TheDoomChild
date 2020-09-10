/***************************************************
 * 
 * Attackers should look for this in order to damage an Object
 * 
 ***************************************************/
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [RequireComponent(typeof(Collider2D))]
    [AddComponentMenu("DChild/Gameplay/Combat/Picky Layer Hitbox")]
    public class PickyLayerHitbox : Hitbox
    {
        [SerializeField]
        private LayerMask m_mask;

        public override bool CanBeDamageBy(params Collider2D[] colliders)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (m_mask.value == (m_mask.value | 1 << colliders[i].gameObject.layer))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
