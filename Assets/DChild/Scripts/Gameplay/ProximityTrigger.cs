using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay
{
    public class ProximityTrigger : MonoBehaviour
    {
        [SerializeField]
        private Collider2D m_referenceTrigger;
        [SerializeField, MinValue(1)]
        private int m_checkEveryXFrame = 1;
        [SerializeField, MinValue(0)]
        private float m_proximityThreshold = 1;
        [SerializeField]
        private UnityEvent m_OnTrigger;

        private List<Transform> m_validTransform;
        private int m_frameCount;

        private bool IsWithinThreshold(List<Transform> transforms)
        {
            var center = m_referenceTrigger.bounds.center;
            for (int i = 0; i < transforms.Count; i++)
            {
                if (Vector2.Distance(transforms[i].position, center) <= m_proximityThreshold)
                {
                    return true;
                }
            }

            return false;
        }

        private void Awake()
        {
            m_validTransform = new List<Transform>();
            enabled = false;
        }

        private void LateUpdate()
        {
            m_frameCount++;
            if (m_frameCount == m_checkEveryXFrame)
            {
                if (IsWithinThreshold(m_validTransform))
                {
                    m_OnTrigger?.Invoke();
                };
                m_frameCount = 0;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(DChildUtility.GetSensorTag()) == false)
            {
                var collisionTransform = collision.transform;
                var center = m_referenceTrigger.bounds.center;
                if (Vector2.Distance(collisionTransform.position, center) <= m_proximityThreshold)
                {
                    m_OnTrigger?.Invoke();
                }
                else
                {
                    m_validTransform.Add(collisionTransform);
                    if (enabled == false && m_validTransform.Count > 0)
                    {
                        enabled = true;
                        m_frameCount = 0;
                    }
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag(DChildUtility.GetSensorTag()) == false)
            {
                m_validTransform.Remove(collision.transform);
                if (enabled == false && m_validTransform.Count == 0)
                {
                    enabled = false;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            var color = Color.yellow;
            color.a = 0.25f;
            Gizmos.color = color;
            var center = m_referenceTrigger.bounds.center;
            Gizmos.DrawSphere(center, m_proximityThreshold);
        }
    }
}