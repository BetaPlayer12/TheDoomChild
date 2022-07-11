using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace DChild.Menu.Inputs
{
    public class ButtonPromptUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_label;

        public void ChangePromptTo(KeyCode keyCode)
        {
            if (keyCode.ToString().Contains("Joystick"))
            {

            }
            else
            {
                ChangeToKeyboardPrompt(keyCode);
            }
        }


        protected virtual void ChangeToKeyboardPrompt(KeyCode keyCode)
        {
            m_label.text = keyCode.ToString();
        }
    }

}