using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace DChild.CustomInput.Keybind
{
    public class KeyboardBindButton : MonoBehaviour
    {
        [SerializeField]
        private KeyboardBindUI m_ui;
        [SerializeField]
        private KeybindSelection m_selection;
        [SerializeField, ReadOnly]
        private string m_currentPath;

        private KeybindAddressesList m_addressList;

        public KeybindSelection selection => m_selection;

        public void SetAddressList(KeybindAddressesList keybindAddressesList)
        {
            m_addressList = keybindAddressesList;
            UpdateUI();
        }

        public void UpdateUI()
        {
            var address = m_addressList.GetAddress(0);
            var binding = address.actionMap.action.bindings[address.index];
            m_ui.UpdateVisual(binding);
            m_currentPath = binding.effectivePath;
        }

        [Button]
        public void RebindKey()
        {
            var address = m_addressList.GetAddress(0);
            var action = address.actionMap.action;
            action.Disable();
            var rebind = action.PerformInteractiveRebinding(address.index);
            rebind.OnComplete((operation) =>
            {
                if (m_addressList.count > 1)
                {
                    var overridePath = action.bindings[address.index].overridePath;
                    for (int i = 1; i < m_addressList.count; i++)
                    {
                        address = m_addressList.GetAddress(i);
                        RebindKey(address.actionMap.action, address.index, overridePath);

                    }
                }

                UpdateUI();
                action.Enable();
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
