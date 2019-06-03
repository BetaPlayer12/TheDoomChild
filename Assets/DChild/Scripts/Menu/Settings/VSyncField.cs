using DChild.Configurations;
using DChild.UI;

namespace DChild.Menu.UI
{
    public class VSyncField : ToggleField, IReferenceUI<VisualSettings>
    {
        private VisualSettings m_settings;

        protected override bool value
        {
            get
            {
                return m_settings.vsync;
            }

            set
            {
                m_settings.vsync = value;
            }
        }

        public void SetReference(VisualSettings reference)
        {
            m_settings = reference;
        }
    }
}
