using DChild.Gameplay.Characters;
using DChild.Gameplay.Combat;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class CrusherColliderDamage : MonoBehaviour
    {
        private Collider2D m_collider;
        private IDamageDealer m_damageDealer;

        private List<Damageable> m_damageable;

        protected virtual void Awake()
        {
            m_collider = GetComponent<Collider2D>();
            m_damageable = new List<Damageable>();
            m_damageDealer = GetComponentInParent<IDamageDealer>();
        }

        protected void InitializeTargetInfo(Cache<TargetInfo> cache, Damageable damageable)
        {
            if (damageable.CompareTag(Character.objectTag))
            {
                var character = damageable.GetComponent<Character>();
                cache.Value.Initialize(damageable, false, character, character.GetComponentInChildren<IFlinch>());
            }
            else
            {
                cache.Value.Initialize(damageable, false, damageable.GetComponent<BreakableObject>());
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.enabled)
            {
                var colliderGameObject = collision.gameObject;
                if (colliderGameObject.CompareTag("DamageCollider") || colliderGameObject.CompareTag("Sensor"))
                    return;

                if (colliderGameObject.TryGetComponentInParent(out Damageable damageable) && colliderGameObject.TryGetComponentInParent(out Character character))
                {
                    if (m_damageable.Contains(damageable) == false)
                    {
                        Raycaster.SetLayerMask(DChildUtility.GetEnvironmentMask());
                        var hits = Raycaster.Cast(collision.GetContact(0).point, -transform.up, character.height, true, out int hitCount);
                        if (hitCount > 0)
                        {
                            m_damageable.Add(damageable);
                            using (Cache<TargetInfo> cacheTargetInfo = Cache<TargetInfo>.Claim())
                            {
                                InitializeTargetInfo(cacheTargetInfo, damageable);
                                m_damageDealer?.Damage(cacheTargetInfo.Value, new BodyDefense());
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
                        }
                    }
                }
            }
        }


        private void Reset()
        {
            m_damageable.Clear();
        }
    }

}