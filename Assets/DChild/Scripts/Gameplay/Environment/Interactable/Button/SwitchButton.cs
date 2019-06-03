/**************************************
 * 
 * A Generic Button that have 2 states,
 * Events are called to the listeners depending
 * on what state the button is in. Interacting with
 * the button toggles the button between two states
 * 
 **************************************/

using System.Collections;
using DChild.Gameplay.Environment.Interractables;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    [RequireComponent(typeof(Collider2D))]
    public class SwitchButton : MonoBehaviour, IInteractable
    {
        [SerializeField]
        [Tooltip("Delay Between Interactions")]
        [MinValue(0f)]
        private float m_interactionDelay = 0.1f;
        [SerializeField]
        private UnityEvent m_onEvents;
        [SerializeField]
        private UnityEvent m_offEvents;
        [SerializeField]
        [HideInInspector]
        private Collider2D m_collider;
        private bool m_isOn;

        public Vector3 position => transform.position;

        public IInteractable Interact(IInteractingAgent agent)
        {
            if (m_isOn)
            {
                m_offEvents?.Invoke();
                m_isOn = false;
            }
            else
            {
                m_onEvents?.Invoke();
                m_isOn = true;
            }
            StartCoroutine(InteractionDelay());
            return this;
        }

        private IEnumerator InteractionDelay()
        {
            m_collider.enabled = false;
            yield return new WaitForSeconds(m_interactionDelay);
            m_collider.enabled = true;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < m_onEvents.GetPersistentEventCount(); i++)
            {
                if (m_onEvents.GetPersistentTarget(i) != null)
                {
                    Gizmos.DrawLine(transform.position, ((Component)m_onEvents.GetPersistentTarget(i)).transform.position);
                }
            }

            Gizmos.color = Color.red;
            for (int i = 0; i < m_offEvents.GetPersistentEventCount(); i++)
            {
                if (m_offEvents.GetPersistentTarget(i) != null)
                {
                    Gizmos.DrawLine(transform.position, ((Component)m_offEvents.GetPersistentTarget(i)).transform.position);
                }
            }
        }

        private void OnValidate()
        {
            m_collider = GetComponent<Collider2D>();
        }
    }
}
