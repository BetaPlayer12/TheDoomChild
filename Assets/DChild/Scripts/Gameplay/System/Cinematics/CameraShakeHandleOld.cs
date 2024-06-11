using System.Collections.Generic;
using Cinemachine;
using DChild.Gameplay.Cinematics;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace DChild.Gameplay.Cinematic
{
    [System.Serializable, ShowOdinSerializedPropertiesInInspector]
    public class CameraShakeHandleOld
    {
        [SerializeField]
        private Dictionary<CameraShakeType, NoiseSettings> m_noiseSettings;
        [ShowInInspector, OnValueChanged("EnableCameraShake"), PropertyOrder(-1)]
        private bool m_cameraShake;
        [SerializeField, ShowIf("m_cameraShake"), PropertyOrder(-1)]
        private CameraShakeType m_currentShakeType;
        [ShowInInspector, MinValue(0), ShowIf("m_cameraShake"), PropertyOrder(-1)]
        private float m_shakeAmplitude;
        [ShowInInspector, MinValue(0), ShowIf("m_cameraShake"), PropertyOrder(-1)]
        private float m_shakeFrequency;

        private List<CinemachineBasicMultiChannelPerlin> m_noiseList;
        public CameraShakeType currentShakeType => m_currentShakeType;

        public void Initialize()
        {
            m_noiseList = new List<CinemachineBasicMultiChannelPerlin>();
        }

        public void RegisterNoiseModule(CinemachineBasicMultiChannelPerlin noise)
        {
            m_noiseList.Add(noise);
            if (m_cameraShake)
            {
                noise.m_AmplitudeGain = m_shakeAmplitude;
                noise.m_FrequencyGain = m_shakeFrequency;
            }
            else
            {
                noise.m_AmplitudeGain = 0;
                noise.m_FrequencyGain = 0;
            }
        }

        public void UnregisterNoiseModule(CinemachineBasicMultiChannelPerlin noise)
        {
            m_noiseList.Remove(noise);
        }

        public void EnableCameraShake(bool enable)
        {
            float amplitude = 0;
            float frequency = 0;

            if (enable)
            {
                amplitude = m_shakeAmplitude;
                frequency = m_shakeFrequency;
            }

            for (int i = 0; i < m_noiseList.Count; i++)
            {
                var noise = m_noiseList[i];
                noise.m_AmplitudeGain = amplitude;
                noise.m_FrequencyGain = frequency;
            }
            m_cameraShake = enable;
        }

        public void SetCameraShake(float amplitude, float frequency)
        {
            m_shakeAmplitude = amplitude;
            m_shakeFrequency = frequency;

            if (m_cameraShake)
            {
                for (int i = 0; i < m_noiseList.Count; i++)
                {
                    var noise = m_noiseList[i];
                    noise.m_AmplitudeGain = m_shakeAmplitude;
                    noise.m_FrequencyGain = m_shakeFrequency;
                }
            }
        }

        public void SetCameraShakeProfile(CameraShakeType shakeType)
        {
            m_currentShakeType = shakeType;
            var noiseProfile = m_noiseSettings[shakeType];
            for (int i = 0; i < m_noiseList.Count; i++)
            {
                m_noiseList[i].m_NoiseProfile = noiseProfile;
            }
        }

        public void ClearList()
        {
            m_noiseList?.Clear();
        }
    }
}