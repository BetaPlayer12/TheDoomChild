using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    [CreateAssetMenu(fileName = "AttackProjectileData", menuName = "DChild/Gameplay/Attack Projectile Data")]
    public class AttackProjectileData : ProjectileData
    {
        [SerializeField]
        private bool m_canPassThroughEnvironment;
        [SerializeField]
        private bool m_isPiercing;


        public bool canPassThroughEnvironment => m_canPassThroughEnvironment;
        public bool isPiercing => m_isPiercing;
    }
}
