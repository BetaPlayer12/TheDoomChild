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
            var parentAttacker = GetComponent<Attacker>();
            explosion.transform.parent = null;
            parentAttacker.PassParentAttacker(parentAttacker);
            UnloadProjectile();
            CallImpactedEvent();
        }


    }
}
