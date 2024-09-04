using DChild.Configurations;
using DChild.UI;

namespace DChild.Menu.UI
{

    public class BloomField : ToggleField, IReferenceUI<VisualSettingsHandle>
    {

        private VisualSettingsHandle m_settings;

        protected override bool value
        {
            get
            {
                return m_settings.bloom;
            }

            set => m_settings.bloom = value;
        }

        public void SetReference(VisualSettingsHandle reference)
        {
            m_settings = reference;
        }


    }
}
