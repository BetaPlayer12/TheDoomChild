/***************************************************
 * 
 * Attackers should look for this in order to damage an Object
 * 
 ***************************************************/
using Holysoft;
using Refactor.DChild.Gameplay;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public class ColliderDamage : MonoBehaviour
    {
        private IDamageDealer m_damageDealer;

        private void Awake()
        {
            m_damageDealer = GetComponentInParent<IDamageDealer>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("DamageCollider"))
                return;

            var hitbox = collision.GetComponent<Hitbox>();
            if (hitbox != null)
            {
                m_damageDealer?.Damage(CreateInfo(hitbox), hitbox.defense);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("DamageCollider"))
                return;

            var hitbox = collision.gameObject.GetComponent<Hitbox>();
            if (hitbox != null)
            {
                m_damageDealer.Damage(CreateInfo(hitbox), hitbox.defense);
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
    }
}
