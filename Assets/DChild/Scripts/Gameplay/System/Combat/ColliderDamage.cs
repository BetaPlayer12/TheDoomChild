/***************************************************
 * 
 * Attackers should look for this in order to damage an Object
 * 
 ***************************************************/
using Holysoft;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public class ColliderDamage : MonoBehaviour
    {
        private IDamageDealer m_damageDealer;
        private Hitbox m_hitbox;

        private void Awake()
        {
            m_damageDealer = GetComponentInParent<IDamageDealer>();
            m_hitbox = this.GetComponentInParent<Actor>().GetComponentInChildren<Hitbox>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("DamageCollider"))
                return;

            var hitbox = collision.GetComponent<Hitbox>();
            if (hitbox != null && hitbox != m_hitbox)
            {
                m_damageDealer?.Damage(hitbox.damageable, hitbox.defense);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("DamageCollider"))
                return;

            var hitbox = collision.gameObject.GetComponent<Hitbox>();
            if (hitbox != null && hitbox != m_hitbox)
            {
                m_damageDealer.Damage(hitbox.damageable, hitbox.defense);
            }
        }
    }
}
