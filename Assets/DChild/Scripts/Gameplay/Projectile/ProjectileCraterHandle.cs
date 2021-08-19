using DChild.Gameplay.Pooling;
using Holysoft.Pooling;
using UnityEngine;

namespace DChild.Gameplay.Projectiles
{
    [System.Serializable]
    public class ProjectileCraterHandle
    {
        private GameObject m_craterPrefab;
        private Rigidbody2D m_rigidbody;
        private Collider2D m_selfCollider;
        private ContactFilter2D m_filter;
        private static RaycastHit2D[] raycastHits = new RaycastHit2D[16];

        public ProjectileCraterHandle(GameObject craterPrefab, Rigidbody2D rigidbody, Collider2D selfCollider)
        {
            m_craterPrefab = craterPrefab;
            m_rigidbody = rigidbody;
            m_selfCollider = selfCollider;
            m_filter = new ContactFilter2D();
            m_filter.useLayerMask = true;
            m_filter.SetLayerMask(LayerMask.GetMask("Environment"));
        }

        public void GenerateCrater()
        {
            var hitCount = m_selfCollider.Cast(m_rigidbody.velocity.normalized, m_filter, raycastHits, 1);
            for (int i = 0; i < raycastHits.Length; i++)
            {
                var hit = raycastHits[i];
                var instance = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_craterPrefab);
                var rotation = Quaternion.Euler(0, 0, Vector2.SignedAngle(Vector2.up, hit.normal));
                instance.SpawnAt(hit.point, rotation);
            }
        }
    }
}
