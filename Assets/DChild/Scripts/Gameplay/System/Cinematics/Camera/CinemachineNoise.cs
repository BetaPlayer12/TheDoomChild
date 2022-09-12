using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.Cinematics.Cameras
{
    /// <summary>
    /// An add-on module for Cinemachine to shake the camera
    /// </summary>
    [ExecuteInEditMode]
    [SaveDuringPlay]
    [AddComponentMenu("")] // Hide in menu
    public class CinemachineNoise : CinemachineExtension
    {
#if UNITY_EDITOR
        [SerializeField]
        private NoiseSettings m_profile;
#endif
        private CinemachineBasicMultiChannelPerlin m_perlinNoise;

        [SerializeField, MinValue(0)]
        private float m_amplitudeGain = 1;
        [SerializeField, MinValue(0)]
        private float m_frequencyGain = 1;

        public void EnableExtention(bool isEnabled)
        {
            if (m_perlinNoise != null)
            {
                if (isEnabled)
                {
                    m_perlinNoise.m_AmplitudeGain = m_amplitudeGain;
                    m_perlinNoise.m_FrequencyGain = m_frequencyGain;
                }
                else
                {
                    m_perlinNoise.m_AmplitudeGain = 0;
                    m_perlinNoise.m_FrequencyGain = 0;
                }
            }
        }

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
        }

        protected override void Awake()
        {
            base.Awake();
            var vCam = ((CinemachineVirtualCamera)VirtualCamera);
            m_perlinNoise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            m_amplitudeGain = m_perlinNoise.m_AmplitudeGain;
            m_frequencyGain = m_perlinNoise.m_FrequencyGain;
            EnableExtention(false);
            enabled = false;
        }

        private void OnEnable()
        {
            EnableExtention(true);
        }

        private void OnDisable()
        {
            EnableExtention(false);
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            var vCam = ((CinemachineVirtualCamera)VirtualCamera);
            m_perlinNoise = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (m_perlinNoise == null)
            {
                m_perlinNoise = vCam.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                m_perlinNoise.m_NoiseProfile = m_profile;
                m_perlinNoise.m_AmplitudeGain = m_amplitudeGain;
                m_perlinNoise.m_FrequencyGain = m_frequencyGain;
            }
#endif
        }
    }
}