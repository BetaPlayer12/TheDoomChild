using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Gameplay.Environment
{
    [ExecuteAlways]
    public class CompositeLightIntensityAdjuster : MonoBehaviour
    {
        [SerializeField, Range(0, 100), OnValueChanged("OnIntesityChanged")]
        private float m_intensityPercent = 100f;
        [SerializeField]
        public Light2D[] m_light;
        [SerializeField, HideInInspector]
        private float[] m_originalIntensity;

        private float m_previousIntesityPercent;

        public void SetIntensity(float intensitypercent)
        {
            m_intensityPercent = intensitypercent;
            for (int i = 0; i < m_light.Length; i++)
            {
                float temp = 0;
                float temp2 = 0;
                temp = m_originalIntensity[i];
                temp2 = m_intensityPercent / 100f;
                temp = temp * temp2;
                m_light[i].intensity = temp;
            }

#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                var sceneView = EditorWindow.GetWindow<SceneView>();
                sceneView?.Repaint();
            }
#endif
        }
        private void SaveProperties()
        {
            m_originalIntensity = new float[m_light.Length];
            for (int i = 0; i < m_light.Length; i++)
            {
                m_originalIntensity[i] = m_light[i].intensity;
            }
        }

        private void OnIntesityChanged()
        {
            SetIntensity(m_intensityPercent);
        }

        private void Awake()
        {
            SaveProperties();
            SetIntensity(m_intensityPercent);
            m_previousIntesityPercent = m_intensityPercent;
        }

        private void LateUpdate()
        {
            if (m_previousIntesityPercent != m_intensityPercent)
            {
                m_previousIntesityPercent = m_intensityPercent;
                SetIntensity(m_intensityPercent);
            }
        }


        private bool m_hasCopiedValue;

        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            if (m_hasCopiedValue == false)
            {
                m_hasCopiedValue = true;
                SaveProperties();
                SetIntensity(m_intensityPercent);
            }
#endif
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (m_hasCopiedValue)
            {
                if (Selection.activeGameObject != gameObject)
                {
                    m_hasCopiedValue = false;
                    var intesity = m_intensityPercent;
                    SetIntensity(100);
                    m_intensityPercent = intesity;
                }
            }
#endif
        }
    }
}
