using UnityEngine;

namespace DChild.CustomInput.Keybind
{
    public class KeyboardBindHandle : MonoBehaviour
    {
        private KeyboardBindButton[] m_buttons;

        private void Start()
        {
            m_buttons = GetComponentsInChildren<KeyboardBindButton>(true);
            var keybind = GameSystem.settings.keybind;
            for (int i = 0; i < m_buttons.Length; i++)
            {
                var button = m_buttons[i];
                button.SetAddressList(keybind.GetKeyboardAddressList(button.selection));
            }
        }
    }
}
