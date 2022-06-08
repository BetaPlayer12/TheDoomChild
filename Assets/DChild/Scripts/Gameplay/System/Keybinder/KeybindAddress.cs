using UnityEngine;

namespace DChild.CustomInput.Keybind
{
    [System.Serializable]
    public class KeybindAddress
    {
        [SerializeField]
        private string m_actionMap;
        [SerializeField]
        private int m_index;

        public string actionMap => m_actionMap;
        public int index => m_index;
    }

}