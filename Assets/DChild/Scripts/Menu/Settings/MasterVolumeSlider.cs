using DChild.Configurations;
using DChild.UI;

namespace DChild.Menu.UI
{
    public class MasterVolumeSlider : ReferenceSlider, IValueUI, IReferenceUI<AudioSettingsHandle>
    {
        private AudioSettingsHandle m_settings;

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

        public void SetReference(AudioSettingsHandle reference)
        {
            m_settings = reference;
        }

        public void UpdateUI()
        {
            m_slider.value = m_settings.masterVolume;
        }
    }
}
