using DChild.Configurations;
using DChild.UI;

namespace DChild.Menu.UI
{
    public class FullScreenField : ToggleField, IReferenceUI<VisualSettings>
    {
        private VisualSettings m_settings;


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

        public void SetReference(VisualSettings reference)
        {
            m_settings = reference;
        }
    }
}