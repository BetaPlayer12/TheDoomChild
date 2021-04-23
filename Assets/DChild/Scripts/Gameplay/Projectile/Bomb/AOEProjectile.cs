
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using Holysoft.Event;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    public abstract class AOEProjectile : Projectile
    {
        [SerializeField]
        private AOEProjectileData m_data;

        protected override ProjectileData projectileData => m_data;

        public override void ResetState()
        {
            base.ResetState();
            gameObject.SetActive(true);
        }
        public override void ForceCollision()
        {
            Detonate(transform.position);
        }

        public virtual void Detonate(Vector2 position)
        {
            var explosion = (AOEExplosion)GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_data.impactFX);
            explosion.transform.parent = null;
            explosion.SpawnAt(position, Quaternion.identity);
            explosion.Detonate();
            gameObject.SetActive(false);
            UnloadProjectile();
            CallImpactedEvent();
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