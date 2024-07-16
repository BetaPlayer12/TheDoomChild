using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using Holysoft.Pooling;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class RoyalDeathGuardHealingSoul : PoolableObject
    {
        [SerializeField]
        private Collider2D m_collider;
        [SerializeField]
        private float m_speed;
        [SerializeField]
        private int m_healAmount;
        [SerializeField]
        private bool m_hasHealed;
        [SerializeField]
        private GameObject m_targetObject;
        [SerializeField]
        private Vector3 m_targetDestination;

        private IEnumerator MoveToTarget(Vector2 target)
        {
            var step = m_speed * Time.deltaTime;
            while(!m_hasHealed)
            {
                transform.position = Vector2.MoveTowards(transform.position, target, step);
                yield return null;
            }
        }

        private void HealTarget(Damageable damageable)
        {
            damageable.Heal(m_healAmount);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Hitbox")
            {
                var targetDamageable = m_targetObject.GetComponent<Damageable>();
                HealTarget(targetDamageable);
                m_hasHealed = true;

                DestroyInstance();
            }
        }

        private void Start()
        {
            m_targetObject = FindObjectOfType<RoyalDeathGuardAI>().gameObject;
            //Set Target for Heal
            var hitbox = m_targetObject.transform.GetComponentInChildren<Hitbox>();
            var hurtbox = hitbox.GetComponentInChildren<CapsuleCollider2D>();
            m_targetDestination = hurtbox.transform.position;
            StartCoroutine(MoveToTarget(m_targetDestination));
        }
    }

}
