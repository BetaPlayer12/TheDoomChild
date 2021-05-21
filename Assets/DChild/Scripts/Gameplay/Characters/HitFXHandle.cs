using UnityEngine;

namespace DChild.Gameplay.Characters
{
    public class HitFXHandle : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_fx;

        private FXSpawnHandle<FX> m_spawnHandle;

        public void SetFX(GameObject fx) => m_fx = fx;

        public void SpawnFX(Vector2 position, HorizontalDirection direction)
        {
            if (m_fx != null)
            {
                m_spawnHandle.InstantiateFX(m_fx, position, direction);
            }
        }
    }
}