using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class ElevatorCubeSensor : MonoBehaviour
    {
        public RaySensor m_sensor;
        private bool m_cubefound = false;
        private bool m_isdetecting = false;
        [SerializeField]
        private MovingPlatform m_elevator;
        [SerializeField]
        private UnityEvent m_cubedetected;
        [SerializeField]
        private UnityEvent m_cuberemoved;
        
        private void Start()
        {
            m_elevator.DestinationReached += destination;
            m_elevator.DestinationChanged += change;
        }
        private void change(object sender, MovingPlatform.UpdateEventArgs eventArgs)
        {

            m_isdetecting = true;
        }

        private void destination(object sender, MovingPlatform.UpdateEventArgs eventArgs)
        {
            m_isdetecting = false;
        }

        void Update()
        {
            if (m_isdetecting == true)
            {
                m_sensor.Cast();
                RaycastHit2D[] RayGameObject = m_sensor.GetValidHits();
                for (int i = 0; i < RayGameObject.Length; i++)
                {
                    if (RayGameObject[i].collider.GetComponentInParent<CelestialCube>() != null && RayGameObject[i].collider.GetComponentInParent<MovingPlatform>() == null)
                    {
                        m_cubefound = true;
                        m_cubedetected?.Invoke();
                    }
                    if (m_cubefound == true)
                    {
                        m_cubefound = false;
                        m_cuberemoved?.Invoke();
                    }
                }
            }

        }
    }
}
