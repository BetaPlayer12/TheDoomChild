using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class ElevatorCubeSensor : MonoBehaviour
    {
        private Collider2D m_collider;
        [SerializeField]
        private UnityEvent m_event;
       
        void Start()
        {
            m_collider = GetComponent<Collider2D>();
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            var colliderGameObject = collision.gameObject;
            if (colliderGameObject.GetComponent<CelestialCube>() != null)
            {
                m_event?.Invoke();
            }
        }


        }
}
