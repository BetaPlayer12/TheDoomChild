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

        public bool earthShakerPressed;

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
            slashPressed = value.Get<float>() == 1;
        }

        private void OnSlashHeld(InputValue value)
        {
            slashHeld = value.Get<float>() == 1;
        }

        private void OnEarthShaker(InputValue value)
        {
            earthShakerPressed = value.Get<float>() == 1;
        }

        private void LateUpdate()
        {
            dashPressed = false;
            jumpPressed = false;
            slashPressed = false;

            earthShakerPressed = false;
        }
    }
}
