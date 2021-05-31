using DChild.Configurations;
using DChild.UI;

namespace DChild.Menu.UI
{
    public class SoundVolumeSlider : ReferenceSlider, IValueUI, IReferenceUI<AudioSettings>
    {
        private AudioSettings m_settings;

        protected override float value
        {
            get
            {
                return m_settings.soundVolume;
            }

            set
            {
                m_settings.soundVolume = value;
            }
        }

        public void SetReference(AudioSettings reference)
        {
            m_settings = reference;
        }

        public void UpdateUI()
        {
            m_slider.value = m_settings.soundVolume;
        }
    }
}
