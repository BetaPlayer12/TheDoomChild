using UnityEngine;
using UnityEngine.InputSystem;

namespace DChild.CustomInput.Keybind
{
    [System.Serializable]
    public class KeybindAddress
    {
        [SerializeField]
        private InputActionReference m_actionMap;
        [SerializeField]
        private int m_keyboardIndex;
        [SerializeField]
        private int m_gamepadIndex;

        public InputActionReference actionMap => m_actionMap;
        public int keyboardIndex => m_keyboardIndex;
        public int gamepadIndex => m_gamepadIndex;
    }

}