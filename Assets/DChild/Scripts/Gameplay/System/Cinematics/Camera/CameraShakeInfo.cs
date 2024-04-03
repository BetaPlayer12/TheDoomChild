using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Cinematics.Cameras
{
    [System.Serializable]
    public class CameraShakeInfo
    {
        [System.Serializable]
        public class Property
        {
            [SerializeField]
            private bool m_useCurve;
            [SerializeField, HideIf("m_useCurve")]
            private float m_value;
            [SerializeField, ShowIf("m_useCurve")]
            private AnimationCurve m_curve;

            public float GetValue(float time) => m_useCurve ? m_curve.Evaluate(time) : m_value;
        }

        [SerializeField, MinValue(0f)]
        private float m_duration;
        [TitleGroup("Amplitude")]
        [SerializeField, HideLabel]
        private Property m_amplitudeProperty;
        [TitleGroup("Frequency")]
        [SerializeField, HideLabel]
        private Property m_frequencyProperty;
        public float duration => m_duration;

        public float GetAmplitude(float time) => m_amplitudeProperty.GetValue(time);
        public float GetFrequency(float time) => m_frequencyProperty.GetValue(time);
    }
}