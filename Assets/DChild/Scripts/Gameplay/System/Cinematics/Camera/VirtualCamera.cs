using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Cinematics.Cameras
{
    public class VirtualCamera : MonoBehaviour, IVirtualCamera, ITrackingCamera
    {
#if UNITY_EDITOR
        [SerializeField, OnValueChanged("ChangeName")]
        private string cameraName;

        private void ChangeName()
        {
            if (cameraName != string.Empty)
            {
                gameObject.name = cameraName;
                if (m_vCam == null)
                {
                    m_vCam = GetComponentInChildren<CinemachineVirtualCamera>(true);
                }
                if (m_vCam != null)
                {
                    m_vCam.gameObject.name = cameraName + "VCam";
                }
            }
        }
#endif

        [SerializeField]
        private bool m_trackPlayer = true;
        [SerializeField]
        private bool m_isAlreadyTracking;
        [SerializeField]
        [HideInInspector]
        private CinemachineVirtualCamera m_vCam;
        private CinemachineNoise m_noiseModule;

        public CinemachineNoise noiseModule => m_noiseModule;

        public void Track(Transform target) => m_vCam.m_Follow = target;

        public void Activate()
        {
            m_vCam.enabled = true;
        }

        public void Deactivate()
        {
            m_vCam.enabled = false;
        }

        private void OnEnable()
        {
            GameplaySystem.cinema.Register(this);
            if (m_isAlreadyTracking == false && m_trackPlayer)
            {
                GameplaySystem.cinema.AllowTracking(this);
                m_isAlreadyTracking = true;
            }
        }

        private void OnDisable()
        {
            GameplaySystem.cinema.Unregister(this);
            if (m_isAlreadyTracking)
            {
                GameplaySystem.cinema.RemoveTracking(this);
                m_isAlreadyTracking = false;
            }
        }

        private void OnValidate()
        {
            m_vCam = GetComponentInChildren<CinemachineVirtualCamera>(true);
        }

        private void Awake()
        {
            m_noiseModule = GetComponent<CinemachineNoise>();
            m_vCam.enabled = false;
        }

#if UNITY_EDITOR
        [Button, HideInEditorMode]
        private void UseThis()
        {
            GameplaySystem.cinema.TransistionTo(this);
        }
#endif
    }

}