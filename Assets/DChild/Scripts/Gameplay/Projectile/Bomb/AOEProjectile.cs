
using DChild.Gameplay.Combat;
using DChild.Gameplay.Pooling;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    [CreateAssetMenu(fileName = "AOEProjectileData", menuName = "DChild/Gameplay/AOE Projectile Data")]
    public class AOEProjectileData : ProjectileData
    {
        [SerializeField, ValidateInput("ValidateExplosion"), PreviewField]
        private GameObject m_explosion;

        public GameObject explosion { get => m_explosion; }

#if UNITY_EDITOR
        private bool ValidateExplosion(GameObject newExplosion)
        {
            var hasAOEExplosion = m_explosion.GetComponent<AOEExplosion>() != null;
            if (hasAOEExplosion == false)
            {
                m_explosion = null;
            }
            return hasAOEExplosion;
        }
#endif
    }

    public abstract class AOEProjectile : Projectile
    {
        [SerializeField]
        private AOEProjectileData m_data;

        protected override ProjectileData projectileData => m_data;

        public override void ResetState()
        {
            gameObject.SetActive(true);
        }

        public virtual void Detonate(Vector2 position)
        {
            var explosion = (AOEExplosion)GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_data.explosion);
            explosion.transform.parent = null;
            explosion.SpawnAt(position, Quaternion.identity);
            explosion.Detonate();
            gameObject.SetActive(false);
            CallPoolRequest();
        }
    }
}