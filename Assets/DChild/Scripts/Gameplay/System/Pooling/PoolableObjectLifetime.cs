using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Pooling
{
    public class PoolableObjectLifetime : MonoBehaviour
    {
        [SerializeField]
        private float m_lifetime;

        private PoolableObject m_source;
        private float m_timer;

        private void Awake()
        {
            m_source = GetComponent<PoolableObject>();
        }

        private void OnEnable()
        {
            m_timer = 0;
        }

        private void Update()
        {
            m_timer += GameplaySystem.time.deltaTime;
            if(m_timer >= m_lifetime)
            {
                m_source.CallPoolRequest();
            }
        }
    }
}