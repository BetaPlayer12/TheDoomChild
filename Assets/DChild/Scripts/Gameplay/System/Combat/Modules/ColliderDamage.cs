/***************************************************
 * 
 * Attackers should look for this in order to damage an Object
 * 
 ***************************************************/
using DChild.Gameplay.Environment.Interractables;
using UnityEngine;
using System;
using DChild.Gameplay.Environment;
using Sirenix.Utilities;
using DChild.Gameplay.Characters;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.Combat
{
    public abstract class ColliderDamage : MonoBehaviour
    {
        [System.Serializable]
        public class Collider2DInfo
        {
            [SerializeField]
            private Collider2D m_target;
            [SerializeField, ShowIf("@m_target != null")]
            private Collider2D[] m_ignoreList;

            public void IgnoreColliders(bool value)
            {
                if (m_target != null)
                {
                    for (int i = 0; i < m_ignoreList.Length; i++)
                    {
                        Physics2D.IgnoreCollision(m_target, m_ignoreList[i], value);
                    }
                }
            }
        }

        [SerializeField]
        protected bool m_canDetectInteractables;
        [SerializeField]
        private bool m_damageUniqueHitboxesOnly;
        [SerializeField, ShowIf("m_damageUniqueHitboxesOnly")]
        private CollisionRegistrator m_collisionRegistrator;
        [SerializeField]
        private DamageFXHandle m_damageFXHandle;
        [SerializeField]
        private Collider2DInfo[] m_ignoreColliderList;


        private Collider2D[] m_colliders;
        private Collider2D m_triggeredCollider;
        private ColliderDistance2D m_triggeredColliderDistance2D;
        private IDamageDealer m_damageDealer;
        public event Action<TargetInfo, Collider2D> DamageableDetected; //Turn this into EventActionArgs After

        protected abstract bool IsValidToHit(Collider2D collision);

        protected void InitializeTargetInfo(Cache<TargetInfo> cache, Hitbox hitbox)
        {
            if (hitbox.damageable.CompareTag(Character.objectTag))
            {
                var character = hitbox.GetComponentInParent<Character>();
                cache.Value.Initialize(hitbox.damageable, character, character.GetComponentInChildren<IFlinch>());
            }
            else
            {
                cache.Value.Initialize(hitbox.damageable, hitbox.GetComponentInParent<BreakableObject>());
            }
        }

        protected void SpawnHitFX(Collider2D collision)
        {
            var hasHitFX = collision.TryGetComponentInParent(out HitFXHandle onHitFX);
            if (m_damageFXHandle != null || hasHitFX)
            {
                ColliderDistance2D colliderDistance = m_triggeredColliderDistance2D;
                if (!colliderDistance.isValid)
                {
                    return;
                }

                Vector2 hitPoint;

                // if its overlapped then this collider's nearest vertex is inside the other collider
                // so the position adjustment shouldn't be necessary
                if (colliderDistance.isOverlapped)
                {
                    hitPoint = colliderDistance.pointA; // point on the surface of this collider
                }
                else
                {
                    // move the hit location inside the collider a bit
                    // this assumes the colliders are basically touching
                    hitPoint = colliderDistance.pointB - (0.01f * colliderDistance.normal);
                }
                var hitDirection = GameplayUtility.GetHorizontalDirection(m_triggeredCollider.bounds.center, hitPoint);

                m_damageFXHandle?.SpawnFX(hitPoint, hitDirection);
                onHitFX?.SpawnFX(hitPoint, hitDirection);
            }
        }

        protected void DealDamage(Collider2D collision, Hitbox hitbox)
        {
            using (Cache<TargetInfo> cacheTargetInfo = Cache<TargetInfo>.Claim())
            {
                InitializeTargetInfo(cacheTargetInfo, hitbox);
                DamageableDetected?.Invoke(cacheTargetInfo.Value, collision);
                m_damageDealer?.Damage(cacheTargetInfo.Value, hitbox.defense);
                cacheTargetInfo?.Release();
            }
        }

        protected virtual void OnValidCollider(Collider2D collision, Hitbox hitbox)
        {
            SpawnHitFX(collision);
            DealDamage(collision, hitbox);
            Debug.Log($"Deal Damage to: {hitbox} via {collision.name}");
        }

        private void InterractWith(Collider2D collision)
        {
            if (collision.TryGetComponentInParent(out IHitToInteract interactable))
            {
                if (interactable.CanBeInteractedWith(m_triggeredCollider))
                {
                    interactable.Interact(GameplayUtility.GetHorizontalDirection(interactable.position, m_damageDealer.position));
                }
            }
        }

        private Collider2D GetTriggeredCollider(Collider2D collider2D)
        {
            if (m_colliders.Length == 1)
            {
                var otherCollider = m_colliders[0];
                m_triggeredColliderDistance2D = otherCollider.Distance(collider2D);
                return otherCollider;
            }
            else
            {
                foreach (var otherCollider in m_colliders)
                {
                    if (otherCollider != null)
                    {
                        var colliderDistance = otherCollider.Distance(collider2D);
                        if (colliderDistance.isOverlapped)
                        {
                            m_triggeredColliderDistance2D = colliderDistance;
                            return otherCollider;
                        }
                    }
                }
                return null;
            }
        }

        protected virtual void Awake()
        {
            m_damageDealer = GetComponentInParent<IDamageDealer>();
            for (int i = 0; i < m_ignoreColliderList.Length; i++)
            {
                m_ignoreColliderList[i].IgnoreColliders(true);
            }
        }

        private void Start()
        {
            m_colliders = GetComponentsInChildren<Collider2D>(true);
        }

        protected virtual void OnTriggerEnter2D(Collider2D collider2D)
        {
            if (collider2D.CompareTag("DamageCollider") || collider2D.CompareTag("Sensor"))
                return;

            var validToHit = IsValidToHit(collider2D);

            if (m_damageUniqueHitboxesOnly)
            {
                var hitbox = m_collisionRegistrator.GetHitbox(collider2D);
                if (hitbox != null && m_collisionRegistrator.HasDamagedHitbox(hitbox) == false)
                {
                    m_triggeredCollider = GetTriggeredCollider(collider2D);
                    if (hitbox.CanBeDamageBy(m_colliders))
                    {
                        if (validToHit)
                        {
                            OnValidCollider(collider2D, hitbox);
                        }
                    }
                    m_collisionRegistrator.RegisterHitboxAs(hitbox, true);
                }
            }
            else
            {
                if (collider2D.TryGetComponentInParent(out Hitbox hitbox))
                {
                    m_triggeredCollider = GetTriggeredCollider(collider2D);
                    if (hitbox.CanBeDamageBy(m_colliders))
                    {
                        if (validToHit)
                        {
                            OnValidCollider(collider2D, hitbox);
                        }
                    }
                }
            }

            if (m_canDetectInteractables && validToHit)
            {
                m_triggeredCollider = GetTriggeredCollider(collider2D);
                InterractWith(collider2D);
            }
        }



        private void OnCollisionEnter2D(Collision2D collision)
        {
            var colliderGameObject = collision.gameObject;
            if (colliderGameObject.CompareTag("DamageCollider") || colliderGameObject.CompareTag("Sensor"))
                return;

            if (colliderGameObject.TryGetComponent(out Hitbox hitbox) && hitbox.invulnerabilityLevel <= m_damageDealer.ignoreInvulnerability)
            {
                m_triggeredCollider = collision.otherCollider;
                using (Cache<TargetInfo> cacheTargetInfo = Cache<TargetInfo>.Claim())
                {
                    InitializeTargetInfo(cacheTargetInfo, hitbox);
                    m_damageDealer?.Damage(cacheTargetInfo.Value, hitbox.defense);
                    cacheTargetInfo?.Release();
                }

                if (colliderGameObject.TryGetComponentInParent(out HitFXHandle onHitFX))
                {
                    if (collision.contactCount > 0)
                    {
                        var hitPoint = collision.GetContact(0).point;
                        onHitFX.SpawnFX(hitPoint, GameplayUtility.GetHorizontalDirection(collision.otherCollider.bounds.center, hitPoint));
                    }
                }

                if (m_canDetectInteractables)
                {
                    InterractWith(collision.collider);
                }
            }
        }
    }
}
