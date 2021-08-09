using DChild.Gameplay.Combat;

namespace DChild.Gameplay.Projectiles
{
    public class SimpleAttackProjectile : AttackProjectile
    {
        private static FXSpawnHandle<FX> m_spawnHandle;
        private static bool m_fxHandleInstantiated;

        protected override void Awake()
        {
            base.Awake();
            if (m_fxHandleInstantiated == false)
            {
                m_spawnHandle = new FXSpawnHandle<FX>();
                m_fxHandleInstantiated = true;
            }
        }

        protected override void Collide()
        {
            var explosion = m_spawnHandle.InstantiateFX(projectileData.impactFX, transform.position);
            var explosionObject = explosion.gameObject.GetComponent<Attacker>();
            PassProjectileAttacker(explosionObject);    
            explosion.transform.parent = null;
            //PassProjectileAttacker(parentAttacker);
            UnloadProjectile();
            CallImpactedEvent();
        }

        private void PassProjectileAttacker(Attacker damageDealer)
        {
            var parentAttacker = GetComponent<Attacker>();
            if (projectileData.impactFX.TryGetComponent(out Attacker explosionAttacker))
            {
                if (parentAttacker != null)
                {
                    parentAttacker.SetParentAttacker(damageDealer);

                }
                else
                {
                    parentAttacker.SetRootParentAttacker(damageDealer);
                }
            }
            else
            {
                parentAttacker.SetParentAttacker(damageDealer);
            }
           
            

        }


    }
}
