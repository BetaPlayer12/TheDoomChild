
using DChild.Gameplay.Combat;
using System;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    [Flags]
    public enum DamageSource
    {
        DoBoth = (1 << 0),
        ImpactFx = (1 << 1),
        Projectile = (1 << 2),
    }
    public class SimpleAttackProjectile : AttackProjectile
    {
        [SerializeField]
        private DamageSource m_damageSource;
        private static FXSpawnHandle<FX> m_spawnHandle;
        private static bool m_fxHandleInstantiated;

        [SerializeField]
        AttackDamageInfo m_attackDamageInfo;

        [SerializeField]
        AttackDamageInfo m_projectileFXDamage;

        protected override void Collide()
        {
            base.Collide();
            var projectileAttacker = GetComponent<Attacker>();
            var explosion = m_spawnHandle.InstantiateFX(projectileData.impactFX, transform.position);
            var explosionAttacker = explosion.gameObject.GetComponent<Attacker>();
            PassProjectileAttacker(projectileAttacker);
            explosion.transform.parent = null;
            SetImpactFxInfo(explosionAttacker);
            UnloadProjectile();
            CallImpactedEvent();

        }

        private void PassProjectileAttacker(Attacker damageDealer)
        {
            var parentAttacker = GetComponent<Attacker>();

            if (projectileData.impactFX.TryGetComponent(out Attacker explosionAttacker))
            {

                if (parentAttacker.rootParentAttacker != null)
                {
                    parentAttacker.SetParentAttacker(damageDealer);
                }
                else
                {
                    parentAttacker.SetRootParentAttacker(damageDealer);
                }
            }
            else if (parentAttacker.parentAttacker == null)
            {
                parentAttacker.SetParentAttacker(damageDealer);
            }
        }

        private void SetImpactFxInfo(Attacker explosionAttacker)
        {
            if (m_damageSource == DamageSource.DoBoth || m_damageSource == DamageSource.ImpactFx)
            {
                explosionAttacker.SetDamage(m_projectileFXDamage.damage);
                explosionAttacker.SetDamageModifier(1);
            }
        }

        private void ProjectileDamageConfigHandle()
        {
            if (m_damageSource == DamageSource.DoBoth)
            {
                if (projectileData.impactFX)
                {
                    var projectileAttacker = GetComponent<Attacker>();
                    var projectileDamage = projectileData.damage;
                    m_attackDamageInfo.damage.value = projectileDamage.value;
                    m_attackDamageInfo.damage.type = projectileDamage.type;
                    projectileAttacker.SetDamageModifier(1);
                    projectileAttacker.SetDamage(m_attackDamageInfo.damage);
                    if (m_attackDamageInfo.criticalDamageInfo.chance != 0)
                    {
                        projectileAttacker.SetCrit(m_attackDamageInfo.criticalDamageInfo);
                    }


                    if (projectileData.impactFX.GetComponent<Attacker>())
                    {
                        m_projectileFXDamage.damage.type = projectileData.fxDamage.type;
                        m_projectileFXDamage.damage.value = projectileData.fxDamage.value;
                    }

                }
            }
            else if (m_damageSource == DamageSource.ImpactFx)
            {
                var impactFxAttacker = projectileData.impactFX.GetComponent<Attacker>();
                if (impactFxAttacker)
                {
                    m_projectileFXDamage.damage.type = projectileData.fxDamage.type;
                    m_projectileFXDamage.damage.value = projectileData.fxDamage.value;

                }

            }
            else if (m_damageSource == DamageSource.Projectile)
            {
                var projectileAttacker = GetComponent<Attacker>();
                var projectileDamage = projectileData.damage;
                m_attackDamageInfo.damage.value = projectileDamage.value;
                m_attackDamageInfo.damage.type = projectileDamage.type;
                //if (m_attackDamageInfo.criticalDamageInfo.chance != 0)
                //{
                //    projectileAttacker.SetCrit(m_attackDamageInfo.criticalDamageInfo);
                //    //projectileAttacker.SetDamageModifier(m_attackDamageInfo.criticalDamageInfo.damageModifier);
                //}
                //else
                //{
                //    projectileAttacker.SetDamageModifier(1);
                //}

                projectileAttacker.SetDamageModifier(1);

                projectileAttacker.SetDamage(m_attackDamageInfo.damage);

            }
        }

        protected override void Awake()
        {
            base.Awake();
            if (m_fxHandleInstantiated == false)
            {
                m_spawnHandle = new FXSpawnHandle<FX>();
                m_fxHandleInstantiated = true;
            }
        }

        private void Start()
        {
            ProjectileDamageConfigHandle();
        }


    }
}

