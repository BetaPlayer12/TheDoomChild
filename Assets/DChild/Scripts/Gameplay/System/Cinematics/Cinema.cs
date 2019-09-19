using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DChild.Gameplay.Cinematics.Cameras;
using DChild.Gameplay.Systems;
using UnityEngine;

namespace DChild.Gameplay.Cinematics
{
    public class Cinema : MonoBehaviour, ICinema, IGameplaySystemModule, IGameplayInitializable
    {
        [SerializeField]
        private Camera m_mainCamera;
        [SerializeField]
        private PlayerCamera m_camera;

        private CinemachineBrain m_brain;
        private IVirtualCamera m_currentVCam;
        private IVirtualCamera m_defaultCam;
        private List<ITrackingCamera> m_trackingCameras;
        [SerializeField]
        private Transform m_trackingTarget;

        public Camera mainCamera => m_mainCamera;
        public new PlayerCamera camera => m_camera;

        public void SetDefaultCam(IVirtualCamera vCam) => m_defaultCam = vCam;

        public void SetBlend(CinemachineBlendDefinition.Style style, float duration)
        {
            m_brain.m_DefaultBlend.m_Style = style;
            m_brain.m_DefaultBlend.m_Time = duration;
        }

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

        public void Register(ITrackingCamera trackingCamera)
        {
            trackingCamera.Track(m_trackingTarget);
            m_trackingCameras.Add(trackingCamera);
        }

        public void Unregister(ITrackingCamera trackingCamera) => m_trackingCameras.Remove(trackingCamera);

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
            m_brain = m_mainCamera?.GetComponent<CinemachineBrain>() ?? null;
        }

        public void Initialize()
        {
            m_trackingCameras = new List<ITrackingCamera>();
        }

        private void Awake()
        {
            if (m_mainCamera)
            {
                m_brain = m_mainCamera?.GetComponent<CinemachineBrain>() ?? null;
            }
        }

#if UNITY_EDITOR
        public void Initialized(Camera mainCamera)
        {
            m_mainCamera = mainCamera;
        }

     
#endif
    }
}