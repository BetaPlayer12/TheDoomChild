using Cinemachine;
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
            [SerializeField, ShowIf("m_useCurve")]
            private AnimationCurve m_curve;
            [SerializeField, HideIf("m_useCurve")]
            private float m_value;
            [SerializeField, HideIf("m_useCurve")]
            private bool m_useCurveModifier;
            [SerializeField, ShowIf("@!m_useCurve && m_useCurveModifier")]
            private AnimationCurve m_curveModifier;

            public float GetValue(float time)
            {
                if (m_useCurve)
                {
                    return m_curve.Evaluate(time);
                }
                else
                {
                    if (m_useCurveModifier)
                    {
                        return m_value * m_curveModifier.Evaluate(time);
                    }
                    else
                    {
                        return m_value;
                    }
                }
            }
        }

        [SerializeField,Min(0)]
        private int m_priority;
        [SerializeField, HideLabel]
        private NoiseSettings m_noiseProfile;

        [SerializeField, MinValue(0f)]
        private float m_duration;
        [TitleGroup("Amplitude")]
        [SerializeField, HideLabel]
        private Property m_amplitudeProperty;
        [TitleGroup("Frequency")]
        [SerializeField, HideLabel]
        private Property m_frequencyProperty;


        public int priority => m_priority;
        public NoiseSettings noiseProfile => m_noiseProfile;
        public float duration => m_duration;

        public float GetAmplitude(float time) => m_amplitudeProperty.GetValue(time/m_duration);
        public float GetFrequency(float time) => m_frequencyProperty.GetValue(time/m_duration);
    }
}