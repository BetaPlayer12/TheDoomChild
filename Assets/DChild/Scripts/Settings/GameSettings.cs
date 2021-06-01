using DChild.Configurations.Visuals;
using DChild.Serialization;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DChild.Configurations
{
    public class GameSettings : MonoBehaviour
    {
        [SerializeField]
        private GameSettingsConfiguration m_configuration;

        [SerializeField, Title("Visual Settings"), HideLabel]
        private VisualSettingsHandle m_visual;
        [SerializeField]
        private SupportedResolutions m_supportedResolutions;
        [SerializeField, Title("Audio Settings"), HideLabel]
        private AudioSettingsHandle m_audio;
        [SerializeField, Title("Gameplay Settings"), HideLabel]
        private GameplaySettings m_gameplay;

        public GameSettingsConfiguration configuration => m_configuration;

        public VisualSettingsHandle visual => m_visual;
        public SupportedResolutions supportedResolutions => m_supportedResolutions;
        public new AudioSettingsHandle audio => m_audio;
        public GameplaySettings gameplay => m_gameplay;

        public void SaveSettings()
        {
            SerializationHandle.SaveConfiguration(m_configuration);
        }

        public void LoadDefaultSettings()
        {
            
        }

        public void Initialize()
        {
            SerializationHandle.LoadConfiguration(ref m_configuration);
            if(m_configuration == null)
            {
                m_configuration = new GameSettingsConfiguration();
                LoadDefaultSettings();
            }

            m_visual.Initialize(m_configuration);
            m_audio.Initialize(m_configuration);
        }

#if UNITY_EDITOR
        public void Initialize(SupportedResolutions supportedResolutions)
        {
            m_supportedResolutions = supportedResolutions;
        }
#endif
    }

}