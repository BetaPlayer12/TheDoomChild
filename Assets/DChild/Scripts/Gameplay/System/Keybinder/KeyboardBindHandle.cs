using UnityEngine;

namespace DChild.CustomInput.Keybind
{
    public class KeyboardBindHandle : MonoBehaviour
    {
        private KeyboardBindButton[] m_buttons;

        public void PerformRebind(KeyboardBindButton keyboardBindButton)
        {
            GameSystem.settings.keybind.RebindKeyboard(keyboardBindButton.selection, keyboardBindButton.UpdateUI);
        }

        private void Start()
        {
            m_buttons = GetComponentsInChildren<KeyboardBindButton>(true);
            var keybind = GameSystem.settings.keybind;
            for (int i = 0; i < m_buttons.Length; i++)
            {
                var button = m_buttons[i];
                button.UpdateUI(keybind.GetCurrentKeyboardInputBindind(button.selection));
            }
        }
    }
}
