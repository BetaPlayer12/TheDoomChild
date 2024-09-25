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
        [SerializeField]
        private CameraShakeHandle m_cameraShakeHandle;
        [SerializeField]
        public bool m_useCameraPrioritization;

        private bool m_hasTemporaryShakeProfile;
        private CameraShakeType m_temporaryShakeProfile;

        public event Action<Camera> OnMainCameraChange;

        public Camera mainCamera => m_mainCamera;

        public CinemachineBrain currentBrain => m_currentBrain;

        public void TransistionTo(IVirtualCamera vCam)
        {
            if (m_useCameraPrioritization)
            {
                vCam.Activate();
                m_offsetHandle.ApplyOffset(vCam, m_currentLookAhead);
                m_cameraShakeHandle.RegisterCamera(vCam);
            }
            else
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
            }
            m_cameraShakeHandle.SetCamera(m_currentVCam);
#if UNITY_EDITOR
            Debug.Log($"Camera Activated: {vCam.name}");
#endif
            vCam.Activate();
            m_offsetHandle.ApplyOffset(vCam, m_currentLookAhead);
        }

        public void ResolveCamTransistion(IVirtualCamera vCam)
        {
            if (m_useCameraPrioritization)
            {
                vCam.Deactivate();
                m_cameraShakeHandle.UnregisterCamera(vCam);
            }
            else
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
            if (m_hasTemporaryShakeProfile && enable == false)
            {
            }
        }



        public void ExecuteCameraShake(CameraShakeData info)
        {
            if (info == null)
            {
                Debug.LogWarning("WARNING: THERE WAS AN ATTEMPT TO EXECUTE A NULL CAMERA SHAKE");
            }
            else
            {
                m_cameraShakeHandle.Execute(info);
            }
        }



        public void ClearLists()
        {
            m_currentVCam = null;
            m_previousCam = null;
            m_trackingCameras?.Clear();
            m_cameraShakeHandle.ClearRegisteredCameras();
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
            }
        }

        public void Unregister(ITrackingCamera trackingCamera)
        {
            m_trackingCameras.Remove(trackingCamera);
            if (trackingCamera.noiseModule != null)
            {
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
            OnMainCameraChange?.Invoke(m_mainCamera);
            m_currentBrain = m_mainCamera.GetComponent<CinemachineBrain>();
        }

        public void Initialize()
        {
            m_trackingCameras = new List<ITrackingCamera>();
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