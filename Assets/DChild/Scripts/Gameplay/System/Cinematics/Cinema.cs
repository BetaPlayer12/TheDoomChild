using System.Collections.Generic;
using Cinemachine;
using DChild.Gameplay.Cinematics.Cameras;
using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Cinematics
{
    public class Cinema : SerializedMonoBehaviour, ICinema, IGameplaySystemModule, IGameplayInitializable
    {
        [SerializeField]
        private Camera m_mainCamera;
        private IVirtualCamera m_currentVCam;
        private IVirtualCamera m_previousCam;
        private List<ITrackingCamera> m_trackingCameras;
        private List<CinemachineBasicMultiChannelPerlin> m_noiseList;
        [SerializeField]
        private Transform m_trackingTarget;
        private CameraPeekHandle m_offsetHandle;
        private CameraPeekMode m_currentLookAhead;

        private bool m_leavePreviousCamAsNull;
        [ShowInInspector, OnValueChanged("EnableCameraShake")]
        private bool m_cameraShake;
        [SerializeField]
        private Dictionary<CameraShakeType, NoiseSettings> m_noiseSettings;
        [SerializeField, ShowIf("m_cameraShake")]
        private CameraShakeType m_currentShakeType;
        [ShowInInspector, MinValue(0), ShowIf("m_cameraShake")]
        private float m_shakeAmplitude;
        [ShowInInspector, MinValue(0), ShowIf("m_cameraShake")]
        private float m_shakeFrequency;

        public Camera mainCamera => m_mainCamera;

        public void TransistionTo(IVirtualCamera vCam)
        {
            if (m_currentVCam != null)
            {
                m_currentVCam.Deactivate();
                m_offsetHandle.CopyOffset(m_currentVCam, vCam);
                if (m_leavePreviousCamAsNull)
                {
                    m_currentVCam = vCam;
                    m_leavePreviousCamAsNull = false;
                }
                else
                {
                    m_previousCam = m_currentVCam;
                    m_currentVCam = vCam;
                }
            }
            else
            {
                m_currentVCam = vCam;
                m_leavePreviousCamAsNull = false;
            }
#if UNITY_EDITOR
            Debug.Log($"Camera Activated: {vCam.name}");
#endif
            vCam.Activate();
            m_offsetHandle.ApplyOffset(vCam, m_currentLookAhead);
        }

        public void ResolveCamTransistion(IVirtualCamera vCam)
        {
            if (vCam == m_previousCam)
            {
                m_previousCam = null;
            }
            else if (vCam == m_currentVCam)
            {
                if (m_previousCam == null)
                {
                    m_leavePreviousCamAsNull = true;
                }
                else
                {
                    TransistionTo(m_previousCam);
                    m_previousCam = null;
                }
            }
        }

        public void SetCameraPeekConfiguration(CameraPeekConfiguration configuration)
        {
            m_offsetHandle.SetConfiguration(configuration);
            m_offsetHandle.ApplyOffset(m_currentVCam, m_currentLookAhead);
        }

        public void ApplyCameraPeekMode(CameraPeekMode look)
        {
            m_currentLookAhead = look;
            m_offsetHandle.ApplyOffset(m_currentVCam, m_currentLookAhead);
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

        public void ClearLists()
        {
            m_currentVCam = null;
            m_previousCam = null;
            m_trackingCameras?.Clear();
            m_noiseList?.Clear();
        }

        public void AllowTracking(ITrackingCamera trackingCamera)
        {
            trackingCamera.Track(m_trackingTarget);
            m_trackingCameras.Add(trackingCamera);
        }

        public void RemoveTracking(ITrackingCamera trackingCamera)
        {
            m_trackingCameras.Remove(trackingCamera);
        }

        public void Register(ITrackingCamera trackingCamera)
        {
            if (trackingCamera.noiseModule != null)
            {
                var noise = trackingCamera.noiseModule;
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
        }

        public void Unregister(ITrackingCamera trackingCamera)
        {
            m_trackingCameras.Remove(trackingCamera);
            if (trackingCamera.noiseModule != null)
            {
                m_noiseList.Remove(trackingCamera.noiseModule);
            }
        }

        public void SetTrackingTarget(Transform trackingTarget)
        {
            m_trackingTarget = trackingTarget;
            for (int i = 0; i < m_trackingCameras.Count; i++)
            {
                m_trackingCameras[i].Track(m_trackingTarget);
            }
        }

        public void SetMainCamera(Camera camera)
        {
            m_mainCamera = camera;
        }

        public void SetCameraShakeProfile(CameraShakeType shakeType)
        {
            m_currentShakeType = shakeType;
            for (int i = 0; i < m_noiseList.Count; i++)
            {
                m_noiseList[i].m_NoiseProfile = m_noiseSettings[shakeType];
            }
        }

        public void Initialize()
        {
            m_trackingCameras = new List<ITrackingCamera>();
            m_noiseList = new List<CinemachineBasicMultiChannelPerlin>();
            m_offsetHandle = GetComponent<CameraPeekHandle>();
        }


#if UNITY_EDITOR
        public void Initialized(Camera mainCamera)
        {
            m_mainCamera = mainCamera;
        }

        public void Initialize(Transform centerOfMass)
        {
            m_trackingTarget = centerOfMass;
        }
#endif
    }
}