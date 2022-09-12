using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DChild.CustomInput.Keybind
{
    [System.Serializable]
    public class KeybindInfo
    {
        [SerializeField, ReadOnly]
        private bool m_isOverriden;
        [SerializeField]
        private string m_keybind;

        public string keybind => m_keybind;
        public bool isOverriden => m_isOverriden;

        public void SetKeybind(string keybind)
        {
            m_keybind = keybind;
            m_isOverriden = true;
        }

        public void ResetBind()
        {
            m_keybind = "";
            m_isOverriden = false;
        }
    }
}