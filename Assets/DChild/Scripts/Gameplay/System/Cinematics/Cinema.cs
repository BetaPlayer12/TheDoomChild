using System;
using System.Collections.Generic;
using Cinemachine;
using DChild.Gameplay.Cinematics.Cameras;
using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace DChild.Gameplay.Cinematics
{
    public class Cinema : SerializedMonoBehaviour, ICinema, IGameplaySystemModule, IGameplayInitializable
    {
        [SerializeField]
        private Camera m_mainCamera;
        [ShowInInspectorAttribute, ReadOnly]
        private IVirtualCamera m_currentVCam;
        [ShowInInspectorAttribute, ReadOnly]
        private IVirtualCamera m_previousCam;
        private List<ITrackingCamera> m_trackingCameras;
        [SerializeField]
        private Transform m_trackingTarget;

        private CinemachineBrain m_currentBrain;
        private CameraPeekHandle m_offsetHandle;
        private CameraPeekMode m_currentLookAhead;

        private bool m_leavePreviousCamAsNull;
        [OdinSerialize]
        private CameraShakeHandle m_cameraShakeHandle;
        private bool m_hasTemporaryShakeProfile;
        private CameraShakeType m_temporaryShakeProfile;

        public Camera mainCamera => m_mainCamera;

        public CinemachineBrain currentBrain => m_currentBrain;

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
            if (m_offsetHandle != null)
            {
                m_currentLookAhead = look;
                m_offsetHandle.ApplyOffset(m_currentVCam, m_currentLookAhead);
            }
        }

        public void EnableCameraShake(bool enable)
        {
            m_cameraShakeHandle.EnableCameraShake(enable);
            if (m_hasTemporaryShakeProfile && enable == false)
            {
                SetCameraShakeProfile(m_temporaryShakeProfile);
            }
        }

        public void SetCameraShake(float amplitude, float frequency)
        {
            m_cameraShakeHandle.SetCameraShake(amplitude, frequency);
        }

        public void SetCameraShakeProfile(CameraShakeType shakeType, bool onNextShakeOnly = false)
        {
            if (onNextShakeOnly)
            {
                m_temporaryShakeProfile = m_cameraShakeHandle.currentShakeType;
                m_hasTemporaryShakeProfile = true;
            }
            else
            {
                m_hasTemporaryShakeProfile = false;
            }
            m_cameraShakeHandle.SetCameraShakeProfile(shakeType);
        }

        public void ClearLists()
        {
            m_currentVCam = null;
            m_previousCam = null;
            m_trackingCameras?.Clear();
            m_cameraShakeHandle.ClearList();
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
                m_cameraShakeHandle.RegisterNoiseModule(trackingCamera.noiseModule);
            }
        }

        public void Unregister(ITrackingCamera trackingCamera)
        {
            m_trackingCameras.Remove(trackingCamera);
            if (trackingCamera.noiseModule != null)
            {
                m_cameraShakeHandle.UnregisterNoiseModule(trackingCamera.noiseModule);
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
            m_currentBrain = m_mainCamera.GetComponent<CinemachineBrain>();
        }

        public void Initialize()
        {
            m_trackingCameras = new List<ITrackingCamera>();
            m_cameraShakeHandle.Initialize();
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