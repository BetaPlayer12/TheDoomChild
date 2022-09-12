using DChild.Configurations;
using DChild.UI;
using UnityEngine;

namespace DChild.Menu.UI
{
    public class MusicSlider : ReferenceSlider, IValueUI, IReferenceUI<AudioSettingsHandle>
    {
        [SerializeField]
        private IndexSliderInterpreter m_interpreter;
        private AudioSettingsHandle m_settings;

        protected override float value
        {
            get
            {
                return m_interpreter.InterpretOutput(m_settings.musicVolume);
            }

            set
            {
                m_settings.musicVolume = m_interpreter.GetOutput((int)value);
            }
        }

        public void SetReference(AudioSettingsHandle reference)
        {
            m_settings = reference;
        }

        public void UpdateUI()
        {
            m_slider.value = value;
        }
    }
}