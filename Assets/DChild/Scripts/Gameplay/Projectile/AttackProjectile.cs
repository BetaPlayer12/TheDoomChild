﻿
using DChild.Gameplay.Combat;
using Holysoft.Pooling;
using Sirenix.Utilities;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    public abstract class AttackProjectile : Projectile
    {
        [SerializeField]
        private AttackProjectileData m_data;

        protected bool m_collidedWithEnvironment;
        private static Hitbox m_cacheToDamage;

        protected override ProjectileData projectileData => m_data;

        public override void ResetState()
        {
            base.ResetState();
            m_collidedWithEnvironment = false;
        }

        protected abstract void Collide();

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (LayerMask.LayerToName(collision.gameObject.layer) == "Environment")
            {
                if (m_data.canPassThroughEnvironment == false)
                {
                    m_collidedWithEnvironment = true;
                    Collide();
                }
            }
            else if (collision.CompareTag("Hitbox"))
            {
                if (collision.TryGetComponent(out Hitbox m_cacheToDamage) && m_cacheToDamage.isInvulnerable == false)
                {
                    var damage = m_data.damage;
                    using (Cache<AttackerCombatInfo> cacheInfo = Cache<AttackerCombatInfo>.Claim())
                    {
<<<<<<< HEAD
                        AttackerInfo info = new AttackerInfo(transform.position, 0, 1, damage[i]);
                        var targetInfo = new TargetInfo(m_cacheToDamage.damageable, m_cacheToDamage.defense.damageReduction);
                        var result = GameplaySystem.combatManager.ResolveConflict(info, targetInfo);
                        CallAttackerAttacked(new CombatConclusionEventArgs(info, targetInfo, result));
=======
                        using (Cache<TargetInfo> cacheTargetInfo = Cache<TargetInfo>.Claim())
                        {
                            using (Cache<CombatConclusionEventArgs> cacheEventArgs = Cache<CombatConclusionEventArgs>.Claim())
                            {
                                for (int i = 0; i < damage.Length; i++)
                                {
                                    cacheInfo.Value.Initialize(transform.position, 0, 1, damage[i]);
                                    cacheTargetInfo.Value.Initialize(m_cacheToDamage.damageable, m_cacheToDamage.defense.damageReduction);
                                    using (Cache<AttackInfo> cacheResult = GameplaySystem.combatManager.ResolveConflict(cacheInfo, cacheTargetInfo.Value))
                                    {
                                        cacheEventArgs.Value.Initialize(cacheInfo, cacheTargetInfo.Value, cacheResult);
                                        CallAttackerAttacked(cacheEventArgs.Value);
                                        cacheResult.Release();
                                    }
                                }
                                cacheEventArgs.Release();
                            }
                            cacheTargetInfo?.Release();
                        }
                        cacheInfo.Release();
>>>>>>> 1da651e7110817459d92af99c3db2a4e35b13b23
                    }
                }
                if (m_data.isPiercing == false)
                {
                    m_collidedWithEnvironment = false;
                    Collide();
                }
            }
        }

        protected override void Awake()
        {
            base.Awake();
            GetComponent<Attacker>().SetDamage(projectileData.damage);
        }

        private void OnValidate()
        {
            if (m_data != null)
            {
                var physics = GetComponent<IsolatedPhysics2D>();
                if (physics.simulateGravity != !m_data.hasConstantSpeed)
                {
                    physics.simulateGravity = !m_data.hasConstantSpeed;
                }
            }
        }
    }
}
