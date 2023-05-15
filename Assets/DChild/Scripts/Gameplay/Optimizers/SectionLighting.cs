using DChild.Gameplay.Cinematics;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace DChild.Gameplay.Optimization.Lights
{
    public class SectionLighting : MonoBehaviour
    {
        [SerializeField]
        private Vector2 m_boundOffset;
        [SerializeField]
        private Vector2 m_boundSize;
        [SerializeField, PropertyOrder(100)]
        private Light2D[] m_lights;

        private Bounds m_sectionBounds;
        private bool m_isEnabled;
        private bool m_lightsEnabled;

        public Vector2 boundCenter => (Vector2)transform.position + m_boundOffset;
        public Vector2 boundSize => m_boundSize;

        /// <summary>
        /// This is used For Integrity Checks, No real purpose here
        /// </summary>
        public bool HasMissingLights()
        {
            for (int i = 0; i < m_lights.Length; i++)
            {
                if (m_lights[i] == null)
                    return true;
            }
            return false;
        }

        public bool IsVisible(CameraBounds cameraBounds)
        {
            return m_sectionBounds.Intersects(cameraBounds.value);
        }

        public void DisableAllLigts()
        {
            if (m_isEnabled == false || m_lightsEnabled == false)
                return;

            for (int i = 0; i < m_lights.Length; i++)
            {
                m_lights[i].enabled = false;
            }
            m_lightsEnabled = false;
        }

        public void EnableAllLights()
        {
            if (m_isEnabled == false || m_lightsEnabled)
                return;

            for (int i = 0; i < m_lights.Length; i++)
            {
                m_lights[i].enabled = true;
            }
            m_lightsEnabled = true;
        }

        [Button]
        private void EncapsulateAllLights()
        {
            Bounds bounds = new Bounds(transform.position, Vector3.one);
            for (int i = 0; i < m_lights.Length; i++)
            {
                bounds.Encapsulate(m_lights[i].transform.position);
            }
            bounds.Expand(1f);
            SetBounds(bounds);
        }

        public void SetBounds(Vector2 centerOffset, Vector2 size)
        {
            m_boundOffset = centerOffset;
            m_boundSize = size;
        }

        public void SetBounds(Bounds bounds)
        {
            SetBounds(bounds.center - transform.position, bounds.size);
        }

        private IEnumerator RegistrationRoutine()
        {
            while (SectionLightingManager.instance == null)
                yield return null;

            SectionLightingManager.instance.Register(this);
        }

        private void Awake()
        {
            m_sectionBounds = new Bounds(boundCenter, m_boundSize);
            m_isEnabled = true;
            m_lightsEnabled = m_lights[0].enabled;
        }

        private void OnEnable()
        {
            StopAllCoroutines();
            //Made Specifically for the Editor Only version because OnEnable is sometimes called first that
            //SectionLightingManager's Awake
            StartCoroutine(RegistrationRoutine());
        }

        private void OnDisable()
        {
            SectionLightingManager.instance.Unregister(this);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(255, 255, 0, 0.25f);
            Gizmos.DrawCube(boundCenter, m_boundSize);

            Gizmos.color = new Color(255, 255, 0, 1f);
            Gizmos.DrawWireCube(boundCenter, m_boundSize);
            foreach (var light in m_lights)
            {
                var position = light.transform.position;
                Gizmos.DrawLine(transform.position, position);
                Gizmos.DrawWireSphere(position, 10f);
            }
        }
    }

}