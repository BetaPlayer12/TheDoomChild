using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{

    public class HeavyFalls : MonoBehaviour
    {
        [SerializeField]
        private float m_downwardForce;

        private Dictionary<Rigidbody2D, int> m_rigidbodyReferenceCount;
        private List<Rigidbody2D> m_rigidbodyList;

        private void Awake()
        {
            m_rigidbodyReferenceCount = new Dictionary<Rigidbody2D, int>();
            m_rigidbodyList = new List<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < m_rigidbodyList.Count; i++)
            {
                m_rigidbodyList[i].AddForce(Vector3.down * m_downwardForce);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponentInParent(out Rigidbody2D rigidbody))
            {
                if (m_rigidbodyReferenceCount.ContainsKey(rigidbody))
                {
                    m_rigidbodyReferenceCount[rigidbody] += 1;
                }
                else
                {
                    m_rigidbodyReferenceCount.Add(rigidbody, 1);
                    m_rigidbodyList.Add(rigidbody);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponentInParent(out Rigidbody2D rigidbody))
            {
                if (m_rigidbodyReferenceCount.ContainsKey(rigidbody))
                {
                    m_rigidbodyReferenceCount[rigidbody] -= 1;
                    if (m_rigidbodyReferenceCount[rigidbody] <= 0)
                    {
                        m_rigidbodyReferenceCount.Remove(rigidbody);
                        m_rigidbodyList.Remove(rigidbody);
                    }
                }
            }
        }
    }
}