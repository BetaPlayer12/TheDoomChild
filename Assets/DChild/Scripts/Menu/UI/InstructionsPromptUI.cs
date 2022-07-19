using DChild.CustomInput.Keybind;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace DChild.Menu.Inputs
{
    public class InstructionsPromptUI : InputPromptUI
    {
        [SerializeField]
        private TextMeshPro m_label;
        [SerializeField]
        private string m_text;
        [SerializeField]
        private KeybindAddressesList[] m_addressesLists;

        protected const string TOKEN = "$#";

        private InputBinding GetBinding(KeybindAddressesList addressesLists, bool useGamepad)
        {
            var address = addressesLists.GetAddress(0);
            var index = useGamepad ? address.gamepadIndex : address.keyboardIndex;
            return address.actionMap.action.bindings[index];
        }

        protected override void UpdateGamepadInputIcons(GamepadIconData iconData)
        {
            m_label.spriteAsset = iconData.spriteAsset;
            var currentText = m_text;
            for (int i = 0; i < m_addressesLists.Length; i++)
            {
                var token = TOKEN + i;
                var textMesh = GetBinding(m_addressesLists[i], true).effectivePath;
                var text = iconData.GetSprite(textMesh);
                currentText = currentText.Replace(token, text);

            }
            m_label.text = currentText;
        }

        protected override void UpdateKeyboardInputIcons(GamepadIconData iconData)
        {
            m_label.spriteAsset = iconData.spriteAsset;
            var currentText = m_text;
            for (int i = 0; i < m_addressesLists.Length; i++)
            {
                var token = TOKEN + i;
                var textMesh = GetBinding(m_addressesLists[i], false).effectivePath;
                currentText = currentText.Replace(token, textMesh);

            }
            m_label.text = currentText.Replace("<Keyboard>", "");
        }
    }
}