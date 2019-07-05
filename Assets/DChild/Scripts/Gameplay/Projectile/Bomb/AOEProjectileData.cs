using Sirenix.OdinInspector;
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
}