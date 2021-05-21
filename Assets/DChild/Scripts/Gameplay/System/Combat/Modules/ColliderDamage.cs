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


        private Collider2D m_collider;
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
            ColliderDistance2D colliderDistance = m_collider.Distance(collision);
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
            var hitDirection = GameplayUtility.GetHorizontalDirection(m_collider.bounds.center, hitPoint);

            m_damageFXHandle?.SpawnFX(hitPoint, hitDirection);
            if (collision.TryGetComponentInParent(out HitFXHandle onHitFX))
            {
                onHitFX.SpawnFX(hitPoint, hitDirection);
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
                if (interactable.CanBeInteractedWith(m_collider))
                {
                    interactable.Interact(GameplayUtility.GetHorizontalDirection(interactable.position, m_damageDealer.position));
                }
            }
        }

        protected virtual void Awake()
        {
            m_collider = GetComponent<Collider2D>();
            m_damageDealer = GetComponentInParent<IDamageDealer>();
            for (int i = 0; i < m_ignoreColliderList.Length; i++)
            {
                m_ignoreColliderList[i].IgnoreColliders(true);
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("DamageCollider") || collision.CompareTag("Sensor"))
                return;

            var validToHit = IsValidToHit(collision);
            if (m_damageUniqueHitboxesOnly)
            {
                var hitbox = m_collisionRegistrator.GetHitbox(collision);
                if (hitbox != null && m_collisionRegistrator.HasDamagedHitbox(hitbox) == false)
                {
                    if (hitbox.CanBeDamageBy(m_collider))
                    {
                        if (validToHit)
                        {
                            OnValidCollider(collision, hitbox);
                        }
                    }
                    m_collisionRegistrator.RegisterHitboxAs(hitbox, true);
                }
            }
            else
            {
                if (collision.TryGetComponentInParent(out Hitbox hitbox))
                {
                    if (hitbox.CanBeDamageBy(m_collider))
                    {
                        if (validToHit)
                        {
                            OnValidCollider(collision, hitbox);
                        }
                    }
                }
            }

            if (m_canDetectInteractables && validToHit)
            {
                InterractWith(collision);
            }
        }



        private void OnCollisionEnter2D(Collision2D collision)
        {
            var colliderGameObject = collision.gameObject;
            if (colliderGameObject.CompareTag("DamageCollider") || colliderGameObject.CompareTag("Sensor"))
                return;

            if (colliderGameObject.TryGetComponent(out Hitbox hitbox) && hitbox.invulnerabilityLevel <= m_damageDealer.ignoreInvulnerability)
            {
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
                        onHitFX.SpawnFX(hitPoint, GameplayUtility.GetHorizontalDirection(m_collider.bounds.center, hitPoint));
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
