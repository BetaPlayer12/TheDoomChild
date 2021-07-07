﻿using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Gameplay.Environment
{
    public class LightIntensityAdjuster : MonoBehaviour
    {
        [SerializeField, Range(0, 100), OnValueChanged("OnIntesityChanged")]
        private int m_intensityPercent;
        [SerializeField]
        public Light2D[] m_light;
        [SerializeField, HideInInspector]
        private float[] m_originalIntensity;

        private int m_previousIntesityPercent;

        public void SetIntensity(int intensitypercent)
        {
            m_intensityPercent = intensitypercent;
            for (int i = 0; i < m_light.Length; i++)
            {
                float temp = 0;
                float temp2 = 0;
                temp = m_originalIntensity[i];
                Debug.Log(temp);
                temp2 = (float)m_intensityPercent / 100;
                Debug.Log(temp2);
                temp = temp * temp2;
                m_light[i].intensity = temp;
            }

#if UNITY_EDITOR
            var sceneView = EditorWindow.GetWindow<SceneView>();
            sceneView?.Repaint();
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
