using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Cinematics.Cameras
{
    [System.Serializable]
    public class BlendedCameraShake : ICameraShakeHandle
    {
        private class ShakeHandle
        {
            private CameraShakeInfo m_shakeInfo;
            private float m_time;

            public bool isDone => m_time >= (m_shakeInfo?.duration ?? 0f);

            public void SetShakeInfo(CameraShakeInfo info)
            {
                m_shakeInfo = info;
                m_time = 0;
            }

            public void UpdateTime(float delta)
            {
                m_time += delta;
            }

            public float GetCurrentAmplitude() => m_shakeInfo.GetAmplitude(m_time);
            public float GetCurrentFrequency() => m_shakeInfo.GetFrequency(m_time);
        }
        [SerializeField,HideReferenceObjectPicker]
        private AnimationCurve m_blendCurve = new AnimationCurve(new Keyframe(0,1),new Keyframe(1,0));
        [SerializeField, MinValue(0)]
        private float m_duration;

        private ShakeHandle m_currentShake;
        private ShakeHandle m_previousShake;
        private float m_blendTimer;
        private bool m_isBlending;
        private bool m_isShaking;

        public bool isDone => m_currentShake?.isDone ?? true;

        public void SetShakeTo(CameraShakeInfo cameraShakeInfo)
        {
            if (isDone)
            {
                if (m_currentShake == null)
                {
                    m_currentShake = new ShakeHandle();
                    m_previousShake = new ShakeHandle();
                }
                m_currentShake.SetShakeInfo(cameraShakeInfo);
                m_isBlending = false;
            }
            else
            {
                m_previousShake = m_currentShake;
                m_currentShake.SetShakeInfo(cameraShakeInfo);
                m_blendTimer = m_duration;
                m_isBlending = true;
            }
        }

        public void UpdateShake(ICinema cinema, float deltaTime)
        {
            if (m_isBlending)
            {
                m_currentShake.UpdateTime(deltaTime);
                m_previousShake.UpdateTime(deltaTime);
                m_blendTimer += deltaTime;
                var fromWeight = m_blendCurve.Evaluate(m_blendTimer);
                var toWeight = 1f - fromWeight;
                var amplitude = (m_previousShake.GetCurrentAmplitude() * fromWeight) + (m_currentShake.GetCurrentAmplitude() * toWeight);
                var frequency = (m_previousShake.GetCurrentFrequency() * fromWeight) + (m_currentShake.GetCurrentFrequency() * toWeight);
                ApplyShake(cinema, amplitude, frequency);

                if (m_blendTimer >= m_duration)
                {
                    m_isBlending = false;
                }
            }
            else
            {
                m_currentShake.UpdateTime(deltaTime);
                ApplyShake(cinema, m_currentShake.GetCurrentAmplitude(), m_currentShake.GetCurrentFrequency());
            }
        }

        private void ApplyShake(ICinema cinema, float amplitude, float frequency)
        {
            cinema.SetCameraShake(amplitude, frequency);
        }
    }

}