
using DChild.Gameplay.Combat;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    public class SimpleAttackProjectile : AttackProjectile
    {
        private static FXSpawnHandle<FX> m_spawnHandle;
        private static bool m_fxHandleInstantiated;

        protected override void Collide()
        {
            var projectileAttacker = GetComponent<Attacker>();
            var explosion = m_spawnHandle.InstantiateFX(projectileData.impactFX, transform.position);
            PassProjectileAttacker(projectileAttacker);
            explosion.transform.parent = null;
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
            else if(parentAttacker.parentAttacker == null)
            {
                parentAttacker.SetParentAttacker(damageDealer);
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
    }
}

