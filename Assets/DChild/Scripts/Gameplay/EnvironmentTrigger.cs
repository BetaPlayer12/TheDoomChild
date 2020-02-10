using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay
{
    public class EnvironmentTrigger : MonoBehaviour
    {
        [SerializeField, OnValueChanged("OnValueChange")]
        private bool m_oneTimeOnly;
        [SerializeField]
        private UnityEvent m_enterEvents;
        [SerializeField, HideIf("m_oneTimeOnly")]
        private UnityEvent m_exitEvents;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Hitbox"))
            {
                m_enterEvents?.Invoke();
                if (m_oneTimeOnly)
                {
                    enabled = false;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Hitbox"))
            {
                m_exitEvents?.Invoke();
            }
        }

#if UNITY_EDITOR
        private void OnValueChange()
        {
            if (m_oneTimeOnly)
            {
                m_exitEvents.RemoveAllListeners();
            }
        }
#endif
    }
}