using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class Fragile : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D m_rigidbody;
        [SerializeField, MinValue(0)]
        private float m_breakingPoint;
        [SerializeField]
        private bool m_destroyObject;
        [SerializeField]
        private UnityEvent m_onBreak;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.enabled)
            {
                if (m_rigidbody.velocity.magnitude >= m_breakingPoint)
                {
                    m_onBreak?.Invoke();
                    if (m_destroyObject)
                    {
                        Destroy(gameObject);
                    }
                    else
                    {
                        m_rigidbody.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
