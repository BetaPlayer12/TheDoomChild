using UnityEngine;
using Cinemachine;

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
        private CinemachineBasicMultiChannelPerlin m_perlinNoise;

        public void EnableExtention(bool isEnabled)
        {
            m_perlinNoise.enabled = isEnabled;
        }

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
        }

        protected override void Awake()
        {
            base.Awake();
            m_perlinNoise = ((CinemachineVirtualCamera)VirtualCamera).GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
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
    }
}