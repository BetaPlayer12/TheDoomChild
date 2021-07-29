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
        [SerializeField,HorizontalGroup("Light")]
        public Light2D[] m_lights;
        [SerializeField, ReadOnly, HorizontalGroup("Light")]
        private float[] m_originalIntensity;

        private float m_previousIntesityPercent;

        public void SetIntensity(float intensitypercent)
        {
            m_intensityPercent = intensitypercent;
            for (int i = 0; i < m_lights.Length; i++)
            {
                float temp = 0;
                float temp2 = 0;
                temp = m_originalIntensity[i];
                temp2 = m_intensityPercent / 100f;
                temp = temp * temp2;
                m_lights[i].intensity = temp;
            }

            //#if UNITY_EDITOR
            //            if (Application.isPlaying == false)
            //            {
            //                var sceneView = EditorWindow.GetWindow<SceneView>();
            //                sceneView?.Repaint();
            //            }
            //#endif
        }

        [Button("Save Current As 100%"), PropertyOrder(-1)]
        private void SaveProperties()
        {
            m_originalIntensity = new float[m_lights.Length];
            for (int i = 0; i < m_lights.Length; i++)
            {
                m_originalIntensity[i] = m_lights[i].intensity;
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

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (Selection.activeGameObject != gameObject)
            {
                SetIntensity(100);
                m_intensityPercent = 100;
            }
#endif
        }
    }
}
