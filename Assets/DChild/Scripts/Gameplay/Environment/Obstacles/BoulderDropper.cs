using DChild.Gameplay.Pooling;
using DChild.Gameplay.Projectiles;
using Holysoft.Collections;
using Holysoft.Event;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Environment.Obstacles
{
    public class BoulderDropper : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_boulder;
        [SerializeField]
        private Transform m_spawnPoint;
        [SerializeField]
        private PoolableObject m_spawnedBoulder;
        [SerializeField]
        private CountdownTimer m_spawnNewBoulderDelay;
        [SerializeField]
        private Collider2D m_ceilingColliderToIgnore;

        [SerializeField]
        private float m_setupYOffset;

        private bool m_dropOnSpawn;
        private Collider2D m_spawnedBoulderCollider;

        public void Release()
        {
            if (m_spawnedBoulder != null)
            {
                var rigidbody = m_spawnedBoulder.GetComponent<Rigidbody2D>();
                rigidbody.simulated = true;
                rigidbody.velocity = Vector2.zero;

                m_spawnedBoulderCollider.enabled = true;
                m_spawnedBoulder.transform.parent = null;
                m_spawnNewBoulderDelay.Reset();
                m_spawnedBoulder = null;
            }
        }

        public void SetDropOnSpawn(bool value) => m_dropOnSpawn = value;

        private void InitializeBoulder(PoolableObject boulder)
        {
            // boulder.GetComponent<IsolatedPhysics2D>().bodyType = RigidbodyType2D.Kinematic;
            boulder.gameObject.SetActive(true);
            boulder.transform.position = m_spawnPoint.position;
            boulder.transform.parent = m_spawnPoint;
            boulder.transform.localScale = Vector3.one;
            boulder.GetComponent<Rigidbody2D>().simulated = false;
            m_spawnedBoulderCollider = boulder.GetComponentInChildren<Collider2D>();
            Physics2D.IgnoreCollision(m_ceilingColliderToIgnore, m_spawnedBoulderCollider,true);
            m_spawnedBoulderCollider.enabled = false;
        }

        private void SpawnNewBoulder(object sender, EventActionArgs eventArgs)
        {
            m_spawnedBoulder = GameSystem.poolManager.GetPool<PoolableObjectPool>().GetOrCreateItem(m_boulder);
            StartCoroutine(SetupBoulderRoutine());
        }

        private IEnumerator SetupBoulderRoutine()
        {
            InitializeBoulder(m_spawnedBoulder);
            var spawnPosition = m_spawnPoint.position;
            var setupPosition = m_spawnPoint.position + (Vector3.up * m_setupYOffset);
            float lerpValue = 0;

            do
            {
                m_spawnedBoulder.transform.position = Vector3.Lerp(setupPosition, spawnPosition, lerpValue);
                lerpValue += GameplaySystem.time.deltaTime;
                yield return null;
            } while (lerpValue <= 1);

            if (m_dropOnSpawn)
            {
                Release();
            }
        }

        private void Start()
        {
            GameSystem.poolManager.GetPool<PoolableObjectPool>().Register(m_spawnedBoulder);
            InitializeBoulder(m_spawnedBoulder);


            m_spawnNewBoulderDelay.CountdownEnd += SpawnNewBoulder;
            m_spawnNewBoulderDelay.EndTime(false);

            if (m_dropOnSpawn)
            {
                Release();
            }
        }

        private void LateUpdate()
        {
            m_spawnNewBoulderDelay.Tick(GameplaySystem.time.deltaTime);
        }
    }
}
