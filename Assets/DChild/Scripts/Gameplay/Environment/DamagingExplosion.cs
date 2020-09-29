using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class DamagingExplosion : MonoBehaviour
    {
        [SerializeField]
        [BoxGroup("Damage")]
        private AttackDamage m_damage;
        [SerializeField]
        [BoxGroup("Damage")]
        private float m_damageRadius;
        [SerializeField]
        [BoxGroup("Damage")]
        private Invulnerability m_ignoreLevel;

        [SerializeField]
        [BoxGroup("Explosion")]
        private float m_force;
        [SerializeField]
        [BoxGroup("Explosion")]
        private float m_radius;

        private Rigidbody2D m_rigidbody;

        public void Explode()
        {
            DamageAffectObjects();
            m_rigidbody.CastExplosiveForce(m_force, m_radius);
        }

        private void DamageAffectObjects()
        {
            var affectedColliders = Physics2D.OverlapCircleAll(transform.position, m_damageRadius, gameObject.layer);
            if (affectedColliders.Length > 0)
            {
                using (Cache<AttackerCombatInfo> info = Cache<AttackerCombatInfo>.Claim())
                {
                    using (Cache<TargetInfo> targetInfo = Cache<TargetInfo>.Claim())
                    {
                        for (int i = 0; i < affectedColliders.Length; i++)
                        {
                            if (affectedColliders[i].CompareTag("DamageCollider") == false)
                            {
                                var hitbox = affectedColliders[i].GetComponentInParent<Hitbox>();
                                var bodyDefense = hitbox.defense;
                                if (bodyDefense.invulnerabilityLevel <= m_ignoreLevel)
                                {
                                    info.Value.Initialize(transform.position, 0, 1, m_damage);
                                    targetInfo.Value.Initialize(hitbox.damageable, bodyDefense.damageReduction);
                                    using (Cache<AttackInfo> cacheAttackInfo = GameplaySystem.combatManager.ResolveConflict(info, targetInfo.Value))
                                    {
                                        cacheAttackInfo.Release();
                                    }
                                }
                            }
                            targetInfo.Release();
                        }
                        info.Release();
                    }
                }
            }
        }

        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
        }

#if UNITY_EDITOR
        public float damageRadius => m_damageRadius;
        public float radius => m_radius;

#endif
    }
}