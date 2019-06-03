using DChild.Configurations;
using DChild.UI;

namespace DChild.Menu.UI
{
    public class DamageValuesField : ToggleField, IReferenceUI<GameplaySettings>
    {
        private GameplaySettings m_settings;

        protected override bool value
        {
            get
            {
                return m_settings.showDamageValues;
            }

            set
            {
                m_settings.showDamageValues = value;
            }
        }

        public void SetReference(GameplaySettings reference)
        {
            m_settings = reference;
        }
    }
}