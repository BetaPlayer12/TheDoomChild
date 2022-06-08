using UnityEngine;

namespace DChild.CustomInput.Keybind
{
    [System.Serializable]
    public class KeybindManager
    {
        [SerializeField]
        private KeyboardKeybindSelectionAddresses m_keyboardSelectionAddresses;


        public KeyboardKeybindSelectionAddresses keyboardSelectionAddresses => m_keyboardSelectionAddresses;
    }
}