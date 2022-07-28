using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class WaterCurrent : MonoBehaviour
    {
        [SerializeField]
        private float m_flowForce;

        private Collider2D m_registeredCollider;
        private Rigidbody2D m_registeredRigidbody;

        private void FixedUpdate()
        {
            if (m_registeredRigidbody)
                m_registeredRigidbody.position += Vector2.right * m_flowForce * GameplaySystem.time.fixedDeltaTime;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_registeredCollider == null)
            {
                if (collision.CompareTag("Sensor"))
                    return;

                if (collision.TryGetComponentInParent(out Rigidbody2D rigidbody2D))
                {
                    m_registeredCollider = collision;
                    m_registeredRigidbody = rigidbody2D;
                    enabled = true;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (m_registeredCollider == collision)
            {
                m_registeredCollider = null;
                m_registeredRigidbody = null;
                enabled = false;
            }
        }
    }
}