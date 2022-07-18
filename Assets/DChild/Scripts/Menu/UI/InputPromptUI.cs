using UnityEngine;

namespace DChild.Menu.Inputs
{
    public abstract class InputPromptUI : MonoBehaviour
    {
        protected abstract void UpdateGamepadInputIcons(GamepadIconData iconData);
        protected abstract void UpdateKeyboardInputIcons(GamepadIconData iconData);

        private void UpdateInputIcons(object sender, InputIconChangeEventArgs eventArgs)
        {
            UpdateInputIcons(eventArgs.iconData);
        }

        private void UpdateInputIcons(GamepadIconData iconData)
        {
            if (InputIconHandle.useGamepad)
            {
                UpdateGamepadInputIcons(iconData);

            }
            else
            {
                UpdateKeyboardInputIcons(iconData);
            }
        }

        private void Awake()
        {
            InputIconHandle.UpdateInputIcons += UpdateInputIcons;
            UpdateInputIcons(InputIconHandle.GetCurrentInputIcon());
        }


        private void OnDestroy()
        {
            InputIconHandle.UpdateInputIcons -= UpdateInputIcons;
        }
    }
}