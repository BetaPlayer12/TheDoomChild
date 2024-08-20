using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;
using Holysoft.Event;
using DChild.Gameplay.Pooling;
using Holysoft.Pooling;
using System;

namespace DChild.Gameplay.Characters.Enemies
{
    public class RoyalDeathGuardDeathStenchSegment : MonoBehaviour
    {
        [SerializeField]
        private Collider2D m_collider;
        [SerializeField]
        private float m_colliderDuration;
        [SerializeField]
        private float m_colliderSpawnDelay;

        public event EventAction<EventActionArgs> Done;


        [Button]
        public void Execute()
        {
            gameObject.SetActive(true);
        }

        private IEnumerator ColliderLifetime()
        {
            yield return new WaitForSeconds(m_colliderSpawnDelay);
            m_collider.enabled = true;
            yield return new WaitForSeconds(m_colliderDuration);
            m_collider.enabled = false;
        }

        private void OnPoolRequest(object sender, PoolItemEventArgs eventArgs)
        {
            Done?.Invoke(this, EventActionArgs.Empty);
        }

        private void Awake()
        {
            gameObject.SetActive(false);
            GetComponent<PoolableObject>().PoolRequest += OnPoolRequest;
        }


        private void OnEnable()
        {
            StartCoroutine(ColliderLifetime());
        }

        private void OnDisable()
        {
            //Particle System Disables GameObject
            StopAllCoroutines();
        }
    }
}