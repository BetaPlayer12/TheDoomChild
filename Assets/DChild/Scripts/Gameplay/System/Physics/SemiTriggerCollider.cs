using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Physics
{
    public class SemiTriggerCollider : MonoBehaviour
    {
        [SerializeField, MinValue(0f)]
        private float m_pushAwayForce;
        [SerializeField]
        private bool m_interactOnceOnEveryEnable;

        private List<Rigidbody2D> m_rigidbodies;
        private Dictionary<Collider2D, Rigidbody2D> m_colliderPair;
        private Dictionary<Rigidbody2D, List<Collider2D>> m_rigidbodyToColliderPair;
        private List<Rigidbody2D> m_interactedRigidBodies;

        private void Awake()
        {
            m_rigidbodies = new List<Rigidbody2D>();
            m_colliderPair = new Dictionary<Collider2D, Rigidbody2D>();
            m_rigidbodyToColliderPair = new Dictionary<Rigidbody2D, List<Collider2D>>();
            m_interactedRigidBodies = new List<Rigidbody2D>();
        }

        private void OnDisable()
        {
            m_interactedRigidBodies.Clear();
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
                if (rigidbody != null && rigidbody.gameObject.CompareTag("Character"))
                {
                    if (m_rigidbodies.Contains(rigidbody) == false)
                    {
                        if (m_interactOnceOnEveryEnable && m_interactedRigidBodies.Contains(rigidbody))
                        {
                            return;
                        }

                        m_rigidbodies.Add(rigidbody);
                        m_interactedRigidBodies.Add(rigidbody);
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
                    var disabledColliders = m_rigidbodyToColliderPair[rigidbody].FindAll(x => x.enabled == false);
                    if(disabledColliders.Count > 0)
                    {
                        var pair = m_rigidbodyToColliderPair[rigidbody];
                        for (int i = 0; i < disabledColliders.Count; i++)
                        {
                            var toRemove = disabledColliders[i];

                            m_colliderPair.Remove(toRemove);
                            pair.Remove(toRemove);
                        }
                    }
                    
                    Debug.Log($"{collision.name}-{rigidbody.name}-{m_rigidbodyToColliderPair[rigidbody].Count}");

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
