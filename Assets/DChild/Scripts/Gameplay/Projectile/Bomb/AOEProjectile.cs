﻿
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

        public virtual void Detonate(Vector2 position)
        {
            var explosion = (AOEExplosion)GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_data.explosion);
            explosion.transform.parent = null;
            explosion.SpawnAt(position, Quaternion.identity);
            explosion.Detonate();
            gameObject.SetActive(false);
            UnloadProjectile();
        }
    }
}