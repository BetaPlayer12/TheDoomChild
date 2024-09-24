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
using UnityEngine.Profiling;

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
        protected bool m_damageUniqueHitboxesOnly;
        [SerializeField, ShowIf("m_damageUniqueHitboxesOnly")]
        protected CollisionRegistrator m_collisionRegistrator;
        [SerializeField]
        private Collider2DInfo[] m_ignoreColliderList;


        protected Collider2D[] m_colliders;
        private Collider2D m_triggeredCollider;
        protected IDamageDealer m_damageDealer;
        private static RaycastHit2D[] m_blockHitBuffer;
        public event Action<TargetInfo, Collider2D> DamageableDetected; //Turn this into EventActionArgs After

        protected abstract bool IsValidColliderToHit(Collider2D collision);

        protected void InitializeTargetInfo(Cache<TargetInfo> cache, Collider2D collider2D, Hitbox hitbox)
        {
            if (hitbox.damageable.CompareTag(Character.objectTag))
            {
                var character = hitbox.GetComponentInParent<Character>();
                cache.Value.Initialize(hitbox, collider2D, character, character.GetComponentInChildren<IFlinch>());
            }
            else
            {
                cache.Value.Initialize(hitbox, collider2D, hitbox.GetComponentInParent<BreakableObject>());
            }
        }

        protected void DealDamage(Collider2D collision, Hitbox hitbox)
        {
            using (Cache<TargetInfo> cacheTargetInfo = Cache<TargetInfo>.Claim())
            {
                InitializeTargetInfo(cacheTargetInfo, collision, hitbox);
                m_damageDealer?.Damage(cacheTargetInfo.Value, m_triggeredCollider);
                DamageableDetected?.Invoke(cacheTargetInfo.Value, collision);
                cacheTargetInfo?.Release();
            }
        }

        protected bool CanBypassHitboxInvulnerability(Hitbox hitbox) => hitbox.invulnerabilityLevel <= m_damageDealer.ignoreInvulnerability;

        protected bool IsTargetBlocked(Collider2D target)
        {
            var toTarget = target.bounds.center - transform.position;
            Raycaster.SetLayerCollisionMask(gameObject.layer);
            m_blockHitBuffer = Raycaster.CastAll(transform.position, toTarget.normalized, toTarget.magnitude);
            foreach (var hitBuffer in m_blockHitBuffer)
            {
                if (target != hitBuffer.collider && hitBuffer.collider.TryGetComponent(out Hitbox hitbox))
                {
                    if (hitbox.canBlockDamage)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected bool IsBlockPreceededByTarget(Collider2D block, IDamageable damageable)
        {
            var toTarget = block.bounds.center - transform.position;
            Raycaster.SetLayerCollisionMask(gameObject.layer);
            m_blockHitBuffer = Raycaster.CastAll(transform.position, toTarget.normalized, toTarget.magnitude);
            foreach (var hitBuffer in m_blockHitBuffer)
            {
                if (block != hitBuffer.collider && hitBuffer.collider.TryGetComponent(out Hitbox hitbox))
                {
                    if (hitbox.canBlockDamage == false && hitbox.damageable == damageable)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        protected virtual void OnValidCollider(Collider2D collision, Hitbox hitbox)
        {
            DealDamage(collision, hitbox);
//#if UNITY_EDITOR
//            Debug.Log($"Deal Damage to: {hitbox} via {collision.name}", this);
//#endif
        }

        protected virtual void HandleDamageUniqueHitboxes(Collider2D collider2D)
        {
            var hitbox = m_collisionRegistrator.GetHitbox(collider2D);
            if (hitbox != null && m_collisionRegistrator.HasDamagedDamageable(hitbox.damageable) == false)
            {
                if (IsValidHitboxToHit(collider2D, hitbox))
                {
                    OnValidCollider(collider2D, hitbox);
                    m_collisionRegistrator.RegisterHitboxAs(hitbox, true);
                }
            }
        }

        protected virtual bool IsValidHitboxToHit(Collider2D collider2D, Hitbox hitbox)
        {
            if (hitbox.canBlockDamage ? (IsBlockPreceededByTarget(collider2D, hitbox.damageable) == false) : (IsBlockedByOtherBlockingHitboxes(collider2D) == false))
            {
                m_triggeredCollider = GetTriggeredCollider(collider2D);
                if (hitbox.CanBeDamageBy(m_colliders))
                {
                    return true;
                }
            }
            return false;
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

        private bool IsBlockedByOtherBlockingHitboxes(Collider2D collider2D) { return m_damageDealer.ignoresBlock == false && IsTargetBlocked(collider2D); }

        private Collider2D GetTriggeredCollider(Collider2D collider2D)
        {
            if (m_colliders.Length == 1)
            {
                return m_colliders[0];
            }
            else
            {
                var colliderBounds = collider2D.bounds;
                foreach (var otherCollider in m_colliders)
                {
                    if (otherCollider != null)
                    {
                        if (otherCollider.bounds.Intersects(colliderBounds))
                        {
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

#if UNITY_EDITOR
            Profiler.BeginSample("Collider Damage: Validation For Damage", this);
#endif
            if (IsValidColliderToHit(collider2D))
            {
                if (m_damageUniqueHitboxesOnly)
                {
                    HandleDamageUniqueHitboxes(collider2D);
                }
                else
                {
                    if (collider2D.TryGetComponentInParent(out Hitbox hitbox))
                    {
                        if (IsValidHitboxToHit(collider2D, hitbox))
                        {
                            OnValidCollider(collider2D, hitbox);
                        }
                    }
                }

                if (m_canDetectInteractables && IsBlockedByOtherBlockingHitboxes(collider2D) == false)
                {
                    m_triggeredCollider = GetTriggeredCollider(collider2D);
                    InterractWith(collider2D);
                }
            }
#if UNITY_EDITOR
            Profiler.EndSample();
#endif
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var colliderGameObject = collision.gameObject;
            if (colliderGameObject.CompareTag("DamageCollider") || colliderGameObject.CompareTag("Sensor"))
                return;

            if (colliderGameObject.TryGetComponent(out Hitbox hitbox) && CanBypassHitboxInvulnerability(hitbox))
            {
                m_triggeredCollider = collision.otherCollider;
                using (Cache<TargetInfo> cacheTargetInfo = Cache<TargetInfo>.Claim())
                {
                    InitializeTargetInfo(cacheTargetInfo, collision.collider, hitbox);
                    m_damageDealer?.Damage(cacheTargetInfo.Value, collision.otherCollider);
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
