using DChild.Gameplay.Characters;
using DChild.Gameplay.VFX;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public class CombatFXHandle
    {
        private FXSpawnHandle<FX> m_fXSpawnHandle;

        public void SpawnFX(Collider2D attackerCollision, GameObject attackerFX, Collider2D targetCollision,IDamageable target,FXSpawnConfigurationInfo targetFX)
        {
            if (attackerFX != null || targetFX != null)
            {
                Vector2 hitPoint = FindNearestContactPoint(attackerCollision, targetCollision);
                var hitDirection = attackerCollision != null ? GameplayUtility.GetHorizontalDirection(attackerCollision.bounds.center, hitPoint) : HorizontalDirection.Right;
                if (attackerFX != null)
                {
                    m_fXSpawnHandle.InstantiateFX(attackerFX, hitPoint, hitDirection);
                    Debug.Log(hitPoint);
                }
                if (targetFX.fx != null)
                {
                    m_fXSpawnHandle.InstantiateFX(targetFX, hitPoint, hitDirection, target.transform);
                    Debug.Log(hitPoint);
                }
            }
        }

        private Vector3 FindNearestContactPoint(Collider2D attackerCollision, Collider2D targetCollision)
        {
            if (attackerCollision != null)
            {
                ColliderDistance2D colliderDistance = attackerCollision.Distance(targetCollision);
                if (!colliderDistance.isValid)
                {
                    return Vector3.zero;
                }

                // if its overlapped then this collider's nearest vertex is inside the other collider
                // so the position adjustment shouldn't be necessary
                if (colliderDistance.isOverlapped)
                {
                    return colliderDistance.pointA; // point on the surface of this collider
                }
                else
                {
                    // move the hit location inside the collider a bit
                    // this assumes the colliders are basically touching
                    return colliderDistance.pointB - (0.01f * colliderDistance.normal);
                }
            }
            else
            {
                var bounds = targetCollision.bounds;
                var extents = bounds.extents;
                var xExtents = bounds.extents.x;
                var yExtents = bounds.extents.y;
                return bounds.center + new Vector3(Random.Range(-xExtents, xExtents), Random.Range(-yExtents, yExtents));
            }
        }
    }
}