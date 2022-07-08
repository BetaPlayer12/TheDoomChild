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
        [SerializeField]
        private UnityEvent m_cubedetected;
        [SerializeField]
        private UnityEvent m_cuberemoved;

       
        void Update()
        {
            m_sensor.Cast();
            RaycastHit2D[] RayGameObject = m_sensor.GetValidHits();
            for (int i = 0; i < RayGameObject.Length; i++)
            {
                if (RayGameObject[i].collider.GetComponentInParent<CelestialCube>() != null)
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
