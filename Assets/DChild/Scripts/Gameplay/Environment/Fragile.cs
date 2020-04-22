using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Fragile : MonoBehaviour
    {
        [SerializeField, MinValue(0)]
        private float m_breakingPoint;
        [SerializeField]
        private bool m_destroyObject;
        [SerializeField]
        private UnityEvent m_onBreak;

        private Rigidbody2D m_rigidbody;

        private void Awake()
        {
            m_rigidbody = GetComponent<Rigidbody2D>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
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
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
