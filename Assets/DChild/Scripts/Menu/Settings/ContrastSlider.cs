using System;
using DChild.Configurations;
using DChild.UI;
using UnityEngine;

namespace DChild.Menu.UI
{
    public class ContrastSlider : ReferenceSlider, IValueUI, IReferenceUI<VisualSettingsHandle>
    {
        [SerializeField]
        private IndexSliderInterpreter m_interpreter;
        private VisualSettingsHandle m_settings;

        protected override float value
        {
            get
            {
                return m_interpreter.InterpretOutput(m_settings.contrast);
            }

            set
            {
                m_settings.contrast = m_interpreter.GetOutput((int)value);
            }
        }

        public void SetReference(VisualSettingsHandle reference)
        {
            m_settings = reference;
        }

        public void UpdateUI()
        {
            m_slider.value = m_interpreter.InterpretOutput(m_settings.contrast);
        }
    }
}