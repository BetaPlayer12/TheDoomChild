using UnityEngine;
using Cinemachine;
using Sirenix.OdinInspector;

namespace DChild.Gameplay.Cinematics.Cameras
{
    /// <summary>
    /// An add-on module for Cinemachine to shake the camera
    /// </summary>
    [ExecuteInEditMode]
    [SaveDuringPlay]
    [AddComponentMenu("")] // Hide in menu
    public class PhysicalCameraExtention : CinemachineExtension
    {
        public bool m_isPhysicalCamera;
        [ShowIf("m_isPhysicalCamera"), Indent]
        public float m_focalLength;
        [ShowIf("m_isPhysicalCamera"), Indent]
        public Vector2 m_sensorSize;
        [ShowIf("m_isPhysicalCamera"), Indent]
        public Vector2 m_lensShift;
        [ShowIf("m_isPhysicalCamera"), Indent]
        public Camera.GateFitMode m_gateFit;

        private static CinemachineBrain brain;
        private static Camera m_camera;

        private Camera camera
        {
            get
            {
                if (m_camera == null)
                {
                    if (brain == null)
                    {
                        brain = FindObjectOfType<CinemachineBrain>();
                    }
                    m_camera = brain?.OutputCamera;
                }
                return m_camera;
            }
        }
        

        protected override void ConnectToVcam(bool connect)
        {
            base.ConnectToVcam(connect);
            if (connect)
            {
                if (camera.usePhysicalProperties != m_isPhysicalCamera)
                {
                    camera.usePhysicalProperties = m_isPhysicalCamera;
                    if (m_isPhysicalCamera)
                    {
                        camera.focalLength = m_focalLength;
                        camera.sensorSize = m_sensorSize;
                        camera.lensShift = m_lensShift;
                        camera.gateFit = m_gateFit;
                    }
                }
            }
        }

        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {

        }
    }
}