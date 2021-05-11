using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class WeightSensor : MonoBehaviour
    {
        [ShowInInspector,ReadOnly]
        private float m_currentWeight;
        private Dictionary<Rigidbody2D, int> m_instanceCountPair;

        public event EventAction<EventActionArgs> MassChange;

        public float currentWeight => m_currentWeight;

        private void Awake()
        {
            m_instanceCountPair = new Dictionary<Rigidbody2D, int>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.enabled)
            {
                var rigidbody = collision.rigidbody;
                if (m_instanceCountPair.ContainsKey(collision.rigidbody))
                {
                    m_instanceCountPair[rigidbody] += 1;
                }
                else
                {
                    m_instanceCountPair.Add(rigidbody, 1);
                    m_currentWeight += rigidbody.mass;
                    MassChange?.Invoke(this, EventActionArgs.Empty);
                }
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.enabled)
            {
                var rigidbody = collision.rigidbody;
                if (m_instanceCountPair.ContainsKey(collision.rigidbody))
                {
                    m_instanceCountPair[rigidbody] -= 1;
                    if (m_instanceCountPair[rigidbody] == 0)
                    {
                        m_instanceCountPair.Remove(rigidbody);
                        m_currentWeight -= rigidbody.mass;
                        MassChange?.Invoke(this, EventActionArgs.Empty);
                    }
                }

            }
        }
    }
}