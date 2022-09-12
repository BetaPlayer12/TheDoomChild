using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Gameplay.Environment
{

    [RequireComponent(typeof(Light2D)), ExecuteAlways]
    public class Light2DIntensityAdjuster : BaseLightIntensityAdjuster
    {
        [SerializeField, MinValue(0), ReadOnly, HorizontalGroup("OriginalIntesity")]
        private float m_originalIntesity;
        [SerializeField, Range(0f, 100f), SuffixLabel("%", overlay: true), OnValueChanged("OnIntensityChange")]
        private float m_intensityPercent = 100f;
        private Light2D m_source;

        private float m_previousIntesityPercent;

        public float intensityPercentage
        {
            get => m_intensityPercent;
            set
            {
                SetIntensity(value);
            }
        }

        private float adjustedIntensity => m_originalIntesity * (m_intensityPercent / 100f);

        public override void SetIntensity(float intensitypercent)
        {
            m_intensityPercent = intensitypercent;
            m_source.intensity = adjustedIntensity;
#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                var sceneView = EditorWindow.GetWindow<SceneView>();
                sceneView?.Repaint();
            }
#endif
        }

        private void OnIntensityChange()
        {
            if (m_source == null)
            {
                m_source = GetComponent<Light2D>();
                m_originalIntesity = m_source.intensity;
            }
            intensityPercentage = m_intensityPercent;
        }

        [Button, HorizontalGroup("OriginalIntesity"), HideInPlayMode]
        private void CopyProjectedIntensity()
        {
            var intesity = GetComponent<Light2D>().intensity;
            m_originalIntesity = intesity / (m_intensityPercent / 100f);
        }

        private void Awake()
        {
            m_intensityPercent = 100;
            m_source = GetComponent<Light2D>();
            m_originalIntesity = m_source.intensity;
            m_previousIntesityPercent = m_intensityPercent;
        }

        private void LateUpdate()
        {
            if (m_previousIntesityPercent != m_intensityPercent)
            {
                m_previousIntesityPercent = m_intensityPercent;
                intensityPercentage = m_intensityPercent;
            }
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (Selection.activeGameObject != gameObject)
            {
                m_intensityPercent = 100;
                OnIntensityChange();
            }
#endif
        }

        //On First Validate Get Value of the thing
        //Show the referenced Value afterwards
        //Revert when object is not selected
    }
}
