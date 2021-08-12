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
            var projectileAttacker = GetComponent<Attacker>();
            var explosion = m_spawnHandle.InstantiateFX(projectileData.impactFX, transform.position);
            //var explosionObject = explosion.gameObject.GetComponent<Attacker>();
            PassProjectileAttacker(projectileAttacker);
            explosion.transform.parent = null;
            //PassProjectileAttacker(parentAttacker);
            UnloadProjectile();
            CallImpactedEvent();
        }

        public void PassProjectileAttacker(Attacker damageDealer)
        {
            var parentAttacker = GetComponent<Attacker>();

            if (projectileData.impactFX.TryGetComponent(out Attacker explosionAttacker))
            {

                if (parentAttacker.rootParentAttacker != null)
                {

                    parentAttacker.SetParentAttacker(damageDealer);
                    //SetRootParentAttacker(damageDealer);

                }
                else
                {

                    parentAttacker.SetRootParentAttacker(damageDealer);
                    //SetParentAttacker(damageDealer);
                }
            }
            else if(parentAttacker.parentAttacker != null)
            {
                
                //SetParentAttacker(damageDealer);
            }
            else
            {
                parentAttacker.SetParentAttacker(damageDealer);
            }
        }





    }


}

