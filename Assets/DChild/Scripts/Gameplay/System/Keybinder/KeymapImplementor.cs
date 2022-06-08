using DChild.CustomInput.Keybind;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DChild.CustomInput.Keybind
{
    public class KeymapImplementor : MonoBehaviour
    {
        [SerializeField]
        private KeyboardKeybindSelectionAddresses m_keyboardSelectionAddresses;
        [SerializeField]
        private InputActionAsset m_inputAction;

        public void ResetOverrides()
        {
            m_inputAction.AddActionMap("Gameplay").RemoveAllBindingOverrides();
        }

        public void LoadKeyboardKeymap(KeyboardKeymapData keyboardKeymapData)
        {
            for (int i = 0; i < keyboardKeymapData.count; i++)
            {
                LoadKeyboardBinding(i,keyboardKeymapData.GetKeybindInfo(i));
            }
        }

        public void LoadKeyboardBinding(int selectionIndex,KeybindInfo keybindInfo)
        {
            var addresses = m_keyboardSelectionAddresses.GetAddressList(selectionIndex);
            for (int i = 0; i < addresses.count; i++)
            {
                var address = addresses.GetAddress(i);
                var action = m_inputAction.FindAction(address.actionMap.action.name);
                action.Disable();
                if (keybindInfo.isOverriden)
                {
                    action.ApplyBindingOverride(address.index, keybindInfo.keybind);
                }
                else
                {
                    action.RemoveBindingOverride(address.index);
                }
                action.Enable();
            }
        }
    }
}
