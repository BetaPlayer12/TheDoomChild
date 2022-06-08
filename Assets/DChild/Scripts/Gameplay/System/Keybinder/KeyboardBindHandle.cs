using UnityEngine;

namespace DChild.CustomInput.Keybind
{
    public class KeyboardBindHandle : MonoBehaviour
    {
        [SerializeField]
        private KeybindSelection m_selection;

        private KeybindAddressesList m_addressList;

        public void SetAddressList(KeybindAddressesList keybindAddressesList) => m_addressList = keybindAddressesList;
    }
}
