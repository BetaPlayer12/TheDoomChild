using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    [CreateAssetMenu(fileName = "AOEProjectileData", menuName = "DChild/Gameplay/AOE Projectile Data")]
    public class AOEProjectileData : ProjectileData
    {
        protected override bool ValidateExplosion(GameObject newExplosion)
        {
            var hasAOEExplosion = m_impactFX.GetComponent<ExplosionEffects>() != null;
            if (hasAOEExplosion == false)
            {
                m_impactFX = null;
            }
            return hasAOEExplosion;
        }
    }
}