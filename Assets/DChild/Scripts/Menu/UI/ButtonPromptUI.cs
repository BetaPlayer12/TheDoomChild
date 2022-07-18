using DChild.CustomInput.Keybind;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DChild.Menu.Inputs
{
    public class ButtonPromptUI : InputPromptUI
    {
        [SerializeField]
        private TextMeshProUGUI m_label;
        [SerializeField]
        private TextMeshProUGUI m_empahsisLabel;
        [SerializeField]
        private KeybindAddressesList m_addressesList;

        private InputBinding GetBinding(bool useGamepadIndex)
        {
            var address = m_addressesList.GetAddress(0);
            var index = useGamepadIndex ? address.gamepadIndex : address.keyboardIndex;
            return address.actionMap.action.bindings[index];
        }

        protected override void UpdateGamepadInputIcons(GamepadIconData iconData)
        {
            var binding = GetBinding(true);
            if (iconData != null)
            {
                var text = iconData.GetSprite(binding.effectivePath);
                m_label.spriteAsset = iconData.spriteAsset;
                m_label.text = text;
                if (m_empahsisLabel)
                {
                    m_empahsisLabel.spriteAsset = iconData.spriteAsset;
                    m_empahsisLabel.text = text;
                }
            }
        }

        protected override void UpdateKeyboardInputIcons(GamepadIconData iconData)
        {
            var binding = GetBinding(false);
            if (iconData == null)
            {
                var text = binding.effectivePath.Replace("<Keyboard>/", "");
                m_label.text = text;
                if (m_empahsisLabel)
                {
                    m_empahsisLabel.text = text;
                }
            }
        }
    }
}