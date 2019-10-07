using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DChild.Gameplay.Cinematics.Cameras;
using DChild.Gameplay.Systems;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Cinematics
{
    public class Cinema : MonoBehaviour, ICinema, IGameplaySystemModule, IGameplayInitializable
    {
        [SerializeField]
        private Camera m_mainCamera;
        private IVirtualCamera m_currentVCam;
        private IVirtualCamera m_defaultCam;
        private List<ITrackingCamera> m_trackingCameras;
        private List<CinemachineNoise> m_noiseList;
        [SerializeField]
        private Transform m_trackingTarget;
#if UNITY_EDITOR
        [ShowInInspector, OnValueChanged("SetShake")]
        private bool m_cameraShake;
#endif

        public Camera mainCamera => m_mainCamera;

        public void SetDefaultCam(IVirtualCamera vCam) => m_defaultCam = vCam;


        public void TransistionTo(IVirtualCamera vCam)
        {
            m_currentVCam?.Deactivate();
            vCam.Activate();
            m_currentVCam = vCam;
        }

        public void TransistionToDefaultCamera()
        {
            m_currentVCam?.Deactivate();
            m_defaultCam?.Activate();
            m_currentVCam = m_defaultCam;
        }

        public void EnableCameraShake(bool enable)
        {
            for (int i = 0; i < m_noiseList.Count; i++)
            {
                m_noiseList[i].EnableExtention(enable);
            }
        }

        public void ClearLists()
        {
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
                m_noiseList.Add(trackingCamera.noiseModule);
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

        public void Initialize()
        {
            m_trackingCameras = new List<ITrackingCamera>();
            m_noiseList = new List<CinemachineNoise>();
        }


#if UNITY_EDITOR
        public void Initialized(Camera mainCamera)
        {
            m_mainCamera = mainCamera;
        }

    





#endif
    }
}