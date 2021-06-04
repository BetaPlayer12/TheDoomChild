﻿using DChild.Configurations.Visuals;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Configurations
{
    [System.Serializable]
    public class VisualSettingsHandle
    {
        [SerializeField]
        private SupportedResolutions m_supportedResolutions;
        [BoxGroup("Configurators")]
        [SerializeField]
        private ScreenResolutionHandle m_screenResolution;
        [BoxGroup("Configurators")]
        [SerializeField]
        private ScreenLighting m_screenLighting;

        private GameSettingsConfiguration m_configuration;

        public SupportedResolutions supportedResolutions => m_supportedResolutions;


#if UNITY_EDITOR
        public void Initialize(ScreenResolutionHandle screenResolution, ScreenLighting screenLighting)
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
                m_configuration.visualConfiguration.resolutionIndex = value;
                m_screenResolution.SetResolution(value);
                m_screenResolution.Apply();
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
                Debug.LogError($"Set Fullscreen: {value}");
                m_configuration.visualConfiguration.fullscreen = value;
                m_screenResolution.SetFullscreen(value);
                m_screenResolution.Apply();
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

        public void Initialize(GameSettingsConfiguration configuration)
        {
            m_configuration = configuration;
            var visualConfiguration = m_configuration.visualConfiguration;
            m_screenResolution.SetSupportedResolutions(m_supportedResolutions);
            m_screenResolution.SetResolution(visualConfiguration.resolutionIndex);
            m_screenResolution.SetFullscreen(visualConfiguration.fullscreen);
            m_screenResolution.Apply();


            m_screenLighting.brightness = visualConfiguration.brightness;
            m_screenLighting.contrast = visualConfiguration.contrast;
            QualitySettings.vSyncCount = visualConfiguration.vsync ? 1 : 0;
        }

        public IEnumerator ForceAssignFullScreenRoutine(bool isFullscreen)
        {
            var endOfFrame = new WaitForEndOfFrame();
            yield return endOfFrame;

            m_screenResolution.SetFullscreen(!isFullscreen);
            m_screenResolution.Apply();

            yield return endOfFrame;

            m_screenResolution.SetFullscreen(isFullscreen);
            m_screenResolution.Apply();
            yield return endOfFrame;
        }
    }
}