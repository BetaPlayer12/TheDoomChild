using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.Characters.Enemies
{
    public class RoyalDeathGuardDeathStenchSegment : MonoBehaviour
    {
        [SerializeField]
        private Collider2D m_collider;
        [SerializeField]
        private float m_colliderDuration;

        [Button]
        public void Execute()
        {
            gameObject.SetActive(true);
        }

        private IEnumerator ColliderLifetime()
        {
            m_collider.enabled = true;
            yield return new WaitForSeconds(m_colliderDuration);
            m_collider.enabled = false;
        }

        private void Awake()
        {
            gameObject.SetActive(false);
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