using UnityEngine;

namespace DChild.Gameplay.Characters
{
    public class DamageFXHandle : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_fx;

        private FXSpawnHandle<FX> m_spawnHandle;

        public void SetFX(GameObject fx) => m_fx = fx;

        public void SpawnFX(Vector2 position, HorizontalDirection direction)
        {
            m_spawnHandle.InstantiateFX(m_fx, position, direction);
        }
    }
}