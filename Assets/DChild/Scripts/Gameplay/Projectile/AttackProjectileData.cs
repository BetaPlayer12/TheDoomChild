using DChild.Gameplay.Cinematics.Cameras;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    [CreateAssetMenu(fileName = "AttackProjectileData", menuName = "DChild/Gameplay/Attack Projectile Data")]
    public class AttackProjectileData : ProjectileData
    {
        [SerializeField]
        private CameraShakeData m_onCollideShake;
        [SerializeField]
        private bool m_canPassThroughDroppables;
        [SerializeField]
        private bool m_canPassThroughEnvironment;
        [SerializeField]
        private bool m_isPiercing;

        public CameraShakeData onCollideShake => m_onCollideShake;
        public bool canPassThroughDroppables => m_canPassThroughDroppables;
        public bool canPassThroughEnvironment => m_canPassThroughEnvironment;
        public bool isPiercing => m_isPiercing;
    }
}
