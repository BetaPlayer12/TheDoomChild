using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DChild.Gameplay.Projectiles
{
    [System.Serializable]
    public class AddressableProjectileInfo
    {
        [SerializeField]
        private AssetReference m_projectile;
        [SerializeField, MinValue(0.001f)]
        private float m_speed;

        public AssetReference projectile => m_projectile;
        public float speed => m_speed;
    }
}