using UnityEngine;

namespace DChild.Configurations
{
    [System.Serializable]
    public class AudioSettings
    {
        [SerializeField]
        private float m_masterVolume = 1;
        [SerializeField]
        private float m_soundVolume = 1;
        [SerializeField]
        private float m_musicVolume = 1;

        public float masterVolume
        {
            get => m_masterVolume;
            set => m_masterVolume = value;
        }

        public float soundVolume
        {
            get => m_soundVolume;
            set => m_soundVolume = value;
        }

        public float musicVolume
        {
            get => m_musicVolume;
            set => m_musicVolume = value;
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