using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class IceShardGroundHandler : MonoBehaviour
    {
        public RaySensor m_sensor;
        [SerializeField]
        private UnityEvent m_grounddetected;
        private bool m_platform = false;
        


        void Update()
        {
            if (m_platform == false)
            {
                m_sensor.Cast();
                RaycastHit2D[] RayGameObject = m_sensor.GetValidHits();
                for (int i = 0; i < RayGameObject.Length; i++)
                {
                    if (RayGameObject[i].collider.gameObject.layer == LayerMask.NameToLayer("Environment")|| RayGameObject[i].collider.gameObject.layer == LayerMask.NameToLayer("PassableEnvironment"))
                    {
                        m_platform = true;
                        m_grounddetected?.Invoke();
                    }

                }
            }

        }
    }
}
