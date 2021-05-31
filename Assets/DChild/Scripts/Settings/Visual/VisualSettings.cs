using DChild.Configurations.Visuals;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Configurations
{
    [System.Serializable]
    public class VisualSettings
    {
        public struct Info
        {
            public int resolutionIndex { get; }
            public bool fullscreen { get; }
            public float brightness { get; }
            public float contrast { get; }
            public bool vsync { get; }
        }

        [BoxGroup("Configurators")]
        [SerializeField]
        private ScreenResolution m_screenResolution;
        [BoxGroup("Configurators")]
        [SerializeField]
        private ScreenLighting m_screenLighting;

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
                GameSystem.settings.configuration.visualConfiguration.resolutionIndex = value;
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
                GameSystem.settings.configuration.visualConfiguration.fullscreen = value;
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
                GameSystem.settings.configuration.visualConfiguration.brightness = value;
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
                GameSystem.settings.configuration.visualConfiguration.contrast = value;
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
                GameSystem.settings.configuration.visualConfiguration.vsync = value;
            }
        }
    }
}