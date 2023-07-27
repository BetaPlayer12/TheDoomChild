using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay
{
    public class ObjectEnvironmentTrigger : MonoBehaviour
    {
        [SerializeField]
        private Transform m_objectTrigger;
        [SerializeField]
        private bool m_oneTimeOnly;
        [SerializeField]
        private UnityEvent m_enterEvents;

        private bool m_wasTriggered;

        private void TriggerEnterEvent()
        {
            m_enterEvents?.Invoke();
            if (m_oneTimeOnly)
            {
                m_wasTriggered = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (m_oneTimeOnly == false || (m_oneTimeOnly && m_wasTriggered == false))
            {
                if (collision.transform == m_objectTrigger || collision.GetComponentInParent<Rigidbody2D>().transform == m_objectTrigger)
                {
                    TriggerEnterEvent();
                }
            }
        }
    }
}