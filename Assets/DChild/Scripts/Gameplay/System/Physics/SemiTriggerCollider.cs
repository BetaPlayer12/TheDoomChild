using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Physics
{
    public class SemiTriggerCollider : MonoBehaviour
    {
        [SerializeField,MinValue(0f)]
        private float m_pushAwayForce;
        private List<Rigidbody2D> m_rigidbodies;

        private Dictionary<Collider2D, Rigidbody2D> m_colliderPair;
        private Dictionary<Rigidbody2D, List<Collider2D>> m_rigidbodyToColliderPair;

        private void Awake()
        {
            m_rigidbodies = new List<Rigidbody2D>();
            m_colliderPair = new Dictionary<Collider2D, Rigidbody2D>();
            m_rigidbodyToColliderPair = new Dictionary<Rigidbody2D, List<Collider2D>>();
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < m_rigidbodies.Count; i++)
            {
                var rigidbody = m_rigidbodies[i];
                var direction = rigidbody.position.x > transform.position.x ? Vector2.right : Vector2.left;
                rigidbody.velocity = direction * m_pushAwayForce;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Sensor") == false)
            {
                var rigidbody = collision.gameObject.GetComponentInParent<Rigidbody2D>();
                if (rigidbody != null)
                {
                    if (m_rigidbodies.Contains(rigidbody) == false)
                    {
                        m_rigidbodies.Add(rigidbody);
                        m_colliderPair.Add(collision, rigidbody);
                        m_rigidbodyToColliderPair.Add(rigidbody, new List<Collider2D>());
                        m_rigidbodyToColliderPair[rigidbody].Add(collision);
                        var direction = rigidbody.position.x > transform.position.x ? Vector2.right : Vector2.left;
                        rigidbody.velocity = direction * m_pushAwayForce;
                    }
                    else
                    {
                        m_colliderPair.Add(collision, rigidbody);
                        m_rigidbodyToColliderPair[rigidbody].Add(collision);
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Sensor") == false)
            {
                if (m_colliderPair.ContainsKey(collision))
                {
                    var rigidbody = m_colliderPair[collision];
                    m_colliderPair.Remove(collision);
                    m_rigidbodyToColliderPair[rigidbody].Remove(collision);
                    if (m_rigidbodyToColliderPair[rigidbody].Count == 0)
                    {
                        m_rigidbodyToColliderPair.Remove(rigidbody);
                        m_rigidbodies.Remove(rigidbody);
                        rigidbody.velocity = Vector2.zero;
                    }
                }
            }
        }
    } 
}
