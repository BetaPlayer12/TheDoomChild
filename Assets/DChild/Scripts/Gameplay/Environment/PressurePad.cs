using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class PressurePad : MonoBehaviour
    {
        [SerializeField, MinValue(0.1f)]
        private float m_requiredMass;
        [SerializeField, TabGroup("Succeed")]
        private UnityEvent m_massAcheived;
        [SerializeField, TabGroup("Fail")]
        private UnityEvent m_massFail;

        private float m_currentMass;
        private bool m_hasReachedRequiredMass;

        private void ReactOnChangeOnMass()
        {
            if (m_currentMass >= m_requiredMass)
            {
                if (m_hasReachedRequiredMass == false)
                {
                    m_massAcheived?.Invoke();
                    m_hasReachedRequiredMass = true;
                }
            }
            else
            {
                if (m_hasReachedRequiredMass)
                {
                    m_massFail?.Invoke();
                    m_hasReachedRequiredMass = false;
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponentInParent(out Rigidbody2D rigidbody))
            {
                m_currentMass += rigidbody.mass;
                ReactOnChangeOnMass();
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            m_currentMass -= collision.rigidbody.mass;
            StartCoroutine(ExitTriggerDelay(0.5f));
        }

        private IEnumerator ExitTriggerDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            ReactOnChangeOnMass();
        }
    }
}