using Cinemachine;
using Sirenix.OdinInspector;
using System;
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
        [ReadOnly]
        private CinemachineVirtualCamera m_vCam;
        private CinemachineBasicMultiChannelPerlin m_noiseModule;
        private CinemachineCameraOffset m_offsetHandle;
        private Vector2 m_cameraPosition;

        public Vector3 currentOffset => m_offsetHandle?.m_Offset ?? Vector3.zero;
        public CinemachineBasicMultiChannelPerlin noiseModule => m_noiseModule;


        public void Track(Transform target) => m_vCam.m_Follow = target;

        public void Activate()
        {
            if (m_vCam != null)
            {
                m_vCam.enabled = true;
            }
        }

        public void Deactivate()
        {
            if (m_vCam != null)
            {
                m_vCam.enabled = false;
            }
        }

        public void ApplyOffset(Vector3 offset)
        {
            if (m_offsetHandle)
            {
                m_offsetHandle.m_Offset = offset;
            }
        }

        private void GetCameraStartingPosition()
        {
            m_cameraPosition = transform.position;
            Debug.Log(m_cameraPosition);
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
            if (Application.isPlaying)
            {
                m_vCam.enabled = false;
            }
        }

        private void Awake()
        {
            m_noiseModule = m_vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            m_offsetHandle = m_vCam.GetComponent<CinemachineCameraOffset>();
            m_vCam.enabled = false;
        }

#if UNITY_EDITOR
        [Button, HideInEditorMode]
        private void UseThis()
        {
            GameplaySystem.cinema.TransistionTo(this);
            GetCameraStartingPosition();
        }
#endif
    }

}