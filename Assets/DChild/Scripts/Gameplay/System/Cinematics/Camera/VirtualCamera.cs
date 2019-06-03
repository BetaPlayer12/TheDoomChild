using Cinemachine;
using UnityEngine;

namespace DChild.Gameplay.Cinematics.Cameras
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class VirtualCamera : MonoBehaviour, IVirtualCamera, ITrackingCamera
    {
        [SerializeField]
        private bool m_trackPlayer = true;
        [SerializeField]
        private bool m_isAlreadyTracking;
        [SerializeField]
        [HideInInspector]
        private CinemachineVirtualCamera m_vCam;

        public void Track(Transform target) => m_vCam.m_Follow = target;

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            if (m_isAlreadyTracking == false && m_trackPlayer)
            {
                GameplaySystem.cinema.Register(this);
                m_isAlreadyTracking = true;
            }
        }

        private void OnValidate()
        {
            m_vCam = GetComponent<CinemachineVirtualCamera>();
        }
    }

}