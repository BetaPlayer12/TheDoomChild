using DChild.Configurations;
using DChild.UI;

namespace DChild.Menu.UI
{
    public class MasterVolumeSlider : ReferenceSlider, IValueUI, IReferenceUI<AudioSettings>
    {
        private AudioSettings m_settings;

        protected override float value
        {
            get
            {
                return m_settings.masterVolume;
            }

            set
            {
                m_settings.masterVolume = value;
            }
        }

        public void SetReference(AudioSettings reference)
        {
            m_settings = reference;
        }

        public void UpdateUI()
        {
            m_slider.value = m_settings.masterVolume;
        }
    }
}
