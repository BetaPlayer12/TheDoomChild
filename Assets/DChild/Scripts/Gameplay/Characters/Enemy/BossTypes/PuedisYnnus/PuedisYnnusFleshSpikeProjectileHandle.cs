using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class PuedisYnnusFleshSpikeProjectileHandle : MonoBehaviour
    {
        [SerializeField]
        private ProjectileInfo m_projectile;
        [SerializeField]
        private Transform[] m_spawnPoints;

        private ProjectileLauncher m_launcher;

        public void SpawnProjectiles()
        {
            for (int i = 0; i < m_spawnPoints.Length; i++)
            {
                m_launcher.SetSpawnPoint(m_spawnPoints[i]);
                m_launcher.LaunchProjectile();
            }
        }

        private void Awake()
        {
            m_launcher = new ProjectileLauncher(m_projectile,null);
            
        }
    }
}