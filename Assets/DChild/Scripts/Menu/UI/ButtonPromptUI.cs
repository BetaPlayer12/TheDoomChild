using DChild.CustomInput.Keybind;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DChild.Menu.Inputs
{
    public class ButtonPromptUI : MonoBehaviour
    {
        [SerializeField]
        private string m_text;
        [SerializeField]
        private KeybindAddressesList[] m_addressesLists;

        private TextMeshProUGUI m_label;
        private const string TOKEN  = "$#";

        protected void UpdateInputIcons(GamepadIconData iconData, int inputIndex)
        {
            if (iconData)
            {
                m_label.spriteAsset = iconData.spriteAsset;
            }
            else
            {
            
            }
        }

        private void UpdateInputIcons(object sender, InputIconChangeEventArgs eventArgs)
        {
            UpdateInputIcons(eventArgs.iconData, InputIconHandle.inputIndex);
        }
        
        private void Awake()
        {
            InputIconHandle.UpdateInputIcons += UpdateInputIcons;
            UpdateInputIcons(InputIconHandle.GetCurrentInputIcon(), InputIconHandle.inputIndex);
        }


        private void OnDestroy()
        {
            InputIconHandle.UpdateInputIcons -= UpdateInputIcons;
        }
    }
}