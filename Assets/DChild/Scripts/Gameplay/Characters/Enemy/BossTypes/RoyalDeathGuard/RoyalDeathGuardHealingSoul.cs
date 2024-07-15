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
    public class RoyalDeathGuardHealingSoul : MonoBehaviour
    {
        [SerializeField]
        private Collider2D m_collider;
        [SerializeField]
        private int m_healAmount;
        [SerializeField]
        private Transform m_target;
        [SerializeField]
        private float m_speed;

        private bool m_hasHealed;

        public void SetTarget(Transform target)
        {
            m_target = target;
        }

        [Button]
        private IEnumerator MoveTowardsTarget()
        {
            while (!m_hasHealed)
            {
                Vector2.MoveTowards(transform.position, m_target.position, m_speed);
                yield return null;
            }
        }

        private void HealTarget(Damageable damageable, int healAmount)
        {
            damageable.Heal(healAmount);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            var targetDamageable = collision.gameObject.GetComponent<Damageable>();
            HealTarget(targetDamageable, m_healAmount);
            m_hasHealed = true;

        }

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void OnPoolRequest(object sender, PoolItemEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }

        private void OnEnable()
        {
            m_hasHealed = false;
            StartCoroutine(MoveTowardsTarget());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }

}
