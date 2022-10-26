using DChild.Gameplay.Pooling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Enemies
{
    public class MonolithSlam : PoolableObject
    {
        [SerializeField]
        private BoxCollider2D m_impactCollider;
        [SerializeField]
        private BoxCollider2D m_obstacleCollider;
        [SerializeField]
        private RaySensor m_floorSensor;

        private bool m_monolithGrounded;

        public bool destroyMonolith;
        public bool smashMonolith;

        // Start is called before the first frame update
        void Start()
        {
            m_impactCollider.enabled = true;
            m_obstacleCollider.enabled = false;
            m_monolithGrounded = false;
        }

        // Update is called once per frame
        void Update()
        {
            if (smashMonolith)
            {
                StartCoroutine(MonolithSmashOnGround());
                if (!m_monolithGrounded)
                    StartCoroutine(MonolithSmashOnGround());
            }        
        }

        private IEnumerator MonolithSmashOnGround()
        {
            //insert picking up monolith animation here

            if (!m_floorSensor.isDetecting)
            {
                transform.Translate(Vector3.down);
            }
            else
            {
                yield return new WaitForSeconds(2f);

                m_impactCollider.enabled = false;
                m_obstacleCollider.enabled = true;
                m_monolithGrounded = true;

                if (destroyMonolith)
                    DestroyInstance();
            }
        }
    }
}

