using DChild.Configurations;
using DChild.UI;

namespace DChild.Menu.UI
{
    public class FullScreenField : ToggleField, IReferenceUI<VisualSettingsHandle>
    {
        private VisualSettingsHandle m_settings;


        protected override bool value
        {
            get
            {
                return m_settings.fullscreen;
            }

            set
            {
                m_settings.fullscreen = value;
            }
        }

        public void SetReference(VisualSettingsHandle reference)
        {
            m_settings = reference;
        }
    }
}