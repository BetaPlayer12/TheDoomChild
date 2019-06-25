/***************************************************
 * 
 * Attackers should look for this in order to damage an Object
 * 
 ***************************************************/
using Refactor.DChild.Gameplay;
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
                    m_cacheResults = GameplaySystem.combatManager.GetValidTargets(transform.position, m_processingHitbox);
                    for (int i = 0; i < m_cacheResults.Count; i++)
                    {
                        m_cacheHitbox = m_cacheResults[i];
                        m_damageDealer.Damage(CreateInfo(m_cacheHitbox), m_cacheHitbox.defense);
                    }
                }
                else if (m_processingHitbox.Count == 1)
                {
                    m_cacheHitbox = m_processingHitbox[0];
                    m_damageDealer.Damage(CreateInfo(m_cacheHitbox), m_cacheHitbox.defense);
                }

                m_processingHitbox.Clear();
                m_cacheHitbox = null;
                m_processTargets = false;
            }
        }

        protected TargetInfo CreateInfo(Hitbox hitbox)
        {
            if (hitbox.damageable.CompareTag(Character.objectTag))
            {
                var character = hitbox.GetComponentInParent<Character>();
                return new TargetInfo(hitbox.damageable, character, character.GetComponentInChildren<IFlinch>());
            }
            else
            {
                return new TargetInfo(hitbox.damageable);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("DamageCollider"))
                return;

            var hitbox = collision.GetComponent<Hitbox>();
            if (hitbox)
            {
                var targetID = hitbox.GetInstanceID();
                m_processingHitbox.Add(hitbox);
                m_processTargets = true;
            }
        }
    }
}
