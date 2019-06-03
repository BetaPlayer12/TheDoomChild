using DChild.Configurations;
using DChild.UI;
using Holysoft.UI;

namespace DChild.Menu.Settings
{
    public class GameplayScreen : UICanvas
    {
        private IValueUI[] m_fields;

        public override void Hide()
        {
            Disable();
        }

        public override void Show()
        {
            Enable();
            for (int i = 0; i < (m_fields?.Length ?? 0); i++)
            {
                m_fields[i].UpdateUI();
            }
        }

        private void Awake()
        {
            m_fields = GetComponentsInChildren<IValueUI>();
            var gameplaySettings = GameSystem.settings.gameplay;
            var gameplaySeRefrences = GetComponentsInChildren<IReferenceUI<GameplaySettings>>();
            for (int i = 0; i < gameplaySeRefrences.Length; i++)
            {
                gameplaySeRefrences[i].SetReference(gameplaySettings);
            }
        }
    }

}