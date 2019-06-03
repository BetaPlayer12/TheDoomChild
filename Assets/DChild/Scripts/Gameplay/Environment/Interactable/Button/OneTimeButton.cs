/**************************************
 * 
 * A Generic Button that calls an event to 
 * those that are concerned only once.
 * After that the button will no longer function
 * 
 **************************************/

using DChild.Gameplay.Environment.Interractables;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    [RequireComponent(typeof(Collider2D))]
    public class OneTimeButton : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private UnityEvent m_events;
        [SerializeField]
        [HideInInspector]
        private Collider2D m_collider;
        private bool m_isPressed;

        public Vector3 position => transform.position;

        public IInteractable Interact(IInteractingAgent agent)
        {
            if (m_isPressed == false)
            {
                m_events?.Invoke();
                m_collider.enabled = false;
                m_isPressed = true;
            }
            return this;

        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < m_events.GetPersistentEventCount(); i++)
            {
                if (m_events.GetPersistentTarget(i) != null)
                {
                    Gizmos.DrawLine(transform.position, ((Component)m_events.GetPersistentTarget(i)).transform.position);
                }
            }
        }

        private void OnValidate()
        {
            m_collider = GetComponent<Collider2D>();
        }
    }
}