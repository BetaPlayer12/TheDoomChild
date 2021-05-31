using DChild.Configurations.Visuals;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Configurations
{
    [System.Serializable]
    public class VisualSettingsHandle
    {
        [BoxGroup("Configurators")]
        [SerializeField]
        private ScreenResolution m_screenResolution;
        [BoxGroup("Configurators")]
        [SerializeField]
        private ScreenLighting m_screenLighting;

        private GameSettingsConfiguration m_configuration;

        public void Initialize(GameSettingsConfiguration configuration)
        {
            m_configuration = configuration;
            var visualConfiguration = m_configuration.visualConfiguration;
            m_screenResolution.SetResolution(visualConfiguration.resolutionIndex);
            m_screenResolution.SetFullscreen(visualConfiguration.fullscreen);
            m_screenLighting.brightness = visualConfiguration.brightness;
            m_screenLighting.contrast = visualConfiguration.contrast;
            QualitySettings.vSyncCount = visualConfiguration.vsync ? 1 : 0;
        }

#if UNITY_EDITOR
        public void Initialize(ScreenResolution screenResolution, ScreenLighting screenLighting)
        {
            m_screenResolution = screenResolution;
            m_screenLighting = screenLighting;
        }
#endif

        public int resolution
        {
            get
            {
               return m_screenResolution.resolutionIndex;
            }

            set
            {
                m_screenResolution.SetResolution(value);
                m_screenResolution.Apply();
                m_configuration.visualConfiguration.resolutionIndex = value;
            }
        }

        public bool fullscreen
        {
            get
            {
                return m_screenResolution.fullScreen;
            }

            set
            {
                m_screenResolution.SetFullscreen(value);
                m_screenResolution.Apply();
                m_configuration.visualConfiguration.fullscreen = value;
            }
        }

        public float brightness
        {
            get
            {
                return m_screenLighting.brightness;
            }

            set
            {
                m_screenLighting.brightness = value;
                m_configuration.visualConfiguration.brightness = value;
            }
        }

        public float contrast
        {
            get
            {
                return m_screenLighting.contrast;
            }

            set
            {
                m_screenLighting.contrast = value;
                m_configuration.visualConfiguration.contrast = value;
            }
        }

        public bool vsync
        {
            get
            {
               return QualitySettings.vSyncCount == 1;
            }
            set
            {
                QualitySettings.vSyncCount = value ? 1 : 0;
                m_configuration.visualConfiguration.vsync = value;
            }
        }
    }
}