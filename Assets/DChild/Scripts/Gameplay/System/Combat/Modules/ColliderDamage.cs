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

namespace DChild.Gameplay.Combat
{
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
                    Physics2D.IgnoreCollision(m_target, m_ignoreList[i],value);
                }
            }
        }

        [SerializeField]
        private bool m_canDetectInteractables;
        [SerializeField]
        private Collider2DInfo[] m_ignoreColliderList;

        private IDamageDealer m_damageDealer;

        private void Awake()
        {
            m_damageDealer = GetComponentInParent<IDamageDealer>();
            for (int i = 0; i < m_ignoreColliderList.Length; i++)
            {
                m_ignoreColliderList[i].IgnoreColliders(true);
            }
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

            if (m_canDetectInteractables)
            {
                collision.GetComponentInParent<IInteractable>()?.Interact();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("DamageCollider"))
                return;

            var hitbox = collision.gameObject.GetComponent<Hitbox>();
            if (hitbox != null && hitbox.isInvulnerable == false)
            {
                m_damageDealer.Damage(CreateInfo(hitbox), hitbox.defense);
            }

            if (m_canDetectInteractables)
            {
                collision.gameObject.GetComponentInParent<IInteractable>()?.Interact();
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
