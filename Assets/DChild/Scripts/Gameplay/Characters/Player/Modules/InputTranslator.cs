using UnityEngine;
using UnityEngine.InputSystem;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class InputTranslator : MonoBehaviour
    {
        public float horizontalInput;
        public float verticalInput;
        public bool crouchHeld;
        public bool dashPressed;
        public bool jumpPressed;
        public bool jumpHeld;

        public bool slashPressed;
        public bool slashHeld;

        private void OnHorizontalInput(InputValue value)
        {
            horizontalInput = value.Get<float>();
        }
        private void OnVerticalInput(InputValue value)
        {
            verticalInput = value.Get<float>();
        }

        private void OnCrouch(InputValue value)
        {
            crouchHeld = value.Get<float>() == 1;
        }

        private void OnDash(InputValue value)
        {
            dashPressed = value.Get<float>() == 1;
        }

        private void OnJump(InputValue value)
        {
            var isTrue = value.Get<float>() == 1;
            jumpPressed = isTrue;
            jumpHeld = isTrue;
        }

        private void OnSlash(InputValue value)
        {
            var isTrue = value.Get<float>() == 1;
            slashPressed = isTrue;
            slashHeld = isTrue;
        }

        private void LateUpdate()
        {
            dashPressed = false;
            jumpPressed = false;
            slashPressed = false;
        }
    }
}
