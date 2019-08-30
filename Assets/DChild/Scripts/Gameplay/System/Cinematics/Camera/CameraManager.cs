using Holysoft;
using Cinemachine;
using DChild.Gameplay.Cinematics.Cameras;
using DChild.Gameplay.Systems;
using UnityEngine;

namespace DChild.Gameplay.Cinematics
{
    public class CameraManager : MonoBehaviour, IGameplaySystemModule, IGameplayActivatable
    {
        [SerializeField]
        private Camera m_mainCamera;

        private CinemachineBrain m_brain;
        private VirtualCamera m_currentVCam;
        private VirtualCamera m_defaultCam;

        public Camera mainCamera => m_mainCamera;

        public void SetDefaultCam(VirtualCamera vCam) => m_defaultCam = vCam;

        public void AllowTrackingTo(VirtualCamera vCam, Transform model) => vCam.Track(model);

        public void SetBlend(CinemachineBlendDefinition.Style style, float duration)
        {
            m_brain.m_DefaultBlend.m_Style = style;
            m_brain.m_DefaultBlend.m_Time = duration;
        }

        public void TransistionTo(VirtualCamera vCam)
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

        public void Disable()
        {
            m_brain = null;
        }

        public void Enable()
        {
            if (m_mainCamera != null)
            {
                m_brain = m_mainCamera.GetComponent<CinemachineBrain>();
            }
        }
    }
}
