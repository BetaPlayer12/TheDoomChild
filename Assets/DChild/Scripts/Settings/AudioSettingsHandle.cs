using UnityEngine;

namespace DChild.Configurations
{
    [System.Serializable]
    public class AudioSettingsHandle
    {
        [SerializeField]
        private float m_masterVolume = 1;
        [SerializeField]
        private float m_soundVolume = 1;
        [SerializeField]
        private float m_musicVolume = 1;

        private GameSettingsConfiguration m_configuration;

        public void Initialize(GameSettingsConfiguration configuration)
        {
            m_configuration = configuration;
            var audioConfiguration = m_configuration.audioConfiguration;
            m_masterVolume = audioConfiguration.masterVolume;
            m_soundVolume = audioConfiguration.soundVolume;
            m_musicVolume = audioConfiguration.musicVolume;
        }

        public float masterVolume
        {
            get => m_masterVolume;
            set
            {
                m_masterVolume = value;
                m_configuration.audioConfiguration.masterVolume = value;
            }
        }

        public float soundVolume
        {
            get => m_soundVolume;
            set
            {
                m_soundVolume = value;
                m_configuration.audioConfiguration.soundVolume = value;
            }
        }

        public float musicVolume
        {
            get => m_musicVolume;
            set
            {
                m_musicVolume = value;
                m_configuration.audioConfiguration.musicVolume = value;
            }
        }


        public float sfxVolume
        {
            get
            {
                return 0;
            }

            set
            {

            }
        }
    }

}