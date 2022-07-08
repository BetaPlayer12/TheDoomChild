using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DChild.CustomInput.Keybind
{
    [System.Serializable]
    public class KeybindManager
    {
        [SerializeField]
        private InputActionAsset m_inputAction;
        [SerializeField]
        private KeyboardKeybindSelectionAddresses m_keyboardSelectionAddresses;

        private KeyboardKeymapData m_keyboardKeymapData;

        public void ResetOverrides()
        {
            m_inputAction.AddActionMap("Gameplay").RemoveAllBindingOverrides();
        }

        public InputBinding GetCurrentKeyboardInputBindind(KeybindSelection keybindSelection)
        {
            var address = GetKeyboardAddressList(keybindSelection).GetAddress(0);
            return address.actionMap.action.bindings[address.index];
        }

        public void RebindKeyboard(KeybindSelection keybindSelection, Action<InputBinding> callback = null)
        {
            RebindKey(keybindSelection, GetKeyboardAddressList(keybindSelection), callback);
        }


        public void Initialize(KeyboardKeymapData keyboardKeymapData)
        {
            if (keyboardKeymapData == null)
                return;

            m_keyboardKeymapData = keyboardKeymapData;
            for (int i = 0; i < keyboardKeymapData.count; i++)
            {
                LoadKeyboardBinding(i, keyboardKeymapData.GetKeybindInfo(i));
            }
        }

        public void LoadKeyboardBinding(int selectionIndex, KeybindInfo keybindInfo)
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


        public KeybindAddressesList GetKeyboardAddressList(KeybindSelection selection) => m_keyboardSelectionAddresses.GetAddressList((int)selection);

        private void RebindKey(KeybindSelection selection, KeybindAddressesList keybindAddressesList, Action<InputBinding> callback)
        {
            var address = keybindAddressesList.GetAddress(0);
            var action = address.actionMap.action;
            action.Disable();
            var rebind = action.PerformInteractiveRebinding(address.index);
            rebind.OnComplete((operation) =>
            {
                var binding = action.bindings[address.index];
                var overridePath = binding.overridePath;
                if (keybindAddressesList.count > 1)
                {
                    for (int i = 1; i < keybindAddressesList.count; i++)
                    {
                        address = keybindAddressesList.GetAddress(i);
                        RebindKey(address.actionMap.action, address.index, overridePath);
                    }
                }

                callback?.Invoke(binding);
                action.Enable();
                m_keyboardKeymapData.GetKeybindInfo((int)selection).SetKeybind(overridePath);
                operation.Dispose();
            }
            );

            rebind.OnCancel((operation) =>
            {
                action.Enable();
                operation.Dispose();
            }
            );

            rebind.Start();
        }

        private void RebindKey(InputAction action, int index, string overridePath)
        {
            action.Disable();
            action.ApplyBindingOverride(index, overridePath);
            action.Enable();
        }
    }
}