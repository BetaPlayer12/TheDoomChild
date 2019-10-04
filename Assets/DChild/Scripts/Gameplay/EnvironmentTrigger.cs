using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay
{
    public class EnvironmentTrigger : MonoBehaviour
    {
        [SerializeField]
        private bool m_oneTimeOnly;
        [SerializeField]
        private UnityEvent m_events;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Hitbox"))
            {
                m_events?.Invoke();
                if (m_oneTimeOnly)
                {
                    enabled = false;
                }
            }
        }
    }
}