/***************************************************
 * 
 * Attackers should look for this in order to damage an Object
 * 
 ***************************************************/
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    [RequireComponent(typeof(Collider2D))]
    [AddComponentMenu("DChild/Gameplay/Combat/Picky Tag Hitbox")]
    public class PickyTagHitbox : Hitbox
    {
        [SerializeField]
        private string[] m_viableTags;

        public override bool CanBeDamageBy(params Collider2D[] colliders)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                for (int j = 0; j < m_viableTags.Length; j++)
                {
                    if (colliders[i].CompareTag(m_viableTags[j]))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
