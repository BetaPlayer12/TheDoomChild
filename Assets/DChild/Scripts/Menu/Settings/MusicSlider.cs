using DChild.Configurations;
using DChild.UI;

namespace DChild.Menu.UI
{
    public class MusicSlider : ReferenceSlider, IValueUI, IReferenceUI<AudioSettingsHandle>
    {
        private AudioSettingsHandle m_settings;

        protected override float value
        {
            get
            {
                return m_settings.musicVolume;
            }

            set
            {
                m_settings.musicVolume = value;
            }
        }

        public void SetReference(AudioSettingsHandle reference)
        {
            m_settings = reference;
        }

        public void UpdateUI()
        {
            m_slider.value = m_settings.musicVolume;
        }
    }
}