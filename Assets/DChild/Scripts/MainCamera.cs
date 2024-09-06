using DChild.Configurations;
using DChild.Gameplay;
using Holysoft.Event;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace DChild
{
    [AddComponentMenu("DChild/Misc/Main Camera")]
    public class MainCamera : MonoBehaviour
    {
        [SerializeField]
        private Camera m_camera;
        private UniversalAdditionalCameraData m_cameraData;

        private void Awake()
        {
            m_camera = GetComponent<Camera>();
            GameSystem.SetCamera(m_camera);
            GameplaySystem.cinema?.SetMainCamera(m_camera);
            m_cameraData = m_camera.GetUniversalAdditionalCameraData();
        }

        private void OnEnable() => GameSystem.settings.visual.SceneVisualsChange += OnAntiAliasingModeChange;

        private void OnDisable() => GameSystem.settings.visual.SceneVisualsChange -= OnAntiAliasingModeChange;

        private void OnAntiAliasingModeChange(object sender, EventActionArgs eventArgs) => OnAntiAliasingValueChange();
        private void OnAntiAliasingValueChange() => m_cameraData.antialiasing = (AntialiasingMode)GameSystem.settings.visual.antiAliasing;


        private void OnDestroy()
        {
            var camera = GetComponent<Camera>();
            if (GameSystem.mainCamera == camera)
            {
                GameSystem.SetCamera(null);
            }
            var cinema = GameplaySystem.cinema;
            if (cinema != null && cinema.mainCamera == camera)
            {
                cinema.SetMainCamera(null);
            }
        }
    }
}