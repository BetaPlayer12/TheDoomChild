using DChild.Configurations;
using DChild.UI;
using UnityEngine;

namespace DChild.Menu.UI
{
    public class BrightnessSlider : ReferenceSlider, IValueUI, IReferenceUI<VisualSettings>
    {
        [SerializeField]
        private IndexSliderInterpreter m_interpreter;
        private VisualSettings m_settings;

        protected override float value
        {
            get
            {
                return m_interpreter.InterpretOutput(m_settings.brightness);
            }

            set
            {
                m_settings.brightness = m_interpreter.GetOutput((int)value);
            }
        }

        public void SetReference(VisualSettings reference)
        {
            m_settings = reference;
        }

        public void UpdateUI()
        {
            m_slider.value = m_interpreter.InterpretOutput(m_settings.brightness);
        }
    }
}