/***************************************************
 * 
 * Attackers should look for this in order to damage an Object
 * 
 ***************************************************/
using DChild.Gameplay.Characters.Players;
using DChild.Gameplay.Environment.Interractables;
using Holysoft;
using DChild.Gameplay;
using DChild.Gameplay.Combat;
using System.Collections.Generic;
using UnityEngine;
using System;
using DChild.Gameplay.Environment;
using Sirenix.Utilities;
using DChild.Gameplay.Characters;

namespace DChild.Gameplay.Combat
{
    [AddComponentMenu("DChild/Gameplay/Combat/Collider Damage")]
    public class ColliderDamage : MonoBehaviour
    {
        [System.Serializable]
        public class Collider2DInfo
        {
            [SerializeField]
            private Collider2D m_target;
            [SerializeField]
            private Collider2D[] m_ignoreList;

            public void IgnoreColliders(bool value)
            {
                for (int i = 0; i < m_ignoreList.Length; i++)
                {
                    Physics2D.IgnoreCollision(m_target, m_ignoreList[i], value);
                }
            }
        }

        [SerializeField]
        private bool m_canDetectInteractables;
        [SerializeField]
        private Collider2DInfo[] m_ignoreColliderList;

        private Collider2D m_collider;
        private IDamageDealer m_damageDealer;
        public event Action<Collider2D> DamageableDetected; //Turn this into EventActionArgs After

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

        private void Awake()
        {
            m_collider = GetComponent<Collider2D>();
            m_damageDealer = GetComponentInParent<IDamageDealer>();
            for (int i = 0; i < m_ignoreColliderList.Length; i++)
            {
                m_ignoreColliderList[i].IgnoreColliders(true);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
		//Debug.Log("DAMAGED");
            if (collision.CompareTag("DamageCollider"))
                return;

            if (collision.TryGetComponent(out Hitbox hitbox))
            {
                using (Cache<TargetInfo> cacheTargetInfo = Cache<TargetInfo>.Claim())
                {
                    InitializeTargetInfo(cacheTargetInfo, hitbox);
                    m_damageDealer?.Damage(cacheTargetInfo.Value, hitbox.defense);
                    DamageableDetected?.Invoke(collision);
                    cacheTargetInfo?.Release();
                }
                if (collision.TryGetComponentInParent(out HitFXHandle onHitFX))
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

                    onHitFX.SpawnFX(hitPoint, GameplayUtility.GetHorizontalDirection(m_collider.bounds.center, hitPoint));
                }
            }

            if (m_canDetectInteractables)
            {
                if(collision.TryGetComponentInParent(out IHitToInteract interactable))
                {
                    interactable.Interact(GameplayUtility.GetHorizontalDirection(interactable.position,m_damageDealer.position));

                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var colliderGameObject = collision.gameObject;
            if (colliderGameObject.CompareTag("DamageCollider"))
                return;

            if (colliderGameObject.TryGetComponent(out Hitbox hitbox) && hitbox.isInvulnerable == false)
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
                    collision.gameObject.GetComponentInParent<IInteractable>()?.Interact();
                }
            }
        }
    }
}
