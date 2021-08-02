/***************************************************
 * 
 * Attackers should look for this in order to damage an Object
 * 
 ***************************************************/
using DChild.Gameplay.Environment.Interractables;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public class WhipColliderDamage : ColliderDamage
    {
        private const string ENVIRONMENT_LAYER = "Environment";

        private static List<Collider2D> m_canPassthroughList;

        public static void InitializePassables(IEnumerable<Collider2D> list)
        {
            if (m_canPassthroughList == null)
            {
                m_canPassthroughList = new List<Collider2D>();
            }
            else
            {
                m_canPassthroughList.Clear();
            }

            m_canPassthroughList.AddRange(list);
        }

        protected override bool IsValidColliderToHit(Collider2D collision)
        {
            var position = transform.position;
            var direction = collision.bounds.center - position;
            var castedAll = Raycaster.CastAll(position, direction.normalized, direction.magnitude);
            // Cannot Hit Enemies on the other side of grills

            if (collision.TryGetComponentInParent(out IHitToInteract interactable))
            {
                if (m_canDetectInteractables)
                {
                    return HasNoObstruction(collision, castedAll);
                }
                else
                {
                    return false;
                }
            }
            else if (collision.TryGetComponentInParent(out IDamageable damageable))
            {
                if (Raycaster.SearchCast(transform.position, collision.bounds.center, LayerMask.GetMask(ENVIRONMENT_LAYER), out RaycastHit2D[] buffer))
                {
                    return true;
                }
                else
                {
                    return HasNoObstruction(collision, castedAll);
                }
            }
            else
            {
                return Raycaster.SearchCast(transform.position, collision.bounds.center, LayerMask.GetMask(ENVIRONMENT_LAYER), out RaycastHit2D[] buffer);
            }
        }

        private bool HasNoObstruction(Collider2D collision, RaycastHit2D[] castedAll)
        {
            for (int i = 0; i < castedAll.Length; i++)
            {
                var collider = castedAll[i].collider;
                if (collider == collision || collider.CompareTag("Sensor"))
                {
                    break;
                }
                else if (m_canPassthroughList.Contains(collider) == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
