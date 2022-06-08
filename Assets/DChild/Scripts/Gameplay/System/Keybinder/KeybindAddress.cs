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
        private int m_index;

        public InputActionReference actionMap => m_actionMap;
        public int index => m_index;
    }

}