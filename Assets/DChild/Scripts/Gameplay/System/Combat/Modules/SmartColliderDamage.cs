/***************************************************
 * 
 * Attackers should look for this in order to damage an Object
 * 
 ***************************************************/
using DChild.Gameplay;
using DChild.Gameplay.Environment;
using Sirenix.Serialization.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public class SmartColliderDamage : MonoBehaviour
    {
        private IDamageDealer m_damageDealer;
        private List<Hitbox> m_processingHitbox;
        private bool m_processTargets;

        private static List<Hitbox> m_cacheResults;
        private static Hitbox m_cacheHitbox;

        private void Awake()
        {
            m_processingHitbox = new List<Hitbox>();
        }

        private void Update()
        {
            if (m_processTargets)
            {
                if (m_processingHitbox.Count > 1)
                {
                    using (Cache<TargetInfo> cacheTargetInfo = Cache<TargetInfo>.Claim())
                    {
                        m_cacheResults = GameplaySystem.combatManager.GetValidTargets(transform.position, m_damageDealer.ignoreInvulnerability, m_processingHitbox);
                        for (int i = 0; i < m_cacheResults.Count; i++)
                        {
                            m_cacheHitbox = m_cacheResults[i];
                            InitializeTargetInfo(cacheTargetInfo, m_cacheHitbox);
                            m_damageDealer.Damage(cacheTargetInfo.Value, m_cacheHitbox.defense);
                        }
                        cacheTargetInfo.Release();
                    }
                }
                else if (m_processingHitbox.Count == 1)
                {
                    using (Cache<TargetInfo> cacheTargetInfo = Cache<TargetInfo>.Claim())
                    {
                        InitializeTargetInfo(cacheTargetInfo, m_cacheHitbox);
                        m_damageDealer.Damage(cacheTargetInfo.Value, m_cacheHitbox.defense);
                        cacheTargetInfo.Release();
                    }

                    m_processingHitbox.Clear();
                    m_cacheHitbox = null;
                    m_processTargets = false;
                }
            }
        }

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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("DamageCollider"))
                return;

            if (collision.TryGetComponent(out Hitbox hitbox) && hitbox.invulnerabilityLevel <= m_damageDealer.ignoreInvulnerability)
            {
                var targetID = hitbox.GetInstanceID();
                m_processingHitbox.Add(hitbox);
                m_processTargets = true;
            }
        }
    }
}
