using DChild.Gameplay.Characters;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild.Gameplay
{
    [System.Serializable]
    public class FXSpawner
    {
        [SerializeField]
        private GameObject m_fx;
        [SerializeField]
        private Transform m_spawnPoint;
        private FXSpawnHandle<FX> m_spawnHandle;

        public void SetFX(GameObject fx) => m_fx = fx;
        public void SpawnFX(Scene scene) => m_spawnHandle.InstantiateFX(m_fx, m_spawnPoint.position, scene);
        public void SpawnFX(HorizontalDirection direction) => m_spawnHandle.InstantiateFX(m_fx, m_spawnPoint.position, direction);
        public void SpawnFX() => m_spawnHandle.InstantiateFX(m_fx, m_spawnPoint.position);
    }
}